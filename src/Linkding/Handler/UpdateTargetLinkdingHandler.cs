using Core.Entities.Linkding;
using Core.Handler;
using Linkding.Client;
using Linkding.Client.Extensions;

namespace Linkding.Handler;

public class UpdateTargetLinkdingHandler : ILinkdingSyncTaskHandler
{
    public string Command { get; } = "UpdateTargetLinkding";

    public async Task ProcessAsync(IEnumerable<Bookmark> bookmarks, ILinkdingService linkdingService, ILogger logger,
        IConfiguration configuration)
    {
        var linkdingBookmarks = await linkdingService.GetAllBookmarksAsync();
        var addedBookmarks = new List<BookmarkCreatePayload>();
        var updatedBookmarks = new List<Bookmark>();

        if (linkdingBookmarks.Count() > 0)
        {
            
            foreach (var bookmark in bookmarks)
            {
                var linkdingBookmark = linkdingBookmarks.FirstOrDefault(x => x.Url == bookmark.Url);
                if (linkdingBookmark == null)
                {
                    addedBookmarks.Add(bookmark.MapToCreatePayload());
                }
                else
                {
                    bookmark.Id = linkdingBookmark.Id;
                    bookmark.Title = !string.IsNullOrEmpty(bookmark.Title) ? bookmark.Title.Trim() : bookmark.WebsiteTitle;
                    bookmark.Description = !string.IsNullOrEmpty(bookmark.Description) ? bookmark.Description.Trim() : bookmark.WebsiteDescription;

                    if (!string.IsNullOrEmpty(bookmark.Title) && !string.IsNullOrEmpty(bookmark.Description) &&
                        (!linkdingBookmark.Title.Equals(bookmark.Title.Trim(), StringComparison.OrdinalIgnoreCase) || 
                        !linkdingBookmark.Description.Equals(bookmark.Description.Trim(), StringComparison.OrdinalIgnoreCase) ||
                        linkdingBookmark.TagNames.Count() != bookmark.TagNames.Count()))
                    {
                        updatedBookmarks.Add(bookmark);
                    }
                    else
                    {
                        var difference = linkdingBookmark.TagNames.Where(t =>
                            !bookmark.TagNames.Any(b => b.Equals(t, StringComparison.OrdinalIgnoreCase)));

                        if (difference.Count() > 0)
                        {
                            updatedBookmarks.Add(bookmark);
                        }
                    }
                }
            }
        }
        else
        {
            foreach (var bookmark in linkdingBookmarks)
            {
                addedBookmarks.Add(bookmark.MapToCreatePayload());
            }
        }
        
        if (updatedBookmarks.Count > 0)
        {
            await linkdingService.UpdateBookmarkCollectionAsync(updatedBookmarks);
        }

        if (addedBookmarks.Count > 0)
        {
            await linkdingService.AddBookmarkCollectionAsync(addedBookmarks);
        }
    }
}