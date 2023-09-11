
namespace EnchCoreApi.Common.CSharp.MSBuild {
    public interface ICompileLogger {
        public IReadOnlyList<CompilerError> CompilerErrors { get; }
        public TextWriter? Writer { get; }
        public void Reset();
        public void OnFinishedCompile();
        public void OnStartedCompile();
    }
}
