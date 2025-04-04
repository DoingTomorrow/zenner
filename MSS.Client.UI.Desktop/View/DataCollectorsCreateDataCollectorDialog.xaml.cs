// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.DataCollectors.CreateDataCollectorDialog
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

#nullable disable
namespace MSS.Client.UI.Desktop.View.DataCollectors
{
  public partial class CreateDataCollectorDialog : ResizableMetroWindow, IComponentConnector
  {
    internal TextBox MasterRadioIdTextBox;
    internal TextBox SimPinTextBox;
    internal TextBox SimCardNumberTextBox;
    internal TextBox PollingTextBox;
    internal Button OkButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public CreateDataCollectorDialog()
    {
      this.InitializeComponent();
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~CreateDataCollectorDialog()
    {
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.MasterRadioIdTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
      this.SimPinTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
      this.SimCardNumberTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
      this.PollingTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = CreateDataCollectorDialog.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/datacollectors/createdatacollectordialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.MasterRadioIdTextBox = (TextBox) target;
          this.MasterRadioIdTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 2:
          this.SimPinTextBox = (TextBox) target;
          this.SimPinTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 3:
          this.SimCardNumberTextBox = (TextBox) target;
          this.SimCardNumberTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 4:
          this.PollingTextBox = (TextBox) target;
          this.PollingTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 5:
          this.OkButton = (Button) target;
          break;
        case 6:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
