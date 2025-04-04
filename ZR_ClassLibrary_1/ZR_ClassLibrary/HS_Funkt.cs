// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.HS_Funkt
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Globalization;

#nullable disable
namespace ZR_ClassLibrary
{
  public class HS_Funkt
  {
    public int getMaskScroll(long lValue)
    {
      int maskScroll;
      for (maskScroll = 0; (lValue & 1L) == 0L && maskScroll < 64; ++maskScroll)
        lValue >>= 1;
      return maskScroll;
    }

    public string DeleteSpaces(string instr)
    {
      instr.Trim();
      string[] strArray = instr.Split(' ');
      string str = "";
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] != "")
          str = str + strArray[index] + " ";
      }
      return str;
    }

    public bool StringToMBusSerialNumber(string SerialNumberIn, out int SerialNumberOut)
    {
      SerialNumberIn.Trim();
      SerialNumberIn = SerialNumberIn.ToUpper();
      SerialNumberOut = 0;
      if (SerialNumberIn.Length > 8)
        return false;
      while (SerialNumberIn.Length > 0)
      {
        SerialNumberOut <<= 4;
        int num = (int) SerialNumberIn[0] - 48;
        if (num < 0 || num > 9)
          return false;
        SerialNumberIn = SerialNumberIn.Remove(0, 1);
        SerialNumberOut += num;
      }
      return true;
    }

    public bool MBusSerialNumberToString(int SerialNumberIn, out string SerialNumberOut)
    {
      bool flag = true;
      SerialNumberOut = SerialNumberIn.ToString("X8");
      return flag;
    }

    public char IntToHexChar(byte inValue)
    {
      return ((int) inValue & 15) <= 9 ? (char) (((int) inValue & 15) + 48) : (char) (((int) inValue & 15) + 55);
    }

    public int HexchartoInt(string Value)
    {
      string upper = Value.ToUpper();
      int num = 0;
      if (upper == "0")
        num = 0;
      if (upper == "1")
        num = 1;
      if (upper == "2")
        num = 2;
      if (upper == "3")
        num = 3;
      if (upper == "4")
        num = 4;
      if (upper == "5")
        num = 5;
      if (upper == "6")
        num = 6;
      if (upper == "7")
        num = 7;
      if (upper == "8")
        num = 8;
      if (upper == "9")
        num = 9;
      if (upper == "A")
        num = 10;
      if (upper == "B")
        num = 11;
      if (upper == "C")
        num = 12;
      if (upper == "D")
        num = 13;
      if (upper == "E")
        num = 14;
      if (upper == "F")
        num = 15;
      return num;
    }

    public string makeHexText(int Value, int DigitCount)
    {
      string str = "0x";
      int num1 = Value;
      for (int index = DigitCount; index > 0; --index)
      {
        int num2 = num1 >> (index - 1) * 4;
        str += this.IntToHexChar((byte) (num2 & 15)).ToString();
        num1 = Value;
      }
      return str;
    }

    public int getIntValue(string Value, out int Result)
    {
      Result = 0;
      int intValue;
      if (Value != "")
      {
        string upper = Value.ToUpper();
        int length = upper.Length;
        int num = upper.IndexOf("0X", 0);
        if (num >= 0)
        {
          for (int startIndex = num + 2; startIndex < length; ++startIndex)
          {
            string str = upper.Substring(startIndex, 1);
            Result <<= 4;
            Result += this.HexchartoInt(str);
          }
          intValue = 0;
        }
        else
        {
          try
          {
            Result = int.Parse(Value, NumberStyles.None);
            intValue = 0;
          }
          catch
          {
            Result = 0;
            intValue = 1;
          }
        }
      }
      else
      {
        intValue = 2;
        Result = 0;
      }
      return intValue;
    }

    public int getIntValueDecOfHex(string Value, out int Result)
    {
      int intValueDecOfHex;
      if (Value != "")
      {
        string upper = Value.ToUpper();
        int length = upper.Length;
        int num = upper.IndexOf("0X", 0);
        if (num >= 0)
          Value = upper.Substring(num + 2);
        try
        {
          Result = int.Parse(Value);
          intValueDecOfHex = 0;
        }
        catch
        {
          Result = 0;
          intValueDecOfHex = 1;
        }
      }
      else
      {
        intValueDecOfHex = 2;
        Result = 0;
      }
      return intValueDecOfHex;
    }

    public int getIntValueOfHex(string Value, out int Result)
    {
      Result = 0;
      int intValueOfHex;
      if (Value != "")
      {
        string upper = Value.ToUpper();
        int length = upper.Length;
        int num = upper.IndexOf("0X", 0);
        if (num >= 0)
        {
          for (int startIndex = num + 2; startIndex < length; ++startIndex)
          {
            string str = upper.Substring(startIndex, 1);
            Result <<= 4;
            Result += this.HexchartoInt(str);
          }
          intValueOfHex = 0;
        }
        else
        {
          try
          {
            Result = int.Parse(Value, NumberStyles.AllowHexSpecifier);
            intValueOfHex = 0;
          }
          catch
          {
            Result = 0;
            intValueOfHex = 1;
          }
        }
      }
      else
      {
        intValueOfHex = 2;
        Result = 0;
      }
      return intValueOfHex;
    }

    public static void getZelsiusTime(DateTime PCTime, out long ZelsiusTime)
    {
      DateTime dateTime = new DateTime(1980, 1, 1, 0, 0, 0, 0);
      ZelsiusTime = (PCTime.Ticks - dateTime.Ticks) / 10000000L;
    }

    public static void convertZelsiusTimeToPCTime(long ZelsiusTime, out DateTime PCTime)
    {
      DateTime dateTime = new DateTime(1980, 1, 1, 0, 0, 0, 0);
      PCTime = new DateTime(ZelsiusTime * 10000000L + dateTime.Ticks);
    }
  }
}
