// Decompiled with JetBrains decompiler
// Type: HandlerLib.MBusEventLog
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public sealed class MBusEventLog : ReturnValue
  {
    public byte FlowControl { get; set; }

    public byte EntryFormat { get; set; }

    public byte SystemEventType { get; set; }

    public byte[] EventTime { get; set; }

    public byte[] Channel0Value { get; set; }

    public byte[] Channel1Value { get; set; }
  }
}
