using EnchCoreApi.Common.DB.DBProvider;
using System.Text;

namespace EnchCoreApi.Common.DB.Core {
    public class WhereInteriNode<RowType> : Where<RowType>, IWhere<RowType> where RowType : new() {
        public bool IsEmpty => Nodes.Count == 0;
        public List<IWhere<RowType>> Nodes { get; private set; } = new List<IWhere<RowType>>();
        protected virtual Func<Table, WhereTermiNode<RowType>> WhereTermiNode => (t) => new WhereTermiNode<RowType>(t, Provider);
        public WhereInteriNode(Table table, DBAccessAbstractProvider provider) : base(table, provider) {
        }

        #region OrWhere
        public WhereInteriNode<RowType> OrWhere(IWhere<RowType> where) {
            if (where.IsEmpty) {
                return this;
            }
            Nodes.Add(where);
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1>(string column1, T1 value1) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1));
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1, column2, value2));
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1, column2, value2, column3, value3));
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4));
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5));
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6));
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7));
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8));
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9));
            return this;
        }

        public WhereInteriNode<RowType> OrWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            OrWhere(WhereTermiNode(Table).AndWhere(column1, value1, column2, value2, column3, value3, column4, value4, column5, value5, column6, value6, column7, value7, column8, value8, column9, value9, column10, value10));
            return this;
        }

        public WhereInteriNode<RowType> OrWhereIn<T>(string column, params T[] values) {
            OrWhere(WhereTermiNode(Table).AndWhere(column, values));
            return this;
        }

        public WhereInteriNode<RowType> OrEqualToMax(string column, IWhere<RowType> scope) {
            OrWhere(WhereTermiNode(Table).AndEqualToMax(column, scope));
            return this;
        }

        public WhereInteriNode<RowType> OrEqualToMin(string column, IWhere<RowType> scope) {
            OrWhere(WhereTermiNode(Table).AndEqualToMin(column, scope));
            return this;
        }

        public WhereInteriNode<RowType> OrEqualToAvg(string column, IWhere<RowType> scope) {
            OrWhere(WhereTermiNode(Table).AndEqualToAvg(column, scope));
            return this;
        }

        public WhereInteriNode<RowType> OrEqualToSum(string column, IWhere<RowType> scope) {
            OrWhere(WhereTermiNode(Table).AndEqualToSum(column, scope));
            return this;
        }
        #endregion

        #region Interface

        #region AndWhere
        IWhere<RowType> IWhere<RowType>.AndWhere<T1>(string column1, T1 value1) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2>(string column1, T1 value1, string column2, T2 value2) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7, T8>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhere<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string column1, T1 value1, string column2, T2 value2, string column3, T3 value3, string column4, T4 value4, string column5, T5 value5, string column6, T6 value6, string column7, T7 value7, string column8, T8 value8, string column9, T9 value9, string column10, T10 value10) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndWhereIn<T>(string column, params T[] values) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToMax(string column, IWhere<RowType> scope) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToMin(string column, IWhere<RowType> scope) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToAvg(string column, IWhere<RowType> scope) {
            return this;
        }

        IWhere<RowType> IWhere<RowType>.AndEqualToSum(string column, IWhere<RowType> scope) {
            return this;
        }
        #endregion

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

        IWhere<RowType> IWhere<RowType>.OrWhereIn<T>(string column, params T[] values) {
            return OrWhereIn(column, values);
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToMax(string column, IWhere<RowType> scope) {
            return OrEqualToMax(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToMin(string column, IWhere<RowType> scope) {
            return OrEqualToMin(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToAvg(string column, IWhere<RowType> scope) {
            return OrEqualToAvg(column, scope);
        }

        IWhere<RowType> IWhere<RowType>.OrEqualToSum(string column, IWhere<RowType> scope) {
            return OrEqualToSum(column, scope);
        }
        #endregion

        public virtual string GetPlainStatementContent() {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Nodes.Count; i++) {
                stringBuilder.Append('(');
                stringBuilder.Append(Nodes[i].GetPlainStatementContent());
                stringBuilder.Append(')');
                if (i != Nodes.Count - 1) {
                    stringBuilder.Append(" or ");
                }
            } 
            return stringBuilder.ToString();
        }

        public virtual string GetStatementContent(ref ICollection<object?> paramCollector) {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Nodes.Count; i++) {
                stringBuilder.Append('(');
                stringBuilder.Append(Nodes[i].GetStatementContent(ref paramCollector));
                stringBuilder.Append(')');
                if (i != Nodes.Count - 1) {
                    stringBuilder.Append(" or ");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
