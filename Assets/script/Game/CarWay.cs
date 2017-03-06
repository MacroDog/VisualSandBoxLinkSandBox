using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CarWay : MonoBehaviour
{

    public CarWayPoint[] Carwaypoint = new CarWayPoint[2];//表示路段的两头)
    public Transform[] Ways;
    private Transform[] ways;
    private CarWayPoint[] carwaypoint = new CarWayPoint[2];
    //private Transform[] returnWays;

    void Start()
    {
        carwaypoint = Carwaypoint;
        ways = Ways;
    }


    public Transform netWayzhenxiang(Transform last)
    {
        Transform temp = null;
        for (int i = 0; i < ways.Length; i++)
        {
            if (last == ways[i])
            {
                if (i < ways.Length - 1)
                {
                    temp = ways[i++];
                    break;
                }
                else
                {
                    temp = carwaypoint[1].GetComponent<Transform>();
                }
            }
        }
        return temp;
    }
    public Transform nextwayfanxiang(Transform last)
    {
        Transform temp = null;
        for (int i = ways.Length - 1; i > 0; i--)
        {
            if (last == ways[i])
            {
                if (i > 0)
                {
                    temp = ways[i--];
                }
                else
                {
                    temp = carwaypoint[0].GetComponent<Transform>();
                }
            }
        }
        return temp;
    }

    public List<Transform> returnWays(CarWayPoint distence)
    {

       List<Transform> temp = null;
        if (distence == carwaypoint[1])
        {
            temp = new List<Transform>(ways);
            temp.Add(carwaypoint[1].transform);
        }
        if (distence==carwaypoint[0])
        {
           
            Transform[] a = ways;
            Array.Reverse(a);
            temp = new List<Transform>(a);
            temp.Add(carwaypoint[0].transform);

        }
        
        return temp;
    }

    public bool isInTheCarwaypoint(CarWayPoint carwayPoint)
    {
        for (int i = 0; i < carwaypoint.Length; i++)
        {
            if (carwayPoint == carwaypoint[i])
            {
                return true;
            }
        }
        return false;
    }

}
