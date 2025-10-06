namespace VsaSample.Application.Abstractions.Repositories;

public interface IDietPlanRepository : IRepository<DietPlan>
{
    Task<DietPlan?> GetEntityByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
