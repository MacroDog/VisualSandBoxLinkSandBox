using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;


public class ReceiveMessageArgs : EventArgs
{
    public GetDataStructure data;
}
public class Serv
{

    public delegate void ReceiveMessageHandler(object sender, ReceiveMessageArgs e);
    public event ReceiveMessageHandler receiveMessageEvent;
    private Socket localSocket;//用于本地socket
    private static readonly object lockHelper = new object();
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
    #region Tcp 连接 接收 发送变量
    //public Conn[] conns;
    //public int MaxConn = 1;//当前项目只允许一个连入

    #endregion
    #region Udp 无连接 接收 发送变量
    public int localport { get; private set; }
    public string UdpIpAdress { get;private set; }
    public int UdpPort { get; private set; }
    public bool isRunning { get; private set; }
    private IPEndPoint RemoteEP;
    private SocketAsyncEventArgs sendToAsyncEventArgs = new SocketAsyncEventArgs();
    private SocketAsyncEventArgs receiveAsyncEventArgs = new SocketAsyncEventArgs();
    private byte[] sendBuffer = new byte[1024] ;
    private byte[] receiveBuffer = new byte[1024];
    #endregion 



    public Serv()
    {

        string AddressIP = string.Empty;
        foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                AddressIP = _IPAddress.ToString();
               
            }
        }
        localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        InitServer();
        //StartServ(AddressIP, 9002);
        
    }


    #region Tcp连接异步接收发送
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
    //            conn.socket.BeginReceive (conn.readbuffer,conn.bufferCount,conn.BuffRamain(),SocketFlags.None, ReceiveCb,conn);
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
    //        if (count<=0)
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

    //           // GetDataStructure temp=new GetDataStructure ()  
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
    #endregion
   
    #region Udp无连接异步接收发送消息
    public void InitServer()
    {
        IPAddress AddressIP = IPAddress.Any;
        foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                AddressIP = _IPAddress;
                
            }
        }
        IPEndPoint localIpEndPort = new IPEndPoint(AddressIP, 0);
        localSocket.Bind(localIpEndPort);
        localport = localIpEndPort.Port;
        Debug.Log(AddressIP+" " +localport);
        //receiveMessage+=new ReceiveMessageHandler() 
    }
    public bool StartServ()
    {
        //检测是否绑定
        if (!localSocket.IsBound)
        {
            IPAddress AddressIP = IPAddress.Any;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    AddressIP = _IPAddress;
                }
            }
            IPEndPoint localIpEndPort = new IPEndPoint(AddressIP, 0);
            localSocket.Bind(localIpEndPort);
            localport = localIpEndPort.Port;
            return false; 
        }
        else
        {
            //检测是否 有远程目标
            if (RemoteEP.Port != 0 && RemoteEP.Address != null)
            {
                CloseServer();
               
                receiveAsyncEventArgs.RemoteEndPoint = RemoteEP;
                receiveAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(receiveCompleted);
                receiveAsyncEventArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                try
                {
                    localSocket.ReceiveAsync(receiveAsyncEventArgs);
                }
                catch 
                {

                    return false;
                }
                
                return true;
            }
            else
            {
                isRunning = false;
                return false;
            }
        }   
        //localSocket.ReceiveAsync()
    }
    public void SendMessage(byte[]message)
    {
        sendToAsyncEventArgs.RemoteEndPoint = RemoteEP;
        sendToAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(sendToCompleted);
        sendToAsyncEventArgs.SetBuffer(message, 0, message.Length);
        localSocket.SendToAsync(sendToAsyncEventArgs);
    }
    public void SetRemoteEndPoint(IPAddress host,int port)
    {
        RemoteEP = new IPEndPoint(host, port);
        StartServ();
    }
    private void receiveCompleted(object sender,SocketAsyncEventArgs args)
    {
        GetDataStructure data = new GetDataStructure(args.Buffer);
        ReceiveMessageArgs receiveMessagesArgs= new ReceiveMessageArgs();
        receiveMessagesArgs.data = data;
        receiveMessageEvent(this, receiveMessagesArgs);
        Debug.Log("receiveCompletd");

    }
    public void sendToCompleted(object sender,SocketAsyncEventArgs Args)
    {
        Debug.Log("completed send message to " + RemoteEP.Address.ToString());
    }
    public void CloseServer()
    {
        try
        {
            localSocket.Close();
        }
        catch 
        {

           
        }
            
    }
    #endregion
    public IPEndPoint ReturnServerIPEndPoint()
    {
       // return localSocket.RemoteEndPoint as IPEndPoint;
        return localSocket.LocalEndPoint as IPEndPoint;
    }

    //private ip
}
