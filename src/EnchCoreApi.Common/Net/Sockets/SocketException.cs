namespace EnchCoreApi.Common.Net.Sockets {
    public class ServerException : Exception {
        public Exception SourceException { get; set; }
        public bool ExceptionOnStart { get; set; }
        public bool ExceptionOnListen { get; set; }
        public bool IsServerException { get; set; }
        public bool IsRemoteClientException { get; set; }
        public bool ExceptionOnRemoteClientSendData { get; set; }
        public bool ExceptionOnRemoteClientReadData { get; set; }
        public override string ToString() {
            return
                base.ToString() +
                $"\n" +
                $"ExceptionOnStart {ExceptionOnStart}\n" +
                $"ExceptionOnListen {ExceptionOnListen}\n" +
                $"IsServerException {IsServerException}\n" +
                $"IsRemoteClientException {IsRemoteClientException}\n" +
                $"ExceptionOnRemoteClientSendData {ExceptionOnRemoteClientSendData}\n" +
                $"ExceptionOnRemoteClientReadData {ExceptionOnRemoteClientReadData}\n" +
                SourceException;
        }
    }
    public class ClientException : Exception {
        public Exception SourceException { get; set; }
        public bool ExceptionOnInitialize { get; set; }
        public bool ExceptionOnStart { get; set; }
        public bool IsRemoteServerException { get; set; }
        public bool ExceptionOnSendData { get; set; }
        public bool ExceptionOnReadData { get; set; }
        public override string ToString() {
            return
                base.ToString() +
                $"\n" +
                $"ExceptionOnInitialize {ExceptionOnInitialize}\n" +
                $"ExceptionOnStart {ExceptionOnStart}\n" +
                $"IsRemoteServerException {IsRemoteServerException}\n" +
                $"ExceptionOnSendData {ExceptionOnSendData}\n" +
                $"ExceptionOnReadData {ExceptionOnReadData}\n" +
                SourceException;
        }
    }
    public class SocketNotInitilizedException : Exception {

    }
}
