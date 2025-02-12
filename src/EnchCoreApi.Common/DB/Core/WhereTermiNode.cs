using EnchCoreApi.Common.DB.DBConditions;
using EnchCoreApi.Common.DB.DBProvider;
using System.Text;

namespace EnchCoreApi.Common.DB.Core
{
    public class WhereTermiNode<RowType> : Where<RowType>, IWhere<RowType> where RowType : notnull, new()
    {
        public bool IsEmpty => Leaf.Count == 0;
        public List<IConditionElement> Leaf { get; private set; } = new List<IConditionElement>();
        protected virtual Func<Value, EqualConditionElement> EqualCondition => (value) => new EqualConditionElement(value);
        protected virtual Func<Column, Value[], InConditionElement> InCondition => (c, values) => new InConditionElement(c, values);
        protected virtual Func<Column, Table, IWhere<RowType>?, MaxConditionElement<RowType>> MaxCondition => (c, t, s) => new MaxConditionElement<RowType>(c, t, s);
        protected virtual Func<Column, Table, IWhere<RowType>?, MinConditionElement<RowType>> MinCondition => (c, t, s) => new MinConditionElement<RowType>(c, t, s);
        protected virtual Func<Column, Table, IWhere<RowType>?, AvgConditionElement<RowType>> AvgCondition => (c, t, s) => new AvgConditionElement<RowType>(c, t, s);
        protected virtual Func<Column, Table, IWhere<RowType>?, SumConditionElement<RowType>> SumCondition => (c, t, s) => new SumConditionElement<RowType>(c, t, s);
        protected virtual Func<Table, WhereInteriNode<RowType>> WhereInteriNode => (t) => new WhereInteriNode<RowType>(t, Provider);

        public WhereTermiNode(Table table, DBAccessAbstractProvider provider) : base(table, provider) {
        }

        #region AndWhere

