using System.Text;

namespace EnchCoreApi.Common.Utilities
{
    public class RandomHelper
    {
        private readonly Random rand;
        public RandomHelper(int seed) {
            rand = new Random(seed);
        }
        public RandomHelper() {
            rand = new Random();
        }
        public RandomHelper(Random rand) {
            this.rand = rand;
        }
        public string NextString(int length) {
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++) {
                switch (rand.Next(0, 3)) {
                    case 0:
                        sb.Append((char)rand.Next('a', 'z' + 1));
                        break;
                    case 1:
                        sb.Append((char)rand.Next('A', 'Z' + 1));
                        break;
                    case 2:
                        sb.Append((char)rand.Next('0', '9' + 1));
                        break;
                }
            }
            return sb.ToString();
        }
        public RandomEnumerable<T> GetRandomEnumerable<T>(IEnumerable<T> items) {
            return new RandomEnumerable<T>(rand, items);
        }
        public IEnumerable<T> GetRandomItems<T>(int count, params T[] array) {
            if (array is null) {
                throw new ArgumentNullException("array");
            }
            if (count > array.Length) {
                throw new ArgumentException("count > array.Length");
            }
            if (array.Length == 0 || count == array.Length) {
                return array;
            }
            else {
                List<T> list = new List<T>(array);
                while (list.Count > count) {
                    list.RemoveAt(rand.Next(0, list.Count));
                }
                return list;
            }
        }
        public T GetRandomItem<T>(List<T> list) {
            if (list is null) {
                throw new ArgumentNullException("list");
            }
            if (list.Count == 0) {
                throw new ArgumentOutOfRangeException("There are not element in the list");
            }
            else {
                return list.ElementAt(rand.Next(0, list.Count));
            }
        }
        public T GetRandomItem<T>(params T[] array) {
            if (array is null) {
                throw new ArgumentNullException("list");
            }
            if (array.Length == 0) {
                throw new ArgumentOutOfRangeException("There are not element in the array");
            }
            else {
                return array[rand.Next(0, array.Length)];
            }
        }
        public T GetRandomItem<T>(IEnumerable<T> list) {
            if (list is null) {
                throw new ArgumentNullException("list");
            }
            int count = list.Count();
            if (count == 0) {
                throw new ArgumentOutOfRangeException("There are not element in the list");
            }
            else {
                return list.ElementAt(rand.Next(0, count));
            }
        }
        public bool InRate(double probablity) {
            var i = 1000000000;
            return probablity > (rand.Next(i) / (double)i);
        }
        public int Next() {
            return rand.Next();
        }
        public int Next(int maxValue) {
            return rand.Next(maxValue);
        }
        public int Next(int minValue, int maxValue) {
            return rand.Next(minValue, maxValue);
        }
        public void NextBytes(byte[] buffer) {
            rand.NextBytes(buffer);
        }
        public double NextDouble() {
            return rand.NextDouble();
        }
    }
}
