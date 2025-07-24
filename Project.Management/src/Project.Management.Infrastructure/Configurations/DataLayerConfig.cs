using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Infrastructure.Configurations
{
    public static class DataLayerConfig
    {
        public static IServiceCollection ConfigureDataLayer(this IServiceCollection services, IConfiguration configuration, bool development)
        {
            if (development)
            {
                services.AddDbContext<ProjectManagementDbContext>(options =>
                options.UseInMemoryDatabase("DevInMemoryDb"));

                return services;
            }

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ProjectManagementDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
