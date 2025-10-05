namespace VsaSample.Application.Features.Appointments.Get;

using VsaSample.Application.Features.Appointments.Shared;

internal sealed class GetAppointmentsPagedQueryHandler(
    IAppointmentRepository repository,
    ISieveProcessor sieve,
    ILogger<GetAppointmentsPagedQueryHandler> logger)
    : IQueryHandler<GetAppointmentsPagedQuery, PagedResult<AppointmentResponse>>
{
    public async Task<Result<PagedResult<AppointmentResponse>>> Handle(GetAppointmentsPagedQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Appointment> source = repository.Query;

        if (query.ClientId is not null)
        {
            source = source.Where(a => a.ClientId == query.ClientId);
        }

        if (query.Status is not null)
        {
            var status = query.Status.Value;
            source = source.Where(a => a.Status == status);
        }

        if (query.StartsAfter is not null)
        {
            source = source.Where(a => a.StartsAt >= query.StartsAfter);
        }

        if (query.EndsBefore is not null)
        {
            source = source.Where(a => a.EndsAt <= query.EndsBefore);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim().ToLowerInvariant();
            source = source.Where(a =>
                a.Title.ToLower().Contains(term) ||
                (a.Description != null && a.Description.ToLower().Contains(term)));
        }

        var filtered = sieve.Apply(query.Sieve, source, applyPagination: false);
        var total = await filtered.CountAsync(cancellationToken);

        var paged = sieve.Apply(query.Sieve, filtered, applyPagination: true);
        var items = await paged
            .Select(a => a.ToResponse())
            .ToListAsync(cancellationToken);

        var pageSize = query.Sieve.PageSize ?? 10;
        var pageNumber = query.Sieve.Page ?? 1;
        var payload = PagedResult<AppointmentResponse>.Create(items, pageNumber, pageSize, total);

        logger.LogInformation("Fetched appointments page {Page}/{Size} (Total: {Total})", pageNumber, pageSize, total);

        return Result<PagedResult<AppointmentResponse>>.Success(payload);
    }
}
