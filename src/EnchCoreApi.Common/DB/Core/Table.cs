using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace EnchCoreApi.Common.DB.Core
{
    public class Table : TableInfo, IEnumerable<TableColumn>
    {
        class TableColumnDictionary(IReadOnlyDictionary<string, TableColumn> columns) : IReadOnlyDictionary<string, TableColumn>, IReadOnlyDictionary<string, Column>
        {
            public Column this[string key] => columns[key];
            TableColumn IReadOnlyDictionary<string, TableColumn>.this[string key] => columns[key];
            public IEnumerable<string> Keys => columns.Keys;
            public IEnumerable<Column> Values => columns.Values.Cast<Column>();
            public int Count => columns.Count;
            IEnumerable<string> IReadOnlyDictionary<string, TableColumn>.Keys => columns.Keys;
            IEnumerable<TableColumn> IReadOnlyDictionary<string, TableColumn>.Values => columns.Values;
            int IReadOnlyCollection<KeyValuePair<string, TableColumn>>.Count => columns.Count;
            public bool ContainsKey(string key) 
                => columns.ContainsKey(key);
            public IEnumerator<KeyValuePair<string, Column>> GetEnumerator()
                => columns.Select(kvp => new KeyValuePair<string, Column>(kvp.Key, kvp.Value)).GetEnumerator();
            public bool TryGetValue(string key, [MaybeNullWhen(false)] out Column value) {
                if (columns.TryGetValue(key, out var tableColumn)) {
                    value = tableColumn;
                    return true;
                }
                value = null;
                return false;
            }
            bool IReadOnlyDictionary<string, TableColumn>.ContainsKey(string key)
                => columns.ContainsKey(key);
            IEnumerator<KeyValuePair<string, TableColumn>> IEnumerable<KeyValuePair<string, TableColumn>>.GetEnumerator()
                => columns.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() 
                => columns.GetEnumerator();
            bool IReadOnlyDictionary<string, TableColumn>.TryGetValue(string key, [NotNullWhen(true)] out TableColumn? value) 
                => columns.TryGetValue(key, out value);
        }
        private new readonly IReadOnlyDictionary<string, TableColumn> Columns;
        public Table(string name, List<TableColumn> columns) : base(name, new TableColumnDictionary(columns.ToDictionary(c => c.Name, c => c))) {
            Columns = (TableColumnDictionary)base.Columns;
        }
        public new TableColumn this[string column] {
            get => Columns[column];
        }
        public IEnumerator<TableColumn> GetEnumerator() {
            return Columns.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return Columns.Values.GetEnumerator();
        }
    }
}
