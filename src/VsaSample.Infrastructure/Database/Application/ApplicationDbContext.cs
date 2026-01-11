using System.Linq.Expressions;
using System.Security;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace VsaSample.Infrastructure.Database.Application;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IOrganizationContext organizationContext)
    : DbContext(options), IApplicationDbContext
{
    private static readonly JsonSerializerOptions GoalsSerializerOptions = new(JsonSerializerDefaults.Web);
    private static readonly JsonSerializerOptions TemplateSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly IOrganizationContext _organizationContext = organizationContext;

    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<DietPlan> DietPlans { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);

        ConfigureOrganizations(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureAppointments(modelBuilder);
        ConfigureClients(modelBuilder);
        ConfigureTemplates(modelBuilder);
        ConfigureDietPlans(modelBuilder);

        ApplyOrganizationQueryFilters(modelBuilder);
    }

    private static void ConfigureOrganizations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organization>(builder =>
        {
            builder.ToTable("Organizations");

            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(o => o.Name)
                .IsUnique();
        });
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasIndex(u => new { u.OrganizationId, u.Id });

            builder.HasOne(u => u.Organization)
                .WithMany()
                .HasForeignKey(u => u.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

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

            builder.HasIndex(a => new { a.OrganizationId, a.Id });
            builder.HasIndex(a => new { a.OrganizationId, a.ClientId });
            builder.HasIndex(a => a.ClientId);
            builder.HasIndex(a => a.StartsAt);
            builder.HasIndex(a => a.EndsAt);

            builder.HasOne(a => a.Organization)
                .WithMany()
                .HasForeignKey(a => a.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

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
            builder.HasIndex(c => new { c.OrganizationId, c.Id });

            builder.HasOne(c => c.Organization)
                .WithMany()
                .HasForeignKey(c => c.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

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
            builder.HasIndex(t => new { t.OrganizationId, t.Id });

            builder.HasOne(t => t.Organization)
                .WithMany()
                .HasForeignKey(t => t.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

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

    private static void ConfigureDietPlans(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DietPlan>(builder =>
        {
            builder.HasIndex(p => new { p.OrganizationId, p.Id });
            builder.HasIndex(p => new { p.OrganizationId, p.ClientId });

            builder.HasOne(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Name).HasMaxLength(256);
            builder.Property(p => p.Notes).HasMaxLength(2048);

            builder.Property(p => p.DateStart)
                .HasColumnType("date");

            builder.Property(p => p.DateEnd)
                .HasColumnType("date");

            var sectionsProperty = builder.Property(p => p.Sections)
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

            builder.HasIndex(p => p.ClientId);
            builder.HasIndex(p => p.TemplateId);
            builder.HasIndex(p => p.DateStart);

            builder.HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Template)
                .WithMany()
                .HasForeignKey(p => p.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);
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

    public override int SaveChanges()
    {
        ApplyOrganizationScopeRules();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyOrganizationScopeRules();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyOrganizationScopeRules()
    {
        var currentOrgId = _organizationContext.OrganizationId;
        var isSuperAdmin = _organizationContext.IsSuperAdmin;

        foreach (var entry in ChangeTracker.Entries<IOrganizationScoped>()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            var entity = entry.Entity;

            if (entry.State == EntityState.Added && entity.OrganizationId == Guid.Empty && currentOrgId != Guid.Empty)
            {
                entity.OrganizationId = currentOrgId;
            }

            if (currentOrgId == Guid.Empty || isSuperAdmin)
            {
                continue;
            }

            if (entity.OrganizationId != currentOrgId)
            {
                throw new SecurityException("OrganizationId does not match the current tenant context.");
            }
        }
    }

    private void ApplyOrganizationQueryFilters(ModelBuilder modelBuilder)
    {
        Expression<Func<Appointment, bool>> appointmentFilter =
            e => _organizationContext.IsSuperAdmin ||
                 _organizationContext.OrganizationId == Guid.Empty ||
                 e.OrganizationId == _organizationContext.OrganizationId;

        modelBuilder.Entity<Appointment>().HasQueryFilter(appointmentFilter);

        Expression<Func<Client, bool>> clientFilter =
            e => _organizationContext.IsSuperAdmin ||
                 _organizationContext.OrganizationId == Guid.Empty ||
                 e.OrganizationId == _organizationContext.OrganizationId;

        modelBuilder.Entity<Client>().HasQueryFilter(clientFilter);

        Expression<Func<DietPlan, bool>> dietPlanFilter =
            e => _organizationContext.IsSuperAdmin ||
                 _organizationContext.OrganizationId == Guid.Empty ||
                 e.OrganizationId == _organizationContext.OrganizationId;

        modelBuilder.Entity<DietPlan>().HasQueryFilter(dietPlanFilter);

        Expression<Func<Template, bool>> templateFilter =
            e => _organizationContext.IsSuperAdmin ||
                 _organizationContext.OrganizationId == Guid.Empty ||
                 e.OrganizationId == _organizationContext.OrganizationId;

        modelBuilder.Entity<Template>().HasQueryFilter(templateFilter);

        Expression<Func<User, bool>> userFilter =
            e => _organizationContext.IsSuperAdmin ||
                 _organizationContext.OrganizationId == Guid.Empty ||
                 e.OrganizationId == _organizationContext.OrganizationId;

        modelBuilder.Entity<User>().HasQueryFilter(userFilter);
    }
}
