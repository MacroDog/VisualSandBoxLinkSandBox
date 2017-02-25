using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System;

public class MainMaun : BaseUI
{


    public Text ServerIP;
    public Text ServerPort;

    public Text RemoteIP;
    public Text RemotePort;
    public Button button;

    // Use this for initialization

    void Start()
    {
        
    }
    protected override void  Init()
    {
        //this.transform.parent = GameObject.FindObjectOfType<Canvas>().transform;
        ServerIP.text = Serv._Serv.ReturnServerIPEndPoint().Address.ToString();
        ServerPort.text = Serv._Serv.ReturnServerIPEndPoint().Port.ToString();
    }
    private void SetRemoteIPEndPort()
    {
        IPAddress ip;
        if (IPAddress.TryParse(RemoteIP.text, out ip) && int.Parse(RemotePort.text) != 0)
        {
            Serv._Serv.SetRemoteEndPoint(ip, int.Parse(RemotePort.text));
        }
        else
        {
            Debug.Log("Error Remote IPEndPort");
        }
        Init();
    }
    public override EnumUIPlaneType GetUIType()
    {
        return EnumUIPlaneType.MainUI;

    }

}
   
