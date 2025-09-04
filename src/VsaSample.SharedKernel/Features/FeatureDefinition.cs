using VsaSample.SharedKernel.Caching;

namespace VsaSample.SharedKernel.Features;

public abstract class FeatureDefinition
{
    protected FeatureDefinition(string slice, PermissionNames permissions)
        : this(permissions, new CacheKeys(slice), new TagTemplateKeys(slice))
    {
    }

    private FeatureDefinition(
        PermissionNames permissions,
        CacheKeys cache,
        TagTemplateKeys tagTemplates)
    {
        Cache = cache;
        TagTemplates = tagTemplates;
        Permissions = permissions;
    }

    public CacheKeys Cache { get; }

    public TagTemplateKeys TagTemplates { get; }
    public PermissionNames Permissions { get; }

    public IEnumerable<string> GetAllPermissions()
        => Permissions.GetType()
            .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            .Select(p => (string)p.GetValue(Permissions)!);

    public abstract class PermissionNames(string scope)
    {
        private string Scope { get; } = scope;

        public string Read { get; } = $"{scope}:Read";
        public string Create { get; } = $"{scope}:Create";
        public string Update { get; } = $"{scope}:Update";
        public string Delete { get; } = $"{scope}:Delete";

        protected string Custom(string name) => $"{Scope}:{name}";
    }
}
