using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Project.Management.Aspire.ServiceDefaults;
using Project.Management.Hangfire.Jobs;
using Project.Management.Infrastructure.Configurations;
using Project.Management.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureDataLayer(builder.Configuration, builder.Environment.IsEnvironment("Development"));

builder.Services.AddHangfire((sp, config) =>
{
    config.UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings();

    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    config.UsePostgreSqlStorage(opt => opt.UseNpgsqlConnection(cs));
});

builder.Services.AddScoped<ProjectMaintenanceJob>();
builder.Services.AddHangfireServer();
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ProjectManagementDbContext>();

if (app.Environment.IsDevelopment())
{
    db.SeedData();
    app.UseHangfireDashboard("/hangfire");
}
else
{
    db.Database.Migrate();
}

var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
recurringJobs.AddOrUpdate<ProjectMaintenanceJob>(
    "project-maintenance",
    job => job.ExecuteAsync(CancellationToken.None),
    "0 * * * *",
    new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

app.MapGet("/", () => "OK");
app.Run();
