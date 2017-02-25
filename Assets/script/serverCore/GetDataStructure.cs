using UnityEngine;
using System.Collections;

public class GetDataStructure 
{
    private byte head = 0xAA;
    public int  carId { get; private set; }
    public byte STA { get; private set; }
    public int  NowLocate { get; private set; }
    public int NextLocate { get; private set; }
    public float speed { get; private set; }
    public byte CRCCheck { get; private set; }
    public byte tail { get; private set; }
    public byte[] DataFrame { get; private set; }

    public GetDataStructure(byte[] DataFrame)
    {
        if (DataFrame[0]==0xAA&&DataFrame.Length==6)
        {
            this.DataFrame = DataFrame;
            carId = System.BitConverter.ToInt32(DataFrame, 1);
            STA = DataFrame[2];
            NowLocate = System.BitConverter.ToInt32(DataFrame, 3);
            speed = System.BitConverter.ToInt32(DataFrame, 4);
            CRCCheck = DataFrame[5];
        }
        else
        {
            Debug.Log("erron GetDataStructure");
        }
    }
   // public void 



}
