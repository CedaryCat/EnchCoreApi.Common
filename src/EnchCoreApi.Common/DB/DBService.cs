using EnchCoreApi.Common.DB.DBProvider;
using System.Data;

namespace EnchCoreApi.Common.DB
{
    public class DBService
    {
        public IDbConnection db;
        protected DBAccessAbstractProvider Provider { get; set; }
        protected DBService(DBAccessAbstractProvider provider) {
            Provider = provider;
            db = provider.DbConnection;
        }
        public static DBService CreateService<TDBConn>(DBAccessProvider<TDBConn> provider) where TDBConn : IDbConnection {
            return new DBService(provider);
        }
        public TableService<RowType> CreateTableService<RowType>() where RowType : notnull, new() {
            return TableService<RowType>.CreateTableService(Provider);
        }
        public TableService<RowType> CreateTableService<RowType>(string tableName) where RowType : notnull, new() {
            return TableService<RowType>.CreateTableService(Provider, tableName);
        }
    }
}
