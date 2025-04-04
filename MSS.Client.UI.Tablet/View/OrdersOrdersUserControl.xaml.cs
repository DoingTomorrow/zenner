// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.OrdersUserControl
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
namespace MSS.Client.UI.Tablet.View.Orders
{
  public partial class OrdersUserControl : UserControl, IComponentConnector
  {
    private int _currentPagingNumber = 0;
    internal OrdersUserControl ordersUserControl;
    internal Button AddInstallationOrderButton;
    internal TextBlock CreateInstallationOrderTextBox;
    internal Button EditInstallationOrderButton;
    internal TextBlock EditInstallationOrderCommandTextBox;
    internal Button RemoveInstallationOrderButton;
    internal TextBlock DeleteInstallationOrderCommandTextBox;
    internal Button ExecuteInstallationOrderButton;
    internal TextBlock ExecuteInstallationOrderCommandTextBox;
    internal Button PrintInstallationOrderButton;
    internal TextBlock PrintInstallationOrderTextBox;
    internal RadGridView InstallationOrderGridView;
    internal RadDataPager RadDataPager;
    internal Button AddReadingOrderButton;
    internal TextBlock CreateReadingOrderTextBox;
    internal Button EditReadingOrderButton;
    internal TextBlock EditReadingOrderTextBox;
    internal Button RemoveReadingOrderButton;
    internal TextBlock DeleteReadingOrderTextBox;
    internal Button ExecuteReadingOrderButton;
    internal TextBlock ExecuteReadingOrderTextBox;
    internal Button UnlockReadingOrderButton;
    internal TextBlock UnlockReadingOrderCommandTextBox;
    internal Button PrintReadingOrderButton;
    internal TextBlock PrintReadingOrderTextBox;
    internal Button btnReadingValues;
    internal TextBlock textBlockReadingValues;
    internal RadGridView ReadingOrderGridView;
    internal RadDataPager RadDataPager2;
    private bool _contentLoaded;

    public OrdersUserControl()
    {
      this.InitializeComponent();
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
    }

    ~OrdersUserControl()
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/ordersusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ordersUserControl = (OrdersUserControl) target;
          break;
        case 2:
          this.AddInstallationOrderButton = (Button) target;
          break;
        case 3:
          this.CreateInstallationOrderTextBox = (TextBlock) target;
          break;
        case 4:
          this.EditInstallationOrderButton = (Button) target;
          break;
        case 5:
          this.EditInstallationOrderCommandTextBox = (TextBlock) target;
          break;
        case 6:
          this.RemoveInstallationOrderButton = (Button) target;
          break;
        case 7:
          this.DeleteInstallationOrderCommandTextBox = (TextBlock) target;
          break;
        case 8:
          this.ExecuteInstallationOrderButton = (Button) target;
          break;
        case 9:
          this.ExecuteInstallationOrderCommandTextBox = (TextBlock) target;
          break;
        case 10:
          this.PrintInstallationOrderButton = (Button) target;
          break;
        case 11:
          this.PrintInstallationOrderTextBox = (TextBlock) target;
          break;
        case 12:
          this.InstallationOrderGridView = (RadGridView) target;
          break;
        case 13:
          this.RadDataPager = (RadDataPager) target;
          this.RadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 14:
          this.AddReadingOrderButton = (Button) target;
          break;
        case 15:
          this.CreateReadingOrderTextBox = (TextBlock) target;
          break;
        case 16:
          this.EditReadingOrderButton = (Button) target;
          break;
        case 17:
          this.EditReadingOrderTextBox = (TextBlock) target;
          break;
        case 18:
          this.RemoveReadingOrderButton = (Button) target;
          break;
        case 19:
          this.DeleteReadingOrderTextBox = (TextBlock) target;
          break;
        case 20:
          this.ExecuteReadingOrderButton = (Button) target;
          break;
        case 21:
          this.ExecuteReadingOrderTextBox = (TextBlock) target;
          break;
        case 22:
          this.UnlockReadingOrderButton = (Button) target;
          break;
        case 23:
          this.UnlockReadingOrderCommandTextBox = (TextBlock) target;
          break;
        case 24:
          this.PrintReadingOrderButton = (Button) target;
          break;
        case 25:
          this.PrintReadingOrderTextBox = (TextBlock) target;
          break;
        case 26:
          this.btnReadingValues = (Button) target;
          break;
        case 27:
          this.textBlockReadingValues = (TextBlock) target;
          break;
        case 28:
          this.ReadingOrderGridView = (RadGridView) target;
          break;
        case 29:
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
