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
    [SerializeField]
    //private ClickPoint 
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
                //changeCarState
            }

        }
    }
   /// <summary>
   /// 改变小车itween目的地
   /// </summary>
    private void changeCarDestination(Transform s)
    {
       
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
    private void changeCarPostion(Vector3 postion)
    {
        this.transform.position = postion;
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
    
}
