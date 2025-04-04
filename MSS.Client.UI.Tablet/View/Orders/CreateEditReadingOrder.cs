// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.CreateEditReadingOrder
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Orders
{
  public class CreateEditReadingOrder : ResizableMetroWindow, IComponentConnector
  {
    internal Grid OrderGrid;
    internal Button BtnStructure;
    internal RadDateTimePicker DueDateTimePicker;
    internal RadNumericUpDown dueDateYearValue;
    internal CheckBox ExportedOrderCheckBox;
    internal Button Filter;
    internal Button AddButton;
    internal Button EditButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public CreateEditReadingOrder()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.DueDateTimePicker.Culture = new CultureInfo("en-US");
      this.DueDateTimePicker.Culture.DateTimeFormat.ShortDatePattern = "dd/MM";
      this.dueDateYearValue.NumberFormatInfo = new NumberFormatInfo()
      {
        NumberGroupSeparator = ""
      };
    }

    ~CreateEditReadingOrder()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/createeditorder.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.OrderGrid = (Grid) target;
          break;
        case 2:
          this.BtnStructure = (Button) target;
          break;
        case 3:
          this.DueDateTimePicker = (RadDateTimePicker) target;
          break;
        case 4:
          this.dueDateYearValue = (RadNumericUpDown) target;
          break;
        case 5:
          this.ExportedOrderCheckBox = (CheckBox) target;
          break;
        case 6:
          this.Filter = (Button) target;
          break;
        case 7:
          this.AddButton = (Button) target;
          break;
        case 8:
          this.EditButton = (Button) target;
          break;
        case 9:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
