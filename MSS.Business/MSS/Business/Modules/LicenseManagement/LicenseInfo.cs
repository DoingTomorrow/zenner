// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.LicenseManagement.LicenseInfo
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using PlugInLib;

#nullable disable
namespace MSS.Business.Modules.LicenseManagement
{
  public class LicenseInfo
  {
    public SignedLicense SignedLicense { get; set; }

    public string FullLicenseText { get; set; }

    public LicenseInfo()
    {
      this.SignedLicense = (SignedLicense) null;
      this.FullLicenseText = string.Empty;
    }
  }
}
