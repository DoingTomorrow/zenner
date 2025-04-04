// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.RepairModeDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.Common;
using MSS.Utils.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using WpfKb.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Orders
{
  public partial class RepairModeDialog : ResizableMetroWindow, IComponentConnector
  {
    internal FloatingTouchScreenKeyboard Keyboard;
    internal CheckBox PlaceHolder;
    internal Grid GridSuccess;
    internal Grid GridError;
    private bool _contentLoaded;

    public RepairModeDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~RepairModeDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void RadMaskedNumericInput_GotKeyboardFocus(
      object sender,
      KeyboardFocusChangedEventArgs e)
    {
      Application.Current.Dispatcher.InvokeAsync((Action) (() => CommonHandlers<OpenKeyboardEventParams>.OpenKeyboard((Func<OpenKeyboardEventParams>) (() => new OpenKeyboardEventParams()
      {
        FrameworkElement = sender.SafeCast<FrameworkElement>(),
        RoutedEventArgs = (RoutedEventArgs) e,
        UserControlElement = (object) this.Keyboard
      }))));
    }

    private void RadMaskedNumericInput_LostFocus(object sender, RoutedEventArgs e)
    {
      CommonHandlers<OpenKeyboardEventParams>.CloseKeyboard(new OpenKeyboardEventParams()
      {
        FrameworkElement = sender.SafeCast<FrameworkElement>(),
        RoutedEventArgs = e,
        UserControlElement = (object) this.Keyboard
      });
    }

    private void WriteMinolID_LostFocus(object sender, RoutedEventArgs e)
    {
      CommonHandlers<OpenKeyboardEventParams>.CloseKeyboard(new OpenKeyboardEventParams()
      {
        FrameworkElement = sender.SafeCast<FrameworkElement>(),
        RoutedEventArgs = e,
        UserControlElement = (object) this.Keyboard
      });
    }

    private void WriteMinolID_PreviewGotKeyboardFocus(
      object sender,
      KeyboardFocusChangedEventArgs e)
    {
      Application.Current.Dispatcher.InvokeAsync((Action) (() => CommonHandlers<OpenKeyboardEventParams>.OpenKeyboard((Func<OpenKeyboardEventParams>) (() => new OpenKeyboardEventParams()
      {
        FrameworkElement = sender.SafeCast<FrameworkElement>(),
        RoutedEventArgs = (RoutedEventArgs) e,
        UserControlElement = (object) this.Keyboard
      }))));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/repairmodedialog.xaml", UriKind.Relative));
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
          this.Keyboard = (FloatingTouchScreenKeyboard) target;
          break;
        case 2:
          this.PlaceHolder = (CheckBox) target;
          break;
        case 3:
          this.GridSuccess = (Grid) target;
          break;
        case 4:
          this.GridError = (Grid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
