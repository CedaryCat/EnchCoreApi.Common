using EnchCoreApi.Common.CSharp.MSBuild;
using EnchCoreApi.Common.CSharp.MSBuild.Platform.Compiler;
using EnchCoreApi.Common.Dynamic;

namespace EnchCoreApi.Common.Compiler {
    public class CompileResult {
        public CompileResult(CompileResultData data) {
            ProjectileName = data.ProjectileName;
            Success = data.Success;
            OutPutPath = data.OutPutPath;
            errors = new CompilerError[data.Errors.Length];
            for (int i = 0; i < data.Errors.Length; i++) {
                errors[i] = new CompilerError() {
                    Line = data.Errors[i].Line,
                    Column = data.Errors[i].Column,
                    ErrorNumber = data.Errors[i].ErrorNumber,
                    ErrorText = data.Errors[i].ErrorText,
                    IsWarning = data.Errors[i].IsWarning,
                    FileName = data.Errors[i].FileName,
                };
            }
        }
        public string ProjectileName { get; private set; }
        public bool Success { get; private set; }
        public string OutPutPath { get; private set; }

        private CompilerError[] errors;
        public IReadOnlyList<CompilerError> Errors => errors;
    }
    public class CompileResultData : SerializableData {

        public CompileResultData(string projectileName, bool success, string outPutPath, int errorCount, InternalCompilerError[] errors) {
            ProjectileName = projectileName;
            Success = success;
            OutPutPath = outPutPath;
            ErrorCount = errorCount;
            Errors = errors;
        }
#nullable disable
        public CompileResultData(string base64) {
            FromBase64(base64);
        }
#nullable restore

        public string ProjectileName { get; set; }
        public bool Success { get; set; }
        public string OutPutPath { get; set; }
        public int ErrorCount { get; set; }
        public InternalCompilerError[] Errors { get; set; }

        public class InternalCompilerError {
            public int Line { get; set; }
            public int Column { get; set; }
            public string ErrorNumber { get; set; }
            public string ErrorText { get; set; }
            public bool IsWarning { get; set; }
            public string FileName { get; set; }
            public InternalCompilerError() : this(string.Empty, 0, 0, string.Empty, string.Empty) {
            }

            public InternalCompilerError(string fileName, int line, int column, string errorNumber, string errorText) {
                Line = line;
                Column = column;
                ErrorNumber = errorNumber;
                ErrorText = errorText;
                FileName = fileName;
            }
            public InternalCompilerError(CompilerError error) : this(error.FileName, error.Line, error.Column, error.ErrorNumber, error.ErrorText) {

            }

            public void Serialize(BinaryWriter writer) {
                writer.Write(Line);
                writer.Write(Column);
                writer.Write(ErrorNumber);
                writer.Write(ErrorText);
                writer.Write(IsWarning);
                writer.Write(FileName);
            }

            public void Deserialize(BinaryReader reader) {
                Line = reader.ReadInt32();
                Column = reader.ReadInt32();
                ErrorNumber = reader.ReadString();
                ErrorText = reader.ReadString();
                IsWarning = reader.ReadBoolean();
                FileName = reader.ReadString();
            }
        }

        protected override void Serialize(BinaryWriter writer)
        {
            writer.Write(ProjectileName);
            writer.Write(Success);
            writer.Write(OutPutPath);
            writer.Write(Errors.Length);
            for (int i=0;i<Errors.Length;i++)
            {
                Errors[i].Serialize(writer);
            }
        }

        protected override void Deserialize(BinaryReader reader)
        {
            ProjectileName = reader.ReadString();
            Success = reader.ReadBoolean();
            OutPutPath = reader.ReadString();
            int errorCount = reader.ReadInt32();
            for(int i=0;i< errorCount; i++)
            {
                Errors[i] = new InternalCompilerError();
                Errors[i].Deserialize(reader);
            }
        }
    }
}
