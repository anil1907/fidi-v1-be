namespace VsaSample.Domain.Entities;

public interface IOrganizationScoped
{
    Guid OrganizationId { get; set; }
}
