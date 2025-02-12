using EnchCoreApi.Common.DB.Core;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace EnchCoreApi.Common.DB.DBVistor
{
    public class QueryReader : IDisposable, ICloneable
    {
        protected IDbConnection? connection;
        protected IDataReader? reader;
        public IDbConnection Connection {
            get => connection ?? throw new ObjectDisposedException(nameof(connection));
            protected set => connection = value;
        }
        public IDataReader Reader {
            get => reader ?? throw new ObjectDisposedException(nameof(reader));
            protected set => reader = value;
        }
        private QueryReaderBackup Backup { get; set; }
        public IDBFieldAccessor FieldAccessor { get; set; }

        public QueryReader(IDbConnection conn, IDataReader reader, QueryReaderBackup backup, IDBFieldAccessor dbAccess) {
            Connection = conn;
            Reader = reader;
            Backup = backup;
            FieldAccessor = dbAccess;
        }

        public object Clone() {
            return Backup.CreateReader();
        }

        ~QueryReader() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                Reader.Dispose();
                Connection.Dispose();
            }
        }

        public bool Read() {
            if (Reader == null)
                return false;
            return Reader.Read();
        }

        public T Get<T>(Column column) {
            return new DBValue<T>(column, FieldAccessor, Reader).GetTypedValue();
        }
    }
}
