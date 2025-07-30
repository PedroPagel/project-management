using Microsoft.Extensions.DependencyInjection;
using Project.Management.Domain.Repositories;
using Project.Management.Infrastructure.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Infrastructure.Extensions
{
    [ExcludeFromCodeCoverage]
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
