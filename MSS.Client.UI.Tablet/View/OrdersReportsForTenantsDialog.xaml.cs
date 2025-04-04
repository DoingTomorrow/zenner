// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.ReportsForTenantsDialog
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
using Telerik.Windows;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Orders
{
  public partial class ReportsForTenantsDialog : 
    ResizableMetroWindow,
    IComponentConnector,
    IStyleConnector
  {
    private DataGrid _lastShownRow;
    internal ReportsForTenantsDialog ReportsForTenantsWindow;
    internal DataGrid TenantsGridView;
    internal RadBusyIndicator BusyIndicator;
    internal RadGridView MastersInfoCollection;
    internal RadGridView SlavesInfoCollection;
    internal RadGridView MetersInfoCollection;
    private bool _contentLoaded;

    public ReportsForTenantsDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~ReportsForTenantsDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void Expander_Expanded(object sender, RoutedEventArgs e)
    {
      FrameworkElement originalSource = e.OriginalSource as FrameworkElement;
      if (!(originalSource is RadExpander))
        return;
      DataGridRow dataGridRow = originalSource.ParentOfType<DataGridRow>();
      if (dataGridRow != null)
      {
        if (!dataGridRow.IsSelected)
          dataGridRow.IsSelected = true;
        dataGridRow.DetailsVisibility = Visibility.Visible;
      }
    }

    private void Expander_Collapsed(object sender, RoutedEventArgs e)
    {
      FrameworkElement originalSource = e.OriginalSource as FrameworkElement;
      if (!(originalSource is RadExpander))
        return;
      DataGridRow dataGridRow = originalSource.ParentOfType<DataGridRow>();
      if (dataGridRow != null)
      {
        if (!dataGridRow.IsSelected)
          dataGridRow.IsSelected = true;
        dataGridRow.DetailsVisibility = Visibility.Collapsed;
      }
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count <= 0)
        return;
      this._lastShownRow = sender as DataGrid;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/reportsfortenantsdialog.xaml", UriKind.Relative));
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
          this.ReportsForTenantsWindow = (ReportsForTenantsDialog) target;
          break;
        case 2:
          this.TenantsGridView = (DataGrid) target;
          break;
        case 6:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        case 7:
          this.MastersInfoCollection = (RadGridView) target;
          break;
        case 8:
          this.SlavesInfoCollection = (RadGridView) target;
          break;
        case 9:
          this.MetersInfoCollection = (RadGridView) target;
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
      switch (connectionId)
      {
        case 3:
          ((RadExpander) target).Collapsed += new RadRoutedEventHandler(this.Expander_Collapsed);
          ((RadExpander) target).Expanded += new RadRoutedEventHandler(this.Expander_Expanded);
          break;
        case 4:
          ((Selector) target).SelectionChanged += new SelectionChangedEventHandler(this.Selector_OnSelectionChanged);
          break;
        case 5:
          ((RadExpander) target).Collapsed += new RadRoutedEventHandler(this.Expander_Collapsed);
          ((RadExpander) target).Expanded += new RadRoutedEventHandler(this.Expander_Expanded);
          break;
      }
    }
  }
}
