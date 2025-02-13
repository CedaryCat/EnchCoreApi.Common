using EnchCoreApi.Common.DB.Core;
using EnchCoreApi.Common.DB.DBVistor;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

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
        /// Creates a root node for a WHERE clause for the specified table.
        /// </summary>
        /// <param name="table">The table to create the WHERE clause for.</param>
        /// <returns>A root node for the WHERE clause.</returns>
        public abstract WhereTermiNode<RowType> CreateWhereRoot<RowType>(Table table) where RowType : notnull, new();
        /// <summary>
        /// Queries all tables in the database.
        /// </summary>
        /// <returns>An array of table names.</returns>
        public abstract string[] QueryAllTable();
        /// <summary>
        /// Queries the structure of a specific table.
        /// </summary>
        /// <param name="tableName">The name of the table to query.</param>
        /// <param name="table">The structure of the table if found.</param>
        /// <returns>True if the table exists, otherwise false.</returns>
        public abstract bool QueryTable(string tableName, [NotNullWhen(true)] out TableInfo? table);
        /// <summary>
        /// Ensures the structure of the specified table matches the database.
        /// </summary>
        /// <param name="table">The table to ensure the structure for.</param>
        /// <returns>True if the structure is ensured, otherwise false.</returns>
        public abstract bool EnsureTableStructure(Table table);
        /// <summary>
        /// Creates a new table in the database.
        /// </summary>
        /// <param name="table">The table to create.</param>
        /// <returns>True if the table is created, otherwise false.</returns>
        public abstract bool CreateTable(Table table);
        /// <summary>
        /// Drops a table from the database.
        /// </summary>
        /// <param name="table">The name of the table to drop.</param>
        /// <returns>True if the table is dropped, otherwise false.</returns>
        public abstract bool DropTable(string table);
        /// <summary>
        /// Renames a table in the database.
        /// </summary>
        /// <param name="tableFrom">The current name of the table.</param>
        /// <param name="tableTo">The new name of the table.</param>
        /// <returns>True if the table is renamed, otherwise false.</returns>
        public abstract bool RenameTable(string tableFrom, string tableTo);
        /// <summary>
        /// Alters the structure of a table.
        /// </summary>
        /// <param name="tableFrom">The current structure of the table.</param>
        /// <param name="tableTo">The new structure of the table.</param>
        /// <returns>True if the table structure is altered, otherwise false.</returns>
        public abstract bool AlterTableStruct(TableInfo tableFrom, Table tableTo);
        /// <summary>
        /// Deletes rows from a table based on a WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause to determine which rows to delete.</param>
        /// <returns>The number of rows deleted.</returns>
        public abstract int DeleteRow<RowType>(IWhere<RowType> where) where RowType : notnull, new();
        /// <summary>
        /// Inserts rows into a table.
        /// </summary>
        /// <param name="table">The table to insert rows into.</param>
        /// <param name="values">The values to insert.</param>
        /// <returns>The number of rows inserted.</returns>
        public abstract int InsertRow(Table table, params Value[] values);
        /// <summary>
        /// Updates rows in a table based on a WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause to determine which rows to update.</param>
        /// <param name="updates">The values to update.</param>
        /// <returns>The number of rows updated.</returns>
        public abstract int UpdateRow<RowType>(IWhere<RowType> where, params Value[] updates) where RowType : notnull, new();
        /// <summary>
        /// Selects the maximum value of a column based on a WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause to determine which rows to consider.</param>
        /// <param name="column">The column to select the maximum value from.</param>
        /// <returns>A QueryReader to read the result.</returns>
        public abstract QueryReader SelectMax<RowType>(IWhere<RowType> where, Column column) where RowType : notnull, new();
        /// <summary>
        /// Selects the minimum value of a column based on a WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause to determine which rows to consider.</param>
        /// <param name="column">The column to select the minimum value from.</param>
        /// <returns>A QueryReader to read the result.</returns>
        public abstract QueryReader SelectMin<RowType>(IWhere<RowType> where, Column column) where RowType : notnull, new();
        /// <summary>
        /// Selects the average value of a column based on a WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause to determine which rows to consider.</param>
        /// <param name="column">The column to select the average value from.</param>
        /// <returns>A QueryReader to read the result.</returns>
        public abstract QueryReader SelectAverage<RowType>(IWhere<RowType> where, Column column) where RowType : notnull, new();
        /// <summary>
        /// Selects the sum of a column based on a WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause to determine which rows to consider.</param>
        /// <param name="column">The column to select the sum from.</param>
        /// <returns>A QueryReader to read the result.</returns>
        public abstract QueryReader SelectSum<RowType>(IWhere<RowType> where, Column column) where RowType : notnull, new();
        /// <summary>
        /// Selects all rows from a table based on a WHERE clause.
        /// </summary>
        /// <param name="where">The WHERE clause to determine which rows to select.</param>
        /// <returns>A QueryReader to read the result.</returns>
        public abstract QueryReader SelectAll<RowType>(IWhere<RowType> where) where RowType : notnull, new();
        /// <summary>
        /// Appends an asynchronous action to be executed.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public virtual void AppendAsyncAction(Action action) {
            _ = channel.Writer.WriteAsync(action);
        }
        private readonly Channel<Action> channel = Channel.CreateUnbounded<Action>();
        public DBAccessAbstractProvider() {
            Task.Run(async () => {
                Thread.CurrentThread.Name = "DBAccessProviderThread";
                while (await channel.Reader.WaitToReadAsync()) {
                    while (channel.Reader.TryRead(out var task)) {
                        try {
                            task();
                        }
                        catch {
                        }
                    }
                }
            });
        }
    }
}
