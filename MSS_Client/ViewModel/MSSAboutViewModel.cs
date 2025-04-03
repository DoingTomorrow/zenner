// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.MSSAboutViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.LicenseManagement;
using MVVM.ViewModel;
using System.Configuration;
using ZENNER;

#nullable disable
namespace MSS_Client.ViewModel
{
  public class MSSAboutViewModel : ViewModelBase
  {
    public string ApplicationName => ConfigurationManager.AppSettings[nameof (ApplicationName)];

    public string VersionNumber => ConfigurationManager.AppSettings["ApplicationVersion"];

    public string CurrentLicense
    {
      get => LicenseHelper.GetCurrentLicenseType(LicenseHelper.GetValidHardwareKey());
    }

    public string GMMMetrologicalCore => GmmInterface.GetMetrologicalCore();

    public string AboutText
    {
      get => LicenseHelper.GetCurrentLicenseAboutText(LicenseHelper.GetValidHardwareKey());
    }
  }
}
