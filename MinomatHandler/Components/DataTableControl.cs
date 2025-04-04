// Decompiled with JetBrains decompiler
// Type: MinomatHandler.Components.DataTableControl
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler.Components
{
  public class DataTableControl : UserControl
  {
    private IContainer components = (IContainer) null;
    internal Button btnGet;
    internal DataGridView table;
    internal Label lblTitel;
    internal Label lblCMD;
    internal Label label1;
    internal Button btnSave;
    internal Button btnPrint;
    internal ComboBox txtValue1;
    internal Label lblValueName1;
    private Label label2;
    private Label lblCount;

    public DataTableControl() => this.InitializeComponent();

    private void table_DataSourceChanged(object sender, EventArgs e)
    {
      this.btnPrint.Enabled = this.table.RowCount > 0;
      this.btnSave.Enabled = this.table.RowCount > 0;
      this.lblCount.Text = this.table.RowCount.ToString();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.table.RowCount == 0 || !(this.table.DataSource is DataTable dataSource))
        return;
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.FileName = string.Format("{0} {1:yyyy-MM-dd_HH-mm-ss}.txt", (object) this.lblTitel.Text, (object) DateTime.Now);
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        dataSource.WriteXml(saveFileDialog.FileName);
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      if (this.table.RowCount == 0)
        return;
      PrintDataGridView.Print(this.table, this.lblTitel.Text);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DataTableControl));
      this.btnGet = new Button();
      this.table = new DataGridView();
      this.lblTitel = new Label();
      this.lblCMD = new Label();
      this.label1 = new Label();
      this.btnSave = new Button();
      this.btnPrint = new Button();
      this.txtValue1 = new ComboBox();
      this.lblValueName1 = new Label();
      this.label2 = new Label();
      this.lblCount = new Label();
      ((ISupportInitialize) this.table).BeginInit();
      this.SuspendLayout();
      this.btnGet.Image = (Image) componentResourceManager.GetObject("btnGet.Image");
      this.btnGet.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnGet.Location = new Point(10, 41);
      this.btnGet.Name = "btnGet";
      this.btnGet.Size = new Size(78, 35);
      this.btnGet.TabIndex = 7;
      this.btnGet.Text = "Read";
      this.btnGet.TextAlign = ContentAlignment.MiddleRight;
      this.btnGet.UseVisualStyleBackColor = true;
      this.table.AllowUserToAddRows = false;
      this.table.AllowUserToDeleteRows = false;
      this.table.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.table.BackgroundColor = Color.WhiteSmoke;
      this.table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.table.Location = new Point(10, 102);
      this.table.Name = "table";
      this.table.RowHeadersVisible = false;
      this.table.Size = new Size(388, 306);
      this.table.TabIndex = 8;
      this.table.DataSourceChanged += new System.EventHandler(this.table_DataSourceChanged);
      this.lblTitel.AutoSize = true;
      this.lblTitel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitel.Location = new Point(7, 9);
      this.lblTitel.Name = "lblTitel";
      this.lblTitel.Size = new Size(51, 16);
      this.lblTitel.TabIndex = 9;
      this.lblTitel.Text = "{Titel}";
      this.lblCMD.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCMD.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCMD.Location = new Point(332, 9);
      this.lblCMD.Name = "lblCMD";
      this.lblCMD.Size = new Size(60, 16);
      this.lblCMD.TabIndex = 17;
      this.lblCMD.Text = "0x0000";
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(284, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(50, 16);
      this.label1.TabIndex = 16;
      this.label1.Text = "CMD:";
      this.btnSave.Enabled = false;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(109, 41);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(100, 35);
      this.btnSave.TabIndex = 19;
      this.btnSave.Text = "Save to file";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.btnPrint.Enabled = false;
      this.btnPrint.Image = (Image) componentResourceManager.GetObject("btnPrint.Image");
      this.btnPrint.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPrint.Location = new Point(220, 41);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(78, 35);
      this.btnPrint.TabIndex = 18;
      this.btnPrint.Text = "Print";
      this.btnPrint.TextAlign = ContentAlignment.MiddleRight;
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
      this.txtValue1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtValue1.FormattingEnabled = true;
      this.txtValue1.Location = new Point(94, 78);
      this.txtValue1.Name = "txtValue1";
      this.txtValue1.Size = new Size(304, 21);
      this.txtValue1.TabIndex = 21;
      this.lblValueName1.AutoSize = true;
      this.lblValueName1.Location = new Point(8, 81);
      this.lblValueName1.Name = "lblValueName1";
      this.lblValueName1.Size = new Size(80, 13);
      this.lblValueName1.TabIndex = 20;
      this.lblValueName1.Text = "{Value name 1}";
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 413);
      this.label2.Name = "label2";
      this.label2.Size = new Size(38, 13);
      this.label2.TabIndex = 22;
      this.label2.Text = "Count:";
      this.lblCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblCount.AutoSize = true;
      this.lblCount.Location = new Point(55, 413);
      this.lblCount.Name = "lblCount";
      this.lblCount.Size = new Size(13, 13);
      this.lblCount.TabIndex = 23;
      this.lblCount.Text = "0";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblCount);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtValue1);
      this.Controls.Add((Control) this.lblValueName1);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.lblCMD);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblTitel);
      this.Controls.Add((Control) this.table);
      this.Controls.Add((Control) this.btnGet);
      this.Name = nameof (DataTableControl);
      this.Size = new Size(401, 429);
      ((ISupportInitialize) this.table).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
