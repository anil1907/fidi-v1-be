namespace VsaSample.Application.Features.Clients;

using VsaSample.SharedKernel.Features;

public sealed class ClientsFeature : FeatureDefinition
{
    private ClientsFeature() : base("Client", Permissions.Instance) { }

    public static readonly ClientsFeature Instance = new();

    public new sealed class Permissions : PermissionNames
    {
        private Permissions() : base("Clients") { }

        public static readonly Permissions Instance = new();
    }
}
