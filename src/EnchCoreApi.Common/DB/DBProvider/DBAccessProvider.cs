using System.Data;

namespace EnchCoreApi.Common.DB.DBProvider
{
    public abstract class DBAccessProvider<TDBConn> : DBAccessAbstractProvider where TDBConn : IDbConnection
    {
        public DBAccessProvider(TDBConn db) {
            DB = db;
        }
        public sealed override IDbConnection DbConnection => DB;
        public TDBConn DB { get; protected set; }
    }
}
