// Decompiled with JetBrains decompiler
// Type: MinomatHandler.Components.TextBoxesControl
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace MinomatHandler.Components
{
  public class TextBoxesControl : UserControl
  {
    private IContainer components = (IContainer) null;
    internal Label lblValueName1;
    internal Button btnGet;
    internal Label lblTitel;
    internal Button btnSet;
    internal Label lblValueName2;
    internal Label lblValueName3;
    internal Label lblValueName4;
    internal Label label1;
    internal Label lblCMD;
    internal ComboBox txtValue1;
    internal ComboBox txtValue2;
    internal ComboBox txtValue3;
    internal ComboBox txtValue4;
    internal ComboBox txtValue5;
    internal Label lblValueName5;

    public TextBoxesControl() => this.InitializeComponent();

    private void txtValue_TextChanged(object sender, EventArgs e)
    {
      this.txtValue1.BackColor = SystemColors.Window;
      this.txtValue2.BackColor = SystemColors.Window;
      this.txtValue3.BackColor = SystemColors.Window;
      this.txtValue4.BackColor = SystemColors.Window;
      this.txtValue5.BackColor = SystemColors.Window;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TextBoxesControl));
      this.lblValueName1 = new Label();
      this.btnGet = new Button();
      this.lblTitel = new Label();
      this.btnSet = new Button();
      this.lblValueName2 = new Label();
      this.lblValueName3 = new Label();
      this.lblValueName4 = new Label();
      this.label1 = new Label();
      this.lblCMD = new Label();
      this.txtValue1 = new ComboBox();
      this.txtValue2 = new ComboBox();
      this.txtValue3 = new ComboBox();
      this.txtValue4 = new ComboBox();
      this.txtValue5 = new ComboBox();
      this.lblValueName5 = new Label();
      this.SuspendLayout();
      this.lblValueName1.AutoSize = true;
      this.lblValueName1.Location = new Point(7, 83);
      this.lblValueName1.Name = "lblValueName1";
      this.lblValueName1.Size = new Size(80, 13);
      this.lblValueName1.TabIndex = 0;
      this.lblValueName1.Text = "{Value name 1}";
      this.btnGet.Image = (Image) componentResourceManager.GetObject("btnGet.Image");
      this.btnGet.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnGet.Location = new Point(12, 39);
      this.btnGet.Name = "btnGet";
      this.btnGet.Size = new Size(101, 35);
      this.btnGet.TabIndex = 2;
      this.btnGet.Text = "Read";
      this.btnGet.TextAlign = ContentAlignment.MiddleRight;
      this.btnGet.UseVisualStyleBackColor = true;
      this.lblTitel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblTitel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitel.Location = new Point(7, 9);
      this.lblTitel.Name = "lblTitel";
      this.lblTitel.Size = new Size(270, 16);
      this.lblTitel.TabIndex = 3;
      this.lblTitel.Text = "{Titel}";
      this.btnSet.Image = (Image) componentResourceManager.GetObject("btnSet.Image");
      this.btnSet.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSet.Location = new Point((int) sbyte.MaxValue, 39);
      this.btnSet.Name = "btnSet";
      this.btnSet.Size = new Size(101, 35);
      this.btnSet.TabIndex = 5;
      this.btnSet.Text = "Write";
      this.btnSet.TextAlign = ContentAlignment.MiddleRight;
      this.btnSet.UseVisualStyleBackColor = true;
      this.lblValueName2.AutoSize = true;
      this.lblValueName2.Location = new Point(7, 128);
      this.lblValueName2.Name = "lblValueName2";
      this.lblValueName2.Size = new Size(80, 13);
      this.lblValueName2.TabIndex = 8;
      this.lblValueName2.Text = "{Value name 2}";
      this.lblValueName3.AutoSize = true;
      this.lblValueName3.Location = new Point(7, 173);
      this.lblValueName3.Name = "lblValueName3";
      this.lblValueName3.Size = new Size(80, 13);
      this.lblValueName3.TabIndex = 10;
      this.lblValueName3.Text = "{Value name 3}";
      this.lblValueName4.AutoSize = true;
      this.lblValueName4.Location = new Point(7, 218);
      this.lblValueName4.Name = "lblValueName4";
      this.lblValueName4.Size = new Size(80, 13);
      this.lblValueName4.TabIndex = 12;
      this.lblValueName4.Text = "{Value name 4}";
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(283, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(50, 16);
      this.label1.TabIndex = 14;
      this.label1.Text = "CMD:";
      this.lblCMD.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCMD.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCMD.Location = new Point(332, 9);
      this.lblCMD.Name = "lblCMD";
      this.lblCMD.Size = new Size(60, 16);
      this.lblCMD.TabIndex = 15;
      this.lblCMD.Text = "0x0000";
      this.txtValue1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtValue1.FormattingEnabled = true;
      this.txtValue1.Location = new Point(10, 101);
      this.txtValue1.Name = "txtValue1";
      this.txtValue1.Size = new Size(373, 21);
      this.txtValue1.TabIndex = 16;
      this.txtValue2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtValue2.FormattingEnabled = true;
      this.txtValue2.Location = new Point(10, 146);
      this.txtValue2.Name = "txtValue2";
      this.txtValue2.Size = new Size(373, 21);
      this.txtValue2.TabIndex = 17;
      this.txtValue3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtValue3.FormattingEnabled = true;
      this.txtValue3.Location = new Point(10, 191);
      this.txtValue3.Name = "txtValue3";
      this.txtValue3.Size = new Size(373, 21);
      this.txtValue3.TabIndex = 18;
      this.txtValue4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtValue4.FormattingEnabled = true;
      this.txtValue4.Location = new Point(10, 236);
      this.txtValue4.Name = "txtValue4";
      this.txtValue4.Size = new Size(373, 21);
      this.txtValue4.TabIndex = 19;
      this.txtValue5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtValue5.FormattingEnabled = true;
      this.txtValue5.Location = new Point(10, 280);
      this.txtValue5.Name = "txtValue5";
      this.txtValue5.Size = new Size(373, 21);
      this.txtValue5.TabIndex = 21;
      this.lblValueName5.AutoSize = true;
      this.lblValueName5.Location = new Point(7, 262);
      this.lblValueName5.Name = "lblValueName5";
      this.lblValueName5.Size = new Size(80, 13);
      this.lblValueName5.TabIndex = 20;
      this.lblValueName5.Text = "{Value name 5}";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.txtValue5);
      this.Controls.Add((Control) this.lblValueName5);
      this.Controls.Add((Control) this.txtValue4);
      this.Controls.Add((Control) this.txtValue3);
      this.Controls.Add((Control) this.txtValue2);
      this.Controls.Add((Control) this.txtValue1);
      this.Controls.Add((Control) this.lblCMD);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblValueName4);
      this.Controls.Add((Control) this.lblValueName3);
      this.Controls.Add((Control) this.lblValueName2);
      this.Controls.Add((Control) this.btnSet);
      this.Controls.Add((Control) this.btnGet);
      this.Controls.Add((Control) this.lblValueName1);
      this.Controls.Add((Control) this.lblTitel);
      this.Name = nameof (TextBoxesControl);
      this.Size = new Size(401, 429);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
