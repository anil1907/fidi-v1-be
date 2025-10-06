namespace VsaSample.Application.Features.DietPlans;

using VsaSample.SharedKernel.Features;

public sealed class DietPlansFeature : FeatureDefinition
{
    private DietPlansFeature() : base("DietPlan", Permissions.Instance) { }

    public static readonly DietPlansFeature Instance = new();

    public new sealed class Permissions : PermissionNames
    {
        private Permissions() : base("DietPlans") { }

        public static readonly Permissions Instance = new();
    }
}
