// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_LCD_Menu
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Properties;

#nullable disable
namespace S3_Handler
{
  internal class S3_LCD_Menu : Form
  {
    private readonly Color USED_FUNCTION_COLOR = Color.LightYellow;
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private SplitContainer splitContainer1;
    private TreeView treeViewMenu;
    private TreeView treeViewAvailableFunctions;
    private SplitContainer splitContainer3;
    private Button btnOk;
    private Button btnCancel;
    private GroupBox groupBox1;
    private TextBox txtFunctionNumber;
    private Label label4;
    private ContextMenuStrip contextMenuActivFunction;
    private ToolStripMenuItem btnDeleteActivFunction;
    internal Button btnPrint;
    private ToolStripMenuItem setAsProtectedToolStripMenuItem;
    private ToolStripMenuItem btnSetAsProtected;
    private ToolStripMenuItem btnSetAsNotProtected;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem btnTreeCollaps;
    private ToolStripMenuItem btnTreeExpand;
    private ToolStripMenuItem btnAddLayer;
    private TextBox txtFunctionVersion;
    private Label label1;
    private Label label2;
    private ListBox listNeedResources;
    private Label lblOldFunction;
    private Button buttonGetText;
    private TextBox textBoxFirmwareRange;
    private Label label3;

    internal S3_LCD_Menu(Form owner, S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.Owner = owner;
      this.MyFunctions = MyFunctions;
      if (!this.MyFunctions.MyMeters.NewWorkMeter("LCD menu changed"))
        return;
      this.MyMeter = this.MyFunctions.MyMeters.GetActiveMeter();
      this.InitializeComponent();
      ImageList imageList = new ImageList();
      imageList.Images.Add((Image) Resources.FolderNode);
      imageList.Images.Add((Image) Resources.Function);
      imageList.Images.Add((Image) Resources.ProtectedMethod);
      imageList.Images.Add((Image) Resources.Property);
      imageList.Images.Add((Image) Resources.Event);
      this.treeViewMenu.ImageList = imageList;
      this.treeViewAvailableFunctions.ImageList = imageList;
    }

    private void S3_LCD_Menu_Load(object sender, EventArgs e) => this.LoadFunctions();

    private void LoadFunctions()
    {
      this.treeViewAvailableFunctions.Nodes.Clear();
      this.treeViewMenu.Nodes.Clear();
      List<Function> cachedFunctionList = this.MyFunctions.MyDatabase.CachedFunctionList;
      if (cachedFunctionList == null)
        return;
      foreach (Function f in cachedFunctionList)
        this.AddFunction(this.treeViewAvailableFunctions, f);
      foreach (TreeNode node in this.treeViewAvailableFunctions.Nodes)
        node.Expand();
      this.CreateUsedFunctionsTree();
    }

    private void CreateUsedFunctionsTree()
    {
      this.treeViewMenu.Nodes.Clear();
      foreach (S3_FunctionLayer childMemoryBlock1 in this.MyMeter.MyFunctionManager.MyFunctionTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks != null)
        {
          int layerNr = childMemoryBlock1.LayerNr;
          string str = "***** Layer " + layerNr.ToString() + " *****";
          if (childMemoryBlock1.LayerNr == 0)
            str = "***** Fixed  *****";
          TreeNodeCollection nodes = this.treeViewMenu.Nodes;
          layerNr = childMemoryBlock1.LayerNr;
          string key = layerNr.ToString();
          string text = str;
          TreeNode treeNode = nodes.Add(key, text, 0, 0);
          foreach (S3_Function childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            TreeNode functionNode = this.FindFunctionNode(this.treeViewAvailableFunctions.Nodes, (int) childMemoryBlock2.FunctionNumber);
            if (functionNode != null)
            {
              TreeNode node = (TreeNode) functionNode.Clone();
              node.ImageIndex = childMemoryBlock2.IsProtected ? 2 : 1;
              node.SelectedImageIndex = childMemoryBlock2.IsProtected ? 2 : 1;
              node.BackColor = Color.Empty;
              treeNode.Nodes.Add(node);
              functionNode.BackColor = this.USED_FUNCTION_COLOR;
            }
          }
          treeNode.Expand();
        }
      }
      if (this.treeViewMenu.Nodes.Count > 0)
        this.treeViewMenu.Nodes[0].EnsureVisible();
      List<Function> cachedFunctionList = this.MyFunctions.MyDatabase.CachedFunctionList;
      if (cachedFunctionList == null)
        return;
      foreach (Function function in cachedFunctionList)
      {
        Function f = function;
        if (cachedFunctionList.Exists((Predicate<Function>) (e => e.FunctionName == f.FunctionName && e.FunctionVersion > f.FunctionVersion)))
        {
          TreeNode functionNode1 = this.FindFunctionNode(this.treeViewAvailableFunctions.Nodes, f.FunctionNumber);
          functionNode1?.Parent.Nodes.Remove(functionNode1);
          TreeNode functionNode2 = this.FindFunctionNode(this.treeViewMenu.Nodes, f.FunctionNumber);
          if (functionNode2 != null)
            functionNode2.BackColor = this.lblOldFunction.BackColor;
        }
      }
      if (this.treeViewAvailableFunctions.Nodes.Count <= 0)
        return;
      this.treeViewAvailableFunctions.Nodes[0].EnsureVisible();
    }

