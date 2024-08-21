using EnchCoreApi.Common.DB.Core;
using EnchCoreApi.Common.DB.DBAttribute;
using EnchCoreApi.Common.DB.DBProvider;
using EnchCoreApi.Common.Dynamic;
using System.Reflection;

namespace EnchCoreApi.Common.DB
{
    public class TableService<RowType> where RowType : new()
    {
        protected DBAccessAbstractProvider Provider { get; set; }
        public Table Table { get; protected set; }
        private TableService(DBAccessAbstractProvider provider, Table table) {
            Provider = provider;
            Table = table;
        }
        public static TableService<RowType> CreateTableService(DBAccessAbstractProvider provider, string tableName) {
            var type = typeof(RowType);
            var typeInfo = type.GetTypeInfo();

            var onlyColumn = type.GetCustomAttribute<ColumnOnlyAttribute>() != null;
            var columnList = new List<Column>();

            static Column ColumnSetting(Column column, MemberInfo member) {
                column.Unique = member.GetCustomAttribute<ColumnUniqueAttribute>() is not null;
                column.Primary = member.GetCustomAttribute<ColumnPrimaryAttribute>() is not null;
                column.NotNull = member.GetCustomAttribute<ColumnNotNullAttribute>() is not null;
                column.AutoIncrement = member.GetCustomAttribute<ColumnAutoIncrementAttribute>() is not null;
                column.DefaultCurrentTimestamp = member.GetCustomAttribute<ColumnDefaultCurrentTimestampAttribute>() is not null;
                column.DefaultValue = member.GetCustomAttribute<ColumnDefaultValueAttribute>()?.Default;
                column.JsonStorage = member.GetCustomAttribute<ColumnJsonStorageAttribute>() is not null;
                column.Length = member.GetCustomAttribute<ColumnLengthAttribute>()?.Length;
                return column;
            }
            int i = 0;
            foreach (var prop in typeInfo.GetRuntimeProperties()) {
                if (prop.GetMethod is null || prop.SetMethod is null)
                    continue;
                if (!prop.GetMethod.IsPublic || !prop.SetMethod.IsPublic || prop.GetMethod.IsStatic || prop.SetMethod.IsStatic)
                    continue;
                if (prop.GetCustomAttribute<ColumnIgnoreAttribute>() is not null)
                    continue;
                if (onlyColumn && prop.GetCustomAttribute<ColumnIgnoreAttribute>() is null)
                    continue;
                columnList.Add(ColumnSetting(new Column(prop.GetCustomAttribute<ColumnNameAttribute>()?.Name ?? prop.Name, i++, prop.PropertyType, ValueAccessor.CreateAccessor(prop), provider.DBFieldAccessor), prop));
            }
            var fields = typeInfo.GetRuntimeFields();
            foreach (var field in fields) {
                if (!field.IsPublic || field.IsStatic)
                    continue;
                if (field.GetCustomAttribute<ColumnIgnoreAttribute>() is not null)
                    continue;
                if (onlyColumn && field.GetCustomAttribute<ColumnIgnoreAttribute>() is null)
                    continue;
                columnList.Add(ColumnSetting(new Column(field.GetCustomAttribute<ColumnNameAttribute>()?.Name ?? field.Name, i++, field.FieldType, ValueAccessor.CreateAccessor(field), provider.DBFieldAccessor), field));
            }
            Table table = new Table(tableName, columnList);
            return new TableService<RowType>(provider, table);
        }

        public static TableService<RowType> CreateTableService(DBAccessAbstractProvider provider) {
            var type = typeof(RowType);
            return CreateTableService(provider, type.GetCustomAttribute<TableNameAttribute>()?.Name ?? type.Name);
        }
        public void EnsureTableStructure() {
            Provider.EnsureTableStructure(Table);
        }

