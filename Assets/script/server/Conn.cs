using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class Conn : MonoBehaviour {

    private const int BUFFER_SIZE = 1024;//每个客户端收到信息缓存空间大小
    public Socket socket;
    public bool isUse { get; set; }
    public  byte[] readbuffer = new byte[BUFFER_SIZE];
    public int bufferCount = 0;




    public Conn()
    {
        readbuffer = new byte[BUFFER_SIZE];

    }
    //初始化
    public void Init(Socket socket)
    {
        socket = this.socket;
        isUse = true;
        //bufferCount;
    }

    //缓存区剩余容量
    public int BuffRamain()
    {
        return BUFFER_SIZE - bufferCount;
    }
    public string GetAdress()
    {
        if (isUse != true)
            return "无法获取地址";
        return socket.RemoteEndPoint.ToString();



    }
    public void Close()
    {
        if (!isUse)
            return;
        Debug.Log("断开连接");
        socket.Close();
        isUse = false;
    }
}
