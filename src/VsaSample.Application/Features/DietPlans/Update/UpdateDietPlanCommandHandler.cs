using VsaSample.Application.Features.DietPlans.Shared;

namespace VsaSample.Application.Features.DietPlans.Update;

internal sealed class UpdateDietPlanCommandHandler(
    IDietPlanRepository repository,
    IApplicationDbContext context,
    ITemplateRepository templateRepository,
    ILogger<UpdateDietPlanCommandHandler> logger)
    : ICommandHandler<UpdateDietPlanCommand>
{
    public async Task<Result> Handle(UpdateDietPlanCommand request, CancellationToken cancellationToken)
    {
        var dietPlan = await repository.GetEntityByIdAsync(request.Id, cancellationToken);
        if (dietPlan is null)
        {
            return Result.Failure(DietPlanConstants.Errors.NotFound(request.Id));
        }

        var clientExists = await context.Clients.AnyAsync(c => c.Id == request.ClientId, cancellationToken);
        if (!clientExists)
        {
            return Result.Failure(ClientConstants.Errors.NotFound(request.ClientId).Localize(request.Culture));
        }

        var template = await templateRepository.GetEntityByIdAsync(request.TemplateId, cancellationToken);
        if (template is null)
        {
            return Result.Failure(TemplateConstants.Errors.NotFound(request.TemplateId).Localize(request.Culture));
        }

        if (request.DateEnd < request.DateStart)
        {
            return Result.Failure(DietPlanConstants.Errors.InvalidDateRange.Localize(request.Culture));
        }

        dietPlan.ClientId = request.ClientId;
        dietPlan.TemplateId = request.TemplateId;
        dietPlan.Name = request.Name.Trim();
        dietPlan.DateStart = request.DateStart;
        dietPlan.DateEnd = request.DateEnd;
        dietPlan.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();

        if (request.IsActive.HasValue)
        {
            dietPlan.IsActive = request.IsActive.Value;
        }

        dietPlan.SetSections((request.Sections ?? Array.Empty<DietPlanSectionDto>()).Select(section => new TemplateSection(
            section.Id ?? string.Empty,
            section.Title,
            section.Items?.Select(item => new TemplateSectionItem(
                item.Id ?? string.Empty,
                item.Label,
                item.Amount ?? string.Empty,
                item.Note,
                item.Calories)).ToList() ?? new List<TemplateSectionItem>())));

        await repository.UpdateAsync(dietPlan, cancellationToken);

        logger.LogInformation("Diet plan {DietPlanId} updated", dietPlan.Id);

        return Result.Success();
    }
}

