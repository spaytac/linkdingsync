using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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

    public async Task AddBookmarkCollectionAsync(IEnumerable<Bookmark> bookmarks)
    {
        foreach (var bookmark in bookmarks)
        {
            var payload = _mapper.Map<BookmarkCreatePayload>(bookmark);
            await AddBookmarkAsync(payload);
        }
    }
    
    public async Task AddBookmarkCollectionAsync(IEnumerable<BookmarkCreatePayload> bookmarks)
    {
        foreach (var bookmark in bookmarks)
        {
            await AddBookmarkAsync(bookmark);
        }
    }
    
    public async Task AddBookmarkAsync(BookmarkCreatePayload bookmark)
    {
        var content = JsonSerializer.Serialize(bookmark);
        var requestContent = new StringContent(content, Encoding.UTF8, "application/json");
        var uri = $"/api/bookmarks/";

        try
        {
            var response = await _client.PostAsync(uri, requestContent);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // throw;
        }
        
        var result = await _client.PostAsJsonAsync($"/api/bookmarks/", bookmark);
        if (result.IsSuccessStatusCode)
        {
                
        }
        else
        {
                
        }
    }

    public async Task UpdateBookmarkCollectionAsync(IEnumerable<Bookmark> bookmarks)
    {
        foreach (var bookmark in bookmarks)
        {
            try
            {
                var payload = _mapper.Map<Bookmark, BookmarkUpdatePayload>(bookmark);
                await UpdateBookmarkAsync(bookmark.Id, payload);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // throw;
            }
        }
    }
    
    public async Task UpdateBookmarkAsync(int id, BookmarkUpdatePayload bookmark)
    {
        var content = JsonSerializer.Serialize(bookmark);
        var requestContent = new StringContent(content, Encoding.UTF8, "application/json");
        var uri = $"/api/bookmarks/{id}/";// Path.Combine("api/bookmarks", $"{id}");

        try
        {
            var response = await _client.PutAsync(uri, requestContent);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // throw;
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