using Microsoft.Win32;
using System.Collections.Generic;

namespace YTMusicUploader.Updater
{
    /// <summary>
    /// Uses the registry (Windows MSI Uninstaller Key) to gather a list of installed applications
    /// </summary>
    public static class InstalledApplicationHelper
    {
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
        ///  Uses the registry (Windows MSI Uninstaller Key) to gather a list of installed applications
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
    }
}
