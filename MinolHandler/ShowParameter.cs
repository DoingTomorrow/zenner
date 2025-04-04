// Decompiled with JetBrains decompiler
// Type: MinolHandler.ShowParameter
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using CorporateDesign;
using StartupLib;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace MinolHandler
{
  public class ShowParameter : Form
  {
    private MinolDevice WorkDevice;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Panel panel1;
    private DataGridView tableParameter;
    private Button btnSave;
    private Button btnCancel;
    private Button btnPrintTable;
    private Button btnConverter;
    private Label lblSignature;
    private Label label1;

    internal ShowParameter(MinolDevice WorkDevice)
    {
      this.InitializeComponent();
      this.Text = "Parameters (" + WorkDevice.GetIdentificationString() + ")";
      this.WorkDevice = WorkDevice;
      this.tableParameter.DataSource = (object) WorkDevice.GetParameterTable();
      if (this.tableParameter.DataSource == null)
        return;
      this.tableParameter.Sort(this.tableParameter.Columns["Address"], ListSortDirection.Ascending);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Developer) && !UserManager.CheckPermission(UserRights.Rights.ProfessionalConfig))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change configuration paramter!");
        ZR_ClassLibMessages.ShowAndClearErrors();
        ZR_ClassLibMessages.ClearErrors();
      }
      else
      {
        try
        {
          foreach (DataGridViewRow row in (IEnumerable) this.tableParameter.Rows)
          {
            row.Cells[0].Value.ToString();
            string hex = row.Cells[2].Value.ToString();
            int integer = Util.ToInteger(row.Cells[1].Value);
            byte[] byteArray = Util.HexStringToByteArray(hex);
            Array.Reverse((Array) byteArray);
            this.WorkDevice.SetConfigurationParameters(integer, byteArray);
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message);
          return;
        }
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void btnPrintTable_Click(object sender, EventArgs e)
    {
      PrintDataGridView.Print(this.tableParameter, this.WorkDevice.GetIdentificationString());
    }

    private void btnConverter_Click(object sender, EventArgs e) => new MinolConverterForm().Show();

    private void dataGridViewParameter_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex != 2)
        return;
      string parameterDataHexString = this.WorkDevice.GetParameterDataHexString(this.tableParameter[0, e.RowIndex].Value.ToString());
      string str = this.tableParameter[e.ColumnIndex, e.RowIndex].Value.ToString();
      this.tableParameter[e.ColumnIndex, e.RowIndex].Style.BackColor = !(parameterDataHexString.ToUpper() == str.ToUpper()) ? Color.Yellow : Color.White;
    }

    private void tableParameter_Sorted(object sender, EventArgs e)
    {
      this.ModifyDataGridViewValues();
    }

    private void ModifyDataGridViewValues()
    {
      foreach (DataGridViewRow row in (IEnumerable) this.tableParameter.Rows)
      {
        string parameterDataHexString = this.WorkDevice.GetParameterDataHexString(row.Cells[0].Value.ToString());
        row.Cells[2].Value = (object) parameterDataHexString;
      }
    }

    public static void Show(MinolDevice device)
    {
      if (device == null)
        return;
      ShowParameter showParameter = new ShowParameter(device);
      showParameter.lblSignature.Text = "0x" + device.Signature.ToString("X4");
      showParameter.btnSave.Enabled = UserManager.CheckPermission(UserRights.Rights.Developer) || UserManager.CheckPermission(UserRights.Rights.ProfessionalConfig);
      try
      {
        int num = (int) showParameter.ShowDialog();
      }
      finally
      {
        showParameter.Dispose();
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
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ShowParameter));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.panel1 = new Panel();
      this.btnConverter = new Button();
      this.tableParameter = new DataGridView();
      this.btnPrintTable = new Button();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.lblSignature = new Label();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.tableParameter).BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign1.Dock = DockStyle.Fill;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(820, 483);
      this.zennerCoroprateDesign1.TabIndex = 1;
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.btnConverter);
      this.panel1.Controls.Add((Control) this.tableParameter);
      this.panel1.Controls.Add((Control) this.btnPrintTable);
      this.panel1.Controls.Add((Control) this.btnSave);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.lblSignature);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Location = new Point(0, 38);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(820, 445);
      this.panel1.TabIndex = 2;
      this.btnConverter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnConverter.Location = new Point(88, 413);
      this.btnConverter.Name = "btnConverter";
      this.btnConverter.Size = new Size(70, 23);
      this.btnConverter.TabIndex = 14;
      this.btnConverter.Text = "Converter";
      this.btnConverter.UseVisualStyleBackColor = true;
      this.btnConverter.Click += new System.EventHandler(this.btnConverter_Click);
      this.tableParameter.AllowUserToAddRows = false;
      this.tableParameter.AllowUserToDeleteRows = false;
      this.tableParameter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableParameter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.tableParameter.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
      this.tableParameter.BackgroundColor = Color.White;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Window;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.ControlText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.tableParameter.DefaultCellStyle = gridViewCellStyle1;
      this.tableParameter.Location = new Point(0, 19);
      this.tableParameter.Name = "tableParameter";
      this.tableParameter.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.True;
      this.tableParameter.RowsDefaultCellStyle = gridViewCellStyle2;
      this.tableParameter.RowTemplate.Resizable = DataGridViewTriState.True;
      this.tableParameter.Size = new Size(820, 388);
      this.tableParameter.TabIndex = 0;
      this.tableParameter.CellValueChanged += new DataGridViewCellEventHandler(this.dataGridViewParameter_CellValueChanged);
      this.tableParameter.Sorted += new System.EventHandler(this.tableParameter_Sorted);
      this.btnPrintTable.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnPrintTable.Location = new Point(12, 413);
      this.btnPrintTable.Name = "btnPrintTable";
      this.btnPrintTable.Size = new Size(70, 23);
      this.btnPrintTable.TabIndex = 13;
      this.btnPrintTable.Text = "Print...";
      this.btnPrintTable.UseVisualStyleBackColor = true;
      this.btnPrintTable.Click += new System.EventHandler(this.btnPrintTable_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(736, 413);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 10;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(646, 413);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(4, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(69, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Signature: ";
      this.lblSignature.AutoSize = true;
      this.lblSignature.BackColor = Color.Transparent;
      this.lblSignature.Location = new Point(73, 3);
      this.lblSignature.Name = "lblSignature";
      this.lblSignature.Size = new Size(50, 13);
      this.lblSignature.TabIndex = 15;
      this.lblSignature.Text = "{0x0000}";
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(820, 483);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ShowParameter);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Parameter list";
      this.WindowState = FormWindowState.Maximized;
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.tableParameter).EndInit();
      this.ResumeLayout(false);
    }
  }
}
