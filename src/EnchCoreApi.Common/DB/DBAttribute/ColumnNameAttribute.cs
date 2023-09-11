namespace EnchCoreApi.Common.DB.DBAttribute {
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnNameAttribute : Attribute {
        public string Name;
        public ColumnNameAttribute(string name) {
            Name = name;
        }
    }
}
