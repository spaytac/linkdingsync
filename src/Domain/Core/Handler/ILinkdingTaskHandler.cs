using System.Threading.Tasks;
using Core.Entities.Linkding;
using LinkdingUpdater.Handler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Handler
{
    public interface ILinkdingTaskHandler
    {
        string Command { get; }
        Task<HandlerResult> ProcessAsync(Bookmark bookmark, ILogger logger, IConfiguration configuration);
    }
}