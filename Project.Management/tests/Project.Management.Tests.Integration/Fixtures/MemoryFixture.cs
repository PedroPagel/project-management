using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.Management.Api;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Tests.Integration.Fixtures
{
    public class MemoryFixture : IAsyncLifetime
    {
        public HttpClient Client { get; private set; }

        public async Task InitializeAsync()
        {
            var inMemoryDbName = Guid.NewGuid().ToString();

            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<ProjectManagementDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<ProjectManagementDbContext>(options =>
                        {
                            options.UseInMemoryDatabase(inMemoryDbName);
                        });

                        var sp = services.BuildServiceProvider();

                        using var scope = sp.CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<ProjectManagementDbContext>();

                        context.Database.EnsureCreated();
                        context.SeedData();
                    });
                });

            Client = appFactory.CreateClient();

            await Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
