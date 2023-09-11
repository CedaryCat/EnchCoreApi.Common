namespace EnchCoreApi.Common.CSharp {
    public interface ICSharpCodeLineCollection : IEnumerable<string>, IEnumerable<CSharpClassBuilder> {
        public int CodeLinesCount { get; }
        public int ClassesCount { get; }
        public CSharpClassBuilder GetClassFromFileName(string className);
        public string GetCodeLineFromCodeLineIndex(string className, int index);
        public void Reset();

    }
}
