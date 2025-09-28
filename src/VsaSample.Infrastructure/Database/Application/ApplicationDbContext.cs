using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace VsaSample.Infrastructure.Database.Application;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    private static readonly JsonSerializerOptions GoalsSerializerOptions = new(JsonSerializerDefaults.Web);

    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductTranslation> ProductTranslation { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);

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
}
