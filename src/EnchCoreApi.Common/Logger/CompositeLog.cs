using EnchCoreApi.Common.DB;

namespace EnchCoreApi.Common.Logger {
    public class CompositeLog : GenericLog, IDisposable {
        protected GenericLog GLog { get; set; }
        protected ConsoleLog CLog { get; set; }
        protected CompositeLog(string name, GenericLog gl) : base(name) {
            GLog = gl;
            CLog = new ConsoleLog(Name);
        }

        public static CompositeLog CreateSqlBaseLog(string name, DBService db) {
            return new CompositeLog(name, new SqlLog(name, db));
        }

        public static CompositeLog CreateTextBaseLog(string name, string filename, bool clear) {
            return new CompositeLog(name, new TextLog(name, filename, clear));
        }

        public override void Log(string log, LogFlag flag) {
            GLog.Log(log, flag);
            CLog.Log(log, flag);
        }

        public void Dispose() {
            (GLog as TextLog)?.Dispose();
        }
    }
}
