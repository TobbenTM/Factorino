using FNO.Domain.Models;
using System.Threading.Tasks;

namespace FNO.Orchestrator
{
    public interface IProvisioner
    {
        Task<ProvisioningResult> ProvisionFactory(Factory factory);
    }
}
