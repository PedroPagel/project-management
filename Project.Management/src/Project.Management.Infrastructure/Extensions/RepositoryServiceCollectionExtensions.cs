using Microsoft.Extensions.DependencyInjection;
using Project.Management.Domain.Repositories;
using Project.Management.Infrastructure.Repositories;

namespace Project.Management.Infrastructure.Extensions
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();

            return services;
        }
    }
}
