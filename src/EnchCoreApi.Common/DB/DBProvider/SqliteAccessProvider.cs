using EnchCoreApi.Common.DB.Core;
using EnchCoreApi.Common.DB.DBVistor;
using EnchCoreApi.Common.Utilities;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace EnchCoreApi.Common.DB.DBProvider
{
    public class SqliteAccessProvider : DBAccessProvider<SqliteConnection> {
        public SqliteAccessProvider(SqliteConnection db) : base(db) {
            DB = db;
        }

        private RandomHelper rand = new RandomHelper();

        private new SqliteConnection DB;
        public sealed override IDBFieldAccessor DBFieldAccessor { get; } = new DefaultFieldAccessor();

        public sealed override IDbConnection Clone() => new SqliteConnection(DB.ConnectionString);

        private int Query(string query) {
            try {
                using var db = Clone();
                db.Open();
                using var com = db.CreateCommand();
                com.CommandText = query;
                return com.ExecuteNonQuery();
            }
            catch (Exception ex) {
                throw new Exception($"Fatal EnchCoreApi.Common initialization exception: failed to connect to database in execute\n{query}\n. See inner exception for details.", ex);
            }
        }

        private class SqliteReaderBackup : QueryReaderBackup {
            private readonly SqliteConnection DB;
            private readonly string CommandText;
            private readonly IDBFieldAccessor FieldAccessor;
            public SqliteReaderBackup(SqliteConnection db, string cmd, IDBFieldAccessor accessor) {
                DB = db;
                CommandText = cmd;
                FieldAccessor = accessor;
            }

            public sealed override QueryReader CreateReader() {
                string query = "";
                try
                {
                    var db = new SqliteConnection(DB.ConnectionString);
                    db.Open();
                    using var com = db.CreateCommand();
                    query = com.CommandText = CommandText;
                    return new QueryReader(db, com.ExecuteReader(), this, FieldAccessor);
                }
                catch (Exception ex) {
                    throw new Exception($"Fatal EnchCoreApi.Common initialization exception: failed to connect to database in execute\n{query}\n. See inner exception for details.", ex);
                }
            }
        }

        private QueryReader QueryReader(string query) {
            try
            {
                var db = Clone();
                db.Open();
                var com = db.CreateCommand();
                com.CommandText = query;
                return new QueryReader(db, com.ExecuteReader(), new SqliteReaderBackup(DB, query, DBFieldAccessor), DBFieldAccessor);
            }
            catch (Exception ex) {
                throw new Exception($"Fatal EnchCoreApi.Common initialization exception: failed to connect to database in execute\n{query}\n. See inner exception for details.", ex);
            }
        }

        public sealed override string[] QueryAllTable() {
            List<string> list = new List<string>();
            using var reader = QueryReader($"SELECT name FROM sqlite_Master");
            while (reader.Read()) {
                list.Add(reader.Get<string>(new Column("name", null, typeof(string), DBFieldAccessor)));
            }
            return list.ToArray();
        }

        public sealed override bool QueryTable(string tableName, out Table table) {
            List<Column> columns = new List<Column>();
            using var reader = QueryReader($"PRAGMA table_info({tableName})");
            while (reader.Read()) {
                columns.Add(new Column(reader.Reader.GetString(reader.Reader.GetOrdinal("name")), reader.Reader.GetInt32(reader.Reader.GetOrdinal("cid")), reader.Reader.GetFieldType(reader.Reader.GetOrdinal("dflt_value")), DBFieldAccessor));
            }
            if (columns.Count > 0) {
                table = new Table(tableName, columns);
                return true;
            }
            table = null;
            return false;
        }

        public sealed override bool DropTable(string table) {
            return Query($"DROP TABLE {table}") > 0;
        }

        public sealed override bool CreateTable(Table table) {
            var columns = table.Select(c =>
            $"'{c.Name}' " +

            $"{DbTypeToString(c.DBType)} {(c.Primary ? "PRIMARY KEY" : "")} " +

            $"{(c.AutoIncrement ? "PRIMARY KEY AUTOINCREMENT" : "")} " +

            $"{(c.NotNull ? "NOT NULL" : "")} " +

            $"{(c.DefaultCurrentTimestamp ? "DEFAULT CURRENT_TIMESTAMP" : "")}");

            var uniques = table.Where(c => c.Unique).Select(c => c.Name);
            return Query($"CREATE TABLE '{table.Name}' ({string.Join(", ", columns)} {(uniques.Count() > 0 ? $", UNIQUE({string.Join(", ", uniques)})" : "")})") > 0;

        }

        private string DbTypeToString(Type type) {
            if (type == typeof(string)) return "TEXT";
            else if (type == typeof(int) || type == typeof(int?)) return "INTEGER";
            else if (type == typeof(bool) || type == typeof(bool?)) return "INTEGER";
            else if (type == typeof(double) || type == typeof(double?)) return "REAL";
            else if (type == typeof(float) || type == typeof(float?)) return "REAL";
            else if (type == typeof(long) || type == typeof(long?)) return "BIGINT";
            else if (type == typeof(short) || type == typeof(short?)) return "INTEGER";
            else if (type == typeof(byte) || type == typeof(byte?)) return "INTEGER";
            else if (type == typeof(decimal) || type == typeof(decimal?)) return "REAL";
            else if (type == typeof(DateTime) || type == typeof(DateTime?)) return "DATETIME";
            else if (type == typeof(byte[])) return "BLOB";
            throw new NotSupportedException($"type {type.Name} is not support in sqlite database field");
        }

        public sealed override bool RenameTable(string tableFrom, string tableTo) {
            return Query($"ALTER TABLE {tableFrom} RENAME TO {tableTo}") > 0;
        }

        public sealed override bool EnsureTableStructure(Table table) {
            if (QueryTable(table.Name, out var old)) {
                if (!table.All(c => old.Select(c => c.Name).Contains(c.Name)) || !old.Select(o => o.Name).All(c => table.Any(c2 => c2.Name == c))) {
                    AlterTableStruct(old, table);
                    return true;
                }
            }
            else {
                return CreateTable(table);
            }
            return false;
        }

        public sealed override bool AlterTableStruct(Table tableFrom, Table tableTo) {
            if (!QueryAllTable().Contains(tableFrom.Name)) {
                return false;
            }
            try {
                string tmpTable = $"{tableFrom.Name}_{rand.NextString(20)}";
                RenameTable(tableFrom.Name, tmpTable);
                CreateTable(tableTo);
                var columns = string.Join(", ", tableFrom.Where(c => tableTo.Any(c2 => c2.Name == c.Name)).Select(c => $"'{c.Name}'"));
                Query($"INSERT INTO {tableFrom.Name} ({columns}) SELECT {columns} FROM {tmpTable}");
                DropTable(tmpTable);
                return true;
            }
            catch {
                return false;
            }
        }

        public sealed override int DeleteRow<RowType>(IWhere<RowType> where) {
            return Query($"DELETE FROM {where.Table.Name} {where.SerializeFullStatement()}");
        }

        public sealed override int InsertRow(Table table, params Value[] values) {
            var sbnames = new StringBuilder();
            var sbvalues = new StringBuilder();
            int count = 0;
            foreach (var value in values) {
                sbnames.Append(value.Column.Name);
                sbvalues.Append(value.Column.AutoIncrement ? "null" : value.Serialize());

                if (count != values.Length - 1) {
                    sbnames.Append(", ");
                    sbvalues.Append(", ");
                }
                count++;
            }
            return Query($"INSERT INTO {table.Name} ({sbnames}) VALUES ({sbvalues})");
        }

        public sealed override int UpdateRow<RowType>(IWhere<RowType> where, params Value[] updates) {
            if (updates.Length == 0)
                throw new ArgumentException("No values supplied");
            return Query($"UPDATE {where.Table.Name} SET {string.Join(", ", updates.Where(v => !v.Column.AutoIncrement).Select(v => v.Column.Name + " = " + v.Serialize()))} {where.SerializeFullStatement()}");
        }

        public sealed override WhereTermiNode<RowType> NewInitialWhere<RowType>(Table table) {
            return new WhereTermiNode<RowType>(table, this);
        }

        public sealed override QueryReader SelectAll<RowType>(IWhere<RowType> where) {
            return QueryReader($"SELECT * FROM {where.Table.Name} {where.SerializeFullStatement()}");
        }

        public sealed override QueryReader SelectAverage<RowType>(IWhere<RowType> where, Column column) {
            return QueryReader($"SELECT AVG({column.Name}) FROM {where.Table.Name} {where.SerializeFullStatement()}");
        }

        public sealed override QueryReader SelectMax<RowType>(IWhere<RowType> where, Column column) {
            return QueryReader($"SELECT MAX({column.Name}) FROM {where.Table.Name} {where.SerializeFullStatement()}");
        }

        public sealed override QueryReader SelectMin<RowType>(IWhere<RowType> where, Column column) {
            return QueryReader($"SELECT MIN({column.Name}) FROM {where.Table.Name} {where.SerializeFullStatement()}");
        }

        public sealed override QueryReader SelectSum<RowType>(IWhere<RowType> where, Column column) {
            return QueryReader($"SELECT SUM({column.Name}) FROM {where.Table.Name} {where.SerializeFullStatement()}");
        }
    }
}
