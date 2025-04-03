// Decompiled with JetBrains decompiler
// Type: PDC_Handler.MBusParameter
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public sealed class MBusParameter
  {
    public ControlWord0 Control0 { get; set; }

    public List<byte> VifDif { get; set; }

    public ushort? Value { get; set; }

    public ushort? LogHistoryIndex { get; set; }

    public ushort? LogResetPointer { get; set; }

    internal static MBusParameter Parse(byte[] buffer, ref ushort addr)
    {
      ushort uint16 = BitConverter.ToUInt16(buffer, (int) addr);
      MBusParameter mbusParameter = new MBusParameter();
      mbusParameter.Control0 = new ControlWord0(uint16);
      addr += (ushort) 2;
      mbusParameter.VifDif = new List<byte>();
      for (int index = 0; index < mbusParameter.Control0.VifDifCount; ++index)
        mbusParameter.VifDif.Add(buffer[(int) addr++]);
      if (mbusParameter.Control0.VifDifCount % 2 != 0)
        ++addr;
      if (mbusParameter.Control0.ParamLog == ParamLog.LOG_HISTORY_INDEX)
      {
        mbusParameter.LogHistoryIndex = new ushort?(BitConverter.ToUInt16(buffer, (int) addr));
        addr += (ushort) 2;
      }
      if (mbusParameter.Control0.ParamLog == ParamLog.LOG_RESET)
      {
        mbusParameter.LogResetPointer = new ushort?(BitConverter.ToUInt16(buffer, (int) addr));
        addr += (ushort) 2;
      }
      bool flag1 = mbusParameter.Control0.Param == Param.VALUE || mbusParameter.Control0.Param == Param.VALUE_BCD || mbusParameter.Control0.Param == Param.DATE || mbusParameter.Control0.Param == Param.DATE_TIME || mbusParameter.Control0.Param == Param.STORE_SAVE || mbusParameter.Control0.Param == Param.STORE_DIFF || mbusParameter.Control0.Param == Param.STORE_DIFF_BCD || mbusParameter.Control0.Param == Param.STORE2_BYTE || mbusParameter.Control0.Param == Param.STORE2_SHORT || mbusParameter.Control0.ParamLog == ParamLog.LOG_CHANNEL;
      bool flag2 = mbusParameter.Control0.Param == Param.LOG_DATE || mbusParameter.Control0.Param == Param.LOG_DATE_TIME || mbusParameter.Control0.Param == Param.LOG_VALUE || mbusParameter.Control0.Param == Param.LOG_VALUE_BCD || mbusParameter.Control0.ParamLog == ParamLog.LOG_NULL;
      if (flag1)
      {
        mbusParameter.Value = new ushort?(BitConverter.ToUInt16(buffer, (int) addr));
        addr += (ushort) 2;
      }
      else if (!flag2)
        throw new NotImplementedException();
      return mbusParameter;
    }

    public override string ToString()
    {
      object[] objArray = new object[4]
      {
        this.Control0 != null ? (object) this.Control0.ToString() : (object) "NULL",
        this.VifDif.Count > 0 ? (object) ("0x" + Util.ByteArrayToHexString(this.VifDif.ToArray())) : (object) "NULL",
        null,
        null
      };
      ushort? logHistoryIndex;
      string str1;
      if (!this.Value.HasValue)
      {
        str1 = "NULL";
      }
      else
      {
        logHistoryIndex = this.Value;
        str1 = "0x" + logHistoryIndex.Value.ToString("X4");
      }
      objArray[2] = (object) str1;
      logHistoryIndex = this.LogHistoryIndex;
      string str2;
      if (!logHistoryIndex.HasValue)
      {
        str2 = "NULL";
      }
      else
      {
        logHistoryIndex = this.LogHistoryIndex;
        str2 = logHistoryIndex.ToString();
      }
      objArray[3] = (object) str2;
      return string.Format("{0} DIFVIF: {1}, Value: {2}, LogHistoryIndex: {3}", objArray);
    }

    internal byte[] CreateMemory()
    {
      if (this.Control0 == null)
        throw new ArgumentNullException("Control0");
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Control0.ControlWord));
      if (this.VifDif != null && this.VifDif.Count > 0)
      {
        byteList.AddRange((IEnumerable<byte>) this.VifDif.ToArray());
        if (this.VifDif.Count % 2 != 0)
          byteList.Add((byte) 0);
      }
      if (this.LogHistoryIndex.HasValue)
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.LogHistoryIndex.Value));
      if (this.LogResetPointer.HasValue)
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.LogResetPointer.Value));
      if (this.Value.HasValue)
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.Value.Value));
      return byteList.ToArray();
    }
  }
}
