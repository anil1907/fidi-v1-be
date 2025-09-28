namespace VsaSample.Application.Features.Clients.Shared;

public static class ClientMappings
{
    public static ClientResponse ToResponse(this Client client) => new()
    {
        Id = client.Id,
        FirstName = client.FirstName,
        LastName = client.LastName,
        Email = client.Email,
        Phone = client.Phone,
        Notes = client.Notes,
        Goals = client.Goals.AsReadOnly(),
        IsActive = client.IsActive,
        CreateDate = client.CreateDate
    };
}
