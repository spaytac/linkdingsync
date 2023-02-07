using System.Net.Http.Json;
using Core.Entities.Wallabag;
using Wallabag.Client.Models;

namespace Wallabag.Client;

public partial class WallabagService
{
    public async Task<IEnumerable<WallabagItem>> GetEntries(string format = "json", int limit = 50, bool full = false)
    {
        var bookmarks = new List<WallabagItem>();
        var url = $"/api/entries.{format}?perPage={limit}";
        if (!full)
        {
            url = $"{url}&detail=metadata";
        }
        
        var allQuery = await GetJsonAsync<WallabagQuery>(url);

        if (allQuery != null && allQuery.Embedded != null && allQuery.Embedded.Items != null && allQuery.Embedded.Items.Count() > 0)
        {
            bookmarks = allQuery.Embedded.Items;
            
            if (allQuery.Total > limit)
            {
                while (allQuery.QueryLinks.Next != null && !string.IsNullOrEmpty(allQuery.QueryLinks.Next.Href))
                {
                    // url = allQuery.QueryLinks.Next.Href.Replace(_settings.Url, "");
                    url = allQuery.QueryLinks.Next.Href;
                    if (_client.BaseAddress.Scheme == "https")
                    {
                        url = allQuery.QueryLinks.Next.Href.Replace("http://", "https://");
                    }

                    allQuery = await GetJsonAsync<WallabagQuery>(url);
                    bookmarks.AddRange(allQuery.Embedded.Items);
                }
            }
        }

        return bookmarks;
    }

    public async Task<WallabagItem> GetEntryById(int id, string format = "json")
    {
        var url = $"/api/entries/{id}.{format}";
        var item = await GetJsonAsync<WallabagItem>(url);

        return item;
    }

    public async Task<WallabagItem> AddEntryByUrl(string url, IEnumerable<string> tags = null, string format = "json")
    {
        var endpoint = $"/api/entries.{format}";

        var keyVals = new Dictionary<string, string>();
        keyVals.Add("url", url);
        if (tags != null && tags.Count() > 0)
        {
            keyVals.Add("tags", string.Join(",", tags));
        }
        
        var content = new FormUrlEncodedContent(keyVals);
        var response = await PostWithFormDataAsnyc(endpoint, content);

        var item = await response.Content.ReadFromJsonAsync<WallabagItem>();
        return item;
    }
    
    // private async Task<WallabagEntry> GetBookmarkResultsAsync(int limit = 100, int offset = 0)
    // {
    //     WallabagEntry bookmarkResult = null;
    //
    //     var url = $"/api/bookmarks/";
    //
    //     bookmarkResult = await GetBookmarkResultsAsync(url);
    //
    //     return bookmarkResult;
    // }
    //
    // private async Task<WallabagEntry> GetBookmarkResultsAsync(string url)
    // {
    //     WallabagEntry bookmarkResult = null;
    //
    //     bookmarkResult = await _client.GetFromJsonAsync<WallabagEntry>(url);
    //
    //     return bookmarkResult;
    // }

}