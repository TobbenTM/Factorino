using FNO.Domain;
using FNO.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Threading.Tasks;

namespace FNO.ReadModel.Tests.EventHandlers
{
    public class EventHandlerTestBase : IDisposable
    {
        private readonly DbContextOptions<ReadModelDbContext> _dbOpts;
        protected readonly ILogger _logger;

        public EventHandlerTestBase()
        {
            _logger = new LoggerConfiguration().CreateLogger();

            _dbOpts = new DbContextOptionsBuilder<ReadModelDbContext>()
                .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
                .Options;
        }

        protected ReadModelDbContext GetInMemoryDatabase()
        {
            return new ReadModelDbContext(_dbOpts);
        }

        protected async Task When<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            using (var dbContext = GetInMemoryDatabase())
            {
                var dispatcher = new EventDispatcher(dbContext, _logger);
                await dispatcher.Handle(evnt);
                dbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
        }
    }
}
