using EnchCoreApi.Common.DB.Core;
using EnchCoreApi.Common.DB.DBVistor;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace EnchCoreApi.Common.DB.DBProvider
{
    public class MysqlAccessProvider(MySqlConnection db) : DBAccessProvider<MySqlConnection>(db)
    {
        public override IDbConnection Clone() {
            return new MySqlConnection(DB.ConnectionString);
        }
        public sealed override IDBFieldAccessor DBFieldAccessor { get; } = new DefaultFieldAccessor();

        public override WhereTermiNode<RowType> CreateWhereRoot<RowType>(Table table) {
            return new WhereTermiNode<RowType>(table, this);
        }

        public override string[] QueryAllTable() {
            var tables = new List<string>();
            using (var cmd = DB.CreateCommand()) {
                cmd.CommandText = "SHOW TABLES";
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        tables.Add(reader.GetString(0));
                    }
                }
            }
            return tables.ToArray();
        }

        public override bool QueryTable(string tableName, [NotNullWhen(true)] out TableInfo? table) {
            table = null;
            using (var cmd = DB.CreateCommand()) {
                cmd.CommandText = $"DESCRIBE {tableName}";
                using (var reader = cmd.ExecuteReader()) {
                    if (!reader.HasRows) return false;
                    var columns = new List<Column>();
                    while (reader.Read()) {
                        var column = new Column(
                            reader.GetString("Field"),
                            null,
                            Type.GetType(reader.GetString("Type")) ?? typeof(string),
                            DBFieldAccessor
                        );
                        columns.Add(column);
                    }
                    table = new TableInfo(tableName, columns);
                }
            }
            return true;
        }

        public override bool EnsureTableStructure(Table table) {
            throw new NotImplementedException();
        }

        public override bool CreateTable(Table table) {
            throw new NotImplementedException();
        }

        public override bool DropTable(string table) {
            using (var cmd = DB.CreateCommand()) {
                cmd.CommandText = $"DROP TABLE IF EXISTS {table}";
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public override bool RenameTable(string tableFrom, string tableTo) {
            using (var cmd = DB.CreateCommand()) {
                cmd.CommandText = $"RENAME TABLE {tableFrom} TO {tableTo}";
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public override bool AlterTableStruct(TableInfo tableFrom, Table tableTo) {
            throw new NotImplementedException();
        }

        public override int DeleteRow<RowType>(IWhere<RowType> where) {
            using (var cmd = DB.CreateCommand()) {
                cmd.CommandText = $"DELETE FROM {where.Table.Name} WHERE {where.GetPlainStatementContent()}";
                return cmd.ExecuteNonQuery();
            }
        }

        public override int InsertRow(Table table, params Value[] values) {
            var columns = string.Join(", ", values.Select(v => v.Column.Name));
            var parameters = string.Join(", ", values.Select((v, i) => $"@p{i}"));
            using (var cmd = DB.CreateCommand()) {
                cmd.CommandText = $"INSERT INTO {table.Name} ({columns}) VALUES ({parameters})";
                for (int i = 0; i < values.Length; i++) {
                    cmd.Parameters.AddWithValue($"@p{i}", values[i].GetStatementParam());
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public override int UpdateRow<RowType>(IWhere<RowType> where, params Value[] updates) {
            var setClause = string.Join(", ", updates.Select((v, i) => $"{v.Column.Name} = @p{i}"));
            using (var cmd = DB.CreateCommand()) {
                cmd.CommandText = $"UPDATE {where.Table.Name} SET {setClause} WHERE {where.GetPlainStatementContent()}";
                for (int i = 0; i < updates.Length; i++) {
                    cmd.Parameters.AddWithValue($"@p{i}", updates[i].GetStatementParam());
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public override QueryReader SelectMax<RowType>(IWhere<RowType> where, Column column) {
            return ExecuteSelectQuery(where, $"MAX({column.Name})");
        }

        public override QueryReader SelectMin<RowType>(IWhere<RowType> where, Column column) {
            return ExecuteSelectQuery(where, $"MIN({column.Name})");
        }

        public override QueryReader SelectAverage<RowType>(IWhere<RowType> where, Column column) {
            return ExecuteSelectQuery(where, $"AVG({column.Name})");
        }

        public override QueryReader SelectSum<RowType>(IWhere<RowType> where, Column column) {
            return ExecuteSelectQuery(where, $"SUM({column.Name})");
        }

        public override QueryReader SelectAll<RowType>(IWhere<RowType> where) {
            return ExecuteSelectQuery(where, "*");
        }

        class MysqlReaderBackup : QueryReaderBackup
        {
            private readonly string _connectionString;
            private readonly string _commandText;
            private readonly List<MySqlParameter> _parameters;

            public MysqlReaderBackup(MySqlCommand command) {
                _connectionString = command.Connection.ConnectionString;
                _commandText = command.CommandText;
                _parameters = new List<MySqlParameter>();
                foreach (MySqlParameter param in command.Parameters) {
                    _parameters.Add((MySqlParameter)((ICloneable)param).Clone());
                }
            }

            public override QueryReader CreateReader() {
                var connection = new MySqlConnection(_connectionString);
                connection.Open();
                var command = new MySqlCommand(_commandText, connection);
                command.Parameters.AddRange(_parameters.ToArray());
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return new QueryReader(connection, reader, this, new DefaultFieldAccessor());
            }
        }
        private QueryReader ExecuteSelectQuery<RowType>(IWhere<RowType> where, string selectClause) where RowType : notnull, new() {
            using (var cmd = DB.CreateCommand()) {
                cmd.CommandText = $"SELECT {selectClause} FROM {where.Table.Name} WHERE {where.GetPlainStatementContent()}";
                var reader = cmd.ExecuteReader();
                return new QueryReader(DB, reader, new MysqlReaderBackup(cmd), DBFieldAccessor);
            }
        }
    }
}
