using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Net;
using System.Text;

namespace EnchCoreApi.Common.Net.Http {
    public static class HttpGet {
        static HttpGet() {
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
        }
        public static JObject GetJObject(string url) {
            return JsonConvert.DeserializeObject<JObject>(GetString(url));
        }
        public static Stream GetStream(string url) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            return response.GetResponseStream();
        }
        public static BinaryReader GetBinaryReader(string url) {
            return new BinaryReader(GetStream(url));
        }
        public static StreamReader GetStreamReader(string url) {
            return new StreamReader(GetStream(url), Encoding.UTF8);
        }
        public static string GetString(string url) {
            using var reader = GetStreamReader(url);
            return reader.ReadToEnd();
        }
        public static Image GetImage(string url) {
            //return Image.FromStream(GetStream(url));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using var stream = response.GetResponseStream();
            return Image.FromStream(stream);
        }
        public static bool GetFile(string url, string path, string name, bool coverFile = true) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Directory.CreateDirectory(path);
            if (coverFile) {
                if (File.Exists(path + name))
                    File.Delete(path + name);
                if (File.Exists(path + name + ".downloading"))
                    File.Delete(path + name + ".downloading");
            }
            else {
                if (File.Exists(path + name) || File.Exists(path + name + ".downloading"))
                    return false;
            }
            try {
                using (var responseStream = response.GetResponseStream()) {
                    using (var fileStream = File.Create(path + name + ".downloading")) {
                        byte[] buffer = new byte[1024 * 10];
                        int size = responseStream.Read(buffer, 0, buffer.Length);
                        while (size > 0) {
                            fileStream.Write(buffer, 0, size);
                            size = responseStream.Read(buffer, 0, buffer.Length);
                        }
                    }
                }
                var file = new FileInfo(path + name + ".downloading");
                file.MoveTo(path + name);
                return true;
            }
            catch {
                return false;
            }
            finally {

            }
        }
    }
}
