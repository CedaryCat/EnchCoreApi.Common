namespace EnchCoreApi.Common.DB.DBAttribute
{
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnDefaultValueAttribute : Attribute
    {
        public string Default;
        public ColumnDefaultValueAttribute(string defaultValue) {
            Default = defaultValue;
        }
    }
}
