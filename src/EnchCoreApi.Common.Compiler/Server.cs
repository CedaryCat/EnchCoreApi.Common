using EnchCoreApi.Common.CSharp.MSBuild;
using EnchCoreApi.Common.CSharp.MSBuild.Platform;
using EnchCoreApi.Common.CSharp.MSBuild.Platform.Compiler;

namespace EnchCoreApi.Common.Compiler
{
    public class Server : CompilerServer
    {
        public override bool Build(out string projectName, out string output, out CompilerError[] errors) {
            projectName = Project.ProjectFile.Name;
            output = Project.OutPutPath;
            var res = Project.Build(CompileLogger);
            if (CompileLogger.CompilerErrors.Count > 0) {
                errors = new CompilerError[CompileLogger.CompilerErrors.Count];
                for (int i = 0; i < errors.Length; i++) {
                    errors[i] = CompileLogger.CompilerErrors[i];
                }
            }
            else {
                errors = Array.Empty<CompilerError>();
            }
            return res;
        }

        public override void LoadProject(string projectPath, string loggerDir, IProjectProperties properties, IProjectReferences references) {
            CompilePlatform platform = new CompilePlatform(false);

            Project = platform.CreateProject(projectPath);

            Project.Load(properties);

            Project.Load(references);

            Project.Save();

            CompileLogger = platform.CreateCompileLogger(loggerDir);
        }

        private Project Project;
        private CompileLogger CompileLogger;
    }
}
