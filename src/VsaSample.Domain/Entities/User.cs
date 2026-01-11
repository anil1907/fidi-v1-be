namespace VsaSample.Domain.Entities;

public class User : BaseEntity, IOrganizationScoped
{
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    public string Email { get; set; }  = string.Empty;
    
    public string FirstName { get; set; }  = string.Empty;
    
    public string LastName { get; set; }  = string.Empty;

    public string Username { get; set; }  = string.Empty;
    
    public DateTime? LastLogin { get; set; }
    
    public UserRole Role { get; private set; } = UserRole.Dietitian;
    
    public string PasswordHash { get; private set; } = string.Empty;

    public void SetRole(UserRole role) => Role = role;

    public void SetPasswordHash(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        PasswordHash = passwordHash;
    }
}
