// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Jobs.AddEditIntervals
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Jobs
{
  public partial class AddEditIntervals : ResizableMetroWindow, IComponentConnector
  {
    internal RadDatePicker StarTimePicker;
    internal RadDatePicker EndTimePicker;
    internal RadRadioButton OneTime;
    internal RadRadioButton FixedInterval;
    internal RadRadioButton Daily;
    internal RadRadioButton Weekly;
    internal RadRadioButton Monthly;
    internal RadDatePicker OneTimeDatePicker;
    internal RadMaskedTextInput RadMaskedTextInput;
    internal RadRadioButton DayOfMonth;
    internal RadRadioButton WeekDay;
    internal Button OkButton;
    internal Button CanceButton;
    private bool _contentLoaded;

    public AddEditIntervals()
    {
      this.InitializeComponent();
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~AddEditIntervals()
    {
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/jobs/addeditintervals.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.StarTimePicker = (RadDatePicker) target;
          break;
        case 2:
          this.EndTimePicker = (RadDatePicker) target;
          break;
        case 3:
          this.OneTime = (RadRadioButton) target;
          break;
        case 4:
          this.FixedInterval = (RadRadioButton) target;
          break;
        case 5:
          this.Daily = (RadRadioButton) target;
          break;
        case 6:
          this.Weekly = (RadRadioButton) target;
          break;
        case 7:
          this.Monthly = (RadRadioButton) target;
          break;
        case 8:
          this.OneTimeDatePicker = (RadDatePicker) target;
          break;
        case 9:
          this.RadMaskedTextInput = (RadMaskedTextInput) target;
          break;
        case 10:
          this.DayOfMonth = (RadRadioButton) target;
          break;
        case 11:
          this.WeekDay = (RadRadioButton) target;
          break;
        case 12:
          this.OkButton = (Button) target;
          break;
        case 13:
          this.CanceButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
