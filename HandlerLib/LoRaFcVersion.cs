// Decompiled with JetBrains decompiler
// Type: HandlerLib.LoRaFcVersion
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  public sealed class LoRaFcVersion : ReturnValue
  {
    public ushort Version { get; set; }

    public override string ToString()
    {
      byte[] bytes = BitConverter.GetBytes(this.Version);
      return bytes[1].ToString() + "." + bytes[0].ToString();
    }
  }
}
