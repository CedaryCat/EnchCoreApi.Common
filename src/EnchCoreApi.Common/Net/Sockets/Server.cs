using System.Net;
using System.Net.Sockets;

namespace EnchCoreApi.Common.Net.Sockets
{
    public class Server : IDisposable
    {
        /// <summary>
        /// 指示是否初始化了IPEnd,使用Start()时必须确保此值为true
        /// </summary>
        public bool Initialized { get; protected set; } = false;
        public bool IsStop { get; protected set; }
        private readonly object obj = new object();
        /// <summary>
        /// 信号量
        /// </summary>
        protected Semaphore semap = new Semaphore(5, 225);
        /// <summary>
        /// 服务端
        /// </summary>
        protected TcpListener TcpListener;
        public IPAddress IPaddress;
        public Dictionary<IPEndPoint, RemoteClient> Clients { get; set; }
        public int Port { get; private set; }
        public int bufferLength { get; private set; }
        public Server(int bufferLen = 8 * 1024) {
            bufferLength = Math.Max(bufferLen, 8 * 1024);
        }
        public void InitSocket(IPAddress ipaddress, int port, int bufferLen = 8 * 1024) {
            bufferLength = Math.Max(bufferLen, 8 * 1024);
            Clients = new Dictionary<IPEndPoint, RemoteClient>();
            IPaddress = ipaddress;
            Port = port;
            TcpListener = new TcpListener(IPaddress, Port);
            Initialized = true;
        }
        /// <summary>
        /// 启动监听,并处理连接
        /// </summary>
        public void Start() {
            try {
                if (!Initialized) {
                    throw new SocketNotInitilizedException();
                }
                TcpListener.Start();
                IsStop = false;
                new Task(delegate {
                    while (true) {
                        try {
                            if (IsStop == true) {
                                break;
                            }
                            GetAcceptTcpClient();
                        }
                        catch (Exception ex) {
                            try {
                                GetException?.Invoke(this, new ServerException() {
                                    SourceException = ex,
                                    IsServerException = true,
                                    ExceptionOnListen = true,
                                });
                            }
                            catch {

                            }
                        }
                        Thread.Sleep(1);
                    }
                }).Start();
            }
            catch (Exception ex) {
                GetException?.Invoke(this, new ServerException() {
                    SourceException = ex,
                    IsServerException = true,
                    ExceptionOnStart = true,
                });
                IsStop = true;
            }
        }
        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop() {
            TcpListener?.Stop();
            IsStop = true;
        }
        /// <summary>
        /// 等待处理新的连接
        /// </summary>
        private void GetAcceptTcpClient() {
            if (TcpListener.Pending()) {
                semap.WaitOne();
                TcpClient tclient = TcpListener.AcceptTcpClient();
                RemoteClient client = new RemoteClient(tclient, bufferLength);
                client.GetData += Client_GetData;
                client.GetException += Client_GetException;
                client.LostConnection += Client_LostConnection;
                //加入客户端集合.
                JoinClientQueues(client);
                //OnGetSocket(client);
                //客户端异步接收
                GetConnection?.Invoke(this, client);
                client.Start();
                semap.Release();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client">被加入的client</param>
        /// <param name="success">如果返回false则表明连接已达到上限。</param>
        protected virtual void JoinClientQueues(RemoteClient client) {
            //虽然有信号量,还是用lock增加系数
            lock (obj) {
                //如果不存在则添加,否则更新
                if (Clients.ContainsKey(client.IPE)) {
                    try {
                        Clients[client.IPE]?.Dispose();
                    }
                    finally {
                        Clients[client.IPE] = client;
                    }
                }
                else {
                    Clients.Add(client.IPE, client);
                }
            }
        }

        private void Client_GetData(RemoteClient client, byte[] buffer, int offset, int length) {
            GetData?.Invoke(this, client, buffer, offset, length);
        }

        private void Client_LostConnection(RemoteClient client, bool isException, Exception? ex = null) {
            Clients.Remove(client.IPE);
            LostConnection?.Invoke(this, client, isException, ex);
            client.Dispose();
        }

        private void Client_GetException(RemoteClient client, Exception ex) {
            GetException?.Invoke(this, new ServerException() {
                SourceException = ex,
                IsRemoteClientException = true,
            });
        }

        public void Dispose() {
            foreach (var client in Clients.Values) {
                client.Dispose();
            }
            TcpListener.Stop();
            IsStop = true;
            GetData = null;
            GetException = null;
            LostConnection = null;
        }
        public event ServerGetConnection? GetConnection;
        public event ServerGetData? GetData;
        public event ServerLostConnection? LostConnection;
        public event ServerGetException? GetException;
    }
}
