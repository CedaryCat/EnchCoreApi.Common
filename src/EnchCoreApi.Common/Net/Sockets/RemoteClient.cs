using System.Net;
using System.Net.Sockets;

namespace EnchCoreApi.Common.Net.Sockets {
    public class RemoteClient {
        public RemoteClient(TcpClient connection, int bufferLen = 8 * 1024) {
            bufferLength = bufferLen;
            socket = connection;
            IPE = (IPEndPoint)socket.Client.RemoteEndPoint;
            netStream = new NetworkStream(socket.Client, false);
            SendBufferWriter = new BinaryWriter(new MemoryStream(SendBuffer));
        }
        public readonly int bufferLength;
        public void Dispose() {
            DeInitialize();
            GetData = null;
            GetException = null;
            LostConnection = null;
            Disposed = true;
        }
        public void DeInitialize() {
            IsStop = true;
            socket?.Dispose();
            socket = null;
            netStream?.Dispose();
            netStream = null;
        }
        /// <summary>
        /// 开始读取
        /// </summary>
        public bool Start() {
            try {
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
            catch (Exception) {
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
                    try {
                        GetData?.Invoke(this, RecBuffer, 0, offset);
                    }
                    catch {

                    }
                    netStream.BeginRead(RecBuffer, 0, RecBuffer.Length, new AsyncCallback(EndReader), null);
                }
                else {
                    LostConnection?.Invoke(this, false);
                    IsStop = true;
                }
            }
            catch (Exception ex) {
                GetException?.Invoke(this, ex);
                LostConnection?.Invoke(this, true, ex);
                IsStop = true;
            }
        }
        public void SendData(byte[] bytes) {
            SendData(bytes, 0, bytes.Length);
        }
        public void SendData(byte[] bytes, int offset, int length) {
            try {
                if (!this.Connected || IsStop) {
                    return;
                }
                else {
                    //获取当前流进行写入.
                    NetworkStream nStream = netStream;
                    if (!nStream.CanWrite) {
                        //避免流被关闭,重新从对象中获取流
                        nStream = socket.GetStream();
                        if (!nStream.CanWrite) {
                            //如果还是无法写入,那么认为客户端中断连接.
                            throw new Exception($"失去连接...");
                        }
                    }
                    nStream.Write(bytes, offset, length);
                }
            }
            catch (Exception ex) {
                GetException?.Invoke(this, ex);
                LostConnection?.Invoke(this, true, ex);
                IsStop = true;
            }
        }
        public void SendData(Action<BinaryWriter> writeData) {
            SendBufferWriter.BaseStream.Position = 0;
            writeData(SendBufferWriter);
            SendData(SendBuffer, 0, (int)SendBufferWriter.BaseStream.Position);
        }

        public event RemoteClientGetData GetData;
        public event RemoteClientGetException GetException;
        public event RemoteClientLostConnection LostConnection;

        /// <summary>
        /// 指示是否已释放所占资源，如NetStream,TcpClient
        /// </summary>
        public bool Disposed { get; protected set; }
        /// <summary>
        /// 指示是否暂时关闭数据接收
        /// </summary>
        public bool IsStop { get; protected set; }

        public bool Connected => socket.Connected;
        /// <summary>
        /// 接收缓冲区
        /// </summary>
        private byte[] RecBuffer = new byte[8 * 1024];
        /// <summary>
        /// 发送缓冲区
        /// </summary>
        private byte[] SendBuffer = new byte[8 * 1024];
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
