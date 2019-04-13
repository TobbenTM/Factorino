using System;
using System.Threading.Tasks;
using FNO.Domain;
using FNO.Domain.Events;
using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FNO.ReadModel.Tests.EventHandlers
{
    public abstract class EventHandlerTestBase : IDisposable
    {
        private readonly DbContextOptions<ReadModelDbContext> _dbOpts;
        protected readonly ILogger _logger;

        protected readonly Guid _playerId = Guid.NewGuid();
        protected readonly Guid _factoryId = Guid.NewGuid();
        protected readonly Guid _locationId = Guid.NewGuid();

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

        protected void Given(params object[][] entities)
        {
            using (var dbContext = GetInMemoryDatabase())
            {
                foreach (var set in entities)
                {
                    foreach (var entity in set)
                    {
                        dbContext.Add(entity);
                    }
                }
                dbContext.SaveChanges();
            }
        }

        protected Player[] DefaultPlayer => new[] { new Player { PlayerId = _playerId } };


        protected object[] DefaultFactory => new object[]
        {
            new FactoryLocation { LocationId = _locationId },
            new Factory { FactoryId = _factoryId },
        };

        public void Dispose()
        {
        }
    }
}
