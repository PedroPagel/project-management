var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Project_Management_Api>("apiservice");
var hangfireService = builder.AddProject<Projects.Project_Management_Hangfire>("hangfire");

await builder.Build().RunAsync();
