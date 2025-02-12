namespace EnchCoreApi.Common.Net.Restful
{
    [Serializable]
    public class RestVerbs : Dictionary<string, string>
    {
        /// <summary>
        /// Gets value safely, if it does not exist, return null. Sets/Adds value safely, if null it will remove.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>Returns null if key does not exist.</returns>
        public new string? this[string key] {
            get {
                if (TryGetValue(key, out var ret))
                    return ret;
                return null;
            }
            set {
                if (value == null) {
                    Remove(key);
                    return;
                }
                else {
                    if (!TryAdd(key, value)) {
                        base[key] = value;
                    }
                }
            }
        }
    }
}
