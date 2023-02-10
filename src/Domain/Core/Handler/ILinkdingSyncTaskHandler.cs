using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Linkding;
using Linkding.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Handler
{
    public interface ILinkdingSyncTaskHandler
    {
        string Command { get; }
        Task ProcessAsync(IEnumerable<Bookmark> bookmarks, ILinkdingService linkdingService, ILogger logger, IConfiguration configuration);
    }
}