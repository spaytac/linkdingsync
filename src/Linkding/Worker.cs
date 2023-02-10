using Core.Abstraction;
using Core.Entities.Linkding;
using Core.Handler;
using Linkding.Client;
using Linkding.Client.Options;
using Linkding.Options;
using Microsoft.Extensions.Options;

namespace Linkding;

public class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<Worker> _logger;
    private readonly LinkdingService _linkdingService;
    private readonly LinkdingSettings _linkdingSettings;
    private readonly WorkerSettings _settings;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, LinkdingService linkdingService, 
        IOptions<LinkdingSettings> linkdingSettings, IOptions<WorkerSettings> settings, IConfiguration configuration, IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _linkdingService = linkdingService;
        _configuration = configuration;
        _hostApplicationLifetime = hostApplicationLifetime;
        _settings = settings.Value;
        _linkdingSettings = linkdingSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await RunBookmarksTaskHandler();
            int delay = _settings.Interval * 60000;

            if (delay > 0)
            {
                _logger.LogInformation($"Worker paused for: {_settings.Interval} minutes");

                await Task.Delay(delay, stoppingToken);
            }
            else
            {
                _logger.LogInformation($"Interval value is '0' --> stopping worker");
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
    
    public async Task RunBookmarksTaskHandler()
    {
        IEnumerable<Bookmark> linkdingBookmarks = null;
        if (!string.IsNullOrEmpty(_linkdingSettings.Url) && _linkdingSettings.UpdateBookmarks)
        {
            _logger.LogInformation($"Starting updating bookmarks for {_linkdingSettings.Url}");

            _logger.LogInformation("Collecting Tag Handler");
            var tagHandlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ILinkdingTaskHandler).IsAssignableFrom(p) && p.IsClass);

            var updatedBookmarksCount = 0;
            var updateBookmarks = new List<Bookmark>();
            var deleteBookmarks = new List<Bookmark>();
            if (tagHandlers != null && tagHandlers.Count() > 0)
            {
                linkdingBookmarks = await _linkdingService.GetAllBookmarksAsync();
                if (linkdingBookmarks.Count() > 0)
                {
                    var tasksLowerCase = _settings.Tasks.Select(x => x.ToLower());

                    _logger.LogInformation($"{linkdingBookmarks.Count()} bookmarks found in {_linkdingSettings.Url}");

                    foreach (var handler in tagHandlers)
                    {
                        ILinkdingTaskHandler handlerInstance = null;
                        try
                        {
                            handlerInstance = (ILinkdingTaskHandler) Activator.CreateInstance(handler);

                            var task = tasksLowerCase.FirstOrDefault(x =>
                                x.Equals(handlerInstance.Command, StringComparison.InvariantCultureIgnoreCase));

                            if (string.IsNullOrEmpty(task))
                            {
                                continue;
                            }

                            foreach (var linkdingBookmark in linkdingBookmarks)
                            {
                                try
                                {
                                    _logger.LogDebug($"Start executing {task}");
                                    // var updateBookmark = updateBookmarks.FirstOrDefault(x => x.Id == linkdingBookmark.Id);
                                    var existingBookmarkIndexInt =
                                        updateBookmarks.FindIndex(x => x.Id == linkdingBookmark.Id);

                                    var bookmarkInstance = existingBookmarkIndexInt != -1
                                        ? updateBookmarks[existingBookmarkIndexInt]
                                        : linkdingBookmark;

                                    var result = await handlerInstance.ProcessAsync(bookmarkInstance, _logger, _configuration);

                                    if (result.HasError)
                                    {
                                        _logger.LogWarning(result.ErrorMessage, handlerInstance.Command);
                                    }
                                    else
                                    {
                                        if (result.PerformAction)
                                        {
                                            if (result.Action == LinkdingItemAction.Delete)
                                            {
                                                if (existingBookmarkIndexInt != -1)
                                                {
                                                    updateBookmarks.RemoveAt(existingBookmarkIndexInt);
                                                }

                                                var bookmarkToDelete = deleteBookmarks.FirstOrDefault(x =>
                                                    x.Url.ToLower() == result.Instance.Url.ToLower());
                                                if (bookmarkToDelete == null)
                                                {
                                                    deleteBookmarks.Add(result.Instance);
                                                }
                                            }
                                            else
                                            {
                                                if (existingBookmarkIndexInt != -1)
                                                {
                                                    updateBookmarks[existingBookmarkIndexInt] = result.Instance;
                                                }
                                                else
                                                {
                                                    updateBookmarks.Add(result.Instance);
                                                }

                                                linkdingBookmark.TagNames = result.Instance.TagNames;
                                            }
                                        }
                                    }

                                    _logger.LogDebug($"Finished {handlerInstance.Command}");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    var message = $"... {e.Message}";

                                    if (handlerInstance != null && !string.IsNullOrEmpty(handlerInstance.Command))
                                    {
                                        message = $"Error while executing {handlerInstance.Command}! {message}";
                                    }
                                    else
                                    {
                                        message = $"Error while executing handler! {message}";
                                    }

                                    _logger.LogError(message, "Calling Handler", e);
                                    // throw;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            // throw;
                        }
                    }
                }
                else
                {
                    _logger.LogInformation($"no bookmarks found in {_linkdingSettings.Url}");
                }

                if (updateBookmarks.Count() > 0)
                {
                    _logger.LogDebug($"Start updating bookmarks");
                    await _linkdingService.UpdateBookmarkCollectionAsync(updateBookmarks);
                    _logger.LogDebug($"Successfully updated bookmarks");
                }
            }

            _logger.LogInformation($"Finished updating bookmarks for {_linkdingSettings.Url}");
            
            _logger.LogInformation("Collecting Sync Handler");
            var syncHandlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ILinkdingSyncTaskHandler).IsAssignableFrom(p) && p.IsClass);

            if (syncHandlers != null && syncHandlers.Count() > 0)
            {
                if (linkdingBookmarks == null)
                {
                    linkdingBookmarks = await _linkdingService.GetAllBookmarksAsync();
                }

                if (linkdingBookmarks.Count() > 0)
                {
                    var tasksLowerCase = _settings.Tasks.Select(x => x.ToLower());

                    if (_settings.TargetLinkdingKey.Count == _settings.TargetLinkdingUrl.Count)
                    {
                        var targetInstances = new Dictionary<string, string>();
                        for (var i = 0; i < _settings.TargetLinkdingKey.Count(); i++)
                        {
                            LinkdingService targetService = null;
                            try
                            {
                                var url = _settings.TargetLinkdingUrl[i];
                                var key = _settings.TargetLinkdingKey[i];
                                targetService = LinkdingService.Create(url, key);
                                
                                foreach (var syncHandler in syncHandlers)
                                {
                                    ILinkdingSyncTaskHandler handlerInstance = null;
                                    try
                                    {
                                        handlerInstance = (ILinkdingSyncTaskHandler) Activator.CreateInstance(syncHandler);

                                        var task = tasksLowerCase.FirstOrDefault(x =>
                                            x.Equals(handlerInstance.Command, StringComparison.InvariantCultureIgnoreCase));

                                        if (string.IsNullOrEmpty(task))
                                        {
                                            continue;
                                        }
                            
                                        _logger.LogDebug($"Start executing {task}");
                                        await handlerInstance.ProcessAsync(linkdingBookmarks, targetService, _logger, _configuration);
                                        _logger.LogDebug($"{task} executed successfully!");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                        var message = $"... {ex.Message}";

                                        if (handlerInstance != null && !string.IsNullOrEmpty(handlerInstance.Command))
                                        {
                                            message = $"Error while executing {handlerInstance.Command}! {message}";
                                        }
                                        else
                                        {
                                            message = $"Error while executing handler! {message}";
                                        }

                                        _logger.LogError(message, "Calling Handler", ex);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                {throw;}
                            }
                            finally
                            {
                                targetService = null;
                            }
                        }
                    }
                }
            }
        }
    }
}