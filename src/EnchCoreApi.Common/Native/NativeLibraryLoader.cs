using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnchCoreApi.Common.Native {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal static class NativeLibraryLoader {
        [ModuleInitializer]
        internal static void Initialize() {
            NativeLibrary.SetDllImportResolver(CurrentAssembly, DllImportResolver);
        }

        private static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
        private static readonly RidGraph RidGraph = new RidGraph();

        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath) {
            // 1. 先尝试默认加载方式
            if (NativeLibrary.TryLoad(libraryName, assembly, searchPath, out var handle)) {
                return handle;
            }

            var currentDir = Path.Combine(Directory.GetCurrentDirectory());

            // 2. 获取 RID 继承链（从最具体到最通用）
            var currentRid = RuntimeInformation.RuntimeIdentifier;
            var fallbackRids = RidGraph.ExpandRuntimeIdentifier(currentRid);

            // 3. 按 RID 继承链尝试解压资源并加载
            foreach (var rid in fallbackRids) {
                // 3.1 从嵌入资源中解压对应 RID 的库
                if (TryExtractNativeLibrary(libraryName, rid, out var extractedPath)) {
                    // 3.2 尝试加载解压后的库
                    if (NativeLibrary.TryLoad(Path.Combine(currentDir, extractedPath), out handle)) {
                        return handle;
                    }
                }

                // 3.3 尝试直接加载已存在的库（兼容已部署的文件）
                var guessedPath = GuessLibraryPath(libraryName, rid);
                if (File.Exists(guessedPath) && NativeLibrary.TryLoad(Path.Combine(currentDir, guessedPath), out handle)) {
                    return handle;
                }
            }

            return IntPtr.Zero;
        }

        private static bool TryExtractNativeLibrary(string libName, string rid, [NotNullWhen(true)] out string? extractedPath) {
            var baseResourceName = $"Native\\libs\\{libName}\\{rid}\\";
            var resourceName = CurrentAssembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.StartsWith(baseResourceName));

            if (resourceName == null) {
                extractedPath = null;
                return false;
            }

            // 解析文件名（格式：{libName}.{rid}.{fileName}.{ext}）
            var fileNameParts = resourceName[baseResourceName.Length..].Split('\\');
            var fileNameWithExt = fileNameParts.Last();

            // 构建目标路径（runtimes/{rid}/native/{fileName}.{ext}）
            extractedPath = Path.Combine("runtimes", rid, "native", $"{fileNameWithExt}");
            if (File.Exists(extractedPath)) {
                return true;
            }

            // 解压资源文件
            Directory.CreateDirectory(Path.GetDirectoryName(extractedPath)!);
            using (var stream = CurrentAssembly.GetManifestResourceStream(resourceName))  {
                using (var fs = File.Create(extractedPath)) {
                    stream!.CopyTo(fs);
                }
            }

            return true;
        }

        private static string GuessLibraryPath(string libName, string rid) {
            var extension = GetPlatformExtension();
            return Path.Combine("runtimes", rid, "native", $"{libName}.{extension}");
        }

        private static string GetPlatformExtension() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return "so";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return "dylib";
            throw new PlatformNotSupportedException();
        }
    }
}
