using UnityEngine;
using System.Collections;

public class SendDataStructure 
{
    public byte head = 0xAA;
    public byte CarID { get; set; }
    public byte STA { get; set; }
    public byte[] CRC { get; set; }
    public byte Tail { get; set; }
    public SendDataStructure(byte[] data)
    {
        CRC = new byte[2];
        if (data[0] == 0xaa)
        {
            data[0] = head;
            data[1] = CarID;
            data[3] = STA;
            data[4] = CRC[0];
            data[5] = CRC[1];
            data[6] = Tail;
        }
        
       
        
    }

    //public byte[] sendByte()
    //{

    //}

    //public void User()
    //{

    //}

}
