using System.Reflection;

namespace EnchCoreApi.Common.CSharp.MSBuild {
    public abstract class Project : IProjectLoader<IProjectProperties>, IProjectLoader<IProjectReferences> {
        public string ProjectFilePath { get; private set; }
        public FileInfo ProjectFile { get; private set; }
        public abstract IProjectProperties? Properties { get; protected set; }
        public abstract IProjectReferences? References { get; protected set; }
        public abstract string OutPutPath { get; }
        public Project(string path) {
            ProjectFilePath = path;
            ProjectFile = new FileInfo(path);
        }
        public bool Builded { get; protected set; }
        public abstract void LoadFromExistingFile();
        public abstract void Load(IProjectProperties data);
        public abstract void Load(IProjectReferences data);
        public abstract void Save();
        public abstract bool Build(CompileLogger logger);
        public Assembly? GetBuildedAssemblyFile() {
            if (!Builded) return null;
            return Assembly.LoadFile(OutPutPath);
        }
    }
}
