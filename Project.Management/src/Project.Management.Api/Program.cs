using Microsoft.EntityFrameworkCore;
using Project.Management.Api.Mappings;
using Project.Management.Api.Middlewares;
using Project.Management.Api.Messaging;
using Project.Management.Aspire.ServiceDefaults;
using Project.Management.Infrastructure.Configurations;
using Project.Management.Infrastructure.Data;
using Project.Management.Infrastructure.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Api;

[ExcludeFromCodeCoverage]
public class Program
{
    protected Program()
    {
    }

    static void Main(string[] args)
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
        builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
        builder.Services.AddHostedService<UserCreationRabbitMqListener>();

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Trace);


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
