// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.ExecuteInstallationOrderDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.CustomControls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using Telerik.Windows;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Orders
{
  public partial class ExecuteInstallationOrderDialog : 
    ResizableMetroWindow,
    IComponentConnector,
    IStyleConnector
  {
    private DataGrid _lastShownRow;
    internal TabletButton AddTenantButton;
    internal TabletButton AddMeterButton;
    internal DataGrid TenantDataGrid;
    internal TabletButton AddMasterButton;
    internal TabletButton AddSlaveButton;
    internal TabletButton EditMasterOrSlave;
    internal DataGrid MinomatDataGrid;
    private bool _contentLoaded;

    public ExecuteInstallationOrderDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.Closed += new EventHandler(this.OnWindowClosed);
    }

    ~ExecuteInstallationOrderDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.Closed -= new EventHandler(this.OnWindowClosed);
      this.AddTenantButton.TouchDown -= new EventHandler<TouchEventArgs>(this.AddTenant_OnTouchDown);
      this.AddMeterButton.TouchDown -= new EventHandler<TouchEventArgs>(this.UIElement_OnPreviewTouchDown);
      this.AddMasterButton.TouchDown -= new EventHandler<TouchEventArgs>(this.AddMaster_OnTouchDown);
      this.AddSlaveButton.TouchDown -= new EventHandler<TouchEventArgs>(this.AddSlave_OnTouchDown);
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

    private void UIElement_OnPreviewTouchDown(object sender, TouchEventArgs e)
    {
      Application.Current.Dispatcher.InvokeAsync((Action) (() =>
      {
        if (this.TenantDataGrid.SelectedItem != null)
        {
          RadExpander radExpander = this.TenantDataGrid.ItemContainerGenerator.ContainerFromItem(this.TenantDataGrid.SelectedItem) is DataGridRow element2 ? element2.ChildrenOfType<RadExpander>().LastOrDefault<RadExpander>() : (RadExpander) null;
          if (radExpander == null || radExpander.IsExpanded)
            return;
          radExpander.IsExpanded = true;
        }
        else
        {
          DependencyObject element3 = this._lastShownRow.ItemContainerGenerator.ContainerFromItem(this._lastShownRow.SelectedItem);
          RadExpander radExpander = element3 != null ? element3.ChildrenOfType<RadExpander>().FirstOrDefault<RadExpander>() : (RadExpander) null;
          if (radExpander != null)
            radExpander.IsExpanded = true;
        }
      }), DispatcherPriority.Render);
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count <= 0)
        return;
      this._lastShownRow = sender as DataGrid;
    }

    private void AddTenant_OnTouchDown(object sender, TouchEventArgs e)
    {
      if (this.TenantDataGrid.Items.Count <= 0)
        return;
      this.TenantDataGrid.ScrollIntoView(this.TenantDataGrid.Items[0]);
    }

    private void AddMaster_OnTouchDown(object sender, TouchEventArgs e)
    {
      if (this.MinomatDataGrid.Items.Count <= 0)
        return;
      this.MinomatDataGrid.ScrollIntoView(this.MinomatDataGrid.Items[0]);
    }

    private void AddSlave_OnTouchDown(object sender, TouchEventArgs e)
    {
      if (!(this.MinomatDataGrid.ItemContainerGenerator.ContainerFromItem(this.MinomatDataGrid.SelectedItem) is DataGridRow element))
        return;
      element.ChildrenOfType<RadExpander>().FirstOrDefault<RadExpander>().IsExpanded = true;
    }

    private void OnWindowClosed(object sender, EventArgs e)
    {
      if (!(this.DataContext is IDisposable dataContext))
        return;
      dataContext.Dispose();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/executeinstallationorderdialog.xaml", UriKind.Relative));
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
          this.AddTenantButton = (TabletButton) target;
          break;
        case 2:
          this.AddMeterButton = (TabletButton) target;
          break;
        case 3:
          this.TenantDataGrid = (DataGrid) target;
          break;
        case 7:
          this.AddMasterButton = (TabletButton) target;
          break;
        case 8:
          this.AddSlaveButton = (TabletButton) target;
          break;
        case 9:
          this.EditMasterOrSlave = (TabletButton) target;
          break;
        case 10:
          this.MinomatDataGrid = (DataGrid) target;
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
        case 4:
          ((RadExpander) target).Collapsed += new RadRoutedEventHandler(this.Expander_Collapsed);
          ((RadExpander) target).Expanded += new RadRoutedEventHandler(this.Expander_Expanded);
          break;
        case 5:
          ((Selector) target).SelectionChanged += new SelectionChangedEventHandler(this.Selector_OnSelectionChanged);
          break;
        case 6:
          ((RadExpander) target).Collapsed += new RadRoutedEventHandler(this.Expander_Collapsed);
          ((RadExpander) target).Expanded += new RadRoutedEventHandler(this.Expander_Expanded);
          break;
        case 11:
          ((RadExpander) target).Collapsed += new RadRoutedEventHandler(this.Expander_Collapsed);
          ((RadExpander) target).Expanded += new RadRoutedEventHandler(this.Expander_Expanded);
          break;
      }
    }
  }
}
