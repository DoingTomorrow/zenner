// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Portal
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class Portal : Form
  {
    public string endtext;
    private Label pl1;
    private Button OkButton;
    private Label pl2;
    private Button myCancelButton;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private TextBox PortalLicenseTB;
    private TextBox VorgabeTextBoxAll;
    private Button buttonHelp;
    private Button buttonLoadFromSoftwareServiceSystem;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;

    public Portal(string s1, string s2, string s3)
    {
      this.InitializeComponent();
      this.endtext = "Looser";
      string InputString = "" + ParameterService.GetCharacterCode((int) UserRights.LICENSE_CODE_VERSION_PC).ToString() + s1 + s2 + s3;
      this.VorgabeTextBoxAll.Text = UserRights.GetSeparatedString(InputString + UserRights.GetStringCS(InputString));
      this.Text = ZR_ClassLibMessages.ZR_ClassMessage.GetString("1");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Portal));
      this.pl1 = new Label();
      this.OkButton = new Button();
      this.myCancelButton = new Button();
      this.pl2 = new Label();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.buttonHelp = new Button();
      this.PortalLicenseTB = new TextBox();
      this.VorgabeTextBoxAll = new TextBox();
      this.buttonLoadFromSoftwareServiceSystem = new Button();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.pl1, "pl1");
      this.pl1.Name = "pl1";
      this.OkButton.BackColor = SystemColors.Control;
      this.OkButton.DialogResult = DialogResult.OK;
      componentResourceManager.ApplyResources((object) this.OkButton, "OkButton");
      this.OkButton.Name = "OkButton";
      this.OkButton.UseVisualStyleBackColor = false;
      this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
      this.myCancelButton.BackColor = SystemColors.Control;
      this.myCancelButton.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.myCancelButton, "myCancelButton");
      this.myCancelButton.Name = "myCancelButton";
      this.myCancelButton.UseVisualStyleBackColor = false;
      this.myCancelButton.Click += new System.EventHandler(this.myCancelButton_Click);
      componentResourceManager.ApplyResources((object) this.pl2, "pl2");
      this.pl2.Name = "pl2";
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.buttonHelp.BackColor = SystemColors.Control;
      componentResourceManager.ApplyResources((object) this.buttonHelp, "buttonHelp");
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.UseVisualStyleBackColor = false;
      this.buttonHelp.Click += new System.EventHandler(this.buttonEmail_Click);
      componentResourceManager.ApplyResources((object) this.PortalLicenseTB, "PortalLicenseTB");
      this.PortalLicenseTB.Name = "PortalLicenseTB";
      this.PortalLicenseTB.KeyPress += new KeyPressEventHandler(this.PortalLicenseTB_KeyPress);
      componentResourceManager.ApplyResources((object) this.VorgabeTextBoxAll, "VorgabeTextBoxAll");
      this.VorgabeTextBoxAll.Name = "VorgabeTextBoxAll";
      this.VorgabeTextBoxAll.ReadOnly = true;
      this.buttonLoadFromSoftwareServiceSystem.BackColor = SystemColors.Control;
      componentResourceManager.ApplyResources((object) this.buttonLoadFromSoftwareServiceSystem, "buttonLoadFromSoftwareServiceSystem");
      this.buttonLoadFromSoftwareServiceSystem.Name = "buttonLoadFromSoftwareServiceSystem";
      this.buttonLoadFromSoftwareServiceSystem.UseVisualStyleBackColor = false;
      this.buttonLoadFromSoftwareServiceSystem.Click += new System.EventHandler(this.buttonLoadFromSoftwareServiceSystem_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.VorgabeTextBoxAll);
      this.Controls.Add((Control) this.PortalLicenseTB);
      this.Controls.Add((Control) this.buttonHelp);
      this.Controls.Add((Control) this.pl2);
      this.Controls.Add((Control) this.myCancelButton);
      this.Controls.Add((Control) this.buttonLoadFromSoftwareServiceSystem);
      this.Controls.Add((Control) this.OkButton);
      this.Controls.Add((Control) this.pl1);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Portal);
      this.Load += new System.EventHandler(this.Portal_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void OkButton_Click(object sender, EventArgs e) => this.EndOfWork();

    private void EndOfWork()
    {
      if (!(this.PortalLicenseTB.Text != ""))
        return;
      this.endtext = this.PortalLicenseTB.Text;
    }

    private void Portal_Load(object sender, EventArgs e)
    {
      this.pl2.Text = ZR_ClassLibMessages.ZR_ClassMessage.GetString("7");
      this.pl1.Text = ZR_ClassLibMessages.ZR_ClassMessage.GetString("8");
      this.BringToFront();
    }

    private void buttonEmail_Click(object sender, EventArgs e)
    {
      string ietfLanguageTag = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
      string[] strArray = ietfLanguageTag.Split('-');
      if (strArray.GetLength(0) > 1)
        ietfLanguageTag = strArray[0];
      try
      {
        new Process()
        {
          StartInfo = {
            FileName = string.Format("http://zenner.softwareservicesystem.com/downloads/helpfiles/SSS.{0}.pdf", (object) ietfLanguageTag)
          }
        }.Start();
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("Global Meter Manager", "Konnte die Hilfedatei nicht herunterladen. Bitte starten Sie Ihren Browser, und laden Sie die Datei manuell herunter:" + Environment.NewLine + string.Format("http://zenner.softwareservicesystem.com/downloads/helpfiles/SSS.{0}.pdf", (object) ietfLanguageTag), true);
      }
    }

    private void PortalLicenseTB_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(e.ToString() == Keys.Return.ToString()))
        return;
      this.EndOfWork();
    }

    private void buttonLoadFromSoftwareServiceSystem_Click(object sender, EventArgs e)
    {
      try
      {
        new Process()
        {
          StartInfo = {
            FileName = "http://zenner.softwareservicesystem.com/"
          }
        }.Start();
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Global Meter Manager", ex.ToString(), true);
      }
    }

    private void myCancelButton_Click(object sender, EventArgs e)
    {
    }
  }
}
