namespace VsaSample.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Appointment> Appointments { get; set; }

    public DbSet<DietPlan> DietPlans { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
