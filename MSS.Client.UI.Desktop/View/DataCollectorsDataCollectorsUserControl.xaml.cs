// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.DataCollectors.DataCollectorsUserControl
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
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Desktop.View.DataCollectors
{
  public partial class DataCollectorsUserControl : UserControl, IComponentConnector
  {
    private int _currentPagingNumber = 0;
    internal Button AddMinomatButton;
    internal TextBlock CreateReadingOrderTextBox;
    internal Button EditMinomatButton;
    internal TextBlock EditReadingOrderTextBox;
    internal Button RemoveMinomatButton;
    internal TextBlock DeleteReadingOrderTextBox;
    internal Button EnableLoggingButton;
    internal TextBlock EnableLoggingTextBox;
    internal Button DisableLoggingButton;
    internal TextBlock DisableLoggingTextBox;
    internal RadGridView RadGridView;
    internal RadDataPager RadDataPager;
    internal Button AddDataCollector;
    internal TextBlock AddDataCollectorTextBlock;
    internal Button RemoveDataCollector;
    internal TextBlock RemoveDataCollectorTextBlock;
    internal RadGridView RadGridViewPool;
    internal RadDataPager RadDataPager2;
    internal Button LoadDataLogs;
    internal TextBlock LoadDataLogsCommand;
    internal TextBox MasterNumberValueTextBox;
    internal RadDatePicker StartDateBox;
    internal RadDatePicker EndDateBox;
    internal RadGridView MinomatCommunicationLogsGridView;
    internal RadDataPager RadDataPager3;
    private bool _contentLoaded;

    public DataCollectorsUserControl()
    {
      this.InitializeComponent();
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
    }

    ~DataCollectorsUserControl()
    {
      this.RadDataPager.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.RadDataPager2.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.RadDataPager3.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.MasterNumberValueTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = DataCollectorsUserControl.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/datacollectors/datacollectorsusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.AddMinomatButton = (Button) target;
          break;
        case 2:
          this.CreateReadingOrderTextBox = (TextBlock) target;
          break;
        case 3:
          this.EditMinomatButton = (Button) target;
          break;
        case 4:
          this.EditReadingOrderTextBox = (TextBlock) target;
          break;
        case 5:
          this.RemoveMinomatButton = (Button) target;
          break;
        case 6:
          this.DeleteReadingOrderTextBox = (TextBlock) target;
          break;
        case 7:
          this.EnableLoggingButton = (Button) target;
          break;
        case 8:
          this.EnableLoggingTextBox = (TextBlock) target;
          break;
        case 9:
          this.DisableLoggingButton = (Button) target;
          break;
        case 10:
          this.DisableLoggingTextBox = (TextBlock) target;
          break;
        case 11:
          this.RadGridView = (RadGridView) target;
          break;
        case 12:
          this.RadDataPager = (RadDataPager) target;
          this.RadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 13:
          this.AddDataCollector = (Button) target;
          break;
        case 14:
          this.AddDataCollectorTextBlock = (TextBlock) target;
          break;
        case 15:
          this.RemoveDataCollector = (Button) target;
          break;
        case 16:
          this.RemoveDataCollectorTextBlock = (TextBlock) target;
          break;
        case 17:
          this.RadGridViewPool = (RadGridView) target;
          break;
        case 18:
          this.RadDataPager2 = (RadDataPager) target;
          this.RadDataPager2.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 19:
          this.LoadDataLogs = (Button) target;
          break;
        case 20:
          this.LoadDataLogsCommand = (TextBlock) target;
          break;
        case 21:
          this.MasterNumberValueTextBox = (TextBox) target;
          this.MasterNumberValueTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 22:
          this.StartDateBox = (RadDatePicker) target;
          break;
        case 23:
          this.EndDateBox = (RadDatePicker) target;
          break;
        case 24:
          this.MinomatCommunicationLogsGridView = (RadGridView) target;
          break;
        case 25:
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
