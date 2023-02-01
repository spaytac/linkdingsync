using System.Collections.Generic;

namespace Core.Entities.Linkding
{
    public class BookmarksResult
    {
        public long Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
        public List<Bookmark?> Results { get; set; }
    }
}