// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.WindowApplicationSettings
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Native;
using System;
using System.Configuration;
using System.Windows;

#nullable disable
namespace MahApps.Metro.Controls
{
  internal class WindowApplicationSettings(Window window) : 
    ApplicationSettingsBase(window.GetType().FullName),
    IWindowPlacementSettings
  {
    [UserScopedSetting]
    public WINDOWPLACEMENT? Placement
    {
      get
      {
        return this[nameof (Placement)] != null ? new WINDOWPLACEMENT?((WINDOWPLACEMENT) this[nameof (Placement)]) : new WINDOWPLACEMENT?();
      }
      set => this[nameof (Placement)] = (object) value;
    }

    [UserScopedSetting]
    public bool UpgradeSettings
    {
      get
      {
        try
        {
          if (this[nameof (UpgradeSettings)] != null)
            return (bool) this[nameof (UpgradeSettings)];
        }
        catch (ConfigurationErrorsException ex)
        {
          ConfigurationErrorsException innerException = ex;
          string str = (string) null;
          while (innerException != null && (str = innerException.Filename) == null)
            innerException = innerException.InnerException as ConfigurationErrorsException;
          throw new MahAppsException(string.Format("The settings file '{0}' seems to be corrupted", (object) (str ?? "<unknown>")), (Exception) innerException);
        }
        return true;
      }
      set => this[nameof (UpgradeSettings)] = (object) value;
    }

    void IWindowPlacementSettings.Reload() => this.Reload();
  }
}
