using EnchCoreApi.Common.DB.DBVistor;
using EnchCoreApi.Common.Dynamic;

namespace EnchCoreApi.Common.DB.Core
{
    public class Column
    {
        public string Name;
        public bool Unique;
        public bool Primary;
        public bool AutoIncrement;
        public bool NotNull;
        /// <summary>
        /// Length of the data type, null = default
        /// </summary>
        public string? DefaultValue;
        /// <summary>
        /// Use on DateTime only, if true, sets the default value to the current date when creating the row.
        /// </summary>
        public bool DefaultCurrentTimestamp;

        /// <summary>
        /// Length of the data type, null = default
        /// </summary>
        public int? Length;
        public bool JsonStorage;
        public Type RealType { get; private set; }
        public Type DBType => (JsonStorage || IsTextStorgeValue) ? typeof(string) : RealType;
        private IDBFieldAccessor? dbField;
        public int? Ordinal { get; private set; }
        private bool IsTextStorgeValue => isTextStorageValue ??= typeof(ITextStorgeFieldAccessor<>).MakeGenericType(RealType).IsAssignableFrom(dbField?.GetType());
        private bool? isTextStorageValue;
        public Column(string name, int? ordinal, Type type, IDBFieldAccessor? dbAccess) {
            Name = name;
            Ordinal = ordinal;
            RealType = type;
            dbField = dbAccess;
        }
    }
    public class TableColumn : Column
    {
        public TableColumn(string name, int? ordinal, Type type, ValueAccessor valueAccess, IDBFieldAccessor? dbAccess) : base(name, ordinal, type, dbAccess) {
            value = valueAccess;
            Setter = value.Setter ?? throw new Exception("Can not access setter of table model property");
            Getter = value.Getter ?? throw new Exception("Can not access getter of table model property");
        }
        private readonly ValueAccessor? value;
        /// <summary>
        /// (instance,value)
        /// </summary>
        public Action<object, object?> Setter { get; private set; }
        /// <summary>
        /// (instance,value)
        /// </summary>
        public Func<object, object?> Getter { get; private set; }
    }
    public enum ReflectInfo
    {
        UnKnow,
        FieldInfo,
        PropertyInfo,
    }
}
