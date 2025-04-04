// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ObexParser
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace InTheHand.Net
{
  internal static class ObexParser
  {
    internal static void ParseHeaders(
      byte[] packet,
      bool isConnectPacket,
      ref ushort remoteMaxPacket,
      Stream bodyStream,
      WebHeaderCollection headers)
    {
      int num1 = (int) packet[0];
      int hostOrder1 = (int) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, 1));
      int index = 3;
      int num2 = int.MinValue;
      while (index < hostOrder1)
      {
        num2 = index != num2 ? index : throw new InvalidOperationException("Infinite Loop!");
        ObexHeader obexHeader1 = (ObexHeader) packet[index];
        ObexHeader obexHeader2 = obexHeader1;
        if ((uint) obexHeader2 <= 74U)
        {
          switch (obexHeader2)
          {
            case ObexHeader.None:
              return;
            case (ObexHeader) 16:
              remoteMaxPacket = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, index + 2));
              index += 4;
              continue;
            case ObexHeader.Body:
            case ObexHeader.EndOfBody:
              short hostOrder2 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, index + 1));
              bodyStream.Write(packet, index + 3, (int) hostOrder2 - 3);
              index += (int) hostOrder2;
              continue;
            case ObexHeader.Who:
              short hostOrder3 = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, index + 1));
              byte[] numArray = new byte[16];
              Buffer.BlockCopy((Array) packet, index + 3, (Array) numArray, 0, (int) hostOrder3 - 3);
              Guid guid = new Guid(numArray);
              headers.Add(obexHeader1.ToString().ToUpper(), guid.ToString());
              index += (int) hostOrder3;
              continue;
          }
        }
        else
        {
          switch (obexHeader2)
          {
            case ObexHeader.Count:
            case ObexHeader.Length:
            case ObexHeader.Time4Byte:
            case ObexHeader.ConnectionID:
            case ObexHeader.CreatorID:
              int hostOrder4 = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(packet, index + 1));
              index += 5;
              string upper = obexHeader1.ToString().ToUpper();
              string str1 = hostOrder4.ToString();
              if (-1 != Array.IndexOf<string>(headers.AllKeys, upper))
              {
                string str2 = headers.Get(upper);
                if (str1 == str2)
                  continue;
              }
              headers.Add(obexHeader1.ToString().ToUpper(), hostOrder4.ToString());
              continue;
          }
        }
        int hostOrder5 = (int) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, index + 1));
        if (hostOrder5 > 3)
        {
          string str3 = Encoding.BigEndianUnicode.GetString(packet, index + 3, hostOrder5 - 5);
          if (str3 != null)
          {
            int length = str3.IndexOf(char.MinValue);
            if (length > -1)
              str3 = str3.Substring(0, length);
            if (str3 != string.Empty)
              headers.Add(obexHeader1.ToString().ToUpper(), str3);
          }
        }
        index += hostOrder5;
      }
    }
  }
}
