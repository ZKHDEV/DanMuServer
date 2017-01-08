using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DanMuServer
{
    public class Server
    {
        public Server(int port)
        {
            this.port = port;
            ClientList = new List<ClientInfo>();
        }

        private int port;
        private List<ClientInfo> ClientList;
        Socket listener;

        #region 开启监听
        public void Start()
        {
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Any, port);

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(serverAddress);
            listener.Listen(10);
            listener.BeginAccept(new AsyncCallback(Accept), null);
        }
        #endregion

        #region 接受连接
        private void Accept(IAsyncResult result)
        {
            try
            {
                Socket client = listener.EndAccept(result);

                byte[] buffer = new byte[1024];
                client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Recive), client);
                ClientList.Add(new ClientInfo { Client = client, Buffer = buffer });
                listener.BeginAccept(new AsyncCallback(Accept), null);
            }
            catch
            {
            }
        }
        #endregion

        #region 接收信息
        private void Recive(IAsyncResult result)
        {
            Socket client = result.AsyncState as Socket;

            try
            {
                int length = client.EndReceive(result);
                byte[] buffer = ClientList.First(c => c.Client == client).Buffer;
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                if (message.Trim() != "")
                {
                    Console.WriteLine(message);
                }
                
                //广播弹幕
                ClientList.ForEach(c => { Task.Factory.StartNew(() => Send(c.Client, message)); });

                client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Recive), client);
            }
            catch
            {
                ClientList.Remove(ClientList.Find(c=>c.Client == client));
                client.Close();
                client.Dispose();
            }
        }
        #endregion

        #region 发送信息
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="client">目标Socket</param>
        /// <param name="txt">发送内容</param>
        private void Send(Socket client, string txt)
        {
            byte[] sendbuffer = new byte[1024];
            sendbuffer = Encoding.UTF8.GetBytes(txt);
            try
            {
                IAsyncResult result = client.BeginSend(sendbuffer, 0, sendbuffer.Length, SocketFlags.None, null, null);
                client.EndSend(result);
            }
            catch
            {
                ClientList.Remove(ClientList.Find(c => c.Client == client));
                client.Close();
                client.Dispose();
            }
        }
        #endregion
    }
}
