using Linkding;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.Add_Linkding_HttpClient(ctx.Configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();