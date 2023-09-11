using System.Collections;

namespace EnchCoreApi.Common {

    public class RandomEnumerable<T> : IEnumerable<T>, IEnumerator<T> {
        private readonly T[] _items;
        private readonly bool[] _taked;
        private readonly Random _random;
        private int _currentIndex;
        private int _restCount;

        public RandomEnumerable(Random random, IEnumerable<T> items) {
            _items = items.ToArray();
            _taked = new bool[_items.Length];
            _random = random;
            Reset();
        }

        public T Current => _items[_currentIndex];

        object? IEnumerator.Current => Current;

        public void Dispose() {
        }

        public IEnumerator<T> GetEnumerator() {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this;
        }

        public bool MoveNext() {
            if (_restCount == 0) {
                return false;
            }

            if (_currentIndex == -1) {
                _currentIndex = _random.Next(_restCount);
                return true;
            }
            else {
                _taked[_currentIndex] = true;
                _restCount -= 1;
            }
            if (_restCount <= 0) {
                return false;
            }
            var id = _random.Next(_restCount);
            for (int i = 0; i < _items.Length; i++) {
                if (_taked[i]) continue;
                if (--id <= 0) {
                    _taked[_currentIndex = i] = true;
                    _restCount -= 1;
                    return true;
                }
            }
            return false;
        }

        public void Reset() {
            Array.Fill(_taked, false);
            _restCount = _items.Length;
            _currentIndex = -1;
        }
    }
}
