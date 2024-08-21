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
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = HttpTool.HttpContentTypeString[type];
            req.Timeout = 800;//请求超时时间

            var p = HttpTool.ToParameter(content, type);
            byte[] data = Encoding.UTF8.GetBytes(p);
            req.ContentLength = data.Length;
            using Stream reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            return stream;
        }
        public static JObject PostForJObject(string url, Dictionary<string, string> content, HttpContentType type) {
            return JsonConvert.DeserializeObject<JObject>(PostForString(url, content, type));
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
        public static Image PostForImage(string url, Dictionary<string, string> content, HttpContentType type) {
            using var stream = PostForStream(url, content, type);
            return Image.FromStream(stream);
        }
    }
}
