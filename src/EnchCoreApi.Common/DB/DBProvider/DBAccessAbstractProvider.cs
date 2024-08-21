using EnchCoreApi.Common.DB.Core;
using EnchCoreApi.Common.DB.DBVistor;
using System.Collections.Concurrent;
using System.Data;

namespace EnchCoreApi.Common.DB.DBProvider
{
    public abstract class DBAccessAbstractProvider
    {
        public abstract IDbConnection Clone();
        public abstract IDbConnection DbConnection { get; }
        /// <summary>
        /// If this instance implements the ITextStorgeFieldAccessor interface in addition, the generic type specified by ITextStorgeFieldAccessor can be textualized into DBField
        /// </summary>
        public abstract IDBFieldAccessor DBFieldAccessor { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract WhereTermiNode<RowType> NewInitialWhere<RowType>(Table table) where RowType : new();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string[] QueryAllTable();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract bool QueryTable(string tableName, out Table table);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract bool EnsureTableStructure(Table table);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract bool CreateTable(Table table);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract bool DropTable(string table);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract bool RenameTable(string tableFrom, string tableTo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableFrom"></param>
        /// <param name="tableTo"></param>
        /// <returns></returns>
        public abstract bool AlterTableStruct(Table tableFrom, Table tableTo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract int DeleteRow<RowType>(IWhere<RowType> where) where RowType : new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public abstract int InsertRow(Table table, params Value[] values);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        public abstract int UpdateRow<RowType>(IWhere<RowType> where, params Value[] updates) where RowType : new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public abstract QueryReader SelectMax<RowType>(IWhere<RowType> where, Column column) where RowType : new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public abstract QueryReader SelectMin<RowType>(IWhere<RowType> where, Column column) where RowType : new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public abstract QueryReader SelectAverage<RowType>(IWhere<RowType> where, Column column) where RowType : new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public abstract QueryReader SelectSum<RowType>(IWhere<RowType> where, Column column) where RowType : new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public abstract QueryReader SelectAll<RowType>(IWhere<RowType> where) where RowType : new();
        public virtual void AppendAsyncAction(Action action) {
            tasks.Enqueue(action);
        }
        private readonly ConcurrentQueue<Action> tasks = new ConcurrentQueue<Action>();
        public DBAccessAbstractProvider() {
            new Task(() => {
                while (true) {
                    while (tasks.TryDequeue(out var task)) {
                        task();
                    }
                    Thread.Sleep(1);
                }
            }).Start();
        }
    }
}
