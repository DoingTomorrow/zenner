// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.SelectReadoutSettingsForm
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class SelectReadoutSettingsForm : Form
  {
    private ReadoutType readoutType;
    private List<int> selectedReadoutSettingsIDs;
    private List<ReadoutGmmSettings> allSettings;
    private IContainer components = (IContainer) null;
    private CheckedListBox listReadoutSettings;
    private Button btnGoToReadoutSettings;
    private Panel panel1;
    private Label label2;
    private Label lblTitle;
    private Button btnOK;

    public SelectReadoutSettingsForm()
    {
      this.InitializeComponent();
      this.selectedReadoutSettingsIDs = new List<int>();
    }

    private void SelectReadoutSettingsForm_Load(object sender, EventArgs e)
    {
      this.LoadReadoutTypes();
    }

    public static void Show(ReadoutType readoutType)
    {
      SelectReadoutSettingsForm.Show((Form) null, readoutType);
    }

    public static void Show(Form owner, ReadoutType readoutType)
    {
      if (readoutType == null)
        throw new ArgumentNullException(nameof (readoutType));
      using (SelectReadoutSettingsForm readoutSettingsForm = new SelectReadoutSettingsForm())
      {
        if (owner != null)
          readoutSettingsForm.Owner = owner;
        readoutSettingsForm.lblTitle.Text = readoutType.ReadoutDeviceType;
        readoutSettingsForm.readoutType = readoutType;
        int num = (int) readoutSettingsForm.ShowDialog();
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      foreach (ReadoutGmmSettings readoutGmmSettings in (ListBox.ObjectCollection) this.listReadoutSettings.Items)
      {
        if (this.listReadoutSettings.CheckedItems.Contains((object) readoutGmmSettings))
        {
          if (!this.selectedReadoutSettingsIDs.Contains(readoutGmmSettings.ReadoutSettingsID))
            MeterDatabase.AddReadoutType(this.readoutType.ReadoutDeviceTypeID, readoutGmmSettings.ReadoutSettingsID, string.Empty);
        }
        else if (this.selectedReadoutSettingsIDs.Contains(readoutGmmSettings.ReadoutSettingsID))
          MeterDatabase.DeleteReadoutType(this.readoutType.ReadoutDeviceTypeID, readoutGmmSettings.ReadoutSettingsID);
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnGoToReadoutSettings_Click(object sender, EventArgs e)
    {
      AddReadoutSettingsForm.Show((Form) this);
      this.LoadReadoutTypes();
    }

    private void LoadReadoutTypes()
    {
      this.listReadoutSettings.Items.Clear();
      foreach (ReadoutType readoutType in MeterDatabase.LoadReadoutType(new int?(this.readoutType.ReadoutDeviceTypeID)))
      {
        if (readoutType.ReadoutSettingsID != 0)
          this.selectedReadoutSettingsIDs.Add(readoutType.ReadoutSettingsID);
      }
      this.allSettings = MeterDatabase.LoadReadoutSettings();
      if (this.allSettings == null)
        return;
      foreach (ReadoutGmmSettings allSetting in this.allSettings)
      {
        bool isChecked = this.selectedReadoutSettingsIDs.Contains(allSetting.ReadoutSettingsID);
        this.listReadoutSettings.Items.Add((object) allSetting, isChecked);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectReadoutSettingsForm));
      this.listReadoutSettings = new CheckedListBox();
      this.btnGoToReadoutSettings = new Button();
      this.panel1 = new Panel();
      this.label2 = new Label();
      this.lblTitle = new Label();
      this.btnOK = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.listReadoutSettings.CheckOnClick = true;
      this.listReadoutSettings.FormattingEnabled = true;
      this.listReadoutSettings.Location = new Point(8, 72);
      this.listReadoutSettings.Name = "listReadoutSettings";
      this.listReadoutSettings.Size = new Size(519, 259);
      this.listReadoutSettings.TabIndex = 0;
      this.btnGoToReadoutSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGoToReadoutSettings.Location = new Point(538, 72);
      this.btnGoToReadoutSettings.Name = "btnGoToReadoutSettings";
      this.btnGoToReadoutSettings.Size = new Size(76, 23);
      this.btnGoToReadoutSettings.TabIndex = 1;
      this.btnGoToReadoutSettings.Text = "...";
      this.btnGoToReadoutSettings.UseVisualStyleBackColor = true;
      this.btnGoToReadoutSettings.Click += new System.EventHandler(this.btnGoToReadoutSettings_Click);
      this.panel1.BackColor = Color.White;
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.lblTitle);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(620, 64);
      this.panel1.TabIndex = 33;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9f);
      this.label2.ImeMode = ImeMode.NoControl;
      this.label2.Location = new Point(40, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(257, 15);
      this.label2.TabIndex = 1;
      this.label2.Text = "Please select read-out settings for this device. ";
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.ImeMode = ImeMode.NoControl;
      this.lblTitle.Location = new Point(23, 8);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(135, 24);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "{DeviceType}";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Image = (Image) componentResourceManager.GetObject("btnOK.Image");
      this.btnOK.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOK.ImeMode = ImeMode.NoControl;
      this.btnOK.Location = new Point(538, 311);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 23);
      this.btnOK.TabIndex = 34;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(620, 346);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnGoToReadoutSettings);
      this.Controls.Add((Control) this.listReadoutSettings);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (SelectReadoutSettingsForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Readout Settings";
      this.Load += new System.EventHandler(this.SelectReadoutSettingsForm_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
