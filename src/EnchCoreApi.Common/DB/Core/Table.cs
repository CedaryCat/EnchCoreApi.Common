using System.Collections;

namespace EnchCoreApi.Common.DB.Core
{
    public class Table : IEnumerable<Column>
    {
        private IReadOnlyDictionary<string, Column> Columns { get; set; }
        public string Name { get; private set; }
        public int Length { get; private set; }
        public Table(string name, List<Column> columns) {
            Name = name;
            Length = columns.Count;
            Columns = columns.ToDictionary(c => c.Name, c => c);
        }
        public Column this[string column] {
            get => Columns[column];
        }

        public IEnumerator<Column> GetEnumerator() {
            return Columns.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Columns.Values.GetEnumerator();
        }
    }
}
