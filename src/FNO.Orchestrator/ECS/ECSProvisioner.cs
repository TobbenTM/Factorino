﻿using FNO.Domain.Models;
using FNO.Orchestrator.Models;
using System;
using System.Threading.Tasks;

namespace FNO.Orchestrator.ECS
{
    internal class ECSProvisioner : IProvisioner
    {
        public Task<ProvisioningResult> ProvisionFactory(Factory factory)
        {
            throw new NotImplementedException();
        }

        public Task DecommissionFactory(Factory factory)
        {
            throw new NotImplementedException();
        }
    }
}
