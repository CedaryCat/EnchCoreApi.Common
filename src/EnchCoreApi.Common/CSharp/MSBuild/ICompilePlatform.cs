using System.Reflection;

namespace EnchCoreApi.Common.CSharp.MSBuild
{
    public interface ICompilePlatform
    {
        public string? CSCDirectory { get; }
        public CompileLogger CreateCompileLogger(string loggerDir);
        public Project CreateProject(string projectSavePath);
        public Assembly? Compile(IProjectProvider provider, out CompilerError[] compilerErrors);
    }
    public interface IProjectProvider
    {
        public string ProjectSavePath { get; }
        public IProjectProperties ProjectProperties { get; }
        public IProjectReferences ProjectReferences { get; }
        public CompileLogger Logger { get; }
    }
}
