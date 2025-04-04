// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.MetroDialogSettings
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Threading;
using System.Windows;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public class MetroDialogSettings
  {
    public MetroDialogSettings()
    {
      this.AffirmativeButtonText = "OK";
      this.NegativeButtonText = "Cancel";
      this.ColorScheme = MetroDialogColorScheme.Theme;
      this.AnimateShow = this.AnimateHide = true;
      this.MaximumBodyHeight = double.NaN;
      this.DefaultText = "";
      this.DefaultButtonFocus = MessageDialogResult.Negative;
      this.CancellationToken = CancellationToken.None;
    }

    public string AffirmativeButtonText { get; set; }

    public string NegativeButtonText { get; set; }

    public string FirstAuxiliaryButtonText { get; set; }

    public string SecondAuxiliaryButtonText { get; set; }

    public MetroDialogColorScheme ColorScheme { get; set; }

    public bool AnimateShow { get; set; }

    public bool AnimateHide { get; set; }

    public string DefaultText { get; set; }

    public double MaximumBodyHeight { get; set; }

    public MessageDialogResult DefaultButtonFocus { get; set; }

    public CancellationToken CancellationToken { get; set; }

    public ResourceDictionary CustomResourceDictionary { get; set; }

    public bool SuppressDefaultResources { get; set; }
  }
}
