using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;

public class LinkSandboxServerManager
{
    public enum ServerState
    {
        Initialize,
        Run,
        Close,
        none,
    }
    public ServerState MyServerState { get; set; }
    public static  LinkSandboxServerManager _Server
    {
        get
        {
            if (_server==null)
            {
                _server = new LinkSandboxServerManager();
            }

            return _server;
        }
    }
    private int ClinkPort = 7788;
    private IPAddress ClinkIP;
    private   Socket ServerSocket;
    private int ServerPort = 8080;
    private  IPAddress ServerIPAdress;
    private static LinkSandboxServerManager _server=new LinkSandboxServerManager ();
    private Thread listenClink;
    private byte[] receiveData;
    // Use this for initialization

    private   LinkSandboxServerManager()
    {
        Debug.Log("ss");
        MyServerState = ServerState.Initialize;
        ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        ServerIPAdress = GetIPV4();
        ServerSocket.Bind(new IPEndPoint(ServerIPAdress, ServerPort));
        listenClink = new Thread(ReceiveFromClink);

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
    private void StarRunning()
    {
        try
        {
            ServerSocket.Connect(new IPEndPoint(ClinkIP, ClinkPort));
            listenClink.Start();
            MyServerState = ServerState.Run;
            Debug.Log("Star");
        }
        catch
        {

            Debug.Log("CantFindClink");

        }

    }
    private void ReceiveFromClink()
    {
        while (true)
        {
            if (MyServerState == ServerState.Run)
            {
                ServerSocket.Receive(receiveData);
            }
            else
            {
                listenClink.Abort();
            }
        }
    }
    public void ChangeClinkIP(string ip)
    {
        try
        {
            IPAddress temp = IPAddress.Parse(ip);

        }
        catch
        {


        }
    }
    public void ChangeClinkPort(string port)
    {
        int temp = int.Parse(port);
        if (temp > 600&& temp<10000)
        {
            ClinkPort = temp;
        }
        
    }
    public void LinkClinkRequest()
    {
        if (ClinkIP != null && MyServerState == ServerState.Initialize)
        {

            StarRunning();
        }
    }
}
