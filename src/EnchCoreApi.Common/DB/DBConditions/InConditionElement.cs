using EnchCoreApi.Common.DB.Core;

namespace EnchCoreApi.Common.DB.DBConditions {
    public class InConditionElement : IConditionElement {
        public Value[] Value { get; protected set; }
        public Column Column { get; protected set; }
        public InConditionElement(Column column, params Value[] value) {
            Column = column;
            Value = value;
        }
        public virtual string SerializeToText() {
            return $"{Value[0].Column.Name} IN ({string.Join(",", Value.Select(v => v.Serialize()))})";
        }
    }
}
