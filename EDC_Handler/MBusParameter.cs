// Decompiled with JetBrains decompiler
// Type: EDC_Handler.MBusParameter
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public sealed class MBusParameter
  {
    public ushort StartAddress { get; set; }

    public ControlWord0 Control0 { get; set; }

    public List<byte> VifDif { get; set; }

    public ushort? LogHistoryIndex { get; set; }

    public Parameter Pointer { get; set; }

    internal static MBusParameter Parse(
      byte[] buffer,
      ushort startAddress,
      DeviceVersion version,
      ref ushort offset)
    {
      if (buffer == null || buffer.Length < 1)
        return (MBusParameter) null;
      ushort uint16_1 = BitConverter.ToUInt16(buffer, (int) offset);
      if (uint16_1 == (ushort) 0)
        return (MBusParameter) null;
      MBusParameter mbusParameter = new MBusParameter();
      mbusParameter.StartAddress = (ushort) ((uint) startAddress + (uint) offset);
      mbusParameter.Control0 = new ControlWord0(uint16_1);
      offset += (ushort) 2;
      mbusParameter.VifDif = new List<byte>();
      for (int index = 0; index < mbusParameter.Control0.VifDifCount; ++index)
      {
        mbusParameter.VifDif.Add(buffer[(int) offset]);
        ++offset;
      }
      if (mbusParameter.Control0.VifDifCount % 2 != 0)
        ++offset;
      if (mbusParameter.Control0.ControlLogger == ControlLogger.LOG_HISTORY_INDEX)
      {
        mbusParameter.LogHistoryIndex = new ushort?(BitConverter.ToUInt16(buffer, (int) offset));
        offset += (ushort) 2;
      }
      if (mbusParameter.Control0.ControlLogger == ControlLogger.LOG_RESET || mbusParameter.Control0.ParamCode == ParamCode.STORE_SAVE || mbusParameter.Control0.ByteCount > 0 && mbusParameter.Control0.ParamCode == ParamCode.STORE_DIFF_BCD || mbusParameter.Control0.ByteCount > 0 && mbusParameter.Control0.ParamCode == ParamCode.STORE_DIFF || mbusParameter.Control0.ByteCount > 0 && mbusParameter.Control0.ParamCode == ParamCode.VALUE || mbusParameter.Control0.ByteCount > 0 && mbusParameter.Control0.ParamCode == ParamCode.VALUE_BCD)
      {
        ushort uint16_2 = BitConverter.ToUInt16(buffer, (int) offset);
        offset += (ushort) 2;
        mbusParameter.Pointer = EDC_MemoryMap.GetParameter(version, uint16_2);
      }
      else if (mbusParameter.Control0.ByteCount > 0 && mbusParameter.Control0.ParamCode == ParamCode.DATE || mbusParameter.Control0.ByteCount > 0 && mbusParameter.Control0.ParamCode == ParamCode.DATE_TIME)
      {
        if (version.Type != EDC_Hardware.EDC_Radio)
          throw new NotImplementedException();
        bool flag = mbusParameter.Control0.ParamCode == ParamCode.DATE;
        offset += (ushort) 2;
        mbusParameter.Pointer = new Parameter()
        {
          Address = (ushort) 0,
          DifVif = flag ? "6C02" : "6D04",
          Name = "RTC_A",
          Size = flag ? 2 : 4,
          Type = S3_VariableTypes.UINT16
        };
      }
      return mbusParameter;
    }
  }
}
