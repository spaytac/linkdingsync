using Linkding;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.Add_Linkding_HttpClient(ctx.Configuration);
        services.Add_Linkding_Worker(ctx.Configuration);
        services.AddHostedService<Worker>();
    }).ConfigureHostConfiguration((builder) =>
    {
        builder
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .AddUserSecrets<Program>(true);
    })
    .Build();

await host.RunAsync();