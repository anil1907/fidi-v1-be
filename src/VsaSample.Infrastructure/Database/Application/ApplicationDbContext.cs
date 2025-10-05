using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace VsaSample.Infrastructure.Database.Application;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    private static readonly JsonSerializerOptions GoalsSerializerOptions = new(JsonSerializerDefaults.Web);
    private static readonly JsonSerializerOptions TemplateSerializerOptions = new(JsonSerializerDefaults.Web);

    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductTranslation> ProductTranslation { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);

        ConfigureUsers(modelBuilder);
        ConfigureAppointments(modelBuilder);
        ConfigureClients(modelBuilder);
        ConfigureTemplates(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(32);

            builder.Property(u => u.PasswordHash)
                .HasColumnName("Password")
                .IsRequired()
                .HasMaxLength(256);
        });
    }

    private static void ConfigureAppointments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(builder =>
        {
            builder.Property(a => a.Title).HasMaxLength(256);
            builder.Property(a => a.Description).HasMaxLength(1024);

            builder.Property(a => a.Status)
                .HasConversion<string>()
                .HasMaxLength(32);

            builder.Property(a => a.StartsAt)
                .HasColumnType("timestamp with time zone");

            builder.Property(a => a.EndsAt)
                .HasColumnType("timestamp with time zone");

            builder.HasIndex(a => a.ClientId);
            builder.HasIndex(a => a.StartsAt);
            builder.HasIndex(a => a.EndsAt);

            builder.HasOne(a => a.Client)
                .WithMany()
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureClients(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(builder =>
        {
            builder.Property(c => c.FirstName).HasMaxLength(128);
            builder.Property(c => c.LastName).HasMaxLength(128);
            builder.Property(c => c.Email).HasMaxLength(256);
            builder.Property(c => c.Phone).HasMaxLength(32);

            var goalsProperty = builder.Property(c => c.Goals)
                .HasColumnType("jsonb")
                .HasConversion(
                    goals => JsonSerializer.Serialize(goals, GoalsSerializerOptions),
                    json => string.IsNullOrWhiteSpace(json)
                        ? new List<string>()
                        : JsonSerializer.Deserialize<List<string>>(json, GoalsSerializerOptions) ?? new List<string>());

            goalsProperty.Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (left, right) =>
                    (left ?? new List<string>()).SequenceEqual(right ?? new List<string>(), StringComparer.OrdinalIgnoreCase),
                goals => (goals ?? new List<string>()).Aggregate(0, (hash, goal) => HashCode.Combine(hash, StringComparer.OrdinalIgnoreCase.GetHashCode(goal))),
                goals => goals == null ? new List<string>() : goals.ToList()));

            builder.HasIndex(c => c.Email).IsUnique();
        });
    }

    private static void ConfigureTemplates(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Template>(builder =>
        {
            builder.Property(t => t.Name).HasMaxLength(256);
            builder.Property(t => t.Description).HasMaxLength(1024);

            var sectionsProperty = builder.Property(t => t.Sections)
                .HasColumnType("jsonb")
                .HasConversion(
                    sections => JsonSerializer.Serialize(sections, TemplateSerializerOptions),
                    json => string.IsNullOrWhiteSpace(json)
                        ? new List<TemplateSection>()
                        : JsonSerializer.Deserialize<List<TemplateSection>>(json, TemplateSerializerOptions) ?? new List<TemplateSection>());

            sectionsProperty.Metadata.SetValueComparer(new ValueComparer<List<TemplateSection>>(
                (left, right) => TemplateSectionsEqual(left, right),
                sections => TemplateSectionsHash(sections),
                sections => TemplateSectionsClone(sections))); 

            builder.HasIndex(t => t.Name).IsUnique();
        });
    }

    private static bool TemplateSectionsEqual(List<TemplateSection>? left, List<TemplateSection>? right) =>
        JsonSerializer.Serialize(left ?? new List<TemplateSection>(), TemplateSerializerOptions) ==
        JsonSerializer.Serialize(right ?? new List<TemplateSection>(), TemplateSerializerOptions);

    private static int TemplateSectionsHash(List<TemplateSection>? sections) =>
        JsonSerializer.Serialize(sections ?? new List<TemplateSection>(), TemplateSerializerOptions).GetHashCode();

    private static List<TemplateSection> TemplateSectionsClone(List<TemplateSection>? sections) =>
        JsonSerializer.Deserialize<List<TemplateSection>>(JsonSerializer.Serialize(sections ?? new List<TemplateSection>(), TemplateSerializerOptions), TemplateSerializerOptions)
        ?? new List<TemplateSection>();
}
