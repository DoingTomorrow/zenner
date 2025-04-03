// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MBusParserWindow
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace DeviceCollector
{
  public class MBusParserWindow : Form
  {
    private IContainer components = (IContainer) null;
    private TextBox txtInput;
    private Button btnParse;
    private TextBox txtOutput;
    private Label label1;
    private Label label2;

    public MBusParserWindow() => this.InitializeComponent();

    private void btnParse_Click(object sender, EventArgs e)
    {
      try
      {
        string str = this.txtInput.Text.Replace(" ", "").Replace("\r\n", "");
        this.txtOutput.Text = MBusDevice.ParseMBusDifVif(ZR_ClassLibrary.Util.HexStringToByteArray(str.Substring(38, str.Length - 38 - 4)));
        this.txtOutput.Text = this.txtOutput.Text.Replace(";", Environment.NewLine);
      }
      catch (Exception ex)
      {
        this.txtOutput.Text = ex.Message;
      }
    }

    internal static void ShowWindow()
    {
      using (MBusParserWindow mbusParserWindow = new MBusParserWindow())
      {
        int num = (int) mbusParserWindow.ShowDialog();
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
      this.txtInput = new TextBox();
      this.btnParse = new Button();
      this.txtOutput = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.SuspendLayout();
      this.txtInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtInput.Location = new Point(3, 23);
      this.txtInput.Multiline = true;
      this.txtInput.Name = "txtInput";
      this.txtInput.ScrollBars = ScrollBars.Vertical;
      this.txtInput.Size = new Size(481, 111);
      this.txtInput.TabIndex = 0;
      this.btnParse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnParse.Location = new Point(409, 137);
      this.btnParse.Name = "btnParse";
      this.btnParse.Size = new Size(75, 23);
      this.btnParse.TabIndex = 1;
      this.btnParse.Text = "Parse";
      this.btnParse.UseVisualStyleBackColor = true;
      this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
      this.txtOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOutput.Location = new Point(3, 166);
      this.txtOutput.Multiline = true;
      this.txtOutput.Name = "txtOutput";
      this.txtOutput.ScrollBars = ScrollBars.Vertical;
      this.txtOutput.Size = new Size(481, 358);
      this.txtOutput.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(38, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Buffer:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 146);
      this.label2.Name = "label2";
      this.label2.Size = new Size(31, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "ZDF:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(485, 527);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtOutput);
      this.Controls.Add((Control) this.btnParse);
      this.Controls.Add((Control) this.txtInput);
      this.Name = nameof (MBusParserWindow);
      this.Text = nameof (MBusParserWindow);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
