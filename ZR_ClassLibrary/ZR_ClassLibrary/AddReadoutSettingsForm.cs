// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.AddReadoutSettingsForm
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class AddReadoutSettingsForm : Form
  {
    private GMMSettings currentSettings;
    private int? currentReadoutSettingsID;
    private IContainer components = (IContainer) null;
    private DataGridView tableReadoutSettings;
    private Button btnNew;
    private Button btnSave;
    private Button btnRemove;
    private Panel panel1;
    private Label label2;
    private Label label3;
    private Button btnOK;
    private GroupBox groupBox1;
    private Label lblDescriptionDE;
    private TextBox txtDescriptionDE;
    private Label lblDescriptionEN;
    private Button btnGoToAsyncCom;
    private Label lblAsyncComSettings;
    private TextBox txtDescriptionEN;
    private TextBox txtAsyncComSettings;
    private TextBox txtDeviceCollectorSettings;
    private Label lblDeviceCollectorSettings;
    private Button btnGoToDeviceCollector;
    private Label label7;
    private DataGridViewTextBoxColumn colReadoutDeviceSettings;
    private ErrorProvider error;

    public AddReadoutSettingsForm() => this.InitializeComponent();

    public static void Show() => AddReadoutSettingsForm.Show((Form) null);

    public static void Show(Form owner)
    {
      using (AddReadoutSettingsForm readoutSettingsForm = new AddReadoutSettingsForm())
      {
        if (owner != null)
          readoutSettingsForm.Owner = owner;
        int num = (int) readoutSettingsForm.ShowDialog();
      }
    }

    private void AddReadoutSettingsForm_Load(object sender, EventArgs e)
    {
      this.LoadReadoutSettings();
    }

    private void tableReadoutSettings_SelectionChanged(object sender, EventArgs e)
    {
      if (this.tableReadoutSettings.SelectedRows.Count != 1)
      {
        this.txtDescriptionDE.Text = string.Empty;
        this.txtDescriptionEN.Text = string.Empty;
        this.txtAsyncComSettings.Text = string.Empty;
        this.txtDeviceCollectorSettings.Text = string.Empty;
        this.currentSettings = (GMMSettings) null;
        this.currentReadoutSettingsID = new int?();
      }
      else
      {
        if (!(this.tableReadoutSettings.SelectedRows[0].Tag is ReadoutGmmSettings tag))
          return;
        this.currentSettings = new GMMSettings();
        this.currentSettings.SetSettings(tag.Settings);
        this.currentReadoutSettingsID = new int?(tag.ReadoutSettingsID);
        this.UpdateSelectedSettings();
      }
    }

    private void btnGoToAsyncCom_Click(object sender, EventArgs e)
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.AsyncCom);
      if (!(ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.AsyncCom] is IWindow loadedComponents) || !(loadedComponents.ShowWindow((object) this.txtAsyncComSettings.Text) is SortedList<AsyncComSettings, object> newAsyncComSettings))
        return;
      if (this.currentSettings == null)
        this.currentSettings = new GMMSettings();
      this.currentSettings.SetAsyncComSettings(newAsyncComSettings);
      this.UpdateSelectedSettings();
    }

    private void btnGoToDeviceCollector_Click(object sender, EventArgs e)
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
      if (!(ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector] is IWindow loadedComponents) || !(loadedComponents.ShowWindow((object) this.txtDeviceCollectorSettings.Text) is SortedList<DeviceCollectorSettings, object> deviceCollectorSettings))
        return;
      if (this.currentSettings == null)
        this.currentSettings = new GMMSettings();
      this.currentSettings.SetDeviceCollectorSettings(deviceCollectorSettings);
      this.UpdateSelectedSettings();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.error.Clear();
      bool flag = true;
      if (this.currentSettings == null)
      {
        this.error.SetError((Control) this.lblAsyncComSettings, "Can not be empty!");
        this.error.SetError((Control) this.lblDeviceCollectorSettings, "Can not be empty!");
        flag = false;
      }
      if (string.IsNullOrEmpty(this.txtDescriptionDE.Text))
      {
        this.error.SetError((Control) this.lblDescriptionDE, "Can not be empty!");
        flag = false;
      }
      if (string.IsNullOrEmpty(this.txtDescriptionEN.Text))
      {
        this.error.SetError((Control) this.lblDescriptionEN, "Can not be empty!");
        flag = false;
      }
      if (!flag)
        return;
      string communicationSettings = this.currentSettings.GetCommunicationSettings();
      if (string.IsNullOrEmpty(communicationSettings))
        return;
      if (!this.currentReadoutSettingsID.HasValue)
      {
        ReadoutGmmSettings readoutGmmSettings = MeterDatabase.AddReadoutSettings(communicationSettings);
        if (readoutGmmSettings != null)
        {
          this.currentReadoutSettingsID = new int?(readoutGmmSettings.ReadoutSettingsID);
        }
        else
        {
          ZR_ClassLibMessages.ShowAndClearErrors();
          return;
        }
      }
      else
        MeterDatabase.UpdateReadoutSettings(this.currentReadoutSettingsID.Value, communicationSettings);
      if (!this.currentReadoutSettingsID.HasValue)
        ;
      int? readoutSettingsId = this.currentReadoutSettingsID;
      this.LoadReadoutSettings();
      this.SelectReadoutSettings(readoutSettingsId);
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      this.tableReadoutSettings.ClearSelection();
      this.error.Clear();
      this.txtDescriptionDE.Text = string.Empty;
      this.txtDescriptionEN.Text = string.Empty;
      this.txtAsyncComSettings.Text = string.Empty;
      this.txtDeviceCollectorSettings.Text = string.Empty;
      this.currentSettings = (GMMSettings) null;
      this.currentReadoutSettingsID = new int?();
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (!this.currentReadoutSettingsID.HasValue || DialogResult.Yes != MessageBox.Show((IWin32Window) this, this.GetTranslatedLanguageText("MeterInstaller", "ConfirmReally"), this.GetTranslatedLanguageText("MeterReader", "Delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
        return;
      MeterDatabase.DeleteReadoutTypeBySettingsID(this.currentReadoutSettingsID.Value);
      MeterDatabase.DeleteReadoutSettings(this.currentReadoutSettingsID.Value);
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.LoadReadoutSettings();
    }

    private void LoadReadoutSettings()
    {
      this.tableReadoutSettings.Rows.Clear();
      List<ReadoutGmmSettings> readoutGmmSettingsList = MeterDatabase.LoadReadoutSettings();
      ZR_ClassLibMessages.ShowAndClearErrors();
      if (readoutGmmSettingsList == null || readoutGmmSettingsList.Count == 0)
        return;
      foreach (ReadoutGmmSettings readoutGmmSettings in readoutGmmSettingsList)
        this.tableReadoutSettings.Rows[this.tableReadoutSettings.Rows.Add(new object[1]
        {
          (object) this.GetTranslatedLanguageText("ReadoutSettingsID", readoutGmmSettings.ReadoutSettingsID.ToString())
        })].Tag = (object) readoutGmmSettings;
      this.tableReadoutSettings.ClearSelection();
    }

    private void UpdateSelectedSettings()
    {
      this.txtAsyncComSettings.Text = string.Empty;
      this.txtDeviceCollectorSettings.Text = string.Empty;
      if (!this.currentReadoutSettingsID.HasValue)
        ;
      if (this.currentSettings == null)
        return;
      this.txtAsyncComSettings.Text = this.currentSettings.GetAsyncComSettingsString();
      this.txtDeviceCollectorSettings.Text = this.currentSettings.GetDeviceCollectorSettingsString();
    }

    private void SelectReadoutSettings(int? readoutSettingsID)
    {
      this.tableReadoutSettings.ClearSelection();
      if (!readoutSettingsID.HasValue)
        return;
      foreach (DataGridViewRow row in (IEnumerable) this.tableReadoutSettings.Rows)
      {
        if ((row.Tag as ReadoutGmmSettings).ReadoutSettingsID == readoutSettingsID.Value)
        {
          row.Selected = true;
          break;
        }
      }
    }

    private string GetTranslatedLanguageText(string GmmModule, string TextKey)
    {
      string str = GmmModule + TextKey;
      return Ot.Gtt(Tg.Common, str, str);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddReadoutSettingsForm));
      this.tableReadoutSettings = new DataGridView();
      this.colReadoutDeviceSettings = new DataGridViewTextBoxColumn();
      this.btnNew = new Button();
      this.btnSave = new Button();
      this.btnRemove = new Button();
      this.panel1 = new Panel();
      this.label2 = new Label();
      this.label3 = new Label();
      this.btnOK = new Button();
      this.groupBox1 = new GroupBox();
      this.lblDescriptionDE = new Label();
      this.txtDescriptionDE = new TextBox();
      this.lblDescriptionEN = new Label();
      this.btnGoToAsyncCom = new Button();
      this.lblAsyncComSettings = new Label();
      this.txtDescriptionEN = new TextBox();
      this.txtAsyncComSettings = new TextBox();
      this.txtDeviceCollectorSettings = new TextBox();
      this.lblDeviceCollectorSettings = new Label();
      this.btnGoToDeviceCollector = new Button();
      this.label7 = new Label();
      this.error = new ErrorProvider(this.components);
      ((ISupportInitialize) this.tableReadoutSettings).BeginInit();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.error).BeginInit();
      this.SuspendLayout();
      this.tableReadoutSettings.AllowUserToAddRows = false;
      this.tableReadoutSettings.AllowUserToDeleteRows = false;
      this.tableReadoutSettings.AllowUserToResizeRows = false;
      this.tableReadoutSettings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.tableReadoutSettings.BackgroundColor = Color.White;
      this.tableReadoutSettings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.tableReadoutSettings.ColumnHeadersVisible = false;
      this.tableReadoutSettings.Columns.AddRange((DataGridViewColumn) this.colReadoutDeviceSettings);
      this.tableReadoutSettings.Dock = DockStyle.Fill;
      this.tableReadoutSettings.Location = new Point(3, 16);
      this.tableReadoutSettings.MultiSelect = false;
      this.tableReadoutSettings.Name = "tableReadoutSettings";
      this.tableReadoutSettings.ReadOnly = true;
      this.tableReadoutSettings.RowHeadersVisible = false;
      this.tableReadoutSettings.ScrollBars = ScrollBars.Vertical;
      this.tableReadoutSettings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.tableReadoutSettings.Size = new Size(621, 166);
      this.tableReadoutSettings.TabIndex = 28;
      this.tableReadoutSettings.SelectionChanged += new System.EventHandler(this.tableReadoutSettings_SelectionChanged);
      this.colReadoutDeviceSettings.HeaderText = "ReadoutDeviceSettings";
      this.colReadoutDeviceSettings.Name = "colReadoutDeviceSettings";
      this.colReadoutDeviceSettings.ReadOnly = true;
      this.btnNew.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnNew.Image = (Image) componentResourceManager.GetObject("btnNew.Image");
      this.btnNew.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnNew.ImeMode = ImeMode.NoControl;
      this.btnNew.Location = new Point(13, 500);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(76, 27);
      this.btnNew.TabIndex = 7;
      this.btnNew.Text = "New";
      this.btnNew.TextAlign = ContentAlignment.MiddleRight;
      this.btnNew.UseVisualStyleBackColor = true;
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.ImeMode = ImeMode.NoControl;
      this.btnSave.Location = new Point(95, 500);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(76, 27);
      this.btnSave.TabIndex = 8;
      this.btnSave.Text = "Save";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnRemove.Image = (Image) componentResourceManager.GetObject("btnRemove.Image");
      this.btnRemove.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnRemove.ImeMode = ImeMode.NoControl;
      this.btnRemove.Location = new Point(211, 500);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(76, 27);
      this.btnRemove.TabIndex = 9;
      this.btnRemove.Text = "Remove";
      this.btnRemove.TextAlign = ContentAlignment.MiddleRight;
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      this.panel1.BackColor = Color.White;
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(646, 64);
      this.panel1.TabIndex = 32;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9f);
      this.label2.ImeMode = ImeMode.NoControl;
      this.label2.Location = new Point(40, 32);
      this.label2.Name = "label2";
      this.label2.Size = new Size(218, 15);
      this.label2.TabIndex = 1;
      this.label2.Text = "Tool to create default read-out settings.";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold);
      this.label3.ImeMode = ImeMode.NoControl;
      this.label3.Location = new Point(23, 13);
      this.label3.Name = "label3";
      this.label3.Size = new Size(179, 16);
      this.label3.TabIndex = 0;
      this.label3.Text = "Default read-out Settings";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.Cancel;
      this.btnOK.Image = (Image) componentResourceManager.GetObject("btnOK.Image");
      this.btnOK.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOK.ImeMode = ImeMode.NoControl;
      this.btnOK.Location = new Point(561, 500);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 27);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "OK";
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.tableReadoutSettings);
      this.groupBox1.Location = new Point(10, 70);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(627, 185);
      this.groupBox1.TabIndex = 35;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Type";
      this.lblDescriptionDE.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblDescriptionDE.AutoSize = true;
      this.lblDescriptionDE.Location = new Point(9, 258);
      this.lblDescriptionDE.Name = "lblDescriptionDE";
      this.lblDescriptionDE.Size = new Size(78, 13);
      this.lblDescriptionDE.TabIndex = 36;
      this.lblDescriptionDE.Text = "Description DE";
      this.txtDescriptionDE.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescriptionDE.Location = new Point(12, 274);
      this.txtDescriptionDE.MaxLength = (int) byte.MaxValue;
      this.txtDescriptionDE.Multiline = true;
      this.txtDescriptionDE.Name = "txtDescriptionDE";
      this.txtDescriptionDE.Size = new Size(625, 21);
      this.txtDescriptionDE.TabIndex = 1;
      this.lblDescriptionEN.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblDescriptionEN.AutoSize = true;
      this.lblDescriptionEN.Location = new Point(7, 300);
      this.lblDescriptionEN.Name = "lblDescriptionEN";
      this.lblDescriptionEN.Size = new Size(78, 13);
      this.lblDescriptionEN.TabIndex = 38;
      this.lblDescriptionEN.Text = "Description EN";
      this.btnGoToAsyncCom.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnGoToAsyncCom.Location = new Point(600, 430);
      this.btnGoToAsyncCom.Name = "btnGoToAsyncCom";
      this.btnGoToAsyncCom.Size = new Size(37, 23);
      this.btnGoToAsyncCom.TabIndex = 6;
      this.btnGoToAsyncCom.Text = "...";
      this.btnGoToAsyncCom.UseVisualStyleBackColor = true;
      this.btnGoToAsyncCom.Click += new System.EventHandler(this.btnGoToAsyncCom_Click);
      this.lblAsyncComSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblAsyncComSettings.AutoSize = true;
      this.lblAsyncComSettings.Location = new Point(7, 416);
      this.lblAsyncComSettings.Name = "lblAsyncComSettings";
      this.lblAsyncComSettings.Size = new Size(98, 13);
      this.lblAsyncComSettings.TabIndex = 41;
      this.lblAsyncComSettings.Text = "AsyncCom Settings";
      this.txtDescriptionEN.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescriptionEN.Location = new Point(10, 316);
      this.txtDescriptionEN.MaxLength = (int) byte.MaxValue;
      this.txtDescriptionEN.Multiline = true;
      this.txtDescriptionEN.Name = "txtDescriptionEN";
      this.txtDescriptionEN.Size = new Size(627, 21);
      this.txtDescriptionEN.TabIndex = 2;
      this.txtAsyncComSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAsyncComSettings.Location = new Point(10, 432);
      this.txtAsyncComSettings.Multiline = true;
      this.txtAsyncComSettings.Name = "txtAsyncComSettings";
      this.txtAsyncComSettings.Size = new Size(584, 53);
      this.txtAsyncComSettings.TabIndex = 5;
      this.txtDeviceCollectorSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDeviceCollectorSettings.Location = new Point(10, 360);
      this.txtDeviceCollectorSettings.Multiline = true;
      this.txtDeviceCollectorSettings.Name = "txtDeviceCollectorSettings";
      this.txtDeviceCollectorSettings.Size = new Size(584, 53);
      this.txtDeviceCollectorSettings.TabIndex = 3;
      this.lblDeviceCollectorSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblDeviceCollectorSettings.AutoSize = true;
      this.lblDeviceCollectorSettings.Location = new Point(7, 344);
      this.lblDeviceCollectorSettings.Name = "lblDeviceCollectorSettings";
      this.lblDeviceCollectorSettings.Size = new Size(123, 13);
      this.lblDeviceCollectorSettings.TabIndex = 45;
      this.lblDeviceCollectorSettings.Text = "DeviceCollector Settings";
      this.btnGoToDeviceCollector.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnGoToDeviceCollector.Location = new Point(600, 358);
      this.btnGoToDeviceCollector.Name = "btnGoToDeviceCollector";
      this.btnGoToDeviceCollector.Size = new Size(37, 23);
      this.btnGoToDeviceCollector.TabIndex = 4;
      this.btnGoToDeviceCollector.Text = "...";
      this.btnGoToDeviceCollector.UseVisualStyleBackColor = true;
      this.btnGoToDeviceCollector.Click += new System.EventHandler(this.btnGoToDeviceCollector_Click);
      this.label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(158, 257);
      this.label7.Name = "label7";
      this.label7.Size = new Size(198, 13);
      this.label7.TabIndex = 47;
      this.label7.Text = "( BusMode + Transponder + Equipment )";
      this.error.ContainerControl = (ContainerControl) this;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = new Size(646, 539);
      this.Controls.Add((Control) this.txtDeviceCollectorSettings);
      this.Controls.Add((Control) this.lblDeviceCollectorSettings);
      this.Controls.Add((Control) this.txtAsyncComSettings);
      this.Controls.Add((Control) this.txtDescriptionEN);
      this.Controls.Add((Control) this.btnGoToDeviceCollector);
      this.Controls.Add((Control) this.lblAsyncComSettings);
      this.Controls.Add((Control) this.btnGoToAsyncCom);
      this.Controls.Add((Control) this.lblDescriptionEN);
      this.Controls.Add((Control) this.txtDescriptionDE);
      this.Controls.Add((Control) this.lblDescriptionDE);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnNew);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.label7);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (AddReadoutSettingsForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Default Read-out Settings";
      this.Load += new System.EventHandler(this.AddReadoutSettingsForm_Load);
      ((ISupportInitialize) this.tableReadoutSettings).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      ((ISupportInitialize) this.error).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
