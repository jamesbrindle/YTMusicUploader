using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace YTMusicUploader.Helpers
{
    /// <summary>
    /// Checks if the YT Music Uploader application is installed and if so what version (used for auto update)
    /// </summary>
    public static class InstalledApplicationHelper
    {
        /// <summary>
        /// Checks if the YT Music Uploader application is installed and if so what version (used for auto update)
        /// </summary>
        public class InstalledProgram
        {
            public enum PlatFormType
            {
                X86,
                X64
            }

            public PlatFormType Plaform { get; set; }
            public string DisplayName { get; set; }
            public string Version { get; set; }
            public string InstalledDate { get; set; }
            public string Publisher { get; set; }
            public string UnninstallCommand { get; set; }
            public string ModifyPath { get; set; }
        }

        /// <summary>
        /// Retrieves all installed programs as from the registry
        /// </summary>
        public static List<InstalledProgram> GetInstalledPrograms()
        {
            var installedPrograms = new List<InstalledProgram>();
            installedPrograms.AddRange(ReadRegistryUninstall(RegistryView.Registry32));
            installedPrograms.AddRange(ReadRegistryUninstall(RegistryView.Registry64));

            return installedPrograms;
        }

        private static List<InstalledProgram> ReadRegistryUninstall(RegistryView view)
        {
            var installedPrograms = new List<InstalledProgram>();
            const string REGISTRY_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view))
            {
                using (var subKey = baseKey.OpenSubKey(REGISTRY_KEY))
                {
                    foreach (string subkey_name in subKey.GetSubKeyNames())
                    {
                        using (var key = subKey.OpenSubKey(subkey_name))
                        {
                            if (!string.IsNullOrEmpty(key.GetValue("DisplayName") as string))
                            {
                                installedPrograms.Add(new InstalledProgram
                                {
                                    Plaform = view == RegistryView.Registry32
                                                            ? InstalledProgram.PlatFormType.X86
                                                            : InstalledProgram.PlatFormType.X64,

                                    DisplayName = (string)key.GetValue("DisplayName"),
                                    Version = (string)key.GetValue("DisplayVersion"),
                                    InstalledDate = (string)key.GetValue("InstallDate"),
                                    Publisher = (string)key.GetValue("Publisher"),
                                    UnninstallCommand = (string)key.GetValue("UninstallString"),
                                });
                            }
                            key.Close();
                        }
                    }
                    subKey.Close();
                }

                baseKey.Close();
            }

            return installedPrograms;
        }

        /// <summary>
        /// Gets the platform of the installed YT Music Uploader application as from the registry
        /// </summary>
        public static string GetInstalledPlatform()
        {
            try
            {
                var installedApps = GetInstalledPrograms();
                string plaform = installedApps.Where(a => a.DisplayName.ToLower().Contains("yt music uploader")).FirstOrDefault().Plaform.ToString().ToLower();

                if (string.IsNullOrEmpty(plaform))
                    return "x64";

                return plaform;
            }
            catch
            {
                return "x64";
            }
        }
    }
}
