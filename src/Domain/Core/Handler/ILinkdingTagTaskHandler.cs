using System.Threading.Tasks;
using Core.Entities.Linkding;
using LinkdingUpdater.Handler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Handler
{
    public interface ILinkdingTagTaskHandler
    {
        string Command { get; }
        Task<HandlerResult> ProcessAsync(Tag tag, ILogger logger, IConfiguration configuration);
    }
}