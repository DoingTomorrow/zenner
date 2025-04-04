// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Structures.StructuresUserControl
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

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
namespace MSS.Client.UI.Desktop.View.Structures
{
  public partial class StructuresUserControl : UserControl, IComponentConnector
  {
    private int _currentPagingNumber = 0;
    internal Button ExpandPhysicalStr;
    internal Button CollapsePhysicalStr;
    internal Button btnCreateStructure;
    internal TextBlock txtblockCreateStructure;
    internal Button btnEditStructure;
    internal TextBlock txtblockEditStructure;
    internal Button btnRemoveSelectedStructure;
    internal TextBlock textBlockRemoveStructure;
    internal Button btnDeleteSelectedStructure;
    internal TextBlock textBlockDeleteStructure;
    internal Button UnlockPhysicalStructureButton;
    internal TextBlock UnlockPhysicalStructureTextBox;
    internal Button btnReadingValues;
    internal TextBlock textBlockReadingValues;
    internal Button btnExportToFile;
    internal TextBlock textBlockExportToFile;
    internal Button btnImportFromFile;
    internal TextBlock textBlockImportFromFile;
    internal Button btnMigrateData;
    internal Grid StructureRootGrid;
    internal RadTreeListView treeListView;
    internal RadDataPager RadDataPagerControl;
    internal Button ExpandLogicalStr;
    internal Button CollapseLogicalStr;
    internal Button btnCreateLogicalStructure;
    internal TextBlock txtblockCreateLogicalStructure;
    internal Button btnEditLogicalStructure;
    internal TextBlock txtblockEditLogicalStructure;
    internal Button btnRemoveSelectedLogicalStructure;
    internal TextBlock textBlockRemoveLogicalStructure;
    internal Button btnDeleteSelectedLogicalStructure;
    internal TextBlock textBlockDeleteLogicalStructure;
    internal Button UnlockLogicalStructureButton;
    internal TextBlock UnlockLogicalStructureTextBox;
    internal Button btnLogicalReadingValues;
    internal TextBlock textBlockLogicalReadingValues;
    internal Button btnExportToFileLogical;
    internal TextBlock textBlockExportToFileLogical;
    internal Button btnImportFromFileLogical;
    internal TextBlock textBlockImportFromFileLogical;
    internal Grid LogicalStructureRootGrid;
    internal RadTreeListView logicalTreeListView;
    internal RadDataPager LogicalRadDataPager;
    internal Button ExpandFixedStr;
    internal Button CollapseFixedStr;
    internal Button btnCreateFixedStructure;
    internal TextBlock txtblockCreateFixedStructure;
    internal Button btnEditFixedStructure;
    internal TextBlock txtblockEditFixedStructure;
    internal Button btnRemoveSelectedFixedStructure;
    internal TextBlock textBlockRemoveFixedStructure;
    internal Button btnDeleteSelectedFixedStructure;
    internal TextBlock textBlockDeleteFixedStructure;
    internal Button RadioTestRunDialog;
    internal TextBlock RadioTestRunDialogTextBlock;
    internal Button AssignRadioTestRunDialog;
    internal TextBlock AssignRadioTestRunDialogTextBlock;
    internal Button UnlockFixedStructureButton;
    internal TextBlock UnlockFixedStructureTextBox;
    internal Button btnFixedReadingValues;
    internal TextBlock textBlockFixedReadingValues;
    internal Button btnExportToFileFixed;
    internal TextBlock textBlockExportToFileFixed;
    internal Button btnExportStructureFixed;
    internal TextBlock textBlockExportStructureToFileFixed;
    internal Button btnImportFromFileFixed;
    internal TextBlock textBlockImportFromFileFixed;
    internal Button btnImportFromSasFixed;
    internal TextBlock textBlockImportFromSasFixed;
    internal Button btnSetEvalFactorFixed;
    internal TextBlock textBlockSetEvalFactorFixed;
    internal Grid FixedStructureRootGrid;
    internal RadTreeListView fixedTreeListView;
    internal RadDataPager FixedRadDataPager;
    private bool _contentLoaded;

    public StructuresUserControl()
    {
      this.InitializeComponent();
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
    }

