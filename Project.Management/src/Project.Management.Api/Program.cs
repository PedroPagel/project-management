using Microsoft.EntityFrameworkCore;
using Project.Management.Api.Mappings;
using Project.Management.Api.Middlewares;
using Project.Management.Api.Messaging;
using Project.Management.Aspire.ServiceDefaults;
using Project.Management.Infrastructure.Configurations;
using Project.Management.Infrastructure.Data;
using Project.Management.Infrastructure.Extensions;
using System.Diagnostics;
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
        StartRabbitMqDockerIfConfigured(builder);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.ConfigureDataLayer(builder.Configuration, builder.Environment.IsEnvironment("Development"));
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddRepositories();
        builder.Services.AddLogging();
        builder.Services.AddServices();
        builder.Services.AddSwagger();
        var rabbitMqOptions = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>() ?? new RabbitMqOptions();
        builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

        if (rabbitMqOptions.UseInMemory)
        {
            builder.Services.AddSingleton<IUserCreationQueue, InMemoryUserCreationQueue>();
            builder.Services.AddHostedService<InMemoryUserCreationListener>();
        }
        else
        {
            builder.Services.AddHostedService<UserCreationRabbitMqListener>();
        }

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

    private static void StartRabbitMqDockerIfConfigured(WebApplicationBuilder builder)
    {
        var dockerSection = builder.Configuration.GetSection("Docker");
        if (!dockerSection.GetValue<bool>("StartRabbitMqOnStartup"))
        {
            return;
        }

        var scriptPath = dockerSection.GetValue<string>("ScriptPath") ?? "scripts/run-rabbitmq.sh";
        var fullScriptPath = Path.IsPathRooted(scriptPath)
            ? scriptPath
            : Path.Combine(builder.Environment.ContentRootPath, scriptPath);

        if (!File.Exists(fullScriptPath))
        {
            Console.WriteLine($"Docker startup script not found: {fullScriptPath}");
            return;
        }

        var extension = Path.GetExtension(fullScriptPath);
        var (fileName, arguments) = BuildScriptCommand(extension, fullScriptPath);

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine($"Unsupported script type for Docker startup: {fullScriptPath}");
            return;
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };
        foreach (var argument in arguments)
        {
            process.StartInfo.ArgumentList.Add(argument);
        }

        process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                Console.WriteLine($"[rabbitmq-docker] {e.Data}");
            }
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                Console.WriteLine($"[rabbitmq-docker][stderr] {e.Data}");
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            Console.WriteLine($"RabbitMQ Docker startup script exited with code {process.ExitCode}.");
        }
    }

    private static (string FileName, List<string> Arguments) BuildScriptCommand(string extension, string scriptPath)
    {
        var arguments = new List<string>();

        if (string.Equals(extension, ".ps1", StringComparison.OrdinalIgnoreCase))
        {
            var fileName = OperatingSystem.IsWindows() ? "powershell" : "pwsh";
            arguments.AddRange(new[] { "-NoProfile", "-ExecutionPolicy", "Bypass", "-File", scriptPath });
            return (fileName, arguments);
        }

        if (string.Equals(extension, ".sh", StringComparison.OrdinalIgnoreCase))
        {
            arguments.Add(scriptPath);
            return ("bash", arguments);
        }

        return (string.Empty, arguments);
    }
}
