namespace VsaSample.Application.Features.Templates.GetById;

using VsaSample.Application.Features.Templates.Shared;

public sealed record GetTemplateByIdQuery(Guid Id) : IQuery<TemplateResponse>;
