// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Structures.CreateEditLocationDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.CustomControls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using WpfKb.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Structures
{
  public partial class CreateEditLocationDialog : KeyboardMetroWindow, IComponentConnector
  {
    internal CreateEditLocationDialog CreateLocationUC;
    internal AdornerDecorator AdornerDecorator;
    internal TouchScreenKeyboardUserControl Keyboard;
    internal AdornerDecorator AdornerDecorator2;
    internal RadComboBox ScaleComboBox;
    internal RadComboBox GenerationComboBox;
    internal RadComboBox ScenarioComboBox;
    internal RadDateTimePicker DueDateTimePicker;
    internal TouchScreenKeyboardUserControl Keyboard2;
    private bool _contentLoaded;

    public CreateEditLocationDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.DueDateTimePicker.Culture = new CultureInfo("en-US");
      this.DueDateTimePicker.Culture.DateTimeFormat.ShortDatePattern = "dd/MM";
    }

    ~CreateEditLocationDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = CreateEditLocationDialog.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    private void AdornerDecorator_Loaded(object sender, RoutedEventArgs e)
    {
      this.RegisterKeyboardEvents(this.Keyboard);
    }

    private void AdornerDecorator_Loaded2(object sender, RoutedEventArgs e)
    {
      this.RegisterKeyboardEvents(this.Keyboard2);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/structures/createeditlocationdialog.xaml", UriKind.Relative));
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
          this.CreateLocationUC = (CreateEditLocationDialog) target;
          break;
        case 2:
          this.AdornerDecorator = (AdornerDecorator) target;
          this.AdornerDecorator.Loaded += new RoutedEventHandler(this.AdornerDecorator_Loaded);
          break;
        case 3:
          this.Keyboard = (TouchScreenKeyboardUserControl) target;
          break;
        case 4:
          this.AdornerDecorator2 = (AdornerDecorator) target;
          this.AdornerDecorator2.Loaded += new RoutedEventHandler(this.AdornerDecorator_Loaded2);
          break;
        case 5:
          this.ScaleComboBox = (RadComboBox) target;
          break;
        case 6:
          this.GenerationComboBox = (RadComboBox) target;
          break;
        case 7:
          this.ScenarioComboBox = (RadComboBox) target;
          break;
        case 8:
          this.DueDateTimePicker = (RadDateTimePicker) target;
          break;
        case 9:
          this.Keyboard2 = (TouchScreenKeyboardUserControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
