// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.RadioVersions
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class RadioVersions
  {
    private ushort _lmpSubversion;

    [CLSCompliant(false)]
    public RadioVersions(
      LmpVersion lmpVersion,
      ushort lmpSubversion,
      LmpFeatures lmpSupportedFeatures,
      Manufacturer mfg)
    {
      this.LmpVersion = lmpVersion;
      this.LmpSubversion = (int) lmpSubversion;
      this.LmpSupportedFeatures = lmpSupportedFeatures;
      this.Manufacturer = mfg;
    }

    public LmpVersion LmpVersion { get; private set; }

    public int LmpSubversion
    {
      get => (int) this._lmpSubversion;
      private set => this._lmpSubversion = checked ((ushort) value);
    }

    public LmpFeatures LmpSupportedFeatures { get; private set; }

    public Manufacturer Manufacturer { get; private set; }
  }
}
