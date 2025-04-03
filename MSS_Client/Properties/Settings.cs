// Decompiled with JetBrains decompiler
// Type: MSS_Client.Properties.Settings
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace MSS_Client.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
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

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string MSSId
    {
      get => (string) this[nameof (MSSId)];
      set => this[nameof (MSSId)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("00000000-0000-0000-0000-000000000000")]
    public Guid RememberedUserId
    {
      get => (Guid) this[nameof (RememberedUserId)];
      set => this[nameof (RememberedUserId)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string a
    {
      get => (string) this[nameof (a)];
      set => this[nameof (a)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    public DateTime LastLoginAttempt
    {
      get => (DateTime) this[nameof (LastLoginAttempt)];
      set => this[nameof (LastLoginAttempt)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public short NoOfLoginAttempts
    {
      get => (short) this[nameof (NoOfLoginAttempts)];
      set => this[nameof (NoOfLoginAttempts)] = (object) value;
    }
  }
}
