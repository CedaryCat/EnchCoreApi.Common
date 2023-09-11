namespace EnchCoreApi.Common.Logger {
    public abstract class GenericLog : ILog {
        public GenericLog(string name) {
            Name = name;
        }
        public virtual string Name { get; set; } = "";
        public virtual bool CanLog { get; set; } = true;
        public abstract void Log(string log, LogFlag flag);
        protected virtual string ExceptionFormat(Exception ex) {
            return ex.ToString();
        }
        public virtual void ConsoleException(Exception ex) {
            ConsoleErrorLog(ExceptionFormat(ex));
        }

        public virtual void ConsoleSuccessLog(string log) {
            ConsoleLog(log, LogFlag.Success);
        }

        public virtual void ConsoleWarningLog(string log) {
            ConsoleLog(log, LogFlag.Warning);
        }

        public virtual void ConsoleErrorLog(string log) {
            ConsoleLog(log, LogFlag.Error);
        }

        public virtual void ConsoleInfoLog(string log) {
            ConsoleLog(log, LogFlag.Info);
        }

        public virtual void ConsoleLog(string log) {
            ConsoleLog(log, LogFlag.Normal);
        }
        private object obj = new object();
        private void ConsoleLog(string log, LogFlag flag) {
            lock (obj) {
                if (CanLog) {
                    Log(log, flag);
                }
            }
        }
    }
    public enum LogFlag {
        Normal,
        Info,
        Success,
        Warning,
        Error,
    }
}
