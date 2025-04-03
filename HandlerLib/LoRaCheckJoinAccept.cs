// Decompiled with JetBrains decompiler
// Type: HandlerLib.LoRaCheckJoinAccept
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  public sealed class LoRaCheckJoinAccept : ReturnValue
  {
    public DateTime Timestamp { get; set; }

    public byte[] NetID { get; set; }

    public uint DeviceAddress { get; set; }

    public byte DLSettings { get; set; }

    public byte RXDelay { get; set; }

    public byte[] CFList { get; set; }
  }
}
