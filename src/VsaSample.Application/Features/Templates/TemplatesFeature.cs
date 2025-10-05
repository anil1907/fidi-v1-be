namespace VsaSample.Application.Features.Templates;

using VsaSample.SharedKernel.Features;

public sealed class TemplatesFeature : FeatureDefinition
{
    private TemplatesFeature() : base("Template", Permissions.Instance) { }

    public static readonly TemplatesFeature Instance = new();

    public new sealed class Permissions : PermissionNames
    {
        private Permissions() : base("Templates") { }

        public static readonly Permissions Instance = new();
    }
}
