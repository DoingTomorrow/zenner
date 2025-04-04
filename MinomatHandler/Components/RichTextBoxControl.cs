// Decompiled with JetBrains decompiler
// Type: MinomatHandler.Components.RichTextBoxControl
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace MinomatHandler.Components
{
  public class RichTextBoxControl : UserControl
  {
    private StringReader reader;
    private Font printFont;
    private SolidBrush printBrush;
    internal bool isCanceled;
    private IContainer components = (IContainer) null;
    internal Button btnGet;
    internal Label lblTitel;
    internal RichTextBox txtContent;
    internal Button btnPrint;
    internal Button btnSave;
    internal ComboBox txtValue1;
    internal Label lblValueName1;
    internal ComboBox txtValue2;
    internal Label lblValueName2;
    internal Button btnStop;

    public RichTextBoxControl()
    {
      this.InitializeComponent();
      this.printFont = new Font("Consolas", 8f);
      this.printBrush = new SolidBrush(Color.Black);
      this.isCanceled = false;
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtContent.Text))
        return;
      using (PrintDialog printDialog = new PrintDialog())
      {
        using (PrintDocument printDocument = new PrintDocument())
        {
          printDialog.Document = printDocument;
          if (printDialog.ShowDialog() != DialogResult.OK)
            return;
          using (this.reader = new StringReader(this.txtContent.Text))
          {
            printDocument.DefaultPageSettings.Landscape = false;
            printDocument.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);
            printDocument.PrintPage += new PrintPageEventHandler(this.DocumentToPrint_PrintPage);
            printDocument.Print();
          }
        }
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtContent.Text))
        return;
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.FileName = string.Format("MinomatV4Settings_{0:yyyy-MM-dd_HH-mm-ss}.txt", (object) DateTime.Now);
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        this.txtContent.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
      }
    }

    private void btnStop_Click(object sender, EventArgs e) => this.isCanceled = true;

    private void RichTextBoxControl_VisibleChanged(object sender, EventArgs e)
    {
      if (this.btnGet.Enabled)
        return;
      this.isCanceled = true;
    }

    private void DocumentToPrint_PrintPage(object sender, PrintPageEventArgs e)
    {
      int num1 = 0;
      float left = (float) e.MarginBounds.Left;
      float top = (float) e.MarginBounds.Top;
      string s = (string) null;
      for (float num2 = (float) e.MarginBounds.Height / this.printFont.GetHeight(e.Graphics); (double) num1 < (double) num2 && (s = this.reader.ReadLine()) != null; ++num1)
      {
        float y = top + (float) num1 * this.printFont.GetHeight(e.Graphics);
        e.Graphics.DrawString(s, this.printFont, (Brush) this.printBrush, left, y, new StringFormat());
      }
      if (s != null)
        e.HasMorePages = true;
      else
        e.HasMorePages = false;
    }

    private void txtContent_TextChanged(object sender, EventArgs e)
    {
      this.btnPrint.Enabled = !string.IsNullOrEmpty(this.txtContent.Text);
      this.btnSave.Enabled = this.btnPrint.Enabled;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RichTextBoxControl));
      this.btnGet = new Button();
      this.lblTitel = new Label();
      this.txtContent = new RichTextBox();
      this.btnPrint = new Button();
      this.btnSave = new Button();
      this.txtValue1 = new ComboBox();
      this.lblValueName1 = new Label();
      this.txtValue2 = new ComboBox();
      this.lblValueName2 = new Label();
      this.btnStop = new Button();
      this.SuspendLayout();
      this.btnGet.Image = (Image) componentResourceManager.GetObject("btnGet.Image");
      this.btnGet.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnGet.Location = new Point(10, 41);
      this.btnGet.Name = "btnGet";
      this.btnGet.Size = new Size(78, 35);
      this.btnGet.TabIndex = 3;
      this.btnGet.Text = "Read";
      this.btnGet.TextAlign = ContentAlignment.MiddleRight;
      this.btnGet.UseVisualStyleBackColor = true;
      this.lblTitel.AutoSize = true;
      this.lblTitel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitel.Location = new Point(7, 9);
      this.lblTitel.Name = "lblTitel";
      this.lblTitel.Size = new Size(51, 16);
      this.lblTitel.TabIndex = 4;
      this.lblTitel.Text = "{Titel}";
      this.txtContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContent.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtContent.Location = new Point(10, 106);
      this.txtContent.Name = "txtContent";
      this.txtContent.Size = new Size(434, 320);
      this.txtContent.TabIndex = 5;
      this.txtContent.Text = "";
      this.txtContent.TextChanged += new EventHandler(this.txtContent_TextChanged);
      this.btnPrint.Enabled = false;
      this.btnPrint.Image = (Image) componentResourceManager.GetObject("btnPrint.Image");
      this.btnPrint.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPrint.Location = new Point(218, 41);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(78, 35);
      this.btnPrint.TabIndex = 6;
      this.btnPrint.Text = "Print";
      this.btnPrint.TextAlign = ContentAlignment.MiddleRight;
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnSave.Enabled = false;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.Location = new Point(107, 41);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(100, 35);
      this.btnSave.TabIndex = 7;
      this.btnSave.Text = "Save to file";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.txtValue1.FormattingEnabled = true;
      this.txtValue1.Location = new Point(94, 79);
      this.txtValue1.Name = "txtValue1";
      this.txtValue1.Size = new Size(120, 21);
      this.txtValue1.TabIndex = 18;
      this.lblValueName1.AutoSize = true;
      this.lblValueName1.Location = new Point(8, 82);
      this.lblValueName1.Name = "lblValueName1";
      this.lblValueName1.Size = new Size(80, 13);
      this.lblValueName1.TabIndex = 17;
      this.lblValueName1.Text = "{Value name 1}";
      this.txtValue2.FormattingEnabled = true;
      this.txtValue2.Location = new Point(312, 79);
      this.txtValue2.Name = "txtValue2";
      this.txtValue2.Size = new Size(120, 21);
      this.txtValue2.TabIndex = 20;
      this.lblValueName2.AutoSize = true;
      this.lblValueName2.Location = new Point(226, 82);
      this.lblValueName2.Name = "lblValueName2";
      this.lblValueName2.Size = new Size(80, 13);
      this.lblValueName2.TabIndex = 19;
      this.lblValueName2.Text = "{Value name 2}";
      this.btnStop.Image = (Image) componentResourceManager.GetObject("btnStop.Image");
      this.btnStop.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnStop.Location = new Point(320, 41);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(78, 35);
      this.btnStop.TabIndex = 21;
      this.btnStop.Text = "Stop";
      this.btnStop.TextAlign = ContentAlignment.MiddleRight;
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new EventHandler(this.btnStop_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btnStop);
      this.Controls.Add((Control) this.txtValue2);
      this.Controls.Add((Control) this.lblValueName2);
      this.Controls.Add((Control) this.txtValue1);
      this.Controls.Add((Control) this.lblValueName1);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.txtContent);
      this.Controls.Add((Control) this.lblTitel);
      this.Controls.Add((Control) this.btnGet);
      this.Name = nameof (RichTextBoxControl);
      this.Size = new Size(447, 429);
      this.VisibleChanged += new EventHandler(this.RichTextBoxControl_VisibleChanged);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
