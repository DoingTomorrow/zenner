// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Meters.TranslationRulesDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Meters
{
  public partial class TranslationRulesDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadRadioButton MainRadioButton;
    internal RadRadioButton SubDeviceRadioButton;
    internal TextBox MultiplierTextBox;
    internal Button btnNew;
    internal Button btnDelete;
    internal Button btnClear;
    internal Button btnEdit;
    internal Button btnUpdate;
    internal Button btnCancel;
    internal Button btnExport;
    internal Button btnImport;
    internal Button btnClose;
    private bool _contentLoaded;

    public TranslationRulesDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~TranslationRulesDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.MultiplierTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = TranslationRulesDialog.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/meters/translationrulesdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.MainRadioButton = (RadRadioButton) target;
          break;
        case 2:
          this.SubDeviceRadioButton = (RadRadioButton) target;
          break;
        case 3:
          this.MultiplierTextBox = (TextBox) target;
          this.MultiplierTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 4:
          this.btnNew = (Button) target;
          break;
        case 5:
          this.btnDelete = (Button) target;
          break;
        case 6:
          this.btnClear = (Button) target;
          break;
        case 7:
          this.btnEdit = (Button) target;
          break;
        case 8:
          this.btnUpdate = (Button) target;
          break;
        case 9:
          this.btnCancel = (Button) target;
          break;
        case 10:
          this.btnExport = (Button) target;
          break;
        case 11:
          this.btnImport = (Button) target;
          break;
        case 12:
          this.btnClose = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
