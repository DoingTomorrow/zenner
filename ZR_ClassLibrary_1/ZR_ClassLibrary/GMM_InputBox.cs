// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.GMM_InputBox
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class GMM_InputBox : Form
  {
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Panel panel1;
    private Button buttonOk;
    private Button buttonCancel;
    private TextBox textBoxInputText;
    private TextBox textBoxMessage;

    public GMM_InputBox() => this.InitializeComponent();

    public static string GetInput(string ComponentName, string MessageString, string DefaultInput)
    {
      GMM_InputBox gmmInputBox = new GMM_InputBox();
      gmmInputBox.Text = ComponentName;
      gmmInputBox.textBoxMessage.Text = MessageString;
      gmmInputBox.textBoxInputText.Text = DefaultInput;
      return gmmInputBox.ShowDialog() == DialogResult.Cancel ? string.Empty : gmmInputBox.textBoxInputText.Text;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GMM_InputBox));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.panel1 = new Panel();
      this.textBoxInputText = new TextBox();
      this.textBoxMessage = new TextBox();
      this.buttonOk = new Button();
      this.buttonCancel = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign1.Dock = DockStyle.Fill;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(533, 244);
      this.zennerCoroprateDesign1.TabIndex = 1;
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.textBoxInputText);
      this.panel1.Controls.Add((Control) this.textBoxMessage);
      this.panel1.Controls.Add((Control) this.buttonOk);
      this.panel1.Controls.Add((Control) this.buttonCancel);
      this.panel1.Location = new Point(12, 41);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(521, 203);
      this.panel1.TabIndex = 2;
      this.textBoxInputText.Location = new Point(13, 130);
      this.textBoxInputText.Multiline = true;
      this.textBoxInputText.Name = "textBoxInputText";
      this.textBoxInputText.Size = new Size(388, 59);
      this.textBoxInputText.TabIndex = 5;
      this.textBoxMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxMessage.Enabled = false;
      this.textBoxMessage.Location = new Point(12, 12);
      this.textBoxMessage.Multiline = true;
      this.textBoxMessage.Name = "textBoxMessage";
      this.textBoxMessage.Size = new Size(496, 106);
      this.textBoxMessage.TabIndex = 4;
      this.buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOk.DialogResult = DialogResult.OK;
      this.buttonOk.ImeMode = ImeMode.NoControl;
      this.buttonOk.Location = new Point(429, 167);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new Size(80, 23);
      this.buttonOk.TabIndex = 3;
      this.buttonOk.Text = "ok";
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.ImeMode = ImeMode.NoControl;
      this.buttonCancel.Location = new Point(429, 135);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(80, 23);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "cancel";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(533, 244);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (GMM_InputBox);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (GMM_InputBox);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
