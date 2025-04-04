// Decompiled with JetBrains decompiler
// Type: PlugInLib.SignedLicense
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

using System;

#nullable disable
namespace PlugInLib
{
  [Serializable]
  public class SignedLicense
  {
    public LicenseInfo License;
    public byte[] Signature;

    public SignedLicense() => this.License = new LicenseInfo();
  }
}
