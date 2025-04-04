// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.PrintDataGridViewOptions
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class PrintDataGridViewOptions : Form
  {
    private IContainer components = (IContainer) null;
    internal RadioButton rdoSelectedRows;
    internal RadioButton rdoAllRows;
    internal CheckBox chkFitToPageWidth;
    internal Label lblTitle;
    internal TextBox txtTitle;
    internal GroupBox gboxRowsToPrint;
    internal Label lblColumnsToPrint;
    protected Button btnOK;
    protected Button btnCancel;
    internal CheckedListBox chklst;

    public PrintDataGridViewOptions() => this.InitializeComponent();

    public PrintDataGridViewOptions(List<string> availableFields)
    {
      this.InitializeComponent();
      foreach (object availableField in availableFields)
        this.chklst.Items.Add(availableField, true);
    }

    private void PrintOtions_Load(object sender, EventArgs e)
    {
      this.rdoAllRows.Checked = true;
      this.chkFitToPageWidth.Checked = true;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    public List<string> GetSelectedColumns()
    {
      List<string> selectedColumns = new List<string>();
      foreach (object checkedItem in this.chklst.CheckedItems)
        selectedColumns.Add(checkedItem.ToString());
      return selectedColumns;
    }

    public string PrintTitle
    {
      get => this.txtTitle.Text;
      set => this.txtTitle.Text = value;
    }

    public bool PrintAllRows => this.rdoAllRows.Checked;

    public bool FitToPageWidth => this.chkFitToPageWidth.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PrintDataGridViewOptions));
      this.rdoSelectedRows = new RadioButton();
      this.rdoAllRows = new RadioButton();
      this.chkFitToPageWidth = new CheckBox();
      this.lblTitle = new Label();
      this.txtTitle = new TextBox();
      this.gboxRowsToPrint = new GroupBox();
      this.lblColumnsToPrint = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.chklst = new CheckedListBox();
      this.gboxRowsToPrint.SuspendLayout();
      this.SuspendLayout();
      this.rdoSelectedRows.AutoSize = true;
      this.rdoSelectedRows.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.rdoSelectedRows.Location = new Point(91, 19);
      this.rdoSelectedRows.Name = "rdoSelectedRows";
      this.rdoSelectedRows.Size = new Size(75, 17);
      this.rdoSelectedRows.TabIndex = 1;
      this.rdoSelectedRows.TabStop = true;
      this.rdoSelectedRows.Text = "Selected";
      this.rdoSelectedRows.UseVisualStyleBackColor = true;
      this.rdoAllRows.AutoSize = true;
      this.rdoAllRows.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.rdoAllRows.Location = new Point(9, 19);
      this.rdoAllRows.Name = "rdoAllRows";
      this.rdoAllRows.Size = new Size(39, 17);
      this.rdoAllRows.TabIndex = 0;
      this.rdoAllRows.TabStop = true;
      this.rdoAllRows.Text = "All";
      this.rdoAllRows.UseVisualStyleBackColor = true;
      this.chkFitToPageWidth.AutoSize = true;
      this.chkFitToPageWidth.CheckAlign = ContentAlignment.MiddleRight;
      this.chkFitToPageWidth.FlatStyle = FlatStyle.System;
      this.chkFitToPageWidth.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkFitToPageWidth.Location = new Point(187, 78);
      this.chkFitToPageWidth.Name = "chkFitToPageWidth";
      this.chkFitToPageWidth.Size = new Size((int) sbyte.MaxValue, 18);
      this.chkFitToPageWidth.TabIndex = 21;
      this.chkFitToPageWidth.Text = "Fit to page width";
      this.chkFitToPageWidth.UseVisualStyleBackColor = true;
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(184, 107);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(80, 13);
      this.lblTitle.TabIndex = 20;
      this.lblTitle.Text = "Title of print ";
      this.txtTitle.AcceptsReturn = true;
      this.txtTitle.Location = new Point(184, 124);
      this.txtTitle.Multiline = true;
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ScrollBars = ScrollBars.Vertical;
      this.txtTitle.Size = new Size(176, 77);
      this.txtTitle.TabIndex = 19;
      this.gboxRowsToPrint.Controls.Add((Control) this.rdoSelectedRows);
      this.gboxRowsToPrint.Controls.Add((Control) this.rdoAllRows);
      this.gboxRowsToPrint.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.gboxRowsToPrint.Location = new Point(184, 22);
      this.gboxRowsToPrint.Name = "gboxRowsToPrint";
      this.gboxRowsToPrint.Size = new Size(173, 42);
      this.gboxRowsToPrint.TabIndex = 18;
      this.gboxRowsToPrint.TabStop = false;
      this.gboxRowsToPrint.Text = "Rows to print";
      this.lblColumnsToPrint.AutoSize = true;
      this.lblColumnsToPrint.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblColumnsToPrint.Location = new Point(8, 9);
      this.lblColumnsToPrint.Name = "lblColumnsToPrint";
      this.lblColumnsToPrint.Size = new Size(102, 13);
      this.lblColumnsToPrint.TabIndex = 17;
      this.lblColumnsToPrint.Text = "Columns to print ";
      this.btnOK.BackColor = SystemColors.Control;
      this.btnOK.Cursor = Cursors.Default;
      this.btnOK.FlatStyle = FlatStyle.System;
      this.btnOK.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 178);
      this.btnOK.ForeColor = SystemColors.ControlText;
      this.btnOK.Image = (Image) componentResourceManager.GetObject("btnOK.Image");
      this.btnOK.ImageAlign = ContentAlignment.MiddleRight;
      this.btnOK.Location = new Point(184, 247);
      this.btnOK.Name = "btnOK";
      this.btnOK.RightToLeft = RightToLeft.No;
      this.btnOK.Size = new Size(56, 25);
      this.btnOK.TabIndex = 14;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = false;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.Cursor = Cursors.Default;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.FlatStyle = FlatStyle.System;
      this.btnCancel.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 178);
      this.btnCancel.ForeColor = SystemColors.ControlText;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.Location = new Point(246, 247);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.RightToLeft = RightToLeft.No;
      this.btnCancel.Size = new Size(56, 25);
      this.btnCancel.TabIndex = 15;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = false;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      this.chklst.CheckOnClick = true;
      this.chklst.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chklst.FormattingEnabled = true;
      this.chklst.Location = new Point(8, 28);
      this.chklst.Name = "chklst";
      this.chklst.Size = new Size(170, 244);
      this.chklst.TabIndex = 13;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(369, 281);
      this.Controls.Add((Control) this.chkFitToPageWidth);
      this.Controls.Add((Control) this.lblTitle);
      this.Controls.Add((Control) this.txtTitle);
      this.Controls.Add((Control) this.gboxRowsToPrint);
      this.Controls.Add((Control) this.lblColumnsToPrint);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.chklst);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = "PrintOptions";
      this.SizeGripStyle = SizeGripStyle.Show;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Print Options";
      this.Load += new System.EventHandler(this.PrintOtions_Load);
      this.gboxRowsToPrint.ResumeLayout(false);
      this.gboxRowsToPrint.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
