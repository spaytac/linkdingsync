using System.Text.Json;
using System.Text.RegularExpressions;
using Core.Entities.Wallabag;
using Core.Handler;
using Linkding.Client;
using Wallabag.Client;
using Wallabag.Settings;

namespace Wallabag.Handler
{
    public class LinkdingBookmarkToWallabagHandler : ISyncTaskHandler<WallabagService>
    {
        public Type HandlerType { get; } = typeof(WallabagService);
        public string Command { get; } = "LinkdingBookmarkToWallabag";

        public async Task ProcessAsync(IEnumerable<WallabagItem> items, WallabagService destinationService,
            ILinkdingService linkdingService,
            ILogger logger, IConfiguration configuration)
        {
            var wallabagsNormalized = new Dictionary<int, string>();
            var updatedWallabags = new Dictionary<string, IEnumerable<string>>();
            var wallabagToRemove = new List<int>();
            var linkdingBookmarks = await linkdingService.GetAllBookmarksAsync();

            if (linkdingBookmarks != null && linkdingBookmarks.Count() > 0)
            {
                var settings = SettingsService.Settings;

                // var settingsString = JsonSerializer.Serialize(settings);
                // logger.LogInformation($"settings: {settingsString}");

                var tagName = configuration.GetValue<string>("Worker:SyncTag");

                linkdingBookmarks =
                    linkdingBookmarks.Where(x => x.TagNames.Contains(tagName)).OrderBy(x => x.DateAdded);

                Regex r = null;
                Match m = null;
                foreach (var bookmark in linkdingBookmarks)
                {
                    var cleanUrl =
                        bookmark.Url.Replace(
                            "?utm_source=share&utm_medium=android_app&utm_name=androidcss&utm_term=2&utm_content=share_button",
                            "");
                    // var existingElement = items.FirstOrDefault(x => x.Url.ToLower() == cleanUrl.ToLower());
                    var existingElement = items.FirstOrDefault(x =>
                        x.Url.ToLower() == cleanUrl.ToLower() || x.OriginUrl?.ToLower() == cleanUrl.ToLower());
                    if (existingElement == null)
                    {
                        var addToWallabag = true;
                        foreach (var p in settings.excludedDomains)
                        {
                            r = new Regex(p.pattern, RegexOptions.IgnoreCase);
                            m = r.Match(cleanUrl);

                            if (m.Success)
                            {
                                addToWallabag = false;
                                break;
                            }
                        }

                        if (addToWallabag && !updatedWallabags.ContainsKey(bookmark.Url))
                        {
                            updatedWallabags.Add(cleanUrl,
                                bookmark.TagNames.Where(x => !x.Equals(tagName, StringComparison.OrdinalIgnoreCase)));
                        }
                    }
                }
            }
            else
            {
                logger.LogInformation($"no bookmarks found");
            }

            if (updatedWallabags.Count() > 0)
            {
                logger.LogInformation($"Detected {updatedWallabags.Count()} bookmarks... Start syncing");

                foreach (var (url, tags) in updatedWallabags)
                {
                    var result = await destinationService.AddEntryByUrl(url, tags);

                    if (result.ReadingTime == 0)
                    {
                        wallabagToRemove.Add(result.Id);
                    }
                }

                logger.LogInformation($"{updatedWallabags.Count()} bookmarks synced");
            }
        }
    }
}