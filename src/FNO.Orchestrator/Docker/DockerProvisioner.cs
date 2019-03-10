using FNO.Domain.Models;
using System;
using System.Threading.Tasks;

namespace FNO.Orchestrator.Docker
{
    internal class DockerProvisioner : IProvisioner
    {
        public Task<ProvisioningResult> ProvisionFactory(Factory factory)
        {
            throw new NotImplementedException();
        }
    }
}
