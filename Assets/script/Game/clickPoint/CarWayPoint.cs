using UnityEngine;
using System.Collections;


public class CarWayPoint : MonoBehaviour
{
   
    //public void 
    public int CarWayID;
    public int carID { get; protected set; }
    //public CarWayPoint[] NextCarWayPoint;
    [SerializeField]
    public CarWay[] wayPoint;
    public bool[] isZhenxiang
    {
        get;
        private set;
    }
    
    void Start()
    {
        
    }
    // Update is called once per frame
   
    public virtual void RunOnTrigger(CarControl carControl)
    {
        
    }
    public void SetId()
    {
           
    }

    //private void setis
    public CarWay getCarWay(CarWayPoint nextWayPoint)
    {
        for (int i = 0; i < wayPoint.Length; i++)
        {
            if (wayPoint [i].isInTheCarwaypoint(nextWayPoint))
            {
                return wayPoint[i];
            }
        }
        return null;
    }
   
   
}
