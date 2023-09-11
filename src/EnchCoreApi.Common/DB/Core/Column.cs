using EnchCoreApi.Common.DB.DBVistor;
using EnchCoreApi.Common.Dynamic;

namespace EnchCoreApi.Common.DB.Core
{
    public class Column {
        public string Name { get; set; }
        public bool Unique { get; set; }
        public bool Primary { get; set; }
        public bool AutoIncrement { get; set; }
        public bool NotNull { get; set; }
        /// <summary>
        /// Length of the data type, null = default
        /// </summary>
        public string? DefaultValue { get; set; }
        /// <summary>
        /// Use on DateTime only, if true, sets the default value to the current date when creating the row.
        /// </summary>
        public bool DefaultCurrentTimestamp { get; set; }

        /// <summary>
        /// Length of the data type, null = default
        /// </summary>
        public int? Length { get; set; }
        public bool JsonStorage { get; set; }
        public Type RealType { get; private set; }
        public Type DBType => (JsonStorage || IsTextStorgeValue) ? typeof(string) : RealType;
        private ValueAccessor? value { get; set; }
        private IDBFieldAccessor? dbField { get; set; }
        /// <summary>
        /// (instance,value)
        /// </summary>
        public Action<object, object>? Setter { get; private set; }
        /// <summary>
        /// (instance,value)
        /// </summary>
        public Func<object, object>? Getter { get; private set; }
        public int? Ordinal { get; private set; }
        private bool IsTextStorgeValue => isTextStorageValue ??= typeof(ITextStorgeFieldAccessor<>).MakeGenericType(RealType).IsAssignableFrom(dbField?.GetType());
        private bool? isTextStorageValue;
        public Column(string name, int? ordinal, Type type, IDBFieldAccessor? dbAccess) {
            Name = name;
            Ordinal = ordinal;
            RealType = type;
            dbField = dbAccess;
        }
        public Column(string name, int? ordinal, Type type, ValueAccessor valueAccess, IDBFieldAccessor? dbAccess) {
            Name = name;
            Ordinal = ordinal;
            RealType = type;
            value = valueAccess;
            Setter = value.Setter;
            Getter = value.Getter;
            dbField = dbAccess;
        }
    }
    public enum ReflectInfo {
        UnKnow,
        FieldInfo,
        PropertyInfo,
    }
}
