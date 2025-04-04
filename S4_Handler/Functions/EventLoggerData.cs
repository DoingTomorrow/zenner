// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.EventLoggerData
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using SmartFunctionCompiler;
using System;

#nullable disable
namespace S4_Handler.Functions
{
  public class EventLoggerData : IComparable<EventLoggerData>
  {
    public int No { get; set; }

    public DateTime LoggerTime { get; set; }

    public LoggerEventTypes Event { get; set; }

    public string EventParameter { get; set; }

    public EventLoggerData(byte[] byteArray, ref int scanOffset, int no, bool fromMemory = false)
    {
      this.No = no;
      this.LoggerTime = !fromMemory ? ByteArrayScanner.ScanDateTime(byteArray, ref scanOffset) : CalendarBase2000.Cal_GetDateTime(ByteArrayScanner.ScanUInt32(byteArray, ref scanOffset));
      this.Event = (LoggerEventTypes) ByteArrayScanner.ScanByte(byteArray, ref scanOffset);
      byte num = ByteArrayScanner.ScanByte(byteArray, ref scanOffset);
      if (this.Event == LoggerEventTypes.SmartFunctionEvent)
        this.EventParameter = ((SmartFunctionLoggerEventType) num).ToString();
      else if (this.Event == LoggerEventTypes.OperationChanged)
        this.EventParameter = ((OperationChanges) num).ToString();
      else
        this.EventParameter = num.ToString();
    }

    public int CompareTo(EventLoggerData compareObject)
    {
      int num1 = this.LoggerTime.CompareTo(compareObject.LoggerTime);
      if (num1 != 0)
        return num1;
      int num2 = this.Event.CompareTo((object) compareObject.Event);
      return num2 != 0 ? num2 : this.EventParameter.CompareTo(compareObject.EventParameter);
    }

    public override string ToString()
    {
      return this.No.ToString("d02") + " " + this.LoggerTime.ToString("dd.MM.yyyy HH:mm:ss") + " " + this.Event.ToString().PadRight(30, '.') + " " + this.EventParameter;
    }
  }
}
