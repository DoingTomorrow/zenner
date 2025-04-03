// Decompiled with JetBrains decompiler
// Type: HandlerLib.BusModuleInfo
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace HandlerLib
{
  public class BusModuleInfo : IComparable<BusModuleInfo>
  {
    private const int MinBufferSize = 17;
    public BusModuleTypes BusModuleType;
    public uint BusModuleSerialNumber;
    public uint FirmwareVersion;
    public uint FirmwareRevision;
    public ushort HardwareVersion;
    public byte Status;

    public static List<BusModuleInfo> GetBusModuleInfoList(byte[] readBuffer, int offset)
    {
      List<BusModuleInfo> busModuleInfoList = new List<BusModuleInfo>();
      while (readBuffer.Length - offset >= 17)
      {
        BusModuleInfo busModuleInfo = new BusModuleInfo(readBuffer, ref offset);
        busModuleInfoList.Add(busModuleInfo);
      }
      busModuleInfoList.Sort();
      return busModuleInfoList;
    }

    public BusModuleInfo()
    {
      this.BusModuleType = BusModuleTypes.OUT2_VMCP;
      this.BusModuleSerialNumber = 4711U;
    }

    public BusModuleInfo(byte[] readBuffer, ref int offset)
    {
      this.BusModuleType = (BusModuleTypes) BitConverter.ToUInt16(readBuffer, offset);
      offset += 2;
      this.BusModuleSerialNumber = BitConverter.ToUInt32(readBuffer, offset);
      offset += 4;
      this.FirmwareVersion = BitConverter.ToUInt32(readBuffer, offset);
      offset += 4;
      this.FirmwareRevision = BitConverter.ToUInt32(readBuffer, offset);
      offset += 4;
      this.HardwareVersion = BitConverter.ToUInt16(readBuffer, offset);
      offset += 2;
      this.Status = readBuffer[offset];
      ++offset;
    }

    public int CompareTo(BusModuleInfo obj)
    {
      int num1 = this.BusModuleType.CompareTo((object) obj.BusModuleType);
      if (num1 != 0)
        return num1;
      int num2 = this.BusModuleSerialNumber.CompareTo(obj.BusModuleSerialNumber);
      if (num2 != 0)
        return num2;
      int num3 = this.FirmwareVersion.CompareTo(obj.FirmwareVersion);
      if (num3 != 0)
        return num3;
      int num4 = this.FirmwareRevision.CompareTo(obj.FirmwareRevision);
      if (num4 != 0)
        return num4;
      int num5 = this.HardwareVersion.CompareTo(obj.HardwareVersion);
      return num5 != 0 ? num5 : this.Status.CompareTo(obj.Status);
    }

    public string GetModuleInfoText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Type: " + this.BusModuleType.ToString() + "; SerialNumber: " + this.BusModuleSerialNumber.ToString());
      stringBuilder.AppendLine("FirmwareVersion: 0x" + this.FirmwareVersion.ToString("x08") + "; Revision: " + this.FirmwareRevision.ToString());
      stringBuilder.AppendLine("HardwareVersion: 0x" + this.HardwareVersion.ToString("x04") + "; Status: " + this.Status.ToString());
      return stringBuilder.ToString();
    }

    public override string ToString()
    {
      return this.BusModuleType.ToString() + "_" + this.BusModuleSerialNumber.ToString();
    }
  }
}
