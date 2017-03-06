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
    public List<Transform> WayPoints;
    private Transform carDestination;//itween 下一个点
    private iTween myCarControl;
    private float carSpeed=15;
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
           // Debug.Log(distence);
           // Debug.Log(WayPoints[0].parent.gameObject.name);
           // Debug.Log(WayPoints[0].gameObject.name);
            if (distence <= 0.05)
            {
                if (wayPointCounter<WayPoints.Count)
                {
                    changeCarDestination(WayPoints[wayPointCounter++]);
                }
                //changeCarState
            }
            else
            {
               
            }
        }
    }


   /// <summary>
   /// 改变小车itween目的地
   /// </summary>
    private void changeCarDestination(Transform s)
    {
        
        carDestination = s;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", carDestination.position,
                                            "movetopath", true,
                                            "orienttopath", true,
                                            "speed", carSpeed,
                                            "easetype", iTween.EaseType.linear,
                                            "looktarget", carDestination));
        Debug.Log("change car destination " +carDestination.gameObject.name);
    }


    /// <summary>
    ///改变小车位置用于校验位置 
    /// </summary>
    private void changeCarPostion(CarWayPoint nowCarwayPoint ,CarWayPoint nextCarWayPoint)
    {
        this.transform.position = nowCarwayPoint.transform.position;
        CarWayPoint Min;
        CarWayPoint Max;
        Min = (nowCarwayPoint.CarWayID > nextCarWayPoint.CarWayID) ? nextCarWayPoint : nowCarwayPoint;
        Max= (nowCarwayPoint.CarWayID > nextCarWayPoint.CarWayID) ? nowCarwayPoint : nextCarWayPoint;
        Debug.Log(Min.CarWayID);
        if (Min.getCarWay(Max) !=null)
        {
            WayPoints = Min.getCarWay(Max).returnWays(nextCarWayPoint) ;
            wayPointCounter = 0;
            carDestination = nowCarwayPoint.transform;
            changeCarDestination(WayPoints[0]);
            nowCarwayPoint.RunOnTrigger(this);
            changeCarState(CarState.Run);
            Debug.Log(carDestination.gameObject.name);
        }
        else
        {
            Debug.Log(nowCarwayPoint.gameObject.name + nextCarWayPoint.gameObject.name);
        }
        


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
                    Paush();
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
    public void CarInstructal(GetDataProtocol Data)
    {
       changeCarPostion(MyCarWayPoints[Data.NowLocate], MyCarWayPoints[Data.NextLocate]);
    }
    
    
    //暂停运行 发送暂停命令
    public void Paush()
    {
        iTween.Pause(this.gameObject);
        SendDataProtocol send = new SendDataProtocol(this.CarId, 3);
        Serv._Serv.SendMessage(send.SendData());
        changeCarState(CarState.Pause);
       
    }
    //继续运行 发送运行命令
    public void Continue()
    {
        SendDataProtocol send = new SendDataProtocol(this.CarId, 1);
        Serv._Serv.SendMessage(send.SendData());
        iTween.Resume(this.gameObject);
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
