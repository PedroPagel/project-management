using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Infrastructure.Configurations
{
    public static class DataLayerConfig
    {
        public static IServiceCollection ConfigureDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ProjectManagement:ConnectionString"];

            services.AddDbContext<ProjectManagementDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
