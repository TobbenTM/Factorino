using FNO.Domain.Models;
using System;
using System.Threading.Tasks;

namespace FNO.Orchestrator.K8
{
    internal class K8Provisioner : IProvisioner
    {
        public Task<ProvisioningResult> ProvisionFactory(Factory factory)
        {
            throw new NotImplementedException();
        }
    }
}
