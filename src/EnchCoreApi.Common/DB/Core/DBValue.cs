using EnchCoreApi.Common.DB.DBVistor;
using EnchCoreApi.Common.Dynamic;
using Newtonsoft.Json;
using System.Data;

namespace EnchCoreApi.Common.DB.Core
{
    public class DBValue<T> : Value
    {
        private readonly IDBFieldAccessor access;
        private readonly T value;
        public override Column Column { get; protected set; }
        public DBValue(Column column, IDBFieldAccessor access, T value) {
            this.value = value;
            Column = column;
            this.access = access;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="reader"></param>
        public DBValue(Column column, IDBFieldAccessor access, IDataReader reader) {
            Column = column;
            this.access = access;
            if (access is ITextStorgeFieldAccessor<T> tv) {
                this.value = tv.DeserializeFromTextContent(access.Get<string>(reader, Column.Ordinal ?? reader.GetOrdinal(Column.Name)));
            }
            else if (Column.JsonStorage) {
                this.value = JsonConvert.DeserializeObject<T>(access.Get<string>(reader, Column.Ordinal ?? reader.GetOrdinal(Column.Name)))!;
            }
            else {
                this.value = access.Get<T>(reader, Column.Ordinal ?? reader.GetOrdinal(Column.Name));
            }
        }

        public override string GetStatementPlainParam() {
            if (access is ITextStorgeFieldAccessor<T> tv) {
                return $"'{tv.SerializeTextContent(value)}'";
            }
            else if (Column.JsonStorage) {
                return $"'{JsonConvert.SerializeObject(value)}'";
            }
            else if (value is string) {
                return $"'{value}'";
            }
            else if (value is bool flag) {
                return $"{flag.GetHashCode()}";
            }
            return this.value?.ToString() ?? "null";
        }
        public override object? GetStatementParam() {
            if (access is ITextStorgeFieldAccessor<T> tv) {
                return tv.SerializeTextContent(value);
            }
            else if (Column.JsonStorage) {
                return JsonConvert.SerializeObject(value);
            }
            else if (value is bool flag) {
                return flag.GetHashCode();
            }
            return value;
        }

        public T GetTypedValue() {
            return value;
        }

        public override object? GetValue() {
            return GetTypedValue();
        }
    }
    public class DBValue : Value
    {
        public override Column Column { get; protected set; }
        private readonly Value typedValue;
        private static readonly Dictionary<Type, Func<object?[], object>> constructs_reader = [];
        private static readonly Dictionary<Type, Func<object?[], object>> constructs_value = [];
        private Type GeniType { get; set; }
        private static void InitConstruct<TGeni>() {
            InitConstruct(typeof(TGeni));
        }
        private static void InitConstruct(Type type) {
            var tc = typeof(Column);
            var tr = typeof(IDataReader);
            var ta = typeof(IDBFieldAccessor);
            constructs_reader.TryAdd(type, Constructor.Create(typeof(DBValue<>).MakeGenericType(type), tc, ta, tr).Creator);
            constructs_value.TryAdd(type, Constructor.Create(typeof(DBValue<>).MakeGenericType(type), tc, ta, type).Creator);
        }
        private static object GetInstance(Column column, object? value, IDBFieldAccessor access, Type genType) {
            if (constructs_value.TryGetValue(genType, out var func)) {
                return func([column, access, value]);
            }
            else {
                InitConstruct(genType);
                return constructs_value[genType]([column, access, value]);
            }
        }
        private static object GetInstance(Column column, IDataReader reader, IDBFieldAccessor access, Type genType) {
            if (constructs_reader.TryGetValue(genType, out var func)) {
                return func(new object[] { column, access, reader });
            }
            else {
                InitConstruct(genType);
                return constructs_reader[genType]([column, access, reader]);
            }
        }
        static DBValue() {
            InitConstruct<string>();
            InitConstruct<byte>();
            InitConstruct<short>();
            InitConstruct<int>();
            InitConstruct<long>();
            InitConstruct<float>();
            InitConstruct<double>();
            InitConstruct<decimal>();
            InitConstruct<DateTime>();
            InitConstruct<bool>();
        }
        public DBValue(Column column, object? value, IDBFieldAccessor access, Type type) {
            Column = column;
            GeniType = type;
            typedValue = (Value)GetInstance(column, value, access, GeniType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="reader"></param>
        public DBValue(IDataReader reader, Column column, IDBFieldAccessor access, Type type) {
            Column = column;
            GeniType = type;
            typedValue = (Value)GetInstance(column, reader, access, GeniType);
        }

        public override object? GetValue() {
            return typedValue.GetValue();
        }

        public override string GetStatementPlainParam() {
            return typedValue.GetStatementPlainParam();
        }

        public override object? GetStatementParam() {
            return typedValue.GetStatementParam();
        }
    }
}
