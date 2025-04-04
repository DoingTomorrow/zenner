// Decompiled with JetBrains decompiler
// Type: EDC_Handler.ParameterEditor
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using CorporateDesign;
using GmmDbLib;
using StartupLib;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public class ParameterEditor : Form
  {
    private EDC_HandlerFunctions MyFunctions;
    private bool initialized = false;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private DataGridView tableParameter;
    private TextBox txtDescription;
    private SplitContainer splitContainer1;
    private Button btnSave;
    private Button btnCancel;
    internal Button btnPrint;
    private Label label36;
    private ComboBox cboxHandlerObject;
    private Panel panel;

    internal ParameterEditor() => this.InitializeComponent();

    private void ParameterEditor_Load(object sender, EventArgs e)
    {
      this.cboxHandlerObject.DataSource = (object) Util.GetNamesOfEnum(typeof (HandlerMeterType));
      this.cboxHandlerObject.SelectedItem = (object) HandlerMeterType.WorkMeter;
      if (!this.initialized)
        this.initialized = true;
      this.LoadParameter();
      this.btnSave.Enabled = UserManager.CheckPermission("EDC_Handler.View.Expert");
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.tableParameter.Tag is EDC_Meter tag)
      {
        switch ((HandlerMeterType) Enum.Parse(typeof (HandlerMeterType), this.cboxHandlerObject.SelectedItem.ToString()))
        {
          case HandlerMeterType.WorkMeter:
            this.MyFunctions.WorkMeter = tag;
            break;
          case HandlerMeterType.TypeMeter:
            this.MyFunctions.TypeMeter = tag;
            break;
          case HandlerMeterType.BackupMeter:
            this.MyFunctions.BackupMeter = tag;
            break;
          case HandlerMeterType.ConnectedMeter:
            this.MyFunctions.ConnectedMeter = tag;
            break;
          default:
            throw new NotImplementedException();
        }
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnPrintTable_Click(object sender, EventArgs e)
    {
      if (!(this.tableParameter.Tag is EDC_Meter tag))
        return;
      PrintDataGridView.Print(this.tableParameter, tag.Version.VersionString + " Serialnumber: " + tag.GetSerialnumberFull());
    }

    private void dataGridViewParameter_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (!(this.tableParameter.Tag is EDC_Meter tag) || e.ColumnIndex != 6 && e.ColumnIndex != 7)
        return;
      this.tableParameter[e.ColumnIndex, e.RowIndex].ErrorText = string.Empty;
      try
      {
        ushort uint16 = Convert.ToUInt16(this.tableParameter[5, e.RowIndex].Value.ToString(), 16);
        int int32 = Convert.ToInt32(this.tableParameter[3, e.RowIndex].Value);
        string hexString = Util.ByteArrayToHexString(tag.Map.GetMemoryBytes(uint16, int32));
        string hex = this.tableParameter[6, e.RowIndex].Value.ToString();
        if (hexString.ToUpper() == hex.ToUpper())
        {
          this.tableParameter.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
        }
        else
        {
          this.tableParameter.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
          this.btnSave.Enabled = true;
        }
        byte[] byteArray = Util.HexStringToByteArray(hex);
        tag.Map.SetMemoryBytes(uint16, byteArray);
      }
      catch (Exception ex)
      {
        this.tableParameter[e.ColumnIndex, e.RowIndex].ErrorText = "Invalid value! Reason: " + ex.Message;
      }
    }

    private void tableParameter_SelectionChanged(object sender, EventArgs e)
    {
      if (this.tableParameter.CurrentRow == null)
        return;
      this.ShowParameterInfo(this.tableParameter.CurrentRow.Index);
    }

    private void tableParameter_Sorted(object sender, EventArgs e)
    {
      if (!(this.tableParameter.Tag is EDC_Meter tag))
        return;
      foreach (DataGridViewRow row in (IEnumerable) this.tableParameter.Rows)
      {
        ushort uint16 = Convert.ToUInt16(row.Cells[5].Value.ToString());
        int int32 = Convert.ToInt32(row.Cells[3].Value);
        if (int32 > 0)
        {
          string hexString = Util.ByteArrayToHexString(tag.Map.GetMemoryBytes(uint16, int32));
          row.Cells[6].Value = (object) hexString;
        }
      }
    }

    private void tableParameter_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      this.ShowParameterInfo(e.RowIndex);
    }

    private void cboxHandlerObject_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.initialized)
        return;
      this.LoadParameter();
    }

    private void ShowParameterInfo(int rowIndex)
    {
      if (rowIndex < 0)
        return;
      string TextKey = this.tableParameter[1, rowIndex].Value.ToString();
      string translatedLanguageText = Ot.GetTranslatedLanguageText("S3ParaDesc", TextKey);
      if (translatedLanguageText != "S3ParaDesc" + TextKey)
        this.txtDescription.Text = translatedLanguageText;
      else
        this.txtDescription.Text = string.Empty;
    }

    internal static void ShowDialog(Form owner, EDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (ParameterEditor parameterEditor = new ParameterEditor())
      {
        parameterEditor.MyFunctions = MyFunctions;
        int num = (int) parameterEditor.ShowDialog((IWin32Window) owner);
      }
    }

    private EDC_Meter GetHandlerMeter()
    {
      EDC_Meter edcMeter;
      switch ((HandlerMeterType) Enum.Parse(typeof (HandlerMeterType), this.cboxHandlerObject.SelectedItem.ToString()))
      {
        case HandlerMeterType.WorkMeter:
          edcMeter = this.MyFunctions.WorkMeter;
          break;
        case HandlerMeterType.TypeMeter:
          edcMeter = this.MyFunctions.TypeMeter;
          break;
        case HandlerMeterType.BackupMeter:
          edcMeter = this.MyFunctions.BackupMeter;
          break;
        case HandlerMeterType.ConnectedMeter:
          edcMeter = this.MyFunctions.ConnectedMeter;
          break;
        default:
          throw new NotImplementedException();
      }
      return edcMeter?.DeepCopy();
    }

    private void LoadParameter()
    {
      EDC_Meter handlerMeter = this.GetHandlerMeter();
      this.panel.Visible = handlerMeter != null;
      this.tableParameter.Tag = (object) handlerMeter;
      if (handlerMeter == null)
        this.tableParameter.DataSource = (object) null;
      else
        this.tableParameter.DataSource = (object) handlerMeter.CreateParameterTable();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ParameterEditor));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.tableParameter = new DataGridView();
      this.txtDescription = new TextBox();
      this.splitContainer1 = new SplitContainer();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.btnPrint = new Button();
      this.label36 = new Label();
      this.cboxHandlerObject = new ComboBox();
      this.panel = new Panel();
      ((ISupportInitialize) this.tableParameter).BeginInit();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.panel.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(784, 36);
      this.zennerCoroprateDesign1.TabIndex = 1;
      this.tableParameter.AllowUserToAddRows = false;
      this.tableParameter.AllowUserToDeleteRows = false;
      this.tableParameter.AllowUserToResizeColumns = false;
      this.tableParameter.AllowUserToResizeRows = false;
      gridViewCellStyle1.BackColor = Color.FromArgb(228, 241, 244);
      this.tableParameter.AlternatingRowsDefaultCellStyle = gridViewCellStyle1;
      this.tableParameter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.tableParameter.BackgroundColor = Color.White;
      this.tableParameter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.True;
      this.tableParameter.DefaultCellStyle = gridViewCellStyle2;
      this.tableParameter.Dock = DockStyle.Fill;
      this.tableParameter.Location = new Point(0, 0);
      this.tableParameter.Name = "tableParameter";
      this.tableParameter.RowHeadersVisible = false;
      this.tableParameter.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.tableParameter.RowsDefaultCellStyle = gridViewCellStyle3;
      this.tableParameter.RowTemplate.Resizable = DataGridViewTriState.True;
      this.tableParameter.Size = new Size(777, 373);
      this.tableParameter.TabIndex = 0;
      this.tableParameter.CellClick += new DataGridViewCellEventHandler(this.tableParameter_CellClick);
      this.tableParameter.CellValueChanged += new DataGridViewCellEventHandler(this.dataGridViewParameter_CellValueChanged);
      this.tableParameter.SelectionChanged += new System.EventHandler(this.tableParameter_SelectionChanged);
      this.tableParameter.Sorted += new System.EventHandler(this.tableParameter_Sorted);
      this.txtDescription.BackColor = Color.White;
      this.txtDescription.BorderStyle = BorderStyle.FixedSingle;
      this.txtDescription.Dock = DockStyle.Fill;
      this.txtDescription.Location = new Point(0, 0);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(777, 99);
      this.txtDescription.TabIndex = 14;
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(2, 3);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.Controls.Add((Control) this.tableParameter);
      this.splitContainer1.Panel2.Controls.Add((Control) this.txtDescription);
      this.splitContainer1.Size = new Size(777, 476);
      this.splitContainer1.SplitterDistance = 373;
      this.splitContainer1.TabIndex = 15;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Enabled = false;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.ImeMode = ImeMode.NoControl;
      this.btnSave.Location = new Point(595, 485);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(77, 29);
      this.btnSave.TabIndex = 16;
      this.btnSave.Text = "Save";
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(695, 485);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(77, 29);
      this.btnCancel.TabIndex = 17;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      this.btnPrint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnPrint.Image = (Image) componentResourceManager.GetObject("btnPrint.Image");
      this.btnPrint.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPrint.Location = new Point(12, 485);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(66, 29);
      this.btnPrint.TabIndex = 19;
      this.btnPrint.Text = "Print";
      this.btnPrint.TextAlign = ContentAlignment.MiddleRight;
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new System.EventHandler(this.btnPrintTable_Click);
      this.label36.BackColor = Color.White;
      this.label36.Location = new Point(160, 7);
      this.label36.Name = "label36";
      this.label36.Size = new Size(84, 15);
      this.label36.TabIndex = 53;
      this.label36.Text = "Handler object:";
      this.label36.TextAlign = ContentAlignment.MiddleRight;
      this.cboxHandlerObject.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxHandlerObject.FormattingEnabled = true;
      this.cboxHandlerObject.Location = new Point(252, 5);
      this.cboxHandlerObject.Name = "cboxHandlerObject";
      this.cboxHandlerObject.Size = new Size(132, 21);
      this.cboxHandlerObject.TabIndex = 52;
      this.cboxHandlerObject.SelectedIndexChanged += new System.EventHandler(this.cboxHandlerObject_SelectedIndexChanged);
      this.panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel.Controls.Add((Control) this.splitContainer1);
      this.panel.Controls.Add((Control) this.btnPrint);
      this.panel.Controls.Add((Control) this.btnCancel);
      this.panel.Controls.Add((Control) this.btnSave);
      this.panel.Location = new Point(0, 36);
      this.panel.Name = "panel";
      this.panel.Size = new Size(782, 524);
      this.panel.TabIndex = 54;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(784, 562);
      this.Controls.Add((Control) this.panel);
      this.Controls.Add((Control) this.label36);
      this.Controls.Add((Control) this.cboxHandlerObject);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ParameterEditor);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Parameter Editor";
      this.Load += new System.EventHandler(this.ParameterEditor_Load);
      ((ISupportInitialize) this.tableParameter).EndInit();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.panel.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
