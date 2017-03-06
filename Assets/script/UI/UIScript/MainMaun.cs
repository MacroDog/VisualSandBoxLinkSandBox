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
    private bool serverIsOpen=false;
    public Button button;

    // Use this for initialization

    void Start()
    {
        
    }
    protected override void  Init()
    {
        //this.transform.parent = GameObject.FindObjectOfType<Canvas>().transform;
        ServerIP.text = Serv._Serv.LocalIpAdress.ToString();
        ServerPort.text = Serv._Serv.localport.ToString();
        serverIsOpen = Serv._Serv.isAlive;
    }
    private void SetRemoteIPEndPort()
    {
        if (serverIsOpen)
        {
            Serv._Serv.Close();
            button.transform.FindChild("Text").GetComponent<Text>().text = "连接";
            Init();
        }
        else
        {

            IPAddress ip;
            if (IPAddress.TryParse(RemoteIP.text, out ip) && int.Parse(RemotePort.text) != 0)
            {
                Serv._Serv.SetRemoteEndPoint(ip, int.Parse(RemotePort.text));
                bool isopen = Serv._Serv.StartServ();
                Debug.Log(isopen);
                if (isopen)
                {
                    button.transform.FindChild("Text").GetComponent<Text>().text = "关闭";
                }
            }

            else
            {
                Debug.Log("Error Remote IPEndPort");
            }

            Init();
        }
       
    }
    public override EnumUIPlaneType GetUIType()
    {
        return EnumUIPlaneType.MainUI;

    }

}
   
