// Decompiled with JetBrains decompiler
// Type: EDC_Handler.MbusListEditor
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Properties;

#nullable disable
namespace EDC_Handler
{
  public class MbusListEditor : Form
  {
    private EDC_Meter tempWorkMeter;
    private EDC_Meter tempTypeMeter;
    private EDC_Meter tempBackupMeter;
    private EDC_Meter tempConnectedMeter;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private ToolTip toolTip;
    private Label label36;
    private ComboBox cboxHandlerObject;
    private Panel panel;
    private TreeView tree;

    public MbusListEditor()
    {
      this.InitializeComponent();
      this.tree.ImageList = new ImageList()
      {
        Images = {
          (Image) Resources.FolderNode,
          (Image) Resources.Property,
          (Image) Resources.BottomButton
        }
      };
    }

    private void MbusListEditor_Load(object sender, EventArgs e)
    {
      this.cboxHandlerObject.DataSource = (object) Util.GetNamesOfEnum(typeof (HandlerMeterType));
      this.cboxHandlerObject.SelectedItem = (object) HandlerMeterType.WorkMeter;
    }

    internal static void ShowDialog(Form owner, EDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (MbusListEditor mbusListEditor = new MbusListEditor())
      {
        if (MyFunctions.WorkMeter != null)
          mbusListEditor.tempWorkMeter = MyFunctions.WorkMeter.DeepCopy();
        if (MyFunctions.TypeMeter != null)
          mbusListEditor.tempTypeMeter = MyFunctions.TypeMeter.DeepCopy();
        if (MyFunctions.BackupMeter != null)
          mbusListEditor.tempBackupMeter = MyFunctions.BackupMeter.DeepCopy();
        if (MyFunctions.ConnectedMeter != null)
          mbusListEditor.tempConnectedMeter = MyFunctions.ConnectedMeter.DeepCopy();
        if (mbusListEditor.ShowDialog((IWin32Window) owner) != DialogResult.OK)
          return;
        MyFunctions.WorkMeter = mbusListEditor.tempWorkMeter;
        MyFunctions.TypeMeter = mbusListEditor.tempTypeMeter;
        MyFunctions.BackupMeter = mbusListEditor.tempBackupMeter;
        MyFunctions.ConnectedMeter = mbusListEditor.tempConnectedMeter;
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void cboxHandlerObject_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.LoadList();
    }

    private EDC_Meter GetHandlerMeter()
    {
      EDC_Meter handlerMeter;
      switch ((HandlerMeterType) Enum.Parse(typeof (HandlerMeterType), this.cboxHandlerObject.SelectedItem.ToString()))
      {
        case HandlerMeterType.WorkMeter:
          handlerMeter = this.tempWorkMeter;
          break;
        case HandlerMeterType.TypeMeter:
          handlerMeter = this.tempTypeMeter;
          break;
        case HandlerMeterType.BackupMeter:
          handlerMeter = this.tempBackupMeter;
          break;
        case HandlerMeterType.ConnectedMeter:
          handlerMeter = this.tempConnectedMeter;
          break;
        default:
          throw new NotImplementedException();
      }
      return handlerMeter;
    }

    private void LoadList()
    {
      this.tree.Nodes.Clear();
      EDC_Meter handlerMeter = this.GetHandlerMeter();
      this.panel.Visible = handlerMeter != null;
      if (handlerMeter == null || handlerMeter.Version.Type == EDC_Hardware.EDC_mBus)
        return;
      MBusListStructure mbusListStructure = handlerMeter.GetMBusListStructure();
      if (mbusListStructure == null)
        return;
      TreeNode node1 = new TreeNode("List pointer table");
      if (mbusListStructure.SelectedTransmitList != null)
        node1.Nodes.Add(new TreeNode(string.Format("Selected transmit list: 0x{0:X4}, {1}", (object) mbusListStructure.SelectedTransmitList.StartAddress, (object) mbusListStructure.GetNameOfSelectedTransmitList()), 2, 2));
      if (mbusListStructure.SelectedInstallList != null)
        node1.Nodes.Add(new TreeNode(string.Format("Selected install list: 0x{0:X4}", (object) mbusListStructure.SelectedInstallList.StartAddress), 2, 2));
      this.tree.Nodes.Add(node1);
      node1.Expand();
      TreeNode node2 = new TreeNode("Lists");
      int num = 65;
      foreach (MBusList list in mbusListStructure.Lists)
      {
        TreeNode node3;
        if (mbusListStructure.Lists.IndexOf(list) == 0)
        {
          node3 = new TreeNode(string.Format("List install, 0x{0:X4}", (object) list.StartAddress));
        }
        else
        {
          node3 = new TreeNode(string.Format("LIST_{0}, 0x{1:X4}", (object) (char) num, (object) list.StartAddress));
          ++num;
        }
        node3.Tag = (object) list;
        foreach (MBusParameter parameter in list.Parameters)
        {
          TreeNode node4 = new TreeNode((list.Parameters.IndexOf(parameter) + 1).ToString(), 1, 1);
          node4.Tag = (object) parameter;
          node4.Nodes.Add(new TreeNode(string.Format("Start address: 0x{0:X4}", (object) parameter.StartAddress), 2, 2));
          if (parameter.Control0.ControlLogger != 0)
            node4.Nodes.Add(new TreeNode(string.Format("Param log: {0}", (object) parameter.Control0.ControlLogger), 2, 2));
          node4.Nodes.Add(new TreeNode(string.Format("Byte count: {0}", (object) parameter.Control0.ByteCount), 2, 2));
          node4.Nodes.Add(new TreeNode(string.Format("VIF DIF count: {0}", (object) parameter.Control0.VifDifCount), 2, 2));
          node4.Nodes.Add(new TreeNode(string.Format("Param code: {0}", (object) parameter.Control0.ParamCode), 2, 2));
          if (parameter.VifDif != null && parameter.VifDif.Count > 0)
            node4.Nodes.Add(new TreeNode(string.Format("VIF DIF: {0}", (object) Util.ByteArrayToHexString(parameter.VifDif.ToArray())), 2, 2));
          if (parameter.Control0.ControlLogger == ControlLogger.LOG_HISTORY_INDEX)
            node4.Nodes.Add(new TreeNode(string.Format("Log history index: {0}", (object) parameter.LogHistoryIndex), 2, 2));
          if (parameter.Pointer != null)
            node4.Nodes.Add(new TreeNode(string.Format("Pointer: {0} Address: 0x{1:X4}", (object) parameter.Pointer.Name, (object) parameter.Pointer.Address), 2, 2));
          node3.Nodes.Add(node4);
        }
        node2.Nodes.Add(node3);
      }
      this.tree.Nodes.Add(node2);
      node2.Expand();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MbusListEditor));
      this.toolTip = new ToolTip(this.components);
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.label36 = new Label();
      this.cboxHandlerObject = new ComboBox();
      this.panel = new Panel();
      this.tree = new TreeView();
      this.panel.SuspendLayout();
      this.SuspendLayout();
      this.toolTip.AutoPopDelay = 10000;
      this.toolTip.InitialDelay = 500;
      this.toolTip.ReshowDelay = 100;
      this.toolTip.ShowAlways = true;
      this.zennerCoroprateDesign1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(696, 36);
      this.zennerCoroprateDesign1.TabIndex = 20;
      this.label36.BackColor = Color.White;
      this.label36.Location = new Point(203, 7);
      this.label36.Name = "label36";
      this.label36.Size = new Size(84, 15);
      this.label36.TabIndex = 51;
      this.label36.Text = "Handler object:";
      this.label36.TextAlign = ContentAlignment.MiddleRight;
      this.cboxHandlerObject.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxHandlerObject.FormattingEnabled = true;
      this.cboxHandlerObject.Location = new Point(295, 5);
      this.cboxHandlerObject.Name = "cboxHandlerObject";
      this.cboxHandlerObject.Size = new Size(132, 21);
      this.cboxHandlerObject.TabIndex = 50;
      this.cboxHandlerObject.SelectedIndexChanged += new System.EventHandler(this.cboxHandlerObject_SelectedIndexChanged);
      this.panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel.Controls.Add((Control) this.tree);
      this.panel.Location = new Point(0, 36);
      this.panel.Name = "panel";
      this.panel.Size = new Size(696, 516);
      this.panel.TabIndex = 52;
      this.tree.AllowDrop = true;
      this.tree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tree.HideSelection = false;
      this.tree.Location = new Point(3, 3);
      this.tree.Name = "tree";
      this.tree.Size = new Size(690, 510);
      this.tree.TabIndex = 20;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(696, 551);
      this.Controls.Add((Control) this.panel);
      this.Controls.Add((Control) this.label36);
      this.Controls.Add((Control) this.cboxHandlerObject);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MbusListEditor);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "M-Bus list editor";
      this.Load += new System.EventHandler(this.MbusListEditor_Load);
      this.panel.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
