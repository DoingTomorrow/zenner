// Decompiled with JetBrains decompiler
// Type: S3_Handler.TDC_Map
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace S3_Handler
{
  public class TDC_Map : Form
  {
    private IContainer components = (IContainer) null;
    private Button btnOk;

    public TDC_Map() => this.InitializeComponent();

    private void btnOk_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOk = new Button();
      this.SuspendLayout();
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Location = new Point(623, 297);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(97, 23);
      this.btnOk.TabIndex = 0;
      this.btnOk.Text = "Ok";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(759, 345);
      this.Controls.Add((Control) this.btnOk);
      this.Name = nameof (TDC_Map);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (TDC_Map);
      this.ResumeLayout(false);
    }
  }
}
