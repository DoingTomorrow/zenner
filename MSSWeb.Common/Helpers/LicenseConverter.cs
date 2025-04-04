// Decompiled with JetBrains decompiler
// Type: MSSWeb.Common.Helpers.LicenseConverter
// Assembly: MSSWeb.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3AC43CAD-1714-4F19-BF9F-59E1481A8FBA
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSWeb.Common.dll

using PlugInLib;
using System.IO;
using System.Text;

#nullable disable
namespace MSSWeb.Common.Helpers
{
  public static class LicenseConverter
  {
    public static MemoryStream FromByteArrayToMemoryStream(this byte[] licenseFile)
    {
      MemoryStream memoryStream = new MemoryStream();
      memoryStream.Write(licenseFile, 0, licenseFile.Length);
      return memoryStream;
    }

    public static byte[] FromSignedLicenseToByteArray(this SignedLicense signedLicense)
    {
      return Encoding.Default.GetBytes(LicenseManager.LicensToXML((object) signedLicense, typeof (SignedLicense)));
    }

    public static MemoryStream FromSignedLicenseToMemoryStream(this SignedLicense signedLicense)
    {
      return signedLicense.FromSignedLicenseToByteArray().FromByteArrayToMemoryStream();
    }
  }
}
