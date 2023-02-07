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
#if DEBUG
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
#endif
            .AddUserSecrets<Program>(true)
            .AddCommandLine(args);
    })
    .Build();

await host.RunAsync();