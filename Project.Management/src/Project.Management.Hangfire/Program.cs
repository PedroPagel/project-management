using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using Project.Management.Aspire.ServiceDefaults;
using Project.Management.Hangfire.Jobs;
using Project.Management.Infrastructure.Configurations;
using Project.Management.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureDataLayer(builder.Configuration, builder.Environment.IsEnvironment("Development"));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage());
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("DefaultConnection is not configured for Hangfire storage.");
    }
    builder.Services.AddHangfire(configuration => configuration.UsePostgreSqlStorage(connectionString));
}

builder.Services.AddHangfireServer();

builder.Services.AddScoped<ProjectMaintenanceJob>();

var app = builder.Build();

app.UseHangfireDashboard("/hangfire");

using (var scope = app.Services.CreateScope())
{
    var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJobs.AddOrUpdate<ProjectMaintenanceJob>(
        "project-maintenance",
        job => job.ExecuteAsync(CancellationToken.None),
        "0 * * * *",
        new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
}

await app.RunAsync();
