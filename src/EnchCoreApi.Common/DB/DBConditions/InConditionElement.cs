using EnchCoreApi.Common.DB.Core;

namespace EnchCoreApi.Common.DB.DBConditions {
    public class InConditionElement : IConditionElement {
        public Value[] Value { get; protected set; }
        public Column Column { get; protected set; }
        public InConditionElement(Column column, params Value[] value) {
            Column = column;
            Value = value;
        }
        public virtual string GetPlainStatement() {
            return $"{Value[0].Column.Name} IN ({string.Join(",", Value.Select(v => v.GetStatementPlainParam()))})";
        }
        public string GetStatement(ref ICollection<object?> paramCollector) {
            int start = paramCollector.Count + 1;
            foreach (var p in Value.Select(v => v.GetStatementParam())) {
                paramCollector.Add(p);
            }
            return $"{Value[0].Column.Name} IN ({string.Join(",", Enumerable.Range(start, Value.Length).Select(i => $"@p{i}"))})";
        }
    }
}
