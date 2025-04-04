// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Download.DownloadStructuresDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Business.DTO;
using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Download
{
  public partial class DownloadStructuresDialog : ResizableMetroWindow, IComponentConnector
  {
    internal CheckBox LockBox;
    internal Button SearchButton;
    internal Grid StructureRootGrid;
    internal RadTreeListView treeListView;
    internal Button DownloadButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public DownloadStructuresDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~DownloadStructuresDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void TreeListView_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
    {
      if (!(sender is RadTreeListView radTreeListView))
        return;
      ObservableCollection<object> selectedItems = radTreeListView.SelectedItems;
      if (selectedItems.Count == 0)
      {
        this.DownloadButton.IsEnabled = false;
      }
      else
      {
        foreach (StructureNodeDTO structureNodeDto in (Collection<object>) selectedItems)
        {
          if (structureNodeDto.RootNode != structureNodeDto)
          {
            this.DownloadButton.IsEnabled = false;
            break;
          }
          this.DownloadButton.IsEnabled = true;
        }
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/download/downloadstructuresdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.LockBox = (CheckBox) target;
          break;
        case 2:
          this.SearchButton = (Button) target;
          break;
        case 3:
          this.StructureRootGrid = (Grid) target;
          break;
        case 4:
          this.treeListView = (RadTreeListView) target;
          this.treeListView.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(this.TreeListView_OnSelectionChanged);
          break;
        case 5:
          this.DownloadButton = (Button) target;
          break;
        case 6:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
