using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;

public class LinkSandboxServerManager
{
    public delegate void ServerSendData();
    //public event 
    public enum ServerState
    {
        Initialize,
        Run,
        Close,
        none,
    }
    public ServerState myServerState { get; set; }//服务器状态
    private static readonly object lockHelper = new object();
    public static  LinkSandboxServerManager _Server
    {
        get
        {
            lock (lockHelper)
            {
                if (_server == null)
                {
                    _server = new LinkSandboxServerManager();
                }
            }

            return _server;
        }
    }//单例化
    private int clientPort;//客户端端口
    private IPAddress clientIP;
    private   Socket serverSocket;
    private Socket clientSocket;
    public int serverPort { get; set; }
    public IPAddress serverIPAdress { get; set; }
    private static LinkSandboxServerManager _server=new LinkSandboxServerManager ();
    private Thread listenClientThread;
    private Thread accpetClientThread;
    private byte[] receiveData=new byte [9];
   
    // Use this for initialization

    private   LinkSandboxServerManager()
    {
        Debug.Log("ss");
        myServerState = ServerState.Initialize;
        serverPort = 8080;
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        serverIPAdress = GetIPV4();
        //serverSocket.Listen(1);
        serverSocket.Bind(new IPEndPoint(serverIPAdress, serverPort));
        receiveData = new byte[7];
       
        listenClientThread = new Thread(listinClink);
        accpetClientThread = new Thread(AcceptClient);
        myServerState = ServerState.Run;
        accpetClientThread.Start();
        

    }


    private IPAddress GetIPV4()
    {
        IPAddress A = null;
        IPAddress[] myIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        for (int i = 0; i < myIp.Length; i++)
        {
            if (myIp[i].AddressFamily == AddressFamily.InterNetwork)
            {
                A = myIp[i];
                break;
            }
        }
        return A;

    }
    //private void StarRunning()
    //{
    //    try
    //    {
    //        serverSocket.Connect(new IPEndPoint(clientIP, clientPort));
    //        listenClientThread.Start();
    //        myServerState = ServerState.Run;
    //        Debug.Log("Star");
    //    }
    //    catch
    //    {

    //        Debug.Log("CantFindClient");
            
    //    }

    //}


    /// <summary>
    /// 接收客户端
    /// </summary>
    private void AcceptClient()
    {
        while (true)
        {
            Debug.Log("NUllClient");
            if (myServerState == ServerState.Run)
            {
                
                clientSocket = serverSocket.Accept();
                if (clientSocket!=null)
                {
                    Debug.Log("Accpet");
                    listenClientThread.Start();
                   
                }
            }
            else
            {
                Debug.Log("destoryAccpet");
                //accpetClientThread.Abort();
            }
        }
    }


    private void listinClink()
    {
        while (true)
        {
            clientSocket.Receive(receiveData);
        }
    }
    public void ChangeclientIP(string ip)
    {
        try
        {
            IPAddress temp = IPAddress.Parse(ip);

        }
        catch
        {


        }
    }
    public void ChangeclientPort(string port)
    {
        Debug.Log(port);
        if (port!= "")
        {
            int temp = int.Parse(port);
            if (temp > 1023 && temp < 65535)
            {
                clientPort = temp;
            }
            
        }
        
        
    }
    public void ChangeclientPort(int port)
    {
        if (port  > 1023 && port < 65535)
        {
            clientPort = port;
        }
    }
    public void SendData(byte[] SendData)
    {
        try
        {
            clientSocket.Send(SendData);
        }
        catch (System.Exception)
        {

            throw;
        }
    }
    private void sendData(byte[] sendData)
    {

    }
    //public void LinkClientRequest()
    //{
    //    if (clientIP != null && myServerState == ServerState.Initialize)
    //    {

    //        StarRunning();
    //    }
    //}
}
