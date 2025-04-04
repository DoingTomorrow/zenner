// Decompiled with JetBrains decompiler
// Type: EDC_Handler.ConfiguratorEdcWired
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using CorporateDesign;
using StartupLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public class ConfiguratorEdcWired : Form
  {
    private EDC_Meter tempWorkMeter;
    private EDC_Meter tempTypeMeter;
    private EDC_Meter tempBackupMeter;
    private EDC_Meter tempConnectedMeter;
    private IContainer components = (IContainer) null;
    private Button btnCancel;
    private Button btnSave;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private NumericUpDown txtMeterID;
    private NumericUpDown txtHardwareTypeID;
    private NumericUpDown txtMeterInfoID;
    private NumericUpDown txtBaseTypeID;
    private NumericUpDown txtMeterTypeID;
    private NumericUpDown txtSapMaterialNumber;
    private NumericUpDown txtSapProductionOrderNumber;
    private GroupBox groupBox1;
    private GroupBox groupBox3;
    private CheckBox ckboxPulseEnable;
    private Label label12;
    private NumericUpDown txtTimeZone;
    private Label label13;
    private NumericUpDown txtPulseReading;
    private NumericUpDown txtPulseMultiplier;
    private Label label14;
    private GroupBox groupBox6;
    private CheckedListBox listWarnings;
    private NumericUpDown txtSensorTimeout;
    private Label label15;
    private GroupBox groupBox7;
    private Label label19;
    private Label label20;
    private NumericUpDown txtMBusAddressPrimary;
    private ComboBox cboxMBusMediumPrimary;
    private Label label21;
    private DateTimePicker txtDueDate;
    private Label label22;
    private CheckBox ckboxIsMagnetDetected;
    private CheckBox ckboxFlowCheckEnabled;
    private CheckBox ckboxLogEnabled;
    private NumericUpDown txtSerialnumberPrimary;
    private ToolTip toolTip;
    private Label label26;
    private NumericUpDown txtPulsePeriod;
    private Label label25;
    private NumericUpDown txtMBusGenerationPrimary;
    private TextBox txtSerialnumberFull;
    private Label label31;
    private TextBox txtManufacturerPrimary;
    private Label label33;
    private Label label36;
    private ComboBox cboxHandlerObject;
    private Panel panel;
    private Label label34;
    private NumericUpDown txtPulseActivateRadio;
    private Label label37;
    private NumericUpDown txtCogCount;
    private GroupBox gboxConstants;
    private Label label45;
    private NumericUpDown txtPulseUnleakLimit;
    private Label label44;
    private NumericUpDown txtPulseLeakLimit;
    private Label label43;
    private NumericUpDown txtPulseBlockLimit;
    private Label label49;
    private NumericUpDown txtPulseUnbackLimit;
    private Label label48;
    private NumericUpDown txtPulseBackLimit;
    private Label label47;
    private NumericUpDown txtPulseLeakUpper;
    private Label label46;
    private NumericUpDown txtPulseLeakLower;
    private Label label51;
    private NumericUpDown txtBurstLimit;
    private Label label52;
    private NumericUpDown txtBurstDiff;
    private Label label53;
    private NumericUpDown txtUndersizeLimit;
    private Label label54;
    private NumericUpDown txtUndersizeDiff;
    private Label label55;
    private NumericUpDown txtOversizeLimit;
    private Label label56;
    private NumericUpDown txtOversizeDiff;
    private GroupBox groupBox8;
    private CheckedListBox listHardwareErrors;
    private Label label60;
    private NumericUpDown txtPulseErrorThreshold;
    private Label label61;
    private NumericUpDown txtUARTwatchdog;
    private DateTimePicker txtSystemTime;
    private Label label62;
    private DateTimePicker txtBatteryEndDate;
    private Label label63;
    private CheckBox ckboxShowParameterNamesUsedInFirmware;
    private GroupBox groupBox2;
    private Label label23;
    private NumericUpDown txtMinThreshold;
    private Label label24;
    private NumericUpDown txtMaxThreshold;
    private Label label40;
    private NumericUpDown txtErrorThreshold;
    private Label label39;
    private NumericUpDown txtB_offset;
    private Label label41;
    private NumericUpDown txtAmplitudeLimit;
    private CheckBox ckboxRemovalDetectionMasking;
    private ComboBox cboxPulseoutMode;
    private Label label8;
    private Label label9;
    private NumericUpDown txtPulseoutWidth;
    private Label label10;
    private NumericUpDown txtPulseoutPPP;
    private Label label11;
    private NumericUpDown txtDepassTimeout;
    private Label label16;
    private NumericUpDown txtDepassPeriod;
    private ComboBox cboxBaud;
    private Label label17;
    private ComboBox cboxList;
    private Label label27;
    private DateTimePicker txtStartModule;
    private Label label28;
    private DateTimePicker txtStartMeter;
    private Label label18;
    private TextBox txtObisPrimary;
    private Label label29;
    private GroupBox groupBox4;
    private TextBox txtObisSecondary;
    private Label label30;
    private TextBox txtManufacturerSecondary;
    private NumericUpDown txtMBusGenerationSecondary;
    private NumericUpDown txtSerialnumberSecondary;
    private ComboBox cboxMBusMediumSecondary;
    private NumericUpDown txtMBusAddressSecondary;
    private Label label32;
    private Label label35;
    private Label label38;
    private Label label42;
    private Label label50;

    public ConfiguratorEdcWired() => this.InitializeComponent();

    private void Configurator_Load(object sender, EventArgs e)
    {
      this.InitializeForm();
      this.btnSave.Enabled = UserManager.CheckPermission("EDC_Handler.View.Expert");
    }

    internal static void ShowDialog(Form owner, EDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (ConfiguratorEdcWired configuratorEdcWired = new ConfiguratorEdcWired())
      {
        if (MyFunctions.WorkMeter != null)
          configuratorEdcWired.tempWorkMeter = MyFunctions.WorkMeter.DeepCopy();
        if (MyFunctions.TypeMeter != null)
          configuratorEdcWired.tempTypeMeter = MyFunctions.TypeMeter.DeepCopy();
        if (MyFunctions.BackupMeter != null)
          configuratorEdcWired.tempBackupMeter = MyFunctions.BackupMeter.DeepCopy();
        if (MyFunctions.ConnectedMeter != null)
          configuratorEdcWired.tempConnectedMeter = MyFunctions.ConnectedMeter.DeepCopy();
        if (configuratorEdcWired.ShowDialog((IWin32Window) owner) != DialogResult.OK)
          return;
        MyFunctions.WorkMeter = configuratorEdcWired.tempWorkMeter;
        MyFunctions.TypeMeter = configuratorEdcWired.tempTypeMeter;
        MyFunctions.BackupMeter = configuratorEdcWired.tempBackupMeter;
        MyFunctions.ConnectedMeter = configuratorEdcWired.tempConnectedMeter;
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.SaveValues();
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void cboxHandlerObject_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.LoadValues();
    }

    private void ckboxShowParameterNamesUsedInFirmware_CheckedChanged(object sender, EventArgs e)
    {
      this.SwitchLableNames(this.Controls);
    }

    private void SwitchLableNames(Control.ControlCollection controls)
    {
      if (controls == null || controls.Count == 0)
        return;
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        if (control.Tag != null)
        {
          string str = control.Tag.ToString();
          string text = control.Text;
          control.Text = str;
          control.Tag = (object) text;
        }
        this.SwitchLableNames(control.Controls);
      }
    }

    private void LoadValues()
    {
      try
      {
        EDC_Meter handlerMeter = this.GetHandlerMeter();
        this.panel.Visible = handlerMeter != null;
        if (handlerMeter == null)
          return;
        DeviceIdentification deviceIdentification = handlerMeter.GetDeviceIdentification();
        if (deviceIdentification != null)
        {
          this.txtBaseTypeID.Value = (Decimal) deviceIdentification.BaseTypeID;
          this.txtHardwareTypeID.Value = (Decimal) deviceIdentification.HardwareTypeID;
          this.txtMeterID.Value = (Decimal) deviceIdentification.MeterID;
          this.txtMeterInfoID.Value = (Decimal) deviceIdentification.MeterInfoID;
          this.txtMeterTypeID.Value = (Decimal) deviceIdentification.MeterTypeID;
          this.txtSapMaterialNumber.Value = (Decimal) deviceIdentification.SapMaterialNumber;
          this.txtSapProductionOrderNumber.Value = (Decimal) deviceIdentification.SapProductionOrderNumber;
        }
        uint? serialnumberPrimary = handlerMeter.GetSerialnumberPrimary();
        byte? mbusAddressPrimary = handlerMeter.GetMBusAddressPrimary();
        MBusDeviceType? mediumPrimary = handlerMeter.GetMediumPrimary();
        byte? generationPrimary = handlerMeter.GetMBusGenerationPrimary();
        string manufacturerPrimary = handlerMeter.GetManufacturerPrimary();
        byte? obisPrimary = handlerMeter.GetObisPrimary();
        uint? serialnumberSecondary = handlerMeter.GetSerialnumberSecondary();
        byte? addressSecondary = handlerMeter.GetMBusAddressSecondary();
        MBusDeviceType? mediumSecondary = handlerMeter.GetMediumSecondary();
        byte? generationSecondary = handlerMeter.GetMBusGenerationSecondary();
        string manufacturerSecondary = handlerMeter.GetManufacturerSecondary();
        byte? obisSecondary = handlerMeter.GetObisSecondary();
        byte? pulseMultiplier = handlerMeter.GetPulseMultiplier();
        ushort? sensorTimeout = handlerMeter.GetSensorTimeout();
        DateTime? dueDate = handlerMeter.GetDueDate();
        bool? coilSampling = handlerMeter.GetCoilSampling();
        bool? checkIntervalState = handlerMeter.GetFlowCheckIntervalState();
        bool? magnetDetectionState = handlerMeter.GetMagnetDetectionState();
        bool? dataLoggingState = handlerMeter.GetDataLoggingState();
        Warning? warnings = handlerMeter.GetWarnings();
        int? timeZone = handlerMeter.GetTimeZone();
        int? meterValue = handlerMeter.GetMeterValue();
        ushort? pulsePeriod = handlerMeter.GetPulsePeriod();
        string serialnumberFull = handlerMeter.GetSerialnumberFull();
        byte? cogCount = handlerMeter.GetCogCount();
        ushort? pulseBlockLimit = handlerMeter.GetPulseBlockLimit();
        ushort? pulseLeakLimit = handlerMeter.GetPulseLeakLimit();
        ushort? pulseUnleakLimit = handlerMeter.GetPulseUnleakLimit();
        short? pulseLeakLower = handlerMeter.GetPulseLeakLower();
        short? pulseLeakUpper = handlerMeter.GetPulseLeakUpper();
        ushort? pulseBackLimit = handlerMeter.GetPulseBackLimit();
        ushort? pulseUnbackLimit = handlerMeter.GetPulseUnbackLimit();
        HardwareError? hardwareErrors = handlerMeter.GetHardwareErrors();
        ushort? oversizeDiff = handlerMeter.GetOversizeDiff();
        ushort? oversizeLimit = handlerMeter.GetOversizeLimit();
        ushort? undersizeDiff = handlerMeter.GetUndersizeDiff();
        ushort? undersizeLimit = handlerMeter.GetUndersizeLimit();
        ushort? burstDiff = handlerMeter.GetBurstDiff();
        ushort? burstLimit = handlerMeter.GetBurstLimit();
        byte? uarTwatchdog = handlerMeter.GetUARTwatchdog();
        byte? pulseErrorThreshold = handlerMeter.GetPulseErrorThreshold();
        DateTime? systemTime = handlerMeter.GetSystemTime();
        DateTime? batteryEndDate = handlerMeter.GetBatteryEndDate();
        sbyte? coilErrorThreshold = handlerMeter.GetCoilErrorThreshold();
        sbyte? coilMaxThreshold = handlerMeter.GetCoilMaxThreshold();
        sbyte? coilMinThreshold = handlerMeter.GetCoilMinThreshold();
        sbyte? coilAmplitudeLimit = handlerMeter.GetCoilAmplitudeLimit();
        sbyte? coilBOffset = handlerMeter.GetCoilB_offset();
        bool? removalDetectionState = handlerMeter.GetRemovalDetectionState();
        byte? pulseActivateRadio = handlerMeter.GetPulseActivateRadio();
        PulseoutMode? pulseoutMode = handlerMeter.GetPulseoutMode();
        ushort? pulseoutWidth = handlerMeter.GetPulseoutWidth();
        short? pulseoutPpp = handlerMeter.GetPulseoutPPP();
        ushort? depassTimeout = handlerMeter.GetDepassTimeout();
        ushort? depassPeriod = handlerMeter.GetDepassPeriod();
        MbusBaud? mbusBaud = handlerMeter.GetMbusBaud();
        string mbusListType = handlerMeter.GetMBusListType();
        DateTime? startModule = handlerMeter.GetStartModule();
        DateTime? startMeter = handlerMeter.GetStartMeter();
        this.txtSerialnumberPrimary.Value = (Decimal) (serialnumberPrimary.HasValue ? serialnumberPrimary.Value : 0U);
        this.txtManufacturerPrimary.Text = manufacturerPrimary ?? string.Empty;
        this.txtMBusAddressPrimary.Value = (Decimal) (mbusAddressPrimary.HasValue ? (int) mbusAddressPrimary.Value : 0);
        this.txtMBusGenerationPrimary.Value = (Decimal) (generationPrimary.HasValue ? (int) generationPrimary.Value : 0);
        this.cboxMBusMediumPrimary.SelectedItem = mediumPrimary.HasValue ? (object) mediumPrimary.Value.ToString() : (object) string.Empty;
        this.txtObisPrimary.Text = obisPrimary.HasValue ? obisPrimary.Value.ToString("X2") : "00";
        this.txtSerialnumberSecondary.Value = (Decimal) (serialnumberSecondary.HasValue ? serialnumberSecondary.Value : 0U);
        this.txtManufacturerSecondary.Text = manufacturerSecondary ?? string.Empty;
        this.txtMBusAddressSecondary.Value = (Decimal) (addressSecondary.HasValue ? (int) addressSecondary.Value : 0);
        this.txtMBusGenerationSecondary.Value = (Decimal) (generationSecondary.HasValue ? (int) generationSecondary.Value : 0);
        this.cboxMBusMediumSecondary.SelectedItem = mediumSecondary.HasValue ? (object) mediumSecondary.Value.ToString() : (object) string.Empty;
        this.txtObisSecondary.Text = obisSecondary.HasValue ? obisSecondary.Value.ToString("X2") : "00";
        this.txtSerialnumberFull.Text = serialnumberFull ?? string.Empty;
        this.txtPulseMultiplier.Value = (Decimal) (pulseMultiplier.HasValue ? (int) pulseMultiplier.Value : 1);
        this.txtSensorTimeout.Value = (Decimal) (sensorTimeout.HasValue ? (int) sensorTimeout.Value : 0);
        this.txtDueDate.Value = dueDate.HasValue ? dueDate.Value : EDC_HandlerFunctions.DateTimeNull;
        this.txtTimeZone.Value = (Decimal) (timeZone.HasValue ? timeZone.Value : 0);
        this.txtPulseReading.Value = (Decimal) (meterValue.HasValue ? meterValue.Value : 0);
        this.txtPulsePeriod.Value = (Decimal) (pulsePeriod.HasValue ? (int) pulsePeriod.Value : 0);
        this.ckboxPulseEnable.Checked = coilSampling.HasValue && coilSampling.Value;
        this.ckboxFlowCheckEnabled.Checked = checkIntervalState.HasValue && checkIntervalState.Value;
        this.ckboxIsMagnetDetected.Checked = magnetDetectionState.HasValue && magnetDetectionState.Value;
        this.ckboxLogEnabled.Checked = dataLoggingState.HasValue && dataLoggingState.Value;
        this.ckboxRemovalDetectionMasking.Checked = removalDetectionState.HasValue && removalDetectionState.Value;
        this.txtCogCount.Value = (Decimal) (cogCount.HasValue ? (int) cogCount.Value : 0);
        this.txtPulseBlockLimit.Value = (Decimal) (pulseBlockLimit.HasValue ? (int) pulseBlockLimit.Value : 0);
        this.txtPulseLeakLimit.Value = (Decimal) (pulseLeakLimit.HasValue ? (int) pulseLeakLimit.Value : 0);
        this.txtPulseUnleakLimit.Value = (Decimal) (pulseUnleakLimit.HasValue ? (int) pulseUnleakLimit.Value : 0);
        this.txtPulseLeakLower.Value = (Decimal) (pulseLeakLower.HasValue ? (int) pulseLeakLower.Value : 0);
        this.txtPulseLeakUpper.Value = (Decimal) (pulseLeakUpper.HasValue ? (int) pulseLeakUpper.Value : 0);
        this.txtPulseBackLimit.Value = (Decimal) (pulseBackLimit.HasValue ? (int) pulseBackLimit.Value : 0);
        this.txtPulseUnbackLimit.Value = (Decimal) (pulseUnbackLimit.HasValue ? (int) pulseUnbackLimit.Value : 0);
        this.txtOversizeDiff.Value = (Decimal) (oversizeDiff.HasValue ? (int) oversizeDiff.Value : 0);
        this.txtOversizeLimit.Value = (Decimal) (oversizeLimit.HasValue ? (int) oversizeLimit.Value : 0);
        this.txtUndersizeDiff.Value = (Decimal) (undersizeDiff.HasValue ? (int) undersizeDiff.Value : 0);
        this.txtUndersizeLimit.Value = (Decimal) (undersizeLimit.HasValue ? (int) undersizeLimit.Value : 0);
        this.txtBurstDiff.Value = (Decimal) (burstDiff.HasValue ? (int) burstDiff.Value : 0);
        this.txtBurstLimit.Value = (Decimal) (burstLimit.HasValue ? (int) burstLimit.Value : 0);
        this.txtUARTwatchdog.Value = (Decimal) (uarTwatchdog.HasValue ? (int) uarTwatchdog.Value : 0);
        this.txtPulseErrorThreshold.Value = (Decimal) (pulseErrorThreshold.HasValue ? (int) pulseErrorThreshold.Value : 0);
        this.txtSystemTime.Value = systemTime.HasValue ? systemTime.Value : EDC_HandlerFunctions.DateTimeNull;
        this.txtBatteryEndDate.Value = batteryEndDate.HasValue ? batteryEndDate.Value : EDC_HandlerFunctions.DateTimeNull;
        this.txtErrorThreshold.Value = (Decimal) (coilErrorThreshold.HasValue ? (int) coilErrorThreshold.Value : 0);
        this.txtMaxThreshold.Value = (Decimal) (coilMaxThreshold.HasValue ? (int) coilMaxThreshold.Value : 0);
        this.txtMinThreshold.Value = (Decimal) (coilMinThreshold.HasValue ? (int) coilMinThreshold.Value : 0);
        this.txtAmplitudeLimit.Value = (Decimal) (coilAmplitudeLimit.HasValue ? (int) coilAmplitudeLimit.Value : 0);
        this.txtB_offset.Value = (Decimal) (coilBOffset.HasValue ? (int) coilBOffset.Value : 0);
        this.txtPulseActivateRadio.Value = (Decimal) (pulseActivateRadio.HasValue ? (int) pulseActivateRadio.Value : 0);
        this.cboxPulseoutMode.SelectedItem = pulseoutMode.HasValue ? (object) pulseoutMode.Value.ToString() : (object) string.Empty;
        this.txtPulseoutWidth.Value = (Decimal) (pulseoutWidth.HasValue ? (int) pulseoutWidth.Value : 0);
        this.txtPulseoutPPP.Value = (Decimal) (pulseoutPpp.HasValue ? (int) pulseoutPpp.Value : 0);
        this.txtDepassTimeout.Value = (Decimal) (depassTimeout.HasValue ? (int) depassTimeout.Value : 0);
        this.txtDepassPeriod.Value = (Decimal) (depassPeriod.HasValue ? (int) depassPeriod.Value : 0);
        this.cboxBaud.SelectedItem = mbusBaud.HasValue ? (object) mbusBaud.Value.ToString() : (object) string.Empty;
        this.cboxList.SelectedItem = !string.IsNullOrEmpty(mbusListType) ? (object) mbusListType : (object) string.Empty;
        this.txtStartModule.Value = startModule.HasValue ? startModule.Value : EDC_HandlerFunctions.DateTimeNull;
        this.txtStartMeter.Value = startMeter.HasValue ? startMeter.Value : EDC_HandlerFunctions.DateTimeNull;
        if (warnings.HasValue)
        {
          for (int index = 0; index < this.listWarnings.Items.Count; ++index)
          {
            Warning warning1 = (Warning) Enum.Parse(typeof (Warning), this.listWarnings.GetItemText(this.listWarnings.Items[index]), true);
            Warning? nullable1 = warnings;
            Warning warning2 = warning1;
            Warning? nullable2 = nullable1.HasValue ? new Warning?(nullable1.GetValueOrDefault() & warning2) : new Warning?();
            Warning warning3 = warning1;
            bool flag = nullable2.GetValueOrDefault() == warning3 & nullable2.HasValue;
            this.listWarnings.SetItemChecked(index, flag);
          }
        }
        if (!hardwareErrors.HasValue)
          return;
        for (int index = 0; index < this.listHardwareErrors.Items.Count; ++index)
        {
          HardwareError hardwareError1 = (HardwareError) Enum.Parse(typeof (HardwareError), this.listHardwareErrors.GetItemText(this.listHardwareErrors.Items[index]), true);
          HardwareError? nullable3 = hardwareErrors;
          HardwareError hardwareError2 = hardwareError1;
          HardwareError? nullable4 = nullable3.HasValue ? new HardwareError?(nullable3.GetValueOrDefault() & hardwareError2) : new HardwareError?();
          HardwareError hardwareError3 = hardwareError1;
          bool flag = nullable4.GetValueOrDefault() == hardwareError3 & nullable4.HasValue;
          this.listHardwareErrors.SetItemChecked(index, flag);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Error by load the values", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void SaveValues()
    {
      try
      {
        EDC_Meter handlerMeter = this.GetHandlerMeter();
        if (handlerMeter == null)
          return;
        handlerMeter.SetDeviceIdentification(new DeviceIdentification()
        {
          BaseTypeID = (uint) this.txtBaseTypeID.Value,
          HardwareTypeID = (uint) this.txtHardwareTypeID.Value,
          MeterID = (uint) this.txtMeterID.Value,
          MeterInfoID = (uint) this.txtMeterInfoID.Value,
          MeterTypeID = (uint) this.txtMeterTypeID.Value,
          SapMaterialNumber = (uint) this.txtSapMaterialNumber.Value,
          SapProductionOrderNumber = (uint) this.txtSapProductionOrderNumber.Value
        });
        handlerMeter.SetSerialnumberFull(this.txtSerialnumberFull.Text);
        handlerMeter.SetSerialnumberPrimary(Convert.ToUInt32(this.txtSerialnumberPrimary.Value));
        handlerMeter.SetMBusAddressPrimary(Convert.ToByte(this.txtMBusAddressPrimary.Value));
        handlerMeter.SetMediumPrimary((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMediumPrimary.SelectedItem.ToString(), true));
        handlerMeter.SetMBusGenerationPrimary(Convert.ToByte(this.txtMBusGenerationPrimary.Value));
        handlerMeter.SetManufacturerPrimary(this.txtManufacturerPrimary.Text);
        handlerMeter.SetObisPrimary(Convert.ToByte(this.txtObisPrimary.Text, 16));
        handlerMeter.SetSerialnumberSecondary(Convert.ToUInt32(this.txtSerialnumberSecondary.Value));
        handlerMeter.SetMBusAddressSecondary(Convert.ToByte(this.txtMBusAddressSecondary.Value));
        handlerMeter.SetMediumSecondary((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMediumSecondary.SelectedItem.ToString(), true));
        handlerMeter.SetMBusGenerationSecondary(Convert.ToByte(this.txtMBusGenerationSecondary.Value));
        handlerMeter.SetManufacturerSecondary(this.txtManufacturerSecondary.Text);
        handlerMeter.SetObisSecondary(Convert.ToByte(this.txtObisSecondary.Text, 16));
        handlerMeter.SetPulseMultiplier(Convert.ToByte(this.txtPulseMultiplier.Value));
        handlerMeter.SetSensorTimeout(Convert.ToUInt16(this.txtSensorTimeout.Value));
        handlerMeter.SetDueDate(this.txtDueDate.Value);
        handlerMeter.SetCoilSampling(this.ckboxPulseEnable.Checked);
        handlerMeter.SetFlowCheckIntervalState(this.ckboxFlowCheckEnabled.Checked);
        handlerMeter.SetMagnetDetectionState(this.ckboxIsMagnetDetected.Checked);
        handlerMeter.SetDataLoggingState(this.ckboxLogEnabled.Checked);
        handlerMeter.SetWarnings(this.GetCheckedWarnings());
        handlerMeter.SetTimeZone(Convert.ToInt32(this.txtTimeZone.Value));
        handlerMeter.SetPulsePeriod(Convert.ToUInt16(this.txtPulsePeriod.Value));
        handlerMeter.SetCogCount(Convert.ToByte(this.txtCogCount.Value));
        handlerMeter.SetPulseBlockLimit(Convert.ToUInt16(this.txtPulseBlockLimit.Value));
        handlerMeter.SetPulseLeakLimit(Convert.ToUInt16(this.txtPulseLeakLimit.Value));
        handlerMeter.SetPulseUnleakLimit(Convert.ToUInt16(this.txtPulseUnleakLimit.Value));
        handlerMeter.SetPulseLeakLower(Convert.ToInt16(this.txtPulseLeakLower.Value));
        handlerMeter.SetPulseLeakUpper(Convert.ToInt16(this.txtPulseLeakUpper.Value));
        handlerMeter.SetPulseBackLimit(Convert.ToUInt16(this.txtPulseBackLimit.Value));
        handlerMeter.SetPulseUnbackLimit(Convert.ToUInt16(this.txtPulseUnbackLimit.Value));
        handlerMeter.SetHardwareErrors(this.GetCheckedHardwareErrors());
        handlerMeter.SetOversizeDiff(Convert.ToUInt16(this.txtOversizeDiff.Value));
        handlerMeter.SetOversizeLimit(Convert.ToUInt16(this.txtOversizeLimit.Value));
        handlerMeter.SetUndersizeDiff(Convert.ToUInt16(this.txtUndersizeDiff.Value));
        handlerMeter.SetUndersizeLimit(Convert.ToUInt16(this.txtUndersizeLimit.Value));
        handlerMeter.SetBurstDiff(Convert.ToUInt16(this.txtBurstDiff.Value));
        handlerMeter.SetBurstLimit(Convert.ToUInt16(this.txtBurstLimit.Value));
        handlerMeter.SetUARTwatchdog(Convert.ToByte(this.txtUARTwatchdog.Value));
        handlerMeter.SetPulseErrorThreshold(Convert.ToByte(this.txtPulseErrorThreshold.Value));
        handlerMeter.SetBatteryEndDate(this.txtBatteryEndDate.Value);
        handlerMeter.SetPulseActivateRadio(Convert.ToByte(this.txtPulseActivateRadio.Value));
        handlerMeter.SetCoilErrorThreshold(Convert.ToSByte(this.txtErrorThreshold.Value));
        handlerMeter.SetCoilMaxThreshold(Convert.ToSByte(this.txtMaxThreshold.Value));
        handlerMeter.SetCoilMinThreshold(Convert.ToSByte(this.txtMinThreshold.Value));
        handlerMeter.SetCoilAmplitudeLimit(Convert.ToSByte(this.txtAmplitudeLimit.Value));
        handlerMeter.SetCoilB_offset(Convert.ToSByte(this.txtB_offset.Value));
        handlerMeter.SetRemovalDetectionState(this.ckboxRemovalDetectionMasking.Checked);
        handlerMeter.SetPulseActivateRadio(Convert.ToByte(this.txtPulseActivateRadio.Value));
        handlerMeter.SetPulseoutMode((PulseoutMode) Enum.Parse(typeof (PulseoutMode), this.cboxPulseoutMode.SelectedItem.ToString(), true));
        handlerMeter.SetPulseoutWidth(Convert.ToUInt16(this.txtPulseoutWidth.Value));
        handlerMeter.SetPulseoutPPP(Convert.ToInt16(this.txtPulseoutPPP.Value));
        handlerMeter.SetDepassTimeout(Convert.ToUInt16(this.txtDepassTimeout.Value));
        handlerMeter.SetDepassPeriod(Convert.ToUInt16(this.txtDepassPeriod.Value));
        handlerMeter.SetMbusBaud((MbusBaud) Enum.Parse(typeof (MbusBaud), this.cboxBaud.SelectedItem.ToString(), true));
        handlerMeter.SetMBusListType(this.cboxList.SelectedItem.ToString());
        handlerMeter.SetStartModule(this.txtStartModule.Value);
        handlerMeter.SetStartMeter(this.txtStartMeter.Value);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Error occurred while save the values", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private Warning GetCheckedWarnings()
    {
      Warning checkedWarnings = ~(Warning.APP_BUSY | Warning.ABNORMAL | Warning.BATT_LOW | Warning.PERMANENT_ERROR | Warning.TEMPORARY_ERROR | Warning.TAMPER_A | Warning.REMOVAL_A | Warning.LEAK | Warning.LEAK_A | Warning.UNDERSIZE | Warning.BLOCK_A | Warning.BACKFLOW | Warning.BACKFLOW_A | Warning.INTERFERE | Warning.OVERSIZE | Warning.BURST);
      foreach (object checkedItem in this.listWarnings.CheckedItems)
      {
        Warning warning = (Warning) Enum.Parse(typeof (Warning), checkedItem.ToString(), true);
        checkedWarnings |= warning;
      }
      return checkedWarnings;
    }

    private HardwareError GetCheckedHardwareErrors()
    {
      HardwareError checkedHardwareErrors = (HardwareError) 0;
      foreach (object checkedItem in this.listHardwareErrors.CheckedItems)
      {
        HardwareError hardwareError = (HardwareError) Enum.Parse(typeof (HardwareError), checkedItem.ToString(), true);
        checkedHardwareErrors |= hardwareError;
      }
      return checkedHardwareErrors;
    }

    private EDC_Meter GetHandlerMeter()
    {
      EDC_Meter handlerMeter;
      switch ((HandlerMeterType) Enum.Parse(typeof (HandlerMeterType), this.cboxHandlerObject.SelectedItem.ToString()))
      {
        case HandlerMeterType.WorkMeter:
          handlerMeter = this.tempWorkMeter;
          break;
        case HandlerMeterType.TypeMeter:
          handlerMeter = this.tempTypeMeter;
          break;
        case HandlerMeterType.BackupMeter:
          handlerMeter = this.tempBackupMeter;
          break;
        case HandlerMeterType.ConnectedMeter:
          handlerMeter = this.tempConnectedMeter;
          break;
        default:
          throw new NotImplementedException();
      }
      return handlerMeter;
    }

    private void InitializeForm()
    {
      this.cboxMBusMediumPrimary.DataSource = (object) Util.GetNamesOfEnum(typeof (MBusDeviceType));
      this.cboxMBusMediumSecondary.DataSource = (object) Util.GetNamesOfEnum(typeof (MBusDeviceType));
      this.listWarnings.Items.Clear();
      foreach (object obj in Util.GetNamesOfEnum(typeof (Warning)))
        this.listWarnings.Items.Add(obj, false);
      this.listHardwareErrors.Items.Clear();
      foreach (object obj in Util.GetNamesOfEnum(typeof (HardwareError)))
        this.listHardwareErrors.Items.Add(obj, false);
      this.cboxPulseoutMode.DataSource = (object) Util.GetNamesOfEnum(typeof (PulseoutMode));
      this.cboxBaud.DataSource = (object) Util.GetNamesOfEnum(typeof (MbusBaud));
      this.cboxList.DataSource = (object) new string[3]
      {
        "LIST_A",
        "LIST_B",
        "LIST_C"
      };
      this.cboxHandlerObject.DataSource = (object) Util.GetNamesOfEnum(typeof (HandlerMeterType));
      this.cboxHandlerObject.SelectedItem = (object) HandlerMeterType.WorkMeter;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConfiguratorEdcWired));
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.txtMeterID = new NumericUpDown();
      this.txtHardwareTypeID = new NumericUpDown();
      this.txtMeterInfoID = new NumericUpDown();
      this.txtBaseTypeID = new NumericUpDown();
      this.txtMeterTypeID = new NumericUpDown();
      this.txtSapMaterialNumber = new NumericUpDown();
      this.txtSapProductionOrderNumber = new NumericUpDown();
      this.groupBox1 = new GroupBox();
      this.txtSerialnumberFull = new TextBox();
      this.label31 = new Label();
      this.label34 = new Label();
      this.txtPulseActivateRadio = new NumericUpDown();
      this.groupBox3 = new GroupBox();
      this.txtStartModule = new DateTimePicker();
      this.label28 = new Label();
      this.txtStartMeter = new DateTimePicker();
      this.label18 = new Label();
      this.cboxList = new ComboBox();
      this.label27 = new Label();
      this.cboxBaud = new ComboBox();
      this.label17 = new Label();
      this.label16 = new Label();
      this.txtDepassPeriod = new NumericUpDown();
      this.label11 = new Label();
      this.txtDepassTimeout = new NumericUpDown();
      this.label10 = new Label();
      this.txtPulseoutPPP = new NumericUpDown();
      this.label9 = new Label();
      this.txtPulseoutWidth = new NumericUpDown();
      this.cboxPulseoutMode = new ComboBox();
      this.label8 = new Label();
      this.txtBatteryEndDate = new DateTimePicker();
      this.label63 = new Label();
      this.txtSystemTime = new DateTimePicker();
      this.label62 = new Label();
      this.label37 = new Label();
      this.txtCogCount = new NumericUpDown();
      this.label26 = new Label();
      this.txtPulsePeriod = new NumericUpDown();
      this.label12 = new Label();
      this.txtTimeZone = new NumericUpDown();
      this.txtDueDate = new DateTimePicker();
      this.label22 = new Label();
      this.txtSensorTimeout = new NumericUpDown();
      this.label15 = new Label();
      this.txtPulseMultiplier = new NumericUpDown();
      this.label14 = new Label();
      this.ckboxPulseEnable = new CheckBox();
      this.ckboxFlowCheckEnabled = new CheckBox();
      this.ckboxLogEnabled = new CheckBox();
      this.ckboxIsMagnetDetected = new CheckBox();
      this.label13 = new Label();
      this.txtPulseReading = new NumericUpDown();
      this.groupBox6 = new GroupBox();
      this.listWarnings = new CheckedListBox();
      this.groupBox7 = new GroupBox();
      this.txtObisPrimary = new TextBox();
      this.label29 = new Label();
      this.txtManufacturerPrimary = new TextBox();
      this.txtMBusGenerationPrimary = new NumericUpDown();
      this.txtSerialnumberPrimary = new NumericUpDown();
      this.cboxMBusMediumPrimary = new ComboBox();
      this.txtMBusAddressPrimary = new NumericUpDown();
      this.label33 = new Label();
      this.label25 = new Label();
      this.label21 = new Label();
      this.label20 = new Label();
      this.label19 = new Label();
      this.toolTip = new ToolTip(this.components);
      this.txtMinThreshold = new NumericUpDown();
      this.label36 = new Label();
      this.cboxHandlerObject = new ComboBox();
      this.panel = new Panel();
      this.groupBox4 = new GroupBox();
      this.txtObisSecondary = new TextBox();
      this.label30 = new Label();
      this.txtManufacturerSecondary = new TextBox();
      this.txtMBusGenerationSecondary = new NumericUpDown();
      this.txtSerialnumberSecondary = new NumericUpDown();
      this.cboxMBusMediumSecondary = new ComboBox();
      this.txtMBusAddressSecondary = new NumericUpDown();
      this.label32 = new Label();
      this.label35 = new Label();
      this.label38 = new Label();
      this.label42 = new Label();
      this.label50 = new Label();
      this.ckboxRemovalDetectionMasking = new CheckBox();
      this.groupBox2 = new GroupBox();
      this.label23 = new Label();
      this.label24 = new Label();
      this.txtMaxThreshold = new NumericUpDown();
      this.label40 = new Label();
      this.txtErrorThreshold = new NumericUpDown();
      this.label39 = new Label();
      this.txtB_offset = new NumericUpDown();
      this.label41 = new Label();
      this.txtAmplitudeLimit = new NumericUpDown();
      this.groupBox8 = new GroupBox();
      this.listHardwareErrors = new CheckedListBox();
      this.gboxConstants = new GroupBox();
      this.label60 = new Label();
      this.label61 = new Label();
      this.label51 = new Label();
      this.label52 = new Label();
      this.label53 = new Label();
      this.label54 = new Label();
      this.label55 = new Label();
      this.label56 = new Label();
      this.label49 = new Label();
      this.label48 = new Label();
      this.label47 = new Label();
      this.label46 = new Label();
      this.label45 = new Label();
      this.label44 = new Label();
      this.label43 = new Label();
      this.txtPulseErrorThreshold = new NumericUpDown();
      this.txtUARTwatchdog = new NumericUpDown();
      this.txtBurstLimit = new NumericUpDown();
      this.txtBurstDiff = new NumericUpDown();
      this.txtUndersizeLimit = new NumericUpDown();
      this.txtUndersizeDiff = new NumericUpDown();
      this.txtOversizeLimit = new NumericUpDown();
      this.txtOversizeDiff = new NumericUpDown();
      this.txtPulseUnbackLimit = new NumericUpDown();
      this.txtPulseBackLimit = new NumericUpDown();
      this.txtPulseLeakUpper = new NumericUpDown();
      this.txtPulseLeakLower = new NumericUpDown();
      this.txtPulseUnleakLimit = new NumericUpDown();
      this.txtPulseLeakLimit = new NumericUpDown();
      this.txtPulseBlockLimit = new NumericUpDown();
      this.ckboxShowParameterNamesUsedInFirmware = new CheckBox();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.txtMeterID.BeginInit();
      this.txtHardwareTypeID.BeginInit();
      this.txtMeterInfoID.BeginInit();
      this.txtBaseTypeID.BeginInit();
      this.txtMeterTypeID.BeginInit();
      this.txtSapMaterialNumber.BeginInit();
      this.txtSapProductionOrderNumber.BeginInit();
      this.groupBox1.SuspendLayout();
      this.txtPulseActivateRadio.BeginInit();
      this.groupBox3.SuspendLayout();
      this.txtDepassPeriod.BeginInit();
      this.txtDepassTimeout.BeginInit();
      this.txtPulseoutPPP.BeginInit();
      this.txtPulseoutWidth.BeginInit();
      this.txtCogCount.BeginInit();
      this.txtPulsePeriod.BeginInit();
      this.txtTimeZone.BeginInit();
      this.txtSensorTimeout.BeginInit();
      this.txtPulseMultiplier.BeginInit();
      this.txtPulseReading.BeginInit();
      this.groupBox6.SuspendLayout();
      this.groupBox7.SuspendLayout();
      this.txtMBusGenerationPrimary.BeginInit();
      this.txtSerialnumberPrimary.BeginInit();
      this.txtMBusAddressPrimary.BeginInit();
      this.txtMinThreshold.BeginInit();
      this.panel.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.txtMBusGenerationSecondary.BeginInit();
      this.txtSerialnumberSecondary.BeginInit();
      this.txtMBusAddressSecondary.BeginInit();
      this.groupBox2.SuspendLayout();
      this.txtMaxThreshold.BeginInit();
      this.txtErrorThreshold.BeginInit();
      this.txtB_offset.BeginInit();
      this.txtAmplitudeLimit.BeginInit();
      this.groupBox8.SuspendLayout();
      this.gboxConstants.SuspendLayout();
      this.txtPulseErrorThreshold.BeginInit();
      this.txtUARTwatchdog.BeginInit();
      this.txtBurstLimit.BeginInit();
      this.txtBurstDiff.BeginInit();
      this.txtUndersizeLimit.BeginInit();
      this.txtUndersizeDiff.BeginInit();
      this.txtOversizeLimit.BeginInit();
      this.txtOversizeDiff.BeginInit();
      this.txtPulseUnbackLimit.BeginInit();
      this.txtPulseBackLimit.BeginInit();
      this.txtPulseLeakUpper.BeginInit();
      this.txtPulseLeakLower.BeginInit();
      this.txtPulseUnleakLimit.BeginInit();
      this.txtPulseLeakLimit.BeginInit();
      this.txtPulseBlockLimit.BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(869, 481);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(77, 29);
      this.btnCancel.TabIndex = 19;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Enabled = false;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.ImeMode = ImeMode.NoControl;
      this.btnSave.Location = new Point(769, 481);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(77, 29);
      this.btnSave.TabIndex = 18;
      this.btnSave.Text = "Save";
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.label1.Location = new Point(28, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(120, 15);
      this.label1.TabIndex = 21;
      this.label1.Tag = (object) "Con_MeterId";
      this.label1.Text = "Meter ID:";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.label2.Location = new Point(28, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(120, 15);
      this.label2.TabIndex = 23;
      this.label2.Tag = (object) "Con_HardwareTypeId";
      this.label2.Text = "Hardware Type ID:";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.label3.Location = new Point(28, 55);
      this.label3.Name = "label3";
      this.label3.Size = new Size(120, 15);
      this.label3.TabIndex = 25;
      this.label3.Tag = (object) "Con_MeterInfoId";
      this.label3.Text = "Meter Info ID:";
      this.label3.TextAlign = ContentAlignment.MiddleRight;
      this.label4.Location = new Point(28, 75);
      this.label4.Name = "label4";
      this.label4.Size = new Size(120, 15);
      this.label4.TabIndex = 27;
      this.label4.Tag = (object) "Con_BaseTypeId";
      this.label4.Text = "Base Type ID:";
      this.label4.TextAlign = ContentAlignment.MiddleRight;
      this.label5.Location = new Point(28, 96);
      this.label5.Name = "label5";
      this.label5.Size = new Size(120, 15);
      this.label5.TabIndex = 29;
      this.label5.Tag = (object) "Con_MeterTypeId";
      this.label5.Text = "Meter Type ID:";
      this.label5.TextAlign = ContentAlignment.MiddleRight;
      this.label6.Location = new Point(28, 114);
      this.label6.Name = "label6";
      this.label6.Size = new Size(120, 15);
      this.label6.TabIndex = 31;
      this.label6.Tag = (object) "Con_SAP_MaterialNumber";
      this.label6.Text = "SAP Material Nr:";
      this.label6.TextAlign = ContentAlignment.MiddleRight;
      this.label7.Location = new Point(28, 133);
      this.label7.Name = "label7";
      this.label7.Size = new Size(120, 15);
      this.label7.TabIndex = 33;
      this.label7.Tag = (object) "Con_SAP_ProductionOrderNumber";
      this.label7.Text = "SAP Order Nr:";
      this.label7.TextAlign = ContentAlignment.MiddleRight;
      this.txtMeterID.Location = new Point(151, 13);
      this.txtMeterID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMeterID.Name = "txtMeterID";
      this.txtMeterID.Size = new Size(102, 20);
      this.txtMeterID.TabIndex = 34;
      this.txtHardwareTypeID.Location = new Point(151, 33);
      this.txtHardwareTypeID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtHardwareTypeID.Name = "txtHardwareTypeID";
      this.txtHardwareTypeID.Size = new Size(102, 20);
      this.txtHardwareTypeID.TabIndex = 35;
      this.txtMeterInfoID.Location = new Point(151, 53);
      this.txtMeterInfoID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMeterInfoID.Name = "txtMeterInfoID";
      this.txtMeterInfoID.Size = new Size(102, 20);
      this.txtMeterInfoID.TabIndex = 36;
      this.txtBaseTypeID.Location = new Point(151, 73);
      this.txtBaseTypeID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtBaseTypeID.Name = "txtBaseTypeID";
      this.txtBaseTypeID.Size = new Size(102, 20);
      this.txtBaseTypeID.TabIndex = 37;
      this.txtMeterTypeID.Location = new Point(151, 93);
      this.txtMeterTypeID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMeterTypeID.Name = "txtMeterTypeID";
      this.txtMeterTypeID.Size = new Size(102, 20);
      this.txtMeterTypeID.TabIndex = 38;
      this.txtSapMaterialNumber.Location = new Point(151, 113);
      this.txtSapMaterialNumber.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtSapMaterialNumber.Name = "txtSapMaterialNumber";
      this.txtSapMaterialNumber.Size = new Size(102, 20);
      this.txtSapMaterialNumber.TabIndex = 39;
      this.txtSapProductionOrderNumber.Location = new Point(151, 133);
      this.txtSapProductionOrderNumber.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtSapProductionOrderNumber.Name = "txtSapProductionOrderNumber";
      this.txtSapProductionOrderNumber.Size = new Size(102, 20);
      this.txtSapProductionOrderNumber.TabIndex = 40;
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.txtSerialnumberFull);
      this.groupBox1.Controls.Add((Control) this.txtSapProductionOrderNumber);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.txtSapMaterialNumber);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.txtMeterTypeID);
      this.groupBox1.Controls.Add((Control) this.label31);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.txtBaseTypeID);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.txtMeterInfoID);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.txtHardwareTypeID);
      this.groupBox1.Controls.Add((Control) this.label7);
      this.groupBox1.Controls.Add((Control) this.txtMeterID);
      this.groupBox1.Location = new Point(6, 290);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(262, 209);
      this.groupBox1.TabIndex = 41;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Identification (factory)";
      this.txtSerialnumberFull.Location = new Point(43, 174);
      this.txtSerialnumberFull.Name = "txtSerialnumberFull";
      this.txtSerialnumberFull.Size = new Size(210, 20);
      this.txtSerialnumberFull.TabIndex = 65;
      this.label31.Location = new Point(28, 159);
      this.label31.Name = "label31";
      this.label31.Size = new Size(170, 15);
      this.label31.TabIndex = 64;
      this.label31.Tag = (object) "Con_fullserialnumber";
      this.label31.Text = "(DIN 43863-5) Full serial number";
      this.label31.TextAlign = ContentAlignment.MiddleRight;
      this.label34.Location = new Point(3, 177);
      this.label34.Name = "label34";
      this.label34.Size = new Size(134, 15);
      this.label34.TabIndex = 51;
      this.label34.Tag = (object) "cfg_pulse_activate";
      this.label34.Text = "Pulse activate (logger on):";
      this.label34.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseActivateRadio.Location = new Point(145, 176);
      this.txtPulseActivateRadio.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseActivateRadio.Name = "txtPulseActivateRadio";
      this.txtPulseActivateRadio.Size = new Size(111, 20);
      this.txtPulseActivateRadio.TabIndex = 52;
      this.groupBox3.Controls.Add((Control) this.txtStartModule);
      this.groupBox3.Controls.Add((Control) this.label28);
      this.groupBox3.Controls.Add((Control) this.txtStartMeter);
      this.groupBox3.Controls.Add((Control) this.label18);
      this.groupBox3.Controls.Add((Control) this.cboxList);
      this.groupBox3.Controls.Add((Control) this.label27);
      this.groupBox3.Controls.Add((Control) this.cboxBaud);
      this.groupBox3.Controls.Add((Control) this.label17);
      this.groupBox3.Controls.Add((Control) this.label16);
      this.groupBox3.Controls.Add((Control) this.txtDepassPeriod);
      this.groupBox3.Controls.Add((Control) this.label11);
      this.groupBox3.Controls.Add((Control) this.txtDepassTimeout);
      this.groupBox3.Controls.Add((Control) this.label10);
      this.groupBox3.Controls.Add((Control) this.txtPulseoutPPP);
      this.groupBox3.Controls.Add((Control) this.label9);
      this.groupBox3.Controls.Add((Control) this.txtPulseoutWidth);
      this.groupBox3.Controls.Add((Control) this.cboxPulseoutMode);
      this.groupBox3.Controls.Add((Control) this.label8);
      this.groupBox3.Controls.Add((Control) this.label34);
      this.groupBox3.Controls.Add((Control) this.txtBatteryEndDate);
      this.groupBox3.Controls.Add((Control) this.label63);
      this.groupBox3.Controls.Add((Control) this.txtPulseActivateRadio);
      this.groupBox3.Controls.Add((Control) this.txtSystemTime);
      this.groupBox3.Controls.Add((Control) this.label62);
      this.groupBox3.Controls.Add((Control) this.label37);
      this.groupBox3.Controls.Add((Control) this.txtCogCount);
      this.groupBox3.Controls.Add((Control) this.label26);
      this.groupBox3.Controls.Add((Control) this.txtPulsePeriod);
      this.groupBox3.Controls.Add((Control) this.label12);
      this.groupBox3.Controls.Add((Control) this.txtTimeZone);
      this.groupBox3.Controls.Add((Control) this.txtDueDate);
      this.groupBox3.Controls.Add((Control) this.label22);
      this.groupBox3.Controls.Add((Control) this.txtSensorTimeout);
      this.groupBox3.Controls.Add((Control) this.label15);
      this.groupBox3.Controls.Add((Control) this.txtPulseMultiplier);
      this.groupBox3.Controls.Add((Control) this.label14);
      this.groupBox3.Location = new Point(274, 6);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(262, 383);
      this.groupBox3.TabIndex = 43;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Common settings";
      this.txtStartModule.CustomFormat = "dd.MM.yyyy HH:mm";
      this.txtStartModule.Format = DateTimePickerFormat.Custom;
      this.txtStartModule.Location = new Point(144, 355);
      this.txtStartModule.Name = "txtStartModule";
      this.txtStartModule.ShowUpDown = true;
      this.txtStartModule.Size = new Size(112, 20);
      this.txtStartModule.TabIndex = 98;
      this.label28.Location = new Point(7, 357);
      this.label28.Name = "label28";
      this.label28.Size = new Size(131, 15);
      this.label28.TabIndex = 97;
      this.label28.Tag = (object) "cfg_start_module";
      this.label28.Text = "Start module:";
      this.label28.TextAlign = ContentAlignment.MiddleRight;
      this.txtStartMeter.CustomFormat = "dd.MM.yyyy HH:mm";
      this.txtStartMeter.Format = DateTimePickerFormat.Custom;
      this.txtStartMeter.Location = new Point(144, 335);
      this.txtStartMeter.Name = "txtStartMeter";
      this.txtStartMeter.ShowUpDown = true;
      this.txtStartMeter.Size = new Size(112, 20);
      this.txtStartMeter.TabIndex = 96;
      this.label18.Location = new Point(7, 337);
      this.label18.Name = "label18";
      this.label18.Size = new Size(131, 15);
      this.label18.TabIndex = 95;
      this.label18.Tag = (object) "cfg_start_meter";
      this.label18.Text = "Start meter:";
      this.label18.TextAlign = ContentAlignment.MiddleRight;
      this.cboxList.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxList.FormattingEnabled = true;
      this.cboxList.Location = new Point(145, 313);
      this.cboxList.Name = "cboxList";
      this.cboxList.Size = new Size(111, 21);
      this.cboxList.TabIndex = 93;
      this.label27.Location = new Point(14, 315);
      this.label27.Name = "label27";
      this.label27.Size = new Size(123, 15);
      this.label27.TabIndex = 94;
      this.label27.Tag = (object) "cfg_list";
      this.label27.Text = "List:";
      this.label27.TextAlign = ContentAlignment.MiddleRight;
      this.cboxBaud.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxBaud.FormattingEnabled = true;
      this.cboxBaud.Location = new Point(145, 292);
      this.cboxBaud.Name = "cboxBaud";
      this.cboxBaud.Size = new Size(111, 21);
      this.cboxBaud.TabIndex = 70;
      this.label17.Location = new Point(25, 294);
      this.label17.Name = "label17";
      this.label17.Size = new Size(112, 15);
      this.label17.TabIndex = 69;
      this.label17.Tag = (object) "cfg_mbus_baud";
      this.label17.Text = "M-Bus baud:";
      this.label17.TextAlign = ContentAlignment.MiddleRight;
      this.label16.Location = new Point(8, 274);
      this.label16.Margin = new Padding(3, 0, 0, 0);
      this.label16.Name = "label16";
      this.label16.Size = new Size(129, 15);
      this.label16.TabIndex = 91;
      this.label16.Tag = (object) "cfg_depass_period";
      this.label16.Text = "Depass period:";
      this.label16.TextAlign = ContentAlignment.MiddleRight;
      this.txtDepassPeriod.Location = new Point(145, 273);
      this.txtDepassPeriod.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtDepassPeriod.Name = "txtDepassPeriod";
      this.txtDepassPeriod.Size = new Size(111, 20);
      this.txtDepassPeriod.TabIndex = 92;
      this.label11.Location = new Point(8, (int) byte.MaxValue);
      this.label11.Margin = new Padding(3, 0, 0, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(129, 15);
      this.label11.TabIndex = 89;
      this.label11.Tag = (object) "cfg_depass_timeout";
      this.label11.Text = "Depass timeout:";
      this.label11.TextAlign = ContentAlignment.MiddleRight;
      this.txtDepassTimeout.Location = new Point(145, 254);
      this.txtDepassTimeout.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtDepassTimeout.Name = "txtDepassTimeout";
      this.txtDepassTimeout.Size = new Size(111, 20);
      this.txtDepassTimeout.TabIndex = 90;
      this.label10.Location = new Point(8, 236);
      this.label10.Margin = new Padding(3, 0, 0, 0);
      this.label10.Name = "label10";
      this.label10.Size = new Size(129, 15);
      this.label10.TabIndex = 87;
      this.label10.Tag = (object) "cfg_pulseout_ppp";
      this.label10.Text = "Pulseout PPP:";
      this.label10.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseoutPPP.Location = new Point(145, 235);
      this.txtPulseoutPPP.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseoutPPP.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtPulseoutPPP.Name = "txtPulseoutPPP";
      this.txtPulseoutPPP.Size = new Size(111, 20);
      this.txtPulseoutPPP.TabIndex = 88;
      this.label9.Location = new Point(8, 217);
      this.label9.Margin = new Padding(3, 0, 0, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(129, 15);
      this.label9.TabIndex = 85;
      this.label9.Tag = (object) "cfg_pulseout_width";
      this.label9.Text = "Pulseout width:";
      this.label9.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseoutWidth.Location = new Point(145, 216);
      this.txtPulseoutWidth.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseoutWidth.Name = "txtPulseoutWidth";
      this.txtPulseoutWidth.Size = new Size(111, 20);
      this.txtPulseoutWidth.TabIndex = 86;
      this.cboxPulseoutMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxPulseoutMode.FormattingEnabled = true;
      this.cboxPulseoutMode.Location = new Point(145, 195);
      this.cboxPulseoutMode.Name = "cboxPulseoutMode";
      this.cboxPulseoutMode.Size = new Size(111, 21);
      this.cboxPulseoutMode.TabIndex = 68;
      this.label8.Location = new Point(6, 197);
      this.label8.Name = "label8";
      this.label8.Size = new Size(133, 15);
      this.label8.TabIndex = 67;
      this.label8.Tag = (object) "cfg_pulseout_mode";
      this.label8.Text = "Pulseout mode:";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.txtBatteryEndDate.CustomFormat = "dd.MM.yyyy";
      this.txtBatteryEndDate.Format = DateTimePickerFormat.Custom;
      this.txtBatteryEndDate.Location = new Point(145, 156);
      this.txtBatteryEndDate.Name = "txtBatteryEndDate";
      this.txtBatteryEndDate.ShowUpDown = true;
      this.txtBatteryEndDate.Size = new Size(112, 20);
      this.txtBatteryEndDate.TabIndex = 66;
      this.label63.Location = new Point(8, 158);
      this.label63.Name = "label63";
      this.label63.Size = new Size(131, 15);
      this.label63.TabIndex = 65;
      this.label63.Tag = (object) "cfg_lowbatt";
      this.label63.Text = "Battery end date:";
      this.label63.TextAlign = ContentAlignment.MiddleRight;
      this.txtSystemTime.CustomFormat = "dd.MM.yyyy HH:mm";
      this.txtSystemTime.Enabled = false;
      this.txtSystemTime.Format = DateTimePickerFormat.Custom;
      this.txtSystemTime.Location = new Point(145, 16);
      this.txtSystemTime.Name = "txtSystemTime";
      this.txtSystemTime.ShowUpDown = true;
      this.txtSystemTime.Size = new Size(111, 20);
      this.txtSystemTime.TabIndex = 64;
      this.label62.Location = new Point(8, 16);
      this.label62.Name = "label62";
      this.label62.Size = new Size(131, 15);
      this.label62.TabIndex = 63;
      this.label62.Tag = (object) "RTC_A";
      this.label62.Text = "System time:";
      this.label62.TextAlign = ContentAlignment.MiddleRight;
      this.label37.Location = new Point(8, 136);
      this.label37.Name = "label37";
      this.label37.Size = new Size(131, 15);
      this.label37.TabIndex = 61;
      this.label37.Tag = (object) "cfg_cog_count";
      this.label37.Text = "Cog count:";
      this.label37.TextAlign = ContentAlignment.MiddleRight;
      this.txtCogCount.Location = new Point(145, 136);
      this.txtCogCount.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtCogCount.Name = "txtCogCount";
      this.txtCogCount.Size = new Size(111, 20);
      this.txtCogCount.TabIndex = 62;
      this.toolTip.SetToolTip((Control) this.txtCogCount, "Equates to rollover of 99999999");
      this.label26.Location = new Point(8, 116);
      this.label26.Name = "label26";
      this.label26.Size = new Size(131, 15);
      this.label26.TabIndex = 56;
      this.label26.Tag = (object) "cfg_pulse_period";
      this.label26.Text = "Pulse period:";
      this.label26.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulsePeriod.Location = new Point(145, 116);
      this.txtPulsePeriod.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtPulsePeriod.Name = "txtPulsePeriod";
      this.txtPulsePeriod.Size = new Size(111, 20);
      this.txtPulsePeriod.TabIndex = 57;
      this.label12.Location = new Point(8, 96);
      this.label12.Name = "label12";
      this.label12.Size = new Size(131, 15);
      this.label12.TabIndex = 39;
      this.label12.Tag = (object) "Bak_TimeZoneInQuarterHours";
      this.label12.Text = "Timezone (qarter hours):";
      this.label12.TextAlign = ContentAlignment.MiddleRight;
      this.txtTimeZone.Location = new Point(145, 96);
      this.txtTimeZone.Maximum = new Decimal(new int[4]
      {
        56,
        0,
        0,
        0
      });
      this.txtTimeZone.Minimum = new Decimal(new int[4]
      {
        48,
        0,
        0,
        int.MinValue
      });
      this.txtTimeZone.Name = "txtTimeZone";
      this.txtTimeZone.Size = new Size(111, 20);
      this.txtTimeZone.TabIndex = 40;
      this.txtDueDate.CustomFormat = "dd.MM";
      this.txtDueDate.Format = DateTimePickerFormat.Custom;
      this.txtDueDate.Location = new Point(145, 76);
      this.txtDueDate.Name = "txtDueDate";
      this.txtDueDate.ShowUpDown = true;
      this.txtDueDate.Size = new Size(112, 20);
      this.txtDueDate.TabIndex = 44;
      this.label22.Location = new Point(8, 79);
      this.label22.Name = "label22";
      this.label22.Size = new Size(131, 15);
      this.label22.TabIndex = 43;
      this.label22.Tag = (object) "cfg_stichtag";
      this.label22.Text = "Due date (day, month):";
      this.label22.TextAlign = ContentAlignment.MiddleRight;
      this.txtSensorTimeout.Location = new Point(145, 56);
      this.txtSensorTimeout.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtSensorTimeout.Name = "txtSensorTimeout";
      this.txtSensorTimeout.Size = new Size(111, 20);
      this.txtSensorTimeout.TabIndex = 42;
      this.label15.Location = new Point(8, 56);
      this.label15.Name = "label15";
      this.label15.Size = new Size(131, 15);
      this.label15.TabIndex = 41;
      this.label15.Tag = (object) "cfg_sensor_timeout";
      this.label15.Text = "Sensor timeout (sec):";
      this.label15.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseMultiplier.Location = new Point(145, 36);
      this.txtPulseMultiplier.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtPulseMultiplier.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.txtPulseMultiplier.Name = "txtPulseMultiplier";
      this.txtPulseMultiplier.Size = new Size(111, 20);
      this.txtPulseMultiplier.TabIndex = 40;
      this.txtPulseMultiplier.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.label14.Location = new Point(8, 36);
      this.label14.Name = "label14";
      this.label14.Size = new Size(131, 15);
      this.label14.TabIndex = 39;
      this.label14.Tag = (object) "cfg_pulse_multiplier";
      this.label14.Text = "Pulse multiplier:";
      this.label14.TextAlign = ContentAlignment.MiddleRight;
      this.ckboxPulseEnable.AutoSize = true;
      this.ckboxPulseEnable.Location = new Point(554, 331);
      this.ckboxPulseEnable.Name = "ckboxPulseEnable";
      this.ckboxPulseEnable.Size = new Size(93, 17);
      this.ckboxPulseEnable.TabIndex = 0;
      this.ckboxPulseEnable.Tag = (object) "CONFIG_ENABLE_PULSE";
      this.ckboxPulseEnable.Text = "Pulse enabled";
      this.ckboxPulseEnable.UseVisualStyleBackColor = true;
      this.ckboxFlowCheckEnabled.AutoSize = true;
      this.ckboxFlowCheckEnabled.Location = new Point(554, 349);
      this.ckboxFlowCheckEnabled.Name = "ckboxFlowCheckEnabled";
      this.ckboxFlowCheckEnabled.Size = new Size(122, 17);
      this.ckboxFlowCheckEnabled.TabIndex = 45;
      this.ckboxFlowCheckEnabled.Tag = (object) "CONFIG_FLOW_CHECK";
      this.ckboxFlowCheckEnabled.Text = "Flow check enabled";
      this.ckboxFlowCheckEnabled.UseVisualStyleBackColor = true;
      this.ckboxLogEnabled.AutoSize = true;
      this.ckboxLogEnabled.Location = new Point(554, 367);
      this.ckboxLogEnabled.Name = "ckboxLogEnabled";
      this.ckboxLogEnabled.Size = new Size(85, 17);
      this.ckboxLogEnabled.TabIndex = 47;
      this.ckboxLogEnabled.Tag = (object) "CONFIG_LOG_ENABLED";
      this.ckboxLogEnabled.Text = "Log enabled";
      this.ckboxLogEnabled.UseVisualStyleBackColor = true;
      this.ckboxIsMagnetDetected.AutoSize = true;
      this.ckboxIsMagnetDetected.Location = new Point(554, 385);
      this.ckboxIsMagnetDetected.Name = "ckboxIsMagnetDetected";
      this.ckboxIsMagnetDetected.Size = new Size(114, 17);
      this.ckboxIsMagnetDetected.TabIndex = 46;
      this.ckboxIsMagnetDetected.Tag = (object) "CONFIG_MAGNET_FOUND";
      this.ckboxIsMagnetDetected.Text = "Magnet was found";
      this.ckboxIsMagnetDetected.UseVisualStyleBackColor = true;
      this.label13.Location = new Point(551, 434);
      this.label13.Name = "label13";
      this.label13.Size = new Size(133, 15);
      this.label13.TabIndex = 43;
      this.label13.Tag = (object) "pulseReading";
      this.label13.Text = "Meter value";
      this.label13.TextAlign = ContentAlignment.MiddleLeft;
      this.txtPulseReading.Location = new Point(554, 452);
      this.txtPulseReading.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseReading.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtPulseReading.Name = "txtPulseReading";
      this.txtPulseReading.ReadOnly = true;
      this.txtPulseReading.Size = new Size(172, 20);
      this.txtPulseReading.TabIndex = 44;
      this.groupBox6.Controls.Add((Control) this.listWarnings);
      this.groupBox6.Location = new Point(755, 6);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new Size(192, 263);
      this.groupBox6.TabIndex = 46;
      this.groupBox6.TabStop = false;
      this.groupBox6.Tag = (object) "persistent_warning_flags";
      this.groupBox6.Text = "Warnings";
      this.listWarnings.CheckOnClick = true;
      this.listWarnings.FormattingEnabled = true;
      this.listWarnings.Location = new Point(3, 16);
      this.listWarnings.Name = "listWarnings";
      this.listWarnings.Size = new Size(186, 244);
      this.listWarnings.TabIndex = 0;
      this.groupBox7.Controls.Add((Control) this.txtObisPrimary);
      this.groupBox7.Controls.Add((Control) this.label29);
      this.groupBox7.Controls.Add((Control) this.txtManufacturerPrimary);
      this.groupBox7.Controls.Add((Control) this.txtMBusGenerationPrimary);
      this.groupBox7.Controls.Add((Control) this.txtSerialnumberPrimary);
      this.groupBox7.Controls.Add((Control) this.cboxMBusMediumPrimary);
      this.groupBox7.Controls.Add((Control) this.txtMBusAddressPrimary);
      this.groupBox7.Controls.Add((Control) this.label33);
      this.groupBox7.Controls.Add((Control) this.label25);
      this.groupBox7.Controls.Add((Control) this.label21);
      this.groupBox7.Controls.Add((Control) this.label20);
      this.groupBox7.Controls.Add((Control) this.label19);
      this.groupBox7.Location = new Point(6, 5);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new Size(262, 140);
      this.groupBox7.TabIndex = 47;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "EDC Modul (primary, factory number)";
      this.txtObisPrimary.Location = new Point(151, 115);
      this.txtObisPrimary.MaxLength = 2;
      this.txtObisPrimary.Name = "txtObisPrimary";
      this.txtObisPrimary.Size = new Size(102, 20);
      this.txtObisPrimary.TabIndex = 72;
      this.label29.Location = new Point(4, 117);
      this.label29.Name = "label29";
      this.label29.Size = new Size(141, 15);
      this.label29.TabIndex = 71;
      this.label29.Tag = (object) "cfg_obis";
      this.label29.Text = "Obis: 0x";
      this.label29.TextAlign = ContentAlignment.MiddleRight;
      this.txtManufacturerPrimary.Location = new Point(151, 95);
      this.txtManufacturerPrimary.MaxLength = 3;
      this.txtManufacturerPrimary.Name = "txtManufacturerPrimary";
      this.txtManufacturerPrimary.Size = new Size(102, 20);
      this.txtManufacturerPrimary.TabIndex = 70;
      this.txtMBusGenerationPrimary.Location = new Point(151, 75);
      this.txtMBusGenerationPrimary.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtMBusGenerationPrimary.Name = "txtMBusGenerationPrimary";
      this.txtMBusGenerationPrimary.Size = new Size(102, 20);
      this.txtMBusGenerationPrimary.TabIndex = 55;
      this.txtSerialnumberPrimary.Location = new Point(151, 14);
      this.txtSerialnumberPrimary.Maximum = new Decimal(new int[4]
      {
        99999999,
        0,
        0,
        0
      });
      this.txtSerialnumberPrimary.Name = "txtSerialnumberPrimary";
      this.txtSerialnumberPrimary.Size = new Size(102, 20);
      this.txtSerialnumberPrimary.TabIndex = 53;
      this.cboxMBusMediumPrimary.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMBusMediumPrimary.FormattingEnabled = true;
      this.cboxMBusMediumPrimary.Location = new Point(151, 54);
      this.cboxMBusMediumPrimary.Name = "cboxMBusMediumPrimary";
      this.cboxMBusMediumPrimary.Size = new Size(102, 21);
      this.cboxMBusMediumPrimary.TabIndex = 52;
      this.txtMBusAddressPrimary.Location = new Point(151, 34);
      this.txtMBusAddressPrimary.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtMBusAddressPrimary.Name = "txtMBusAddressPrimary";
      this.txtMBusAddressPrimary.Size = new Size(102, 20);
      this.txtMBusAddressPrimary.TabIndex = 50;
      this.label33.Location = new Point(4, 97);
      this.label33.Name = "label33";
      this.label33.Size = new Size(141, 15);
      this.label33.TabIndex = 69;
      this.label33.Tag = (object) "cfg_mbus_manid";
      this.label33.Text = "Manufacturer:";
      this.label33.TextAlign = ContentAlignment.MiddleRight;
      this.label25.Location = new Point(4, 76);
      this.label25.Name = "label25";
      this.label25.Size = new Size(141, 15);
      this.label25.TabIndex = 54;
      this.label25.Tag = (object) "cfg_mbus_version";
      this.label25.Text = "M-Bus generation:";
      this.label25.TextAlign = ContentAlignment.MiddleRight;
      this.label21.Location = new Point(4, 56);
      this.label21.Name = "label21";
      this.label21.Size = new Size(141, 15);
      this.label21.TabIndex = 51;
      this.label21.Tag = (object) "cfg_mbus_medium";
      this.label21.Text = "M-Bus medium:";
      this.label21.TextAlign = ContentAlignment.MiddleRight;
      this.label20.Location = new Point(4, 35);
      this.label20.Name = "label20";
      this.label20.Size = new Size(141, 15);
      this.label20.TabIndex = 49;
      this.label20.Tag = (object) "cfg_mbus_address";
      this.label20.Text = "M-Bus address:";
      this.label20.TextAlign = ContentAlignment.MiddleRight;
      this.label19.Location = new Point(4, 15);
      this.label19.Name = "label19";
      this.label19.Size = new Size(141, 15);
      this.label19.TabIndex = 47;
      this.label19.Tag = (object) "cfg_serial_primary";
      this.label19.Text = "M-Bus serial number:";
      this.label19.TextAlign = ContentAlignment.MiddleRight;
      this.toolTip.AutoPopDelay = 10000;
      this.toolTip.InitialDelay = 500;
      this.toolTip.ReshowDelay = 100;
      this.toolTip.ShowAlways = true;
      this.txtMinThreshold.Location = new Point(150, 53);
      this.txtMinThreshold.Maximum = new Decimal(new int[4]
      {
        (int) sbyte.MaxValue,
        0,
        0,
        0
      });
      this.txtMinThreshold.Minimum = new Decimal(new int[4]
      {
        128,
        0,
        0,
        int.MinValue
      });
      this.txtMinThreshold.Name = "txtMinThreshold";
      this.txtMinThreshold.Size = new Size(102, 20);
      this.txtMinThreshold.TabIndex = 68;
      this.toolTip.SetToolTip((Control) this.txtMinThreshold, "Equates to rollover of 99999999");
      this.label36.BackColor = Color.White;
      this.label36.Location = new Point(203, 7);
      this.label36.Name = "label36";
      this.label36.Size = new Size(84, 15);
      this.label36.TabIndex = 51;
      this.label36.Text = "Handler object:";
      this.label36.TextAlign = ContentAlignment.MiddleRight;
      this.cboxHandlerObject.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxHandlerObject.FormattingEnabled = true;
      this.cboxHandlerObject.Location = new Point(295, 5);
      this.cboxHandlerObject.Name = "cboxHandlerObject";
      this.cboxHandlerObject.Size = new Size(132, 21);
      this.cboxHandlerObject.TabIndex = 50;
      this.cboxHandlerObject.SelectedIndexChanged += new System.EventHandler(this.cboxHandlerObject_SelectedIndexChanged);
      this.panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel.Controls.Add((Control) this.groupBox4);
      this.panel.Controls.Add((Control) this.ckboxRemovalDetectionMasking);
      this.panel.Controls.Add((Control) this.groupBox2);
      this.panel.Controls.Add((Control) this.label13);
      this.panel.Controls.Add((Control) this.groupBox8);
      this.panel.Controls.Add((Control) this.txtPulseReading);
      this.panel.Controls.Add((Control) this.ckboxIsMagnetDetected);
      this.panel.Controls.Add((Control) this.ckboxPulseEnable);
      this.panel.Controls.Add((Control) this.gboxConstants);
      this.panel.Controls.Add((Control) this.btnCancel);
      this.panel.Controls.Add((Control) this.ckboxFlowCheckEnabled);
      this.panel.Controls.Add((Control) this.btnSave);
      this.panel.Controls.Add((Control) this.groupBox7);
      this.panel.Controls.Add((Control) this.groupBox1);
      this.panel.Controls.Add((Control) this.ckboxLogEnabled);
      this.panel.Controls.Add((Control) this.groupBox6);
      this.panel.Controls.Add((Control) this.groupBox3);
      this.panel.Location = new Point(0, 36);
      this.panel.Name = "panel";
      this.panel.Size = new Size(958, 517);
      this.panel.TabIndex = 52;
      this.groupBox4.Controls.Add((Control) this.txtObisSecondary);
      this.groupBox4.Controls.Add((Control) this.label30);
      this.groupBox4.Controls.Add((Control) this.txtManufacturerSecondary);
      this.groupBox4.Controls.Add((Control) this.txtMBusGenerationSecondary);
      this.groupBox4.Controls.Add((Control) this.txtSerialnumberSecondary);
      this.groupBox4.Controls.Add((Control) this.cboxMBusMediumSecondary);
      this.groupBox4.Controls.Add((Control) this.txtMBusAddressSecondary);
      this.groupBox4.Controls.Add((Control) this.label32);
      this.groupBox4.Controls.Add((Control) this.label35);
      this.groupBox4.Controls.Add((Control) this.label38);
      this.groupBox4.Controls.Add((Control) this.label42);
      this.groupBox4.Controls.Add((Control) this.label50);
      this.groupBox4.Location = new Point(6, 148);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(262, 140);
      this.groupBox4.TabIndex = 73;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Water meter (secondary, used in M-Bus header)";
      this.txtObisSecondary.Location = new Point(151, 115);
      this.txtObisSecondary.MaxLength = 2;
      this.txtObisSecondary.Name = "txtObisSecondary";
      this.txtObisSecondary.Size = new Size(102, 20);
      this.txtObisSecondary.TabIndex = 72;
      this.label30.Location = new Point(4, 117);
      this.label30.Name = "label30";
      this.label30.Size = new Size(141, 15);
      this.label30.TabIndex = 71;
      this.label30.Tag = (object) "cfg_obis_secondary";
      this.label30.Text = "Obis: 0x";
      this.label30.TextAlign = ContentAlignment.MiddleRight;
      this.txtManufacturerSecondary.Location = new Point(151, 95);
      this.txtManufacturerSecondary.MaxLength = 3;
      this.txtManufacturerSecondary.Name = "txtManufacturerSecondary";
      this.txtManufacturerSecondary.Size = new Size(102, 20);
      this.txtManufacturerSecondary.TabIndex = 70;
      this.txtMBusGenerationSecondary.Location = new Point(151, 75);
      this.txtMBusGenerationSecondary.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtMBusGenerationSecondary.Name = "txtMBusGenerationSecondary";
      this.txtMBusGenerationSecondary.Size = new Size(102, 20);
      this.txtMBusGenerationSecondary.TabIndex = 55;
      this.txtSerialnumberSecondary.Location = new Point(151, 14);
      this.txtSerialnumberSecondary.Maximum = new Decimal(new int[4]
      {
        99999999,
        0,
        0,
        0
      });
      this.txtSerialnumberSecondary.Name = "txtSerialnumberSecondary";
      this.txtSerialnumberSecondary.Size = new Size(102, 20);
      this.txtSerialnumberSecondary.TabIndex = 53;
      this.cboxMBusMediumSecondary.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMBusMediumSecondary.FormattingEnabled = true;
      this.cboxMBusMediumSecondary.Location = new Point(151, 54);
      this.cboxMBusMediumSecondary.Name = "cboxMBusMediumSecondary";
      this.cboxMBusMediumSecondary.Size = new Size(102, 21);
      this.cboxMBusMediumSecondary.TabIndex = 52;
      this.txtMBusAddressSecondary.Location = new Point(151, 34);
      this.txtMBusAddressSecondary.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtMBusAddressSecondary.Name = "txtMBusAddressSecondary";
      this.txtMBusAddressSecondary.Size = new Size(102, 20);
      this.txtMBusAddressSecondary.TabIndex = 50;
      this.label32.Location = new Point(4, 97);
      this.label32.Name = "label32";
      this.label32.Size = new Size(141, 15);
      this.label32.TabIndex = 69;
      this.label32.Tag = (object) "cfg_mbus_manid_secondary";
      this.label32.Text = "Manufacturer:";
      this.label32.TextAlign = ContentAlignment.MiddleRight;
      this.label35.Location = new Point(4, 76);
      this.label35.Name = "label35";
      this.label35.Size = new Size(141, 15);
      this.label35.TabIndex = 54;
      this.label35.Tag = (object) "cfg_mbus_version_secondary";
      this.label35.Text = "M-Bus generation:";
      this.label35.TextAlign = ContentAlignment.MiddleRight;
      this.label38.Location = new Point(4, 56);
      this.label38.Name = "label38";
      this.label38.Size = new Size(141, 15);
      this.label38.TabIndex = 51;
      this.label38.Tag = (object) "cfg_mbus_medium_secondary";
      this.label38.Text = "M-Bus medium:";
      this.label38.TextAlign = ContentAlignment.MiddleRight;
      this.label42.Location = new Point(4, 35);
      this.label42.Name = "label42";
      this.label42.Size = new Size(141, 15);
      this.label42.TabIndex = 49;
      this.label42.Tag = (object) "cfg_mbus_address_secondary";
      this.label42.Text = "M-Bus address:";
      this.label42.TextAlign = ContentAlignment.MiddleRight;
      this.label50.Location = new Point(4, 15);
      this.label50.Name = "label50";
      this.label50.Size = new Size(141, 15);
      this.label50.TabIndex = 47;
      this.label50.Tag = (object) "cfg_serial_secondary";
      this.label50.Text = "M-Bus serial number:";
      this.label50.TextAlign = ContentAlignment.MiddleRight;
      this.ckboxRemovalDetectionMasking.AutoSize = true;
      this.ckboxRemovalDetectionMasking.Location = new Point(554, 404);
      this.ckboxRemovalDetectionMasking.Name = "ckboxRemovalDetectionMasking";
      this.ckboxRemovalDetectionMasking.Size = new Size(157, 17);
      this.ckboxRemovalDetectionMasking.TabIndex = 69;
      this.ckboxRemovalDetectionMasking.Tag = (object) "CONFIG_MAGNET_MASK";
      this.ckboxRemovalDetectionMasking.Text = "Removal detection masking";
      this.ckboxRemovalDetectionMasking.UseVisualStyleBackColor = true;
      this.groupBox2.Controls.Add((Control) this.label23);
      this.groupBox2.Controls.Add((Control) this.txtMinThreshold);
      this.groupBox2.Controls.Add((Control) this.label24);
      this.groupBox2.Controls.Add((Control) this.txtMaxThreshold);
      this.groupBox2.Controls.Add((Control) this.label40);
      this.groupBox2.Controls.Add((Control) this.txtErrorThreshold);
      this.groupBox2.Controls.Add((Control) this.label39);
      this.groupBox2.Controls.Add((Control) this.txtB_offset);
      this.groupBox2.Controls.Add((Control) this.label41);
      this.groupBox2.Controls.Add((Control) this.txtAmplitudeLimit);
      this.groupBox2.Location = new Point(274, 391);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(261, 120);
      this.groupBox2.TabIndex = 67;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Coil settings";
      this.label23.Location = new Point(32, 53);
      this.label23.Name = "label23";
      this.label23.Size = new Size(115, 15);
      this.label23.TabIndex = 67;
      this.label23.Tag = (object) "cfg_coil_min_threshold";
      this.label23.Text = "Min threshold:";
      this.label23.TextAlign = ContentAlignment.MiddleRight;
      this.label24.Location = new Point(32, 33);
      this.label24.Name = "label24";
      this.label24.Size = new Size(115, 15);
      this.label24.TabIndex = 65;
      this.label24.Tag = (object) "cfg_coil_max_threshold";
      this.label24.Text = "Max threshold:";
      this.label24.TextAlign = ContentAlignment.MiddleRight;
      this.txtMaxThreshold.Location = new Point(150, 33);
      this.txtMaxThreshold.Maximum = new Decimal(new int[4]
      {
        (int) sbyte.MaxValue,
        0,
        0,
        0
      });
      this.txtMaxThreshold.Minimum = new Decimal(new int[4]
      {
        128,
        0,
        0,
        int.MinValue
      });
      this.txtMaxThreshold.Name = "txtMaxThreshold";
      this.txtMaxThreshold.Size = new Size(102, 20);
      this.txtMaxThreshold.TabIndex = 66;
      this.label40.Location = new Point(32, 13);
      this.label40.Name = "label40";
      this.label40.Size = new Size(115, 15);
      this.label40.TabIndex = 63;
      this.label40.Tag = (object) "cfg_coil_error_threshold";
      this.label40.Text = "Error threshold:";
      this.label40.TextAlign = ContentAlignment.MiddleRight;
      this.txtErrorThreshold.Location = new Point(150, 13);
      this.txtErrorThreshold.Maximum = new Decimal(new int[4]
      {
        (int) sbyte.MaxValue,
        0,
        0,
        0
      });
      this.txtErrorThreshold.Minimum = new Decimal(new int[4]
      {
        128,
        0,
        0,
        int.MinValue
      });
      this.txtErrorThreshold.Name = "txtErrorThreshold";
      this.txtErrorThreshold.Size = new Size(102, 20);
      this.txtErrorThreshold.TabIndex = 64;
      this.label39.Location = new Point(32, 93);
      this.label39.Name = "label39";
      this.label39.Size = new Size(115, 15);
      this.label39.TabIndex = 56;
      this.label39.Tag = (object) "cfg_coil_b_offset";
      this.label39.Text = "B offset:";
      this.label39.TextAlign = ContentAlignment.MiddleRight;
      this.txtB_offset.Location = new Point(150, 93);
      this.txtB_offset.Maximum = new Decimal(new int[4]
      {
        (int) sbyte.MaxValue,
        0,
        0,
        0
      });
      this.txtB_offset.Minimum = new Decimal(new int[4]
      {
        128,
        0,
        0,
        int.MinValue
      });
      this.txtB_offset.Name = "txtB_offset";
      this.txtB_offset.Size = new Size(102, 20);
      this.txtB_offset.TabIndex = 57;
      this.label41.Location = new Point(32, 73);
      this.label41.Name = "label41";
      this.label41.Size = new Size(115, 15);
      this.label41.TabIndex = 39;
      this.label41.Tag = (object) "cfg_coil_amplitude_limit";
      this.label41.Text = "Amplitude limit:";
      this.label41.TextAlign = ContentAlignment.MiddleRight;
      this.txtAmplitudeLimit.Location = new Point(150, 73);
      this.txtAmplitudeLimit.Maximum = new Decimal(new int[4]
      {
        (int) sbyte.MaxValue,
        0,
        0,
        0
      });
      this.txtAmplitudeLimit.Minimum = new Decimal(new int[4]
      {
        128,
        0,
        0,
        int.MinValue
      });
      this.txtAmplitudeLimit.Name = "txtAmplitudeLimit";
      this.txtAmplitudeLimit.Size = new Size(102, 20);
      this.txtAmplitudeLimit.TabIndex = 40;
      this.groupBox8.Controls.Add((Control) this.listHardwareErrors);
      this.groupBox8.Location = new Point(755, 269);
      this.groupBox8.Name = "groupBox8";
      this.groupBox8.Size = new Size(192, 174);
      this.groupBox8.TabIndex = 48;
      this.groupBox8.TabStop = false;
      this.groupBox8.Tag = (object) "hwStatusFlags";
      this.groupBox8.Text = "Hardware errors";
      this.listHardwareErrors.CheckOnClick = true;
      this.listHardwareErrors.FormattingEnabled = true;
      this.listHardwareErrors.Location = new Point(3, 14);
      this.listHardwareErrors.Name = "listHardwareErrors";
      this.listHardwareErrors.Size = new Size(186, 154);
      this.listHardwareErrors.TabIndex = 0;
      this.gboxConstants.Controls.Add((Control) this.label60);
      this.gboxConstants.Controls.Add((Control) this.label61);
      this.gboxConstants.Controls.Add((Control) this.label51);
      this.gboxConstants.Controls.Add((Control) this.label52);
      this.gboxConstants.Controls.Add((Control) this.label53);
      this.gboxConstants.Controls.Add((Control) this.label54);
      this.gboxConstants.Controls.Add((Control) this.label55);
      this.gboxConstants.Controls.Add((Control) this.label56);
      this.gboxConstants.Controls.Add((Control) this.label49);
      this.gboxConstants.Controls.Add((Control) this.label48);
      this.gboxConstants.Controls.Add((Control) this.label47);
      this.gboxConstants.Controls.Add((Control) this.label46);
      this.gboxConstants.Controls.Add((Control) this.label45);
      this.gboxConstants.Controls.Add((Control) this.label44);
      this.gboxConstants.Controls.Add((Control) this.label43);
      this.gboxConstants.Controls.Add((Control) this.txtPulseErrorThreshold);
      this.gboxConstants.Controls.Add((Control) this.txtUARTwatchdog);
      this.gboxConstants.Controls.Add((Control) this.txtBurstLimit);
      this.gboxConstants.Controls.Add((Control) this.txtBurstDiff);
      this.gboxConstants.Controls.Add((Control) this.txtUndersizeLimit);
      this.gboxConstants.Controls.Add((Control) this.txtUndersizeDiff);
      this.gboxConstants.Controls.Add((Control) this.txtOversizeLimit);
      this.gboxConstants.Controls.Add((Control) this.txtOversizeDiff);
      this.gboxConstants.Controls.Add((Control) this.txtPulseUnbackLimit);
      this.gboxConstants.Controls.Add((Control) this.txtPulseBackLimit);
      this.gboxConstants.Controls.Add((Control) this.txtPulseLeakUpper);
      this.gboxConstants.Controls.Add((Control) this.txtPulseLeakLower);
      this.gboxConstants.Controls.Add((Control) this.txtPulseUnleakLimit);
      this.gboxConstants.Controls.Add((Control) this.txtPulseLeakLimit);
      this.gboxConstants.Controls.Add((Control) this.txtPulseBlockLimit);
      this.gboxConstants.Location = new Point(546, 6);
      this.gboxConstants.Name = "gboxConstants";
      this.gboxConstants.Size = new Size(204, 323);
      this.gboxConstants.TabIndex = 52;
      this.gboxConstants.TabStop = false;
      this.gboxConstants.Text = "Constants";
      this.label60.Location = new Point(5, 296);
      this.label60.Margin = new Padding(3, 0, 0, 0);
      this.label60.Name = "label60";
      this.label60.Size = new Size(136, 15);
      this.label60.TabIndex = 89;
      this.label60.Tag = (object) "cfg_pulse_error_threshold";
      this.label60.Text = "Pulse error threshold:";
      this.label60.TextAlign = ContentAlignment.MiddleRight;
      this.label61.Location = new Point(5, 276);
      this.label61.Margin = new Padding(3, 0, 0, 0);
      this.label61.Name = "label61";
      this.label61.Size = new Size(136, 15);
      this.label61.TabIndex = 87;
      this.label61.Tag = (object) "cfg_uart_watchdog";
      this.label61.Text = "UART watchdog:";
      this.label61.TextAlign = ContentAlignment.MiddleRight;
      this.label51.Location = new Point(5, 256);
      this.label51.Margin = new Padding(3, 0, 0, 0);
      this.label51.Name = "label51";
      this.label51.Size = new Size(136, 15);
      this.label51.TabIndex = 85;
      this.label51.Tag = (object) "cfg_burst_limit";
      this.label51.Text = "Burst limit:";
      this.label51.TextAlign = ContentAlignment.MiddleRight;
      this.label52.Location = new Point(5, 236);
      this.label52.Margin = new Padding(3, 0, 0, 0);
      this.label52.Name = "label52";
      this.label52.Size = new Size(136, 15);
      this.label52.TabIndex = 83;
      this.label52.Tag = (object) "cfg_burst_diff";
      this.label52.Text = "Burst diff:";
      this.label52.TextAlign = ContentAlignment.MiddleRight;
      this.label53.Location = new Point(5, 216);
      this.label53.Margin = new Padding(3, 0, 0, 0);
      this.label53.Name = "label53";
      this.label53.Size = new Size(136, 15);
      this.label53.TabIndex = 81;
      this.label53.Tag = (object) "cfg_undersize_limit";
      this.label53.Text = "Undersize limit:";
      this.label53.TextAlign = ContentAlignment.MiddleRight;
      this.label54.Location = new Point(5, 196);
      this.label54.Margin = new Padding(3, 0, 0, 0);
      this.label54.Name = "label54";
      this.label54.Size = new Size(136, 15);
      this.label54.TabIndex = 79;
      this.label54.Tag = (object) "cfg_undersize_diff";
      this.label54.Text = "Undersize diff:";
      this.label54.TextAlign = ContentAlignment.MiddleRight;
      this.label55.Location = new Point(5, 176);
      this.label55.Margin = new Padding(3, 0, 0, 0);
      this.label55.Name = "label55";
      this.label55.Size = new Size(136, 15);
      this.label55.TabIndex = 77;
      this.label55.Tag = (object) "cfg_oversize_limit";
      this.label55.Text = "Oversize limit:";
      this.label55.TextAlign = ContentAlignment.MiddleRight;
      this.label56.Location = new Point(5, 156);
      this.label56.Margin = new Padding(3, 0, 0, 0);
      this.label56.Name = "label56";
      this.label56.Size = new Size(136, 15);
      this.label56.TabIndex = 75;
      this.label56.Tag = (object) "cfg_oversize_diff";
      this.label56.Text = "Oversize diff:";
      this.label56.TextAlign = ContentAlignment.MiddleRight;
      this.label49.Location = new Point(5, 136);
      this.label49.Margin = new Padding(3, 0, 0, 0);
      this.label49.Name = "label49";
      this.label49.Size = new Size(136, 15);
      this.label49.TabIndex = 73;
      this.label49.Tag = (object) "cfg_pulse_unback_limit";
      this.label49.Text = "Pulse unback limit:";
      this.label49.TextAlign = ContentAlignment.MiddleRight;
      this.label48.Location = new Point(5, 116);
      this.label48.Margin = new Padding(3, 0, 0, 0);
      this.label48.Name = "label48";
      this.label48.Size = new Size(136, 15);
      this.label48.TabIndex = 71;
      this.label48.Tag = (object) "cfg_pulse_back_limit";
      this.label48.Text = "Pulse back limit:";
      this.label48.TextAlign = ContentAlignment.MiddleRight;
      this.label47.Location = new Point(5, 96);
      this.label47.Margin = new Padding(3, 0, 0, 0);
      this.label47.Name = "label47";
      this.label47.Size = new Size(136, 15);
      this.label47.TabIndex = 69;
      this.label47.Tag = (object) "cfg_pulse_leak_upper";
      this.label47.Text = "Pulse leak upper:";
      this.label47.TextAlign = ContentAlignment.MiddleRight;
      this.label46.Location = new Point(5, 76);
      this.label46.Margin = new Padding(3, 0, 0, 0);
      this.label46.Name = "label46";
      this.label46.Size = new Size(136, 15);
      this.label46.TabIndex = 67;
      this.label46.Tag = (object) "cfg_pulse_leak_lower";
      this.label46.Text = "Pulse leak lower:";
      this.label46.TextAlign = ContentAlignment.MiddleRight;
      this.label45.Location = new Point(5, 57);
      this.label45.Margin = new Padding(3, 0, 0, 0);
      this.label45.Name = "label45";
      this.label45.Size = new Size(136, 15);
      this.label45.TabIndex = 65;
      this.label45.Tag = (object) "cfg_pulse_unleak_limit";
      this.label45.Text = "Pulse unleak limit:";
      this.label45.TextAlign = ContentAlignment.MiddleRight;
      this.label44.Location = new Point(5, 36);
      this.label44.Margin = new Padding(3, 0, 0, 0);
      this.label44.Name = "label44";
      this.label44.Size = new Size(136, 15);
      this.label44.TabIndex = 63;
      this.label44.Tag = (object) "cfg_pulse_leak_limit";
      this.label44.Text = "Pulse leak limit:";
      this.label44.TextAlign = ContentAlignment.MiddleRight;
      this.label43.Location = new Point(5, 16);
      this.label43.Margin = new Padding(3, 0, 0, 0);
      this.label43.Name = "label43";
      this.label43.Size = new Size(136, 15);
      this.label43.TabIndex = 61;
      this.label43.Tag = (object) "cfg_pulse_block_limit";
      this.label43.Text = "Pulse block limit:";
      this.label43.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseErrorThreshold.Location = new Point(142, 295);
      this.txtPulseErrorThreshold.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtPulseErrorThreshold.Name = "txtPulseErrorThreshold";
      this.txtPulseErrorThreshold.Size = new Size(57, 20);
      this.txtPulseErrorThreshold.TabIndex = 90;
      this.txtUARTwatchdog.Location = new Point(142, 275);
      this.txtUARTwatchdog.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtUARTwatchdog.Name = "txtUARTwatchdog";
      this.txtUARTwatchdog.Size = new Size(57, 20);
      this.txtUARTwatchdog.TabIndex = 88;
      this.txtBurstLimit.Location = new Point(142, (int) byte.MaxValue);
      this.txtBurstLimit.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtBurstLimit.Name = "txtBurstLimit";
      this.txtBurstLimit.Size = new Size(57, 20);
      this.txtBurstLimit.TabIndex = 86;
      this.txtBurstDiff.Location = new Point(142, 235);
      this.txtBurstDiff.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtBurstDiff.Name = "txtBurstDiff";
      this.txtBurstDiff.Size = new Size(57, 20);
      this.txtBurstDiff.TabIndex = 84;
      this.txtUndersizeLimit.Location = new Point(142, 215);
      this.txtUndersizeLimit.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtUndersizeLimit.Name = "txtUndersizeLimit";
      this.txtUndersizeLimit.Size = new Size(57, 20);
      this.txtUndersizeLimit.TabIndex = 82;
      this.txtUndersizeDiff.Location = new Point(142, 195);
      this.txtUndersizeDiff.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtUndersizeDiff.Name = "txtUndersizeDiff";
      this.txtUndersizeDiff.Size = new Size(57, 20);
      this.txtUndersizeDiff.TabIndex = 80;
      this.txtOversizeLimit.Location = new Point(142, 175);
      this.txtOversizeLimit.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtOversizeLimit.Name = "txtOversizeLimit";
      this.txtOversizeLimit.Size = new Size(57, 20);
      this.txtOversizeLimit.TabIndex = 78;
      this.txtOversizeDiff.Location = new Point(142, 155);
      this.txtOversizeDiff.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtOversizeDiff.Name = "txtOversizeDiff";
      this.txtOversizeDiff.Size = new Size(57, 20);
      this.txtOversizeDiff.TabIndex = 76;
      this.txtPulseUnbackLimit.Location = new Point(142, 135);
      this.txtPulseUnbackLimit.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseUnbackLimit.Name = "txtPulseUnbackLimit";
      this.txtPulseUnbackLimit.Size = new Size(57, 20);
      this.txtPulseUnbackLimit.TabIndex = 74;
      this.txtPulseBackLimit.Location = new Point(142, 115);
      this.txtPulseBackLimit.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseBackLimit.Name = "txtPulseBackLimit";
      this.txtPulseBackLimit.Size = new Size(57, 20);
      this.txtPulseBackLimit.TabIndex = 72;
      this.txtPulseLeakUpper.Location = new Point(142, 95);
      this.txtPulseLeakUpper.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseLeakUpper.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtPulseLeakUpper.Name = "txtPulseLeakUpper";
      this.txtPulseLeakUpper.Size = new Size(57, 20);
      this.txtPulseLeakUpper.TabIndex = 70;
      this.txtPulseLeakLower.Location = new Point(142, 75);
      this.txtPulseLeakLower.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseLeakLower.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtPulseLeakLower.Name = "txtPulseLeakLower";
      this.txtPulseLeakLower.Size = new Size(57, 20);
      this.txtPulseLeakLower.TabIndex = 68;
      this.txtPulseUnleakLimit.Location = new Point(142, 55);
      this.txtPulseUnleakLimit.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseUnleakLimit.Name = "txtPulseUnleakLimit";
      this.txtPulseUnleakLimit.Size = new Size(57, 20);
      this.txtPulseUnleakLimit.TabIndex = 66;
      this.txtPulseLeakLimit.Location = new Point(142, 35);
      this.txtPulseLeakLimit.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseLeakLimit.Name = "txtPulseLeakLimit";
      this.txtPulseLeakLimit.Size = new Size(57, 20);
      this.txtPulseLeakLimit.TabIndex = 64;
      this.txtPulseBlockLimit.Location = new Point(142, 15);
      this.txtPulseBlockLimit.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseBlockLimit.Name = "txtPulseBlockLimit";
      this.txtPulseBlockLimit.Size = new Size(57, 20);
      this.txtPulseBlockLimit.TabIndex = 62;
      this.ckboxShowParameterNamesUsedInFirmware.AutoSize = true;
      this.ckboxShowParameterNamesUsedInFirmware.BackColor = Color.White;
      this.ckboxShowParameterNamesUsedInFirmware.Location = new Point(471, 7);
      this.ckboxShowParameterNamesUsedInFirmware.Name = "ckboxShowParameterNamesUsedInFirmware";
      this.ckboxShowParameterNamesUsedInFirmware.Size = new Size(216, 17);
      this.ckboxShowParameterNamesUsedInFirmware.TabIndex = 53;
      this.ckboxShowParameterNamesUsedInFirmware.Text = "Show parameter names used in firmware";
      this.ckboxShowParameterNamesUsedInFirmware.UseVisualStyleBackColor = false;
      this.ckboxShowParameterNamesUsedInFirmware.CheckedChanged += new System.EventHandler(this.ckboxShowParameterNamesUsedInFirmware_CheckedChanged);
      this.zennerCoroprateDesign1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(958, 36);
      this.zennerCoroprateDesign1.TabIndex = 20;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(958, 552);
      this.Controls.Add((Control) this.ckboxShowParameterNamesUsedInFirmware);
      this.Controls.Add((Control) this.panel);
      this.Controls.Add((Control) this.label36);
      this.Controls.Add((Control) this.cboxHandlerObject);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConfiguratorEdcWired);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Configurator for EDC_wired";
      this.Load += new System.EventHandler(this.Configurator_Load);
      this.txtMeterID.EndInit();
      this.txtHardwareTypeID.EndInit();
      this.txtMeterInfoID.EndInit();
      this.txtBaseTypeID.EndInit();
      this.txtMeterTypeID.EndInit();
      this.txtSapMaterialNumber.EndInit();
      this.txtSapProductionOrderNumber.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.txtPulseActivateRadio.EndInit();
      this.groupBox3.ResumeLayout(false);
      this.txtDepassPeriod.EndInit();
      this.txtDepassTimeout.EndInit();
      this.txtPulseoutPPP.EndInit();
      this.txtPulseoutWidth.EndInit();
      this.txtCogCount.EndInit();
      this.txtPulsePeriod.EndInit();
      this.txtTimeZone.EndInit();
      this.txtSensorTimeout.EndInit();
      this.txtPulseMultiplier.EndInit();
      this.txtPulseReading.EndInit();
      this.groupBox6.ResumeLayout(false);
      this.groupBox7.ResumeLayout(false);
      this.groupBox7.PerformLayout();
      this.txtMBusGenerationPrimary.EndInit();
      this.txtSerialnumberPrimary.EndInit();
      this.txtMBusAddressPrimary.EndInit();
      this.txtMinThreshold.EndInit();
      this.panel.ResumeLayout(false);
      this.panel.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.txtMBusGenerationSecondary.EndInit();
      this.txtSerialnumberSecondary.EndInit();
      this.txtMBusAddressSecondary.EndInit();
      this.groupBox2.ResumeLayout(false);
      this.txtMaxThreshold.EndInit();
      this.txtErrorThreshold.EndInit();
      this.txtB_offset.EndInit();
      this.txtAmplitudeLimit.EndInit();
      this.groupBox8.ResumeLayout(false);
      this.gboxConstants.ResumeLayout(false);
      this.txtPulseErrorThreshold.EndInit();
      this.txtUARTwatchdog.EndInit();
      this.txtBurstLimit.EndInit();
      this.txtBurstDiff.EndInit();
      this.txtUndersizeLimit.EndInit();
      this.txtUndersizeDiff.EndInit();
      this.txtOversizeLimit.EndInit();
      this.txtOversizeDiff.EndInit();
      this.txtPulseUnbackLimit.EndInit();
      this.txtPulseBackLimit.EndInit();
      this.txtPulseLeakUpper.EndInit();
      this.txtPulseLeakLower.EndInit();
      this.txtPulseUnleakLimit.EndInit();
      this.txtPulseLeakLimit.EndInit();
      this.txtPulseBlockLimit.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
