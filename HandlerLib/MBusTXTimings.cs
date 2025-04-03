// Decompiled with JetBrains decompiler
// Type: HandlerLib.MBusTXTimings
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public sealed class MBusTXTimings : ReturnValue
  {
    public ushort Interval { get; set; }

    public byte NightTimeStart { get; set; }

    public byte NightTimeEnd { get; set; }

    public byte RadioSuppressionDays { get; set; }

    public uint Reserved { get; set; }
  }
}
