using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace ConsoleApplication1
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();
        public const Int32 Port = 1337;
        public const Int32 BufferSize = 65536;


        static void Main(string[] args)
        {
            TcpListener ServerSocket = new TcpListener(Port);
            TcpClient ClientSocket = default(TcpClient);
            Int32 Counter = 0;

            ServerSocket.Start();
            Console.WriteLine("Server started ...");
            Counter = 0;
            for (; ; )
            {
                Counter++;
                ClientSocket = ServerSocket.AcceptTcpClient();

                byte[] bytesFrom = new byte[BufferSize];
                string dataFromClient = null;

                NetworkStream networkStream = ClientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, (Int32)ClientSocket.ReceiveBufferSize);
                dataFromClient = System.Text.Encoding.UTF8.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                if (clientsList[dataFromClient] == null)
                {
                    clientsList.Add(dataFromClient, ClientSocket);
                    broadcast(dataFromClient + " Joined ", dataFromClient, false);
                    Console.WriteLine(dataFromClient + " joined");
                    HandleClient Client = new HandleClient();
                    Client.startClient(ClientSocket, dataFromClient, clientsList, dataFromClient);
                }
                //else
                //    broadcast("occupied", "qwerty", false);
            }

            ClientSocket.Close();
            ServerSocket.Stop();
            Console.WriteLine("exit");
            Console.ReadLine();
        }

        public static void broadcast(String msg, String uName, Boolean flag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                if (flag == true)
                {
                    broadcastBytes = Encoding.UTF8.GetBytes(uName + ": " + msg);
                }
                else
                {
                    broadcastBytes = Encoding.UTF8.GetBytes(msg);
                }

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }
    }


    public class HandleClient
    {
        TcpClient clientSocket;
        string clNo;
        String Key;
        Hashtable clientsList;
        Thread ctThread = null;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList, String Key)
        {
            this.Key = Key;
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsList = cList;
            ctThread = new Thread(doChat);
            ctThread.Start();
        }

        void Disconnect()
        {
            //Program.broadcast("disconnected", clNo, false);
            Console.WriteLine(clNo + " disconnected");
            Program.clientsList.Remove(Key);
            //clientSocket.Close();
            ctThread.Abort();
            ctThread.Join();
        }

        private void doChat()
        {
            Int32 requestCount = 0;
            byte[] bytesFrom = new byte[Program.BufferSize];
            String dataFromClient = null;
            Byte[] sendBytes = null;
            String serverResponse = null;
            String rCount = null;
            requestCount = 0;
           
            for (; ; )
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.UTF8.GetString(bytesFrom);
                    if (String.IsNullOrEmpty(dataFromClient.Replace("\0", String.Empty)))
                    {
                        Program.broadcast(clNo + " disconnected", clNo, false);
                        Console.WriteLine(clNo + " disconnected");
                        Program.clientsList.Remove(Key);
                        //clientSocket.Close();
                        ctThread.Abort();
                        ctThread.Join();
                        break;
                    }
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine(clNo + " : " + dataFromClient);
                    rCount = Convert.ToString(requestCount);
                    Program.broadcast(dataFromClient, clNo, true);
                }
                catch(ThreadAbortException asd)
                {

                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.ToString());
                    Disconnect();
                }
            }
        }
    }
}