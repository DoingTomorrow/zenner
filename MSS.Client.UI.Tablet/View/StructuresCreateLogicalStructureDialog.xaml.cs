// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Structures.CreateLogicalStructureDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Business.DTO;
using MSS.Business.Modules.StructuresManagement;
using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Structures
{
  public partial class CreateLogicalStructureDialog : ResizableMetroWindow, IComponentConnector
  {
    internal System.Windows.Controls.Button btnEditNode;
    internal TextBlock txtblockEditStructure;
    internal System.Windows.Controls.Button btnDeleteNode;
    internal TextBlock txtBlockDeleteStructure;
    internal System.Windows.Controls.Button btnCutNode;
    internal TextBlock txtBlockCutStructure;
    internal System.Windows.Controls.Button btnPasteNode;
    internal TextBlock txtBlockPasteStructure;
    internal System.Windows.Controls.Button btnPasteAfterNode;
    internal TextBlock txtBlockPasteAfterStructure;
    internal RadTreeListView logicalTreeListView;
    internal RadGridView RadGridView1;
    internal System.Windows.Controls.Button OkButton;
    internal System.Windows.Controls.Button CancelButton;
    private bool _contentLoaded;

    public CreateLogicalStructureDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~CreateLogicalStructureDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.RadGridView1.DataLoaded -= new EventHandler<EventArgs>(this.RadGridView1_OnDataLoaded);
      this.RadGridView1.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(this.RadGridView1_OnSelectionChanged);
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

    private void RadGridView1_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
    {
      if (!(sender is RadTreeView radTreeView) || radTreeView.SelectedItem == null || !StructuresHelper.IsMeterWithMeterParent(radTreeView.SelectedItem as StructureNodeDTO))
        return;
      radTreeView.SelectedItem = (object) null;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/structures/createlogicalstructuredialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.btnEditNode = (System.Windows.Controls.Button) target;
          break;
        case 2:
          this.txtblockEditStructure = (TextBlock) target;
          break;
        case 3:
          this.btnDeleteNode = (System.Windows.Controls.Button) target;
          break;
        case 4:
          this.txtBlockDeleteStructure = (TextBlock) target;
          break;
        case 5:
          this.btnCutNode = (System.Windows.Controls.Button) target;
          break;
        case 6:
          this.txtBlockCutStructure = (TextBlock) target;
          break;
        case 7:
          this.btnPasteNode = (System.Windows.Controls.Button) target;
          break;
        case 8:
          this.txtBlockPasteStructure = (TextBlock) target;
          break;
        case 9:
          this.btnPasteAfterNode = (System.Windows.Controls.Button) target;
          break;
        case 10:
          this.txtBlockPasteAfterStructure = (TextBlock) target;
          break;
        case 11:
          this.logicalTreeListView = (RadTreeListView) target;
          break;
        case 12:
          this.RadGridView1 = (RadGridView) target;
          this.RadGridView1.DataLoaded += new EventHandler<EventArgs>(this.RadGridView1_OnDataLoaded);
          this.RadGridView1.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(this.RadGridView1_OnSelectionChanged);
          break;
        case 13:
          this.OkButton = (System.Windows.Controls.Button) target;
          break;
        case 14:
          this.CancelButton = (System.Windows.Controls.Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