        #region Where
        public WhereTermiNode<RowType> Where() {
            return Provider.NewInitialWhere<RowType>(Table);
        }
        public WhereTermiNode<RowType> Where<T1>(string column1, T1 value1) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1);
        }
        public WhereTermiNode<RowType> Where<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1, column2, value2);
        }
        public WhereTermiNode<RowType> Where<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1, column2, value2, column3, value3);
        }
        public WhereTermiNode<RowType> Where<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4);
        }
        public WhereTermiNode<RowType> Where<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5);
        }
        public WhereTermiNode<RowType> Where<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6);
        }
        public WhereTermiNode<RowType> Where<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7);
        }
        public WhereTermiNode<RowType> Where<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8);
        }
        public WhereTermiNode<RowType> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9);
        }
        public WhereTermiNode<RowType> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            return Provider.NewInitialWhere<RowType>(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9, column10, value10);
        }
        #endregion

        #region Min,Max,Avg,Sum
        public T SelectMax<T>(string column) {
            return SelectMax<T>(column, Provider.NewInitialWhere<RowType>(Table));
        }
        public T SelectMax<T>(string column, IWhere<RowType> where) {
            var c = Table[column];
            var reader = Provider.SelectMax(where, c);
            if (reader.Read()) {
                return new DBValue<T>(new Column($"Max({column})", 0, c.RealType, null), Provider.DBFieldAccessor, reader.Reader).GetTypedValue();
            }
            throw new NotImplementedException("No Row Founded");
        }


        public T SelectMin<T>(string column) {
            return SelectMin<T>(column, Provider.NewInitialWhere<RowType>(Table));
        }
        public T SelectMin<T>(string column, IWhere<RowType> where) {
            var c = Table[column];
            var reader = Provider.SelectMin(where, c);
            if (reader.Read()) {
                return new DBValue<T>(new Column($"Min({column})", 0, c.RealType, null), Provider.DBFieldAccessor, reader.Reader).GetTypedValue();
            }
            throw new NotImplementedException("No Row Founded");
        }


        public double SelectAvg(string column) {
            return SelectAvg(column, Provider.NewInitialWhere<RowType>(Table));
        }
        public double SelectAvg(string column, IWhere<RowType> where) {
            var c = Table[column];
            var reader = Provider.SelectAverage(where, c);
            if (reader.Read()) {
                return new DBValue<double>(new Column($"Avg({column})", 0, c.RealType, null), Provider.DBFieldAccessor, reader.Reader).GetTypedValue();
            }
            throw new NotImplementedException("No Row Founded");
        }


        public T SelectSum<T>(string column) {
            return SelectSum<T>(column, Provider.NewInitialWhere<RowType>(Table));
        }
        public T SelectSum<T>(string column, IWhere<RowType> where) {
            var c = Table[column];
            var reader = Provider.SelectSum(where, c);
            if (reader.Read()) {
                return new DBValue<T>(new Column($"Sum({column})", 0, c.RealType, null), Provider.DBFieldAccessor, reader.Reader).GetTypedValue();
            }
            throw new NotImplementedException("No Row Founded");
        }
        #endregion

        #region TableMethod
        public bool DropTable() {
            return Provider.DropTable(Table.Name);
        }
        public int Insert(RowType row) {
            Value[] values = new Value[Table.Length];
            int i = 0;
            foreach (var column in Table) {
                values[i] = new DBValue(column, column.Getter(row), Provider.DBFieldAccessor, column.RealType);
                i++;
            }
            return Provider.InsertRow(Table, values);
        }

        public void AsyncInsert(RowType row, Action<int> callback) {
            //ThreadPool.QueueUserWorkItem
            Provider.AppendAsyncAction(() => {
                Value[] values = new Value[Table.Length];
                int i = 0;
                foreach (var column in Table) {
                    values[i] = new DBValue(column, column.Getter(row), Provider.DBFieldAccessor, column.RealType);
                    i++;
                }
                callback(Provider.InsertRow(Table, values));
            });
        }
        #endregion
    }
}
