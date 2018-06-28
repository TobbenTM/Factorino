using Newtonsoft.Json;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace FNO.FactoryPod
{
    /// <summary>
    /// Responsible for starting and managing the Factorio server executable
    /// </summary>
    internal class Daemon : IDisposable
    {
        private readonly FactoryPodConfiguration _configuration;
        private readonly ILogger _logger;

        private readonly Process _factorioProc;

        internal event EventHandler OnRconReady;
        internal event DataReceivedEventHandler OnOutputData;
        internal event DataReceivedEventHandler OnErrorData;

        internal Daemon(FactoryPodConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;

            _factorioProc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _configuration.Factorio.ServerPath,
                    Arguments = _configuration.Factorio.Arguments
                        .WithSaveFile(_configuration.Factorino.FactorySavePath)
                        .WithServerSettings(_configuration.Factorio.SettingsFilePath)
                        .WithModsDirectory(_configuration.Factorio.ModsPath)
                        .WithRconSettings(_configuration.Factorio.Rcon)
                        .ToString(),
                    UseShellExecute = _configuration.Factorio.UseShell,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                },
                EnableRaisingEvents = true,
            };

            _factorioProc.OutputDataReceived += new DataReceivedEventHandler(FactorioOutputHandler);
            _factorioProc.ErrorDataReceived += new DataReceivedEventHandler(FactorioErrorHandler);
            _factorioProc.Exited += new EventHandler(FactorioProcessHandler);
        }

        private void FactorioProcessHandler(object sender, EventArgs e)
        {
            _logger.Warning($"Factorio process exited with message: {e}");
            Dispose();
            Environment.Exit(1);
        }

        private void FactorioOutputHandler(object sender, DataReceivedEventArgs e)
        {
            _logger.Information(e.Data);

            if (e.Data.Contains("Starting RCON interface"))
            {
                OnRconReady?.Invoke(this, e);
            }

            OnOutputData?.Invoke(this, e);
        }

        private void FactorioErrorHandler(object sender, DataReceivedEventArgs e)
        {
            _logger.Error(e.Data);

            OnErrorData?.Invoke(this, e);
        }

        /// <summary>
        /// Server configuration can be recreated if needed (if the pod is 
        /// recreated or something), because nothing should change..
        /// </summary>
        internal void EnsureServerConfiguration()
        {
            var settingsPath = _configuration.Factorio.SettingsFilePath;
            if (!File.Exists(settingsPath))
            {
                _logger.Information($"Factorio server configuration not found, creating at {settingsPath}...");

                var settings = new FactorioServerConfiguration();
                var content = JsonConvert.SerializeObject(settings, Formatting.Indented);

                using (var sr = new StringReader(content))
                using (var sw = File.CreateText(Path.GetFullPath(settingsPath)))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sw.WriteLine(line);
                    }
                }

                _logger.Information($"Successfully created Factorio server configuration at {settingsPath}!");
            }
        }

        /// <summary>
        /// Save files needs to be stored on a distributed volume to ensure persistency
        /// </summary>
        internal void EnsureInitialSave()
        {
            var instanceDirectory = _configuration.Factorino.FactoryDirectoryPath;
            if (!Directory.Exists(instanceDirectory))
            {
                _logger.Information($"Instance save directory not found, creating {instanceDirectory}..");

                Directory.CreateDirectory(instanceDirectory);
            }

            var destFile = _configuration.Factorino.FactorySavePath;
            if (!File.Exists(destFile))
            {
                _logger.Information($"Factorio seed file not found, importing to {destFile} from {_configuration.Factorino.SeedFilePath}...");

                File.Copy(_configuration.Factorino.SeedFilePath, destFile);

                _logger.Information($"Successfully imported seed {_configuration.Factorino.Seed} into {destFile}!");
            }
        }

        /// <summary>
        /// Ensure the save mod is installed, using an appropriate version for the pod
        /// </summary>
        internal void EnsureModsInstalled()
        {
            var destFile = _configuration.Factorino.FactorySavePath;
            if (!File.Exists(destFile))
            {
                throw new FileNotFoundException("Could not find save file!");
            }

            using (var saveFile = ZipFile.Open(destFile, ZipArchiveMode.Update))
            {
                var modName = "factorino.lua";
                _logger.Information($"Installing mods: {modName}..");

                // Remove any existing ones
                if (saveFile.Entries.Any(e => e.Name.Equals(modName)))
                {
                    _logger.Information($"Mod {modName} already exists in save, replacing..");
                    saveFile.Entries.First(e => e.Name.Equals(modName)).Delete();
                }

                // Get save directory
                var dir = saveFile.Entries.First().FullName.Split('/')[0];

                // Import mods here
                saveFile.CreateEntryFromFile(modName,  $"{dir}/{modName}");
                _logger.Information($"Installed mods: {modName}!");
            }
        }

        internal void Run()
        {
            // Start factorio server
            _logger.Information($"Starting factorio server..");
            _factorioProc.Start();
            _factorioProc.BeginOutputReadLine();
            _factorioProc.BeginErrorReadLine();
            _logger.Information($"Factorio server [{_factorioProc?.Id}] started!");
        }

        public void Dispose()
        {
            try
            {
                _logger.Information($"Killing factorio process [{_factorioProc?.Id}]..");
                _factorioProc.Kill();
                _factorioProc.CancelOutputRead();
                _factorioProc.CancelErrorRead();
            }
            catch (Exception e)
            {
                _logger.Error($"Could not kill factorio process ({e.Message})");
            }
        }
    }
}
