using VsaSample.Application.Features.Clients.Shared;

namespace VsaSample.Application.Features.Clients.GetById;

public sealed record GetClientByIdQuery(Guid Id) : IQuery<ClientResponse>;
