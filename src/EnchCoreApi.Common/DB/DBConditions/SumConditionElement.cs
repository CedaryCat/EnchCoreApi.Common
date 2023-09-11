using EnchCoreApi.Common.DB.Core;

namespace EnchCoreApi.Common.DB.DBConditions {
    public class SumConditionElement<RowType> : IConditionElement where RowType : new() {
        public Column Column { get; protected set; }
        public Table Table { get; protected set; }
        public IWhere<RowType> Scope { get; protected set; }

        public SumConditionElement(Column column, Table table, IWhere<RowType> sumScope = null) {
            Column = column;
            Table = table;
            Scope = sumScope;
        }
        public virtual string SerializeToText() {
            return $"{Column.Name}=(Select Sum({Column.Name}) from {(Scope?.IsEmpty ?? true ? Table.Name : $"(Select * from {Table.Name} where {Scope.SerializeContent()})")})";
        }
    }
}
