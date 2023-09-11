namespace EnchCoreApi.Common.DB.DBAttribute {
    //Optional
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnUniqueAttribute : Attribute { }
}
