// Decompiled with JetBrains decompiler
// Type: HandlerLib.DebugQueueEntry
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace HandlerLib
{
  public class DebugQueueEntry : IComparable<DebugQueueEntry>
  {
    public string LogEvent;
    public DateTime EventTime;
    public ushort Tick;
    public byte Uint8_param;
    public uint Uint32_param;

    public DebugQueueEntry(
      string theEvent,
      DateTime theTime,
      ushort theTick,
      byte uint8Param = 0,
      uint uint32Param = 0)
    {
      this.LogEvent = theEvent;
      this.EventTime = theTime;
      this.Tick = theTick;
      this.Uint8_param = uint8Param;
      this.Uint32_param = uint32Param;
    }

    public DebugQueueEntry(SortedList<int, string> queueEvents, byte[] buffer, int offset)
    {
      this.LogEvent = queueEvents == null || !queueEvents.ContainsKey((int) buffer[offset]) ? "UnDef_0x" + buffer[offset].ToString("x02") : queueEvents[(int) buffer[offset]];
      this.Uint8_param = buffer[offset + 1];
      this.Tick = BitConverter.ToUInt16(buffer, offset + 2);
      this.EventTime = this.GetDateTimeFromSeconds2000(BitConverter.ToUInt32(buffer, offset + 4));
      this.Uint32_param = BitConverter.ToUInt32(buffer, offset + 8);
    }

    private uint GetSeconds2000(DateTime dateTime)
    {
      return (uint) dateTime.Subtract(new DateTime(2000, 1, 1)).TotalSeconds;
    }

    private DateTime GetDateTimeFromSeconds2000(uint secs2000)
    {
      return new DateTime(2000, 1, 1).AddSeconds((double) secs2000);
    }

    public int CompareTo(DebugQueueEntry obj)
    {
      if (obj == null)
        return 1;
      int num = this.EventTime.CompareTo(obj.EventTime);
      return num != 0 ? num : this.Tick.CompareTo(obj.Tick);
    }
  }
}
