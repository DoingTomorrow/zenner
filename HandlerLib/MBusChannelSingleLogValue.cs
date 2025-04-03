// Decompiled with JetBrains decompiler
// Type: HandlerLib.MBusChannelSingleLogValue
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  public sealed class MBusChannelSingleLogValue : ReturnValue
  {
    public byte Channel { get; set; }

    public byte[] Date { get; set; }

    public DateTime DateAndTime { get; set; }

    public uint Value { get; set; }
  }
}
