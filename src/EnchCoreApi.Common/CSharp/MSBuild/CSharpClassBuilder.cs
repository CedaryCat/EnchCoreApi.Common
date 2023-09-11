using System.CodeDom.Compiler;
using System.Text;

namespace EnchCoreApi.Common.CSharp {
    public class CSharpClassBuilder : CSharpFileBuilder {
        public CSharpClassBuilder(string sourceDir, string nameSpace, string className, string baseClass, params string?[] referencedNamespace) : this(sourceDir, nameSpace, className, referencedNamespace) {
            BaseClass = baseClass;
        }
        public CSharpClassBuilder(string sourceDir, string nameSpace, string className, params string?[] referencedNamespace) : base(sourceDir) {
            ClassName = className;
            Members = new Dictionary<string, CSCMember>();
            ReferencedNamespace = referencedNamespace;
            NameSpace = nameSpace;
            Writer = new StringBuilder();
            CodeLines = new List<string>();
        }
        public StringBuilder Writer { get; set; }
        public string?[] ReferencedNamespace { get; set; }
        public string NameSpace { get; set; }
        public string ClassName { get; set; }
        public string? BaseClass { get; set; }
        public Dictionary<string, CSCMember> Members { get; set; }
        public List<string> CodeLines { get; set; }

        public virtual bool AddMember(CSCMember member) {
            if (ClassName == member.Name) {
                return false;
            }
            if (Members.ContainsKey(member.Name)) {
                return false;
            }
            Members.Add(member.Name, member);
            return true;
        }
        public override string[] GetCode() {
            CodeLines.Clear();
            Writer.Clear();
            foreach (var nameSpace in ReferencedNamespace.Where(n => n != null)) {
                CodeLines.Add($"using {nameSpace};");
            }
            CodeLines.Add("");
            CodeLines.Add($"namespace {NameSpace}");
            CodeLines.Add("{");
            CodeLines.Add($"    public class {ClassName}{(BaseClass is null ? "" : $" : {BaseClass}")}");
            CodeLines.Add("    {");
            foreach (var m in Members.Values) {
                foreach (var c in m.GetCode()) {
                    CodeLines.Add($"        {c}");
                }
            }
            CodeLines.Add("    }");
            CodeLines.Add("}");
            foreach (var str in CodeLines) {
                Writer.AppendLine(str);
            }
            return new string[] { Writer.ToString() };
        }

        public override string[] GetFiles() {
            Directory.CreateDirectory(SourceDir);
            var path = Path.Combine(SourceDir, $"{ClassName}.cs");
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            try {
                using StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                streamWriter.Write(GetCode()[0]);
                streamWriter.Flush();
            }
            finally {
                fileStream.Close();
            }
            return new string[] { path };
        }
        public string GetFileTargetPath() {
            return Path.Combine(SourceDir, $"{ClassName}.cs");
        }

        public override string ErrorTracker(CompilerErrorCollection error) {
            var compilerError = $"命名空间[{NameSpace}]中，类<{ClassName}>编译失败,以下为错误代码:\n";
            foreach (CompilerError compErr in error) {
                if (compErr.Line >= CodeLines.Count || compErr.Line <= 0) {
                    compilerError += $"程序集错误：{compErr.ErrorText}\n\n";
                    continue;
                }
                compilerError += $"●文件：{compErr.FileName}\n";
                compilerError += $"●行号：{compErr.Line}错误：{compErr.ErrorText}\n";
                compilerError += $"●代码：{CodeLines[compErr.Line - 1].TrimStart()}\n\n";
            }
            return compilerError;
        }
    }
}
