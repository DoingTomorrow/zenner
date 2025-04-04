// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Jobs.JobsUserControl
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Jobs
{
  public partial class JobsUserControl : UserControl, IComponentConnector
  {
    private int _currentPagingNumber = 0;
    internal Button AddJob;
    internal TextBlock AddJobCommand;
    internal Button EditJob;
    internal TextBlock EditJobCommand;
    internal Button RemoveJob;
    internal TextBlock RemoveJobCommand;
    internal Button StartJob;
    internal TextBlock StartJobCommand;
    internal Button StopJob;
    internal Button ViewJobStructure;
    internal Button JobLogs;
    internal TextBlock JobLogsCommand;
    internal RadGridView JobGridView;
    internal RadDataPager RadDataPager;
    internal Button LoadJobLogs;
    internal TextBlock LoadJobLogsCommand;
    internal TextBox JobEnityNumberBox;
    internal RadDatePicker JobStartDateBox;
    internal RadDatePicker JobEndDateBox;
    internal RadGridView JobLogsGridView;
    internal RadDataPager RadDataPager2;
    internal Button AddButton;
    internal TextBlock AddButtonCommand;
    internal Button EditButton;
    internal TextBlock EditButtonCommand;
    internal Button RemoveButton;
    internal TextBlock RemoveButtonCommand;
    internal RadGridView JobDefinitionGridView;
    internal RadDataPager RadDataPager3;
    internal Button EditScenario;
    internal TextBlock EditScenarioCommand;
    internal RadGridView ScenarioGridView;
    internal RadDataPager RadDataPager4;
    private bool _contentLoaded;

    public JobsUserControl()
    {
      this.InitializeComponent();
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
    }

    ~JobsUserControl()
    {
      this.RadDataPager.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.RadDataPager2.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.RadDataPager3.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.RadDataPager4.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
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

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/jobs/jobsusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.AddJob = (Button) target;
          break;
        case 2:
          this.AddJobCommand = (TextBlock) target;
          break;
        case 3:
          this.EditJob = (Button) target;
          break;
        case 4:
          this.EditJobCommand = (TextBlock) target;
          break;
        case 5:
          this.RemoveJob = (Button) target;
          break;
        case 6:
          this.RemoveJobCommand = (TextBlock) target;
          break;
        case 7:
          this.StartJob = (Button) target;
          break;
        case 8:
          this.StartJobCommand = (TextBlock) target;
          break;
        case 9:
          this.StopJob = (Button) target;
          break;
        case 10:
          this.ViewJobStructure = (Button) target;
          break;
        case 11:
          this.JobLogs = (Button) target;
          break;
        case 12:
          this.JobLogsCommand = (TextBlock) target;
          break;
        case 13:
          this.JobGridView = (RadGridView) target;
          break;
        case 14:
          this.RadDataPager = (RadDataPager) target;
          this.RadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 15:
          this.LoadJobLogs = (Button) target;
          break;
        case 16:
          this.LoadJobLogsCommand = (TextBlock) target;
          break;
        case 17:
          this.JobEnityNumberBox = (TextBox) target;
          break;
        case 18:
          this.JobStartDateBox = (RadDatePicker) target;
          break;
        case 19:
          this.JobEndDateBox = (RadDatePicker) target;
          break;
        case 20:
          this.JobLogsGridView = (RadGridView) target;
          break;
        case 21:
          this.RadDataPager2 = (RadDataPager) target;
          this.RadDataPager2.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 22:
          this.AddButton = (Button) target;
          break;
        case 23:
          this.AddButtonCommand = (TextBlock) target;
          break;
        case 24:
          this.EditButton = (Button) target;
          break;
        case 25:
          this.EditButtonCommand = (TextBlock) target;
          break;
        case 26:
          this.RemoveButton = (Button) target;
          break;
        case 27:
          this.RemoveButtonCommand = (TextBlock) target;
          break;
        case 28:
          this.JobDefinitionGridView = (RadGridView) target;
          break;
        case 29:
          this.RadDataPager3 = (RadDataPager) target;
          this.RadDataPager3.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 30:
          this.EditScenario = (Button) target;
          break;
        case 31:
          this.EditScenarioCommand = (TextBlock) target;
          break;
        case 32:
          this.ScenarioGridView = (RadGridView) target;
          break;
        case 33:
          this.RadDataPager4 = (RadDataPager) target;
          this.RadDataPager4.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
