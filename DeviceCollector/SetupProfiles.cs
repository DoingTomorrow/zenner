// Decompiled with JetBrains decompiler
// Type: DeviceCollector.SetupProfiles
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using CorporateDesign;
using GmmDbLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace DeviceCollector
{
  public class SetupProfiles : Form
  {
    private DbBasis MyTypedDB;
    private IDbConnection MyDbConnection;
    private List<string> SetupProfilesList;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Button buttonOk;
    private Button buttonCancel;
    private ListBox listBoxProfiles;
    private PictureBox pictureBox1;
    private Button buttonShowParameter;

    public SetupProfiles(DeviceCollectorFunctions TheBus)
    {
      this.InitializeComponent();
      this.MyTypedDB = DbBasis.PrimaryDB;
      this.MyDbConnection = this.MyTypedDB.GetDbConnection();
      this.SetupProfilesList = new List<string>();
      this.SetupProfilesList.Add("Bluetooth / MinoConnect / IrCombiHead / Single MBus device");
      this.SetupProfilesList.Add("Bluetooth / MinoConnect / IrCombiHead / Single Serie1-MBus device");
      this.SetupProfilesList.Add("Bluetooth / MinoConnect / IrCombiHead / Single Serie2-MBus device");
      this.SetupProfilesList.Add("Bluetooth / MinoConnect / IrCombiHead / Minol device");
      this.SetupProfilesList.Add("COM interface / Optical head / Single MBus device");
      this.SetupProfilesList.Add("COM interface / Optical head / Single Serie1-MBus device");
      this.SetupProfilesList.Add("COM interface / Optical head / Single Serie2-MBus device");
      this.SetupProfilesList.Add("COM interface / MBus converter / Single MBus device");
      this.SetupProfilesList.Add("COM interface / MBus converter / Single Serie1-MBus device");
      this.SetupProfilesList.Add("COM interface / MBus converter / Single Serie2-MBus device");
      this.SetupProfilesList.Add("COM interface / MBus converter / MBus / MBus device");
      this.SetupProfilesList.Add("COM interface / MBus converter / MBus / Serie2-MBus device");
      this.SetupProfilesList.Add("MeterVPN / ComServer / MBus converter / Single MBus device");
      this.SetupProfilesList.Add("MeterVPN / ComServer / MBus converter / Single Serie2-MBus device");
      this.SetupProfilesList.Add("MeterVPN / ComServer / MBus converter / MBus / MBus device");
      this.SetupProfilesList.Add("MeterVPN / ComServer / MBus converter / MBus / Serie2-MBus device");
      this.listBoxProfiles.DataSource = (object) this.SetupProfilesList;
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
      if (this.listBoxProfiles.SelectedIndex < 0)
        return;
      int num = (int) MessageBox.Show(this.listBoxProfiles.SelectedValue.ToString());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SetupProfiles));
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.buttonOk = new Button();
      this.buttonCancel = new Button();
      this.listBoxProfiles = new ListBox();
      this.pictureBox1 = new PictureBox();
      this.buttonShowParameter = new Button();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Fill;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(663, 458);
      this.zennerCoroprateDesign2.TabIndex = 15;
      this.buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOk.DialogResult = DialogResult.OK;
      this.buttonOk.Location = new Point(559, 423);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new Size(92, 23);
      this.buttonOk.TabIndex = 1;
      this.buttonOk.Text = "Ok";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(456, 423);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(97, 23);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.listBoxProfiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listBoxProfiles.FormattingEnabled = true;
      this.listBoxProfiles.Location = new Point(12, 248);
      this.listBoxProfiles.Name = "listBoxProfiles";
      this.listBoxProfiles.ScrollAlwaysVisible = true;
      this.listBoxProfiles.Size = new Size(638, 160);
      this.listBoxProfiles.TabIndex = 0;
      this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pictureBox1.Location = new Point(12, 60);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(637, 179);
      this.pictureBox1.TabIndex = 18;
      this.pictureBox1.TabStop = false;
      this.buttonShowParameter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonShowParameter.Location = new Point(12, 423);
      this.buttonShowParameter.Name = "buttonShowParameter";
      this.buttonShowParameter.Size = new Size(128, 23);
      this.buttonShowParameter.TabIndex = 3;
      this.buttonShowParameter.Text = "Show parameter";
      this.buttonShowParameter.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(663, 458);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.listBoxProfiles);
      this.Controls.Add((Control) this.buttonShowParameter);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonOk);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimumSize = new Size(500, 400);
      this.Name = nameof (SetupProfiles);
      this.Text = "Setup profiles";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
