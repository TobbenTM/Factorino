using Serilog;

namespace FNO.EventSourcing
{
    public class EventHandlerBase : IHandler
    {
        protected ILogger _logger;

        protected EventHandlerBase(ILogger logger)
        {
            _logger = logger;
        }
    }
}
