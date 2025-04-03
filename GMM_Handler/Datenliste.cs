// Decompiled with JetBrains decompiler
// Type: GMM_Handler.Datenliste
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class Datenliste : Form
  {
    private string SetupFileName;
    private IContainer components = (IContainer) null;
    private DataGridView dataGridView1;

    public Datenliste(DataTable TheTable)
    {
      this.InitializeComponent();
      this.SetupFileName = Path.Combine(SystemValues.LoggDataPath, "DataListSetup.txt");
      this.dataGridView1.DataSource = (object) TheTable;
    }

    private void Datenliste_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dataGridView1 = new DataGridView();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.SuspendLayout();
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToOrderColumns = true;
      this.dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Dock = DockStyle.Fill;
      this.dataGridView1.Location = new Point(0, 0);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.Size = new Size(954, 494);
      this.dataGridView1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(954, 494);
      this.Controls.Add((Control) this.dataGridView1);
      this.Name = nameof (Datenliste);
      this.Text = nameof (Datenliste);
      this.WindowState = FormWindowState.Maximized;
      this.FormClosing += new FormClosingEventHandler(this.Datenliste_FormClosing);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
