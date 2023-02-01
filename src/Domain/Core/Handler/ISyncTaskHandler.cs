using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Wallabag;
using Linkding.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Handler
{
    public interface ISyncTaskHandler<T>
    {
        Type HandlerType { get; }
        string Command { get; }
        
        Task ProcessAsync(IEnumerable<WallabagItem> items, T destinationService, ILinkdingService linkdingService, ILogger logger, IConfiguration configuration);
    }
}