using VsaSample.Application.Features.DietPlans.Shared;

namespace VsaSample.Application.Features.DietPlans.GetById;

internal sealed class GetDietPlanByIdQueryHandler(
    IDietPlanRepository repository,
    ILogger<GetDietPlanByIdQueryHandler> logger)
    : IQueryHandler<GetDietPlanByIdQuery, DietPlanResponse>
{
    public async Task<Result<DietPlanResponse>> Handle(GetDietPlanByIdQuery query, CancellationToken cancellationToken)
    {
        var dietPlan = await repository.GetByIdAsync(query.Id, cancellationToken);
        if (dietPlan is null)
        {
            return Result<DietPlanResponse>.Failure(DietPlanConstants.Errors.NotFound(query.Id));
        }

        logger.LogInformation("Retrieved diet plan {DietPlanId}", query.Id);

        return Result<DietPlanResponse>.Success(dietPlan.ToResponse());
    }
}
