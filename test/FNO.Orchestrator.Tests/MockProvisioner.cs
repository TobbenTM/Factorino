using FNO.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.Orchestrator.Tests
{
    internal class MockProvisioner : IProvisioner
    {
        public List<Factory> FactoriesProvisioned { get; } = new List<Factory>();

        public Task<ProvisioningResult> ProvisionFactory(Factory factory)
        {
            FactoriesProvisioned.Add(factory);
            return Task.FromResult(new ProvisioningResult());
        }
    }
}
