// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Structures.CreateEditMinomatDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

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
namespace MSS.Client.UI.Tablet.View.Structures
{
  public partial class CreateEditMinomatDialog : ResizableMetroWindow, IComponentConnector
  {
    internal TextBox RadioIdTextBox;
    internal CheckBox isMaster;
    internal TextBox SimPinTextBox;
    internal TextBox PollingTextBox;
    internal Button AddButton;
    internal Button OkButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public CreateEditMinomatDialog()
    {
      this.InitializeComponent();
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~CreateEditMinomatDialog()
    {
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.RadioIdTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
      this.SimPinTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
      this.PollingTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = CreateEditMinomatDialog.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/structures/createeditminomatdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.RadioIdTextBox = (TextBox) target;
          this.RadioIdTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 2:
          this.isMaster = (CheckBox) target;
          break;
        case 3:
          this.SimPinTextBox = (TextBox) target;
          this.SimPinTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 4:
          this.PollingTextBox = (TextBox) target;
          this.PollingTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 5:
          this.AddButton = (Button) target;
          break;
        case 6:
          this.OkButton = (Button) target;
          break;
        case 7:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
