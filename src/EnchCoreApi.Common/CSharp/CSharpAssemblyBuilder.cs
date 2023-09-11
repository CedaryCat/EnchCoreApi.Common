using System.CodeDom.Compiler;

namespace EnchCoreApi.Common.CSharp {
    public class CSharpAssemblyBuilder : CSharpFileBuilder {
        public CSharpClassBuilder[] Classes { get; set; }
        public CSharpAssemblyBuilder(string sourceDir, params CSharpClassBuilder[] classes) : base(sourceDir) {
            Classes = classes;
        }
        public CSharpAssemblyBuilder(string sourceDir, IEnumerable<CSharpClassBuilder> classes) : this(sourceDir, classes.ToArray()) {
        }

        public override string[] GetCode() {
            var list = new List<string>();
            foreach (var c in Classes) {
                list.AddRange(c.GetCode());
            }
            return list.ToArray();
        }

        public override string[] GetFiles() {
            var list = new List<string>();
            foreach (var c in Classes) {
                list.AddRange(c.GetFiles());
            }
            return list.ToArray();
        }

        public override string ErrorTracker(CompilerErrorCollection errors) {
            string compilerError = $"编译失败,以下为错误代码:\n";
            foreach (CompilerError compErr in errors) {
                var classBuilder = Classes.FirstOrDefault(c => c.GetFileTargetPath() == compErr.FileName);
                if (classBuilder == null) {
                    compilerError += $"程序集错误：{compErr.ErrorText}\n\n";
                    continue;
                }
                compilerError += classBuilder.ErrorTracker(errors);
            }
            return compilerError;
        }
    }
}
