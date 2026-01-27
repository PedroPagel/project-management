@echo off
REM Usage: add-migration.cmd MigrationName

IF "%~1"=="" (
    ECHO Please provide a migration name.
    EXIT /B 1
)

dotnet ef migrations add %1 ^
  --project .\Project.Management.Infrastructure\Project.Management.Infrastructure.csproj ^
  --startup-project .\Project.Management.API\Project.Management.API.csproj