        public WhereTermiNode<RowType> AndWhere<T1>(string column1, T1 value1) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            Leaf.Add(EqualCondition(new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            Leaf.Add(EqualCondition(new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2)));
            Leaf.Add(EqualCondition(new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            Leaf.Add(EqualCondition(new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2)));
            Leaf.Add(EqualCondition(new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3)));
            Leaf.Add(EqualCondition(new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            Leaf.Add(EqualCondition(new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2)));
            Leaf.Add(EqualCondition(new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3)));
            Leaf.Add(EqualCondition(new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4)));
            Leaf.Add(EqualCondition(new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            Leaf.Add(EqualCondition(new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2)));
            Leaf.Add(EqualCondition(new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3)));
            Leaf.Add(EqualCondition(new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4)));
            Leaf.Add(EqualCondition(new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5)));
            Leaf.Add(EqualCondition(new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            Leaf.Add(EqualCondition(new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2)));
            Leaf.Add(EqualCondition(new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3)));
            Leaf.Add(EqualCondition(new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4)));
            Leaf.Add(EqualCondition(new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5)));
            Leaf.Add(EqualCondition(new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6)));
            Leaf.Add(EqualCondition(new DBValue<T7>(Table[column7], Provider.DBFieldAccessor, value7)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            Leaf.Add(EqualCondition(new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2)));
            Leaf.Add(EqualCondition(new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3)));
            Leaf.Add(EqualCondition(new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4)));
            Leaf.Add(EqualCondition(new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5)));
            Leaf.Add(EqualCondition(new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6)));
            Leaf.Add(EqualCondition(new DBValue<T7>(Table[column7], Provider.DBFieldAccessor, value7)));
            Leaf.Add(EqualCondition(new DBValue<T8>(Table[column8], Provider.DBFieldAccessor, value8)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            Leaf.Add(EqualCondition(new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2)));
            Leaf.Add(EqualCondition(new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3)));
            Leaf.Add(EqualCondition(new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4)));
            Leaf.Add(EqualCondition(new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5)));
            Leaf.Add(EqualCondition(new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6)));
            Leaf.Add(EqualCondition(new DBValue<T7>(Table[column7], Provider.DBFieldAccessor, value7)));
            Leaf.Add(EqualCondition(new DBValue<T8>(Table[column8], Provider.DBFieldAccessor, value8)));
            Leaf.Add(EqualCondition(new DBValue<T9>(Table[column9], Provider.DBFieldAccessor, value9)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            Leaf.Add(EqualCondition(new DBValue<T1>(Table[column1], Provider.DBFieldAccessor, value1)));
            Leaf.Add(EqualCondition(new DBValue<T2>(Table[column2], Provider.DBFieldAccessor, value2)));
            Leaf.Add(EqualCondition(new DBValue<T3>(Table[column3], Provider.DBFieldAccessor, value3)));
            Leaf.Add(EqualCondition(new DBValue<T4>(Table[column4], Provider.DBFieldAccessor, value4)));
            Leaf.Add(EqualCondition(new DBValue<T5>(Table[column5], Provider.DBFieldAccessor, value5)));
            Leaf.Add(EqualCondition(new DBValue<T6>(Table[column6], Provider.DBFieldAccessor, value6)));
            Leaf.Add(EqualCondition(new DBValue<T7>(Table[column7], Provider.DBFieldAccessor, value7)));
            Leaf.Add(EqualCondition(new DBValue<T8>(Table[column8], Provider.DBFieldAccessor, value8)));
            Leaf.Add(EqualCondition(new DBValue<T9>(Table[column9], Provider.DBFieldAccessor, value9)));
            Leaf.Add(EqualCondition(new DBValue<T10>(Table[column10], Provider.DBFieldAccessor, value10)));
            return this;
        }

        public WhereTermiNode<RowType> AndWhereIn<T>(string column, params T[] values) {
            var c = Table[column];
            var vs = new DBValue<T>[values.Length];
            for (int i = 0; i < values.Length; i++) {
                vs[i] = new DBValue<T>(c, Provider.DBFieldAccessor, values[i]);
            }
            Leaf.Add(InCondition(c, vs));
            return this;
        }

        public WhereTermiNode<RowType> AndEqualToMax(string column, IWhere<RowType>? scope = null) {
            Leaf.Add(MaxCondition(Table[column], Table, scope));
            return this;
        }

        public WhereTermiNode<RowType> AndEqualToMin(string column, IWhere<RowType>? scope = null) {
            Leaf.Add(MinCondition(Table[column], Table, scope));
            return this;
        }

        public WhereTermiNode<RowType> AndEqualToAvg(string column, IWhere<RowType>? scope = null) {
            Leaf.Add(MaxCondition(Table[column], Table, scope));
            return this;
        }

        public WhereTermiNode<RowType> AndEqualToSum(string column, IWhere<RowType>? scope = null) {
            Leaf.Add(MinCondition(Table[column], Table, scope));
            return this;
        }

        #endregion

        #region OrWhere

        public WhereInteriNode<RowType> OrWhere(IWhere<RowType> where) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(where);
        }

        public WhereInteriNode<RowType> OrWhere<T1>(string column1, T1 value1) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1));
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1, column2, value2));
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1, column2, value2, column3, value3));
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4));
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5));
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6));
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7));
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8));
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9));
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9, column10, value10));
        }

        public WhereInteriNode<RowType> OrWhereIn<T>(string column, params T[] values) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndWhereIn(column, values));
        }

        public WhereInteriNode<RowType> OrEqualToMax(string column, IWhere<RowType>? scope) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndEqualToMax(column));
        }

        public WhereInteriNode<RowType> OrEqualToMin(string column, IWhere<RowType>? scope) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndEqualToMin(column));
        }

        public WhereInteriNode<RowType> OrEqualToAvg(string column, IWhere<RowType>? scope) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndEqualToAvg(column));
        }

        public WhereInteriNode<RowType> OrEqualToSum(string column, IWhere<RowType>? scope) {
            return WhereInteriNode(Table).OrWhere(this).OrWhere(new WhereTermiNode<RowType>(Table, Provider).AndEqualToSum(column));
        }

        #endregion

        #region Interface

        IWhere<RowType> IWhere<RowType>.AndWhere<T1>(string column1, T1 value1) {
            return AndWhere<T1>(column1, value1);
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            return AndWhere(column1, value1, column2, value2);
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            return AndWhere(column1, value1, column2, value2, column3, value3);
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            return AndWhere(column1, value1, column2, value2, column3, value3, column4, value4);
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            return AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5);
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            return AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6);
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            return AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7);
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            return AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8);
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            return AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9);
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            return AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9, column10, value10);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere(IWhere<RowType> where) {
            return OrWhere(where);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1>(string column1, T1 value1) {
            return OrWhere(column1, value1);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            return OrWhere(column1, value1, column2, value2);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            return OrWhere(column1, value1, column2, value2, column3, value3);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            return OrWhere(column1, value1, column2, value2, column3, value3, column4, value4);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            return OrWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            return OrWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            return OrWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            return OrWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            return OrWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9);
        }

        IWhere<RowType> IWhere<RowType>.OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            return OrWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9, column10, value10);
        }

        IWhere<RowType> IWhere<RowType>.AndWhereIn<T>(string column, params T[] values) {
            return AndWhereIn(column, values);
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToMax(string column, IWhere<RowType>? scope) {
            return AndEqualToMax(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToMin(string column, IWhere<RowType>? scope) {
            return AndEqualToMin(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToAvg(string column, IWhere<RowType>? scope) {
            return AndEqualToAvg(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToSum(string column, IWhere<RowType>? scope) {
            return AndEqualToSum(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.OrWhereIn<T>(string column, params T[] values) {
            return OrWhereIn(column, values);
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToMax(string column, IWhere<RowType>? scope) {
            return OrEqualToMax(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToMin(string column, IWhere<RowType>? scope) {
            return OrEqualToMin(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToAvg(string column, IWhere<RowType>? scope) {
            return OrEqualToAvg(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToSum(string column, IWhere<RowType>? scope) {
            return OrEqualToSum(column, scope);
        }
        #endregion

        public virtual string GetPlainStatementContent() {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Leaf.Count; i++) {
                stringBuilder.Append(Leaf[i].GetPlainStatement());
                if (i != Leaf.Count - 1) {
                    stringBuilder.Append(" and ");
                }
            }
            return stringBuilder.ToString();
        }

        public virtual string GetStatementContent(ref ICollection<object?> paramCollector) {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Leaf.Count; i++) {
                stringBuilder.Append(Leaf[i].GetStatement(ref paramCollector));
                if (i != Leaf.Count - 1) {
                    stringBuilder.Append(" and ");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
