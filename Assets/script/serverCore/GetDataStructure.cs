using UnityEngine;
using System.Collections;

public class GetDataStructure : MonoBehaviour
{
    private byte head = 0xAA;
    public int  carId { get; set; }
    public byte STA { get; set; }
    public int  NowLocate { get; set; }
    public int NextLocate { get; set; }
    public float speed { get; set; }
    public byte CRCCheck { get; set; }
    public byte tail { get; set; }
    private byte[] DataFrame { get; set; }

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

    }



}
