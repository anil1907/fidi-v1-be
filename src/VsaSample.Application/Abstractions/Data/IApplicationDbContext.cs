namespace VsaSample.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Category> Categories { get; set; }

    public DbSet<SubCategory> SubCategories { get; set; }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<ProductTranslation> ProductTranslation { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}