// Decompiled with JetBrains decompiler
// Type: EDC_Handler.MBusList
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EDC_Handler
{
  public sealed class MBusList
  {
    public ushort StartAddress { get; set; }

    public List<MBusParameter> Parameters { get; set; }

    public MBusList() => this.Parameters = new List<MBusParameter>();

    internal static MBusList Parse(
      byte[] buffer,
      ushort startAddress,
      DeviceVersion version,
      ref ushort offset)
    {
      if (buffer == null || buffer.Length < 2)
        return (MBusList) null;
      ushort uint16 = BitConverter.ToUInt16(buffer, (int) offset);
      if (uint16 == (ushort) 0 || uint16 == ushort.MaxValue)
        return (MBusList) null;
      MBusList mbusList = new MBusList();
      mbusList.StartAddress = (ushort) ((uint) startAddress + (uint) offset);
      for (; uint16 != (ushort) 0 && uint16 != ushort.MaxValue; uint16 = BitConverter.ToUInt16(buffer, (int) offset))
      {
        MBusParameter mbusParameter = MBusParameter.Parse(buffer, startAddress, version, ref offset);
        if (mbusParameter != null)
          mbusList.Parameters.Add(mbusParameter);
      }
      offset += (ushort) 2;
      return mbusList;
    }
  }
}
