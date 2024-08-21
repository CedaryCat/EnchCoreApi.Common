namespace EnchCoreApi.Common.DB.DBAttribute
{
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnLengthAttribute : Attribute
    {
        public int Length;
        public ColumnLengthAttribute(int length) {
            Length = length;
        }
    }
}
