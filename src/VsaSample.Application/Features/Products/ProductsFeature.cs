namespace VsaSample.Application.Features.Products;

using VsaSample.SharedKernel.Features;

public sealed class ProductsFeature : FeatureDefinition
{
    private ProductsFeature() : base("Product", Permissions.Instance) { }

    public static readonly ProductsFeature Instance = new();

    public new sealed class Permissions : PermissionNames
    {
        private Permissions() : base("Products") { }

        public static readonly Permissions Instance = new();
    }
}
