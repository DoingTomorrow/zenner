// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiParserWindow
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public class SCGiParserWindow : Form
  {
    private IContainer components = (IContainer) null;
    private Label label1;
    private Button btnParse;
    private TextBox txtSCGiPacket;
    private RichTextBox txtResult;
    private Label label2;
    private Button btnParseHeader;

    public SCGiParserWindow() => this.InitializeComponent();

    public static void Show(Form owner)
    {
      using (SCGiParserWindow scGiParserWindow = new SCGiParserWindow())
      {
        int num = (int) scGiParserWindow.ShowDialog((IWin32Window) owner);
      }
    }

    private void btnParse_Click(object sender, EventArgs e)
    {
      byte[] buffer = this.GetBuffer();
      if (buffer == null)
        return;
      try
      {
        SCGiPacket scGiPacket = SCGiPacket.Parse(buffer);
        if (scGiPacket == null)
          this.txtResult.Text = "Invalid SCGi packet!";
        else
          this.txtResult.Text = scGiPacket.ToString();
      }
      catch (Exception ex)
      {
        this.txtResult.Text = ex.Message;
      }
    }

    private void btnParseHeader_Click(object sender, EventArgs e)
    {
      byte[] buffer = this.GetBuffer();
      if (buffer == null)
        return;
      try
      {
        SCGiHeader scGiHeader = SCGiHeader.Parse(buffer);
        if (scGiHeader == null)
          this.txtResult.Text = "Invalid SCGi header!";
        else
          this.txtResult.Text = scGiHeader.ToString();
      }
      catch (Exception ex)
      {
        this.txtResult.Text = ex.Message;
      }
    }

    private byte[] GetBuffer()
    {
      if (string.IsNullOrEmpty(this.txtSCGiPacket.Text))
        return (byte[]) null;
      string hex = this.txtSCGiPacket.Text.Replace(" ", "").Replace("\r", "").Replace("\n", "");
      try
      {
        return Util.HexStringToByteArray(hex);
      }
      catch
      {
        return (byte[]) null;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SCGiParserWindow));
      this.label1 = new Label();
      this.btnParse = new Button();
      this.txtSCGiPacket = new TextBox();
      this.txtResult = new RichTextBox();
      this.label2 = new Label();
      this.btnParseHeader = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(68, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "SCGi Packet";
      this.btnParse.Location = new Point(7, 107);
      this.btnParse.Name = "btnParse";
      this.btnParse.Size = new Size(126, 23);
      this.btnParse.TabIndex = 0;
      this.btnParse.Text = "Try parse frame";
      this.btnParse.UseVisualStyleBackColor = true;
      this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
      this.txtSCGiPacket.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSCGiPacket.BorderStyle = BorderStyle.FixedSingle;
      this.txtSCGiPacket.Location = new Point(7, 30);
      this.txtSCGiPacket.Multiline = true;
      this.txtSCGiPacket.Name = "txtSCGiPacket";
      this.txtSCGiPacket.ScrollBars = ScrollBars.Vertical;
      this.txtSCGiPacket.Size = new Size(534, 71);
      this.txtSCGiPacket.TabIndex = 1;
      this.txtResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtResult.BorderStyle = BorderStyle.FixedSingle;
      this.txtResult.Location = new Point(7, 158);
      this.txtResult.Name = "txtResult";
      this.txtResult.Size = new Size(534, 322);
      this.txtResult.TabIndex = 2;
      this.txtResult.Text = "";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(4, 141);
      this.label2.Name = "label2";
      this.label2.Size = new Size(37, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Result";
      this.btnParseHeader.Location = new Point(161, 107);
      this.btnParseHeader.Name = "btnParseHeader";
      this.btnParseHeader.Size = new Size(126, 23);
      this.btnParseHeader.TabIndex = 5;
      this.btnParseHeader.Text = "Try parse header";
      this.btnParseHeader.UseVisualStyleBackColor = true;
      this.btnParseHeader.Click += new System.EventHandler(this.btnParseHeader_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(548, 489);
      this.Controls.Add((Control) this.btnParseHeader);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtResult);
      this.Controls.Add((Control) this.txtSCGiPacket);
      this.Controls.Add((Control) this.btnParse);
      this.Controls.Add((Control) this.label1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (SCGiParserWindow);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "SCGi Parser";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
