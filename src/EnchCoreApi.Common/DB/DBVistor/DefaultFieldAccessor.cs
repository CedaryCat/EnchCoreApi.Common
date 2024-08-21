using System.Data;

namespace EnchCoreApi.Common.DB.DBVistor
{
    public class DefaultFieldAccessor : IDBFieldAccessor, ITextStorgeFieldAccessor<DateTime>
    {
        public T Get<T>(IDataReader reader, int column) {
            return ((IDBFieldGetter<T>)this).Get(reader, column);
        }

        string ITextStorgeFieldAccessor<DateTime>.SerializeTextContent(DateTime from) {
            return from.ToString();
        }

        DateTime ITextStorgeFieldAccessor<DateTime>.DeserializeFromTextContent(string str) {
            return DateTime.Parse(str);
        }

        string IDBFieldGetter<string>.Get(IDataReader r, int column) => r.IsDBNull(column) ? null : r.GetString(column);

        int IDBFieldGetter<int>.Get(IDataReader r, int column) => r.GetInt32(column);

        int? IDBFieldGetter<int?>.Get(IDataReader r, int column) => r.IsDBNull(column) ? null : r.GetInt32(column);

        float IDBFieldGetter<float>.Get(IDataReader r, int column) => r.GetFloat(column);

        float? IDBFieldGetter<float?>.Get(IDataReader r, int column) => r.IsDBNull(column) ? null : r.GetFloat(column);

        bool IDBFieldGetter<bool>.Get(IDataReader r, int column) => r.GetBoolean(column);

        bool? IDBFieldGetter<bool?>.Get(IDataReader r, int column) => r.IsDBNull(column) ? null : r.GetBoolean(column);

        double IDBFieldGetter<double>.Get(IDataReader r, int column) => r.GetDouble(column);

        double? IDBFieldGetter<double?>.Get(IDataReader r, int column) => r.IsDBNull(column) ? null : r.GetDouble(column);

        byte IDBFieldGetter<byte>.Get(IDataReader r, int column) => r.GetByte(column);

        byte? IDBFieldGetter<byte?>.Get(IDataReader r, int column) => r.IsDBNull(column) ? null : r.GetByte(column);

        long IDBFieldGetter<long>.Get(IDataReader r, int column) => r.GetInt64(column);

        long? IDBFieldGetter<long?>.Get(IDataReader r, int column) => r.IsDBNull(column) ? null : r.GetInt64(column);

        short IDBFieldGetter<short>.Get(IDataReader r, int column) => r.GetInt16(column);

        short? IDBFieldGetter<short?>.Get(IDataReader r, int column) => r.IsDBNull(column) ? null : r.GetInt16(column);

        decimal IDBFieldGetter<decimal>.Get(IDataReader r, int column) => r.GetDecimal(column);

        decimal? IDBFieldGetter<decimal?>.Get(IDataReader r, int column) => r.IsDBNull(column) ? null : r.GetDecimal(column);
    }
}
