using UnityEngine;
using System.Collections;
using System.Net;
using System;

public class GetDataProtocol
{ 
    private byte head = 0xAA;
    public int carId { get; private set; }
    public byte STA { get; private set; }
    public int NowLocate { get; private set; }
    public int NextLocate { get; private set; }
    public float speed { get; private set; }
    public byte[] CRCCheck { get; private set; }
    public byte tail { get; private set; }
    public byte[] DataFrame { get; private set; }
    public GetDataProtocol (byte[] data)
    {

       
        if (data[0]==0xAA&&data.Length>=9&&data[8]==0xfe)
        {
            DataFrame = new byte[9];
            CRCCheck = new byte[2];

            for (int i = 0; i < 9; i++)
            {
                DataFrame[i] = data[i];
            }
            Debug.Log("create new GetDataProtocol");
            byte[] temp = new byte[1];
            temp[0] = data[3];
            Debug.Log("beginget new nowlocate"+data[3].ToString());
            NowLocate = Convert.ToInt16(data[3]);
            Debug.Log("get new nowlocate"+NowLocate);
            temp[0] = data[4];
            NextLocate = Convert.ToInt16(data[4]);
            Debug.Log("get new nextLocate");
            CRCCheck[0] = data[6];
            CRCCheck[1] = data[7];
            Debug.Log("get new crccheck");
            Debug.Log(data[8]);
        }
        else
        {

            Debug.Log("message not accord with protocol");
            //throw new Exception("receive error data");
        }
    }
    public GetDataProtocol()
    {

    }
}
