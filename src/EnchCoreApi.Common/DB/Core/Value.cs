namespace EnchCoreApi.Common.DB.Core {
    public abstract class Value {
        public abstract Column Column { get; protected set; }
        public abstract string GetStatementPlainParam();
        public abstract object? GetStatementParam();
        public abstract object? GetValue();
    }
}
