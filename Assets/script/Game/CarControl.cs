using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarControl : MonoBehaviour
{
    public enum CarState
    {
        Stop,
        Pause,
        Run,
    }
    public CarState myCarState { get; set; }
    public List<Transform> WayPoint;
    private Transform carDestination;//itween 下一个点
    private iTween myCarControl;
    private float carSpeed;
    private float carOffset = 0.05f;
   
    public int CarId { get; set; }
    [SerializeField]
    private CarWayPoint[] MyCarWayPoints =new CarWayPoint [19];
    private int wayPointCounter=0;
    //private Transform[] carways;
    //[SerializeField]
    //private ClickPoint asd;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (myCarState == CarState.Run)
        {
            float distence = Vector3.Distance(this.transform.position, carDestination.position);
            if (distence <= 0.05)
            {
                if (wayPointCounter<WayPoint.Count)
                {
                    changeCarDestination(WayPoint[wayPointCounter++]);
                }
                //changeCarState
            }
        }
    }


   /// <summary>
   /// 改变小车itween目的地
   /// </summary>
    private void changeCarDestination(Transform s)
    {
        carDestination = s;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", carDestination,
                                            "movetopath", true,
                                            "orienttopath", true,
                                            "speed", carSpeed,
                                            "easetype", iTween.EaseType.linear,
                                            "looktarget", carDestination));
    }


    /// <summary>
    ///改变小车位置用于校验位置 
    /// </summary>
    private void changeCarPostion(CarWayPoint nowCarwayPoint ,CarWayPoint nextCarWayPoint)
    {
        this.transform.position = nowCarwayPoint.transform.position;
        WayPoint = nextCarWayPoint.getCarWay(nextCarWayPoint).returnWays(nextCarWayPoint);
        wayPointCounter = 0;
        carDestination = nowCarwayPoint.transform;
        nowCarwayPoint.RunOnTrigger(this);



    }
   
    /// <summary>
    /// 改变小车状态
    /// </summary>
    /// <param name="s"></param>
    private void changeCarState(CarState s)
    {
        if (s != myCarState)
        {
            switch (s)
            {
                case CarState.Stop:
                    myCarState = s;
                    break;
                case CarState.Pause:
                    myCarState = s;
                    break;
                case CarState.Run:
                    myCarState = s;
                    break;
            }
        }
    }
    
    /// <summary>
    /// 初始化小车 回到原点，归零目标点
    /// </summary>
   private void initializeCar()
    {

    }
    /// <summary>
    /// 通过指令改变小车
    /// </summary>
    public void CarInstructal(GetDataStructure Data)
    {
        //改变
        //changeCarPostion()
        //changeCarDestination()
    }

    
    //暂停运行 发送暂停命令
    public void Push()
    {
        SendDataProtocol send = new SendDataProtocol(this.CarId, 3);
        Serv._Serv.SendMessage(send.SendData());
    }


    //继续运行 发送运行命令
    public void Continue()
    {
        SendDataProtocol send = new SendDataProtocol(this.CarId, 1);
        Serv._Serv.SendMessage(send.SendData());
    }
    public CarWayPoint getCarWayPointById(int i)
    {
        CarWayPoint temp = null;
        if (i<MyCarWayPoints.Length)
        {
            temp = MyCarWayPoints[i];
        }
        return temp;
    }
 
    
}
