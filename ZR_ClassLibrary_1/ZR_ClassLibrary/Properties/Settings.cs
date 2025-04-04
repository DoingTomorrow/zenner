// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Properties.Settings
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace ZR_ClassLibrary.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.5.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
    {
    }

    private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
    {
    }

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
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"C:\\Dokumente und Einstellungen\\Beck\\Eigene Dateien\\Visual Studio 2005\\Projects\\GMM\\Database\\SecMeterDB.mdb\";Jet OLEDB:Database Password=meterdbpass")]
    public string SecMeterDBConnectionString => (string) this[nameof (SecMeterDBConnectionString)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"|DataDirectory|\\Schema Access\\MeterDB_New.mdb\";Persist Security Info=True;Jet OLEDB:Database Password=meterdbpass")]
    public string MeterDB_NewConnectionString
    {
      get => (string) this[nameof (MeterDB_NewConnectionString)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("data source=\"C:\\Dokumente und Einstellungen\\beck\\Desktop\\MobileMeterDB.sql\"")]
    public string ConnectionString => (string) this[nameof (ConnectionString)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("data source=C:\\Projekte\\GMM\\Database\\MobileMeterDB.sql")]
    public string MobileMeterDB => (string) this[nameof (MobileMeterDB)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=T:\\Produkte\\Softwareprodukte\\GMM\\aktuell\\Datenbank\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString1
    {
      get => (string) this[nameof (MeterDB_NewConnectionString1)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=T:\\Produkte\\Softwareprodukte\\GMM\\aktuell\\Datenbank\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString2
    {
      get => (string) this[nameof (MeterDB_NewConnectionString2)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=L:\\Produkte\\Softwareprodukte\\GMM\\aktuell\\Datenbank\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString3
    {
      get => (string) this[nameof (MeterDB_NewConnectionString3)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Ersch\\Documents\\SVN\\GMM_Interface\\Database\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString4
    {
      get => (string) this[nameof (MeterDB_NewConnectionString4)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\ersch\\Documents\\svn\\GMM_trunk\\Database\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString5
    {
      get => (string) this[nameof (MeterDB_NewConnectionString5)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\ersch\\Documents\\SVN\\GMM\\GmmTrunkNewStructure\\Source\\Database\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString6
    {
      get => (string) this[nameof (MeterDB_NewConnectionString6)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\erschw\\Documents\\SVN\\GMM\\trunk\\Source\\Database\\MeterDB_New.mdb")]
    public string MeterDB_NewConnectionString7
    {
      get => (string) this[nameof (MeterDB_NewConnectionString7)];
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Data Source=meterdb_dev.msh.org;Initial Catalog=meterdb_20161130;User ID=meter-db-cl-sb;Password=gmmmr")]
    public string meterdb_20161130ConnectionString
    {
      get => (string) this[nameof (meterdb_20161130ConnectionString)];
    }
  }
}
