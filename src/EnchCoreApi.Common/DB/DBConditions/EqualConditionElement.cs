using EnchCoreApi.Common.DB.Core;

namespace EnchCoreApi.Common.DB.DBConditions
{
    public class EqualConditionElement : IConditionElement
    {
        public Value Value { get; protected set; }
        public EqualConditionElement(Value value) {
            Value = value;
        }
        public virtual string GetPlainStatement() {
            return $"{Value.Column.Name}={Value.GetStatementPlainParam()}";
        }
        public string GetStatement(ref ICollection<object?> paramCollector) {
            paramCollector.Add(Value.GetStatementParam());
            return $"{Value.Column.Name}=@p{paramCollector.Count}";
        }
    }
}
