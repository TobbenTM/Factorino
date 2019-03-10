using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.Orchestrator
{
    public class Evaluator
    {
        private readonly IProvisioner _provisioner;
        private readonly ILogger _logger;

        public Evaluator(IProvisioner provisioner, ILogger logger)
        {
            _provisioner = provisioner;
            _logger = logger;
        }

        public async Task<IEnumerable<IEvent>> Evaluate(State state)
        {
            var changeSet = new List<IEvent>();

            // For all factories in a creation state, we need to provision resources
            foreach (var factory in state.Factories.Where(f => f.State == FactoryState.Creating))
            {
                var result = await _provisioner.ProvisionFactory(factory);
                var evnt = new FactoryProvisionedEvent(factory.FactoryId, null);
                changeSet.Add(evnt);
            }

            return changeSet;
        }
    }
}
