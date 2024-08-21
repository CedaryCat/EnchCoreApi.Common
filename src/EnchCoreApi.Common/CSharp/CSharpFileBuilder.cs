using System.CodeDom.Compiler;

namespace EnchCoreApi.Common.CSharp
{
    public abstract class CSharpFileBuilder
    {
        public CSharpFileBuilder(string sourceDir) {
            SourceDir = sourceDir;
        }
        public string SourceDir;
        public abstract string[] GetCode();
        public abstract string[] GetFiles();
        public abstract string ErrorTracker(CompilerErrorCollection errors);
    }
}
