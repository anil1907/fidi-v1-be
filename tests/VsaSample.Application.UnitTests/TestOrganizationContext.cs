using VsaSample.Application.Abstractions.Authentication;

namespace VsaSample.Application.UnitTests;

internal sealed class TestOrganizationContext(Guid organizationId) : IOrganizationContext
{
    public Guid OrganizationId { get; } = organizationId;
    public Guid UserId { get; } = Guid.Empty;
    public bool IsSuperAdmin { get; } = false;
}
