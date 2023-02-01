using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoMapper;
using Core.Entities.Linkding;
using Linkding.Client.Options;
using Microsoft.Extensions.Options;

namespace Linkding.Client;

public class LinkdingService : ILinkdingService
{
    private readonly LinkdingSettings _settings;
    private readonly IMapper _mapper;
    public readonly HttpClient _client;

    public LinkdingService(HttpClient client, IOptions<LinkdingSettings> settings, IMapper mapper)
    {
        _settings = settings.Value;
        _client = client;
        _mapper = mapper;
        _client.BaseAddress = new Uri(_settings.Url);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _settings.Key);
    }

    private LinkdingService(string url, string key)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(url);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", key);
    }

    public async Task<IEnumerable<Bookmark>> GetBookmarksAsync(int limit = 100, int offset = 0)
    {
        var bookmarks = new List<Bookmark>();

        var result = await GetBookmarkResultsAsync(limit, offset);
        if (result != null && result.Results?.Count() > 0)
        {
            bookmarks = result.Results;
        }

        return bookmarks;
    }

    public async Task<IEnumerable<Bookmark>> GetAllBookmarksAsync()
    {
        IEnumerable<Bookmark> bookmarks = new List<Bookmark>();
        
        var result = await GetBookmarkResultsAsync();
        if (result != null && result.Results?.Count() > 0)
        {
            bookmarks = result.Results;
            if (result.Count > 100)
            {
                while (!string.IsNullOrEmpty(result.Next))
                {
                    result = await GetBookmarkResultsAsync(result.Next);
                    if (result.Results?.Count() > 0)
                    {
                        bookmarks = bookmarks.Concat(result.Results);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return bookmarks;
    }
    
    public async Task UpdateBookmarkCollectionAsync(IEnumerable<Bookmark> bookmarks)
    {
        foreach (var bookmark in bookmarks)
        {
            var payload = _mapper.Map<BookmarkUpdatePayload>(bookmark);
            await UpdateBookmarkAsync(payload);
        }
    }

    public async Task UpdateBookmarkCollectionAsync(IEnumerable<BookmarkUpdatePayload> bookmarks)
    {
        foreach (var bookmark in bookmarks)
        {
            await UpdateBookmarkAsync(bookmark);
        }
    }

    public async Task UpdateBookmarkAsync(BookmarkUpdatePayload bookmark)
    {
        var result = await _client.PutAsJsonAsync($"/api/bookmarks/{bookmark.Id}/", bookmark);
        if (result.IsSuccessStatusCode)
        {
                
        }
        else
        {
                
        }
    }

    public async Task<BookmarksResult> GetBookmarkResultsAsync(int limit = 100, int offset = 0)
    {
        BookmarksResult bookmarkResult = null;

        var url = $"/api/bookmarks/";

        bookmarkResult = await GetBookmarkResultsAsync(url);

        return bookmarkResult;
    }

    public async Task<BookmarksResult> GetBookmarkResultsAsync(string url)
    {
        BookmarksResult bookmarkResult = null;

        bookmarkResult = await _client.GetFromJsonAsync<BookmarksResult>(url);

        return bookmarkResult;
    }

    public static LinkdingService Create(string url, string key)
    {
        return new LinkdingService(url, key);
    }
}