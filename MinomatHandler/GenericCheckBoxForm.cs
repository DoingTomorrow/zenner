// Decompiled with JetBrains decompiler
// Type: MinomatHandler.GenericCheckBoxForm
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace MinomatHandler
{
  public class GenericCheckBoxForm : Form
  {
    private IContainer components = (IContainer) null;
    private Button btnOK;
    internal CheckedListBox items;

    public GenericCheckBoxForm() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GenericCheckBoxForm));
      this.items = new CheckedListBox();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.items.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.items.CheckOnClick = true;
      this.items.FormattingEnabled = true;
      this.items.HorizontalExtent = 100;
      this.items.HorizontalScrollbar = true;
      this.items.Location = new Point(0, 0);
      this.items.MultiColumn = true;
      this.items.Name = "items";
      this.items.Size = new Size(639, 364);
      this.items.Sorted = true;
      this.items.TabIndex = 0;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(556, 375);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(639, 403);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.items);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (GenericCheckBoxForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Please select items";
      this.ResumeLayout(false);
    }
  }
}
