using System.Diagnostics;

namespace EnchCoreApi.Common.DB {
    public class Watch {
        public static Watch w = new Watch();
        private Stopwatch sw = new Stopwatch();
        private string Name;
        public void Start(string name) {
            if (Name != null) {
                sw.Stop();
                Console.WriteLine($"{Name} ，use time:{sw.Elapsed.TotalMilliseconds}ms({sw.Elapsed.TotalSeconds}s)");
            }
            Name = name;
            sw.Restart();
            sw.Start();
        }
        public void Stop() {
            sw.Stop();
            Console.WriteLine($"{Name} ，use time:{sw.Elapsed.TotalMilliseconds}ms({sw.Elapsed.TotalSeconds}s)");
            Name = null;
        }
    }
}
