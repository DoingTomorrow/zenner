// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Msft.BTH_RADIO_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Globalization;

#nullable disable
namespace InTheHand.Net.Bluetooth.Msft
{
  internal struct BTH_RADIO_INFO
  {
    internal readonly LmpFeatures _lmpSupportedFeatures;
    internal readonly Manufacturer _mfg;
    internal readonly ushort _lmpSubversion;
    internal readonly LmpVersion _lmpVersion;

    internal BTH_RADIO_INFO(
      LmpVersion lmpVersion,
      ushort lmpSubversion,
      Manufacturer mfg,
      ulong lmpSupportedFeatures)
    {
      this._lmpSupportedFeatures = (LmpFeatures) lmpSupportedFeatures;
      this._mfg = mfg;
      this._lmpSubversion = lmpSubversion;
      this._lmpVersion = lmpVersion;
    }

    internal BTH_RADIO_INFO(Version fakeSetAllUnknown)
    {
      this._lmpSupportedFeatures = LmpFeatures.None;
      this._mfg = Manufacturer.Unknown;
      this._lmpSubversion = (ushort) 0;
      this._lmpVersion = LmpVersion.Unknown;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "BTH_RADIO_INFO {0}, {1}, {2}, {3:X16} '{4}'", (object) this._lmpVersion, (object) this._lmpSubversion, (object) this._mfg, (object) (ulong) this._lmpSupportedFeatures, (object) this._lmpSupportedFeatures);
    }

    public RadioVersions ConvertToRadioVersions()
    {
      return new RadioVersions(this._lmpVersion, this._lmpSubversion, this._lmpSupportedFeatures, this._mfg);
    }
  }
}
