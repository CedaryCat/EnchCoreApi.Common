using System.Diagnostics;
using System.Reflection;

namespace EnchCoreApi.Common.CSharp.MSBuild.Platform
{
    public class MSProject : Project
    {
        private bool FromFile;
        public override IProjectProperties? Properties { get; protected set; }
        public override IProjectReferences? References { get; protected set; }
        public MSProject(string path) : base(path) {
            Project = new Microsoft.Build.Evaluation.Project();
            Project.Xml.Sdk = "Microsoft.NET.Sdk";
        }
        public Microsoft.Build.Evaluation.Project Project { get; private set; }
        public string OutPutExtension {
            get {
                if (!FromFile) {
                    return Properties.OutPutType == OutPutType.Library ? ".dll" : $".{Properties.OutPutType.ToString().ToLower()}";
                }
                else {
                    var type = Project.GetPropertyValue("OutPutType");
                    return type.Length == 0 || type == "Library" ? ".dll" : $".{type.ToLower()}";
                }
            }
        }

        public override string OutPutPath {
            get {
                var dir = ProjectFile.DirectoryName ?? throw new Exception();
                if (!FromFile) {
                    if (Properties.OutDir != null && Properties.OutDir.Length > 0) {
                        return Path.Combine(dir, Properties.OutDir, ProjectFile.Name[..^ProjectFile.Extension.Length] + OutPutExtension);
                    }
                    else {
                        return Path.Combine(dir, "bin", Properties.Configuration.ToString(), Properties.TargetFramework, ProjectFile.Name[..^ProjectFile.Extension.Length] + OutPutExtension);
                    }
                }
                else {
                    if (Project.GetPropertyValue("OutDir").Length > 0) {
                        return Path.Combine(dir, Project.GetPropertyValue("OutDir"), ProjectFile.Name[..^ProjectFile.Extension.Length] + OutPutExtension);
                    }
                    else {
                        var conf = Project.GetPropertyValue("Configuration");
                        if (conf.Length == 0) conf = "Debug";
                        return Path.Combine(dir, "bin", conf, Project.GetPropertyValue("TargetFramework"), ProjectFile.Name[..^ProjectFile.Extension.Length] + OutPutExtension);
                    }
                }
            }
        }

        public override void Load(IProjectProperties properties) {
            Properties = properties;

            Project.Xml.AddProperty(nameof(Properties.TargetFramework), Properties.TargetFramework);
            if (Properties.LangVersion != null && Properties.LangVersion.Length > 0) Project.Xml.AddProperty(nameof(Properties.LangVersion), Properties.LangVersion);
            Project.Xml.AddProperty(nameof(Properties.Configuration), Properties.Configuration.ToString());
            Project.Xml.AddProperty(nameof(Properties.OutPutType), Properties.OutPutType.ToString());
            Project.Xml.AddProperty(nameof(Properties.Optimize), Properties.Optimize.ToString().ToLower());
            if (Properties.OutDir != null && Properties.OutDir.Length > 0) Project.Xml.AddProperty(nameof(Properties.OutDir), Properties.OutDir);
            if (Properties.ImplicitUsings != null && Properties.ImplicitUsings.Length > 0) Project.Xml.AddProperty(nameof(Properties.ImplicitUsings), Properties.ImplicitUsings);
            if (Properties.Nullable != null && Properties.Nullable.Length > 0) Project.Xml.AddProperty(nameof(Properties.Nullable), Properties.Nullable);
            Project.Xml.AddProperty(nameof(Properties.AllowUnsafeBlocks), Properties.AllowUnsafeBlocks.ToString().ToLower());
            Project.Xml.AddProperty(nameof(Properties.TieredPGO), Properties.TieredPGO.ToString().ToLower());
            if (Properties.CscToolPath != null && Properties.CscToolPath.Length > 0) Project.Xml.AddProperty(nameof(Properties.CscToolPath), Properties.CscToolPath);
            foreach (var kv in Properties.OtherProperties.Where(kv => kv.Value != null && kv.Value.Length > 0)) {
                Project.Xml.AddProperty(kv.Key, kv.Value);
            }
        }
        public override void LoadFromExistingFile() {
            FromFile = true;
            Project = new Microsoft.Build.Evaluation.Project(ProjectFilePath);
        }

        public override void Load(IProjectReferences references) {
            References = references;
            foreach (var file in References.RefFiles) {
                GetRefFile(file, out var a, out var h);
                Project.Xml.AddItem("Reference", a, h);
            }
        }
        private void GetRefFile(FileInfo file, out string assemblyName, out KeyValuePair<string, string>[] hintpath) {
            assemblyName = AssemblyName.GetAssemblyName(file.FullName).Name ?? throw new Exception();
            var path = Path.GetRelativePath(ProjectFile.DirectoryName ?? throw new Exception(), file.FullName);
            hintpath = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("HintPath", path) };
        }

        public override void Save() {
            Project.Save(ProjectFile.FullName);
        }
        private class DefaultProperties : IProjectProperties
        {
            public string TargetFramework => "net6.0";

            public string? LangVersion => null;

            public Configuration Configuration => Configuration.Debug;

            public OutPutType OutPutType => OutPutType.Library;
        }
        private class DefaultReferences : IProjectReferences
        {
            public IEnumerable<FileInfo> RefFiles { get; } = Array.Empty<FileInfo>();
        }

        public override bool Build(CompileLogger logger) {
            if (!FromFile) {
                if (Properties == null) {
                    Load(new DefaultProperties());
                }
                if (References == null) {
                    Load(new DefaultReferences());
                }
            }
            Builded = false;
            //if (!Debugger.IsAttached) {
            //    Debugger.Launch();
            //}
            var objDir = Path.Combine(ProjectFile.DirectoryName, "obj");
            Directory.CreateDirectory(objDir);
            if (!File.Exists(Path.Combine(objDir, "project.assets.json"))) {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c dotnet restore \"{ProjectFile.FullName}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                //string output = process.StandardOutput.ReadToEnd();
                //Console.WriteLine(output);
                process.WaitForExit();
            }
            return Builded = Project.Build((MSCompileLogger)logger);
        }
    }
}
