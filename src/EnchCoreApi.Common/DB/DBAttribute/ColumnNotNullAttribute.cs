﻿namespace EnchCoreApi.Common.DB.DBAttribute {
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnNotNullAttribute : Attribute { }
}
