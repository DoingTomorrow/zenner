// Decompiled with JetBrains decompiler
// Type: MSSWeb.Common.Licensing.LicenseGenerator
// Assembly: MSSWeb.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3AC43CAD-1714-4F19-BF9F-59E1481A8FBA
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSWeb.Common.dll

using MLS.Core.Model.Licensing;
using MSSWeb.Common.Helpers;
using PlugInLib;
using System;
using System.Configuration;
using System.IO;

#nullable disable
namespace MSSWeb.Common.Licensing
{
  public class LicenseGenerator
  {
    public byte[] GetLicenseAsByteArray(SignedLicense signedLicense)
    {
      return signedLicense.FromSignedLicenseToMemoryStream().ToArray();
    }

    public SignedLicense GenerateLicenseFromTemplate(
      LicenseType licenseType,
      LicenseDetails licenseDetails,
      string privateKey)
    {
      return MLSHelper.FillLicenseFromTemplate(LicenseManager.LicensFromXML(File.ReadAllText(ConfigurationManager.AppSettings["LicenseTemplateFolder"] + licenseType.Type + ".tlf")), licenseDetails, privateKey);
    }

    public DateTime? GetStartDate(SignedLicense signedLicense)
    {
      return signedLicense.License.ValidityStartDate;
    }

    public DateTime? GetEndDate(SignedLicense signedLicense)
    {
      int result;
      return !string.IsNullOrEmpty(signedLicense.License.ValidityPeriod) && int.TryParse(signedLicense.License.ValidityPeriod, out result) ? new DateTime?(signedLicense.License.ValidityStartDate.Value.AddMonths(result)) : new DateTime?();
    }
  }
}
