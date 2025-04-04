// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.GMM_MessageBox
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class GMM_MessageBox : Form
  {
    private Button button1;
    private TextBox textBoxMessage;
    private Button button2;
    private Label labelError;
    private Button button3;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;

    public GMM_MessageBox() => this.InitializeComponent();

    public void ChangeText(string text) => this.textBoxMessage.Text = text;

    private void GMM_MessageBox_Load(object sender, EventArgs e) => this.BringToFront();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GMM_MessageBox));
      this.button1 = new Button();
      this.textBoxMessage = new TextBox();
      this.button2 = new Button();
      this.labelError = new Label();
      this.button3 = new Button();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.button1, "button1");
      this.button1.Name = "button1";
      componentResourceManager.ApplyResources((object) this.textBoxMessage, "textBoxMessage");
      this.textBoxMessage.BackColor = SystemColors.Control;
      this.textBoxMessage.Name = "textBoxMessage";
      componentResourceManager.ApplyResources((object) this.button2, "button2");
      this.button2.Name = "button2";
      componentResourceManager.ApplyResources((object) this.labelError, "labelError");
      this.labelError.BackColor = Color.Red;
      this.labelError.Name = "labelError";
      componentResourceManager.ApplyResources((object) this.button3, "button3");
      this.button3.Name = "button3";
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign2, "zennerCoroprateDesign2");
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.labelError);
      this.Controls.Add((Control) this.textBoxMessage);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (GMM_MessageBox);
      this.Load += new System.EventHandler(this.GMM_MessageBox_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public static DialogResult ShowMessage(
      string ComponentName,
      GMM_MessageBox.StandardMessages TheStandardMessage)
    {
      using (GMM_MessageBox gmmMessageBox = new GMM_MessageBox())
      {
        gmmMessageBox.Text = ComponentName;
        if (TheStandardMessage == GMM_MessageBox.StandardMessages.IgnoreAllChanges)
        {
          gmmMessageBox.textBoxMessage.Text = ZR_ClassLibMessages.ZR_ClassMessage.GetString("IgnoreAllChanges");
          gmmMessageBox.ManageButtons(MessageBoxButtons.YesNo);
        }
        return gmmMessageBox.ShowDialog();
      }
    }

    public static DialogResult ShowMessage(string ComponentName, string MessageString)
    {
      return GMM_MessageBox.ShowMessage(ComponentName, MessageString, MessageBoxButtons.OK, false, (Font) null);
    }

    public static DialogResult ShowMessage(
      string ComponentName,
      string MessageString,
      bool IsError)
    {
      return GMM_MessageBox.ShowMessage(ComponentName, MessageString, MessageBoxButtons.OK, IsError, (Font) null);
    }

    public static DialogResult ShowMessage(
      string ComponentName,
      string MessageString,
      MessageBoxButtons Buttons)
    {
      return GMM_MessageBox.ShowMessage(ComponentName, MessageString, Buttons, false, (Font) null);
    }

    public static DialogResult ShowMessage(
      string ComponentName,
      string MessageString,
      MessageBoxButtons Buttons,
      bool IsError,
      Font TheFont)
    {
      using (GMM_MessageBox gmmMessageBox = new GMM_MessageBox())
      {
        if (TheFont != null)
          gmmMessageBox.Font = TheFont;
        gmmMessageBox.Text = ComponentName;
        gmmMessageBox.textBoxMessage.Text = MessageString;
        gmmMessageBox.ManageButtons(Buttons);
        if (IsError)
          gmmMessageBox.labelError.Visible = true;
        return gmmMessageBox.ShowDialog();
      }
    }

    private void ManageButtons(MessageBoxButtons Buttons)
    {
      switch (Buttons)
      {
        case MessageBoxButtons.OK:
          this.button1.DialogResult = DialogResult.OK;
          break;
        case MessageBoxButtons.OKCancel:
          this.button2.Visible = true;
          this.button1.DialogResult = DialogResult.OK;
          this.button2.DialogResult = DialogResult.Cancel;
          break;
        case MessageBoxButtons.YesNoCancel:
          this.button2.Visible = true;
          this.button3.Visible = true;
          this.button1.DialogResult = DialogResult.Yes;
          this.button2.DialogResult = DialogResult.No;
          this.button3.DialogResult = DialogResult.Cancel;
          this.button1.Text = ZR_ClassLibMessages.ZR_ClassMessage.GetString("Yes");
          this.button2.Text = ZR_ClassLibMessages.ZR_ClassMessage.GetString("No");
          this.button3.Text = ZR_ClassLibMessages.ZR_ClassMessage.GetString("Cancel");
          break;
        case MessageBoxButtons.YesNo:
          this.button2.Visible = true;
          this.button1.DialogResult = DialogResult.Yes;
          this.button2.DialogResult = DialogResult.No;
          this.button1.Text = ZR_ClassLibMessages.ZR_ClassMessage.GetString("Yes");
          this.button2.Text = ZR_ClassLibMessages.ZR_ClassMessage.GetString("No");
          break;
        default:
          throw new ArgumentException("Not supportet button type");
      }
    }

    public enum StandardMessages
    {
      IgnoreAllChanges,
    }
  }
}
