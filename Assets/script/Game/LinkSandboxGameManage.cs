using UnityEngine;
using System.Collections;

public class LinkSandboxGameManage : MonoBehaviour {

    private static LinkSandboxGameManage _linkSandboxManage;
    public static LinkSandboxGameManage _LinkSandboxManage
    {
        get
        {
            if (_linkSandboxManage!=null)
            {
                _linkSandboxManage = GameObject.FindObjectOfType<LinkSandboxGameManage>();

                if (_linkSandboxManage==null)
                {
                    GameObject Temp = new GameObject();
                    Temp.name = "LinkSandboxGamneManage";
                    Temp.AddComponent<LinkSandboxGameManage>();
                    _linkSandboxManage = Temp.GetComponent<LinkSandboxGameManage>();
                }
            }
            return _linkSandboxManage;
        }
    }
    private bool dataNeedUpdata;
    //public MovieTexture movoe;
    //public static Serv ser { get; private set; }
    [SerializeField]
    private CarControl Car;
    private GetDataProtocol receiveNewData=null;
        
    
    void Awake()
    {
        
       
        DontDestroyOnLoad(this);
        Serv._Serv.receiveMessageEvent += new Serv.ReceiveMessageHandler(ReceiveServerInstruction);

    }
    void Start()
    {
        
        UIManager.Instence.OpenUI(EnumUIPlaneType.MainUI, false, null);

    }
     void Update()
    {
        if (receiveNewData!=null)
        {
            Car.CarInstructal(receiveNewData);
            receiveNewData = null;
        }
    }

    public void ReceiveServerInstruction(object sender , ReceiveMessageArgs Instruction)
    {
       
        receiveNewData = Instruction.data;
        Debug.Log(Instruction.data);

    }
    
   public void sendPushProtocol()
    {
        
    }
    void OnApplicationQuit()
    {
        Serv._Serv.Close();
    }



}
