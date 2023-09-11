using EnchCoreApi.Common.DB.DBProvider;

namespace EnchCoreApi.Common.DB.Core {
    public abstract class Where<RowType> : IWhere<RowType> where RowType : new() {
        protected DBAccessAbstractProvider Provider { get; set; }
        public Table Table { get; private set; }
        public Where(Table table, DBAccessAbstractProvider provider) {
            Table = table;
            Provider = provider;
        }

        #region OperateMethod
        public int Delete() {
            return Provider.DeleteRow(this);
        }

        public int Update(RowType row) {
            if (row == null) {
                throw new ArgumentNullException(nameof(row));
            }
            Value[] values = new Value[Table.Length];
            int i = 0;
            foreach (var column in Table) {
                values[i] = new DBValue(column, column.Getter(row), Provider.DBFieldAccessor, column.RealType);
                i++;
            }
            return Provider.UpdateRow(this, values);
        }

        public int Update<T1>(string column1, T1 value1) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1));
        }

        public int Update<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1), new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2));
        }

        public int Update<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1), new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2), new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3));
        }

        public int Update<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1), new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2), new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3), new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4));
        }

        public int Update<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1), new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2), new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3), new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4), new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5));
        }

        public int Update<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1), new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2), new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3), new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4), new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5), new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6));
        }

        public int Update<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1), new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2), new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3), new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4), new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5), new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6), new DBValue<T7>(Table[column7], Provider.DBFieldAccessor, value7));
        }

        public int Update<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1), new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2), new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3), new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4), new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5), new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6), new DBValue<T7>(Table[column7], Provider.DBFieldAccessor, value7), new DBValue<T8>(Table[column8], Provider.DBFieldAccessor, value8));
        }

        public int Update<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1), new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2), new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3), new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4), new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5), new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6), new DBValue<T7>(Table[column7], Provider.DBFieldAccessor, value7), new DBValue<T8>(Table[column8], Provider.DBFieldAccessor, value8), new DBValue<T9>(Table[column9], Provider.DBFieldAccessor, value9));
        }

        public int Update<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            return Provider.UpdateRow(this, new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1), new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2), new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3), new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4), new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5), new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6), new DBValue<T7>(Table[column7], Provider.DBFieldAccessor, value7), new DBValue<T8>(Table[column8], Provider.DBFieldAccessor, value8), new DBValue<T9>(Table[column9], Provider.DBFieldAccessor, value9), new DBValue<T10>(Table[column10], Provider.DBFieldAccessor, value10));
        }
        /// <summary>
        /// it will return null if is no query result
        /// </summary>
        /// <returns></returns>
        public Row<RowType>? SelectAll() {
            var reader = Provider.SelectAll(this);
            if (reader.Read()) {
                return new Row<RowType>(Table, reader, true);
            }
            reader.Dispose();
            return null;
        }

        public RowCollection<RowType> SelectAllAndAllRecord() {
            return new RowCollection<RowType>(Table, Provider.SelectAll(this));
        }

        public T SelectMax<T>(string column) {
            var c = Table[column];
            var reader = Provider.SelectMax(this, c);
            if (reader.Read()) {
                return new DBValue<T>(new Column($"Max({column})", 0, c.RealType, null), Provider.DBFieldAccessor, reader.Reader).Deserialize();
            }
            throw new NotImplementedException("No Row Founded");
        }

        public T SelectMin<T>(string column) {
            var c = Table[column];
            var reader = Provider.SelectMin(this, c);
            if (reader.Read()) {
                return new DBValue<T>(new Column($"Min({column})", 0, c.RealType, null), Provider.DBFieldAccessor, reader.Reader).Deserialize();
            }
            throw new NotImplementedException("No Row Founded");
        }

        public T SelectSum<T>(string column) {
            var c = Table[column];
            var reader = Provider.SelectSum(this, c);
            if (reader.Read()) {
                return new DBValue<T>(new Column($"Sum({column})", 0, c.RealType, null), Provider.DBFieldAccessor, reader.Reader).Deserialize();
            }
            throw new NotImplementedException("No Row Founded");
        }

        public double SelectAverage(string column) {
            var c = Table[column];
            var reader = Provider.SelectAverage(this, c);
            if (reader.Read()) {
                return new DBValue<double>(new Column($"Avg({column})", 0, c.RealType, null), Provider.DBFieldAccessor, reader.Reader).Deserialize();
            }
            throw new NotImplementedException("No Row Founded");
        }
        #endregion

        public virtual string SerializeFullStatement() {
            return (this as IWhere<RowType>).IsEmpty ? "" : $"WHERE {(this as IWhere<RowType>).SerializeContent()}";
        }

        #region Interface
        bool IWhere<RowType>.IsEmpty => throw new NotImplementedException();

        IWhere<RowType> IWhere<RowType>.AndWhere<T1>(string column1, T1 value1) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere(IWhere<RowType> where) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1>(string column1, T1 value1) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndWhereIn<T>(string column, params T[] values) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToMax(string column, IWhere<RowType> scope) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToMin(string column, IWhere<RowType> scope) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToAvg(string column, IWhere<RowType> scope) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToSum(string column, IWhere<RowType> scope) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrWhereIn<T>(string column, params T[] values) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToMax(string column, IWhere<RowType> scope) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToMin(string column, IWhere<RowType> scope) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToAvg(string column, IWhere<RowType> scope) {
            throw new NotImplementedException();
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToSum(string column, IWhere<RowType> scope) {
            throw new NotImplementedException();
        }

        string IWhere<RowType>.SerializeContent() {
            throw new NotImplementedException();
        }
        #endregion
    }
}
