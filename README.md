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
- Domain services and repositories encapsulating business logic and persistence concerns.
- Environment-aware database setup:
  - Development uses an in-memory EF Core database and seeds sample data.
  - Non-development environments use PostgreSQL with migrations applied on startup.
- Swagger/OpenAPI with UI available in development.
- Hangfire background processing with a recurring maintenance job and optional dashboard in development.

## Architecture & Design
- Layered architecture with clear separation of concerns.
- Domain-centric design with explicit boundaries between API, domain, and infrastructure.
- Emphasis on testability, maintainability, and extensibility.

## Quality & Engineering Practices
- Automated testing strategy applied where appropriate.
- Static code analysis and quality gates enforced via **SonarQube** (coverage, complexity, duplication).
- Consistent code style through shared conventions and analyzers.
- Focus on maintainable, readable code over framework-specific abstractions.

## CI/CD
- Continuous Integration implemented with **GitHub Actions**.
- Automated build and test execution on pull requests.
- Static analysis and quality validation integrated into the pipeline.

## Tooling
- .NET / ASP.NET Core
- Entity Framework Core (InMemory provider for dev, Npgsql for PostgreSQL in production)
- Hangfire with PostgreSQL storage
- AutoMapper
- Swagger/OpenAPI
- GitHub Actions
- SonarQube
- .NET Aspire for service orchestration

> **Note:** AI-assisted tooling (Codex) was used selectively for boilerplate generation and exploratory refactoring. All architectural decisions, code reviews, and final implementations were performed manually.

## Running the Solution

### Prerequisites
- .NET SDK compatible with the solution
- PostgreSQL (for non-development environments)

### Run with .NET Aspire
The AppHost coordinates the API, Hangfire, and Redis services.

```bash
dotnet run --project Project.Management.Aspire/Project.Management.Aspire.AppHost
