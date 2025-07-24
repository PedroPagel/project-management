using Microsoft.EntityFrameworkCore;
using Project.Management.Api.Mappings;
using Project.Management.Api.Middlewares;
using Project.Management.Infrastructure.Configurations;
using Project.Management.Infrastructure.Data;
using Project.Management.Infrastructure.Extensions;

namespace Project.Management.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.ConfigureDataLayer(builder.Configuration, builder.Environment.IsEnvironment("Development"));
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddRepositories();
        builder.Services.AddLogging();
        builder.Services.AddServices();
        builder.Services.AddSwagger();

        builder.Services.AddSingleton<ExceptionHandlerMiddleware>();

        var app = builder.Build();

        app.MapDefaultEndpoints();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProjectManagementDbContext>();

        if (app.Environment.IsDevelopment())
        {
            db.SeedData();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Management API v1");
                c.RoutePrefix = string.Empty;
            });

            app.MapOpenApi();
        }
        else
        {
            db.Database.Migrate();
        }

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.Run();
    }
}
