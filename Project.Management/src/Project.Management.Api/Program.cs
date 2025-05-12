using Project.Management.Api.Middlewares;
using Project.Management.Infrastructure.Configurations;
using Project.Management.Infrastructure.Extensions;
using Project.Management.ServiceDefaults;

namespace Project.Management.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.ConfigureDataLayer(builder.Configuration);
        builder.Services.AddRepositories();
        builder.Services.AddSingleton<ExceptionHandlerMiddleware>();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.Run();
    }
}
