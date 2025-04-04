// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Structures.CreateEditTenantDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.Common;
using MSS.Client.UI.Tablet.CustomControls;
using MSS.Utils.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using WpfKb.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Structures
{
  public partial class CreateEditTenantDialog : KeyboardMetroWindow, IComponentConnector
  {
    internal CreateEditTenantDialog CreateTenantUC;
    internal RadTabControl RadTabControl;
    internal AdornerDecorator AdornerDecorator;
    internal TouchScreenKeyboardUserControl Keyboard;
    internal AdornerDecorator AdornerDecorator2;
    internal TextBox TenantNrTextBox;
    internal TextBox TenantNameTextBox;
    internal RadComboBox FloorNameBox;
    internal RadComboBox DirectionBox;
    internal TouchScreenKeyboardUserControl Keyboard2;
    private bool _contentLoaded;

    public CreateEditTenantDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.ContentRendered += new EventHandler(this.MetroWindow_ContentRendered);
    }

    ~CreateEditTenantDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.TenantNrTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
      this.RadTabControl.SelectionChanged -= new RadSelectionChangedEventHandler(this.RadTabControl_SelectionChanged);
      this.ContentRendered -= new EventHandler(this.MetroWindow_ContentRendered);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = CreateEditTenantDialog.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject obj) where T : DependencyObject
    {
      if (obj != null)
      {
        if (obj is T)
          yield return obj as T;
        foreach (DependencyObject child in Enumerable.OfType<DependencyObject>(LogicalTreeHelper.GetChildren(obj)))
        {
          foreach (T logicalChild in CreateEditTenantDialog.FindLogicalChildren<T>(child))
          {
            T c = logicalChild;
            yield return c;
            c = default (T);
          }
        }
      }
    }

    private void AdornerDecorator_Loaded(object sender, RoutedEventArgs e)
    {
      this.RegisterKeyboardEvents(this.Keyboard);
    }

    private void AdornerDecorator_Loaded2(object sender, RoutedEventArgs e)
    {
      this.RegisterKeyboardEvents(this.Keyboard2);
    }

    private void RadTabControl_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
    {
      int num1 = (int) this.Keyboard.IfNotNull<TouchScreenKeyboardUserControl, Visibility>((Func<TouchScreenKeyboardUserControl, Visibility>) (_ => _.Visibility = Visibility.Collapsed));
      int num2 = (int) this.Keyboard2.IfNotNull<TouchScreenKeyboardUserControl, Visibility>((Func<TouchScreenKeyboardUserControl, Visibility>) (_ => _.Visibility = Visibility.Collapsed));
    }

    private void MetroWindow_ContentRendered(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(this.TenantNameTextBox.Text))
        return;
      CommonHandlers<OpenKeyboardEventParams>.SetFirstFocusableItem((Control) this.TenantNameTextBox);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/structures/createedittenantdialog.xaml", UriKind.Relative));
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
          this.CreateTenantUC = (CreateEditTenantDialog) target;
          break;
        case 2:
          this.RadTabControl = (RadTabControl) target;
          this.RadTabControl.SelectionChanged += new RadSelectionChangedEventHandler(this.RadTabControl_SelectionChanged);
          break;
        case 3:
          this.AdornerDecorator = (AdornerDecorator) target;
          this.AdornerDecorator.Loaded += new RoutedEventHandler(this.AdornerDecorator_Loaded);
          break;
        case 4:
          this.Keyboard = (TouchScreenKeyboardUserControl) target;
          break;
        case 5:
          this.AdornerDecorator2 = (AdornerDecorator) target;
          this.AdornerDecorator2.Loaded += new RoutedEventHandler(this.AdornerDecorator_Loaded2);
          break;
        case 6:
          this.TenantNrTextBox = (TextBox) target;
          this.TenantNrTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 7:
          this.TenantNameTextBox = (TextBox) target;
          break;
        case 8:
          this.FloorNameBox = (RadComboBox) target;
          break;
        case 9:
          this.DirectionBox = (RadComboBox) target;
          break;
        case 10:
          this.Keyboard2 = (TouchScreenKeyboardUserControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
