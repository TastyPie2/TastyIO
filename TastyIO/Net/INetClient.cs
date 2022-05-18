using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TastyIO.Net
{
    public interface INetClient : IDisposable
    {
        #region connect/disconnect
        void Connect(IPEndPoint endPoint);

        void Disconnect();
        #endregion

        #region Start/Stop
        void BeginAcceptClient(AsyncCallback callback);

        void EndAcceptClient();
        #endregion

        #region Object
        void SendObject<T>(T data);

        void SendObject<T>(T data, int timeout);

        T ReciveObject<T>();

        T ReciveObject<T>(int timeout);
        #endregion

        #region File
        void SendFile(string filePath);

        void SendFile(string filePath, int timeout);

        void ReciveFile(string filePath);

        void ReciveFile(string filePath, int timeout);
        #endregion

        #region ExpectedStep
        NetTransactionStep GetExpectedMove();

        void SendExpectedMove(NetTransactionStep step);
        #endregion
    }
}