    private TreeNode FindFunctionNode(TreeNodeCollection nodes, int functionNumber)
    {
      if (nodes == null || nodes.Count == 0)
        return (TreeNode) null;
      foreach (TreeNode node in nodes)
      {
        if (node.Tag is FunctionPrecompiled)
          return (TreeNode) null;
        if (node.Tag is Function && ((Function) node.Tag).FunctionNumber == functionNumber)
          return node;
        TreeNode functionNode = this.FindFunctionNode(node.Nodes, functionNumber);
        if (functionNode != null)
          return functionNode;
      }
      return (TreeNode) null;
    }

    private void AddFunction(TreeView tree, Function f)
    {
      uint firmwareVersion = this.MyMeter.MyIdentification.FirmwareVersion;
      if ((long) firmwareVersion < (long) f.FirmwareVersionMin || (long) firmwareVersion > (long) f.FirmwareVersionMax || !this.MyFunctions.baseTypeEditMode && tree != this.treeViewAvailableFunctions && !this.MyMeter.MyResources.AreAllResourcesAvailable(f.NeedResources, out string _))
        return;
      if (!tree.Nodes.ContainsKey(f.FunctionGroup))
        tree.Nodes.Add(f.FunctionGroup, f.FunctionGroup, 0, 0);
      TreeNode node = tree.Nodes[f.FunctionGroup].Nodes.Add(f.FunctionName, f.FunctionName, 1, 1);
      node.Tag = (object) f;
      this.AddStructureOfFunction(node);
    }

