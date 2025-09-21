namespace VsaSample.Infrastructure.Database.Application;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
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
    }
}
