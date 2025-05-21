var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Project_Management_Api>("apiservice");

await builder.Build().RunAsync();
