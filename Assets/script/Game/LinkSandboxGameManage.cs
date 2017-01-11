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
    [SerializeField]
    private CarControl[] Car;

    

    public void ReceiveServerInstruction(GetDataStructure Instruction)
    {
        CarControl ControlCar;
        foreach (var item in Car)
        {
            if (item.CarId == Instruction.carId)
            {
                ControlCar = item;
                item.CarInstructal(Instruction);
            }
        }
        
    }
}
