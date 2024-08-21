namespace EnchCoreApi.Common.CSharp.MSBuild
{
    public abstract class CompileLogger : ICompileLogger
    {
        public CompileLogger(string logDir) {
            Directory.CreateDirectory(logDir);
            LogDir = logDir;
        }
        private readonly string LogDir;
        public IReadOnlyList<CompilerError> CompilerErrors => errors;
        private readonly List<CompilerError> errors = new(50);
        public TextWriter? Writer => writer;
        public StreamWriter? writer;
        public string? CurrentPath { get; private set; }

        public void OnFinishedCompile() {
            writer?.Dispose();
            writer = null;
        }

        public void OnStartedCompile() {
            writer = new StreamWriter(File.OpenWrite(Path.Combine(LogDir, CurrentPath = $"CSCLog-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt")));
        }

        public void OnError(CompilerError error) {
            errors.Add(error);
        }

        public void Reset() {
            errors.Clear();
        }
    }
}
