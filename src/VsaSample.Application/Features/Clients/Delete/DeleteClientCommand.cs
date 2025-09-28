namespace VsaSample.Application.Features.Clients.Delete;

public sealed record DeleteClientCommand(Guid Id) : ICommand;
