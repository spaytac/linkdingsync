using Core.Entities.Linkding;

namespace Linkding.Client.Extensions;

public static class BookmarkExtensions
{
    public static BookmarkCreatePayload MapToCreatePayload(this Bookmark bookmark)
    {
        var payload = new BookmarkCreatePayload();
        payload.Title = bookmark.Title;
        payload.Description = bookmark.Description;
        payload.Url = bookmark.Url;
        payload.Unread = bookmark.Unread;
        payload.IsArchived = payload.IsArchived;
        
        foreach (var tagName in bookmark.TagNames)
        {
            payload.TagNames = payload.TagNames.Add(tagName);
        }

        return payload;
    }
    
    public static BookmarkUpdatePayload MapToUpdatePayload(this Bookmark bookmark)
    {
        var payload = new BookmarkUpdatePayload();
        payload.Title = bookmark.Title;
        payload.Description = bookmark.Description;
        payload.Url = bookmark.Url;
        
        foreach (var tagName in bookmark.TagNames)
        {
            payload.TagNames = payload.TagNames.Add(tagName);
        }
        
        return payload;
    }
}