using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;

public class Serv  {
    public Socket listenfd;//用于监听客户端
    public Conn[] conns;
    public int MaxConn = 1;//当前项目只允许一个连入
    private static readonly object lockHelper = new object();
    private static Serv _serv;
    public static Serv _Serv
    {
        get
        {
            if (_serv==null)
            {
                lock (lockHelper)
                {
                    _serv = new Serv();
                }
            }
            return _serv;
        }
        
    }
    
    //获取链接索引池，返回负数表示获取失败  返回一个空的或者未使用的conn
    public int NewIndex()
    {
        if (conns == null)
            return -1;
        for (int i = 0; i <= conns.Length; i++)
        {
            if (conns[i] == null) 
            {
                conns[i] = new Conn();
                return i;
            }
            else if(conns[i].isUse == false)
            {
                return i;
            }
        }
        return  -1;
    }

    
    public void StartServ(string host, int port)
    {
        conns = new Conn[MaxConn];
        for (int i=0; i <= MaxConn; i++)
        {
            conns[i] = new Conn();
        }
        listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress IPAdr = IPAddress.Parse(host);
        IPEndPoint ipEp = new IPEndPoint(IPAdr, port);
        listenfd.Bind(ipEp);
        listenfd.BeginAccept(AcceptCb, null);//异步开始等待客户端连入，并且当有客户端连入时回调AccpetCb
    }

    private void AcceptCb(IAsyncResult ar)
    {
        try
        {
            Socket socket = listenfd.EndAccept(ar);
            int index = NewIndex();
            if (index <= 0)
            {
                socket.Close();//如果连接池已满 拒绝连入
            }
            else
            {
                Conn conn = conns[index];
                conn.Init(socket);
                string str = conn.GetAdress();
                conn.socket.BeginReceive (conn.readbuffer,conn.bufferCount,conn.BuffRamain(),SocketFlags.None, ReceiveCb,conn);
            }
            listenfd.BeginAccept(AcceptCb, null);//再次回调继续接收数据
        }
        catch
        {
            Console.Write("接收失败");
        }

    }
    //接收回调函数
    private void ReceiveCb(IAsyncResult ar)
    {
        Conn conn = (Conn)ar.AsyncState;
        try
        {
            int count = conn.socket.EndReceive(ar);
            if (count<=0)
            {
                Console.WriteLine("收到{" + conn.GetAdress() + "}断开连接");
                conn.Close();
                return;
            }
            else
            {
                GetDataStructure temp = new GetDataStructure(conn.readbuffer);
                conn.ClearBuffer();
                LinkSandboxGameManage._LinkSandboxManage.ReceiveServerInstruction(temp);

               // GetDataStructure temp=new GetDataStructure ()  
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    // Use this for initialization
    //public 
    
}
