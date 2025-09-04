# VsaSample

Sample solution showcasing a **clean architecture** style built on top of **.NET 9 minimal APIs**. The project is split into multiple layers that separate concerns and highlight common enterprise application patterns.

## Architecture

- **VsaSample.Api** – ASP.NET Core minimal API host. Provides endpoint groups, API versioning, Serilog logging, authentication and global middleware (validation, exception handling, request logging).
- **VsaSample.Application** – Application layer containing commands, queries and handlers following the mediator pattern.
- **VsaSample.Domain** – Domain entities and business rules such as `Product`, `Category`, and multilingual `ProductTranslation` models.
- **VsaSample.Infrastructure** – Infrastructure services including Entity Framework Core access, caching (Hybrid + Redis), JWT authentication, LDAP, Excel and FTP helpers, permission based authorization and Refit HTTP clients (e.g. the Pokémon API).
- **VsaSample.SharedKernel** – Cross‑cutting abstractions and utilities like result types, base entities, constants and permission definitions.

## Features

- Permission‑based authorization using a custom `HasPermission` attribute and policy provider.
- JWT authentication with token generation and validation.
- Entity Framework Core repositories with Sieve based filtering and paged results.
- Global exception handling and request validation middleware.
- Integration with external services using typed Refit clients (sample Pokémon client).
- Utilities for caching, Excel import/export, LDAP queries and FTP file transfer.
- Versioned minimal API endpoints for Products, Categories, SubCategories, Users and Pokémons.
- Serilog structured logging and health checks.

## Getting Started

### Prerequisites
- [.NET SDK 9.0](https://dotnet.microsoft.com/)

### Build and run the API
```bash
dotnet build
dotnet run --project src/VsaSample.Api
```

The API reads configuration from `appsettings.json` and environment specific files inside `src/VsaSample.Api`.

### Database Migrations
Apply Entity Framework Core migrations to keep the database schema up to date.

```bash
dotnet ef migrations add <MigrationName> --project src/VsaSample.Infrastructure --startup-project src/VsaSample.Api --output-dir Database/Migrations
dotnet ef database update --project src/VsaSample.Infrastructure --startup-project src/VsaSample.Api
```

### Running tests
```bash
dotnet test
```

## Project Structure
```
src/
  VsaSample.Api/             # API host and endpoints
  VsaSample.Application/     # Commands, queries and messaging
  VsaSample.Domain/          # Entities and domain logic
  VsaSample.Infrastructure/  # Data access and integrations
  VsaSample.SharedKernel/    # Shared cross‑cutting pieces
tests/
  VsaSample.Api.IntegrationTests/
  VsaSample.Application.UnitTests/
```

## External Dependencies
- ASP.NET Core, EF Core and Serilog
- Refit for typed HTTP clients
- Microsoft.Extensions.Caching (Hybrid + Redis)
- Sieve for filtering and sorting
