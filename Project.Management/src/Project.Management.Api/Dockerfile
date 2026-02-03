FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Project.Management.sln ./
COPY src/Project.Management.Api/Project.Management.Api.csproj src/Project.Management.Api/
COPY src/Project.Management.Infrastructure/Project.Management.Infrastructure.csproj src/Project.Management.Infrastructure/
COPY Project.Management.Aspire/Project.Management.Aspire.ServiceDefaults/Project.Management.Aspire.ServiceDefaults.csproj Project.Management.Aspire/Project.Management.Aspire.ServiceDefaults/

RUN dotnet restore src/Project.Management.Api/Project.Management.Api.csproj

COPY . .
RUN dotnet publish src/Project.Management.Api/Project.Management.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Project.Management.Api.dll"]
