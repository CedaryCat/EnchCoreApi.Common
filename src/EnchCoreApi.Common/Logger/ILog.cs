namespace EnchCoreApi.Common.Logger {
    public interface ILog {
        public void ConsoleLog(string log);
        public void ConsoleInfoLog(string log);
        public void ConsoleSuccessLog(string log);
        public void ConsoleErrorLog(string log);
        public void ConsoleWarningLog(string log);
        public void ConsoleException(Exception ex);
    }
}
