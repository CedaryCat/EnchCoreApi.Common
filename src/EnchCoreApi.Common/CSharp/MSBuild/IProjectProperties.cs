namespace EnchCoreApi.Common.CSharp.MSBuild {
    public interface IProjectProperties : IProjectModule {
        public string TargetFramework { get; }
        public string? LangVersion { get; }
        public Configuration Configuration { get; }
        public OutPutType OutPutType { get; }
        public bool Optimize => false;
        public string? OutDir => null;
        public string? ImplicitUsings => "enable";
        public string? Nullable => "enable";
        public bool AllowUnsafeBlocks => false;
        public bool TieredPGO => false;
        public string? CscToolPath => null;
        public IEnumerable<KeyValuePair<string, string?>> OtherProperties => Array.Empty<KeyValuePair<string, string?>>();
    }
    public enum OutPutType {
        Library,
        Exe,
        Module,
        Winexe,
    }
    public enum Configuration {
        Debug,
        Release,
    }
}
