using EnchCoreApi.Common.DB.Core;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace EnchCoreApi.Common.DB.DBVistor
{
    public class QueryReader : IDisposable, ICloneable
    {
        public IDbConnection? Connection { get; protected set; }
        public IDataReader? Reader { get; protected set; }
        private QueryReaderBackup Backup { get; set; }
        public IDBFieldAccessor FieldAccessor { get; set; }

        public QueryReader(IDbConnection conn, IDataReader reader, QueryReaderBackup backup, IDBFieldAccessor dbAccess)
        {
            Connection = conn;
            Reader = reader;
            Backup = backup;
            FieldAccessor = dbAccess;
        }

        public object Clone()
        {
            return Backup.CreateReader();
        }

        ~QueryReader()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Reader != null)
                {
                    Reader.Dispose();
                    Reader = null;
                }
                if (Connection != null)
                {
                    Connection.Dispose();
                    Connection = null;
                }
            }
        }

        [MemberNotNullWhen(true, nameof(Reader))]
        public bool Read()
        {
            if (Reader == null)
                return false;
            return Reader.Read();
        }

        [MemberNotNull(nameof(Reader))]
        public T Get<T>(Column column)
        {
            if (Reader == null) throw new ObjectDisposedException(nameof(Reader));
            return new DBValue<T>(column, FieldAccessor, Reader).Deserialize();
        }
    }
}
