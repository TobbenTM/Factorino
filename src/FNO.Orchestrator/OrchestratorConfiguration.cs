using FNO.Common;

namespace FNO.Orchestrator
{
    public class OrchestratorConfiguration : ConfigurationBase
    {
        public ProvisionerConfiguration Provisioner { get; set; }
    }

    public class ProvisionerConfiguration
    {
        public string Provider { get; set; }

        public DockerConfiguration Docker { get; set; }
    }

    public class DockerConfiguration
    {
        public bool Native { get; set; }
    }
}
