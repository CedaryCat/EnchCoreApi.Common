using EnchCoreApi.Common.IO;
using Microsoft.Build.Locator;
using System.Reflection;

namespace EnchCoreApi.Common.CSharp.MSBuild.Platform
{
    //[Export("StdCompilePlatform", typeof(ICompilePlatform))]
    public class CompilePlatform : ICompilePlatform
    {
        public CompilePlatform(bool initcsc = false) {
            if (initcsc) {
                var dire = Directory.GetCurrentDirectory();
                CSCDirectory = Path.Combine(dire, "Roslyn");
                if (!Directory.Exists(CSCDirectory)) {
                    using var source = typeof(CompilePlatform).Assembly.GetManifestResourceStream("EnchCoreApi.Common.CSharp.MSBuild.Platform.Roslyn.zip");
                    ZipHelper.ExtractToDirectory(source, dire);
                }
            }
        }
        static CompilePlatform() {
            if (!MSBuildLocator.IsRegistered) {
                var instances = MSBuildLocator.QueryVisualStudioInstances().ToList();
                //for (var i = 0; i < instances.Count; i++)
                //{
                //    var instance = instances[i];
                //    var recommended = string.Empty;
                //    if (instance.DiscoveryType == DiscoveryType.DeveloperConsole)
                //        recommended = " (Recommended!)";

                //    Console.WriteLine($"{i}) MSBuildLocator Instance - {instance.Name} - {instance.Version}{recommended}");
                //}
                MSBuildLocator.RegisterInstance(instances.First());
            }
        }

        public string? CSCDirectory { get; private set; }
        public Assembly? Compile(IProjectProvider provider, out CompilerError[] compilerErrors) {
            var fileInfo = new FileInfo(provider.ProjectSavePath);
            fileInfo.Directory?.Create();
            var proj = new MSProject(provider.ProjectSavePath);
            proj.Load(provider.ProjectProperties);
            proj.Load(provider.ProjectReferences);
            proj.Save();
            proj.Build(provider.Logger);
            compilerErrors = provider.Logger.CompilerErrors.ToArray();
            provider.Logger.Reset();
            if (compilerErrors.Length > 0) return null;
            else return proj.GetBuildedAssemblyFile();
        }

        public CompileLogger CreateCompileLogger(string loggerDir) => new MSCompileLogger(loggerDir);

        public Project CreateProject(string projectSavePath) => new MSProject(projectSavePath);

        public static void Initialize() {
        }
    }
}
