// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.ProgressDialog
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public class ProgressDialog : BaseMetroDialog, IComponentConnector
  {
    public static readonly DependencyProperty ProgressBarForegroundProperty = DependencyProperty.Register(nameof (ProgressBarForeground), typeof (Brush), typeof (ProgressDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (ProgressDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty IsCancelableProperty = DependencyProperty.Register(nameof (IsCancelable), typeof (bool), typeof (ProgressDialog), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, e) => ((ProgressDialog) s).PART_NegativeButton.Visibility = (bool) e.NewValue ? Visibility.Visible : Visibility.Hidden)));
    public static readonly DependencyProperty NegativeButtonTextProperty = DependencyProperty.Register(nameof (NegativeButtonText), typeof (string), typeof (ProgressDialog), new PropertyMetadata((object) "Cancel"));
    internal Button PART_NegativeButton;
    internal MetroProgressBar PART_ProgressBar;
    private bool _contentLoaded;

    internal ProgressDialog(MetroWindow parentWindow, MetroDialogSettings settings)
      : base(parentWindow, settings)
    {
      this.InitializeComponent();
      if (parentWindow.MetroDialogOptions.ColorScheme == MetroDialogColorScheme.Theme)
      {
        try
        {
          this.ProgressBarForeground = ThemeManager.GetResourceFromAppStyle((Window) parentWindow, "AccentColorBrush") as Brush;
        }
        catch (Exception ex)
        {
        }
      }
      else
        this.ProgressBarForeground = (Brush) Brushes.White;
    }

    internal ProgressDialog(MetroWindow parentWindow)
      : this(parentWindow, (MetroDialogSettings) null)
    {
    }

    public string Message
    {
      get => (string) this.GetValue(ProgressDialog.MessageProperty);
      set => this.SetValue(ProgressDialog.MessageProperty, (object) value);
    }

    public bool IsCancelable
    {
      get => (bool) this.GetValue(ProgressDialog.IsCancelableProperty);
      set => this.SetValue(ProgressDialog.IsCancelableProperty, (object) value);
    }

    public string NegativeButtonText
    {
      get => (string) this.GetValue(ProgressDialog.NegativeButtonTextProperty);
      set => this.SetValue(ProgressDialog.NegativeButtonTextProperty, (object) value);
    }

    public Brush ProgressBarForeground
    {
      get => (Brush) this.GetValue(ProgressDialog.ProgressBarForegroundProperty);
      set => this.SetValue(ProgressDialog.ProgressBarForegroundProperty, (object) value);
    }

    internal CancellationToken CancellationToken => this.DialogSettings.CancellationToken;

    internal double Minimum
    {
      get => this.PART_ProgressBar.Minimum;
      set => this.PART_ProgressBar.Minimum = value;
    }

    internal double Maximum
    {
      get => this.PART_ProgressBar.Maximum;
      set => this.PART_ProgressBar.Maximum = value;
    }

    internal double ProgressValue
    {
      get => this.PART_ProgressBar.Value;
      set
      {
        this.PART_ProgressBar.IsIndeterminate = false;
        this.PART_ProgressBar.Value = value;
        this.PART_ProgressBar.ApplyTemplate();
      }
    }

    internal void SetIndeterminate() => this.PART_ProgressBar.IsIndeterminate = true;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MahApps.Metro;component/themes/dialogs/progressdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.PART_ProgressBar = (MetroProgressBar) target;
        else
          this._contentLoaded = true;
      }
      else
        this.PART_NegativeButton = (Button) target;
    }
  }
}
