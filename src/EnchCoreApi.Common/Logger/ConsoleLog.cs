namespace EnchCoreApi.Common.Logger
{
    public class ConsoleLog : GenericLog
    {
        public ConsoleLog(string name) : base(name) {

        }
        public override void Log(string log, LogFlag flag) {
            switch (flag) {
                case LogFlag.Success: {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{Name}: {log}");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                case LogFlag.Warning: {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine($"{Name}: {log}");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;
                    }
                case LogFlag.Error: {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine($"{Name}: {log}");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;
                    }
                case LogFlag.Info: {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{Name}: {log}");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                case LogFlag.Normal:
                default: {
                        Console.WriteLine($"{Name}: {log}");
                        break;
                    }
            }
        }
    }
}
