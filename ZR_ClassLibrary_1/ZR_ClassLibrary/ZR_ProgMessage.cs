// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ZR_ProgMessage
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ZR_ProgMessage : Form
  {
    private PictureBox pictureBox1;
    private Label label1;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;

    public ZR_ProgMessage()
    {
      this.InitializeComponent();
      this.Cursor = Cursors.WaitCursor;
    }

    protected override void Dispose(bool disposing)
    {
      this.Cursor = Cursors.Default;
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (ZR_ProgMessage));
      this.pictureBox1 = new PictureBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.pictureBox1.AccessibleDescription = resourceManager.GetString("pictureBox1.AccessibleDescription");
      this.pictureBox1.AccessibleName = resourceManager.GetString("pictureBox1.AccessibleName");
      this.pictureBox1.Anchor = (AnchorStyles) resourceManager.GetObject("pictureBox1.Anchor");
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.BackgroundImage = (Image) resourceManager.GetObject("pictureBox1.BackgroundImage");
      this.pictureBox1.Dock = (DockStyle) resourceManager.GetObject("pictureBox1.Dock");
      this.pictureBox1.Enabled = (bool) resourceManager.GetObject("pictureBox1.Enabled");
      this.pictureBox1.Font = (Font) resourceManager.GetObject("pictureBox1.Font");
      this.pictureBox1.Image = (Image) resourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.ImeMode = (ImeMode) resourceManager.GetObject("pictureBox1.ImeMode");
      this.pictureBox1.Location = (Point) resourceManager.GetObject("pictureBox1.Location");
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.RightToLeft = (RightToLeft) resourceManager.GetObject("pictureBox1.RightToLeft");
      this.pictureBox1.Size = (Size) resourceManager.GetObject("pictureBox1.Size");
      this.pictureBox1.SizeMode = (PictureBoxSizeMode) resourceManager.GetObject("pictureBox1.SizeMode");
      this.pictureBox1.TabIndex = (int) resourceManager.GetObject("pictureBox1.TabIndex");
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Text = resourceManager.GetString("pictureBox1.Text");
      this.pictureBox1.Visible = (bool) resourceManager.GetObject("pictureBox1.Visible");
      this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
      this.label1.AccessibleDescription = resourceManager.GetString("label1.AccessibleDescription");
      this.label1.AccessibleName = resourceManager.GetString("label1.AccessibleName");
      this.label1.Anchor = (AnchorStyles) resourceManager.GetObject("label1.Anchor");
      this.label1.AutoSize = (bool) resourceManager.GetObject("label1.AutoSize");
      this.label1.BackColor = Color.White;
      this.label1.Dock = (DockStyle) resourceManager.GetObject("label1.Dock");
      this.label1.Enabled = (bool) resourceManager.GetObject("label1.Enabled");
      this.label1.Font = (Font) resourceManager.GetObject("label1.Font");
      this.label1.ForeColor = Color.Red;
      this.label1.Image = (Image) resourceManager.GetObject("label1.Image");
      this.label1.ImageAlign = (ContentAlignment) resourceManager.GetObject("label1.ImageAlign");
      this.label1.ImageIndex = (int) resourceManager.GetObject("label1.ImageIndex");
      this.label1.ImeMode = (ImeMode) resourceManager.GetObject("label1.ImeMode");
      this.label1.Location = (Point) resourceManager.GetObject("label1.Location");
      this.label1.Name = "label1";
      this.label1.RightToLeft = (RightToLeft) resourceManager.GetObject("label1.RightToLeft");
      this.label1.Size = (Size) resourceManager.GetObject("label1.Size");
      this.label1.TabIndex = (int) resourceManager.GetObject("label1.TabIndex");
      this.label1.Text = resourceManager.GetString("label1.Text");
      this.label1.TextAlign = (ContentAlignment) resourceManager.GetObject("label1.TextAlign");
      this.label1.Visible = (bool) resourceManager.GetObject("label1.Visible");
      this.label1.Click += new System.EventHandler(this.label1_Click);
      this.AccessibleDescription = resourceManager.GetString("$this.AccessibleDescription");
      this.AccessibleName = resourceManager.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = (Size) resourceManager.GetObject("$this.AutoScaleBaseSize");
      this.AutoScroll = (bool) resourceManager.GetObject("$this.AutoScroll");
      this.AutoScrollMargin = (Size) resourceManager.GetObject("$this.AutoScrollMargin");
      this.AutoScrollMinSize = (Size) resourceManager.GetObject("$this.AutoScrollMinSize");
      this.BackColor = Color.White;
      this.BackgroundImage = (Image) resourceManager.GetObject("$this.BackgroundImage");
      this.ClientSize = (Size) resourceManager.GetObject("$this.ClientSize");
      this.ControlBox = false;
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.label1);
      this.Enabled = (bool) resourceManager.GetObject("$this.Enabled");
      this.Font = (Font) resourceManager.GetObject("$this.Font");
      this.Icon = (Icon) resourceManager.GetObject("$this.Icon");
      this.ImeMode = (ImeMode) resourceManager.GetObject("$this.ImeMode");
      this.Location = (Point) resourceManager.GetObject("$this.Location");
      this.MaximizeBox = false;
      this.MaximumSize = (Size) resourceManager.GetObject("$this.MaximumSize");
      this.MinimizeBox = false;
      this.MinimumSize = (Size) resourceManager.GetObject("$this.MinimumSize");
      this.Name = nameof (ZR_ProgMessage);
      this.RightToLeft = (RightToLeft) resourceManager.GetObject("$this.RightToLeft");
      this.ShowInTaskbar = false;
      this.StartPosition = (FormStartPosition) resourceManager.GetObject("$this.StartPosition");
      this.Text = resourceManager.GetString("$this.Text");
      this.ResumeLayout(false);
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }
  }
}
