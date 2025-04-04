// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Synchronization.ShowConflictsDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Synchronization
{
  public partial class ShowConflictsDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadGridView ConflictedFiltersGridView;
    internal RadGridView ConflictedRulesGridView;
    internal RadGridView ConflictedOrdersGridView;
    internal RadGridView ConflictedMetersGridView;
    internal RadGridView ConflictedLocationsGridView;
    internal RadGridView ConflictedTenantsGridView;
    internal RadGridView ConflictedUsersGridView;
    internal RadGridView ConflictedRolesGridView;
    internal RadGridView ConflictedStructuresGridView;
    internal RadGridView ConflictedStructuresLinksGridView;
    internal Button ApplyButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public ShowConflictsDialog()
    {
      this.InitializeComponent();
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~ShowConflictsDialog()
    {
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.ConflictedFiltersGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedRulesGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedOrdersGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedMetersGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedLocationsGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedTenantsGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedUsersGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedRolesGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedStructuresGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedStructuresLinksGridView.RowLoaded -= new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
      this.ConflictedFiltersGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
      this.ConflictedRulesGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
      this.ConflictedOrdersGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
      this.ConflictedMetersGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
      this.ConflictedLocationsGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
      this.ConflictedTenantsGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
      this.ConflictedUsersGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
      this.ConflictedRolesGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
      this.ConflictedStructuresGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
      this.ConflictedStructuresLinksGridView.AutoGeneratingColumn -= new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
    }

    private void ConflictedEntityGridView_RowLoaded(object sender, RowLoadedEventArgs e)
    {
      if (!(e.Row is GridViewRow row))
        return;
      row.AddHandler(UIElement.MouseLeftButtonDownEvent, (Delegate) new MouseButtonEventHandler(this.ConflictedEntityGridViewRow_MouseLeftButtonDown), true);
    }

    private void ConflictedEntityGridViewRow_MouseLeftButtonDown(
      object sender,
      MouseButtonEventArgs e)
    {
      if (!(sender is GridViewRowItem gridViewRowItem) || !(gridViewRowItem.Item is DataRowView dataRowView))
        return;
      DataView dataView = dataRowView.DataView;
      dataRowView[0] = (object) "pack://application:,,,/Styles;component/Images/Universal/selected_conflict.png";
      int num = dataRowView.DataView.Table.Rows.IndexOf(dataRowView.Row);
      if (num % 2 == 1)
        dataView[num - 1][0] = (object) "pack://application:,,,/Styles;component/Images/Universal/unselected_conflict.png";
      else
        dataView[num + 1][0] = (object) "pack://application:,,,/Styles;component/Images/Universal/unselected_conflict.png";
    }

    private void ConflictedEntityGridView_AutoGeneratingColumn(
      object sender,
      GridViewAutoGeneratingColumnEventArgs e)
    {
      if (!((string) e.Column.Header == "Id"))
        return;
      e.Cancel = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/synchronization/showconflictsdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ConflictedFiltersGridView = (RadGridView) target;
          this.ConflictedFiltersGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedFiltersGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 2:
          this.ConflictedRulesGridView = (RadGridView) target;
          this.ConflictedRulesGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedRulesGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 3:
          this.ConflictedOrdersGridView = (RadGridView) target;
          this.ConflictedOrdersGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedOrdersGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 4:
          this.ConflictedMetersGridView = (RadGridView) target;
          this.ConflictedMetersGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedMetersGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 5:
          this.ConflictedLocationsGridView = (RadGridView) target;
          this.ConflictedLocationsGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedLocationsGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 6:
          this.ConflictedTenantsGridView = (RadGridView) target;
          this.ConflictedTenantsGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedTenantsGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 7:
          this.ConflictedUsersGridView = (RadGridView) target;
          this.ConflictedUsersGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedUsersGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 8:
          this.ConflictedRolesGridView = (RadGridView) target;
          this.ConflictedRolesGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedRolesGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 9:
          this.ConflictedStructuresGridView = (RadGridView) target;
          this.ConflictedStructuresGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedStructuresGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 10:
          this.ConflictedStructuresLinksGridView = (RadGridView) target;
          this.ConflictedStructuresLinksGridView.AutoGeneratingColumn += new EventHandler<GridViewAutoGeneratingColumnEventArgs>(this.ConflictedEntityGridView_AutoGeneratingColumn);
          this.ConflictedStructuresLinksGridView.RowLoaded += new EventHandler<RowLoadedEventArgs>(this.ConflictedEntityGridView_RowLoaded);
          break;
        case 11:
          this.ApplyButton = (Button) target;
          break;
        case 12:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
