// Decompiled with JetBrains decompiler
// Type: S3_Handler.Settings
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using StartupLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class Settings : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private GroupBox groupBoxDateAndTime;
    private CheckBox checkBoxUsePcTime;
    private GroupBox groupBoxSettings;
    private CheckBox checkBoxSaveSettings;
    private CheckBox checkBoxLoadLastSettings;
    private Button buttonSaveSettingsImmediately;
    private GroupBox groupBoxBackup;
    private CheckBox checkBoxBackupAfterReadDevice;
    private CheckBox checkBoxBackupAfterWriteDevice;
    private CheckBox checkBoxBackupAfterReadOncePerDay;
    private GroupBox groupBoxDevelopment;
    private CheckBox checkBoxUseDevelopmentFunctions;
    private CheckBox checkBoxBaseTypeEditMode;
    private CheckBox checkBoxCheckWriteByRead;
    private CheckBox checkBoxUseBaseTypeByConfig;
    private GroupBox groupBox1;
    private CheckBox checkBoxSuppressOptimizationByRead;
    private GroupBox groupBox2;
    private RadioButton RadioButtonCommPortChanged;
    private RadioButton RadioButtonCommPortCompatible;
    private RadioButton RadioButtonDcAc;

    internal Settings(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyMeter;
      this.InitializeComponent();
      this.checkBoxLoadLastSettings.Checked = this.MyFunctions._loadLastSettingsOnStart;
      this.checkBoxSaveSettings.Checked = this.MyFunctions._saveLastSettingsOnExit;
      this.checkBoxBackupAfterWriteDevice.Checked = this.MyFunctions._meterBackupOnWrite;
      this.checkBoxBackupAfterReadDevice.Checked = this.MyFunctions._meterBackupOnRead;
      this.checkBoxBackupAfterReadOncePerDay.Checked = this.MyFunctions._onlyOneReadBackupPerDay;
      this.checkBoxUseDevelopmentFunctions.Checked = this.MyFunctions.useDevelopmentFunctions;
      this.checkBoxBaseTypeEditMode.Checked = this.MyFunctions.baseTypeEditMode;
      this.checkBoxCheckWriteByRead.Checked = this.MyFunctions.checkWriteByRead;
      this.checkBoxUseBaseTypeByConfig.Checked = this.MyFunctions._useBaseTypeByConfig;
      this.checkBoxSuppressOptimizationByRead.Checked = this.MyFunctions._SuppressCloneOptimization;
      switch (this.MyFunctions.SelectedCommunicationStructure)
      {
        case S3_HandlerFunctions.CommunicationStructure.classicDeviceCollector:
          this.RadioButtonDcAc.Select();
          break;
        case S3_HandlerFunctions.CommunicationStructure.compatible:
          this.RadioButtonCommPortCompatible.Select();
          break;
        case S3_HandlerFunctions.CommunicationStructure.commonDefined:
          this.RadioButtonCommPortChanged.Select();
          break;
      }
      if (!this.MyFunctions.IsPlugin)
      {
        this.checkBoxSaveSettings.Enabled = false;
        this.buttonSaveSettingsImmediately.Visible = false;
      }
      this.checkBoxUsePcTime.Checked = this.MyFunctions._usePcTime;
    }

    private void checkBoxUsePcTime_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions._usePcTime = this.checkBoxUsePcTime.Checked;
    }

    private void checkBoxLoadLastSettings_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions._loadLastSettingsOnStart = this.checkBoxLoadLastSettings.Checked;
    }

    private void checkBoxSaveSettings_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions._saveLastSettingsOnExit = this.checkBoxSaveSettings.Checked;
    }

    private void checkBoxUseDevelopmentFunctions_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions.useDevelopmentFunctions = this.checkBoxUseDevelopmentFunctions.Checked;
    }

    private void checkBoxCheckWriteByRead_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions.checkWriteByRead = this.checkBoxCheckWriteByRead.Checked;
    }

    private void checkBoxBaseTypeEditMode_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions.baseTypeEditMode = this.checkBoxBaseTypeEditMode.Checked;
      this.MyFunctions.MyMeters.TypeMeterCache = (SortedList<int, S3_Meter>) null;
    }

    private void buttonSaveSettingsImmediately_Click(object sender, EventArgs e)
    {
      this.MyFunctions.WriteConfig();
      if (PlugInLoader.GmmConfiguration.WriteConfigFile())
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("S3_Handler", "Config written");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("S3_Handler", "Config write error", true);
      }
    }

    private void checkBoxBackupAfterWriteDevice_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions._meterBackupOnWrite = this.checkBoxBackupAfterWriteDevice.Checked;
    }

    private void checkBoxBackupAfterReadDevice_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions._meterBackupOnRead = this.checkBoxBackupAfterReadDevice.Checked;
    }

    private void checkBoxBackupAfterReadOncePerDay_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions._onlyOneReadBackupPerDay = this.checkBoxBackupAfterReadOncePerDay.Checked;
    }

    private void checkBoxUseBaseTypeByConfig_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions._useBaseTypeByConfig = this.checkBoxUseBaseTypeByConfig.Checked;
    }

    private void checkBoxSuppressOptimizationByRead_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions._SuppressCloneOptimization = this.checkBoxSuppressOptimizationByRead.Checked;
    }

    private void RadioButtonDcAc_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.RadioButtonDcAc.Checked)
        return;
      this.MyFunctions.SetCommunicationStructure(S3_HandlerFunctions.CommunicationStructure.classicDeviceCollector);
    }

    private void RadioButtonCommPortCompatible_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.RadioButtonCommPortCompatible.Checked)
        return;
      this.MyFunctions.SetCommunicationStructure(S3_HandlerFunctions.CommunicationStructure.compatible);
    }

    private void RadioButtonCommPortChanged_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.RadioButtonCommPortChanged.Checked)
        return;
      this.MyFunctions.SetCommunicationStructure(S3_HandlerFunctions.CommunicationStructure.commonDefined);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.groupBoxDateAndTime = new GroupBox();
      this.checkBoxUsePcTime = new CheckBox();
      this.groupBoxSettings = new GroupBox();
      this.checkBoxSaveSettings = new CheckBox();
      this.checkBoxLoadLastSettings = new CheckBox();
      this.buttonSaveSettingsImmediately = new Button();
      this.groupBoxBackup = new GroupBox();
      this.checkBoxBackupAfterReadOncePerDay = new CheckBox();
      this.checkBoxBackupAfterReadDevice = new CheckBox();
      this.checkBoxBackupAfterWriteDevice = new CheckBox();
      this.groupBoxDevelopment = new GroupBox();
      this.checkBoxUseBaseTypeByConfig = new CheckBox();
      this.checkBoxBaseTypeEditMode = new CheckBox();
      this.checkBoxCheckWriteByRead = new CheckBox();
      this.checkBoxUseDevelopmentFunctions = new CheckBox();
      this.groupBox1 = new GroupBox();
      this.checkBoxSuppressOptimizationByRead = new CheckBox();
      this.groupBox2 = new GroupBox();
      this.RadioButtonDcAc = new RadioButton();
      this.RadioButtonCommPortCompatible = new RadioButton();
      this.RadioButtonCommPortChanged = new RadioButton();
      this.groupBoxDateAndTime.SuspendLayout();
      this.groupBoxSettings.SuspendLayout();
      this.groupBoxBackup.SuspendLayout();
      this.groupBoxDevelopment.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(615, 45);
      this.zennerCoroprateDesign2.TabIndex = 16;
      this.groupBoxDateAndTime.Controls.Add((Control) this.checkBoxUsePcTime);
      this.groupBoxDateAndTime.Location = new Point(12, 222);
      this.groupBoxDateAndTime.Name = "groupBoxDateAndTime";
      this.groupBoxDateAndTime.Size = new Size(224, 46);
      this.groupBoxDateAndTime.TabIndex = 17;
      this.groupBoxDateAndTime.TabStop = false;
      this.groupBoxDateAndTime.Text = "Date and Time";
      this.checkBoxUsePcTime.AutoSize = true;
      this.checkBoxUsePcTime.Location = new Point(6, 19);
      this.checkBoxUsePcTime.Name = "checkBoxUsePcTime";
      this.checkBoxUsePcTime.Size = new Size(84, 17);
      this.checkBoxUsePcTime.TabIndex = 18;
      this.checkBoxUsePcTime.Text = "Use PC time";
      this.checkBoxUsePcTime.UseVisualStyleBackColor = true;
      this.checkBoxUsePcTime.CheckedChanged += new System.EventHandler(this.checkBoxUsePcTime_CheckedChanged);
      this.groupBoxSettings.Controls.Add((Control) this.checkBoxSaveSettings);
      this.groupBoxSettings.Controls.Add((Control) this.checkBoxLoadLastSettings);
      this.groupBoxSettings.Location = new Point(12, 55);
      this.groupBoxSettings.Name = "groupBoxSettings";
      this.groupBoxSettings.Size = new Size(224, 64);
      this.groupBoxSettings.TabIndex = 18;
      this.groupBoxSettings.TabStop = false;
      this.groupBoxSettings.Text = "Using of settings";
      this.checkBoxSaveSettings.AutoSize = true;
      this.checkBoxSaveSettings.Location = new Point(7, 40);
      this.checkBoxSaveSettings.Name = "checkBoxSaveSettings";
      this.checkBoxSaveSettings.Size = new Size(124, 17);
      this.checkBoxSaveSettings.TabIndex = 18;
      this.checkBoxSaveSettings.Text = "Save settings on exit";
      this.checkBoxSaveSettings.UseVisualStyleBackColor = true;
      this.checkBoxSaveSettings.CheckedChanged += new System.EventHandler(this.checkBoxSaveSettings_CheckedChanged);
      this.checkBoxLoadLastSettings.AutoSize = true;
      this.checkBoxLoadLastSettings.Location = new Point(7, 19);
      this.checkBoxLoadLastSettings.Name = "checkBoxLoadLastSettings";
      this.checkBoxLoadLastSettings.Size = new Size(146, 17);
      this.checkBoxLoadLastSettings.TabIndex = 18;
      this.checkBoxLoadLastSettings.Text = "Load last settings on start";
      this.checkBoxLoadLastSettings.UseVisualStyleBackColor = true;
      this.checkBoxLoadLastSettings.CheckedChanged += new System.EventHandler(this.checkBoxLoadLastSettings_CheckedChanged);
      this.buttonSaveSettingsImmediately.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSaveSettingsImmediately.Location = new Point(360, 415);
      this.buttonSaveSettingsImmediately.Name = "buttonSaveSettingsImmediately";
      this.buttonSaveSettingsImmediately.Size = new Size(246, 27);
      this.buttonSaveSettingsImmediately.TabIndex = 19;
      this.buttonSaveSettingsImmediately.Text = "Save settings";
      this.buttonSaveSettingsImmediately.UseVisualStyleBackColor = true;
      this.buttonSaveSettingsImmediately.Click += new System.EventHandler(this.buttonSaveSettingsImmediately_Click);
      this.groupBoxBackup.Controls.Add((Control) this.checkBoxBackupAfterReadOncePerDay);
      this.groupBoxBackup.Controls.Add((Control) this.checkBoxBackupAfterReadDevice);
      this.groupBoxBackup.Controls.Add((Control) this.checkBoxBackupAfterWriteDevice);
      this.groupBoxBackup.Location = new Point(12, (int) sbyte.MaxValue);
      this.groupBoxBackup.Name = "groupBoxBackup";
      this.groupBoxBackup.Size = new Size(224, 89);
      this.groupBoxBackup.TabIndex = 18;
      this.groupBoxBackup.TabStop = false;
      this.groupBoxBackup.Text = "Automatic backup to database";
      this.checkBoxBackupAfterReadOncePerDay.AutoSize = true;
      this.checkBoxBackupAfterReadOncePerDay.Location = new Point(25, 63);
      this.checkBoxBackupAfterReadOncePerDay.Name = "checkBoxBackupAfterReadOncePerDay";
      this.checkBoxBackupAfterReadOncePerDay.Size = new Size(158, 17);
      this.checkBoxBackupAfterReadOncePerDay.TabIndex = 18;
      this.checkBoxBackupAfterReadOncePerDay.Text = "after read only once per day";
      this.checkBoxBackupAfterReadOncePerDay.UseVisualStyleBackColor = true;
      this.checkBoxBackupAfterReadOncePerDay.CheckedChanged += new System.EventHandler(this.checkBoxBackupAfterReadOncePerDay_CheckedChanged);
      this.checkBoxBackupAfterReadDevice.AutoSize = true;
      this.checkBoxBackupAfterReadDevice.Location = new Point(7, 40);
      this.checkBoxBackupAfterReadDevice.Name = "checkBoxBackupAfterReadDevice";
      this.checkBoxBackupAfterReadDevice.Size = new Size(106, 17);
      this.checkBoxBackupAfterReadDevice.TabIndex = 18;
      this.checkBoxBackupAfterReadDevice.Text = "after read device";
      this.checkBoxBackupAfterReadDevice.UseVisualStyleBackColor = true;
      this.checkBoxBackupAfterReadDevice.CheckedChanged += new System.EventHandler(this.checkBoxBackupAfterReadDevice_CheckedChanged);
      this.checkBoxBackupAfterWriteDevice.AutoSize = true;
      this.checkBoxBackupAfterWriteDevice.Location = new Point(7, 19);
      this.checkBoxBackupAfterWriteDevice.Name = "checkBoxBackupAfterWriteDevice";
      this.checkBoxBackupAfterWriteDevice.Size = new Size(107, 17);
      this.checkBoxBackupAfterWriteDevice.TabIndex = 18;
      this.checkBoxBackupAfterWriteDevice.Text = "after write device";
      this.checkBoxBackupAfterWriteDevice.UseVisualStyleBackColor = true;
      this.checkBoxBackupAfterWriteDevice.CheckedChanged += new System.EventHandler(this.checkBoxBackupAfterWriteDevice_CheckedChanged);
      this.groupBoxDevelopment.Controls.Add((Control) this.checkBoxUseBaseTypeByConfig);
      this.groupBoxDevelopment.Controls.Add((Control) this.checkBoxBaseTypeEditMode);
      this.groupBoxDevelopment.Controls.Add((Control) this.checkBoxCheckWriteByRead);
      this.groupBoxDevelopment.Controls.Add((Control) this.checkBoxUseDevelopmentFunctions);
      this.groupBoxDevelopment.Location = new Point(12, 274);
      this.groupBoxDevelopment.Name = "groupBoxDevelopment";
      this.groupBoxDevelopment.Size = new Size(224, 111);
      this.groupBoxDevelopment.TabIndex = 17;
      this.groupBoxDevelopment.TabStop = false;
      this.groupBoxDevelopment.Text = "Development";
      this.checkBoxUseBaseTypeByConfig.AutoSize = true;
      this.checkBoxUseBaseTypeByConfig.Location = new Point(6, 88);
      this.checkBoxUseBaseTypeByConfig.Name = "checkBoxUseBaseTypeByConfig";
      this.checkBoxUseBaseTypeByConfig.Size = new Size(140, 17);
      this.checkBoxUseBaseTypeByConfig.TabIndex = 19;
      this.checkBoxUseBaseTypeByConfig.Text = "Use base type by config";
      this.checkBoxUseBaseTypeByConfig.UseVisualStyleBackColor = true;
      this.checkBoxUseBaseTypeByConfig.CheckedChanged += new System.EventHandler(this.checkBoxUseBaseTypeByConfig_CheckedChanged);
      this.checkBoxBaseTypeEditMode.AutoSize = true;
      this.checkBoxBaseTypeEditMode.Location = new Point(6, 65);
      this.checkBoxBaseTypeEditMode.Name = "checkBoxBaseTypeEditMode";
      this.checkBoxBaseTypeEditMode.Size = new Size(122, 17);
      this.checkBoxBaseTypeEditMode.TabIndex = 18;
      this.checkBoxBaseTypeEditMode.Text = "Base type edit mode";
      this.checkBoxBaseTypeEditMode.UseVisualStyleBackColor = true;
      this.checkBoxBaseTypeEditMode.CheckedChanged += new System.EventHandler(this.checkBoxBaseTypeEditMode_CheckedChanged);
      this.checkBoxCheckWriteByRead.AutoSize = true;
      this.checkBoxCheckWriteByRead.Location = new Point(6, 42);
      this.checkBoxCheckWriteByRead.Name = "checkBoxCheckWriteByRead";
      this.checkBoxCheckWriteByRead.Size = new Size(120, 17);
      this.checkBoxCheckWriteByRead.TabIndex = 18;
      this.checkBoxCheckWriteByRead.Text = "Check write by read";
      this.checkBoxCheckWriteByRead.UseVisualStyleBackColor = true;
      this.checkBoxCheckWriteByRead.CheckedChanged += new System.EventHandler(this.checkBoxCheckWriteByRead_CheckedChanged);
      this.checkBoxUseDevelopmentFunctions.AutoSize = true;
      this.checkBoxUseDevelopmentFunctions.Location = new Point(6, 19);
      this.checkBoxUseDevelopmentFunctions.Name = "checkBoxUseDevelopmentFunctions";
      this.checkBoxUseDevelopmentFunctions.Size = new Size(155, 17);
      this.checkBoxUseDevelopmentFunctions.TabIndex = 18;
      this.checkBoxUseDevelopmentFunctions.Text = "Use development functions";
      this.checkBoxUseDevelopmentFunctions.UseVisualStyleBackColor = true;
      this.checkBoxUseDevelopmentFunctions.CheckedChanged += new System.EventHandler(this.checkBoxUseDevelopmentFunctions_CheckedChanged);
      this.groupBox1.Controls.Add((Control) this.checkBoxSuppressOptimizationByRead);
      this.groupBox1.Location = new Point(243, 55);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(360, 64);
      this.groupBox1.TabIndex = 20;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Device optimization";
      this.checkBoxSuppressOptimizationByRead.AutoSize = true;
      this.checkBoxSuppressOptimizationByRead.Location = new Point(6, 19);
      this.checkBoxSuppressOptimizationByRead.Name = "checkBoxSuppressOptimizationByRead";
      this.checkBoxSuppressOptimizationByRead.Size = new Size(166, 17);
      this.checkBoxSuppressOptimizationByRead.TabIndex = 18;
      this.checkBoxSuppressOptimizationByRead.Text = "Suppress optimization by read";
      this.checkBoxSuppressOptimizationByRead.UseVisualStyleBackColor = true;
      this.checkBoxSuppressOptimizationByRead.CheckedChanged += new System.EventHandler(this.checkBoxSuppressOptimizationByRead_CheckedChanged);
      this.groupBox2.Controls.Add((Control) this.RadioButtonCommPortChanged);
      this.groupBox2.Controls.Add((Control) this.RadioButtonCommPortCompatible);
      this.groupBox2.Controls.Add((Control) this.RadioButtonDcAc);
      this.groupBox2.Location = new Point(243, (int) sbyte.MaxValue);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(360, 112);
      this.groupBox2.TabIndex = 20;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Communication system";
      this.RadioButtonDcAc.AutoSize = true;
      this.RadioButtonDcAc.Location = new Point(6, 19);
      this.RadioButtonDcAc.Name = "RadioButtonDcAc";
      this.RadioButtonDcAc.Size = new Size(174, 17);
      this.RadioButtonDcAc.TabIndex = 19;
      this.RadioButtonDcAc.TabStop = true;
      this.RadioButtonDcAc.Text = "DeviceCollector and AsyncCom";
      this.RadioButtonDcAc.UseVisualStyleBackColor = true;
      this.RadioButtonDcAc.CheckedChanged += new System.EventHandler(this.RadioButtonDcAc_CheckedChanged);
      this.RadioButtonCommPortCompatible.AutoSize = true;
      this.RadioButtonCommPortCompatible.Location = new Point(6, 39);
      this.RadioButtonCommPortCompatible.Name = "RadioButtonCommPortCompatible";
      this.RadioButtonCommPortCompatible.Size = new Size(264, 17);
      this.RadioButtonCommPortCompatible.TabIndex = 19;
      this.RadioButtonCommPortCompatible.TabStop = true;
      this.RadioButtonCommPortCompatible.Text = "CommunicationPort by compatible 16bit commands";
      this.RadioButtonCommPortCompatible.UseVisualStyleBackColor = true;
      this.RadioButtonCommPortCompatible.CheckedChanged += new System.EventHandler(this.RadioButtonCommPortCompatible_CheckedChanged);
      this.RadioButtonCommPortChanged.AutoSize = true;
      this.RadioButtonCommPortChanged.Location = new Point(6, 62);
      this.RadioButtonCommPortChanged.Name = "RadioButtonCommPortChanged";
      this.RadioButtonCommPortChanged.Size = new Size(258, 17);
      this.RadioButtonCommPortChanged.TabIndex = 19;
      this.RadioButtonCommPortChanged.TabStop = true;
      this.RadioButtonCommPortChanged.Text = "CommunicationPort by changed  32bit commands";
      this.RadioButtonCommPortChanged.UseVisualStyleBackColor = true;
      this.RadioButtonCommPortChanged.CheckedChanged += new System.EventHandler(this.RadioButtonCommPortChanged_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(615, 453);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.buttonSaveSettingsImmediately);
      this.Controls.Add((Control) this.groupBoxBackup);
      this.Controls.Add((Control) this.groupBoxSettings);
      this.Controls.Add((Control) this.groupBoxDevelopment);
      this.Controls.Add((Control) this.groupBoxDateAndTime);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (Settings);
      this.Text = "S3_Handler settings";
      this.groupBoxDateAndTime.ResumeLayout(false);
      this.groupBoxDateAndTime.PerformLayout();
      this.groupBoxSettings.ResumeLayout(false);
      this.groupBoxSettings.PerformLayout();
      this.groupBoxBackup.ResumeLayout(false);
      this.groupBoxBackup.PerformLayout();
      this.groupBoxDevelopment.ResumeLayout(false);
      this.groupBoxDevelopment.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
