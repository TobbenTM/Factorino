using FNO.Domain.Models;
using System.Threading.Tasks;

namespace FNO.Orchestrator
{
    public interface IProvisioner
    {
        Task ProvisionFactory(Factory factory);
    }
}
