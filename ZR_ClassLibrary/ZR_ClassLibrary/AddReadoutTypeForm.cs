// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.AddReadoutTypeForm
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class AddReadoutTypeForm : Form
  {
    private IContainer components = (IContainer) null;
    private Button btnCancel;
    private TextBox txtName;
    private Label label2;
    private Button btnOK;

    public AddReadoutTypeForm() => this.InitializeComponent();

    public static string Show() => AddReadoutTypeForm.Show((Form) null, (string) null);

    public static string Show(Form owner) => AddReadoutTypeForm.Show(owner, (string) null);

    public static string Show(Form owner, string name)
    {
      using (AddReadoutTypeForm addReadoutTypeForm = new AddReadoutTypeForm())
      {
        if (owner != null)
          addReadoutTypeForm.Owner = owner;
        if (name != null)
          addReadoutTypeForm.txtName.Text = name;
        return addReadoutTypeForm.ShowDialog() != DialogResult.OK ? (string) null : addReadoutTypeForm.txtName.Text;
      }
    }

    private void AddReadoutTypeForm_Load(object sender, EventArgs e) => this.txtName.Focus();

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddReadoutTypeForm));
      this.btnCancel = new Button();
      this.txtName = new TextBox();
      this.label2 = new Label();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(141, 71);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.txtName.Location = new Point(12, 35);
      this.txtName.MaxLength = 50;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(231, 20);
      this.txtName.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.ImeMode = ImeMode.NoControl;
      this.label2.Location = new Point(12, 18);
      this.label2.Name = "label2";
      this.label2.Size = new Size(35, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Name";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Image = (Image) componentResourceManager.GetObject("btnOK.Image");
      this.btnOK.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOK.ImeMode = ImeMode.NoControl;
      this.btnOK.Location = new Point(42, 71);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size((int) byte.MaxValue, 106);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddReadoutTypeForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Readout Type";
      this.Load += new System.EventHandler(this.AddReadoutTypeForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
