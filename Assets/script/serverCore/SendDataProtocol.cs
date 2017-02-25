using UnityEngine;
using System.Collections;
using System;

public class SendDataProtocol
{

    public byte head = 0xAA;
    public byte CarID { get;private set; }
    public byte STA { get;private set; }
    public byte[] CRC { get;private set; }
    public byte Tail = 0xfe;
    private byte[] data;
    public SendDataProtocol(int carid,int carState)
    {
        CRC = new byte[2];
        if (carid>=3)
        {
            throw new Exception(":error carId from sendDataProtocol");
        }
        else
        {
            CarID = Convert.ToByte(carid);
        }
        if (carState >= 3)
        {
            throw new Exception(":error carid from sendDataProtocol");
        }
        else
        {
            STA = Convert.ToByte(carState);
        }
        CRC = CRC16(new byte[] { CarID, STA });
        data = new byte[] { CarID, STA, CRC[0], CRC[1], Tail };


    }
    public byte[] SendData()
    {
        return data;
    }
    //生成crc校验码
    private static byte[] CRC16(byte[] data)
    {
        byte[] returnVal = new byte[2];
        byte CRC16Lo, CRC16Hi, CL, CH, SaveHi, SaveLo;
        int i, Flag;
        CRC16Lo = 0xFF;
        CRC16Hi = 0xFF;
        CL = 0x86;
        CH = 0x68;
        for (i = 0; i < data.Length; i++)
        {
            CRC16Lo = (byte)(CRC16Lo ^ data[i]);//每一个数据与CRC寄存器进行异或
            for (Flag = 0; Flag <= 7; Flag++)
            {
                SaveHi = CRC16Hi;
                SaveLo = CRC16Lo;
                CRC16Hi = (byte)(CRC16Hi >> 1);//高位右移一位
                CRC16Lo = (byte)(CRC16Lo >> 1);//低位右移一位
                if ((SaveHi & 0x01) == 0x01)//如果高位字节最后一位为
                {
                    CRC16Lo = (byte)(CRC16Lo | 0x80);//则低位字节右移后前面补 否则自动补0
                }
                if ((SaveLo & 0x01) == 0x01)//如果LSB为1，则与多项式码进行异或
                {
                    CRC16Hi = (byte)(CRC16Hi ^ CH);
                    CRC16Lo = (byte)(CRC16Lo ^ CL);
                }
            }
        }
        returnVal[0] = CRC16Hi;//CRC高位
        returnVal[1] = CRC16Lo;//CRC低位
        return returnVal;
    }

   
}