    private void AddStructureOfFunction(TreeNode node)
    {
      Function tag = node.Tag as Function;
      TreeNode n = node;
      List<TreeNode> treeNodeList = new List<TreeNode>();
      FunctionRecordType functionRecordType = FunctionRecordType.None;
      foreach (FunctionPrecompiled functionPrecompiled in tag.Precompiled)
      {
        FunctionPrecompiled item = functionPrecompiled;
        if (item.RecordType == FunctionRecordType.DisplayCode)
        {
          if (item.RecordOrder == (short) 0)
          {
            functionRecordType = item.RecordType;
          }
          else
          {
            TreeNode treeNode = treeNodeList.Find((Predicate<TreeNode>) (e => ((FunctionPrecompiled) e.Tag).Name == item.Name));
            if (treeNode == null)
            {
              if (functionRecordType == item.RecordType)
              {
                treeNode = n.Nodes.Add(item.Name, item.Name, 1, 1);
                treeNode.Tag = (object) item;
                treeNodeList.Add(treeNode);
              }
              else
              {
                treeNode = node.Nodes.Add(item.RecordType.ToString(), item.RecordType.ToString(), 1, 1);
                treeNode.Tag = (object) item;
                treeNodeList.Add(treeNode);
              }
            }
            else if (functionRecordType != item.RecordType)
            {
              treeNode = node.Nodes.Add(item.RecordType.ToString(), item.RecordType.ToString(), 1, 1);
              treeNode.Tag = (object) item;
              treeNodeList.Add(treeNode);
            }
            n = treeNode;
          }
        }
        if (item.RecordType == FunctionRecordType.Pointer)
          n.Nodes.Add(item.Name, item.Name, 3, 3).Tag = (object) item;
        else if (item.RecordType == FunctionRecordType.Event_Timeout)
        {
          string str = "Timeout -> " + item.Name;
          n.Nodes.Add(str, str, 4, 4).Tag = (object) item;
        }
        else if (item.RecordType == FunctionRecordType.Event_Hold)
        {
          string str = "Hold -> " + item.Name;
          n.Nodes.Add(str, str, 4, 4).Tag = (object) item;
        }
        else if (item.RecordType == FunctionRecordType.Event_None)
        {
          string str = "None";
          n.Nodes.Add(str, str, 4, 4).Tag = (object) item;
        }
        else if (item.RecordType == FunctionRecordType.Event_Click)
        {
          if (item.Name == "MAIN" || item.Name == "NEXT" || item.Name == "RIGHT" || this.IsFunctionAlreadyExistsByParent(n, item.Name))
          {
            string str = "Click -> " + item.Name;
            n.Nodes.Add(str, str, 4, 4).Tag = (object) item;
          }
          else
          {
            TreeNode treeNode1 = n.Nodes.Add("Click", "Click", 4, 4);
            treeNode1.Tag = (object) item;
            TreeNode treeNode2 = treeNode1.Nodes.Add(item.Name, item.Name, 1, 1);
            treeNode2.Tag = (object) item;
            treeNodeList.Add(treeNode2);
          }
        }
        else if (item.RecordType == FunctionRecordType.Event_Press)
        {
          if (item.Name == "MAIN" || item.Name == "NEXT" || item.Name == "RIGHT" || this.IsFunctionAlreadyExistsByParent(n, item.Name))
          {
            string str = "Press -> " + item.Name;
            n.Nodes.Add(str, str, 4, 4).Tag = (object) item;
          }
          else
          {
            TreeNode treeNode3 = n.Nodes.Add("Press", "Press", 4, 4);
            treeNode3.Tag = (object) item;
            TreeNode treeNode4 = treeNode3.Nodes.Add(item.Name, item.Name, 1, 1);
            treeNode4.Tag = (object) item;
            treeNodeList.Add(treeNode4);
          }
        }
      }
    }

    private bool IsFunctionAlreadyExistsByParent(TreeNode n, string functionName)
    {
      for (TreeNode parent = n.Parent; parent != null; parent = parent.Parent)
      {
        if (parent.Name == functionName)
          return true;
      }
      return false;
    }

    private void btnAddLayer_Click(object sender, EventArgs e)
    {
      S3_FunctionLayer s3FunctionLayer = this.MyMeter.MyFunctionManager.AddLayer();
      if (s3FunctionLayer == null)
        return;
      int layerNr = s3FunctionLayer.LayerNr;
      string str = "***** Layer " + layerNr.ToString() + " *****";
      TreeNodeCollection nodes = this.treeViewMenu.Nodes;
      layerNr = s3FunctionLayer.LayerNr;
      string key = layerNr.ToString();
      string text = str;
      nodes.Add(key, text, 0, 0);
    }

    private void btnTreeCollaps_Click(object sender, EventArgs e)
    {
      if (this.treeViewMenu.SelectedNode == null)
        return;
      this.treeViewMenu.SelectedNode.Collapse();
    }

    private void btnTreeExpand_Click(object sender, EventArgs e)
    {
      if (this.treeViewMenu.SelectedNode == null)
        return;
      this.treeViewMenu.SelectedNode.ExpandAll();
    }

    private void contextMenuActivFunction_Opening(object sender, CancelEventArgs e)
    {
      this.btnDeleteActivFunction.Enabled = false;
      this.setAsProtectedToolStripMenuItem.Enabled = false;
      this.btnTreeCollaps.Enabled = false;
      this.btnTreeExpand.Enabled = false;
      TreeNode selectedNode = this.treeViewMenu.SelectedNode;
      if (selectedNode == null)
        return;
      this.btnTreeCollaps.Enabled = true;
      this.btnTreeExpand.Enabled = true;
      this.btnDeleteActivFunction.Enabled = selectedNode.Parent != null && selectedNode.Parent.Name != "0";
      this.setAsProtectedToolStripMenuItem.Enabled = this.btnDeleteActivFunction.Enabled;
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      using (Bitmap bitmap = new Bitmap(this.treeViewMenu.Width, this.treeViewMenu.Height))
      {
        this.treeViewMenu.DrawToBitmap(bitmap, this.treeViewMenu.ClientRectangle);
        bitmap.Save("LCD.bmp");
        Process.Start("LCD.bmp");
      }
    }

