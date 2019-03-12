using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using Serilog;
using System;
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
            var factoriesToProvision = state.Factories
                .Where(f => f.State == FactoryState.Creating)
                .ToList();
            _logger.Information($"Provisioning {factoriesToProvision.Count()} factories..");
            foreach (var factory in factoriesToProvision)
            {
                try
                {
                    var result = await _provisioner.ProvisionFactory(factory);
                    var evnt = new FactoryProvisionedEvent(factory.FactoryId, null)
                    {
                        ResourceId = result.ResourceId,
                        Port = result.Port,
                        Host = result.Host,
                    };
                    factory.State = FactoryState.Starting;
                    _logger.Information($"Successfully provisioned resources for factory {factory.FactoryId}, producing event..");
                    changeSet.Add(evnt);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Could not provision resources for factory {factory.FactoryId}!");
                }
            }

            return changeSet;
        }
    }
}
