namespace VsaSample.Application.Abstractions.Authentication;

public interface IOrganizationContext
{
    Guid OrganizationId { get; }
    Guid UserId { get; }
    bool IsSuperAdmin { get; }
}
