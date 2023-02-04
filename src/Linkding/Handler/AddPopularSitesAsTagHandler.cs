using System.Text.Json;
using System.Text.RegularExpressions;
using Core.Abstraction;
using Core.Entities.Linkding;
using Core.Handler;
using Linkding.Settings;
using LinkdingUpdater.Handler;

namespace Linkding.Handler;

public class AddPopularSitesAsTagHandler : ILinkdingTaskHandler
{
    public string Command { get; } = "AddPopularSitesAsTag";
    public async Task<HandlerResult> ProcessAsync(Bookmark bookmark, ILogger logger, IConfiguration configuration)
    {
        var settings = SettingsService.Settings;
        
        var returnValue = new HandlerResult() {Instance = bookmark};
        Regex r = null;
        Match m = null;
        foreach (var regexEntry in settings.taggingRule)
        {
            try
            {
                r = new Regex(regexEntry.pattern, RegexOptions.IgnoreCase);
                m = r.Match(returnValue.Instance.Url);
                if (m.Success)
                {
                    var tagsCommaSeparated = r.Replace(returnValue.Instance.Url, regexEntry.replace);
                    if (!string.IsNullOrEmpty(tagsCommaSeparated))
                    {
                        var tags = tagsCommaSeparated.Split(',');
                        foreach (var tag in tags)
                        {
                            var normalizeTag = tag.NormalizeTag();
                            if (!string.IsNullOrEmpty(normalizeTag) && !returnValue.Instance.TagNames.Contains(normalizeTag) &&
                                returnValue.Instance.TagNames.FirstOrDefault(x => x.ToLower() == normalizeTag.ToLower()) == null)
                            {
                                
                                returnValue.Instance.TagNames = returnValue.Instance.TagNames.Add(normalizeTag);
                                returnValue.PerformAction = true;
                                returnValue.Action = LinkdingItemAction.Update;
                            }
                        }
                    }
                }
            }
            finally
            {
                r = null;
                m = null;
            }
            
        }
        
        
        foreach (var urlKeyValue in settings.urlTagMapping)
        {
            if (returnValue.Instance.Url.ToLower().StartsWith(urlKeyValue.url.ToLower()) && returnValue.Instance.TagNames.FirstOrDefault(x => x.ToLower() == urlKeyValue.name.ToLower()) == null)
            {
                returnValue.Instance.TagNames = returnValue.Instance.TagNames.Add(urlKeyValue.name);
                
                returnValue.PerformAction = true;
            }
        }


        return returnValue;
    }
}
