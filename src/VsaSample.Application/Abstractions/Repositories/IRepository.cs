using VsaSample.SharedKernel.Entities;

namespace VsaSample.Application.Abstractions.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> Query { get; }
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}

