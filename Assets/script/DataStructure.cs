using UnityEngine;
using System.Collections;

public class DataStructure : MonoBehaviour
{
    public byte head { get; set; }
    public byte carid { get; set; }
    public byte STA { get; set; }
    public byte Locate { get; set; }
    public byte Speed { get; set; }
    public byte CRCCheck { get; set; }
    
    public DataStructure(byte[] DataFrame)
    {
        if (DataFrame[0]==0xaa)
        {
            head = DataFrame[0];
        }

    }



}
