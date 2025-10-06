namespace VsaSample.Application.Features.DietPlans.Delete;

public sealed record DeleteDietPlanCommand(Guid Id) : ICommand;
