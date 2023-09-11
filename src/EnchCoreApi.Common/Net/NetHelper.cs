using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace EnchCoreApi.Common.Net {
    public class NetHelper {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPorDomain"></param>
        /// <returns>不成功返回null</returns>
        public static bool GetIPFromDomainOrIPText(string iPorDomain, [NotNullWhen(true)]out IPAddress? ip) {
            if (!IPAddress.TryParse(iPorDomain, out ip)) {
                ip = null;
                var ips = Dns.GetHostAddresses(iPorDomain);
                if (ips.Length == 0)
                    return false;
                else
                    ip = ips[0];
                return true;
            }
            return true;
        }
        public static IPAddress GetCurrentIp() {
            var entry = Dns.GetHostEntry(Dns.GetHostName()) ?? throw new Exception();
            return entry.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
        public static IPAddress MapIPAddress4FromInt(int item) {
            var bytes = BitConverter.GetBytes(item);
            return IPAddress.Parse($"{bytes[0]}.{bytes[1]}.{bytes[2]}.{bytes[3]}");
        }
    }
}
