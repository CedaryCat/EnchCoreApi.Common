namespace EnchCoreApi.Common.DB.Core {
    public interface IWhere<RowType> where RowType : new() {
        public Table Table { get; }
        public bool IsEmpty { get; }

        #region AndWhere
        IWhere<RowType> AndWhere<T1>(string column1, T1 value1);

        IWhere<RowType> AndWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2);

        IWhere<RowType> AndWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3);

        IWhere<RowType> AndWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4);

        IWhere<RowType> AndWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5);

        IWhere<RowType> AndWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6);

        IWhere<RowType> AndWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7);

        IWhere<RowType> AndWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8);

        IWhere<RowType> AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9);

        IWhere<RowType> AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10);
        #endregion

        #region OrWhere
        IWhere<RowType> OrWhere(IWhere<RowType> where);

        IWhere<RowType> OrWhere<T1>(string column1, T1 value1);

        IWhere<RowType> OrWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2);

        IWhere<RowType> OrWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3);

        IWhere<RowType> OrWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4);

        IWhere<RowType> OrWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5);

        IWhere<RowType> OrWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6);

        IWhere<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7);

        IWhere<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8);

        IWhere<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9);

        IWhere<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10);
        #endregion

        #region Min,Max,Avg,Sum
        IWhere<RowType> AndWhereIn<T>(string column, params T[] values);
        IWhere<RowType> AndEqualToMax(string column, IWhere<RowType> scope = null);
        IWhere<RowType> AndEqualToMin(string column, IWhere<RowType> scope = null);
        IWhere<RowType> AndEqualToAvg(string column, IWhere<RowType> scope = null);
        IWhere<RowType> AndEqualToSum(string column, IWhere<RowType> scope = null);
        IWhere<RowType> OrWhereIn<T>(string column, params T[] values);
        IWhere<RowType> OrEqualToMax(string column, IWhere<RowType> scope = null);
        IWhere<RowType> OrEqualToMin(string column, IWhere<RowType> scope = null);
        IWhere<RowType> OrEqualToAvg(string column, IWhere<RowType> scope = null);
        IWhere<RowType> OrEqualToSum(string column, IWhere<RowType> scope = null);
        #endregion;

        int Delete();

        #region Update
        int Update(RowType row);
        int Update<T1>(string column1, T1 value1);
        int Update<T1, T2>(string column1, T1 value1, string column2, T2 value2);
        int Update<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3);
        int Update<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4);
        int Update<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5);
        int Update<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6);
        int Update<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7);
        int Update<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8);
        int Update<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9);
        int Update<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10);
        #endregion;

        #region Select
        Row<RowType>? SelectAll();
        RowCollection<RowType> SelectAllAndAllRecord();
        T SelectMax<T>(string column);
        T SelectMin<T>(string column);
        T SelectSum<T>(string column);
        double SelectAverage(string column);
        #endregion

        /// <summary>
        /// GetPlainStatement the 'where statement' and it is not including 'WHERE', if this is a Empty 'where statement' it will return string.empty
        /// </summary>
        /// <returns></returns>
        string GetPlainStatementContent();
        /// <summary>
        /// GetPlainStatement the full 'where statement' and it is including 'WHERE', if this is a Empty 'where statement' it will return string.empty
        /// </summary>
        /// <returns></returns>
        string GetPlainStatement();

        string GetStatementContent(ref ICollection<object?> paramCollector);
        string GetStatement(ref ICollection<object?> paramCollector);
    }
}
