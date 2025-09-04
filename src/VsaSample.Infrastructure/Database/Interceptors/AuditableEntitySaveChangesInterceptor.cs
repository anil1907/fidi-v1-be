using VsaSample.SharedKernel.Entities;

namespace VsaSample.Infrastructure.Database.Interceptors;

public sealed class AuditableEntitySaveChangesInterceptor(IUserContext userContext) : SaveChangesInterceptor
{
    private void UpdateEntities(DbContext? context)
    {
        if (context is null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>()
                     .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            var isNew = entry.Entity.Id == 0;
            SetAuditFields(entry.Entity, isNew);
        }
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetAuditFields(BaseEntity entity, bool isNew)
    {
        var userId = userContext.UserId;
        var currentUtcDateTime = DateTime.UtcNow;

        if (isNew)
        {
            entity.CreateDate = currentUtcDateTime;
            entity.CreatedBy = userId;
        }
        else
        {
            entity.UpdateDate = currentUtcDateTime;
            entity.UpdatedBy = userId;
        }
    }
}
