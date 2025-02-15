﻿using EnchCoreApi.Common.DB.Core;

namespace EnchCoreApi.Common.DB.DBConditions
{
    public class MaxConditionElement<RowType> : IConditionElement where RowType : notnull, new()
    {
        public Column Column { get; protected set; }
        public Table Table { get; protected set; }
        public IWhere<RowType>? Scope { get; protected set; }

        public MaxConditionElement(Column column, Table table, IWhere<RowType>? scope = null) {
            Column = column;
            Table = table;
            Scope = scope;
        }
        public virtual string GetPlainStatement() {
            return $"{Column.Name}=(Select Max({Column.Name}) from {(Scope?.IsEmpty ?? true ? Table.Name : $"(Select * from {Table.Name} where {Scope.GetPlainStatementContent()})")})";
        }
        public string GetStatement(ref ICollection<object?> paramCollector) {
            return $"{Column.Name}=(Select Max({Column.Name}) from {(Scope?.IsEmpty ?? true ? Table.Name : $"(Select * from {Table.Name} where {Scope.GetStatementContent(ref paramCollector)})")})";
        }
    }
}
