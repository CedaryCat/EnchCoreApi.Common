using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchCoreApi.Common.DB.Core
{
    public class TableInfo
    {
        protected readonly IReadOnlyDictionary<string, Column> Columns;
        public readonly string Name;
        public readonly int Length;
        public TableInfo(string name, List<Column> columns) {
            Name = name;
            Length = columns.Count;
            Columns = columns.ToDictionary(c => c.Name, c => c);
        }
        public TableInfo(string name, IReadOnlyDictionary<string, Column> columns) {
            Name = name;
            Length = columns.Count;
            Columns = columns;
        }
        public Column this[string column] {
            get => Columns[column];
        }
        public IEnumerable<Column> TableColumns => Columns.Values;
    }
}
