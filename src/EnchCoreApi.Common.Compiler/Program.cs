using EnchCoreApi.Common.CSharp.MSBuild.Platform;

namespace EnchCoreApi.Common.Compiler
{
    internal class Program
    {
        private static void Main(string[] args) {
            var server = new Server();
            server.Run();
        }
    }
}