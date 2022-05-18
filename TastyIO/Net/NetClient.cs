using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TastyIO.Net
{
    public class NetClient : INetClient
    {
        #region Connect/Disconnect
        public void Connect(IPEndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Start/Stop
        public void BeginAcceptClient(AsyncCallback callback)
        {
            throw new NotImplementedException();
        }

        public void EndAcceptClient()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Object/File
        public void SendObject<T>(T data)
        {
            throw new NotImplementedException();
        }

        public void SendObject<T>(T data, int timeout)
        {
            throw new NotImplementedException();
        }

        public T ReciveObject<T>()
        {
            throw new NotImplementedException();
        }

        public T ReciveObject<T>(int timeout)
        {
            throw new NotImplementedException();
        }

        public void SendFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void SendFile(string filePath, int timeout)
        {
            throw new NotImplementedException();
        }

        public void ReciveFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void ReciveFile(string filePath, int timeout)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ExpectedMove
        public NetTransactionStep GetExpectedMove()
        {
            throw new NotImplementedException();
        }

        public void SendExpectedMove(NetTransactionStep step)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
