// Decompiled with JetBrains decompiler
// Type: AsyncCom.Properties.Settings
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace AsyncCom.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        return defaultInstance;
      }
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.WebServiceUrl)]
    [DefaultSettingValue("http://1.0.0.1/server.php")]
    public string AsyncCom_MeterVPN_MeterVPNService
    {
      get => (string) this[nameof (AsyncCom_MeterVPN_MeterVPNService)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.WebServiceUrl)]
    [DefaultSettingValue("http://1.0.0.1/server.php")]
    public string AsyncCom_MeterVPNServer_MeterVPNService
    {
      get => (string) this[nameof (AsyncCom_MeterVPNServer_MeterVPNService)];
    }

    private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
    {
    }

    private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
    {
    }
  }
}
