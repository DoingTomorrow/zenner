// Decompiled with JetBrains decompiler
// Type: HandlerLib.MBusChannelIdentification
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public sealed class MBusChannelIdentification : ReturnValue
  {
    public byte Channel { get; set; }

    public long SerialNumber { get; set; }

    public string Manufacturer { get; set; }

    public byte Generation { get; set; }

    public string Medium { get; set; }
  }
}
