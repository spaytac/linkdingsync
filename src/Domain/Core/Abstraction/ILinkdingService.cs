using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Linkding;

namespace Linkding.Client
{
    public interface ILinkdingService
    {
        Task<IEnumerable<Bookmark>> GetBookmarksAsync(int limit = 100, int offset = 0);
        Task<IEnumerable<Bookmark>> GetAllBookmarksAsync();
        Task AddBookmarkCollectionAsync(IEnumerable<Bookmark> bookmarks);
        Task AddBookmarkCollectionAsync(IEnumerable<BookmarkCreatePayload> bookmarks);
        Task AddBookmarkAsync(BookmarkCreatePayload bookmark);
        
        Task UpdateBookmarkCollectionAsync(IEnumerable<Bookmark> bookmarks);
        Task UpdateBookmarkCollectionAsync(IEnumerable<BookmarkUpdatePayload> bookmarks);
        Task UpdateBookmarkAsync(BookmarkUpdatePayload bookmark);
        Task<BookmarksResult> GetBookmarkResultsAsync(int limit = 100, int offset = 0);
        Task<BookmarksResult> GetBookmarkResultsAsync(string url);
    }
}