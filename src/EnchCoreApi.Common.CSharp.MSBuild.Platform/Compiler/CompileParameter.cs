using EnchCoreApi.Common.CSharp.MSBuild;
using EnchCoreApi.Common.CSharp.MSBuild.Platform.Compiler;
using EnchCoreApi.Common.Dynamic;
using Terraria;

namespace EnchCoreApi.Common.Compiler {
    public class CompileParameter : SerializableData {
#nullable disable
        public string ProjectPath { get; set; }
        public string LoggerDirectory { get; set; }
        public Properties Properties { get; set; }
        public References References { get; set; }

        protected override void Deserialize(BinaryReader reader)
        {
            ProjectPath = reader.ReadString();
            LoggerDirectory = reader.ReadString();
            Properties = new Properties();
            Properties.Deserialize(reader);
            References = new References();
            References.Deserialize(reader);
        }

        protected override void Serialize(BinaryWriter writer)
        {
            writer.Write(ProjectPath);
            writer.Write(LoggerDirectory);
            Properties.Serialize(writer);
            References.Serialize(writer);
        }
#nullable restore
    }
    public class Properties : IProjectProperties {
#nullable disable
        public Properties() {
#nullable restore
        }
        public Properties(IProjectProperties properties) {
            TargetFramework = properties.TargetFramework;
            LangVersion = properties.LangVersion;
            Configuration = properties.Configuration;
            OutPutType = properties.OutPutType;
            Optimize = properties.Optimize;
            OutDir = properties.OutDir;
            ImplicitUsings = properties.ImplicitUsings;
            Nullable = properties.Nullable;
            AllowUnsafeBlocks = properties.AllowUnsafeBlocks;
            TieredPGO = properties.TieredPGO;
            CscToolPath = properties.CscToolPath;
            OtherProperties = properties.OtherProperties;
        }
        public void CopyFrom(IProjectProperties properties) {
            TargetFramework = properties.TargetFramework;
            LangVersion = properties.LangVersion;
            Configuration = properties.Configuration;
            OutPutType = properties.OutPutType;
            Optimize = properties.Optimize;
            OutDir = properties.OutDir;
            ImplicitUsings = properties.ImplicitUsings;
            Nullable = properties.Nullable;
            AllowUnsafeBlocks = properties.AllowUnsafeBlocks;
            TieredPGO = properties.TieredPGO;
            CscToolPath = properties.CscToolPath;
            OtherProperties = properties.OtherProperties;
        }
        public string TargetFramework { get; set; }
        public string? LangVersion { get; set; }
        public Configuration Configuration { get; set; }
        public OutPutType OutPutType { get; set; }
        public bool Optimize { get; set; }
        public string? OutDir { get; set; }
        public string? ImplicitUsings { get; set; }
        public string? Nullable { get; set; }
        public bool AllowUnsafeBlocks { get; set; }
        public bool TieredPGO { get; set; }
        public string? CscToolPath { get; set; }
        public IEnumerable<KeyValuePair<string, string?>> OtherProperties { get; set; } = Array.Empty<KeyValuePair<string, string?>>();

        public void Deserialize(BinaryReader reader) {
            BitsByte options = reader.ReadByte();
            TargetFramework = reader.ReadString();
            if (options[0]) LangVersion = reader.ReadString();
            Configuration = (Configuration)reader.ReadByte();
            OutPutType = (OutPutType)reader.ReadByte();
            Optimize = options[1];
            if (options[2]) OutDir = reader.ReadString();
            if (options[3]) ImplicitUsings = reader.ReadString();
            if (options[4]) Nullable = reader.ReadString();
            AllowUnsafeBlocks = options[5];
            TieredPGO = options[6];
            if (options[7]) CscToolPath = reader.ReadString();
            var list = new List<KeyValuePair<string, string?>>();
            int count = reader.ReadInt32();
            while (count-- > 0) {
                list.Add(new KeyValuePair<string, string?>(reader.ReadString(), reader.ReadString()));
            }
            OtherProperties = list;
        }

        public void Serialize(BinaryWriter writer) {
            BitsByte options = 0;

            options[0] = LangVersion != null;
            options[1] = Optimize;
            options[2] = OutDir != null;
            options[3] = ImplicitUsings != null;
            options[4] = Nullable != null;
            options[5] = AllowUnsafeBlocks;
            options[6] = TieredPGO;
            options[7] = CscToolPath != null;

            writer.Write(options);
            writer.Write(TargetFramework);
            if (LangVersion != null) writer.Write(LangVersion);
            writer.Write((byte)Configuration);
            writer.Write((byte)OutPutType);
            if (OutDir != null) writer.Write(OutDir);
            if (ImplicitUsings != null) writer.Write(ImplicitUsings);
            if (Nullable != null) writer.Write(Nullable);
            if (CscToolPath != null) writer.Write(CscToolPath);

            int count = 0;
            foreach (var kv in OtherProperties) {
                if (kv.Value != null) {
                    count++;
                }
            }
            writer.Write(count);
            foreach (var kv in OtherProperties) {
                if (kv.Value != null) {
                    writer.Write(kv.Key);
                    writer.Write(kv.Value);
                }
            }
        }
    }
    public class References : IProjectReferences {
        public References() {

        }
        public References(IProjectReferences properties) {
            CopyFrom(properties);
        }
        public void CopyFrom(IProjectReferences properties) {
            RefFiles = properties.RefFiles;
        }
        public IEnumerable<FileInfo> RefFiles { get; set; } = Array.Empty<FileInfo>();

        public void Deserialize(BinaryReader reader) {
            int count = reader.ReadInt32();
            var arr = new FileInfo[count];
            for (int i = 0; i < count; i++) {
                arr[i] = new FileInfo(reader.ReadString());
            }
            RefFiles = arr;
        }

        public void Serialize(BinaryWriter writer) {
            writer.Write(RefFiles.Count());
            foreach (var file in RefFiles) {
                writer.Write(file.FullName);
            }
        }
    }
}
