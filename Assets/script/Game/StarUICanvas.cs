using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class StarUICanvas : MonoBehaviour
{
    [SerializeField]
    private InputField ipInput;
    [SerializeField]
    private InputField portInput;
    [SerializeField]
    private Image StarPanel;
    [SerializeField]
    private Text ServerIp;
    [SerializeField]
    private Text serverPort;
    // Use this for initialization
    void Start()
    {
        //ServerIp.text = LinkSandboxServerManager._Server.serverIPAdress.ToString();
        //serverPort.text = LinkSandboxServerManager._Server.serverPort.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    //public void StarServer()
    //{

    //    LinkSandboxServerManager._Server.ChangeclientPort(portInput.text);
    //    LinkSandboxServerManager._Server.ChangeclientIP(ipInput.text);
    //    LinkSandboxServerManager._Server.LinkClientRequest();
    //    if (LinkSandboxServerManager._Server.myServerState == LinkSandboxServerManager.ServerState.Run)
    //    {
    //        StarPanel.gameObject.SetActive(false);
    //    }
    //}
}
