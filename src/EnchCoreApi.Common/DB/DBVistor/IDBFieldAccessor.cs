using System.Data;

namespace EnchCoreApi.Common.DB.DBVistor
{
    /// <summary>
    /// Display inherits this and IDBFieldGetter interface to enable explicit type access to IDBReader fields
    /// </summary>
    public interface IDBFieldAccessor : IDBFieldGetter<string?>, IDBFieldGetter<int>, IDBFieldGetter<int?>, IDBFieldGetter<float>, IDBFieldGetter<float?>, IDBFieldGetter<bool>, IDBFieldGetter<bool?>, IDBFieldGetter<double>, IDBFieldGetter<double?>, IDBFieldGetter<byte>, IDBFieldGetter<byte?>, IDBFieldGetter<long>, IDBFieldGetter<long?>, IDBFieldGetter<short>, IDBFieldGetter<short?>, IDBFieldGetter<decimal>, IDBFieldGetter<decimal?>
    {
        T Get<T>(IDataReader reader, int column);
    }
}
