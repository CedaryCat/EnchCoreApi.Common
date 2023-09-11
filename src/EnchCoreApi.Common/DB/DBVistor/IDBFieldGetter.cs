using System.Data;

namespace EnchCoreApi.Common.DB.DBVistor
{
    public interface IDBFieldGetter<T>
    {
        T Get(IDataReader reader, int column);
    }
}
