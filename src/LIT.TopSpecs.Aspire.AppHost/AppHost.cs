var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.LIT_TopSpecs_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.LIT_TopSpecs_WebApp>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
