// Decompiled with JetBrains decompiler
// Type: PDC_Handler.TypeEditor
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using CorporateDesign;
using StartupLib;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public class TypeEditor : Form
  {
    private PDC_HandlerFunctions MyFunctions;
    private PDC_Meter selectedMeter;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Button btnNew;
    private DataGridView tableTypes;
    private Button btnDelete;
    private Button btnLoad;
    private Button btnEdit;
    private GroupBox groupBox3;
    private TextBox txtInfo;

    public TypeEditor() => this.InitializeComponent();

    internal static void ShowDialog(Form owner, PDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (TypeEditor typeEditor = new TypeEditor())
      {
        typeEditor.MyFunctions = MyFunctions;
        int num = (int) typeEditor.ShowDialog((IWin32Window) owner);
      }
    }

    private void TypeEditor_Load(object sender, EventArgs e) => this.InitializeControls();

    private void tableTypes_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.btnLoad_Click(sender, (EventArgs) e);
    }

    private void tableTypes_SelectionChanged(object sender, EventArgs e)
    {
      if (this.tableTypes.SelectedRows.Count != 1 || this.tableTypes.SelectedRows[0].DataBoundItem == null)
        return;
      if (!(this.tableTypes.SelectedRows[0].DataBoundItem is MeterInfo dataBoundItem))
        return;
      try
      {
        this.selectedMeter = TypeEditor.GetMeterObject(dataBoundItem.MeterInfoID);
        this.txtInfo.Text = this.selectedMeter != null ? this.selectedMeter.ToString() : string.Empty;
      }
      catch (Exception ex)
      {
        this.txtInfo.Text = ex.Message;
      }
      if (dataBoundItem.PPSArtikelNr == "PDC_BASETYPE")
        this.btnDelete.Enabled = UserManager.CheckPermission("Developer");
      else
        this.btnDelete.Enabled = true;
    }

    private void btnLoad_Click(object sender, EventArgs e)
    {
      MeterInfo selectedType = this.GetSelectedType();
      if (selectedType == null)
        return;
      try
      {
        if (!this.MyFunctions.OpenType(selectedType.MeterInfoID))
          return;
        if (this.MyFunctions.TypeMeter != null && this.MyFunctions.WorkMeter == null)
          this.MyFunctions.WorkMeter = this.MyFunctions.TypeMeter.DeepCopy();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Load type error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.Close();
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      TypeDataEditor.ShowDialog((Form) this, PDC_DeviceIdentity.PDC_WmBus, this.MyFunctions);
      this.InitializeControls();
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      MeterInfo selectedType = this.GetSelectedType();
      if (selectedType == null)
        return;
      TypeDataEditor.ShowDialog((Form) this, PDC_DeviceIdentity.PDC_WmBus, this.MyFunctions, this.selectedMeter, selectedType);
      this.InitializeControls();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      MeterInfo selectedType = this.GetSelectedType();
      if (selectedType == null || MessageBox.Show("Are you sure to delete the type?", "Delete?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
        return;
      PDC_Database.DeleteType(selectedType.MeterInfoID);
      this.InitializeControls();
    }

    private void cboxEdcHardware_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.InitializeControls();
    }

    private MeterInfo GetSelectedType()
    {
      return this.tableTypes.SelectedRows == null || this.tableTypes.SelectedRows.Count != 1 ? (MeterInfo) null : this.tableTypes.SelectedRows[0].DataBoundItem as MeterInfo;
    }

    private void InitializeControls()
    {
      this.txtInfo.Text = string.Empty;
      this.tableTypes.DataSource = (object) MeterDatabase.LoadMeterInfoByHardwareName(PDC_DeviceIdentity.PDC_WmBus.ToString());
      if (this.MyFunctions.TypeMeter == null)
        return;
      DeviceIdentification deviceIdentification = this.MyFunctions.WorkMeter.GetDeviceIdentification();
      if (deviceIdentification != null && deviceIdentification.IsChecksumOK)
      {
        uint meterInfoId = deviceIdentification.MeterInfoID;
        foreach (DataGridViewRow row in (IEnumerable) this.tableTypes.Rows)
        {
          if ((long) (row.DataBoundItem as MeterInfo).MeterInfoID == (long) meterInfoId)
          {
            row.Selected = true;
            break;
          }
        }
      }
    }

    private static PDC_Meter GetMeterObject(int meterInfoID)
    {
      MeterTypeData meterTypeData = PDC_Database.LoadType(meterInfoID);
      return meterTypeData == null ? (PDC_Meter) null : PDC_Meter.Unzip(meterTypeData.EEPdata);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TypeEditor));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.btnNew = new Button();
      this.tableTypes = new DataGridView();
      this.btnDelete = new Button();
      this.btnLoad = new Button();
      this.btnEdit = new Button();
      this.groupBox3 = new GroupBox();
      this.txtInfo = new TextBox();
      ((ISupportInitialize) this.tableTypes).BeginInit();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(784, 42);
      this.zennerCoroprateDesign2.TabIndex = 26;
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.Image = (Image) componentResourceManager.GetObject("btnNew.Image");
      this.btnNew.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnNew.ImeMode = ImeMode.NoControl;
      this.btnNew.Location = new Point(683, 47);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(92, 29);
      this.btnNew.TabIndex = 57;
      this.btnNew.Text = "New";
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      this.tableTypes.AllowUserToAddRows = false;
      this.tableTypes.AllowUserToDeleteRows = false;
      this.tableTypes.AllowUserToResizeRows = false;
      this.tableTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.tableTypes.BackgroundColor = Color.White;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.tableTypes.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.tableTypes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.tableTypes.DefaultCellStyle = gridViewCellStyle2;
      this.tableTypes.Location = new Point(12, 47);
      this.tableTypes.MultiSelect = false;
      this.tableTypes.Name = "tableTypes";
      this.tableTypes.ReadOnly = true;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.tableTypes.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.tableTypes.RowHeadersVisible = false;
      this.tableTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.tableTypes.Size = new Size(662, 284);
      this.tableTypes.TabIndex = 58;
      this.tableTypes.SelectionChanged += new System.EventHandler(this.tableTypes_SelectionChanged);
      this.tableTypes.MouseDoubleClick += new MouseEventHandler(this.tableTypes_MouseDoubleClick);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.Image = (Image) componentResourceManager.GetObject("btnDelete.Image");
      this.btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDelete.ImeMode = ImeMode.NoControl;
      this.btnDelete.Location = new Point(683, 117);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(92, 29);
      this.btnDelete.TabIndex = 61;
      this.btnDelete.Text = "Delete";
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      this.btnLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnLoad.Image = (Image) componentResourceManager.GetObject("btnLoad.Image");
      this.btnLoad.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnLoad.ImeMode = ImeMode.NoControl;
      this.btnLoad.Location = new Point(683, 521);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new Size(92, 29);
      this.btnLoad.TabIndex = 64;
      this.btnLoad.Text = "Load";
      this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.Image = (Image) componentResourceManager.GetObject("btnEdit.Image");
      this.btnEdit.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnEdit.ImeMode = ImeMode.NoControl;
      this.btnEdit.Location = new Point(683, 82);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(92, 29);
      this.btnEdit.TabIndex = 67;
      this.btnEdit.Text = "Edit";
      this.btnEdit.UseVisualStyleBackColor = true;
      this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
      this.groupBox3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox3.Controls.Add((Control) this.txtInfo);
      this.groupBox3.Location = new Point(7, 337);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(670, 213);
      this.groupBox3.TabIndex = 68;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Meter object data";
      this.txtInfo.Dock = DockStyle.Fill;
      this.txtInfo.Font = new Font("Consolas", 8.25f);
      this.txtInfo.Location = new Point(3, 16);
      this.txtInfo.Multiline = true;
      this.txtInfo.Name = "txtInfo";
      this.txtInfo.ScrollBars = ScrollBars.Vertical;
      this.txtInfo.Size = new Size(664, 194);
      this.txtInfo.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(784, 562);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.btnEdit);
      this.Controls.Add((Control) this.btnLoad);
      this.Controls.Add((Control) this.btnDelete);
      this.Controls.Add((Control) this.tableTypes);
      this.Controls.Add((Control) this.btnNew);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (TypeEditor);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Type editor";
      this.Load += new System.EventHandler(this.TypeEditor_Load);
      ((ISupportInitialize) this.tableTypes).EndInit();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
