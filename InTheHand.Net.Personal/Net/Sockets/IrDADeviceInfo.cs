// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.IrDADeviceInfo
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public class IrDADeviceInfo
  {
    private IrDAAddress address;
    private string name;
    private IrDAHints hints;
    private IrDACharacterSet charset;

    internal IrDADeviceInfo(
      IrDAAddress id,
      string name,
      IrDAHints hints,
      IrDACharacterSet charset)
    {
      this.address = id;
      this.name = name;
      this.hints = hints;
      this.charset = charset;
    }

    public IrDAAddress DeviceAddress => this.address;

    [Obsolete("Use the DeviceAddress property to access the device Address.", false)]
    public byte[] DeviceID => this.address.ToByteArray();

    public string DeviceName => this.name;

    public IrDACharacterSet CharacterSet => this.charset;

    public IrDAHints Hints => this.hints;

    public override bool Equals(object obj)
    {
      return obj is IrDADeviceInfo irDaDeviceInfo ? this.DeviceAddress.Equals((object) irDaDeviceInfo.DeviceAddress) : base.Equals(obj);
    }

    public override int GetHashCode() => this.address.GetHashCode();
  }
}
