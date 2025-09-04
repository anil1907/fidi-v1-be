namespace VsaSample.Application.Features.SubCategories;

using VsaSample.SharedKernel.Features;

public sealed class SubCategoriesFeature : FeatureDefinition
{
    private SubCategoriesFeature() : base("SubCategory", Permissions.Instance) { }

    public static readonly SubCategoriesFeature Instance = new();

    public new sealed class Permissions : PermissionNames
    {
        private Permissions() : base("SubCategories") { }

        public static readonly Permissions Instance = new();

        public string Export => Custom("Export");
        public string Import => Custom("Import");
    }
}

