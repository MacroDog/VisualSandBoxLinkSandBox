using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;


public class ReceiveMessageArgs : EventArgs
{
    public GetDataProtocol data;
}

public class Serv
{
    public delegate void ReceiveMessageHandler(object sender, ReceiveMessageArgs e);
    public event ReceiveMessageHandler receiveMessageEvent;
    private Socket localSocket;//用于本地socket
    private static readonly object lockHelper = new object();
    private IPAddress localIpAdress = IPAddress.Any;
    public IPAddress LocalIpAdress { get { return localIpAdress; } }
    private byte[] sendBuffer = new byte[1024];
    private byte[] receiveBuffer = new byte[1024];
    public int localport { get; private set; }
    private static Serv _serv;
    public static Serv _Serv
    {
        get
        {
            if (_serv == null)
            {
                lock (lockHelper)
                {
                    _serv = new Serv();
                }
            }
            return _serv;
        }
    }
    public bool isAlive { get;private set; }//表示服务器是否开启
    #region Tcp 连接 通信变量
    //public Conn[] conns;
    //public int MaxConn = 1;//当前项目只允许一个连入

    #endregion
    #region Udp 无连接  通信变量
    //public IPAddress localIpAdress { get; private set; }
    //public int localport { get; private set; }
    //public string UdpIpAdress { get; private set; }
    //public int UdpPort { get; private set; }
    //public bool isRunning { get; private set; }
    //private IPEndPoint RemoteEP;
    //private SocketAsyncEventArgs sendToAsyncEventArgs = new SocketAsyncEventArgs();
    //private SocketAsyncEventArgs receiveAsyncEventArgs = new SocketAsyncEventArgs();
    #endregion
    #region udp无连接多线程 通信变量
    Thread receiveThread;
    Thread sendThread;
    EndPoint receiveEP = new IPEndPoint(IPAddress.Any, 0);
    private IPEndPoint remoteEP;
    public IPEndPoint remoteEndPort { get; private set; }
    #endregion
    public Serv()
    {
        remoteEndPort = new IPEndPoint(IPAddress.Any, 0);
        sendThread = new Thread(sendData);
        localport = 8899;
        foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                localIpAdress = _IPAddress;

            }
        }
        localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        isAlive = false;
    }


    //#region Tcp连接异步接收发送
    //public int NewIndex()//获取链接索引池，返回负数表示获取失败  返回一个空的或者未使用的conn
    //{
    //    if (conns == null)
    //        return -1;
    //    for (int i = 0; i <= conns.Length; i++)
    //    {
    //        if (conns[i] == null)
    //        {
    //            conns[i] = new Conn();
    //            return i;
    //        }
    //        else if (conns[i].isUse == false)
    //        {
    //            return i;
    //        }
    //    }
    //    return -1;
    //}
    //private void AcceptCb(IAsyncResult ar)
    //{
    //    try
    //    {
    //        Socket socket = localSocket.EndAccept(ar);
    //        int index = NewIndex();
    //        if (index <= 0)
    //        {
    //            socket.Close();//如果连接池已满 拒绝连入
    //        }
    //        else
    //        {
    //            Conn conn = conns[index];
    //            conn.Init(socket);
    //            string str = conn.GetAdress();
    //            conn.socket.BeginReceive(conn.readbuffer, conn.bufferCount, conn.BuffRamain(), SocketFlags.None, ReceiveCb, conn);
    //        }
    //        localSocket.BeginAccept(AcceptCb, null);//再次回调继续接收数据
    //    }
    //    catch
    //    {
    //        Console.Write("接收失败");
    //    }

    //}
    //接收回调函数
    //private void ReceiveCb(IAsyncResult ar)
    //{
    //    Conn conn = (Conn)ar.AsyncState;
    //    try
    //    {
    //        int count = conn.socket.EndReceive(ar);
    //        if (count <= 0)
    //        {
    //            Console.WriteLine("收到{" + conn.GetAdress() + "}断开连接");
    //            conn.Close();
    //            return;
    //        }
    //        else
    //        {
    //            GetDataStructure temp = new GetDataStructure(conn.readbuffer);
    //            conn.ClearBuffer();
    //            LinkSandboxGameManage._LinkSandboxManage.ReceiveServerInstruction(temp);

    //            // GetDataStructure temp=new GetDataStructure ()  
    //        }
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    //public void StartServ(string host, int port)
    //{
    //    conns = new Conn[MaxConn];
    //    for (int i = 0; i < MaxConn; i++)
    //    {
    //        Debug.Log(i);
    //        conns[i] = new Conn();
    //    }
    //    localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //    IPAddress IPAdr = IPAddress.Parse(host);
    //    IPEndPoint ipEp = new IPEndPoint(IPAdr, port);
    //    Debug.Log(ipEp);
    //    localSocket.Bind(ipEp);
    //    localSocket.Listen(MaxConn);
    //    localSocket.BeginAccept(AcceptCb, null);//异步开始等待客户端连入，并且当有客户端连入时回调AccpetCb

    //}
    //#endregion

    //#region Udp无连接异步接收发送消息
    //public void InitServer()
    //{
    //    IPAddress AddressIP = IPAddress.Any;
    //    foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
    //    {
    //        if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
    //        {
    //            AddressIP = _IPAddress;

    //        }
    //    }
    //    IPEndPoint localIpEndPort = new IPEndPoint(AddressIP, 9001);
    //    localSocket.Bind(localIpEndPort);
    //    localport = localIpEndPort.Port;
    //    SerIPAddressa = AddressIP.ToString();
    //    SerEndProt = localIpEndPort.Port.ToString();

    //    Debug.Log(SerIPAddressa + " " + SerEndProt);

    //}
    //public bool StartServ()
    //{
    //    bool isStart = false;
    //    //检测是否绑定
    //    if (!localSocket.IsBound)
    //    {
    //        IPAddress AddressIP = IPAddress.Any;
    //        foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
    //        {
    //            if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
    //            {
    //                AddressIP = _IPAddress;
    //            }
    //        }
    //        IPEndPoint localIpEndPort = new IPEndPoint(AddressIP, 0);
    //        localSocket.Bind(localIpEndPort);
    //        localport = localIpEndPort.Port;
    //        isStart = false;
    //        return isStart;
    //    }
    //    else
    //    {
    //        //检测是否 有远程目标
    //        if (RemoteEP.Port != 0 && RemoteEP.Address != null)
    //        {


    //            sendToAsyncEventArgs.RemoteEndPoint = RemoteEP;
    //            sendToAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(sendToCompleted);

    //            receiveAsyncEventArgs.RemoteEndPoint = RemoteEP;
    //            receiveAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(receiveCompleted);
    //            receiveAsyncEventArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
    //            try
    //            {
    //                localSocket.ReceiveFromAsync(receiveAsyncEventArgs);
    //                Debug.Log("begin receive" + receiveAsyncEventArgs.RemoteEndPoint.ToString() + localSocket.IsBound);
    //                isStart = true;
    //            }
    //            catch
    //            {
    //                isStart = false;

    //            }


    //        }
    //        else
    //        {
    //            CloseServer();
    //            isRunning = false;
    //            isStart = false;
    //        }
    //    }
    //    Debug.Log(isStart);
    //    return isStart;

    //}
    //public void SendMessage(byte[] message)
    //{

    //    sendToAsyncEventArgs.SetBuffer(message, 0, message.Length);
    //    localSocket.SendToAsync(sendToAsyncEventArgs);
    //}
    //public void SetRemoteEndPoint(IPAddress host, int port)
    //{
    //    RemoteEP = new IPEndPoint(host, port);
    //    StartServ();
    //}
    //private void receiveCompleted(object sender, SocketAsyncEventArgs args)
    //{
    //    GetDataProtocol data = new GetDataProtocol(args.Buffer);
    //    ReceiveMessageArgs receiveMessagesArgs = new ReceiveMessageArgs();
    //    receiveMessagesArgs.data = data;
    //    receiveMessageEvent(this, receiveMessagesArgs);
    //    Debug.Log("receiveCompletd");
    //    Debug.Log(receiveBuffer);

    //}
    //public void sendToCompleted(object sender, SocketAsyncEventArgs Args)
    //{
    //    Debug.Log("completed send message to " + RemoteEP.Address.ToString());
    //}
    //public void CloseServer()
    //{
    //    try
    //    {
    //        localSocket.Close();
    //    }
    //    catch
    //    {


    //    }

    //}
    //#endregion
    #region 无连接多线程接收发送消息
    public bool StartServ()
    {

        if (localSocket.IsBound)
        {
            Close();
        }
        try
        {

            IPEndPoint temp = new IPEndPoint(localIpAdress, localport);
            localSocket.Bind(temp);

        }
        catch
        {
            isAlive= false;
        }
        if (remoteEndPort != null)
        {
            try
            {
                receiveThread = new Thread(receiveData);
                receiveThread.Start();
                Debug.Log("begin receive" + remoteEndPort);
                isAlive= true;
            }
            catch (Exception)
            {
                Debug.Log("server fail to Receive ");
                isAlive= false;
            }

        }
        else
        {
            Debug.Log("remoteEndPort is null");
            isAlive= false;
        }
        return isAlive;
    }
    private void receiveData()
    {
        while (true)
        {
            localSocket.ReceiveFrom(receiveBuffer, ref receiveEP);
            Debug.Log("receiveMessage");
            handleWhenGetMessage();
        }
    }
    private void handleWhenGetMessage()
    {
        ReceiveMessageArgs e = new ReceiveMessageArgs();

        GetDataProtocol data = new GetDataProtocol(receiveBuffer);
        if (data.DataFrame != null)
        {
            Debug.Log("have new getData");

            e.data = data;
            receiveMessageEvent(this, e);
        }
    }
    public void SendMessage(byte[] data)
    {
        sendBuffer = data;
        if (!sendThread.IsAlive)
        {
            sendThread = new Thread(sendData);
        }
        else
        {
            Debug.Log(sendThread.IsAlive);
        }
                
       
    }
    private void sendData()
    {
        Debug.Log("Begin send data");
        localSocket.SendTo(sendBuffer, remoteEndPort);
        Debug.Log("Achieve send");
        sendThread.Abort();
       
    }
    public void SetRemoteEndPoint(IPAddress ip, int i)
    {
        remoteEndPort.Address = ip;
        remoteEndPort.Port = i;
    }
    public void Close()
    {
        if (receiveThread.IsAlive==true )
        {
            receiveThread.Abort();
        }
        if (sendThread.IsAlive==false)
        {
            sendThread.Abort();
        }
        localSocket.Close();
        isAlive = false;
    }
   
    #endregion
}
