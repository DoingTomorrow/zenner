// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.LoginDialogSettings
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public class LoginDialogSettings : MetroDialogSettings
  {
    private const string DefaultUsernameWatermark = "Username...";
    private const string DefaultPasswordWatermark = "Password...";
    private const Visibility DefaultNegativeButtonVisibility = Visibility.Collapsed;
    private const bool DefaultShouldHideUsername = false;
    private const bool DefaultEnablePasswordPreview = false;

    public LoginDialogSettings()
    {
      this.UsernameWatermark = "Username...";
      this.PasswordWatermark = "Password...";
      this.NegativeButtonVisibility = Visibility.Collapsed;
      this.ShouldHideUsername = false;
      this.AffirmativeButtonText = "Login";
      this.EnablePasswordPreview = false;
    }

    public string InitialUsername { get; set; }

    public string InitialPassword { get; set; }

    public string UsernameWatermark { get; set; }

    public bool ShouldHideUsername { get; set; }

    public string PasswordWatermark { get; set; }

    public Visibility NegativeButtonVisibility { get; set; }

    public bool EnablePasswordPreview { get; set; }
  }
}