    ~StructuresUserControl()
    {
      this.RadDataPagerControl.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.LogicalRadDataPager.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.FixedRadDataPager.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/structures/structuresusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ExpandPhysicalStr = (Button) target;
          break;
        case 2:
          this.CollapsePhysicalStr = (Button) target;
          break;
        case 3:
          this.btnCreateStructure = (Button) target;
          break;
        case 4:
          this.txtblockCreateStructure = (TextBlock) target;
          break;
        case 5:
          this.btnEditStructure = (Button) target;
          break;
        case 6:
          this.txtblockEditStructure = (TextBlock) target;
          break;
        case 7:
          this.btnRemoveSelectedStructure = (Button) target;
          break;
        case 8:
          this.textBlockRemoveStructure = (TextBlock) target;
          break;
        case 9:
          this.btnDeleteSelectedStructure = (Button) target;
          break;
        case 10:
          this.textBlockDeleteStructure = (TextBlock) target;
          break;
        case 11:
          this.UnlockPhysicalStructureButton = (Button) target;
          break;
        case 12:
          this.UnlockPhysicalStructureTextBox = (TextBlock) target;
          break;
        case 13:
          this.btnReadingValues = (Button) target;
          break;
        case 14:
          this.textBlockReadingValues = (TextBlock) target;
          break;
        case 15:
          this.btnExportToFile = (Button) target;
          break;
        case 16:
          this.textBlockExportToFile = (TextBlock) target;
          break;
        case 17:
          this.btnImportFromFile = (Button) target;
          break;
        case 18:
          this.textBlockImportFromFile = (TextBlock) target;
          break;
        case 19:
          this.btnMigrateData = (Button) target;
          break;
        case 20:
          this.StructureRootGrid = (Grid) target;
          break;
        case 21:
          this.treeListView = (RadTreeListView) target;
          break;
        case 22:
          this.RadDataPagerControl = (RadDataPager) target;
          this.RadDataPagerControl.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 23:
          this.ExpandLogicalStr = (Button) target;
          break;
        case 24:
          this.CollapseLogicalStr = (Button) target;
          break;
        case 25:
          this.btnCreateLogicalStructure = (Button) target;
          break;
        case 26:
          this.txtblockCreateLogicalStructure = (TextBlock) target;
          break;
        case 27:
          this.btnEditLogicalStructure = (Button) target;
          break;
        case 28:
          this.txtblockEditLogicalStructure = (TextBlock) target;
          break;
        case 29:
          this.btnRemoveSelectedLogicalStructure = (Button) target;
          break;
        case 30:
          this.textBlockRemoveLogicalStructure = (TextBlock) target;
          break;
        case 31:
          this.btnDeleteSelectedLogicalStructure = (Button) target;
          break;
        case 32:
          this.textBlockDeleteLogicalStructure = (TextBlock) target;
          break;
        case 33:
          this.UnlockLogicalStructureButton = (Button) target;
          break;
        case 34:
          this.UnlockLogicalStructureTextBox = (TextBlock) target;
          break;
        case 35:
          this.btnLogicalReadingValues = (Button) target;
          break;
        case 36:
          this.textBlockLogicalReadingValues = (TextBlock) target;
          break;
        case 37:
          this.btnExportToFileLogical = (Button) target;
          break;
        case 38:
          this.textBlockExportToFileLogical = (TextBlock) target;
          break;
        case 39:
          this.btnImportFromFileLogical = (Button) target;
          break;
        case 40:
          this.textBlockImportFromFileLogical = (TextBlock) target;
          break;
        case 41:
          this.LogicalStructureRootGrid = (Grid) target;
          break;
        case 42:
          this.logicalTreeListView = (RadTreeListView) target;
          break;
        case 43:
          this.LogicalRadDataPager = (RadDataPager) target;
          this.LogicalRadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 44:
          this.ExpandFixedStr = (Button) target;
          break;
        case 45:
          this.CollapseFixedStr = (Button) target;
          break;
        case 46:
          this.btnCreateFixedStructure = (Button) target;
          break;
        case 47:
          this.txtblockCreateFixedStructure = (TextBlock) target;
          break;
        case 48:
          this.btnEditFixedStructure = (Button) target;
          break;
        case 49:
          this.txtblockEditFixedStructure = (TextBlock) target;
          break;
        case 50:
          this.btnRemoveSelectedFixedStructure = (Button) target;
          break;
        case 51:
          this.textBlockRemoveFixedStructure = (TextBlock) target;
          break;
        case 52:
          this.btnDeleteSelectedFixedStructure = (Button) target;
          break;
        case 53:
          this.textBlockDeleteFixedStructure = (TextBlock) target;
          break;
        case 54:
          this.RadioTestRunDialog = (Button) target;
          break;
        case 55:
          this.RadioTestRunDialogTextBlock = (TextBlock) target;
          break;
        case 56:
          this.AssignRadioTestRunDialog = (Button) target;
          break;
        case 57:
          this.AssignRadioTestRunDialogTextBlock = (TextBlock) target;
          break;
        case 58:
          this.UnlockFixedStructureButton = (Button) target;
          break;
        case 59:
          this.UnlockFixedStructureTextBox = (TextBlock) target;
          break;
        case 60:
          this.btnFixedReadingValues = (Button) target;
          break;
        case 61:
          this.textBlockFixedReadingValues = (TextBlock) target;
          break;
        case 62:
          this.btnExportToFileFixed = (Button) target;
          break;
        case 63:
          this.textBlockExportToFileFixed = (TextBlock) target;
          break;
        case 64:
          this.btnExportStructureFixed = (Button) target;
          break;
        case 65:
          this.textBlockExportStructureToFileFixed = (TextBlock) target;
          break;
        case 66:
          this.btnImportFromFileFixed = (Button) target;
          break;
        case 67:
          this.textBlockImportFromFileFixed = (TextBlock) target;
          break;
        case 68:
          this.btnImportFromSasFixed = (Button) target;
          break;
        case 69:
          this.textBlockImportFromSasFixed = (TextBlock) target;
          break;
        case 70:
          this.btnSetEvalFactorFixed = (Button) target;
          break;
        case 71:
          this.textBlockSetEvalFactorFixed = (TextBlock) target;
          break;
        case 72:
          this.FixedStructureRootGrid = (Grid) target;
          break;
        case 73:
          this.fixedTreeListView = (RadTreeListView) target;
          break;
        case 74:
          this.FixedRadDataPager = (RadDataPager) target;
          this.FixedRadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
