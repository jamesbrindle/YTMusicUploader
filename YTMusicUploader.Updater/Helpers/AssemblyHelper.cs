using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace YTMusicUploader.Updater
{
    /// <summary>
    /// Loads embedded assemblies from the YTMusicUploader.Update.dll
    /// </summary>
    internal class AssemblyHelper
    {
        private static Dictionary<string, Assembly> AssemblyDictionary = null;

        /// <summary>
        /// Loads embedded assemblies from the YTMusicUploader.Update.dll
        /// </summary>
        internal static void PreloadAssemblies()
        {
            Parallel.For(0, 2, i =>
            {
                switch (i)
                {
                    case 0:
                        PreloadAssembly("Microsoft.WindowsAPICodePack.Shell.dll");
                        break;
                    case 1:
                        PreloadAssembly("Microsoft.WindowsAPICodePack.dll");
                        break;
                    default:
                        break;
                }
            });
        }

        private static void PreloadAssembly(string fileName)
        {
            if (AssemblyDictionary == null)
                AssemblyDictionary = new Dictionary<string, Assembly>();

            bool extractResourceToTempPath = false;

            try
            {
                using (var stm = Assembly.GetExecutingAssembly().GetManifestResourceStream("Embed." + fileName))
                {
                    if (stm != null)
                    {
                        byte[] ba = new byte[(int)stm.Length];
                        stm.Read(ba, 0, (int)stm.Length);

                        var asm = Assembly.Load(ba);
                        AssemblyDictionary.Add(asm.FullName, asm);

                        return;
                    }
                    else
                        extractResourceToTempPath = true;
                }
            }
            catch
            {
                extractResourceToTempPath = true;
            }

            if (extractResourceToTempPath)
            {
                string outputPath = ExtractEmbeddedResourceToTempPath(fileName);
                var asm = Assembly.LoadFrom(outputPath);
                AssemblyDictionary.Add(asm.FullName, asm);
            }
        }

        internal static Assembly LoadAssembly(string assemblyFullName)
        {
            if (AssemblyDictionary == null || AssemblyDictionary.Count == 0)
                return null;

            if (AssemblyDictionary.ContainsKey(assemblyFullName))
                return AssemblyDictionary[assemblyFullName];

            return null;
        }

        internal static string ExtractEmbeddedResourceToTempPath(string fileName)
        {
            string tempPath = Path.GetTempPath();
            string filePath = Path.Combine(tempPath, fileName);

            if (File.Exists(filePath))
                return filePath;
            else
            {
                ExtractEmbeddedResource(fileName, Path.GetTempPath(), fileName);
                return filePath;
            }
        }

        private static void ExtractEmbeddedResource(
                string fileName,
                string outputDirectory,
                string outputFilename)
        {

            using (var s = Assembly.GetExecutingAssembly()
                                        .GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName()
                                        .Name.Replace("-", "_") + ".Embed." + fileName))
            {
                if (s != null)
                {
                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);

                    using (var sw = new BinaryWriter(
                        File.Open(Path.Combine(outputDirectory, string.IsNullOrEmpty(outputFilename)
                            ? fileName
                            : outputFilename),
                        FileMode.Create)))
                        sw.Write(buffer);

                    return;
                }
            }
        }
    }
}
