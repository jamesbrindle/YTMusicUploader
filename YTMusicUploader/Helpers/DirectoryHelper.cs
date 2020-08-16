using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace JBToolkit.Windows
{

    /// <summary>
    /// Class containing methods to retrieve specific file system paths.
    /// </summary>
    public static class DirectoryHelper
    {
        private static readonly string[] _knownFolderGuids = new string[]
        {
            "{56784854-C6CB-462B-8169-88E350ACB882}", // Contacts            
            "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
            "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", // Documents
            "{1777F761-68AD-4D8A-87BD-30B759FA33DD}", // Favorites
            "{BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968}", // Links
            "{4BD8D571-6D19-48D3-BE97-422220080E43}", // Music
            "{33E28130-4E1E-4676-835A-98395C3BC3BB}", // Pictures
            "{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}", // SavedGames
            "{7D1D3A04-DEBB-4115-95CF-2F29DA2920DA}", // SavedSearches
            "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", // Videos
            "{BD84B380-8CA2-1069-AB1D-08000948F534}", // Fonts
            "{4336a54d-038b-4685-ab02-99bb52d3fb8b}", // Public Folder
            "{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}", // 3D Objects
            "{018D5C66-4533-4307-9B53-224DE2ED1FE6}", // OneDrive
            "{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
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
            return SafeStream.SafeFileStream.ReadAllLines(filePath);
        }

        /// <summary>
        /// Read text file without locking it. If it is locked it should read anyway
        /// </summary>
        public static string ReadAllTextEvenLocked(string filePath)
        {
            return SafeStream.SafeFileStream.ReadAllText(filePath);
        }

        /// <summary>
        /// Read file bytes without locking it. If it is locked it should read anyway
        /// </summary>
        public static byte[] ReadAllBytesEvenLocked(string filePath)
        {
            return SafeStream.SafeFileStream.GetBytes(filePath);
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
        Downloads
    }
}