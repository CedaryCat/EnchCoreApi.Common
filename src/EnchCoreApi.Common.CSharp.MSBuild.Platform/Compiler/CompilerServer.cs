using EnchCoreApi.Common.Compiler;

namespace EnchCoreApi.Common.CSharp.MSBuild.Platform.Compiler
{
    public abstract class CompilerServer
    {
        public void Run() {
            var param = new CompileParameter();
            param.FromBase64(Console.ReadLine() ?? throw new Exception());
            LoadProject(param.ProjectPath, param.LoggerDirectory, param.Properties, param.References);
            var success = Build(out var projectName, out var output, out var errors);
            var res = new CompileResultData(projectName, success, output, errors.Length, errors.Select(e => new CompileResultData.InternalCompilerError(e)).ToArray());
            var b64 = res.GetBase64();
            Console.Write(b64);
        }
        public abstract void LoadProject(string projectPath, string loggerDir, IProjectProperties properties, IProjectReferences references);
        public abstract bool Build(out string projectName, out string output, out CompilerError[] errors);
    }
}
