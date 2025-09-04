namespace VsaSample.Application.Features.Categories;

using VsaSample.SharedKernel.Features;

public sealed class CategoriesFeature : FeatureDefinition
{
    private CategoriesFeature() : base("Category", Permissions.Instance) { }

    public static readonly CategoriesFeature Instance = new();

    public new sealed class Permissions : PermissionNames
    {
        private Permissions() : base("Categories") { }

        public static readonly Permissions Instance = new();
    }
}
