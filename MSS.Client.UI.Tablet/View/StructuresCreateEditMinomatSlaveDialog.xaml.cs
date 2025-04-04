// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Structures.CreateEditMinomatSlaveDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.CustomControls;
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
using WpfKb.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Structures
{
  public partial class CreateEditMinomatSlaveDialog : KeyboardMetroWindow, IComponentConnector
  {
    internal CreateEditMinomatSlaveDialog UserControl;
    internal RadBusyIndicator BusyIndicator;
    internal TextBox MastersComboBox;
    internal RadComboBox FloorNameBox;
    internal RadComboBox DirectionBox;
    internal TouchScreenKeyboardUserControl Keyboard;
    private bool _contentLoaded;

    public CreateEditMinomatSlaveDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~CreateEditMinomatSlaveDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = CreateEditMinomatSlaveDialog.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    private void Grid_Loaded(object sender, RoutedEventArgs e)
    {
      this.RegisterKeyboardEvents(this.Keyboard);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/structures/createeditminomatslavedialog.xaml", UriKind.Relative));
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
      switch (connectionId)
      {
        case 1:
          this.UserControl = (CreateEditMinomatSlaveDialog) target;
          break;
        case 2:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        case 3:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Grid_Loaded);
          break;
        case 4:
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 5:
          this.MastersComboBox = (TextBox) target;
          break;
        case 6:
          this.FloorNameBox = (RadComboBox) target;
          break;
        case 7:
          this.DirectionBox = (RadComboBox) target;
          break;
        case 8:
          this.Keyboard = (TouchScreenKeyboardUserControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
