using System.Net;
using System.Net.Sockets;

namespace EnchCoreApi.Common.Net.Sockets {
    public class Client : IDisposable {
        /// <summary>
        /// 空构造
        /// </summary>
        public Client(int recBufferL = 8 * 1024, int sendBufferL = 8 * 1024) {
            RecBuffer = new byte[Math.Max(recBufferL, 8 * 1024)];
            SendBuffer = new byte[Math.Max(sendBufferL, 8 * 1024)];
            SendBufferWriter = new BinaryWriter(new MemoryStream(SendBuffer));
        }

        public void Initialize(IPAddress ip, int port) {
            Initialize(new IPEndPoint(ip, port));
        }
        public void Initialize(IPEndPoint address) {
            try {
                IPE = address;
                socket?.Dispose();
                socket = new TcpClient();
                Initialized = true;
            }
            catch (Exception ex) {
                GetException?.Invoke(this, new ClientException() {
                    SourceException = ex,
                    ExceptionOnInitialize = true,
                });
                IsStop = true;
            }
        }
        public void DeInitialize() {
            Initialized = false;
            IsStop = true;
            IPE = null;
            socket?.Dispose();
            socket = null;
            netStream?.Dispose();
            netStream = null;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            DeInitialize();
            GetData = null;
            GetException = null;
            LostConnection = null;
            Disposed = true;
        }
        /// <summary>
        /// 开始读取
        /// </summary>
        public bool Start() {
            try {
                if (!Initialized) {
                    throw new SocketNotInitilizedException();
                }
                if (IsStop) {
                    IsStop = false;
                }
                if (!socket.Connected) {
                    socket.Connect(IPE);
                    netStream?.Dispose();
                    netStream = new NetworkStream(socket.Client, false);
                }
                netStream.BeginRead(RecBuffer, 0, RecBuffer.Length, new AsyncCallback(EndReader), null);
            }
            catch (Exception ex) {
                GetException?.Invoke(this, new ClientException() {
                    SourceException = ex,
                    ExceptionOnStart = true,
                });
                LostConnection?.Invoke(this, true, ex);
                IsStop = true;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 暂时停止读取
        /// </summary>
        public void Stop() {
            IsStop = true;
        }

        private void EndReader(IAsyncResult ir) {
            try {
                if (IsStop) {
                    return;
                }
                var offset = netStream.EndRead(ir);
                if (offset != 0) {
                    GetData?.Invoke(this, RecBuffer, 0, offset);
                    netStream.BeginRead(RecBuffer, 0, RecBuffer.Length, new AsyncCallback(EndReader), null);
                }
                else {
                    LostConnection?.Invoke(this, false);
                    IsStop = true;
                }
            }
            catch (Exception ex) {
                GetException?.Invoke(this, new ClientException() {
                    SourceException = ex,
                    IsRemoteServerException = true,
                    ExceptionOnReadData = true,
                });
                LostConnection?.Invoke(this, true, ex);
                IsStop = true;
            }
        }
        public void SendData(byte[] bytes) {
            SendData(bytes, 0, bytes.Length);
        }
        public void SendData(byte[] bytes, int offset, int length) {
            try {
                if (!this.Connected || IsStop || !Initialized) {
                    return;
                }
                else {
                    EnsureNetStreamCanWrite();
                    netStream.Write(bytes, offset, length);
                }
            }
            catch (Exception ex) {
                GetException?.Invoke(this, new ClientException() {
                    SourceException = ex,
                    IsRemoteServerException = true,
                    ExceptionOnSendData = true,
                });
                LostConnection?.Invoke(this, true, ex);
                IsStop = true;
            }
        }
        public void SendData(Action<BinaryWriter> writeData) {
            SendBufferWriter.BaseStream.Position = 0;
            writeData(SendBufferWriter);
            SendData(SendBuffer, 0, (int)SendBufferWriter.BaseStream.Position);
        }

        private void SendCallback(IAsyncResult result) {
            Tuple<ClientAsyncSendCallback, object> tuple = (Tuple<ClientAsyncSendCallback, object>)result.AsyncState;
            try {
                EnsureNetStreamCanWrite();
                netStream.EndWrite(result);
                tuple.Item1(tuple.Item2);
            }
            catch (Exception ex) {
                GetException?.Invoke(this, new ClientException() {
                    SourceException = ex,
                    IsRemoteServerException = true,
                    ExceptionOnSendData = true,
                });
                LostConnection?.Invoke(this, true, ex);
                IsStop = true;
            }
        }

        public void AsyncSend(byte[] data, int offset, int size, ClientAsyncSendCallback callback, object state = null) {
            try {
                if (!this.Connected || IsStop || !Initialized) {
                    return;
                }
                else {
                    EnsureNetStreamCanWrite();
                    netStream.BeginWrite(data, offset, size, SendCallback, new Tuple<ClientAsyncSendCallback, object>(callback, state));
                }
            }
            catch (Exception ex) {
                GetException?.Invoke(this, new ClientException() {
                    SourceException = ex,
                    IsRemoteServerException = true,
                    ExceptionOnSendData = true,
                });
                LostConnection?.Invoke(this, true, ex);
                IsStop = true;
            }
        }
        public void EnsureNetStreamCanWrite() {
            if (!netStream.CanWrite) {
                //避免流被关闭,重新从对象中获取流
                netStream = socket.GetStream();
                if (!netStream.CanWrite) {
                    //如果还是无法写入,那么认为客户端中断连接.
                    throw new Exception($"失去连接...");
                }
            }
        }

        public event ClientGetData GetData;
        public event ClientLostConnection LostConnection;
        public event ClientGetException GetException;
        /// <summary>
        /// 指示是否释放所占资源，如NetStream,TcpClient
        /// </summary>
        public bool Disposed { get; protected set; }
        /// <summary>
        /// 指示是否暂时关闭数据接收
        /// </summary>
        public bool IsStop { get; protected set; }
        /// <summary>
        /// 指示是否初始化了IPEnd,使用Start()时必须确保此值为true
        /// </summary>
        public bool Initialized { get; protected set; } = false;

        public bool Connected => socket.Connected;
        /// <summary>
        /// 接收缓冲区
        /// </summary>
        public byte[] RecBuffer;
        /// <summary>
        /// 发送缓冲区
        /// </summary>
        private byte[] SendBuffer;
        private BinaryWriter SendBufferWriter;
        /// <summary>
        /// 当前IP地址,端口号
        /// </summary>
        public IPEndPoint IPE { get; set; }
        /// <summary>
        /// 客户端主通信程序
        /// </summary>
        private TcpClient socket;
        /// <summary>
        /// 承载客户端Socket的网络流
        /// </summary>
        private NetworkStream netStream { get; set; }
    }
}
