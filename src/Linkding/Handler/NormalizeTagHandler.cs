using Core.Entities.Linkding;
using Core.Handler;
using LinkdingUpdater.Handler;

namespace Linkding.Handler;

public class NormalizeTagHandler : ILinkdingTaskHandler
{
    public string Command { get; } = "NormalizeTag";

    public async Task<HandlerResult> ProcessAsync(Bookmark bookmark, ILogger logger, IConfiguration configuration)
    {
        var returnValue = new HandlerResult() {Instance = bookmark};
        var update = false;

        var maxTagLength = configuration.GetValue<int>("Worker:TagNameLength");

        var normalizedTagnames = new List<string>();
        foreach (var tagName in returnValue.Instance.TagNames)
        {
            if (tagName.Length > maxTagLength)
            {
                var normalizeTag = tagName.NormalizeTag();
                normalizedTagnames.Add(normalizeTag);
                update = true;
            }
            else
            {
                normalizedTagnames.Add(tagName);
            }
        }
        
        if (update)
        {
            logger.LogInformation($"Start updating bookmark {returnValue.Instance.WebsiteTitle} - {returnValue.Instance.Id}");
            returnValue.Instance.TagNames = normalizedTagnames;
            returnValue.PerformAction = true;
            returnValue.Action = LinkdingItemAction.Update;
        }

        return returnValue;
    }
}