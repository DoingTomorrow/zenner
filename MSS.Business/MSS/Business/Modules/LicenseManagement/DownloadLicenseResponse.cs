// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.LicenseManagement.DownloadLicenseResponse
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

#nullable disable
namespace MSS.Business.Modules.LicenseManagement
{
  public class DownloadLicenseResponse
  {
    public byte[] FullLicenseBytes { get; set; }

    public bool IsSuccessfullyDownloaded { get; set; }

    public DownloadLicenseResponse()
    {
      this.FullLicenseBytes = (byte[]) null;
      this.IsSuccessfullyDownloaded = false;
    }
  }
}
