# Project Management

## Overview
Project Management is a .NET solution that exposes a REST API for managing projects, tasks, users, roles, and project membership. It follows a layered architecture with a dedicated domain model, infrastructure layer, and an API surface, plus a Hangfire worker for scheduled maintenance jobs. A .NET Aspire AppHost ties the services together for local orchestration.

## Solution Structure
- **Project.Management.Api**: ASP.NET Core Web API with controllers, DTOs, AutoMapper mappings, middleware, and Swagger/OpenAPI support.
- **Project.Management.Domain**: Domain entities (Project, TaskItem, User, Role, ProjectMember) plus enums, repositories, and service contracts.
- **Project.Management.Infrastructure**: EF Core DbContext, repository implementations, data configuration, migrations, and Swagger configuration helpers.
- **Project.Management.Hangfire**: Background job host using Hangfire with a recurring project maintenance job and dashboard support.
- **Project.Management.Aspire.AppHost**: .NET Aspire distributed application host wiring the API, Hangfire, and Redis cache together.
- **Project.Management.Aspire.ServiceDefaults**: Shared service defaults for Aspire projects.

## Key Features
- CRUD endpoints for projects, tasks, users, roles, and project members.
- AutoMapper DTO mapping between API contracts and domain models.
- Domain services and repositories for business logic and persistence concerns.
- Environment-aware database setup:
  - Development uses an in-memory EF Core database and seeds sample data.
  - Non-development environments use PostgreSQL with migrations applied on startup.
- Swagger/OpenAPI with UI available in development.
- Hangfire background processing with a recurring maintenance job and optional dashboard in development.
- RabbitMQ listener for user creation events with optional in-memory mode for local development.

## Architecture & Patterns
- **Domain-Driven Design (DDD)**: The solution separates Domain, Infrastructure, and API layers, with entities and services housed in the domain project.
- **Repository Pattern**: Domain repositories define contracts (`IRepository<TEntity>`), with EF Core implementations in the Infrastructure layer.
- **Notification Pattern**: Domain services use the `INotificator`/`Notificator` to collect and surface validation/business errors to the API layer.
- **Validation Pattern**: FluentValidation validators enforce business rules for create/update flows (projects, tasks, users, roles, members).

## Technology Stack
- .NET / ASP.NET Core
- Entity Framework Core (InMemory provider for dev, Npgsql for PostgreSQL in production)
- Hangfire with PostgreSQL storage
- RabbitMQ (RabbitMQ.Client)
- AutoMapper
- Swagger/OpenAPI
- .NET Aspire for service orchestration

## Running the Solution
### Prerequisites
- .NET SDK compatible with the solution
- PostgreSQL (for non-development environments)

### Run with .NET Aspire
The AppHost coordinates the API, Hangfire, and Redis services.

```bash
dotnet run --project Project.Management.Aspire/Project.Management.Aspire.AppHost
```

### Run the API directly
```bash
dotnet run --project src/Project.Management.Api
```

In development, Swagger UI is available at `/swagger` and the OpenAPI document at `/openapi/v1.json`.

### Run the Hangfire worker directly
```bash
dotnet run --project src/Project.Management.Hangfire
```

In development, the Hangfire dashboard is available at `/hangfire`.

## Configuration
- Connection strings are read from `ConnectionStrings:DefaultConnection`.
- In development, the system uses an in-memory database and seeds data at startup.
- In other environments, migrations run automatically on startup.
- RabbitMQ settings live under `RabbitMq` in API configuration (`appsettings*.json`), including:
  - `Enabled`, `UseInMemory`, `HostName`, `Port`, `UserName`, `Password`, `VirtualHost`, and `QueueName`.
  - `Docker:StartRabbitMqOnStartup` and `Docker:ScriptPath` for optional local Docker startup.

### RabbitMQ (local)
- Docker Compose includes a RabbitMQ container (`rabbitmq:3-management`) for local development.
- Helper scripts are available in `scripts/run-rabbitmq.sh` and `scripts/run-rabbitmq.ps1`.
- When enabled, the API hosts a background listener that consumes the `user.create` queue and creates users from incoming messages.

## Testing
```bash
dotnet test
```
