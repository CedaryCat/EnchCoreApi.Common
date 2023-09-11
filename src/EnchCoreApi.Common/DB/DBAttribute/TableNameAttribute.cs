namespace EnchCoreApi.Common.DB.DBAttribute {
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Struct)]
    public class TableNameAttribute : Attribute {
        public string Name;
        public TableNameAttribute(string name) {
            Name = name;
        }
    }
}
