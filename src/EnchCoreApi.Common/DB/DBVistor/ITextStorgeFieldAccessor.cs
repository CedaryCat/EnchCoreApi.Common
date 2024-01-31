namespace EnchCoreApi.Common.DB.DBVistor
{
    public interface ITextStorgeFieldAccessor<T>
    {
        string SerializeTextContent(T? from);
        T DeserializeFromTextContent(string text);
    }
}
