using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace YTMusicUploader.AssemblyHelper
{
    /// <summary>
    /// Methods for extracting and utilising resources embedded in the DLL (including assemblies and command line utilities)
    /// </summary>
    public static class EmbeddedResourceHelper
    {
        public enum TargetAssemblyType
        {
            Calling,
            Executing
        }

        /// <summary>
        /// Returns the embedded resource if it's present in the working folder or if it's been extracted. If it's not present it will extract the embedded resource
        /// to the users AppData folder and return the full path to it
        /// </summary>
        /// <param name="fileName">Filename of embedded resource. I.e. CsvHelper.dll</param>
        /// <param name="resourcePath">i.e. the folder as a namename excluding the assembly name. I.e. Dependencies.Helpers</param>
        /// <param name="targetAssemblyType">I.e. Calling or Executing</param>
        /// <param name="skipHashCheck">Skips hash check on extracted resource</param>
        /// <returns>Full path to present or extracted directory</returns>
        public static string GetEmbeddedResourcePath(
            string fileName,
            string resourcePath,
            TargetAssemblyType targetAssemblyType,
            bool skipHashCheck = false)
        {
            Assembly assembly;
            if (targetAssemblyType == TargetAssemblyType.Calling)
                assembly = Assembly.GetCallingAssembly();
            else
                assembly = Assembly.GetExecutingAssembly();

            return GetEmbeddedResourcePath(assembly, fileName, resourcePath, skipHashCheck);
        }

        /// <summary>
        /// Returns the embedded resource if it's present in the working folder or if it's been extracted. If it's not present it will extract the embedded resource
        /// to the users AppData folder and return the full path to it
        /// </summary>
        /// <param name="fileName">Filename of embedded resource. I.e. CsvHelper.dll</param>
        /// <param name="resourcePath">i.e. the folder as a namename excluding the assembly name. I.e. Dependencies.Helpers</param>
        /// <param name="targetAssembly">A given assembly (i.e. You can do Assembly.GetCallingAssembly() or Assembly.GetExecutingAssembly()</param>
        /// <param name="skipHashCheck">Skips hash check on extracted resource</param>
        /// <returns>Full path to present or extracted directory</returns>
        public static string GetEmbeddedResourcePath(
            Assembly targetAssembly,
            string fileName,
            string resourcePath,
            bool skipHashCheck = false)
        {
            string executingFolder = Path.GetDirectoryName(targetAssembly.Location);
            string filePath = Path.Combine(executingFolder, fileName);

            if (File.Exists(filePath))
                return filePath;

            filePath = Path.Combine(Global.AppDataLocation, fileName);
            if (File.Exists(filePath))
            {
                if (skipHashCheck)
                    return filePath;
                else
                {
                    // File comparison check
                    using (var sha1 = new SHA1CryptoServiceProvider())
                    {
                        byte[] ba = null;

                        bool resourcePresent = true;
                        string targetAssemblyName = targetAssembly.GetName().Name;
                        using (var stm = targetAssembly.GetManifestResourceStream(targetAssemblyName + "." + resourcePath + "." + fileName))
                        {
                            try
                            {
                                ba = new byte[(int)stm.Length];
                                stm.Read(ba, 0, (int)stm.Length);
                            }
                            catch
                            {
                                // Comparison check won't work if we've zipped it... Just continue
                                resourcePresent = false;
                            }
                        }

                        if (resourcePresent)
                        {
                            string fileHash = BitConverter.ToString(sha1.ComputeHash(ba)).Replace("-", string.Empty);

                            byte[] bb = File.ReadAllBytes(filePath);
                            string fileHash2 = BitConverter.ToString(sha1.ComputeHash(bb)).Replace("-", string.Empty);

                            if (fileHash != fileHash2)
                                ExtractEmbeddedResource(targetAssembly, fileName, resourcePath, Global.AppDataLocation, fileName);
                        }
                        else
                        {
                            ExtractEmbeddedResource(targetAssembly, fileName, resourcePath, Global.AppDataLocation, fileName);
                        }
                    }
                }
            }
            else
            {
                ExtractEmbeddedResource(targetAssembly, fileName, resourcePath, Global.AppDataLocation, fileName);
            }

            if (File.Exists(filePath))
                return filePath;

            throw new Exception("Cannot find embedded resource '" + targetAssembly.GetName().Name.Replace("-", "_") + "." + resourcePath + "." + fileName);
        }

        /// <summary>
        /// Extracts an embedded resource to a given location
        /// </summary>
        /// <param name="fileName">Filename of embedded resource. I.e. CsvHelper.dll</param>
        /// <param name="resourcePath">i.e. the folder as a namename excluding the assembly name. I.e. Dependencies.Helpers</param>
        /// <param name="outputDirectory">The parent directory to outoput to</param>
        /// <param name="outputFilename">The filename to give the extracted resources. Use null to default to the present filename</param>
        /// <param name="targetAssemblyType">I.e Calling or executing</param>
        public static void ExtractEmbeddedResource(
            string fileName,
            string resourcePath,
            string outputDirectory,
            string outputFilename,
            TargetAssemblyType targetAssemblyType)
        {
            Assembly assembly;
            if (targetAssemblyType == TargetAssemblyType.Calling)
                assembly = Assembly.GetCallingAssembly();
            else
                assembly = Assembly.GetExecutingAssembly();

            ExtractEmbeddedResource(assembly, fileName, resourcePath, outputDirectory, outputFilename);
        }

        /// <summary>
        /// Extracts an embedded resource to a given location
        /// </summary>
        /// <param name="fileName">Filename of embedded resource. I.e. CsvHelper.dll</param>
        /// <param name="resourcePath">i.e. the folder as a namename excluding the assembly name. I.e. Dependencies.Helpers</param>
        /// <param name="outputDirectory">The parent directory to outoput to</param>
        /// <param name="outputFilename">The filename to give the extracted resources. Use null to default to the present filename</param>
        /// <param name="targetAssembly">A given assembly (i.e. You can do Assembly.GetCallingAssembly() or Assembly.GetExecutingAssembly()</param>
        public static void ExtractEmbeddedResource(
            Assembly targetAssembly,
            string fileName,
            string resourcePath,
            string outputDirectory,
            string outputFilename)
        {
            string targetAssemblyName = targetAssembly.GetName().Name;
            using (var s = targetAssembly.GetManifestResourceStream(targetAssemblyName.Replace("-", "_") + "." + resourcePath + "." + fileName))
            {
                if (s != null)
                {
                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);

                    using (var sw = new BinaryWriter(File.Open(Path.Combine(outputDirectory, string.IsNullOrEmpty(outputFilename) ? fileName : outputFilename), FileMode.Create)))
                        sw.Write(buffer);

                    return;
                }
            }

            throw new Exception("Cannot find embedded resource '" + targetAssemblyName.Replace("-", "_") + "." + resourcePath + "." + fileName);
        }
    }
}
