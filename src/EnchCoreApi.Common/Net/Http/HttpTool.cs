using Newtonsoft.Json;
using System.Net;

namespace EnchCoreApi.Common.Net.Http
{
    public static class HttpTool
    {
        public static string ToParameter(Dictionary<string, string> source, HttpContentType type) {
            if (type == HttpContentType.Json) {
                return JsonConvert.SerializeObject(source);
            }
            else {
                var sourceStr = "";
                int i = source.Count;
                foreach (var s in source) {
                    i--;
                    sourceStr += ($"{UrlEncode(s.Key)}={UrlEncode(s.Value ?? "")}{(i != 0 ? "&" : "")}");
                }
                return sourceStr;
            }
        }
        public static string UrlEncode(string str) {
            //var sb = new StringBuilder();
            //var bytes = Encoding.UTF8.GetBytes(str);
            //for (int i = 0; i < bytes.Length; i++)
            //    sb.Append(@"%" + Convert.ToString(bytes[i], 16).ToUpper());
            //return sb.ToString();
            return WebUtility.UrlEncode(str);
        }
        public static Dictionary<string, string> SortByKey(Dictionary<string, string> array) {
            var keys = array.Keys.OrderBy(x => x, StringComparer.Ordinal).ToArray();
            var result = new Dictionary<string, string>();
            foreach (var k in keys)
                result.Add(k, array[k]);
            return result;
        }
        public static Dictionary<HttpContentType, string> HttpContentTypeString = new Dictionary<HttpContentType, string>()
        {
            {HttpContentType.Json,"application/json" },
            {HttpContentType.UrlEncode, "application/x-www-form-urlencoded" },
        };
    }
    public enum HttpContentType
    {
        Json,
        UrlEncode,
    }
}
