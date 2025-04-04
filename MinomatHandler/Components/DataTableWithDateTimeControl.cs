// Decompiled with JetBrains decompiler
// Type: MinomatHandler.Components.DataTableWithDateTimeControl
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler.Components
{
  public class DataTableWithDateTimeControl : UserControl
  {
    internal bool isCanceled;
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
    internal Label lblValueName4;
    internal Label lblValueName3;
    internal DateTimePicker txtValue3;
    internal DateTimePicker txtValue4;
    internal ComboBox txtValue2;
    internal Label lblValueName2;
    private Label lblCount;
    private Label label2;
    internal ProgressBar progress;
    internal Button btnValue1;
    internal Button btnSaveToClipBoard;
    internal Button btnStop;
    internal CheckBox ckboxValue1;

    public DataTableWithDateTimeControl()
    {
      this.InitializeComponent();
      this.isCanceled = false;
    }

    private void table_DataSourceChanged(object sender, EventArgs e) => this.UpdateCtrls();

    private void table_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      this.UpdateCtrls();
      this.table.CurrentCell = this.table[0, this.table.Rows.Count - 1];
    }

    private void table_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
      this.UpdateCtrls();
    }

    private void UpdateCtrls()
    {
      this.btnPrint.Enabled = this.table.RowCount > 0;
      this.btnSave.Enabled = this.table.RowCount > 0;
      this.btnSaveToClipBoard.Enabled = this.table.RowCount > 0;
      this.lblCount.Text = this.table.RowCount.ToString();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.table.RowCount == 0)
        return;
      string str = ";";
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.FileName = string.Format("{0}_{1}_{2}_{3}_{4}_{5:yyyy-MM-dd_HH-mm-ss}.csv", (object) this.lblTitel.Text, (object) this.txtValue1.Text, (object) this.txtValue2.Text, (object) this.txtValue3.Text, (object) this.txtValue4.Text, (object) DateTime.Now);
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
        {
          int count = this.table.Columns.Count;
          for (int index = 0; index < count; ++index)
            streamWriter.Write(this.table.Columns[index].Name.ToString().ToUpper() + str);
          streamWriter.WriteLine();
          for (int index1 = 0; index1 < this.table.Rows.Count - 1; ++index1)
          {
            for (int index2 = 0; index2 < count; ++index2)
            {
              if (this.table.Rows[index1].Cells[index2].Value != null)
                streamWriter.Write(this.table.Rows[index1].Cells[index2].Value?.ToString() + str);
              else
                streamWriter.Write(str);
            }
            streamWriter.WriteLine();
          }
        }
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      if (this.table.RowCount == 0)
        return;
      PrintDataGridView.Print(this.table, this.lblTitel.Text);
    }

    private void btnStop_Click(object sender, EventArgs e) => this.isCanceled = true;

    private void btnSaveToClipBoard_Click(object sender, EventArgs e)
    {
      this.table.SelectAll();
      Clipboard.SetDataObject((object) this.table.GetClipboardContent(), true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DataTableWithDateTimeControl));
      this.btnGet = new Button();
      this.table = new DataGridView();
      this.lblTitel = new Label();
      this.lblCMD = new Label();
      this.label1 = new Label();
      this.btnSave = new Button();
      this.btnPrint = new Button();
      this.txtValue1 = new ComboBox();
      this.lblValueName1 = new Label();
      this.lblValueName4 = new Label();
      this.lblValueName3 = new Label();
      this.txtValue3 = new DateTimePicker();
      this.txtValue4 = new DateTimePicker();
      this.txtValue2 = new ComboBox();
      this.lblValueName2 = new Label();
      this.lblCount = new Label();
      this.label2 = new Label();
      this.progress = new ProgressBar();
      this.btnValue1 = new Button();
      this.btnSaveToClipBoard = new Button();
      this.btnStop = new Button();
      this.ckboxValue1 = new CheckBox();
      ((ISupportInitialize) this.table).BeginInit();
      this.SuspendLayout();
      this.btnGet.Image = (Image) componentResourceManager.GetObject("btnGet.Image");
      this.btnGet.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnGet.Location = new Point(10, 40);
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
      this.table.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
      this.table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.table.Location = new Point(10, 163);
      this.table.Name = "table";
      this.table.RowHeadersVisible = false;
      this.table.Size = new Size(537, 259);
      this.table.TabIndex = 8;
      this.table.DataSourceChanged += new System.EventHandler(this.table_DataSourceChanged);
      this.table.RowsAdded += new DataGridViewRowsAddedEventHandler(this.table_RowsAdded);
      this.table.RowsRemoved += new DataGridViewRowsRemovedEventHandler(this.table_RowsRemoved);
      this.lblTitel.AutoSize = true;
      this.lblTitel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitel.Location = new Point(7, 9);
      this.lblTitel.Name = "lblTitel";
      this.lblTitel.Size = new Size(51, 16);
      this.lblTitel.TabIndex = 9;
      this.lblTitel.Text = "{Titel}";
      this.lblCMD.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCMD.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCMD.Location = new Point(481, 9);
      this.lblCMD.Name = "lblCMD";
      this.lblCMD.Size = new Size(60, 16);
      this.lblCMD.TabIndex = 17;
      this.lblCMD.Text = "0x0000";
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(433, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(50, 16);
      this.label1.TabIndex = 16;
      this.label1.Text = "CMD:";
      this.btnSave.Enabled = false;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(180, 40);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(128, 35);
      this.btnSave.TabIndex = 19;
      this.btnSave.Text = "Save to CSV-file";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.btnPrint.Enabled = false;
      this.btnPrint.Image = (Image) componentResourceManager.GetObject("btnPrint.Image");
      this.btnPrint.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPrint.Location = new Point(462, 40);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(78, 35);
      this.btnPrint.TabIndex = 18;
      this.btnPrint.Text = "Print";
      this.btnPrint.TextAlign = ContentAlignment.MiddleRight;
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
      this.txtValue1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtValue1.FormattingEnabled = true;
      this.txtValue1.Location = new Point(94, 85);
      this.txtValue1.Name = "txtValue1";
      this.txtValue1.Size = new Size(399, 21);
      this.txtValue1.TabIndex = 21;
      this.lblValueName1.AutoSize = true;
      this.lblValueName1.Location = new Point(8, 88);
      this.lblValueName1.Name = "lblValueName1";
      this.lblValueName1.Size = new Size(80, 13);
      this.lblValueName1.TabIndex = 20;
      this.lblValueName1.Text = "{Value name 1}";
      this.lblValueName4.AutoSize = true;
      this.lblValueName4.Location = new Point(184, 140);
      this.lblValueName4.Name = "lblValueName4";
      this.lblValueName4.Size = new Size(80, 13);
      this.lblValueName4.TabIndex = 23;
      this.lblValueName4.Text = "{Value name 4}";
      this.lblValueName3.AutoSize = true;
      this.lblValueName3.Location = new Point(8, 140);
      this.lblValueName3.Name = "lblValueName3";
      this.lblValueName3.Size = new Size(80, 13);
      this.lblValueName3.TabIndex = 22;
      this.lblValueName3.Text = "{Value name 3}";
      this.txtValue3.Format = DateTimePickerFormat.Short;
      this.txtValue3.Location = new Point(94, 137);
      this.txtValue3.MinDate = new DateTime(2001, 1, 1, 0, 0, 0, 0);
      this.txtValue3.Name = "txtValue3";
      this.txtValue3.Size = new Size(74, 20);
      this.txtValue3.TabIndex = 24;
      this.txtValue4.Format = DateTimePickerFormat.Short;
      this.txtValue4.Location = new Point(270, 137);
      this.txtValue4.MinDate = new DateTime(2001, 1, 1, 0, 0, 0, 0);
      this.txtValue4.Name = "txtValue4";
      this.txtValue4.Size = new Size(74, 20);
      this.txtValue4.TabIndex = 25;
      this.txtValue2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtValue2.FormattingEnabled = true;
      this.txtValue2.Location = new Point(94, 111);
      this.txtValue2.Name = "txtValue2";
      this.txtValue2.Size = new Size(399, 21);
      this.txtValue2.TabIndex = 27;
      this.lblValueName2.AutoSize = true;
      this.lblValueName2.Location = new Point(8, 114);
      this.lblValueName2.Name = "lblValueName2";
      this.lblValueName2.Size = new Size(80, 13);
      this.lblValueName2.TabIndex = 26;
      this.lblValueName2.Text = "{Value name 2}";
      this.lblCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblCount.Location = new Point(52, 425);
      this.lblCount.Name = "lblCount";
      this.lblCount.Size = new Size(51, 13);
      this.lblCount.TabIndex = 29;
      this.lblCount.Text = "0";
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 425);
      this.label2.Name = "label2";
      this.label2.Size = new Size(38, 13);
      this.label2.TabIndex = 28;
      this.label2.Text = "Count:";
      this.progress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.progress.Location = new Point(109, 425);
      this.progress.Name = "progress";
      this.progress.Size = new Size(438, 15);
      this.progress.TabIndex = 30;
      this.btnValue1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnValue1.Location = new Point(499, 83);
      this.btnValue1.Name = "btnValue1";
      this.btnValue1.Size = new Size(41, 23);
      this.btnValue1.TabIndex = 31;
      this.btnValue1.Text = "...";
      this.btnValue1.UseVisualStyleBackColor = true;
      this.btnSaveToClipBoard.Enabled = false;
      this.btnSaveToClipBoard.Image = (Image) componentResourceManager.GetObject("btnSaveToClipBoard.Image");
      this.btnSaveToClipBoard.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSaveToClipBoard.Location = new Point(325, 40);
      this.btnSaveToClipBoard.Name = "btnSaveToClipBoard";
      this.btnSaveToClipBoard.Size = new Size(126, 35);
      this.btnSaveToClipBoard.TabIndex = 32;
      this.btnSaveToClipBoard.Text = "Save to clipboard";
      this.btnSaveToClipBoard.TextAlign = ContentAlignment.MiddleRight;
      this.btnSaveToClipBoard.UseVisualStyleBackColor = true;
      this.btnSaveToClipBoard.Click += new System.EventHandler(this.btnSaveToClipBoard_Click);
      this.btnStop.Image = (Image) componentResourceManager.GetObject("btnStop.Image");
      this.btnStop.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnStop.Location = new Point(94, 40);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(78, 35);
      this.btnStop.TabIndex = 33;
      this.btnStop.Text = "Stop";
      this.btnStop.TextAlign = ContentAlignment.MiddleRight;
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
      this.ckboxValue1.AutoSize = true;
      this.ckboxValue1.Location = new Point(384, 140);
      this.ckboxValue1.Name = "ckboxValue1";
      this.ckboxValue1.Size = new Size(116, 17);
      this.ckboxValue1.TabIndex = 34;
      this.ckboxValue1.Text = "{Checked Value 1}";
      this.ckboxValue1.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.ckboxValue1);
      this.Controls.Add((Control) this.btnStop);
      this.Controls.Add((Control) this.btnSaveToClipBoard);
      this.Controls.Add((Control) this.btnValue1);
      this.Controls.Add((Control) this.lblTitel);
      this.Controls.Add((Control) this.progress);
      this.Controls.Add((Control) this.lblCount);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtValue2);
      this.Controls.Add((Control) this.lblValueName2);
      this.Controls.Add((Control) this.txtValue4);
      this.Controls.Add((Control) this.txtValue3);
      this.Controls.Add((Control) this.lblValueName4);
      this.Controls.Add((Control) this.lblValueName3);
      this.Controls.Add((Control) this.txtValue1);
      this.Controls.Add((Control) this.lblValueName1);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.lblCMD);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.table);
      this.Controls.Add((Control) this.btnGet);
      this.Name = nameof (DataTableWithDateTimeControl);
      this.Size = new Size(550, 443);
      ((ISupportInitialize) this.table).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
