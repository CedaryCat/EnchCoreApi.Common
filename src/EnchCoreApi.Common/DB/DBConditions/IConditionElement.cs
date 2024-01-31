namespace EnchCoreApi.Common.DB.DBConditions {
    public interface IConditionElement {
        string GetPlainStatement();
        string GetStatement(ref ICollection<object?> statementParams);
    }
}
