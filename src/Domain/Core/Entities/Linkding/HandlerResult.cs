using Core.Entities.Linkding;

namespace LinkdingUpdater.Handler
{
    public class HandlerResult
    {
        public bool PerformAction { get; set; } = false;
    
        public LinkdingItemAction Action { get; set; }
        public bool HasError { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public Bookmark Instance { get; set; }
    }
}

public enum LinkdingItemAction
{
    Update,
    Delete
}