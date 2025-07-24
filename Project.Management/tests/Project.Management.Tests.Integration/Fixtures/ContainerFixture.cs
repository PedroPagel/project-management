using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Management.Api;
using Project.Management.Infrastructure.Data;
using Testcontainers.PostgreSql;

namespace Project.Management.Tests.Integration.Fixtures
{
    internal class ContainerFixture : IAsyncLifetime
    {
        public PostgreSqlContainer Container { get; private set; }
        public HttpClient Client { get; private set; }

        public async Task DisposeAsync()
        {
            await Container.StopAsync();
            await Container.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            Container = new PostgreSqlBuilder()
                .WithImage("postgres:15.1")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithDatabase("project_management")
                .Build();

            await Container.StartAsync();

            var options = new DbContextOptionsBuilder<ProjectManagementDbContext>()
                .UseNpgsql(Container.GetConnectionString())
                .Options;

            using var context = new ProjectManagementDbContext(options);
            await context.Database.MigrateAsync();

            context.SeedData();

            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        var settings = new Dictionary<string, string>
                        {
                            ["ConnectionStrings:Default"] = Container.GetConnectionString()
                        };

                        config.AddInMemoryCollection(settings!);
                    });
                });

            Client = appFactory.CreateClient();
        }
    }
}
