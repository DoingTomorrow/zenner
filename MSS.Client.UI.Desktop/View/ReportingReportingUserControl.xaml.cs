// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Reporting.ReportingUserControl
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

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
namespace MSS.Client.UI.Desktop.View.Reporting
{
  public partial class ReportingUserControl : UserControl, IComponentConnector
  {
    private int _currentPagingNumber = 0;
    internal Button Consumption;
    internal TextBlock ConsumptionCommand;
    internal Button LoadData;
    internal TextBlock LoadDataCommand;
    internal Button btnExportData;
    internal Button Print;
    internal TextBlock PrintCommand;
    internal Button Expand;
    internal Button Collapse;
    internal TextBox SearchTextBox;
    internal Grid StructureRootGrid;
    internal RadTreeListView treeListView;
    internal RadDateTimePicker StartDate;
    internal RadDateTimePicker EndDate;
    internal Button BtnStructure;
    internal CheckBox ShowOnlyLast;
    internal RadGridView MeterReadingValuesGridView;
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
    private bool _contentLoaded;

    public ReportingUserControl() => this.InitializeComponent();

    ~ReportingUserControl()
    {
      this.RadDataPager.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.Expand.Click -= new RoutedEventHandler(this.Expand_OnClick);
      this.Collapse.Click -= new RoutedEventHandler(this.Collapse_OnClick);
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

    private void Expand_OnClick(object sender, RoutedEventArgs e)
    {
      this.treeListView.ExpandAllHierarchyItems();
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = ReportingUserControl.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    private void Collapse_OnClick(object sender, RoutedEventArgs e)
    {
      this.treeListView.CollapseAllHierarchyItems();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/reporting/reportingusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.Consumption = (Button) target;
          break;
        case 2:
          this.ConsumptionCommand = (TextBlock) target;
          break;
        case 3:
          this.LoadData = (Button) target;
          break;
        case 4:
          this.LoadDataCommand = (TextBlock) target;
          break;
        case 5:
          this.btnExportData = (Button) target;
          break;
        case 6:
          this.Print = (Button) target;
          break;
        case 7:
          this.PrintCommand = (TextBlock) target;
          break;
        case 8:
          this.Expand = (Button) target;
          this.Expand.Click += new RoutedEventHandler(this.Expand_OnClick);
          break;
        case 9:
          this.Collapse = (Button) target;
          this.Collapse.Click += new RoutedEventHandler(this.Collapse_OnClick);
          break;
        case 10:
          this.SearchTextBox = (TextBox) target;
          break;
        case 11:
          this.StructureRootGrid = (Grid) target;
          break;
        case 12:
          this.treeListView = (RadTreeListView) target;
          break;
        case 13:
          this.StartDate = (RadDateTimePicker) target;
          break;
        case 14:
          this.EndDate = (RadDateTimePicker) target;
          break;
        case 15:
          this.BtnStructure = (Button) target;
          break;
        case 16:
          this.ShowOnlyLast = (CheckBox) target;
          break;
        case 17:
          this.MeterReadingValuesGridView = (RadGridView) target;
          break;
        case 18:
          this.CreateJob = (Button) target;
          break;
        case 19:
          this.CreateExportJobCommand = (TextBlock) target;
          break;
        case 20:
          this.EditJob = (Button) target;
          break;
        case 21:
          this.EditUserCommand = (TextBlock) target;
          break;
        case 22:
          this.DeleteJob = (Button) target;
          break;
        case 23:
          this.DeleteExportJobCommand = (TextBlock) target;
          break;
        case 24:
          this.ExportReadingValuesButton = (Button) target;
          break;
        case 25:
          this.ExportAllCommand = (TextBlock) target;
          break;
        case 26:
          this.MDMExportButton = (Button) target;
          break;
        case 27:
          this.MDMExportTextBox = (TextBlock) target;
          break;
        case 28:
          this.JobsGridView = (RadGridView) target;
          break;
        case 29:
          this.RadDataPager = (RadDataPager) target;
          this.RadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
