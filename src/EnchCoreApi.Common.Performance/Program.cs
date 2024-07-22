
using EnchCoreApi.Common.DB;
using EnchCoreApi.Common.DB.DBProvider;
using EnchCoreApi.Common.Performance;
using Microsoft.Data.Sqlite;

DBService db = DBService.CreateService(new SqliteAccessProvider(new SqliteConnection($"Data Source=test.sqlite")));
var table = db.CreateTableService<TableData>();
table.EnsureTableStructure();

table.Insert(new TableData() {
    Count = 1,
    Name = "test",
    Date = DateTime.Now,
    Percent = 0.5,
    Value = "100",
});

var data = table.Where(nameof(TableData.Name), "test").SelectAll()?.Parse() ?? throw new Exception();

Console.WriteLine(data.Name);