namespace EnchCoreApi.Common.CSharp.MSBuild {
    public interface IProjectReferences : IProjectModule {
        public IEnumerable<FileInfo> RefFiles { get; }
    }
}
