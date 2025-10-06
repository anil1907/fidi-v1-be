namespace VsaSample.Application.Features.DietPlans.Delete;

internal sealed class DeleteDietPlanCommandHandler(
    IDietPlanRepository repository,
    ILogger<DeleteDietPlanCommandHandler> logger)
    : ICommandHandler<DeleteDietPlanCommand>
{
    public async Task<Result> Handle(DeleteDietPlanCommand request, CancellationToken cancellationToken)
    {
        var dietPlan = await repository.GetEntityByIdAsync(request.Id, cancellationToken);
        if (dietPlan is null)
        {
            return Result.Failure(DietPlanConstants.Errors.NotFound(request.Id));
        }

        await repository.DeleteAsync(dietPlan, cancellationToken);

        logger.LogInformation("Diet plan {DietPlanId} deleted", request.Id);

        return Result.Success();
    }
}
