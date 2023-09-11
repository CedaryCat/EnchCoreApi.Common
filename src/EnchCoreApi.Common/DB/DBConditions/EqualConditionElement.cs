using EnchCoreApi.Common.DB.Core;

namespace EnchCoreApi.Common.DB.DBConditions {
    public class EqualConditionElement : IConditionElement {
        public Value Value { get; protected set; }
        public EqualConditionElement(Value value) {
            Value = value;
        }
        public virtual string SerializeToText() {
            return $"{Value.Column.Name}={Value.Serialize()}";
        }
    }
}
