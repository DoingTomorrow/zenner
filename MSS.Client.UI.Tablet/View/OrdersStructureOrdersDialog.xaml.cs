// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.StructureOrdersDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Business.DTO;
using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeListView;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Orders
{
  public partial class StructureOrdersDialog : ResizableMetroWindow, IComponentConnector
  {
    internal TextBox SearchTextBox;
    internal Grid StructureRootGrid;
    internal RadTreeListView treeListView;
    internal RadDateTimePicker DueDateTimePicker;
    internal RadDateTimePicker DueDateStartTimePicker;
    internal RadDateTimePicker DueDateEndTimePicker;
    internal Button OkButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public StructureOrdersDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~StructureOrdersDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void TreeListView_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
    {
      foreach (TreeListViewRow treeListViewRow in (sender as RadTreeListView).ChildrenOfType<TreeListViewRow>())
      {
        if (treeListViewRow.IsSelected && treeListViewRow.Item is StructureNodeDTO structureNodeDto && structureNodeDto.RootNode != structureNodeDto)
          treeListViewRow.IsSelected = false;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/structureordersdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.SearchTextBox = (TextBox) target;
          break;
        case 2:
          this.StructureRootGrid = (Grid) target;
          break;
        case 3:
          this.treeListView = (RadTreeListView) target;
          break;
        case 4:
          this.DueDateTimePicker = (RadDateTimePicker) target;
          break;
        case 5:
          this.DueDateStartTimePicker = (RadDateTimePicker) target;
          break;
        case 6:
          this.DueDateEndTimePicker = (RadDateTimePicker) target;
          break;
        case 7:
          this.OkButton = (Button) target;
          break;
        case 8:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
