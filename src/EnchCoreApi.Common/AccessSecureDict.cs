namespace EnchCoreApi.Common {
    public class AccessSecureDict<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull where TValue : struct {
        private Dictionary<TKey, TValue> _dict;

        private Func<TValue>? _getDefaultValue;

        private Func<TKey, TValue>? _getDefaultValue2;
        public AccessSecureDict() {
            _dict = new Dictionary<TKey, TValue>();
            _getDefaultValue = delegate { return default; };
        }
        public AccessSecureDict(IDictionary<TKey, TValue> dictionary) {
            _dict = new Dictionary<TKey, TValue>(dictionary);
            _getDefaultValue = delegate { return default; };
        }
        public AccessSecureDict(IDictionary<TKey, TValue> dictionary, Func<TValue> getDefaultValue) {
            _dict = new Dictionary<TKey, TValue>(dictionary);
            _getDefaultValue = getDefaultValue;
        }
        public AccessSecureDict(Func<TValue> getDefaultValue) {
            _dict = new Dictionary<TKey, TValue>();
            _getDefaultValue = getDefaultValue;
        }
        public AccessSecureDict(Func<TKey, TValue> getDefaultValue) {
            _dict = new Dictionary<TKey, TValue>();
            _getDefaultValue2 = getDefaultValue;
        }
        /// <summary>
        /// when get: if the key does not exist in the dictionary,the defaultVaule is returned.
        /// when set: if the key does not exist in the dictionary,the key-value pair is added
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key] {
            get {
                if (_dict.TryGetValue(key, out var value)) {
                    return value;
                }
                if (_getDefaultValue != null) value = _getDefaultValue.Invoke();
                if (_getDefaultValue2 != null) value = _getDefaultValue2.Invoke(key);
                _dict.Add(key, value);
                return value;
            }
            set {
                if (_dict.ContainsKey(key)) {
                    _dict[key] = value;
                }
                else {
                    _dict.Add(key, value);
                }
            }
        }

        public ICollection<TKey> Keys => _dict.Keys;

        public ICollection<TValue> Values => _dict.Values;

        public int Count => _dict.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) {
            if (_dict.ContainsKey(key)) {
                _dict[key] = value;
            }
            else {
                _dict.Add(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            if (_dict.ContainsKey(item.Key)) {
                _dict[item.Key] = item.Value;
            }
            else {
                _dict.Add(item.Key, item.Value);
            }
        }

        public void Clear() {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return _dict.Contains(item);
        }

        public bool ContainsKey(TKey key) {
            return _dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) {
            if (array == null) {
                throw new ArgumentNullException();
            }

            if (index < 0 || index > array.Length) {
                throw new ArgumentOutOfRangeException();
            }

            if (array.Length - index < Count) {
                throw new ArgumentException();
            }

            int num = Count;
            KeyValuePair<TKey, TValue>[] array2 = _dict.ToArray();
            for (int i = 0; i < num; i++) {
                array[index++] = new KeyValuePair<TKey, TValue>(array2[i].Key, array2[i].Value);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return _dict.GetEnumerator();
        }

        public bool Remove(TKey key) {
            return _dict.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return _dict.Remove(item.Key);
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return _dict.TryGetValue(key, out value);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((System.Collections.IEnumerable)_dict).GetEnumerator();
        }
    }

    public class AccessSecureRefDict<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull where TValue : class {
        private Dictionary<TKey, TValue> _dict;

        private Func<TValue>? _getDefaultValue;

        private Func<TKey, TValue>? _getDefaultValue2;
        public AccessSecureRefDict(TValue defValue) {
            _dict = new Dictionary<TKey, TValue>();
            _getDefaultValue = delegate { return defValue; };
        }
        public AccessSecureRefDict(IDictionary<TKey, TValue> dictionary, TValue defValue) {
            _dict = new Dictionary<TKey, TValue>(dictionary);
            _getDefaultValue = delegate { return defValue; };
        }
        public AccessSecureRefDict(IDictionary<TKey, TValue> dictionary, Func<TValue> getDefaultValue) {
            _dict = new Dictionary<TKey, TValue>(dictionary);
            _getDefaultValue = getDefaultValue;
        }
        public AccessSecureRefDict(Func<TValue> getDefaultValue) {
            _dict = new Dictionary<TKey, TValue>();
            _getDefaultValue = getDefaultValue;
        }
        public AccessSecureRefDict(Func<TKey, TValue> getDefaultValue) {
            _dict = new Dictionary<TKey, TValue>();
            _getDefaultValue2 = getDefaultValue;
        }
        /// <summary>
        /// when get: if the key does not exist in the dictionary,the defaultVaule is returned.
        /// when set: if the key does not exist in the dictionary,the key-value pair is added
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key] {
            get {
                if (_dict.TryGetValue(key, out var value)) {
                    return value;
                }
                if (_getDefaultValue != null) value = _getDefaultValue.Invoke();
                if (_getDefaultValue2 != null) value = _getDefaultValue2.Invoke(key);
                _dict.Add(key, value);
                return value;
            }
            set {
                if (_dict.ContainsKey(key)) {
                    _dict[key] = value;
                }
                else {
                    _dict.Add(key, value);
                }
            }
        }

        public ICollection<TKey> Keys => _dict.Keys;

        public ICollection<TValue> Values => _dict.Values;

        public int Count => _dict.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) {
            if (_dict.ContainsKey(key)) {
                _dict[key] = value;
            }
            else {
                _dict.Add(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            if (_dict.ContainsKey(item.Key)) {
                _dict[item.Key] = item.Value;
            }
            else {
                _dict.Add(item.Key, item.Value);
            }
        }

        public void Clear() {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return _dict.Contains(item);
        }

        public bool ContainsKey(TKey key) {
            return _dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) {
            if (array == null) {
                throw new ArgumentNullException();
            }

            if (index < 0 || index > array.Length) {
                throw new ArgumentOutOfRangeException();
            }

            if (array.Length - index < Count) {
                throw new ArgumentException();
            }

            int num = Count;
            KeyValuePair<TKey, TValue>[] array2 = _dict.ToArray();
            for (int i = 0; i < num; i++) {
                array[index++] = new KeyValuePair<TKey, TValue>(array2[i].Key, array2[i].Value);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return _dict.GetEnumerator();
        }

        public bool Remove(TKey key) {
            return _dict.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return _dict.Remove(item.Key);
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return _dict.TryGetValue(key, out value);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((System.Collections.IEnumerable)_dict).GetEnumerator();
        }
    }
}
