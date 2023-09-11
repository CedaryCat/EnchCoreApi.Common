using Microsoft.Build.Framework;

namespace EnchCoreApi.Common.CSharp.MSBuild.Platform {
    public class MSCompileLogger : CompileLogger, ILogger {
        public MSCompileLogger(string logDir) : base(logDir) {
        }
        public LoggerVerbosity Verbosity { get; set; }
        public string Parameters { get; set; }
        public void Initialize(IEventSource eventSource) {
            eventSource.BuildStarted += (_, _) => OnStartedCompile();
            eventSource.AnyEventRaised += (_, e) => Writer?.WriteLine(e.Message);
            eventSource.ErrorRaised += (_, e) => {
                OnError(new CompilerError(e.File, e.LineNumber, e.ColumnNumber, e.Code, e.Message));
            };
            eventSource.BuildFinished += (_, _) => OnFinishedCompile();
        }

        public void Shutdown() {

        }
    }
}
