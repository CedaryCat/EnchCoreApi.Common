using Newtonsoft.Json;
using System.Text;

namespace EnchCoreApi.Common.Utilities {
    public class JsonConfigHelper : ConfigHelper {
        public override T LoadFromFile<T>(string filePath) {
            if (!File.Exists(filePath))
                return new T();
            return JsonConvert.DeserializeObject<T>(FileHelper.ReadFileTextInCorrectEncoding(filePath));
        }

        public override bool LoadFromFile<T>(string filePath, out T config) {
            config = null;
            if (!File.Exists(filePath))
                return false;
            config = JsonConvert.DeserializeObject<T>(FileHelper.ReadFileTextInCorrectEncoding(filePath));
            return true;
        }

        public override void SaveToFile(string filePath, object configObj) {
            if (!File.Exists(filePath))
                File.Create(filePath).Dispose();
            File.WriteAllBytes(filePath, new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(configObj, Formatting.Indented)));
        }
    }
}
