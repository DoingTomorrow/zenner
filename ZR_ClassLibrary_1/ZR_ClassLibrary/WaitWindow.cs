// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.WaitWindow
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class WaitWindow : Form
  {
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    public Label labelWaitText;
    public ProgressBar progressBarWait2;
    public ProgressBar progressBarWait1;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;

    public WaitWindow() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.labelWaitText = new Label();
      this.progressBarWait2 = new ProgressBar();
      this.progressBarWait1 = new ProgressBar();
      this.SuspendLayout();
      this.zennerCoroprateDesign1.Dock = DockStyle.Fill;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(554, 311);
      this.zennerCoroprateDesign1.TabIndex = 0;
      this.labelWaitText.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelWaitText.Location = new Point(32, 64);
      this.labelWaitText.Name = "labelWaitText";
      this.labelWaitText.Size = new Size(504, 168);
      this.labelWaitText.TabIndex = 1;
      this.labelWaitText.Text = "Wait info";
      this.labelWaitText.TextAlign = ContentAlignment.TopCenter;
      this.progressBarWait2.Location = new Point(24, 280);
      this.progressBarWait2.Name = "progressBarWait2";
      this.progressBarWait2.Size = new Size(520, 24);
      this.progressBarWait2.TabIndex = 2;
      this.progressBarWait1.Location = new Point(24, 248);
      this.progressBarWait1.Name = "progressBarWait1";
      this.progressBarWait1.Size = new Size(520, 24);
      this.progressBarWait1.TabIndex = 2;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(554, 311);
      this.ControlBox = false;
      this.Controls.Add((Control) this.labelWaitText);
      this.Controls.Add((Control) this.progressBarWait2);
      this.Controls.Add((Control) this.progressBarWait1);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = nameof (WaitWindow);
      this.Text = "Please Wait ...";
      this.ResumeLayout(false);
    }
  }
}
