// Decompiled with JetBrains decompiler
// Type: MSSWeb.Common.Helpers.MLSHelper
// Assembly: MSSWeb.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3AC43CAD-1714-4F19-BF9F-59E1481A8FBA
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSWeb.Common.dll

using PlugInLib;
using System;

#nullable disable
namespace MSSWeb.Common.Helpers
{
  public class MLSHelper
  {
    public static string GetLicenseFileName(string hardwareKey)
    {
      return MLSHelper.GetLicenseFileNameNoExtension(hardwareKey) + ".zlf";
    }

    public static string GetLicenseFileNameNoExtension(string hardwareKey)
    {
      return "MSSLicense_" + hardwareKey;
    }

    public static SignedLicense FillLicenseFromTemplate(
      SignedLicense signedLicenseTemplate,
      LicenseDetails licenseDetails,
      string privateKey)
    {
      SignedLicense signedLicense = new SignedLicense()
      {
        License = {
          Id = Guid.NewGuid(),
          Name = licenseDetails.LicenseName,
          HardwareKey = licenseDetails.HardwareKey,
          Customer = licenseDetails.CustomerName,
          Plugins = signedLicenseTemplate.License.Plugins,
          Rights = signedLicenseTemplate.License.Rights,
          DisplayEvaluationFactor = signedLicenseTemplate.License.DisplayEvaluationFactor,
          IsClientLicense = signedLicenseTemplate.License.IsClientLicense,
          IsMinoConnectedNeeded = signedLicenseTemplate.License.IsMinoConnectedNeeded,
          IsServerLicense = signedLicenseTemplate.License.IsServerLicense,
          LicenseGenerationDate = DateTime.Now,
          LogoImage = signedLicenseTemplate.License.LogoImage,
          ServerConnectionInterval = signedLicenseTemplate.License.ServerConnectionInterval,
          ServerConnectionTolerance = signedLicenseTemplate.License.ServerConnectionTolerance,
          ValidityStartDate = new DateTime?(licenseDetails.ValidityStartDate),
          ValidityPeriod = signedLicenseTemplate.License.ValidityPeriod,
          DeviceTypes = signedLicenseTemplate.License.DeviceTypes,
          IsStandaloneClient = signedLicenseTemplate.License.IsStandaloneClient,
          AboutText = signedLicenseTemplate.License.AboutText,
          CurrentLicenseType = signedLicenseTemplate.License.CurrentLicenseType
        }
      };
      signedLicense.Signature = LicenseManager.ApplySignature(signedLicense.License, privateKey);
      return signedLicense;
    }
  }
}
