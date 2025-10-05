namespace VsaSample.Application.Features.Templates.Get;

using VsaSample.Application.Features.Templates.Shared;

public sealed record GetTemplatesPagedQuery(SieveModel Sieve, string? Search)
    : IQuery<PagedResult<TemplateResponse>>;
