using Microsoft.Extensions.DependencyInjection;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Projects;
using Project.Management.Domain.Services.Roles;
using Project.Management.Domain.Services.Tasks;
using Project.Management.Domain.Services.Users;
using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Infrastructure.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServicesColletionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<INotificator, Notificator>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskItemService, TaskItemService>();
            services.AddScoped<IProjectMemberService, ProjectMemberService>();

            return services;
        }
    }
}
