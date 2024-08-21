using EnchCoreApi.Common.DB;

namespace EnchCoreApi.Common.Logger
{
    public class SqlLog : GenericLog
    {
        public SqlLog(string name, DBService db) : base(name) {
            table = db.CreateTableService<LogItem>();
            table.EnsureTableStructure();
        }

        public TableService<LogItem> table;

        public override void Log(string log, LogFlag flag) {
            table.Insert(new LogItem() {
                Source = Name,
                Message = log,
                Type = flag.ToString(),
                Date = DateTime.Now,
            });
        }
        public class LogItem
        {
            public string Source { get; set; }
            public string Message { get; set; }
            public string Type { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
