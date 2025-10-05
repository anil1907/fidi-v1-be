namespace VsaSample.Application.Abstractions.Repositories;

public interface ITemplateRepository : IRepository<Template>
{
    Task<Template?> GetEntityByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
