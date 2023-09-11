using EnchCoreApi.Common.DB.DBVistor;
using EnchCoreApi.Common.Dynamic;
using Newtonsoft.Json;
using System.Data;

namespace EnchCoreApi.Common.DB.Core
{
    public class DBValue<T> : Value {
        private IDBFieldAccessor access;
        private T value;
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
        /// <param name="unknowType">only use in an implicity RealType (T is object)</param>
        public DBValue(Column column, IDBFieldAccessor access, IDataReader reader) {
            Column = column;
            this.access = access;
            if (access is ITextStorgeFieldAccessor<T> tv) {
                this.value = tv.DeserializeFromTextContent(access.Get<string>(reader, Column.Ordinal ?? reader.GetOrdinal(Column.Name)));
            }
            else if (Column.JsonStorage) {
                this.value = JsonConvert.DeserializeObject<T>(access.Get<string>(reader, Column.Ordinal ?? reader.GetOrdinal(Column.Name)));
            }
            else {
                //object[] arr = new object[10];
                //reader.GetValues(arr);
                //var obj=reader.GetValue(0);
                this.value = access.Get<T>(reader, Column.Ordinal ?? reader.GetOrdinal(Column.Name));
            }
        }

        public override string Serialize() {
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

        public T Deserialize() {
            return value;
        }

        public override object DeserializeValueObj() {
            return Deserialize();
        }
    }
    public class DBValue : Value {
        public override Column Column { get; protected set; }
        private Value redirectedValue;
        private static Dictionary<Type, Func<object[], object>> constructs_reader = new Dictionary<Type, Func<object[], object>>();
        private static Dictionary<Type, Func<object[], object>> constructs_value = new Dictionary<Type, Func<object[], object>>();
        private Type GeniType { get; set; }
        private static void InitConstruct<TGeni>() {
            InitConstruct(typeof(TGeni));
        }
        private static void InitConstruct(Type type) {
            var tc = typeof(Column);
            var tr = typeof(IDataReader);
            var ta = typeof(IDBFieldAccessor);
            if (!constructs_reader.ContainsKey(type)) {
                constructs_reader.Add(type, Constructor.Create(typeof(DBValue<>).MakeGenericType(type), tc, ta, tr).Creator);
            }
            if (!constructs_value.ContainsKey(type)) {
                constructs_value.Add(type, Constructor.Create(typeof(DBValue<>).MakeGenericType(type), tc, ta, type).Creator);
            }
        }
        private static object GetInstance(Column column, object value, IDBFieldAccessor access, Type genType) {
            if (constructs_value.TryGetValue(genType, out var func)) {
                return func(new object[] { column, access, value });
            }
            else {
                InitConstruct(genType);
                return constructs_value[genType](new object[] { column, access, value });
            }
        }
        private static object GetInstance(Column column, IDataReader reader, IDBFieldAccessor access, Type genType) {
            if (constructs_reader.TryGetValue(genType, out var func)) {
                return func(new object[] { column, access, reader });
            }
            else {
                InitConstruct(genType);
                return constructs_reader[genType](new object[] { column, access, reader });
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
        public DBValue(Column column, object value, IDBFieldAccessor access, Type type) {
            Column = column;
            GeniType = type;
            redirectedValue = GetInstance(column, value, access, GeniType) as Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="reader"></param>
        public DBValue(IDataReader reader, Column column, IDBFieldAccessor access, Type type) {
            Column = column;
            GeniType = type;
            redirectedValue = GetInstance(column, reader, access, GeniType) as Value;
        }

        public override object DeserializeValueObj() {
            return redirectedValue.DeserializeValueObj();
        }

        public override string Serialize() {
            return redirectedValue.Serialize();
        }
    }
}
