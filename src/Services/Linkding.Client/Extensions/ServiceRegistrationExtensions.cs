using System.Net.Http.Headers;
using Linkding.Client;
using Linkding.Client.Options;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection Add_Linkding_HttpClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        var configSection = configuration.GetSection(LinkdingSettings.Position);
        services.Configure<LinkdingSettings>(configSection);
        services.AddHttpClient<LinkdingService>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5)) //Set lifetime to five minutes
            .AddPolicyHandler(GetRetryPolicy());
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}