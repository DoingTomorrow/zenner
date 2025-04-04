// Decompiled with JetBrains decompiler
// Type: EDC_Handler.ZoomEditor
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EDC_Handler
{
  public sealed class ZoomEditor : Form
  {
    private StringReader reader;
    private Font printFont;
    private SolidBrush printBrush;
    private IContainer components = (IContainer) null;
    private Button btnExport;
    private Button btnPrint;
    internal RichTextBox txtContent;

    public ZoomEditor()
    {
      this.InitializeComponent();
      this.printFont = new Font("Consolas", 8f);
      this.printBrush = new SolidBrush(Color.Black);
    }

    internal static void ShowDialog(Form owner, EDC_Meter meter)
    {
      if (meter == null)
        return;
      using (ZoomEditor zoomEditor = new ZoomEditor())
      {
        zoomEditor.txtContent.Text = meter.ToString();
        zoomEditor.txtContent.Select(0, 0);
        int num = (int) zoomEditor.ShowDialog((IWin32Window) owner);
      }
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtContent.Text))
        return;
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.FileName = string.Format("EDC_{0:yyyy-MM-dd_HH-mm-ss}.txt", (object) DateTime.Now);
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        this.txtContent.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
      }
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
      this.btnExport.Enabled = this.btnPrint.Enabled;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ZoomEditor));
      this.btnExport = new Button();
      this.btnPrint = new Button();
      this.txtContent = new RichTextBox();
      this.SuspendLayout();
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Image = (Image) componentResourceManager.GetObject("btnExport.Image");
      this.btnExport.ImeMode = ImeMode.NoControl;
      this.btnExport.Location = new Point(45, 3);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(30, 30);
      this.btnExport.TabIndex = 6;
      this.btnExport.UseVisualStyleBackColor = false;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Image = (Image) componentResourceManager.GetObject("btnPrint.Image");
      this.btnPrint.ImeMode = ImeMode.NoControl;
      this.btnPrint.Location = new Point(9, 3);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(30, 30);
      this.btnPrint.TabIndex = 5;
      this.btnPrint.UseVisualStyleBackColor = false;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.txtContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtContent.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtContent.Location = new Point(-1, 37);
      this.txtContent.Name = "txtContent";
      this.txtContent.Size = new Size(785, 526);
      this.txtContent.TabIndex = 7;
      this.txtContent.Text = "";
      this.txtContent.TextChanged += new EventHandler(this.txtContent_TextChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(784, 562);
      this.Controls.Add((Control) this.txtContent);
      this.Controls.Add((Control) this.btnExport);
      this.Controls.Add((Control) this.btnPrint);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ZoomEditor);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Zoom";
      this.ResumeLayout(false);
    }
  }
}
