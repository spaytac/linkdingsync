using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using Wallabag.Client;
using Wallabag.Client.Contracts;
using Wallabag.Client.OAuth;
using Wallabag.Client.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection Add_Wallabag_HttpClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        var configSection = configuration.GetSection(WallabagSettings.Position);
        services.Configure<WallabagSettings>(configSection);
        // services.AddScoped<IAccessTokenProvider, OAuthTokenProvider>();
        // services.AddScoped<AuthenticationClient>();
        services.AddSingleton<IAccessTokenProvider, OAuthTokenProvider>();
        services.AddSingleton<AuthenticationClient>();
        services.AddHttpClient<WallabagService>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5)) //Set lifetime to five minutes
            .AddPolicyHandler(GetRetryPolicy());
        
        // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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