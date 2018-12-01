using FNO.Domain;
using FNO.Domain.Models;
using FNO.EventSourcing.Exceptions;
using FNO.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.WebApp.Filters
{
    public class EnsureConsumerConsistencyAttribute : TypeFilterAttribute
    {
        public EnsureConsumerConsistencyAttribute() : base(typeof(EnsureConsumerConsistencyFilter))
        {
        }

        private class EnsureConsumerConsistencyFilter : IAsyncResultFilter
        {
            private readonly ILogger _logger;
            private readonly ReadModelDbContext _dbContext;

            public EnsureConsumerConsistencyFilter(ILogger logger, ReadModelDbContext dbContext)
            {
                _logger = logger;
                _dbContext = dbContext;
            }

            public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
            {
                var response = await next();
                if (response.Result is ObjectResult obj)
                {
                    if (obj.Value is IEventResult result)
                    {
                        var desiredState = result.Results
                            .GroupBy(r => new { r.Topic, r.Partition })
                            .Select(g => new EventMetadata{
                                Topic= g.Key.Topic,
                                Partition= g.Key.Partition,
                                Offset = g.Max(r => r.Offset),
                            })
                            .ToList();
                        _logger.Debug($"Ensuring consumer consistency, need state {string.Join(", ", desiredState)}");

                        var tries = 0;

                        // We need to wait for consumer to catch up
                        while (desiredState.Any(s => s.Offset > GetState(s).Offset))
                        {
                            if (tries >= 20)
                            {
                                throw new ConsumerOutOfSyncException($"Could not reach desired state: {string.Join(", ", desiredState)}!");
                            }

                            _logger.Debug($"Consumer lagging, waiting {tries} tries so far..");
                            await Task.Delay(200);
                            tries += 1;
                        }
                    }
                    else
                    {
                        _logger.Warning($"Consumer consistency filter used on an action that doesn't return CreatedEntityResult! {{{response.ActionDescriptor.DisplayName}}}");
                    }
                }
                else
                {
                    _logger.Warning($"Consumer consistency filter used on an action that doesn't return CreatedEntityResult! {{{response.ActionDescriptor.DisplayName}}}");
                }
            }

            private ConsumerState GetState(EventMetadata target)
            {
                return _dbContext.ConsumerStates
                    .FirstOrDefault(s => s.Topic == target.Topic && s.Partition == target.Partition)
                    ?? new ConsumerState();
            }
        }
    }
}
