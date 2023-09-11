namespace EnchCoreApi.Common.Net.Sockets {
    public delegate void ClientGetData(Client client, byte[] buffer, int offset, int length);
    public delegate void ClientLostConnection(Client client, bool isException, Exception ex = null);
    public delegate void ClientGetException(Client client, ClientException ex);

    public delegate void ClientAsyncSendCallback(object state);

    public delegate void RemoteClientGetData(RemoteClient client, byte[] buffer, int offset, int length);
    public delegate void RemoteClientLostConnection(RemoteClient client, bool isException, Exception ex = null);
    public delegate void RemoteClientGetException(RemoteClient client, Exception ex);

    public delegate void ServerGetConnection(Server server, RemoteClient connection);
    public delegate void ServerGetData(Server server, RemoteClient connection, byte[] buffer, int offset, int length);
    public delegate void ServerLostConnection(Server server, RemoteClient connection, bool isException, Exception ex = null);
    public delegate void ServerGetException(Server server, ServerException ex, RemoteClient connection = null);
}
