using System;
using System.Collections.Generic;
using System.Data.HashFunction.xxHash;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace JBToolkit.Windows
{
    /// <summary>
    /// Class containing methods to retrieve specific file system paths.
    /// </summary>
    public static class DirectoryHelper
    {
        private static xxHashConfig XxHashConfig = new xxHashConfig() { HashSizeInBits = 64 };
        private static IxxHash XxHashFactory = xxHashFactory.Instance.Create(XxHashConfig);

        private static readonly string[] _knownFolderGuids = new string[]
        {
            "{56784854-C6CB-462B-8169-88E350ACB882}", // Contacts            
            "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
            "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", // Documents
            "{1777F761-68AD-4D8A-87BD-30B759FA33DD}", // Favorites
            "{BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968}", // Links
            "{4BD8D571-6D19-48D3-BE97-422220080E43}", // Music
            "{33E28130-4E1E-4676-835A-98395C3BC3BB}", // Pictures
            "{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}", // Saved Games
            "{7D1D3A04-DEBB-4115-95CF-2F29DA2920DA}", // Saved Searches
            "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", // Videos
            "{BD84B380-8CA2-1069-AB1D-08000948F534}", // Fonts
            "{4336a54d-038b-4685-ab02-99bb52d3fb8b}", // Public Folder
            "{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}", // 3D Objects
            "{018D5C66-4533-4307-9B53-224DE2ED1FE6}", // OneDrive
            "{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
            "{F1B32785-6FBA-4FCF-9D55-7B8E7F157091}", // Local App Data
            "{F1B32785-6FBA-4FCF-9D55-7B8E7F157091}", // Add New Programs
            "{724EF170-A42D-4FEF-9F26-B60E846FBA4F}", // Admin Tools
            "{D0384E7D-BAC3-4797-8F14-CBA229B392B5}", // Common Admin Tools
            "{0139D44E-6AFE-49F2-8690-3DAFCAE6FFB8}", // Common Programs
            "{A4115719-D62E-491D-AA7C-E74B8BE3B067}", // CommonStartMenu
            "{82A5EA35-D9CD-47C5-9629-E15D2F714E6E}", // CommonStartup
            "{B94237E7-57AC-4347-9151-B08C6C32D1F7}", // CommonTemplates
            "{0AC0837C-BBF8-452A-850D-79D08E667CA7}", // Computer
            "{82A74AEB-AEB4-465C-A014-D097EE346D63}", // ControlPanel
            "{CAC52C1A-B53D-4EDC-92D7-6B2E8AC19434}", // Games
            "{D20BEEC4-5CA8-4905-AE3B-BF251EA09B53}", // Network
            "{76FC4E2D-D6AD-4519-A663-37BD56068185}", // Printers
            "{62AB5D82-FDC1-4DC3-A9DD-070D1D495D97}", // ProgramData
            "{905E63B6-C1BF-494E-B29C-65B732D3D21A}", // ProgramFiles
            "{6D809377-6AF0-444B-8957-A3773F02200E}", // ProgramFilesX64
            "{7C5A40EF-A0FB-4BFC-874A-C0F2E0B9FA8E}", // ProgramFilesX86
            "{A77F5D77-2E2B-44C3-A6A2-ABA601054A51}", // Programs
            "{DFDF76A2-C82A-4D63-906A-5644AC457385}", // Public
            "{C4AA340D-F20F-4863-AFEF-F87EF2E6BA25}", // PublicDesktop
            "{ED4824AF-DCE4-45A8-81E2-FC7965083634}", // PublicDocuments
            "{3D644C9B-1FB8-4F30-9B45-F670235F79C0}", // PublicDownloads
            "{3214FAB5-9757-4298-BB61-92A9DEAA44FF}", // PublicMusic
            "{B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5}", // PublicPictures
            "{2400183A-6185-49FB-A2D8-4A392A602BA3}", // PublicVideos
            "{52A4F021-7B75-48A9-9F6B-4B87A210BC8F}", // QuickLaunch
            "{AE50C081-EBD2-438A-8655-8A092E34987A}", // RecordedTV
            "{BD85E001-112E-431E-983B-7B15AC09FFF1}", // Recent
            "{B7534046-3ECB-4C18-BE4E-64CD4CB7D6AC}", // RecycleBin
            "{3EB685DB-65F9-4CF6-A03A-E3EF65729F3D}", // RoamingAppData
            "{625B53C3-AB48-4EC1-BA1F-A1EF4146FC19}", // StartMenu
            "{B97D20BB-F46A-4C97-BA10-5E3608430854}", // Startup
            "{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}", // System
            "{D65231B0-B2F1-4857-A4CE-A8E7C6EA7D27}", // SystemX86
            "{A63293E8-664E-48DB-A079-DF759E0509F7}", // Templates
            "{F3CE0F7C-4901-4ACC-8648-D5D44B04EF8F}", // UsersFiles
            "{F38BF404-1D43-42F2-9305-67DE0B28FC23}"  // Windows
        };

        /// <summary>
        /// Return whether or not path string is that of a file
        /// </summary>
        /// <param name="path">String path to file or directory</param>
        /// <returns>True if file, false otherwise</returns>
        public static bool IsFile(this string path)
        {
            try
            {
                return !File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            }
            catch { return false; }
        }

        /// <summary>
        /// Return whether or not path string is that of a directory
        /// </summary>
        /// <param name="path">String path to file or directory</param>
        /// <returns>True if directory, false otherwise</returns>
        public static bool IsDirectory(this string path)
        {
            try
            {
                return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            }
            catch { return false; }
        }

        /// <summary>
        /// Returns recursed list of directories in a traversal order
        /// </summary>
        /// <param name="targetDirectory">Parent directory</param>
        /// <returns>List of directories</string></returns>
        public static List<string> GetAllDirectoriesTraversive(this string targetDirectory)
        {
            List<string> directories = new List<string>();
            TraverseDirectories(targetDirectory, directories);

            return directories;
        }

        private static void TraverseDirectories(string targetDirectory, List<string> directories)
        {
            directories.Add(targetDirectory);

            try
            {
                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    TraverseDirectories(subdirectory, directories);
                }
            }
            catch { }
        }

        /// <summary>
        /// Recursively fetches files in a directory and subdirectory in a more organised order
        /// </summary>
        public static List<string> GetAllFilesTraversive(string targetDirectory)
        {
            List<string> directories = new List<string>();
            List<string> files = new List<string>();

            TraverseDirectories(targetDirectory, directories);

            for (int i = 0; i < directories.Count; i++)
            {
                try
                {
                    files.AddRange(Directory.GetFiles(directories[i]));
                }
                catch { }
            }

            return files;
        }

        /// <summary>
        /// Recursively fetches file infos in a directory and subdirectory in a more organised order
        /// </summary>
        public static List<FileInfo> GetAllFileInfoTraversive(string targetDirectory)
        {
            List<string> directories = new List<string>();
            List<FileInfo> files = new List<FileInfo>();
            TraverseDirectories(targetDirectory, directories);

            for (int i = 0; i < directories.Count; i++)
            {
                try
                {
                    files.AddRange(new DirectoryInfo(directories[i]).GetFiles());
                }
                catch { }
            }

            return files;
        }

        /// <summary>
        /// Shortens a file path to something more readable and containable be adding ... in in the enter. I.e. C:\users\jamesb1\...\Documents
        /// </summary>
        public static string EllipsisPath(string filePath, int maxLength = 100)
        {
            if (filePath.Length > maxLength)
            {
                // Find last '\' character
                int i = filePath.LastIndexOf('\\');

                string tokenRight = filePath.Substring(i, filePath.Length - i);
                string tokenCenter = @"\...";
                string tokenLeft = filePath.Substring(0, maxLength - (tokenRight.Length + tokenCenter.Length));

                return tokenLeft + tokenCenter + tokenRight;
            }
            else
                return filePath;
        }

        /// <summary>
        /// Gets the current path to the specified known folder as currently configured. This does
        /// not require the folder to be existent.
        /// </summary>
        /// <param name="knownFolder">The known folder which current path will be returned.</param>
        /// <returns>The default path of the known folder.</returns>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
        ///     could not be retrieved.</exception>
        public static string GetPath(KnownFolder knownFolder)
        {
            return GetPath(knownFolder, false);
        }

        /// <summary>
        /// Gets the current path to the specified known folder as currently configured. This does
        /// not require the folder to be existent.
        /// </summary>
        /// <param name="knownFolder">The known folder which current path will be returned.</param>
        /// <param name="defaultUser">Specifies if the paths of the default user (user profile
        ///     template) will be used. This requires administrative rights.</param>
        /// <returns>The default path of the known folder.</returns>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
        ///     could not be retrieved.</exception>
        public static string GetPath(KnownFolder knownFolder, bool defaultUser)
        {
            return GetPath(knownFolder, KnownFolderFlags.DontVerify, defaultUser);
        }

        /// <summary>
        /// Gets the current path to the specified known folder as currently configured. This does
        /// not require the folder to be existent.
        /// </summary>
        /// <param name="knownFolder">The known folder which current path will be returned.</param>
        /// <param name="defaultUser">Specifies if the paths of the default user (user profile
        ///     template) will be used. This requires administrative rights.</param>
        /// <returns>The default path of the known folder.</returns>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
        ///     could not be retrieved.</exception>
        private static string GetPath(KnownFolder knownFolder, KnownFolderFlags flags,
            bool defaultUser)
        {
            int result = SHGetKnownFolderPath(new Guid(_knownFolderGuids[(int)knownFolder]),
                (uint)flags, new IntPtr(defaultUser ? -1 : 0), out IntPtr outPath);
            if (result >= 0)
            {
                return Marshal.PtrToStringUni(outPath);
            }
            else
            {
                throw new ExternalException("Unable to retrieve the known folder path. It may not "
                    + "be available on this system.", result);
            }
        }

        /// <summary>
        /// Gets the temp path of the Windows environment of the current user
        /// </summary>
        /// <returns>Temp path location</returns>
        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// Get unique filename by DateTime ticks and return the full path
        /// </summary>
        /// <returns>Temp file path</returns>
        public static string GetTempFile()
        {
            string ticks = DateTime.Now.Ticks.ToString();
            return Path.Combine(GetTempPath(), "temp-" + ticks);
        }

        /// <summary>
        /// Get unique filename by DateTime ticks and return the full path
        /// </summary>
        /// <returns>Temp file path</returns>
        public static string GenerateTempFilename()
        {
            return "temp-" + DateTime.Now.Ticks;
        }

        /// <summary>
        /// Get timestamp to add to a filename (returns date and time in reverse)
        /// </summary>
        public static string Timestamp()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }

        /// <summary>
        /// Get datestamp to add to a filename (returns date in reverse)
        /// </summary>
        public static string Datestamp()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
        }

        /// <summary>
        /// Get time-unique string based on DateTime ticks
        /// </summary>
        public static string Tickstamp()
        {
            return (DateTime.Now.Ticks - new DateTime(2020, 1, 1).Ticks).ToString("x");
        }

        /// <summary>
        /// Get Short Ticks as integer
        /// </summary>
        public static int ShortTickIntStamp()
        {
            string ticks = DateTime.Now.Ticks.ToString();
            ticks = ticks.Substring(10, ticks.Length - 10);

            return ticks.ToInt();
        }

        /// <summary>
        /// Checks whether a file is locked - Useful if we want to write to a file
        /// </summary>
        /// <param name="path">Path of file to check if locked</param>
        /// <returns>True is locked, false otherwise</returns>
        public static bool IsFileLocked(string path)
        {
            // it must exist for it to be locked
            if (File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                FileStream stream = null;

                try
                {
                    stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (IOException)
                {
                    //the file is unavailable because it is:
                    //still being written to
                    //or being processed by another thread
                    //or does not exist (has already been processed)
                    return true;
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// Read tex file and return lines without locking it. If it is locked it should read anyway
        /// </summary>
        public static string[] ReadAllLinesEvenLocked(string filePath)
        {
            return StreamHelpers.SafeFileStream.ReadAllLines(filePath);
        }

        /// <summary>
        /// Read text file without locking it. If it is locked it should read anyway
        /// </summary>
        public static string ReadAllTextEvenLocked(string filePath)
        {
            return StreamHelpers.SafeFileStream.ReadAllText(filePath);
        }

        /// <summary>
        /// Read file bytes without locking it. If it is locked it should read anyway
        /// </summary>
        public static byte[] ReadAllBytesEvenLocked(string filePath)
        {
            return StreamHelpers.SafeFileStream.GetBytes(filePath);
        }

        /// <summary>
        /// Calculate the hash of a file with buffered read
        /// </summary>
        public static async Task<string> GetFileHash(string path)
        {
            using (var stream = new BufferedStream(File.OpenRead(path), 1200000))
            {                
                byte[] hashedValue = XxHashFactory.ComputeHash(stream).Hash;
                return await Task.FromResult(BitConverter.ToString(hashedValue));                
            }
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken,
            out IntPtr ppszPath);

        [Flags]
        private enum KnownFolderFlags : uint
        {
            SimpleIDList = 0x00000100,
            NotParentRelative = 0x00000200,
            DefaultPath = 0x00000400,
            Init = 0x00000800,
            NoAlias = 0x00001000,
            DontUnexpand = 0x00002000,
            DontVerify = 0x00004000,
            Create = 0x00008000,
            NoAppcontainerRedirection = 0x00010000,
            AliasOnly = 0x80000000
        }
    }

    /// <summary>
    /// Standard folders registered with the system. These folders are installed with Windows Vista
    /// and later operating systems, and a computer will have only folders appropriate to it
    /// installed.
    /// </summary>
    public enum KnownFolder
    {
        /*
            "{56784854-C6CB-462B-8169-88E350ACB882}", // Contacts            
            "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
            "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", // Documents
            "{1777F761-68AD-4D8A-87BD-30B759FA33DD}", // Favorites
            "{BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968}", // Links
            "{4BD8D571-6D19-48D3-BE97-422220080E43}", // Music
            "{33E28130-4E1E-4676-835A-98395C3BC3BB}", // Pictures
            "{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}", // Saved Games
            "{7D1D3A04-DEBB-4115-95CF-2F29DA2920DA}", // Saved Searches
            "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", // Videos
            "{BD84B380-8CA2-1069-AB1D-08000948F534}", // Fonts
            "{4336a54d-038b-4685-ab02-99bb52d3fb8b}", // Public Folder
            "{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}", // 3D Objects
            "{018D5C66-4533-4307-9B53-224DE2ED1FE6}", // OneDrive
            "{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
            "{F1B32785-6FBA-4FCF-9D55-7B8E7F157091}", // Local App Data
            "{F1B32785-6FBA-4FCF-9D55-7B8E7F157091}", // Add New Programs
            "{724EF170-A42D-4FEF-9F26-B60E846FBA4F}", // Admin Tools
            "{D0384E7D-BAC3-4797-8F14-CBA229B392B5}", // Common Admin Tools
            "{0139D44E-6AFE-49F2-8690-3DAFCAE6FFB8}", // Common Programs
            "{A4115719-D62E-491D-AA7C-E74B8BE3B067}", // Common StartMenu
            "{82A5EA35-D9CD-47C5-9629-E15D2F714E6E}", // Common Startup
            "{B94237E7-57AC-4347-9151-B08C6C32D1F7}", // Common Templates
            "{0AC0837C-BBF8-452A-850D-79D08E667CA7}", // Computer
            "{82A74AEB-AEB4-465C-A014-D097EE346D63}", // Control Panel
            "{CAC52C1A-B53D-4EDC-92D7-6B2E8AC19434}", // Games
            "{D20BEEC4-5CA8-4905-AE3B-BF251EA09B53}", // Network
            "{76FC4E2D-D6AD-4519-A663-37BD56068185}", // Printers
            "{62AB5D82-FDC1-4DC3-A9DD-070D1D495D97}", // Program Data
            "{905E63B6-C1BF-494E-B29C-65B732D3D21A}", // Program Files
            "{6D809377-6AF0-444B-8957-A3773F02200E}", // Program Files X64
            "{7C5A40EF-A0FB-4BFC-874A-C0F2E0B9FA8E}", // Program Files X86
            "{A77F5D77-2E2B-44C3-A6A2-ABA601054A51}", // Programs
            "{DFDF76A2-C82A-4D63-906A-5644AC457385}", // Public
            "{C4AA340D-F20F-4863-AFEF-F87EF2E6BA25}", // Public Desktop
            "{ED4824AF-DCE4-45A8-81E2-FC7965083634}", // Public Documents
            "{3D644C9B-1FB8-4F30-9B45-F670235F79C0}", // Public Downloads
            "{3214FAB5-9757-4298-BB61-92A9DEAA44FF}", // Public Music
            "{B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5}", // Public Pictures
            "{2400183A-6185-49FB-A2D8-4A392A602BA3}", // Public Videos
            "{52A4F021-7B75-48A9-9F6B-4B87A210BC8F}", // Quick Launch
            "{AE50C081-EBD2-438A-8655-8A092E34987A}", // Recorded TV
            "{BD85E001-112E-431E-983B-7B15AC09FFF1}", // Recent
            "{B7534046-3ECB-4C18-BE4E-64CD4CB7D6AC}", // Recycle Bin
            "{3EB685DB-65F9-4CF6-A03A-E3EF65729F3D}", // Roaming App Data
            "{625B53C3-AB48-4EC1-BA1F-A1EF4146FC19}", // Start Menu
            "{B97D20BB-F46A-4C97-BA10-5E3608430854}", // Startup
            "{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}", // System
            "{D65231B0-B2F1-4857-A4CE-A8E7C6EA7D27}", // System X86
            "{A63293E8-664E-48DB-A079-DF759E0509F7}", // Templates
            "{F3CE0F7C-4901-4ACC-8648-D5D44B04EF8F}", // Users Files
            "{F38BF404-1D43-42F2-9305-67DE0B28FC23}"  // Windows                
         */


        Contacts,
        Desktop,
        Documents,
        Favorites,
        Links,
        Music,
        Pictures,
        SavedGames,
        SavedSearches,
        Videos,
        Fonts,
        PublicFolder,
        ThreeDObjects,
        OneDrive,
        Downloads,
        LocalAppData,
        AddNewPrograms,
        AdminTools,
        CommonAdminTools,
        CommonPrograms,
        CommonStartMenu,
        CommonStartup,
        CommonTemplates,
        Computer,
        ControlPanel,
        Games,
        Network,
        Printers,
        ProgramData,
        ProgramFiles,
        ProgramFilesX64,
        ProgramFilesX86,
        Programs,
        Public,
        PublicDesktop,
        PublicDocuments,
        PublicDownloads,
        PublicMusic,
        PublicPictures,
        PublicVideos,
        QuickLaunch,
        RecordedTV,
        Recent,
        RecycleBin,
        RoamingAppData,
        StartMenu,
        Startup,
        System,
        SystemX86,
        Templates,
        UsersFiles,
        Windows
    }
}