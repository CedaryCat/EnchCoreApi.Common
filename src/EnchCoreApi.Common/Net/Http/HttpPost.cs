using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Net;
using System.Text;

namespace EnchCoreApi.Common.Net.Http
{
    public static class HttpPost
    {
        public static Stream PostForStream(string url, Dictionary<string, string> content, HttpContentType type) {
            using var client = new HttpClient();
            var requestContent = new StringContent(HttpTool.ToParameter(content, type), Encoding.UTF8, HttpTool.HttpContentTypeString[type]);
            var response = client.PostAsync(url, requestContent).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStream();
        }

        public static JObject PostForJObject(string url, Dictionary<string, string> content, HttpContentType type) {
            return JsonConvert.DeserializeObject<JObject>(PostForString(url, content, type)) ?? new();
        }

        public static BinaryReader PostForBinaryReader(string url, Dictionary<string, string> content, HttpContentType type) {
            var reader = new BinaryReader(PostForStream(url, content, type));
            return reader;
        }

        public static StreamReader PostForStreamReader(string url, Dictionary<string, string> content, HttpContentType type) {
            var reader = new StreamReader(PostForStream(url, content, type), Encoding.UTF8);
            return reader;
        }

        public static string PostForString(string url, Dictionary<string, string> content, HttpContentType type) {
            using var reader = PostForStreamReader(url, content, type);
            return reader.ReadToEnd();
        }
    }
}
