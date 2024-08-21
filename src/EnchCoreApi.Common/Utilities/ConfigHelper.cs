namespace EnchCoreApi.Common.Utilities
{
    public abstract class ConfigHelper
    {
        public abstract T LoadFromFile<T>(string filePath) where T : class, new();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="config"></param>
        /// <returns> if load success</returns>
        public abstract bool LoadFromFile<T>(string filePath, out T config) where T : class;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultConfig">如果在该路径下未查到目标Config,则使用此委托生成,并且保存在该路径。</param>
        /// <returns></returns>
        public T EnsureConfigExist<T>(string filePath, Func<T> defaultConfig) where T : class {
            if (!LoadFromFile<T>(filePath, out var config)) {
                config = defaultConfig();
                SaveToFile(filePath, config);
            }
            return config;
        }
        public abstract void SaveToFile(string filePath, object configObj);
    }
}
