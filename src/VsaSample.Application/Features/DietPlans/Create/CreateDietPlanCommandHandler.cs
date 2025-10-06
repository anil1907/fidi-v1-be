using VsaSample.Application.Features.DietPlans.Shared;

namespace VsaSample.Application.Features.DietPlans.Create;

public sealed class CreateDietPlanCommandHandler(
    IApplicationDbContext context,
    ITemplateRepository templateRepository,
    IDietPlanRepository repository,
    ILogger<CreateDietPlanCommandHandler> logger)
    : ICommandHandler<CreateDietPlanCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateDietPlanCommand request, CancellationToken cancellationToken)
    {
        var clientExists = await context.Clients.AnyAsync(c => c.Id == request.ClientId, cancellationToken);
        if (!clientExists)
        {
            return Result<Guid>.Failure(ClientConstants.Errors.NotFound(request.ClientId).Localize(request.Culture));
        }

        var template = await templateRepository.GetEntityByIdAsync(request.TemplateId, cancellationToken);
        if (template is null)
        {
            return Result<Guid>.Failure(TemplateConstants.Errors.NotFound(request.TemplateId).Localize(request.Culture));
        }

        if (request.DateEnd < request.DateStart)
        {
            return Result<Guid>.Failure(DietPlanConstants.Errors.InvalidDateRange.Localize(request.Culture));
        }

        var dietPlan = new DietPlan
        {
            ClientId = request.ClientId,
            TemplateId = request.TemplateId,
            Name = request.Name.Trim(),
            DateStart = request.DateStart,
            DateEnd = request.DateEnd,
            Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
            IsActive = true
        };

        dietPlan.SetSections((request.Sections ?? Array.Empty<DietPlanSectionDto>()).Select(section => new TemplateSection(
            section.Id ?? string.Empty,
            section.Title,
            section.Items?.Select(item => new TemplateSectionItem(
                item.Id ?? string.Empty,
                item.Label,
                item.Amount ?? string.Empty,
                item.Note,
                item.Calories)).ToList() ?? new List<TemplateSectionItem>())));

        var id = await repository.AddAsync(dietPlan, cancellationToken);

        logger.LogInformation("Diet plan {DietPlanId} created for client {ClientId}", id, request.ClientId);

        return Result<Guid>.Success(id);
    }
}

