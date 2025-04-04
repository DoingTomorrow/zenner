// Decompiled with JetBrains decompiler
// Type: StartupLib.Properties.Settings
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace StartupLib.Properties
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
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\ersch\\Desktop\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString
    {
      get => (string) this[nameof (MeterDB_NewConnectionString)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\ersch\\Documents\\SVN\\GMM\\GmmTrunkNewStructure\\Source\\Database\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString1
    {
      get => (string) this[nameof (MeterDB_NewConnectionString1)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\erschw\\Documents\\SVN\\GMM\\trunk\\Source\\Database\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString2
    {
      get => (string) this[nameof (MeterDB_NewConnectionString2)];
    }
  }
}
