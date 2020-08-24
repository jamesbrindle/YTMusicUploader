using System;
using System.IO;
using System.Linq;
using System.Text;

namespace YTMusicUploader.Business
{
    /// <summary>
    /// Used for checking Edge Core file dependencies
    /// </summary>
    public class EdgeDependencyChecker
    {
        /// <summary>
        /// Check all the required Edge Core files are present and are the right byte site (i.e. have copied over successfully)
        /// </summary>
        /// <returns>True is valid, false otherwise</returns>
        public static bool CheckEdgeCoreFilesArePresentAndCorrect()
        {
            var edgeCoreFilesList = new EdgeDependencyChecker().EdgeFilesList.ToList();
            foreach (var edgeFile in edgeCoreFilesList)
            {
                if (!(File.Exists(edgeFile.Path) && new FileInfo(edgeFile.Path).Length == edgeFile.Size))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Developer use only, to generate a list of Edge core file dependencies to check against
        /// </summary>
        internal static void GenerateEdgeFilesListArray()
        {
            var sb = new StringBuilder();
            foreach (string file in Directory.GetFiles(Global.EdgeFolder, "*.*", SearchOption.AllDirectories))
            {
                sb.AppendLine("new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @\"" +
                    file.Replace(Global.EdgeFolder + "\\", "") + "\"), Size = " + new FileInfo(file).Length + "},");
            }

            File.WriteAllText(@"C:\Temp\EdgeFileList.txt", sb.ToString());
        }

        private class EdgeCoreFile
        {
            public string Path { get; set; }
            public int Size { get; set; }
        }

        private EdgeCoreFile[] EdgeFilesList { get; } = {
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, Global.EdgeVersion + ".manifest"), Size = 222},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"d3dcompiler_47.dll"), Size = 4502464},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"dual_engine_adapter_x64.dll"), Size = 2792240},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"eventlog_provider.dll"), Size = 14728},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"icudtl.dat"), Size = 10688448},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"libEGL.dll"), Size = 439120},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"libsmartscreen.dll"), Size = 3821456},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"microsoft_apis.dll"), Size = 406416},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"msedge.dll"), Size = 151319936},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"msedge.dll.sig"), Size = 1389},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"msedgewebview2.exe"), Size = 2455952},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"msedgewebview2.exe.sig"), Size = 1389},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"msedge_100_percent.pak"), Size = 679556},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"msedge_200_percent.pak"), Size = 1254756},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"msedge_elf.dll"), Size = 981888},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"resources.pak"), Size = 10267888},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"v8_context_snapshot.bin"), Size = 170746},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"EBWebView\x64\EmbeddedBrowserWebView.dll"), Size = 3100032},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"EBWebView\x86\EmbeddedBrowserWebView.dll"), Size = 2504080},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Extensions\external_extensions.json"), Size = 99},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\beta.identity_helper.exe.manifest"), Size = 1449},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\canary.identity_helper.exe.manifest"), Size = 1451},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\dev.identity_helper.exe.manifest"), Size = 1448},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\identity_helper.Sparse.Beta.msix"), Size = 53470},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\identity_helper.Sparse.Canary.msix"), Size = 53441},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\identity_helper.Sparse.Dev.msix"), Size = 52844},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\identity_helper.Sparse.Internal.msix"), Size = 56768},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\identity_helper.Sparse.Stable.msix"), Size = 56764},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\internal.identity_helper.exe.manifest"), Size = 1453},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\resources.pri"), Size = 1168},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"identity_proxy\stable.identity_helper.exe.manifest"), Size = 1451},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Locales\en-GB.pak"), Size = 293413},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Locales\en-US.pak"), Size = 288463},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"MEIPreload\manifest.json"), Size = 228},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"MEIPreload\preloaded_data.pb"), Size = 7022},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"MLModels\autofill_labeling.onnx"), Size = 13040},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"MLModels\autofill_labeling_email.onnx"), Size = 17141},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"MLModels\autofill_labeling_features.txt"), Size = 1538},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"MLModels\autofill_labeling_features_email.txt"), Size = 3084},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"MLModels\nexturl.onnx"), Size = 131258},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"swiftshader\libEGL.dll"), Size = 450376},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"swiftshader\libGLESv2.dll"), Size = 2638152},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\Advertising"), Size = 24990},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\Analytics"), Size = 4636},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\CompatExceptions"), Size = 206},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\Content"), Size = 7379},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\Cryptomining"), Size = 1084},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\Entities"), Size = 67497},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\Fingerprinting"), Size = 1575},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\LICENSE"), Size = 35147},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\manifest.json"), Size = 132},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\Other"), Size = 34},
            new EdgeCoreFile{ Path = Path.Combine(Global.EdgeFolder, @"Trust Protection Lists\Social"), Size = 999}
        };
    }
}
