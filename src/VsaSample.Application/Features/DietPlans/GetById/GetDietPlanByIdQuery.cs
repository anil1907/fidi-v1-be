using VsaSample.Application.Features.DietPlans.Shared;

namespace VsaSample.Application.Features.DietPlans.GetById;

public sealed record GetDietPlanByIdQuery(Guid Id) : IQuery<DietPlanResponse>;
