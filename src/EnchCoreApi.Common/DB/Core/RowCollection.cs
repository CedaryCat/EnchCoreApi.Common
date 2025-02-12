using EnchCoreApi.Common.DB.DBVistor;
using System.Collections;

namespace EnchCoreApi.Common.DB.Core
{
    public class RowCollection<RowType> : IEnumerable<Row<RowType>>, IEnumerator<Row<RowType>> where RowType : notnull, new()
    {
        public Table Table { get; private set; }
        private QueryReader QueryReader;
        internal RowCollection(Table table, QueryReader queryReader) {
            this.Table = table;
            this.QueryReader = queryReader;
        }

        public IEnumerator<Row<RowType>> GetEnumerator() {
            current = new Row<RowType>(Table, QueryReader, false);
            return this;
        }

        public void Dispose() {
            current.Dispose();
            QueryReader.Dispose();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }



        public Row<RowType> Current {
            get => current;
        }
        private Row<RowType> current;
        object IEnumerator.Current => current;

        public bool MoveNext() {
            var hasRead = QueryReader.Read();
            if (!hasRead) {
                this.Dispose();
            }
            return hasRead;
        }

        public void Reset() {
            var newReader = QueryReader.Clone();
            QueryReader.Dispose();
            QueryReader = (QueryReader)newReader;
        }
    }
}
