using EnchCoreApi.Common.Compiler;

namespace EnchCoreApi.Common.CSharp.MSBuild.Platform.Compiler {
    public class CompilerClient {
        private CompileResult? CompileResult;
        public CompileResult Run(string cscpath, CompileParameter parameter) {
            var csc = new ConsoleAppManager(cscpath, false);

            csc.ErrorTextReceived += Csc_ErrorTextReceived;

            csc.StandartTextReceived += Csc_StandartTextReceived;

            csc.ExecuteAsync();

            csc.WriteLine(parameter.GetBase64());

            while (csc.Running) {
                Thread.Sleep(50);
            }

            csc.ErrorTextReceived -= Csc_ErrorTextReceived;

            csc.StandartTextReceived -= Csc_StandartTextReceived;

            csc.Dispose();

            return CompileResult ?? throw new Exception("compiler process exit before return compile result");
        }

        private void Csc_StandartTextReceived(object? sender, string e) {
            var data = new CompileResultData(e);
            CompileResult = new CompileResult(data);
        }

        private void Csc_ErrorTextReceived(object? sender, string e) {
            if (!string.IsNullOrWhiteSpace(e)) {
                Console.WriteLine(e);
            }
        }
    }
}
