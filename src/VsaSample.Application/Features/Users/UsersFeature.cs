namespace VsaSample.Application.Features.Users;

using VsaSample.SharedKernel.Features;

public sealed class UsersFeature : FeatureDefinition
{
    private UsersFeature() : base("User", Permissions.Instance) { }

    public static readonly UsersFeature Instance = new();

    public new sealed class Permissions : PermissionNames
    {
        private Permissions() : base("Users") { }

        public static readonly Permissions Instance = new();

        public string Register => Custom("Register");
    }
}