    private void btnDeleteActivFunction_Click(object sender, EventArgs e)
    {
      this.DeleteSelectedActivFunction();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (!this.MyMeter.Compile())
      {
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (!this.MyFunctions.MyMeters.Undo())
        return;
      this.Close();
    }

    private void treeViewMenu_KeyUp(object sender, KeyEventArgs e)
    {
      this.btnDeleteActivFunction.Enabled = true;
      if (e.KeyCode != Keys.Delete)
        return;
      this.DeleteSelectedActivFunction();
    }

    private void btnSetAsProtected_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.treeViewMenu.SelectedNode;
      if (selectedNode == null || selectedNode.Parent != null && selectedNode.Parent.Name == "0" || !(selectedNode.Tag is Function tag))
        return;
      if (!this.MyMeter.MyFunctionManager.SetAsProtected((ushort) tag.FunctionNumber))
      {
        int num = (int) MessageBox.Show("Can not change selected function as protected. Please set firstly the previous function as protected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        selectedNode.ImageIndex = selectedNode.SelectedImageIndex = 2;
    }

    private void btnSetAsNotProtected_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.treeViewMenu.SelectedNode;
      if (selectedNode == null || selectedNode.Parent != null && selectedNode.Parent.Name == "0" || !(selectedNode.Tag is Function tag))
        return;
      if (!this.MyMeter.MyFunctionManager.SetAsNotProtected((ushort) tag.FunctionNumber))
      {
        int num = (int) MessageBox.Show("Can not change selected function as not protected. Please set firstly the next function as not protected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        selectedNode.ImageIndex = selectedNode.SelectedImageIndex = 1;
    }

    private void treeViewAvailableFunctions_ItemDrag(object sender, ItemDragEventArgs e)
    {
      if (!(((TreeNode) e.Item).Tag is Function))
        return;
      int num = (int) this.DoDragDrop(e.Item, DragDropEffects.Move);
    }

    private void treeViewMenu_DragOver(object sender, DragEventArgs e)
    {
      if (this.treeViewMenu.GetNodeAt(this.treeViewMenu.PointToClient(new Point(e.X, e.Y))) != null)
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void treeViewMenu_DragDrop(object sender, DragEventArgs e)
    {
      TreeNode nodeAt = this.treeViewMenu.GetNodeAt(this.treeViewMenu.PointToClient(new Point(e.X, e.Y)));
      TreeNode data = e.Data.GetData(typeof (TreeNode)) as TreeNode;
      if (data.Parent == null || nodeAt == null || nodeAt.Parent != null && nodeAt.Parent.Name == "0" || nodeAt.Name == "0")
        return;
      TreeNode node = (TreeNode) data.Clone();
      int int32;
      if (nodeAt.Tag is Function)
      {
        int num = nodeAt.Parent.Nodes.IndexOf(nodeAt);
        nodeAt.Parent.Nodes.Insert(num + 1, node);
        int32 = Convert.ToInt32(nodeAt.Parent.Name);
      }
      else
      {
        nodeAt.Nodes.Insert(0, node);
        int32 = Convert.ToInt32(nodeAt.Name);
      }
      Function tag = node.Tag as Function;
      int pos = node.Parent.Nodes.IndexOf(node);
      S3_Function s3Function = this.MyMeter.MyFunctionManager.AddFunction(int32, tag, pos);
      if (s3Function == null)
      {
        node.Parent.Nodes.Remove(node);
      }
      else
      {
        data.BackColor = this.USED_FUNCTION_COLOR;
        if (s3Function.IsProtected)
        {
          node.ImageIndex = 2;
          node.SelectedImageIndex = 2;
        }
        else
        {
          node.ImageIndex = 1;
          node.SelectedImageIndex = 1;
        }
      }
      this.treeViewMenu.SelectedNode = node;
      this.treeViewMenu.BringToFront();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void treeViewAvailableFunctions_Leave(object sender, EventArgs e)
    {
      this.treeViewAvailableFunctions.SelectedNode = (TreeNode) null;
    }

    private void treeViewMenu_Leave(object sender, EventArgs e)
    {
      this.treeViewMenu.SelectedNode = (TreeNode) null;
    }

    private void tree_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.txtFunctionNumber.Text = string.Empty;
      this.txtFunctionVersion.Text = string.Empty;
      this.listNeedResources.DataSource = (object) null;
      if (!(e.Node.Tag is Function tag))
        return;
      this.txtFunctionNumber.Text = tag.FunctionNumber.ToString();
      this.txtFunctionVersion.Text = tag.FunctionVersion.ToString();
      this.textBoxFirmwareRange.Text = ParameterService.GetVersionNumberString((long) tag.FirmwareVersionMin) + "  to  " + ParameterService.GetVersionNumberString((long) tag.FirmwareVersionMax);
      this.listNeedResources.DataSource = (object) tag.NeedResources;
    }

    private void DeleteSelectedActivFunction()
    {
      TreeNode selectedNode = this.treeViewMenu.SelectedNode;
      if (selectedNode == null || selectedNode.Parent != null && selectedNode.Parent.Name == "0" || !(selectedNode.Tag is Function))
        return;
      Function tag = selectedNode.Tag as Function;
      if (!this.MyMeter.MyFunctionManager.RemoveFunction((ushort) tag.FunctionNumber))
        return;
      TreeNode functionNode = this.FindFunctionNode(this.treeViewAvailableFunctions.Nodes, tag.FunctionNumber);
      if (functionNode != null)
        functionNode.BackColor = Color.Empty;
      if (selectedNode.Parent.Nodes.Count == 1)
      {
        selectedNode.Parent.Nodes.Remove(selectedNode);
        this.CreateUsedFunctionsTree();
      }
      else
        selectedNode.Parent.Nodes.Remove(selectedNode);
    }

    private void buttonGetText_Click(object sender, EventArgs e)
    {
      StringBuilder theText = new StringBuilder();
      DateTime now = DateTime.Now;
      string str = "LCD_Menu";
      theText.AppendLine(str + "  " + now.ToLongDateString() + " " + now.ToLongTimeString());
      theText.AppendLine();
      theText.AppendLine("****************************************************************");
      theText.AppendLine("Defined menu");
      theText.AppendLine();
      this.GetMenuNodesText(theText, "", this.treeViewMenu.Nodes);
      theText.AppendLine();
      theText.AppendLine("****************************************************************");
      theText.AppendLine("Unused functions");
      theText.AppendLine();
      this.GetUnusedFunctionsText(theText, "", this.treeViewAvailableFunctions.Nodes);
      string path = Path.Combine(this.MyFunctions.PrintConfigPath, now.ToString("yyMMddHHmmss") + "_" + str + ".txt");
      using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
      {
        streamWriter.Write(theText.ToString());
        streamWriter.Flush();
        streamWriter.Close();
      }
      new Process() { StartInfo = { FileName = path } }.Start();
    }

    private void GetMenuNodesText(
      StringBuilder theText,
      string lineStartString,
      TreeNodeCollection theNodes)
    {
      foreach (TreeNode theNode in theNodes)
      {
        theText.AppendLine(lineStartString + theNode.Text);
        if (theNode.IsExpanded && theNode.Nodes != null && theNode.Nodes.Count > 0)
          this.GetMenuNodesText(theText, lineStartString + "   ", theNode.Nodes);
      }
    }

    private void GetUnusedFunctionsText(
      StringBuilder theText,
      string lineStartString,
      TreeNodeCollection theNodes)
    {
      foreach (TreeNode theNode in theNodes)
      {
        Color color = theNode.BackColor;
        string str1 = color.ToString();
        color = this.USED_FUNCTION_COLOR;
        string str2 = color.ToString();
        if (str1 != str2)
        {
          theText.AppendLine(lineStartString + theNode.Text);
          if (theNode.IsExpanded && theNode.Nodes != null && theNode.Nodes.Count > 0)
            this.GetUnusedFunctionsText(theText, lineStartString + "   ", theNode.Nodes);
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (S3_LCD_Menu));
      this.splitContainer1 = new SplitContainer();
      this.treeViewMenu = new TreeView();
      this.contextMenuActivFunction = new ContextMenuStrip(this.components);
      this.btnAddLayer = new ToolStripMenuItem();
      this.btnDeleteActivFunction = new ToolStripMenuItem();
      this.setAsProtectedToolStripMenuItem = new ToolStripMenuItem();
      this.btnSetAsProtected = new ToolStripMenuItem();
      this.btnSetAsNotProtected = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.btnTreeCollaps = new ToolStripMenuItem();
      this.btnTreeExpand = new ToolStripMenuItem();
      this.splitContainer3 = new SplitContainer();
      this.treeViewAvailableFunctions = new TreeView();
      this.groupBox1 = new GroupBox();
      this.listNeedResources = new ListBox();
      this.label2 = new Label();
      this.textBoxFirmwareRange = new TextBox();
      this.label3 = new Label();
      this.txtFunctionVersion = new TextBox();
      this.label1 = new Label();
      this.txtFunctionNumber = new TextBox();
      this.label4 = new Label();
      this.btnPrint = new Button();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.lblOldFunction = new Label();
      this.buttonGetText = new Button();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.contextMenuActivFunction.SuspendLayout();
      this.splitContainer3.BeginInit();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.Panel2.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(0, 62);
      this.splitContainer1.Margin = new Padding(4, 5, 4, 5);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.treeViewMenu);
      this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer3);
      this.splitContainer1.Size = new Size(1662, 914);
      this.splitContainer1.SplitterDistance = 1186;
      this.splitContainer1.SplitterWidth = 6;
      this.splitContainer1.TabIndex = 20;
      this.treeViewMenu.AllowDrop = true;
      this.treeViewMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.treeViewMenu.ContextMenuStrip = this.contextMenuActivFunction;
      this.treeViewMenu.HideSelection = false;
      this.treeViewMenu.Location = new Point(4, 5);
      this.treeViewMenu.Margin = new Padding(4, 5, 4, 5);
      this.treeViewMenu.Name = "treeViewMenu";
      this.treeViewMenu.Size = new Size(1176, 901);
      this.treeViewMenu.TabIndex = 1;
      this.treeViewMenu.AfterSelect += new TreeViewEventHandler(this.tree_AfterSelect);
      this.treeViewMenu.DragDrop += new DragEventHandler(this.treeViewMenu_DragDrop);
      this.treeViewMenu.DragOver += new DragEventHandler(this.treeViewMenu_DragOver);
      this.treeViewMenu.KeyUp += new KeyEventHandler(this.treeViewMenu_KeyUp);
      this.treeViewMenu.Leave += new System.EventHandler(this.treeViewMenu_Leave);
      this.contextMenuActivFunction.ImageScalingSize = new Size(24, 24);
      this.contextMenuActivFunction.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.btnAddLayer,
        (ToolStripItem) this.btnDeleteActivFunction,
        (ToolStripItem) this.setAsProtectedToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.btnTreeCollaps,
        (ToolStripItem) this.btnTreeExpand
      });
      this.contextMenuActivFunction.Name = "contextMenuActivFunction";
      this.contextMenuActivFunction.Size = new Size(161, 170);
      this.contextMenuActivFunction.Opening += new CancelEventHandler(this.contextMenuActivFunction_Opening);
      this.btnAddLayer.Name = "btnAddLayer";
      this.btnAddLayer.Size = new Size(160, 32);
      this.btnAddLayer.Text = "Add layer";
      this.btnAddLayer.Click += new System.EventHandler(this.btnAddLayer_Click);
      this.btnDeleteActivFunction.Name = "btnDeleteActivFunction";
      this.btnDeleteActivFunction.Size = new Size(160, 32);
      this.btnDeleteActivFunction.Text = "Delete";
      this.btnDeleteActivFunction.Click += new System.EventHandler(this.btnDeleteActivFunction_Click);
      this.setAsProtectedToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.btnSetAsProtected,
        (ToolStripItem) this.btnSetAsNotProtected
      });
      this.setAsProtectedToolStripMenuItem.Name = "setAsProtectedToolStripMenuItem";
      this.setAsProtectedToolStripMenuItem.Size = new Size(160, 32);
      this.setAsProtectedToolStripMenuItem.Text = "Protected";
      this.btnSetAsProtected.Name = "btnSetAsProtected";
      this.btnSetAsProtected.Size = new Size(139, 34);
      this.btnSetAsProtected.Text = "Yes";
      this.btnSetAsProtected.Click += new System.EventHandler(this.btnSetAsProtected_Click);
      this.btnSetAsNotProtected.Name = "btnSetAsNotProtected";
      this.btnSetAsNotProtected.Size = new Size(139, 34);
      this.btnSetAsNotProtected.Text = "No";
      this.btnSetAsNotProtected.Click += new System.EventHandler(this.btnSetAsNotProtected_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(157, 6);
      this.btnTreeCollaps.Name = "btnTreeCollaps";
      this.btnTreeCollaps.Size = new Size(160, 32);
      this.btnTreeCollaps.Text = "Collaps";
      this.btnTreeCollaps.Click += new System.EventHandler(this.btnTreeCollaps_Click);
      this.btnTreeExpand.Name = "btnTreeExpand";
      this.btnTreeExpand.Size = new Size(160, 32);
      this.btnTreeExpand.Text = "Expand";
      this.btnTreeExpand.Click += new System.EventHandler(this.btnTreeExpand_Click);
      this.splitContainer3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer3.Location = new Point(4, 5);
      this.splitContainer3.Margin = new Padding(4, 5, 4, 5);
      this.splitContainer3.Name = "splitContainer3";
      this.splitContainer3.Orientation = Orientation.Horizontal;
      this.splitContainer3.Panel1.Controls.Add((Control) this.treeViewAvailableFunctions);
      this.splitContainer3.Panel2.Controls.Add((Control) this.groupBox1);
      this.splitContainer3.Size = new Size(460, 903);
      this.splitContainer3.SplitterDistance = 589;
      this.splitContainer3.SplitterWidth = 6;
      this.splitContainer3.TabIndex = 2;
      this.treeViewAvailableFunctions.AllowDrop = true;
      this.treeViewAvailableFunctions.Dock = DockStyle.Fill;
      this.treeViewAvailableFunctions.Location = new Point(0, 0);
      this.treeViewAvailableFunctions.Margin = new Padding(4, 5, 4, 5);
      this.treeViewAvailableFunctions.Name = "treeViewAvailableFunctions";
      this.treeViewAvailableFunctions.Size = new Size(460, 589);
      this.treeViewAvailableFunctions.TabIndex = 1;
      this.treeViewAvailableFunctions.ItemDrag += new ItemDragEventHandler(this.treeViewAvailableFunctions_ItemDrag);
      this.treeViewAvailableFunctions.AfterSelect += new TreeViewEventHandler(this.tree_AfterSelect);
      this.treeViewAvailableFunctions.Leave += new System.EventHandler(this.treeViewAvailableFunctions_Leave);
      this.groupBox1.Controls.Add((Control) this.listNeedResources);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.textBoxFirmwareRange);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.txtFunctionVersion);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.txtFunctionNumber);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Dock = DockStyle.Fill;
      this.groupBox1.Location = new Point(0, 0);
      this.groupBox1.Margin = new Padding(4, 5, 4, 5);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4, 5, 4, 5);
      this.groupBox1.Size = new Size(460, 308);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Function info";
      this.listNeedResources.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listNeedResources.FormattingEnabled = true;
      this.listNeedResources.ItemHeight = 20;
      this.listNeedResources.Location = new Point(174, 134);
      this.listNeedResources.Margin = new Padding(4, 5, 4, 5);
      this.listNeedResources.Name = "listNeedResources";
      this.listNeedResources.Size = new Size(282, 164);
      this.listNeedResources.TabIndex = 5;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(36, 134);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(133, 20);
      this.label2.TabIndex = 4;
      this.label2.Text = "Need ressources:";
      this.textBoxFirmwareRange.BorderStyle = BorderStyle.None;
      this.textBoxFirmwareRange.Location = new Point(174, 94);
      this.textBoxFirmwareRange.Margin = new Padding(4, 5, 4, 5);
      this.textBoxFirmwareRange.Name = "textBoxFirmwareRange";
      this.textBoxFirmwareRange.ReadOnly = true;
      this.textBoxFirmwareRange.Size = new Size(272, 19);
      this.textBoxFirmwareRange.TabIndex = 3;
      this.textBoxFirmwareRange.Text = "-";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(34, 94);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(123, 20);
      this.label3.TabIndex = 2;
      this.label3.Text = "Firmware range:";
      this.txtFunctionVersion.BorderStyle = BorderStyle.None;
      this.txtFunctionVersion.Location = new Point(174, 62);
      this.txtFunctionVersion.Margin = new Padding(4, 5, 4, 5);
      this.txtFunctionVersion.Name = "txtFunctionVersion";
      this.txtFunctionVersion.ReadOnly = true;
      this.txtFunctionVersion.Size = new Size(272, 19);
      this.txtFunctionVersion.TabIndex = 3;
      this.txtFunctionVersion.Text = "-";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(34, 62);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(129, 20);
      this.label1.TabIndex = 2;
      this.label1.Text = "Function version:";
      this.txtFunctionNumber.BorderStyle = BorderStyle.None;
      this.txtFunctionNumber.Location = new Point(174, 32);
      this.txtFunctionNumber.Margin = new Padding(4, 5, 4, 5);
      this.txtFunctionNumber.Name = "txtFunctionNumber";
      this.txtFunctionNumber.ReadOnly = true;
      this.txtFunctionNumber.Size = new Size(272, 19);
      this.txtFunctionNumber.TabIndex = 1;
      this.txtFunctionNumber.Text = "-";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(34, 31);
      this.label4.Margin = new Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(69, 20);
      this.label4.TabIndex = 0;
      this.label4.Text = "Number:";
      this.btnPrint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnPrint.Image = (Image) componentResourceManager.GetObject("btnPrint.Image");
      this.btnPrint.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPrint.Location = new Point(6, 978);
      this.btnPrint.Margin = new Padding(4, 5, 4, 5);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(99, 45);
      this.btnPrint.TabIndex = 26;
      this.btnPrint.Text = "Print";
      this.btnPrint.TextAlign = ContentAlignment.MiddleRight;
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Image = (Image) componentResourceManager.GetObject("btnOk.Image");
      this.btnOk.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOk.ImeMode = ImeMode.NoControl;
      this.btnOk.Location = new Point(1377, 988);
      this.btnOk.Margin = new Padding(4, 5, 4, 5);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(129, 37);
      this.btnOk.TabIndex = 22;
      this.btnOk.Text = "Save";
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(1515, 988);
      this.btnCancel.Margin = new Padding(4, 5, 4, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(129, 37);
      this.btnCancel.TabIndex = 21;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1662, 63);
      this.zennerCoroprateDesign2.TabIndex = 19;
      this.lblOldFunction.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblOldFunction.AutoSize = true;
      this.lblOldFunction.BackColor = Color.Pink;
      this.lblOldFunction.Location = new Point(212, 988);
      this.lblOldFunction.Margin = new Padding(4, 0, 4, 0);
      this.lblOldFunction.Name = "lblOldFunction";
      this.lblOldFunction.Size = new Size(97, 20);
      this.lblOldFunction.TabIndex = 27;
      this.lblOldFunction.Text = "*old function";
      this.buttonGetText.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonGetText.Location = new Point(1256, 988);
      this.buttonGetText.Margin = new Padding(4, 5, 4, 5);
      this.buttonGetText.Name = "buttonGetText";
      this.buttonGetText.Size = new Size(112, 35);
      this.buttonGetText.TabIndex = 28;
      this.buttonGetText.Text = "Get text";
      this.buttonGetText.UseVisualStyleBackColor = true;
      this.buttonGetText.Click += new System.EventHandler(this.buttonGetText_Click);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1662, 1029);
      this.Controls.Add((Control) this.buttonGetText);
      this.Controls.Add((Control) this.lblOldFunction);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (S3_LCD_Menu);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (S3_LCD_Menu);
      this.Load += new System.EventHandler(this.S3_LCD_Menu_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.contextMenuActivFunction.ResumeLayout(false);
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.Panel2.ResumeLayout(false);
      this.splitContainer3.EndInit();
      this.splitContainer3.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
