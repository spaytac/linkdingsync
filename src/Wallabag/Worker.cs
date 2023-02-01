using Core.Abstraction;
using Core.Entities.Linkding;
using Core.Handler;
using Linkding.Client;
using Linkding.Client.Options;
using Microsoft.Extensions.Options;
using Wallabag.Client;
using Wallabag.Client.Options;
using Wallabag.Options;

namespace Wallabag;

public class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<Worker> _logger;
    private readonly LinkdingService _linkdingService;
    private readonly WallabagService _wallabagService;
    private readonly LinkdingSettings _linkdingSettings;
    private readonly WallabagSettings _wallabagSettings;
    private readonly WorkerSettings _settings;
    private readonly IConfiguration _configuration;
    

    public Worker(ILogger<Worker> logger, LinkdingService linkdingService, WallabagService wallabagService, 
        IOptions<LinkdingSettings> linkdingSettings, IOptions<WorkerSettings> settings, IOptions<WallabagSettings> wallabagSettings, IConfiguration configuration, IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _linkdingService = linkdingService;
        _wallabagService = wallabagService;
        _configuration = configuration;
        _hostApplicationLifetime = hostApplicationLifetime;
        _wallabagSettings = wallabagSettings.Value;
        _settings = settings.Value;
        _linkdingSettings = linkdingSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await RunSyncWallabag();
            int delay = _settings.Intervall * 60000;

            if (delay > 0)
            {
                _logger.LogInformation($"Worker paused for: {_settings.Intervall} minutes");

                await Task.Delay(delay, stoppingToken);
            }
            else
            {
                _logger.LogInformation($"Delay was set to '0' --> stopping worker");
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
    
    public async Task RunSyncWallabag()
    {
        if (!string.IsNullOrEmpty(_wallabagSettings.Url))
        {

            _logger.LogInformation($"Starting updating bookmarks for {_linkdingSettings.Url}");
            _logger.LogInformation("Collectin LinkdingService Handler");
            var wallabagHandlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ISyncTaskHandler<WallabagService>).IsAssignableFrom(p) && p.IsClass);

            if (wallabagHandlers != null && wallabagHandlers.Count() > 0)
            {
                var wallabags = await _wallabagService.GetEntries();

                foreach (var handler in wallabagHandlers)
                {
                    ISyncTaskHandler<WallabagService> handlerInstance = null;
                    try
                    {
                        handlerInstance = (ISyncTaskHandler<WallabagService>) Activator.CreateInstance(handler);

                        await handlerInstance.ProcessAsync(wallabags, _wallabagService, _linkdingService, _logger, _configuration);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            _logger.LogInformation($"no bookmarks found in {_linkdingSettings.Url}");
        }
    }
}