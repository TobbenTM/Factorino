using FNO.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FNO.FactoryPod
{
    internal class FactoryPodConfiguration : ConfigurationBase
    {
        public FactorinoConfiguration Factorino { get; set; }
        public FactorioConfiguration Factorio { get; set; }
    }

    internal class FactorinoConfiguration
    {
        public Guid FactoryId{ get; set; }
        public string Seed { get; set; }
        public string SavePath { get; set; }
        public string DataPath { get; set; }
        public string SaveFile { get; set; }

        public string FactoryDirectoryPath => Path.GetFullPath(Path.Combine(SavePath, FactoryId.ToString()));
        public string FactorySavePath => Path.GetFullPath(Path.Combine(FactoryDirectoryPath, SaveFile));
    }

    internal class FactorioConfiguration
    {
        public string ServerPath { get; set; }
        public string[] Args { get; set; }
        public RconSettings Rcon { get; set; }
        public bool UseShell { get; set; }
        public string ModsPath { get; set; }
        public string StorePath { get; set; }
        public string SettingsFile { get; set; }
        public string ConfigFile { get; set; }

        public string SettingsFilePath => Path.GetFullPath(Path.Combine(StorePath, SettingsFile));
        public string ConfigFilePath => Path.GetFullPath(Path.Combine(StorePath, ConfigFile));
        public FactorioArguments Arguments => new FactorioArguments(Args);
    }

    internal class FactorioArguments : IEnumerable
    {
        private readonly List<string> _arguments;

        public FactorioArguments(string[] args)
        {
            _arguments = new List<string>(args);
        }

        public FactorioArguments WithSaveFile(string saveFile)
        {
            _arguments.AddRange(new string[]{
                "--start-server",
                saveFile
            });
            return this;
        }

        public FactorioArguments WithServerSettings(string settingsFile)
        {
            _arguments.AddRange(new string[]{
                "--server-settings",
                settingsFile
            });
            return this;
        }

        public FactorioArguments WithFactorioConfig(string configFile)
        {
            _arguments.AddRange(new string[]
            {
                "--config",
                configFile
            });
            return this;
        }

        public FactorioArguments WithModsDirectory(string modsPath)
        {
            _arguments.AddRange(new string[]{
                "--mod-directory",
                modsPath
            });
            return this;
        }

        public FactorioArguments WithRconSettings(RconSettings settings)
        {
            _arguments.AddRange(new string[]{
                "--rcon-port",
                settings.Port.ToString(),
                "--rcon-password",
                settings.Password
            });
            return this;
        }

        public IEnumerator GetEnumerator() => _arguments.GetEnumerator();
        public override string ToString() => string.Join(" ", _arguments);
    }

    internal class RconSettings
    {
        public int Port { get; set; }
        public string Password { get; set; }
    }
}
