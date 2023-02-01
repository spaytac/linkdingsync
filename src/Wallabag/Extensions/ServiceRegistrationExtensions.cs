using Wallabag.Options;
using Wallabag.Settings;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection Add_Wallabag_Worker(this IServiceCollection services,
        IConfiguration configuration)
    {
        var configSection = configuration.GetSection(WorkerSettings.Position);
        services.Configure<WorkerSettings>(configSection);
        services.AddSingleton<SettingsService>();

        return services;
    }
}