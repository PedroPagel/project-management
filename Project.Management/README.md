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

## Technology Stack
- .NET / ASP.NET Core
- Entity Framework Core (InMemory provider for dev, Npgsql for PostgreSQL in production)
- Hangfire with PostgreSQL storage
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

## Testing
```bash
dotnet test
```
