// Decompiled with JetBrains decompiler
// Type: HandlerLib.LoRaWANVersion
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public sealed class LoRaWANVersion : ReturnValue
  {
    public byte MainVersion { get; set; }

    public byte MinorVersion { get; set; }

    public byte ReleaseNr { get; set; }

    public override string ToString()
    {
      return ((int) this.MainVersion).ToString() + "." + ((int) this.MinorVersion).ToString() + "." + ((int) this.ReleaseNr).ToString();
    }
  }
}
