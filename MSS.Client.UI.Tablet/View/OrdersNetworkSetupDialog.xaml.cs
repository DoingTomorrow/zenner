// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.NetworkSetupDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Orders
{
  public partial class NetworkSetupDialog : 
    ResizableMetroWindow,
    IComponentConnector,
    IStyleConnector
  {
    internal RadBusyIndicator BusyIndicator;
    internal System.Windows.Controls.Label MasterLastStartOn;
    internal System.Windows.Controls.Label GSMTestStatus;
    internal System.Windows.Controls.Label GSMStatus;
    internal System.Windows.Controls.Label GSMStatusDate;
    internal ListBox SlavesListBox;
    private bool _contentLoaded;

    public NetworkSetupDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~NetworkSetupDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void SlaveButton_OnClick(object sender, RoutedEventArgs e)
    {
      object dataContext = sender is Button button ? button.DataContext : (object) null;
      if (dataContext == null)
        return;
      this.SlavesListBox.SelectedItem = this.SlavesListBox.Items.GetItemAt(this.SlavesListBox.Items.IndexOf(dataContext));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/networksetupdialog.xaml", UriKind.Relative));
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
        case 2:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        case 3:
          this.MasterLastStartOn = (System.Windows.Controls.Label) target;
          break;
        case 4:
          this.GSMTestStatus = (System.Windows.Controls.Label) target;
          break;
        case 5:
          this.GSMStatus = (System.Windows.Controls.Label) target;
          break;
        case 6:
          this.GSMStatusDate = (System.Windows.Controls.Label) target;
          break;
        case 7:
          this.SlavesListBox = (ListBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
        return;
      ((ButtonBase) target).Click += new RoutedEventHandler(this.SlaveButton_OnClick);
    }
  }
}
