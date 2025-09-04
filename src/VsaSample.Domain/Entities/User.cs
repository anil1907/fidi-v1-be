namespace VsaSample.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }  = string.Empty;
    
    public string FirstName { get; set; }  = string.Empty;
    
    public string LastName { get; set; }  = string.Empty;

    public string Username { get; set; }  = string.Empty;
    
    public DateTime? LastLogin { get; set; }
    
    public string Role { get; set; }  = string.Empty;
}