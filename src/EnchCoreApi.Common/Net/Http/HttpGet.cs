using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Net;
using System.Text;

namespace EnchCoreApi.Common.Net.Http
{
    public static class HttpGet
    {

        private static async Task<HttpResponseMessage> GetResponseAsync(string url) {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return await client.GetAsync(url);
        }

        public static async Task<JObject> GetJObjectAsync(string url) {
            return JsonConvert.DeserializeObject<JObject>(await GetStringAsync(url)) ?? new();
        }

        public static async Task<Stream> GetStreamAsync(string url) {
            var response = await GetResponseAsync(url);
            return await response.Content.ReadAsStreamAsync();
        }

        public static async Task<BinaryReader> GetBinaryReaderAsync(string url) {
            return new BinaryReader(await GetStreamAsync(url));
        }

        public static async Task<StreamReader> GetStreamReaderAsync(string url) {
            return new StreamReader(await GetStreamAsync(url), Encoding.UTF8);
        }

        public static async Task<string> GetStringAsync(string url) {
            using var reader = await GetStreamReaderAsync(url);
            return await reader.ReadToEndAsync();
        }

        public static async Task<bool> GetFileAsync(string url, string directory, string name, bool coverFile = true) {
            var path = Path.Combine(directory, name);
            var tempPath = path + ".downloading";

            if (!coverFile && (File.Exists(path) || File.Exists(tempPath))) {
                return false;
            }

            Directory.CreateDirectory(directory);

            try {
                using (var responseStream = await GetStreamAsync(url))
                using (var fileStream = File.Create(tempPath)) {
                    byte[] buffer = new byte[1024 * 10];
                    int size;
                    while ((size = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                        await fileStream.WriteAsync(buffer, 0, size);
                    }
                }

                if (File.Exists(path)) File.Delete(path);
                File.Move(tempPath, path);
                return true;
            }
            catch {
                if (File.Exists(tempPath)) File.Delete(tempPath);
                return false;
            }
        }
    }
}
