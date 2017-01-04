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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StarServer()
    {
        LinkSandboxServerManager._Server.ChangeClinkPort(portInput.text);
        LinkSandboxServerManager._Server.ChangeClinkIP(ipInput.text);
        LinkSandboxServerManager._Server.LinkClinkRequest();
        if (LinkSandboxServerManager._Server.MyServerState==LinkSandboxServerManager.ServerState.Run)
        {
            StarPanel.gameObject.SetActive(false);
        }
    }
}
