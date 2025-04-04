// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Structures.EditPhysicalStructuresDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MSS.Business.Utils;
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
namespace MSS.Client.UI.Desktop.View.Structures
{
  public partial class EditPhysicalStructuresDialog : ResizableMetroWindow, IComponentConnector
  {
    internal Button btnEditNode;
    internal TextBlock txtblockEditStructure;
    internal Button btnDeleteNode;
    internal TextBlock txtBlockDeleteStructure;
    internal Button btnScanSetting;
    internal TextBlock txtBlockScanSettings;
    internal Button btnImportRadioMeters;
    internal Button btnPhotos;
    internal Button btnNotes;
    internal Button btnImportDeliveryNote;
    internal Button btnChangeDeviceModelParameters;
    internal MultiselectTreeListView treeListView;
    internal RadTreeView AvailableNodesTreeView;
    internal Button StartScanButton;
    internal Button StopScanButton;
    internal Button OkButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public EditPhysicalStructuresDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~EditPhysicalStructuresDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    private void RadContextMenu_Opening(object sender, RoutedEventArgs e)
    {
      TreeListViewRow clickedElement = (sender as RadContextMenu).GetClickedElement<TreeListViewRow>();
      if (clickedElement != null)
      {
        clickedElement.IsSelected = true;
        clickedElement.Focus();
      }
      else
      {
        foreach (RadRowItem radRowItem in this.treeListView.ChildrenOfType<TreeListViewRow>())
          radRowItem.IsSelected = false;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/structures/editphysicalstructuresdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.btnEditNode = (Button) target;
          break;
        case 2:
          this.txtblockEditStructure = (TextBlock) target;
          break;
        case 3:
          this.btnDeleteNode = (Button) target;
          break;
        case 4:
          this.txtBlockDeleteStructure = (TextBlock) target;
          break;
        case 5:
          this.btnScanSetting = (Button) target;
          break;
        case 6:
          this.txtBlockScanSettings = (TextBlock) target;
          break;
        case 7:
          this.btnImportRadioMeters = (Button) target;
          break;
        case 8:
          this.btnPhotos = (Button) target;
          break;
        case 9:
          this.btnNotes = (Button) target;
          break;
        case 10:
          this.btnImportDeliveryNote = (Button) target;
          break;
        case 11:
          this.btnChangeDeviceModelParameters = (Button) target;
          break;
        case 12:
          this.treeListView = (MultiselectTreeListView) target;
          break;
        case 13:
          this.AvailableNodesTreeView = (RadTreeView) target;
          break;
        case 14:
          this.StartScanButton = (Button) target;
          break;
        case 15:
          this.StopScanButton = (Button) target;
          break;
        case 16:
          this.OkButton = (Button) target;
          break;
        case 17:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
