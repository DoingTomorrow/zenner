// Decompiled with JetBrains decompiler
// Type: EDC_Handler.FirmwareManager
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace EDC_Handler
{
  public static class FirmwareManager
  {
    public static byte[] ReadFirmwareFromFile(string path)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader sr = new StreamReader((Stream) fileStream))
          return FirmwareManager.ReadFirmware(sr);
      }
    }

    public static byte[] ReadFirmwareFromText(string firmware)
    {
      if (string.IsNullOrEmpty(firmware))
        throw new ArgumentException("The firmware can not be null!");
      using (MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(firmware)))
      {
        using (StreamReader sr = new StreamReader((Stream) memoryStream))
          return FirmwareManager.ReadFirmware(sr);
      }
    }

    private static byte[] ReadFirmware(StreamReader sr)
    {
      string str = sr.ReadLine();
      if (!str.StartsWith("@"))
        throw new ArgumentException("The firmware file is not valid!");
      byte[] destinationArray = new byte[1048576];
      for (int index = 0; index < destinationArray.Length; ++index)
        destinationArray[index] = byte.MaxValue;
      while (!string.IsNullOrEmpty(str))
      {
        if (str.StartsWith("@"))
        {
          int int32 = Convert.ToInt32(str.Substring(1), 16);
          for (str = sr.ReadLine().Trim(); !str.StartsWith("@") && !str.StartsWith("q"); str = sr.ReadLine().Trim())
          {
            string[] strArray = str.Split(' ');
            byte[] sourceArray = new byte[strArray.Length];
            for (int index = 0; index < strArray.Length; ++index)
              sourceArray[index] = Convert.ToByte(strArray[index], 16);
            Array.Copy((Array) sourceArray, 0, (Array) destinationArray, int32, sourceArray.Length);
            int32 += sourceArray.Length;
          }
        }
        if (str.StartsWith("q"))
          break;
      }
      return destinationArray;
    }
  }
}
