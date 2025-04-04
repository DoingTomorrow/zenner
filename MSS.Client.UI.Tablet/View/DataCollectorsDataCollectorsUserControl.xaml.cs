// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.DataCollectors.DataCollectorsUserControl
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

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
namespace MSS.Client.UI.Tablet.View.DataCollectors
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
    internal RadGridView RadGridView;
    internal RadDataPager RadDataPager;
    internal Button AddDataCollector;
    internal TextBlock AddDataCollectorTextBlock;
    internal Button RemoveDataCollector;
    internal TextBlock RemoveDataCollectorTextBlock;
    internal RadGridView RadGridViewPool;
    internal RadDataPager RadDataPager2;
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/datacollectors/datacollectorsusercontrol.xaml", UriKind.Relative));
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
          this.RadGridView = (RadGridView) target;
          break;
        case 8:
          this.RadDataPager = (RadDataPager) target;
          this.RadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 9:
          this.AddDataCollector = (Button) target;
          break;
        case 10:
          this.AddDataCollectorTextBlock = (TextBlock) target;
          break;
        case 11:
          this.RemoveDataCollector = (Button) target;
          break;
        case 12:
          this.RemoveDataCollectorTextBlock = (TextBlock) target;
          break;
        case 13:
          this.RadGridViewPool = (RadGridView) target;
          break;
        case 14:
          this.RadDataPager2 = (RadDataPager) target;
          this.RadDataPager2.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
