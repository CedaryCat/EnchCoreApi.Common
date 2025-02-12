using EnchCoreApi.Common.DB.DBVistor;
using EnchCoreApi.Common.Dynamic;

namespace EnchCoreApi.Common.DB.Core
{
    public class Row<RowType> : IDisposable where RowType : notnull, new()
    {
        public Table Table { get; private set; }
        private readonly QueryReader QueryReader;
        private static readonly Func<RowType> func = Constructor<RowType>.Create().NoParamsCreator;
        internal Row(Table table, QueryReader queryreader, bool canDispose) {
            Table = table;
            QueryReader = queryreader;
            CanDispose = canDispose;
        }
        public void Dispose() {
            QueryReader?.Dispose();
        }
        public T Get<T>(string column, bool dispose = true) {
            var value = QueryReader.Get<T>(Table[column]);
            if (dispose && CanDispose)
                Dispose();
            return value;
        }
        public object GetRawData(string column, bool dispose = true) {
            var t = Table[column];
            var r = QueryReader.Reader;
            var v = r.GetValue(t.Ordinal ?? r.GetOrdinal(t.Name));
            if (dispose && CanDispose)
                Dispose();
            return v;
        }
        public RowType Parse(bool dispose = true) {
            var result = func.Invoke();
            foreach (var column in Table) {
                column.Setter(result, new DBValue(QueryReader.Reader, column, QueryReader.FieldAccessor, column.RealType).GetValue());
            }
            if (dispose && CanDispose)
                Dispose();
            return result;
        }
        /// <summary>
        /// It should't be disposed in enumerating.
        /// </summary>
        private readonly bool CanDispose;
    }
}
