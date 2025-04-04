// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Structures.EditPhysicalStructuresDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Structures
{
  public partial class EditPhysicalStructuresDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadBusyIndicator BusyIndicator;
    internal System.Windows.Controls.Button btnEditNode;
    internal System.Windows.Controls.Button btnDeleteNode;
    internal System.Windows.Controls.Button btnCutNode;
    internal System.Windows.Controls.Button btnPasteNode;
    internal System.Windows.Controls.Button btnPasteAfterNode;
    internal System.Windows.Controls.Button btnScanSetting;
    internal System.Windows.Controls.Button btnImportRadioMeters;
    internal System.Windows.Controls.Button btnPhotos;
    internal System.Windows.Controls.Button btnNotes;
    internal System.Windows.Controls.Button btnImportDeliveryNote;
    internal System.Windows.Controls.Button btnChangeDeviceModelParameters;
    internal RadTreeListView treeListView;
    internal RadGridView RadGridView1;
    internal System.Windows.Controls.Button StartScanButton;
    internal System.Windows.Controls.Button StopScanButton;
    internal System.Windows.Controls.Button OkButton;
    internal System.Windows.Controls.Button CancelButton;
    private bool _contentLoaded;

    public EditPhysicalStructuresDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~EditPhysicalStructuresDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.RadGridView1.DataLoaded -= new EventHandler<EventArgs>(this.RadGridView1_OnDataLoaded);
    }

    private void RadGridView1_OnDataLoaded(object sender, EventArgs e)
    {
      this.RadGridView1.ExpandAllHierarchyItems();
    }

    private void DisplaySettingsChanged(object sender, EventArgs e)
    {
      if (Screen.PrimaryScreen.Bounds.Height <= Screen.PrimaryScreen.Bounds.Width)
        ;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/structures/editphysicalstructuresdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        case 2:
          this.btnEditNode = (System.Windows.Controls.Button) target;
          break;
        case 3:
          this.btnDeleteNode = (System.Windows.Controls.Button) target;
          break;
        case 4:
          this.btnCutNode = (System.Windows.Controls.Button) target;
          break;
        case 5:
          this.btnPasteNode = (System.Windows.Controls.Button) target;
          break;
        case 6:
          this.btnPasteAfterNode = (System.Windows.Controls.Button) target;
          break;
        case 7:
          this.btnScanSetting = (System.Windows.Controls.Button) target;
          break;
        case 8:
          this.btnImportRadioMeters = (System.Windows.Controls.Button) target;
          break;
        case 9:
          this.btnPhotos = (System.Windows.Controls.Button) target;
          break;
        case 10:
          this.btnNotes = (System.Windows.Controls.Button) target;
          break;
        case 11:
          this.btnImportDeliveryNote = (System.Windows.Controls.Button) target;
          break;
        case 12:
          this.btnChangeDeviceModelParameters = (System.Windows.Controls.Button) target;
          break;
        case 13:
          this.treeListView = (RadTreeListView) target;
          break;
        case 14:
          this.RadGridView1 = (RadGridView) target;
          this.RadGridView1.DataLoaded += new EventHandler<EventArgs>(this.RadGridView1_OnDataLoaded);
          break;
        case 15:
          this.StartScanButton = (System.Windows.Controls.Button) target;
          break;
        case 16:
          this.StopScanButton = (System.Windows.Controls.Button) target;
          break;
        case 17:
          this.OkButton = (System.Windows.Controls.Button) target;
          break;
        case 18:
          this.CancelButton = (System.Windows.Controls.Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
