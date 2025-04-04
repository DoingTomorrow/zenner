// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Structures.CreateEditMinomatMasterDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.CustomControls;
using MSS.Utils.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TabControl;
using WpfKb.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Structures
{
  public partial class CreateEditMinomatMasterDialog : KeyboardMetroWindow, IComponentConnector
  {
    internal CreateEditMinomatMasterDialog CreateEditMinomatMaster;
    internal RadBusyIndicator BusyIndicator;
    internal RadComboBox FloorNameBox;
    internal RadComboBox DirectionBox;
    internal TouchScreenKeyboardUserControl Keyboard;
    internal TouchScreenKeyboardUserControl Keyboard2;
    private bool _contentLoaded;

    public CreateEditMinomatMasterDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~CreateEditMinomatMasterDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = CreateEditMinomatMasterDialog.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    private void AdornerDecorator_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void AdornerDecorator_Loaded1(object sender, RoutedEventArgs e)
    {
      this.RegisterKeyboardEvents(this.Keyboard);
    }

    private void AdornerDecorator_Loaded_2(object sender, RoutedEventArgs e)
    {
      this.RegisterKeyboardEvents(this.Keyboard2);
    }

    private void RadTabControl_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
    {
      int num1 = (int) this.Keyboard.IfNotNull<TouchScreenKeyboardUserControl, Visibility>((Func<TouchScreenKeyboardUserControl, Visibility>) (_ => _.Visibility = Visibility.Collapsed));
      int num2 = (int) this.Keyboard2.IfNotNull<TouchScreenKeyboardUserControl, Visibility>((Func<TouchScreenKeyboardUserControl, Visibility>) (_ => _.Visibility = Visibility.Collapsed));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/structures/createeditminomatmasterdialog.xaml", UriKind.Relative));
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
          this.CreateEditMinomatMaster = (CreateEditMinomatMasterDialog) target;
          break;
        case 2:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        case 3:
          ((RadTabControlBase) target).SelectionChanged += new RadSelectionChangedEventHandler(this.RadTabControl_SelectionChanged);
          break;
        case 4:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.AdornerDecorator_Loaded1);
          break;
        case 5:
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 6:
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 7:
          this.FloorNameBox = (RadComboBox) target;
          break;
        case 8:
          this.DirectionBox = (RadComboBox) target;
          break;
        case 9:
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 10:
          this.Keyboard = (TouchScreenKeyboardUserControl) target;
          break;
        case 11:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.AdornerDecorator_Loaded_2);
          break;
        case 12:
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 13:
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 14:
          this.Keyboard2 = (TouchScreenKeyboardUserControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
