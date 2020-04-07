using Docker.DotNet;
using Docker.DotNet.Models;
using FNO.Domain.Models;
using FNO.Orchestrator.Exceptions;
using FNO.Orchestrator.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.Orchestrator.Docker
{
    internal class DockerProvisioner : IProvisioner
    {
        private static string FACTORIO_PORT => "34197/udp";

        private readonly ILogger _logger;
        private readonly DockerClient _client;

        public DockerProvisioner(DockerConfiguration configuration, ILogger logger)
        {
            _logger = logger;

            if (configuration.Native)
            {
                var socket = Environment.OSVersion.Platform == PlatformID.Win32NT ?
                    "npipe://./pipe/docker_engine" : "unix:///tmp/docker.sock";
                _logger.Information($"Using the native Docker provisioner with socket/pipe '{socket}'");
                _client = new DockerClientConfiguration(new Uri(socket))
                    .CreateClient();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<ProvisioningResult> ProvisionFactory(Factory factory)
        {
            _logger.Information($"Provisioning factory {factory.FactoryId}...");

            var options = new CreateContainerParameters
            {
                Image = "factorino/factory-pod",
                Name = $"factory-pod-{factory.FactoryId}",
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    { FACTORIO_PORT, new EmptyStruct() },
                },
                Env = new List<string>
                {
                    $"factorino__factoryId={factory.FactoryId}",
                    $"factorino__ownerId={factory.OwnerId}",
                    $"factorino__ownerUsername={factory.OwnerFactorioUsername}",
                    $"factorino__deedId={factory.DeedId}",
                },
                HostConfig = new HostConfig
                {
                    PublishAllPorts = true,
                    NetworkMode = "factorino",
                },
            };
            var response = await _client.Containers.CreateContainerAsync(options);

            await _client.Containers.StartContainerAsync(response.ID, new ContainerStartParameters());
            var instance = await _client.Containers.InspectContainerAsync(response.ID);

            return new ProvisioningResult
            {
                ResourceId = response.ID,
                Port = int.Parse(instance.NetworkSettings.Ports[FACTORIO_PORT].Single().HostPort),
                Host = instance.NetworkSettings.IPAddress,
            };
        }

        public async Task DecommissionFactory(Factory factory)
        {
            _logger.Information($"Decommissioning factory {factory.FactoryId}...");
            ValidateFactoryForDecommision(factory);

            var decommissioned = await _client.Containers.StopContainerAsync(factory.ResourceId, new ContainerStopParameters());

            if (decommissioned)
            {
                await _client.Containers.RemoveContainerAsync(factory.ResourceId, new ContainerRemoveParameters());
            }
            else
            {
                throw new UnableToDecommissionException($"The Docker client was unable to stop factory pod {factory.ResourceId}!");
            }
        }

        private void ValidateFactoryForDecommision(Factory factory)
        {
            if (string.IsNullOrEmpty(factory.ResourceId))
            {
                throw new ArgumentException($"Factory {factory.FactoryId} does not have a resource id attached!");
            }
        }
    }
}
