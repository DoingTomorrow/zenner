// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Reporting.ReportingUserControl
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

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
namespace MSS.Client.UI.Tablet.View.Reporting
{
  public partial class ReportingUserControl : UserControl, IComponentConnector
  {
    private int _currentPagingNumber = 0;
    internal Button CreateJob;
    internal TextBlock CreateExportJobCommand;
    internal Button EditJob;
    internal TextBlock EditUserCommand;
    internal Button DeleteJob;
    internal TextBlock DeleteExportJobCommand;
    internal Button ExportReadingValuesButton;
    internal TextBlock ExportAllCommand;
    internal Button MDMExportButton;
    internal TextBlock MDMExportTextBox;
    internal RadGridView JobsGridView;
    internal RadDataPager RadDataPager;
    internal Button LoadDataLogs;
    internal TextBlock LoadDataLogsCommand;
    internal TextBox MasterNumberBox;
    internal RadDatePicker StartDateBox;
    internal RadDatePicker EndDateBox;
    internal RadGridView MinomatCommunicationLogsGridView;
    internal RadDataPager RadDataPager2;
    internal Button LoadJobLogs;
    internal TextBlock LoadJobLogsCommand;
    internal TextBox JobEnityNumberBox;
    internal RadDatePicker JobStartDateBox;
    internal RadDatePicker JobEndDateBox;
    internal RadGridView JobLogsGridView;
    internal RadDataPager RadDataPager3;
    private bool _contentLoaded;

    public ReportingUserControl() => this.InitializeComponent();

    ~ReportingUserControl()
    {
      this.RadDataPager.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.RadDataPager2.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.RadDataPager3.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.MasterNumberBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
      this.JobEnityNumberBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
    }

    public void OnPageIndexChange(object sender, PageIndexChangedEventArgs e)
    {
      if (e.NewPageIndex == this._currentPagingNumber)
        return;
      if (e.OldPageIndex >= 0)
        this._currentPagingNumber = e.NewPageIndex;
      else
        ((RadDataPager) sender).MoveToPage(this._currentPagingNumber);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = ReportingUserControl.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/reporting/reportingusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.CreateJob = (Button) target;
          break;
        case 2:
          this.CreateExportJobCommand = (TextBlock) target;
          break;
        case 3:
          this.EditJob = (Button) target;
          break;
        case 4:
          this.EditUserCommand = (TextBlock) target;
          break;
        case 5:
          this.DeleteJob = (Button) target;
          break;
        case 6:
          this.DeleteExportJobCommand = (TextBlock) target;
          break;
        case 7:
          this.ExportReadingValuesButton = (Button) target;
          break;
        case 8:
          this.ExportAllCommand = (TextBlock) target;
          break;
        case 9:
          this.MDMExportButton = (Button) target;
          break;
        case 10:
          this.MDMExportTextBox = (TextBlock) target;
          break;
        case 11:
          this.JobsGridView = (RadGridView) target;
          break;
        case 12:
          this.RadDataPager = (RadDataPager) target;
          this.RadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 13:
          this.LoadDataLogs = (Button) target;
          break;
        case 14:
          this.LoadDataLogsCommand = (TextBlock) target;
          break;
        case 15:
          this.MasterNumberBox = (TextBox) target;
          this.MasterNumberBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 16:
          this.StartDateBox = (RadDatePicker) target;
          break;
        case 17:
          this.EndDateBox = (RadDatePicker) target;
          break;
        case 18:
          this.MinomatCommunicationLogsGridView = (RadGridView) target;
          break;
        case 19:
          this.RadDataPager2 = (RadDataPager) target;
          this.RadDataPager2.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 20:
          this.LoadJobLogs = (Button) target;
          break;
        case 21:
          this.LoadJobLogsCommand = (TextBlock) target;
          break;
        case 22:
          this.JobEnityNumberBox = (TextBox) target;
          this.JobEnityNumberBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 23:
          this.JobStartDateBox = (RadDatePicker) target;
          break;
        case 24:
          this.JobEndDateBox = (RadDatePicker) target;
          break;
        case 25:
          this.JobLogsGridView = (RadGridView) target;
          break;
        case 26:
          this.RadDataPager3 = (RadDataPager) target;
          this.RadDataPager3.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
