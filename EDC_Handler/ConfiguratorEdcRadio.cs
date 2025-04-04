// Decompiled with JetBrains decompiler
// Type: EDC_Handler.ConfiguratorEdcRadio
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
  public class ConfiguratorEdcRadio : Form
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
    private GroupBox gboxRadio;
    private Label label9;
    private NumericUpDown txtRadioTransmitInterval;
    private Label label8;
    private ComboBox cboxRadioMode;
    private GroupBox groupBox3;
    private CheckBox ckboxPulseEnable;
    private Label label10;
    private ComboBox cboxRadioListType;
    private Label label12;
    private NumericUpDown txtTimeZone;
    private CheckBox ckboxRadioEnabled;
    private Label label13;
    private NumericUpDown txtPulseReading;
    private NumericUpDown txtPulseMultiplier;
    private Label label14;
    private GroupBox groupBox6;
    private CheckedListBox listWarnings;
    private NumericUpDown txtSensorTimeout;
    private Label label15;
    private Label label16;
    private ComboBox cboxRadioPower;
    private Label label17;
    private NumericUpDown txtRadioFrequencyOffset;
    private TextBox txtAESkey;
    private GroupBox groupBox7;
    private Label label19;
    private Label label20;
    private NumericUpDown txtMBusAddress;
    private ComboBox cboxMBusMedium;
    private Label label21;
    private DateTimePicker txtDueDate;
    private Label label22;
    private CheckBox ckboxIsMagnetDetected;
    private CheckBox ckboxFlowCheckEnabled;
    private CheckBox ckboxLogEnabled;
    private NumericUpDown txtSerialnumberMBus;
    private ToolTip toolTip;
    private Label label11;
    private Label label26;
    private NumericUpDown txtPulsePeriod;
    private TextBox txtBewertungsfaktor;
    private Label label25;
    private NumericUpDown txtMBusGeneration;
    private CheckBox ckboxEnableInstallationPacket;
    private CheckBox ckboxSynchronousTransmissionMode;
    private CheckBox ckboxEnableEncryption;
    private CheckBox ckboxUseLongPacketHeader;
    private Label label27;
    private NumericUpDown txtMBusGenerationSec;
    private NumericUpDown txtSerialnumberSec;
    private ComboBox cboxMBusMediumSec;
    private Label label28;
    private Label label29;
    private Label label30;
    private TextBox txtSerialnumberFull;
    private Label label31;
    private NumericUpDown txtSerialnumberRadioMinol;
    private Label label32;
    private TextBox txtManufacturerSec;
    private TextBox txtManufacturer;
    private Label label33;
    private Label label36;
    private ComboBox cboxHandlerObject;
    private Panel panel;
    private Label label34;
    private NumericUpDown txtPulseActivateRadio;
    private Label label37;
    private NumericUpDown txtCogCount;
    private Label label38;
    private ComboBox cboxRadioScenario;
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
    private Label label59;
    private NumericUpDown txtRadioInstallCount;
    private Label label58;
    private NumericUpDown txtRadioErrorInterval;
    private Label label57;
    private NumericUpDown txtRadioInstallInterval;
    private Label label60;
    private NumericUpDown txtPulseErrorThreshold;
    private Label label61;
    private NumericUpDown txtUARTwatchdog;
    private DateTimePicker txtSystemTime;
    private Label label62;
    private DateTimePicker txtBatteryEndDate;
    private Label label63;
    private GroupBox gboxLongHeader;
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
    private Label label18;
    private NumericUpDown txtMBusAddressSec;
    private Label label35;

    public ConfiguratorEdcRadio() => this.InitializeComponent();

    private void Configurator_Load(object sender, EventArgs e)
    {
      this.InitializeForm();
      this.btnSave.Enabled = UserManager.CheckPermission("EDC_Handler.View.Expert");
    }

    internal static void ShowDialog(Form owner, EDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (ConfiguratorEdcRadio configuratorEdcRadio = new ConfiguratorEdcRadio())
      {
        if (MyFunctions.WorkMeter != null)
          configuratorEdcRadio.tempWorkMeter = MyFunctions.WorkMeter.DeepCopy();
        if (MyFunctions.TypeMeter != null)
          configuratorEdcRadio.tempTypeMeter = MyFunctions.TypeMeter.DeepCopy();
        if (MyFunctions.BackupMeter != null)
          configuratorEdcRadio.tempBackupMeter = MyFunctions.BackupMeter.DeepCopy();
        if (MyFunctions.ConnectedMeter != null)
          configuratorEdcRadio.tempConnectedMeter = MyFunctions.ConnectedMeter.DeepCopy();
        if (configuratorEdcRadio.ShowDialog((IWin32Window) owner) != DialogResult.OK)
          return;
        MyFunctions.WorkMeter = configuratorEdcRadio.tempWorkMeter;
        MyFunctions.TypeMeter = configuratorEdcRadio.tempTypeMeter;
        MyFunctions.BackupMeter = configuratorEdcRadio.tempBackupMeter;
        MyFunctions.ConnectedMeter = configuratorEdcRadio.tempConnectedMeter;
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
        MBusListStructure mbusListStructure = handlerMeter.GetMBusListStructure();
        if (mbusListStructure != null)
          this.cboxRadioListType.DataSource = (object) mbusListStructure.GetListNames();
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
        byte? pulseMultiplier = handlerMeter.GetPulseMultiplier();
        ushort? sensorTimeout = handlerMeter.GetSensorTimeout();
        DateTime? dueDate = handlerMeter.GetDueDate();
        bool? coilSampling = handlerMeter.GetCoilSampling();
        bool? checkIntervalState = handlerMeter.GetFlowCheckIntervalState();
        bool? magnetDetectionState = handlerMeter.GetMagnetDetectionState();
        bool? dataLoggingState = handlerMeter.GetDataLoggingState();
        string mbusListType = handlerMeter.GetMBusListType();
        Warning? warnings = handlerMeter.GetWarnings();
        int? timeZone = handlerMeter.GetTimeZone();
        int? meterValue = handlerMeter.GetMeterValue();
        ushort? bewertungsfaktor = handlerMeter.GetBewertungsfaktor();
        ushort? pulsePeriod = handlerMeter.GetPulsePeriod();
        byte? generationPrimary = handlerMeter.GetMBusGenerationPrimary();
        string manufacturerPrimary = handlerMeter.GetManufacturerPrimary();
        string serialnumberFull = handlerMeter.GetSerialnumberFull();
        uint? serialnumberSecondary = handlerMeter.GetSerialnumberSecondary();
        byte? addressSecondary = handlerMeter.GetMBusAddressSecondary();
        string manufacturerSecondary = handlerMeter.GetManufacturerSecondary();
        byte? generationSecondary = handlerMeter.GetMBusGenerationSecondary();
        MBusDeviceType? mediumSecondary = handlerMeter.GetMediumSecondary();
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
        this.txtSerialnumberMBus.Value = (Decimal) (serialnumberPrimary.HasValue ? serialnumberPrimary.Value : 0U);
        this.txtSerialnumberSec.Value = (Decimal) (serialnumberSecondary.HasValue ? serialnumberSecondary.Value : 0U);
        this.txtManufacturer.Text = manufacturerPrimary ?? string.Empty;
        this.txtManufacturerSec.Text = manufacturerSecondary ?? string.Empty;
        this.txtSerialnumberFull.Text = serialnumberFull ?? string.Empty;
        this.txtMBusGenerationSec.Value = (Decimal) (generationSecondary.HasValue ? (int) generationSecondary.Value : 0);
        this.txtMBusAddress.Value = (Decimal) (mbusAddressPrimary.HasValue ? (int) mbusAddressPrimary.Value : 0);
        this.txtMBusAddressSec.Value = (Decimal) (addressSecondary.HasValue ? (int) addressSecondary.Value : 0);
        this.txtPulseMultiplier.Value = (Decimal) (pulseMultiplier.HasValue ? (int) pulseMultiplier.Value : 1);
        this.txtSensorTimeout.Value = (Decimal) (sensorTimeout.HasValue ? (int) sensorTimeout.Value : 0);
        this.txtDueDate.Value = dueDate.HasValue ? dueDate.Value : EDC_HandlerFunctions.DateTimeNull;
        this.txtTimeZone.Value = (Decimal) (timeZone.HasValue ? timeZone.Value : 0);
        this.txtPulseReading.Value = (Decimal) (meterValue.HasValue ? meterValue.Value : 0);
        this.txtBewertungsfaktor.Text = bewertungsfaktor.HasValue ? "0x" + bewertungsfaktor.Value.ToString("X4") : string.Empty;
        this.txtPulsePeriod.Value = (Decimal) (pulsePeriod.HasValue ? (int) pulsePeriod.Value : 0);
        this.txtMBusGeneration.Value = (Decimal) (generationPrimary.HasValue ? (int) generationPrimary.Value : 0);
        this.cboxMBusMedium.SelectedItem = mediumPrimary.HasValue ? (object) mediumPrimary.Value.ToString() : (object) string.Empty;
        this.cboxMBusMediumSec.SelectedItem = mediumSecondary.HasValue ? (object) mediumSecondary.Value.ToString() : (object) string.Empty;
        this.cboxRadioListType.SelectedItem = (object) (mbusListType ?? string.Empty);
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
        if (hardwareErrors.HasValue)
        {
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
        bool? radioState = handlerMeter.GetRadioState();
        RadioMode? radioMode = handlerMeter.GetRadioMode();
        RadioPower? radioPower = handlerMeter.GetRadioPower();
        ushort? transmitInterval = handlerMeter.GetRadioTransmitInterval();
        ushort? radioInstallInterval = handlerMeter.GetRadioInstallInterval();
        ushort? radioErrorInterval = handlerMeter.GetRadioErrorInterval();
        byte? radioInstallCount = handlerMeter.GetRadioInstallCount();
        short? frequencyOffset = handlerMeter.GetFrequencyOffset();
        byte[] aeSkey = handlerMeter.GetAESkey();
        bool? busLongHeaderState = handlerMeter.GetWMBusLongHeaderState();
        bool? busEncryptionState = handlerMeter.GetWMBusEncryptionState();
        bool? transmissioModeState = handlerMeter.GetWMBusSynchronousTransmissioModeState();
        bool? installationPacketsState = handlerMeter.GetWMBusInstallationPacketsState();
        uint? serialnumberRadioMinol = handlerMeter.GetSerialnumberRadioMinol();
        byte? pulseActivateRadio = handlerMeter.GetPulseActivateRadio();
        RadioProtocol? radioScenario = handlerMeter.GetRadioScenario();
        this.txtAESkey.Text = aeSkey != null ? Util.ByteArrayToHexString(aeSkey) : string.Empty;
        this.txtSerialnumberRadioMinol.Value = (Decimal) (serialnumberRadioMinol.HasValue ? serialnumberRadioMinol.Value : 0U);
        this.txtRadioTransmitInterval.Value = (Decimal) (transmitInterval.HasValue ? (int) transmitInterval.Value : 0);
        this.txtRadioInstallInterval.Value = (Decimal) (radioInstallInterval.HasValue ? (int) radioInstallInterval.Value : 0);
        this.txtRadioErrorInterval.Value = (Decimal) (radioErrorInterval.HasValue ? (int) radioErrorInterval.Value : 0);
        this.txtRadioInstallCount.Value = (Decimal) (radioInstallCount.HasValue ? (int) radioInstallCount.Value : 0);
        this.txtRadioFrequencyOffset.Value = (Decimal) (frequencyOffset.HasValue ? (int) frequencyOffset.Value : 0);
        this.txtPulseActivateRadio.Value = (Decimal) (pulseActivateRadio.HasValue ? (int) pulseActivateRadio.Value : 0);
        this.cboxRadioMode.SelectedItem = radioMode.HasValue ? (object) radioMode.Value.ToString() : (object) string.Empty;
        this.cboxRadioPower.SelectedItem = radioPower.HasValue ? (object) radioPower.Value.ToString() : (object) string.Empty;
        this.cboxRadioScenario.SelectedItem = radioScenario.HasValue ? (object) radioScenario.Value.ToString() : (object) string.Empty;
        this.ckboxRadioEnabled.Checked = radioState.HasValue && radioState.Value;
        this.ckboxUseLongPacketHeader.Checked = busLongHeaderState.HasValue && busLongHeaderState.Value;
        this.ckboxEnableEncryption.Checked = busEncryptionState.HasValue && busEncryptionState.Value;
        this.ckboxSynchronousTransmissionMode.Checked = transmissioModeState.HasValue && transmissioModeState.Value;
        this.ckboxEnableInstallationPacket.Checked = installationPacketsState.HasValue && installationPacketsState.Value;
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
        handlerMeter.SetSerialnumberPrimary(Convert.ToUInt32(this.txtSerialnumberMBus.Value));
        handlerMeter.SetMBusAddressPrimary(Convert.ToByte(this.txtMBusAddress.Value));
        handlerMeter.SetMediumPrimary((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMedium.SelectedItem.ToString(), true));
        handlerMeter.SetPulseMultiplier(Convert.ToByte(this.txtPulseMultiplier.Value));
        handlerMeter.SetSensorTimeout(Convert.ToUInt16(this.txtSensorTimeout.Value));
        handlerMeter.SetDueDate(this.txtDueDate.Value);
        handlerMeter.SetCoilSampling(this.ckboxPulseEnable.Checked);
        handlerMeter.SetFlowCheckIntervalState(this.ckboxFlowCheckEnabled.Checked);
        handlerMeter.SetMagnetDetectionState(this.ckboxIsMagnetDetected.Checked);
        handlerMeter.SetDataLoggingState(this.ckboxLogEnabled.Checked);
        if (!string.IsNullOrEmpty(this.cboxRadioListType.Text))
          handlerMeter.SetMBusListType(this.cboxRadioListType.Text);
        handlerMeter.SetWarnings(this.GetCheckedWarnings());
        handlerMeter.SetTimeZone(Convert.ToInt32(this.txtTimeZone.Value));
        handlerMeter.SetPulsePeriod(Convert.ToUInt16(this.txtPulsePeriod.Value));
        handlerMeter.SetMBusGenerationPrimary(Convert.ToByte(this.txtMBusGeneration.Value));
        handlerMeter.SetManufacturerPrimary(this.txtManufacturer.Text);
        handlerMeter.SetSerialnumberSecondary(Convert.ToUInt32(this.txtSerialnumberSec.Value));
        handlerMeter.SetMBusAddressSecondary(Convert.ToByte(this.txtMBusAddressSec.Value));
        handlerMeter.SetManufacturerSecondary(this.txtManufacturerSec.Text);
        handlerMeter.SetMBusGenerationSecondary(Convert.ToByte(this.txtMBusGenerationSec.Value));
        handlerMeter.SetMediumSecondary((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMediumSec.SelectedItem.ToString(), true));
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
        handlerMeter.SetSerialnumberRadioMinol(Convert.ToUInt32(this.txtSerialnumberRadioMinol.Value));
        handlerMeter.SetRadioState(this.ckboxRadioEnabled.Checked);
        handlerMeter.SetRadioMode((RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioMode.SelectedItem.ToString(), true));
        handlerMeter.SetRadioPower((RadioPower) Enum.Parse(typeof (RadioPower), this.cboxRadioPower.SelectedItem.ToString(), true));
        handlerMeter.SetRadioScenario((RadioProtocol) Enum.Parse(typeof (RadioProtocol), this.cboxRadioScenario.SelectedItem.ToString(), true));
        handlerMeter.SetRadioTransmitInterval(Convert.ToUInt16(this.txtRadioTransmitInterval.Value));
        handlerMeter.SetRadioInstallInterval(Convert.ToUInt16(this.txtRadioInstallInterval.Value));
        handlerMeter.SetRadioErrorInterval(Convert.ToUInt16(this.txtRadioErrorInterval.Value));
        handlerMeter.SetRadioInstallCount(Convert.ToByte(this.txtRadioInstallCount.Value));
        handlerMeter.SetFrequencyOffset(Convert.ToInt16(this.txtRadioFrequencyOffset.Value));
        handlerMeter.SetAESkey(Util.HexStringToByteArray(this.txtAESkey.Text));
        handlerMeter.SetWMBusLongHeaderState(this.ckboxUseLongPacketHeader.Checked);
        handlerMeter.SetWMBusEncryptionState(this.ckboxEnableEncryption.Checked);
        handlerMeter.SetWMBusSynchronousTransmissioModeState(this.ckboxSynchronousTransmissionMode.Checked);
        handlerMeter.SetWMBusInstallationPacketsState(this.ckboxEnableInstallationPacket.Checked);
        handlerMeter.SetPulseActivateRadio(Convert.ToByte(this.txtPulseActivateRadio.Value));
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
      this.cboxMBusMedium.DataSource = (object) Util.GetNamesOfEnum(typeof (MBusDeviceType));
      this.cboxMBusMediumSec.DataSource = (object) Util.GetNamesOfEnum(typeof (MBusDeviceType));
      this.cboxRadioMode.DataSource = (object) Util.GetNamesOfEnum(typeof (RadioMode));
      this.cboxRadioPower.DataSource = (object) Util.GetNamesOfEnum(typeof (RadioPower));
      this.cboxRadioScenario.DataSource = (object) Util.GetNamesOfEnum(typeof (RadioProtocol));
      this.listWarnings.Items.Clear();
      foreach (object obj in Util.GetNamesOfEnum(typeof (Warning)))
        this.listWarnings.Items.Add(obj, false);
      this.listHardwareErrors.Items.Clear();
      foreach (object obj in Util.GetNamesOfEnum(typeof (HardwareError)))
        this.listHardwareErrors.Items.Add(obj, false);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConfiguratorEdcRadio));
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
      this.gboxRadio = new GroupBox();
      this.label59 = new Label();
      this.txtRadioInstallCount = new NumericUpDown();
      this.label58 = new Label();
      this.txtRadioErrorInterval = new NumericUpDown();
      this.label57 = new Label();
      this.txtRadioInstallInterval = new NumericUpDown();
      this.label38 = new Label();
      this.cboxRadioScenario = new ComboBox();
      this.label34 = new Label();
      this.txtPulseActivateRadio = new NumericUpDown();
      this.ckboxEnableInstallationPacket = new CheckBox();
      this.ckboxSynchronousTransmissionMode = new CheckBox();
      this.cboxRadioListType = new ComboBox();
      this.label10 = new Label();
      this.ckboxEnableEncryption = new CheckBox();
      this.txtAESkey = new TextBox();
      this.label18 = new Label();
      this.label17 = new Label();
      this.txtRadioFrequencyOffset = new NumericUpDown();
      this.label16 = new Label();
      this.cboxRadioPower = new ComboBox();
      this.ckboxRadioEnabled = new CheckBox();
      this.label9 = new Label();
      this.txtRadioTransmitInterval = new NumericUpDown();
      this.label8 = new Label();
      this.cboxRadioMode = new ComboBox();
      this.ckboxUseLongPacketHeader = new CheckBox();
      this.groupBox3 = new GroupBox();
      this.txtBatteryEndDate = new DateTimePicker();
      this.label63 = new Label();
      this.txtSystemTime = new DateTimePicker();
      this.label62 = new Label();
      this.label37 = new Label();
      this.txtCogCount = new NumericUpDown();
      this.txtBewertungsfaktor = new TextBox();
      this.label26 = new Label();
      this.txtPulsePeriod = new NumericUpDown();
      this.label11 = new Label();
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
      this.txtManufacturer = new TextBox();
      this.txtSerialnumberRadioMinol = new NumericUpDown();
      this.txtSerialnumberFull = new TextBox();
      this.txtMBusGeneration = new NumericUpDown();
      this.txtSerialnumberMBus = new NumericUpDown();
      this.cboxMBusMedium = new ComboBox();
      this.txtMBusAddress = new NumericUpDown();
      this.gboxLongHeader = new GroupBox();
      this.txtMBusAddressSec = new NumericUpDown();
      this.label35 = new Label();
      this.txtSerialnumberSec = new NumericUpDown();
      this.txtManufacturerSec = new TextBox();
      this.cboxMBusMediumSec = new ComboBox();
      this.txtMBusGenerationSec = new NumericUpDown();
      this.label30 = new Label();
      this.label29 = new Label();
      this.label28 = new Label();
      this.label27 = new Label();
      this.label33 = new Label();
      this.label32 = new Label();
      this.label31 = new Label();
      this.label25 = new Label();
      this.label21 = new Label();
      this.label20 = new Label();
      this.label19 = new Label();
      this.toolTip = new ToolTip(this.components);
      this.txtMinThreshold = new NumericUpDown();
      this.label36 = new Label();
      this.cboxHandlerObject = new ComboBox();
      this.panel = new Panel();
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
      this.gboxRadio.SuspendLayout();
      this.txtRadioInstallCount.BeginInit();
      this.txtRadioErrorInterval.BeginInit();
      this.txtRadioInstallInterval.BeginInit();
      this.txtPulseActivateRadio.BeginInit();
      this.txtRadioFrequencyOffset.BeginInit();
      this.txtRadioTransmitInterval.BeginInit();
      this.groupBox3.SuspendLayout();
      this.txtCogCount.BeginInit();
      this.txtPulsePeriod.BeginInit();
      this.txtTimeZone.BeginInit();
      this.txtSensorTimeout.BeginInit();
      this.txtPulseMultiplier.BeginInit();
      this.txtPulseReading.BeginInit();
      this.groupBox6.SuspendLayout();
      this.groupBox7.SuspendLayout();
      this.txtSerialnumberRadioMinol.BeginInit();
      this.txtMBusGeneration.BeginInit();
      this.txtSerialnumberMBus.BeginInit();
      this.txtMBusAddress.BeginInit();
      this.gboxLongHeader.SuspendLayout();
      this.txtMBusAddressSec.BeginInit();
      this.txtSerialnumberSec.BeginInit();
      this.txtMBusGenerationSec.BeginInit();
      this.txtMinThreshold.BeginInit();
      this.panel.SuspendLayout();
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
      this.btnCancel.Location = new Point(824, 591);
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
      this.btnSave.Location = new Point(724, 591);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(77, 29);
      this.btnSave.TabIndex = 18;
      this.btnSave.Text = "Save";
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.label1.Location = new Point(11, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(120, 15);
      this.label1.TabIndex = 21;
      this.label1.Tag = (object) "Con_MeterId";
      this.label1.Text = "Meter ID:";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.label2.Location = new Point(11, 37);
      this.label2.Name = "label2";
      this.label2.Size = new Size(120, 15);
      this.label2.TabIndex = 23;
      this.label2.Tag = (object) "Con_HardwareTypeId";
      this.label2.Text = "Hardware Type ID:";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.label3.Location = new Point(11, 57);
      this.label3.Name = "label3";
      this.label3.Size = new Size(120, 15);
      this.label3.TabIndex = 25;
      this.label3.Tag = (object) "Con_MeterInfoId";
      this.label3.Text = "Meter Info ID:";
      this.label3.TextAlign = ContentAlignment.MiddleRight;
      this.label4.Location = new Point(11, 77);
      this.label4.Name = "label4";
      this.label4.Size = new Size(120, 15);
      this.label4.TabIndex = 27;
      this.label4.Tag = (object) "Con_BaseTypeId";
      this.label4.Text = "Base Type ID:";
      this.label4.TextAlign = ContentAlignment.MiddleRight;
      this.label5.Location = new Point(11, 98);
      this.label5.Name = "label5";
      this.label5.Size = new Size(120, 15);
      this.label5.TabIndex = 29;
      this.label5.Tag = (object) "Con_MeterTypeId";
      this.label5.Text = "Meter Type ID:";
      this.label5.TextAlign = ContentAlignment.MiddleRight;
      this.label6.Location = new Point(11, 116);
      this.label6.Name = "label6";
      this.label6.Size = new Size(120, 15);
      this.label6.TabIndex = 31;
      this.label6.Tag = (object) "Con_SAP_MaterialNumber";
      this.label6.Text = "SAP Material Nr:";
      this.label6.TextAlign = ContentAlignment.MiddleRight;
      this.label7.Location = new Point(11, 135);
      this.label7.Name = "label7";
      this.label7.Size = new Size(120, 15);
      this.label7.TabIndex = 33;
      this.label7.Tag = (object) "Con_SAP_ProductionOrderNumber";
      this.label7.Text = "SAP Order Nr:";
      this.label7.TextAlign = ContentAlignment.MiddleRight;
      this.txtMeterID.Location = new Point(134, 15);
      this.txtMeterID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMeterID.Name = "txtMeterID";
      this.txtMeterID.Size = new Size(87, 20);
      this.txtMeterID.TabIndex = 34;
      this.txtHardwareTypeID.Location = new Point(134, 35);
      this.txtHardwareTypeID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtHardwareTypeID.Name = "txtHardwareTypeID";
      this.txtHardwareTypeID.Size = new Size(87, 20);
      this.txtHardwareTypeID.TabIndex = 35;
      this.txtMeterInfoID.Location = new Point(134, 55);
      this.txtMeterInfoID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMeterInfoID.Name = "txtMeterInfoID";
      this.txtMeterInfoID.Size = new Size(87, 20);
      this.txtMeterInfoID.TabIndex = 36;
      this.txtBaseTypeID.Location = new Point(134, 75);
      this.txtBaseTypeID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtBaseTypeID.Name = "txtBaseTypeID";
      this.txtBaseTypeID.Size = new Size(87, 20);
      this.txtBaseTypeID.TabIndex = 37;
      this.txtMeterTypeID.Location = new Point(134, 95);
      this.txtMeterTypeID.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMeterTypeID.Name = "txtMeterTypeID";
      this.txtMeterTypeID.Size = new Size(87, 20);
      this.txtMeterTypeID.TabIndex = 38;
      this.txtSapMaterialNumber.Location = new Point(134, 115);
      this.txtSapMaterialNumber.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtSapMaterialNumber.Name = "txtSapMaterialNumber";
      this.txtSapMaterialNumber.Size = new Size(87, 20);
      this.txtSapMaterialNumber.TabIndex = 39;
      this.txtSapProductionOrderNumber.Location = new Point(134, 135);
      this.txtSapProductionOrderNumber.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtSapProductionOrderNumber.Name = "txtSapProductionOrderNumber";
      this.txtSapProductionOrderNumber.Size = new Size(87, 20);
      this.txtSapProductionOrderNumber.TabIndex = 40;
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.txtSapProductionOrderNumber);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.txtSapMaterialNumber);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.txtMeterTypeID);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.txtBaseTypeID);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.txtMeterInfoID);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.txtHardwareTypeID);
      this.groupBox1.Controls.Add((Control) this.label7);
      this.groupBox1.Controls.Add((Control) this.txtMeterID);
      this.groupBox1.Location = new Point(273, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(228, 162);
      this.groupBox1.TabIndex = 41;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Identification";
      this.gboxRadio.Controls.Add((Control) this.label59);
      this.gboxRadio.Controls.Add((Control) this.txtRadioInstallCount);
      this.gboxRadio.Controls.Add((Control) this.label58);
      this.gboxRadio.Controls.Add((Control) this.txtRadioErrorInterval);
      this.gboxRadio.Controls.Add((Control) this.label57);
      this.gboxRadio.Controls.Add((Control) this.txtRadioInstallInterval);
      this.gboxRadio.Controls.Add((Control) this.label38);
      this.gboxRadio.Controls.Add((Control) this.cboxRadioScenario);
      this.gboxRadio.Controls.Add((Control) this.label34);
      this.gboxRadio.Controls.Add((Control) this.txtPulseActivateRadio);
      this.gboxRadio.Controls.Add((Control) this.ckboxEnableInstallationPacket);
      this.gboxRadio.Controls.Add((Control) this.ckboxSynchronousTransmissionMode);
      this.gboxRadio.Controls.Add((Control) this.cboxRadioListType);
      this.gboxRadio.Controls.Add((Control) this.label10);
      this.gboxRadio.Controls.Add((Control) this.ckboxEnableEncryption);
      this.gboxRadio.Controls.Add((Control) this.txtAESkey);
      this.gboxRadio.Controls.Add((Control) this.label18);
      this.gboxRadio.Controls.Add((Control) this.label17);
      this.gboxRadio.Controls.Add((Control) this.txtRadioFrequencyOffset);
      this.gboxRadio.Controls.Add((Control) this.label16);
      this.gboxRadio.Controls.Add((Control) this.cboxRadioPower);
      this.gboxRadio.Controls.Add((Control) this.ckboxRadioEnabled);
      this.gboxRadio.Controls.Add((Control) this.label9);
      this.gboxRadio.Controls.Add((Control) this.txtRadioTransmitInterval);
      this.gboxRadio.Controls.Add((Control) this.label8);
      this.gboxRadio.Controls.Add((Control) this.cboxRadioMode);
      this.gboxRadio.Location = new Point(273, 170);
      this.gboxRadio.Name = "gboxRadio";
      this.gboxRadio.Size = new Size(228, 330);
      this.gboxRadio.TabIndex = 42;
      this.gboxRadio.TabStop = false;
      this.gboxRadio.Text = "Radio settings";
      this.label59.Location = new Point(4, 173);
      this.label59.Name = "label59";
      this.label59.Size = new Size(158, 15);
      this.label59.TabIndex = 59;
      this.label59.Tag = (object) "cfg_radio_install_count";
      this.label59.Text = "Install count:";
      this.label59.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioInstallCount.Location = new Point(168, 172);
      this.txtRadioInstallCount.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtRadioInstallCount.Name = "txtRadioInstallCount";
      this.txtRadioInstallCount.Size = new Size(53, 20);
      this.txtRadioInstallCount.TabIndex = 60;
      this.label58.Location = new Point(4, 153);
      this.label58.Name = "label58";
      this.label58.Size = new Size(158, 15);
      this.label58.TabIndex = 57;
      this.label58.Tag = (object) "cfg_radio_error_basetime";
      this.label58.Text = "Error interval:";
      this.label58.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioErrorInterval.Location = new Point(168, 152);
      this.txtRadioErrorInterval.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtRadioErrorInterval.Minimum = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.txtRadioErrorInterval.Name = "txtRadioErrorInterval";
      this.txtRadioErrorInterval.Size = new Size(53, 20);
      this.txtRadioErrorInterval.TabIndex = 58;
      this.txtRadioErrorInterval.Value = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.label57.Location = new Point(4, 133);
      this.label57.Name = "label57";
      this.label57.Size = new Size(158, 15);
      this.label57.TabIndex = 55;
      this.label57.Tag = (object) "cfg_radio_install_basetime";
      this.label57.Text = "Install interval:";
      this.label57.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioInstallInterval.Location = new Point(168, 132);
      this.txtRadioInstallInterval.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtRadioInstallInterval.Minimum = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.txtRadioInstallInterval.Name = "txtRadioInstallInterval";
      this.txtRadioInstallInterval.Size = new Size(53, 20);
      this.txtRadioInstallInterval.TabIndex = 56;
      this.txtRadioInstallInterval.Value = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.label38.Location = new Point(4, 72);
      this.label38.Name = "label38";
      this.label38.Size = new Size(124, 15);
      this.label38.TabIndex = 54;
      this.label38.Tag = (object) "cfg_radio_scenario";
      this.label38.Text = "Scenario:";
      this.label38.TextAlign = ContentAlignment.MiddleRight;
      this.cboxRadioScenario.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioScenario.FormattingEnabled = true;
      this.cboxRadioScenario.Location = new Point(134, 70);
      this.cboxRadioScenario.Name = "cboxRadioScenario";
      this.cboxRadioScenario.Size = new Size(87, 21);
      this.cboxRadioScenario.TabIndex = 53;
      this.label34.Location = new Point(4, 213);
      this.label34.Name = "label34";
      this.label34.Size = new Size(158, 15);
      this.label34.TabIndex = 51;
      this.label34.Tag = (object) "cfg_pulse_activate";
      this.label34.Text = "Pulse activate:";
      this.label34.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseActivateRadio.Location = new Point(168, 212);
      this.txtPulseActivateRadio.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseActivateRadio.Name = "txtPulseActivateRadio";
      this.txtPulseActivateRadio.Size = new Size(53, 20);
      this.txtPulseActivateRadio.TabIndex = 52;
      this.ckboxEnableInstallationPacket.Location = new Point(6, 311);
      this.ckboxEnableInstallationPacket.Name = "ckboxEnableInstallationPacket";
      this.ckboxEnableInstallationPacket.Size = new Size(214, 15);
      this.ckboxEnableInstallationPacket.TabIndex = 50;
      this.ckboxEnableInstallationPacket.Tag = (object) "CONFIG_RADIO_INSTALL";
      this.ckboxEnableInstallationPacket.Text = "Install mode enabled (wMBus)";
      this.ckboxEnableInstallationPacket.UseVisualStyleBackColor = true;
      this.ckboxSynchronousTransmissionMode.Location = new Point(6, 293);
      this.ckboxSynchronousTransmissionMode.Name = "ckboxSynchronousTransmissionMode";
      this.ckboxSynchronousTransmissionMode.Size = new Size(215, 15);
      this.ckboxSynchronousTransmissionMode.TabIndex = 49;
      this.ckboxSynchronousTransmissionMode.Tag = (object) "CONFIG_RADIO_SYNCHRONOUS";
      this.ckboxSynchronousTransmissionMode.Text = "Synchronous mode enabled (wMBus)";
      this.ckboxSynchronousTransmissionMode.UseVisualStyleBackColor = true;
      this.cboxRadioListType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioListType.FormattingEnabled = true;
      this.cboxRadioListType.Location = new Point(135, 91);
      this.cboxRadioListType.Name = "cboxRadioListType";
      this.cboxRadioListType.Size = new Size(87, 21);
      this.cboxRadioListType.TabIndex = 38;
      this.label10.Location = new Point(4, 93);
      this.label10.Name = "label10";
      this.label10.Size = new Size(124, 15);
      this.label10.TabIndex = 39;
      this.label10.Text = "List type:";
      this.label10.TextAlign = ContentAlignment.MiddleRight;
      this.ckboxEnableEncryption.Location = new Point(6, 275);
      this.ckboxEnableEncryption.Name = "ckboxEnableEncryption";
      this.ckboxEnableEncryption.Size = new Size(215, 15);
      this.ckboxEnableEncryption.TabIndex = 48;
      this.ckboxEnableEncryption.Tag = (object) "CONFIG_RADIO_ENCRYPT";
      this.ckboxEnableEncryption.Text = "Encryptioin enabled (wMBus)";
      this.ckboxEnableEncryption.UseVisualStyleBackColor = true;
      this.txtAESkey.Location = new Point(7, 250);
      this.txtAESkey.MaxLength = 32;
      this.txtAESkey.Name = "txtAESkey";
      this.txtAESkey.Size = new Size(215, 20);
      this.txtAESkey.TabIndex = 46;
      this.label18.Location = new Point(10, 231);
      this.label18.Name = "label18";
      this.label18.Size = new Size(149, 15);
      this.label18.TabIndex = 45;
      this.label18.Tag = (object) "cfg_key";
      this.label18.Text = "AES key (32 chars as HEX)";
      this.label17.Location = new Point(4, 193);
      this.label17.Name = "label17";
      this.label17.Size = new Size(158, 15);
      this.label17.TabIndex = 43;
      this.label17.Tag = (object) "cfg_radio_freq_offset";
      this.label17.Text = "Frequency offset:";
      this.label17.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioFrequencyOffset.Location = new Point(168, 192);
      this.txtRadioFrequencyOffset.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtRadioFrequencyOffset.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtRadioFrequencyOffset.Name = "txtRadioFrequencyOffset";
      this.txtRadioFrequencyOffset.Size = new Size(53, 20);
      this.txtRadioFrequencyOffset.TabIndex = 44;
      this.label16.Location = new Point(4, 51);
      this.label16.Name = "label16";
      this.label16.Size = new Size(124, 15);
      this.label16.TabIndex = 42;
      this.label16.Tag = (object) "cfg_radio_power";
      this.label16.Text = "Power:";
      this.label16.TextAlign = ContentAlignment.MiddleRight;
      this.cboxRadioPower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioPower.FormattingEnabled = true;
      this.cboxRadioPower.Location = new Point(134, 49);
      this.cboxRadioPower.Name = "cboxRadioPower";
      this.cboxRadioPower.Size = new Size(87, 21);
      this.cboxRadioPower.TabIndex = 41;
      this.ckboxRadioEnabled.AutoSize = true;
      this.ckboxRadioEnabled.Location = new Point(97, 10);
      this.ckboxRadioEnabled.Name = "ckboxRadioEnabled";
      this.ckboxRadioEnabled.Size = new Size(95, 17);
      this.ckboxRadioEnabled.TabIndex = 40;
      this.ckboxRadioEnabled.Tag = (object) "CONFIG_ENABLE_RADIO";
      this.ckboxRadioEnabled.Text = "Radio enabled";
      this.ckboxRadioEnabled.UseVisualStyleBackColor = true;
      this.label9.Location = new Point(4, 113);
      this.label9.Name = "label9";
      this.label9.Size = new Size(158, 15);
      this.label9.TabIndex = 36;
      this.label9.Tag = (object) "cfg_radio_normal_basetime";
      this.label9.Text = "Transmit interval:";
      this.label9.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioTransmitInterval.Location = new Point(168, 112);
      this.txtRadioTransmitInterval.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtRadioTransmitInterval.Minimum = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.txtRadioTransmitInterval.Name = "txtRadioTransmitInterval";
      this.txtRadioTransmitInterval.Size = new Size(53, 20);
      this.txtRadioTransmitInterval.TabIndex = 37;
      this.txtRadioTransmitInterval.Value = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.label8.Location = new Point(4, 30);
      this.label8.Name = "label8";
      this.label8.Size = new Size(124, 15);
      this.label8.TabIndex = 22;
      this.label8.Tag = (object) "cfg_radio_mode";
      this.label8.Text = "Mode:";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.cboxRadioMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioMode.FormattingEnabled = true;
      this.cboxRadioMode.Location = new Point(134, 28);
      this.cboxRadioMode.Name = "cboxRadioMode";
      this.cboxRadioMode.Size = new Size(87, 21);
      this.cboxRadioMode.TabIndex = 0;
      this.ckboxUseLongPacketHeader.Location = new Point(13, 158);
      this.ckboxUseLongPacketHeader.Name = "ckboxUseLongPacketHeader";
      this.ckboxUseLongPacketHeader.Size = new Size(219, 17);
      this.ckboxUseLongPacketHeader.TabIndex = 47;
      this.ckboxUseLongPacketHeader.Tag = (object) "CONFIG_RADIO_LONGHEADER";
      this.ckboxUseLongPacketHeader.Text = "Use long packet header (wMBus only)";
      this.ckboxUseLongPacketHeader.UseVisualStyleBackColor = true;
      this.groupBox3.Controls.Add((Control) this.txtBatteryEndDate);
      this.groupBox3.Controls.Add((Control) this.label63);
      this.groupBox3.Controls.Add((Control) this.txtSystemTime);
      this.groupBox3.Controls.Add((Control) this.label62);
      this.groupBox3.Controls.Add((Control) this.label37);
      this.groupBox3.Controls.Add((Control) this.txtCogCount);
      this.groupBox3.Controls.Add((Control) this.txtBewertungsfaktor);
      this.groupBox3.Controls.Add((Control) this.label26);
      this.groupBox3.Controls.Add((Control) this.txtPulsePeriod);
      this.groupBox3.Controls.Add((Control) this.label11);
      this.groupBox3.Controls.Add((Control) this.label12);
      this.groupBox3.Controls.Add((Control) this.txtTimeZone);
      this.groupBox3.Controls.Add((Control) this.txtDueDate);
      this.groupBox3.Controls.Add((Control) this.label22);
      this.groupBox3.Controls.Add((Control) this.txtSensorTimeout);
      this.groupBox3.Controls.Add((Control) this.label15);
      this.groupBox3.Controls.Add((Control) this.txtPulseMultiplier);
      this.groupBox3.Controls.Add((Control) this.label14);
      this.groupBox3.Location = new Point(6, 298);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(262, 200);
      this.groupBox3.TabIndex = 43;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Common settings";
      this.txtBatteryEndDate.CustomFormat = "dd.MM.yyyy";
      this.txtBatteryEndDate.Format = DateTimePickerFormat.Custom;
      this.txtBatteryEndDate.Location = new Point(142, 176);
      this.txtBatteryEndDate.Name = "txtBatteryEndDate";
      this.txtBatteryEndDate.ShowUpDown = true;
      this.txtBatteryEndDate.Size = new Size(112, 20);
      this.txtBatteryEndDate.TabIndex = 66;
      this.label63.Location = new Point(5, 178);
      this.label63.Name = "label63";
      this.label63.Size = new Size(131, 15);
      this.label63.TabIndex = 65;
      this.label63.Tag = (object) "cfg_lowbatt";
      this.label63.Text = "Battery end date:";
      this.label63.TextAlign = ContentAlignment.MiddleRight;
      this.txtSystemTime.CustomFormat = "dd.MM.yyyy HH:mm";
      this.txtSystemTime.Enabled = false;
      this.txtSystemTime.Format = DateTimePickerFormat.Custom;
      this.txtSystemTime.Location = new Point(142, 16);
      this.txtSystemTime.Name = "txtSystemTime";
      this.txtSystemTime.ShowUpDown = true;
      this.txtSystemTime.Size = new Size(111, 20);
      this.txtSystemTime.TabIndex = 64;
      this.label62.Location = new Point(5, 16);
      this.label62.Name = "label62";
      this.label62.Size = new Size(131, 15);
      this.label62.TabIndex = 63;
      this.label62.Tag = (object) "RTC_A";
      this.label62.Text = "System time:";
      this.label62.TextAlign = ContentAlignment.MiddleRight;
      this.label37.Location = new Point(5, 156);
      this.label37.Name = "label37";
      this.label37.Size = new Size(131, 15);
      this.label37.TabIndex = 61;
      this.label37.Tag = (object) "cfg_cog_count";
      this.label37.Text = "Cog count:";
      this.label37.TextAlign = ContentAlignment.MiddleRight;
      this.txtCogCount.Location = new Point(142, 156);
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
      this.txtBewertungsfaktor.Location = new Point(142, 56);
      this.txtBewertungsfaktor.Name = "txtBewertungsfaktor";
      this.txtBewertungsfaktor.ReadOnly = true;
      this.txtBewertungsfaktor.Size = new Size(111, 20);
      this.txtBewertungsfaktor.TabIndex = 58;
      this.label26.Location = new Point(5, 136);
      this.label26.Name = "label26";
      this.label26.Size = new Size(131, 15);
      this.label26.TabIndex = 56;
      this.label26.Tag = (object) "cfg_pulse_period";
      this.label26.Text = "Pulse period:";
      this.label26.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulsePeriod.Location = new Point(142, 136);
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
      this.label11.Location = new Point(5, 56);
      this.label11.Name = "label11";
      this.label11.Size = new Size(131, 15);
      this.label11.TabIndex = 48;
      this.label11.Tag = (object) "cfg_bewertungsfaktor";
      this.label11.Text = "Bewertungsfaktor:";
      this.label11.TextAlign = ContentAlignment.MiddleRight;
      this.label12.Location = new Point(5, 116);
      this.label12.Name = "label12";
      this.label12.Size = new Size(131, 15);
      this.label12.TabIndex = 39;
      this.label12.Tag = (object) "Bak_TimeZoneInQuarterHours";
      this.label12.Text = "Timezone (qarter hours):";
      this.label12.TextAlign = ContentAlignment.MiddleRight;
      this.txtTimeZone.Location = new Point(142, 116);
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
      this.txtDueDate.Location = new Point(142, 96);
      this.txtDueDate.Name = "txtDueDate";
      this.txtDueDate.ShowUpDown = true;
      this.txtDueDate.Size = new Size(112, 20);
      this.txtDueDate.TabIndex = 44;
      this.label22.Location = new Point(5, 99);
      this.label22.Name = "label22";
      this.label22.Size = new Size(131, 15);
      this.label22.TabIndex = 43;
      this.label22.Tag = (object) "cfg_stichtag";
      this.label22.Text = "Due date (day, month):";
      this.label22.TextAlign = ContentAlignment.MiddleRight;
      this.txtSensorTimeout.Location = new Point(142, 76);
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
      this.label15.Location = new Point(5, 76);
      this.label15.Name = "label15";
      this.label15.Size = new Size(131, 15);
      this.label15.TabIndex = 41;
      this.label15.Tag = (object) "cfg_sensor_timeout";
      this.label15.Text = "Sensor timeout (sec):";
      this.label15.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseMultiplier.Location = new Point(142, 36);
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
      this.label14.Location = new Point(5, 36);
      this.label14.Name = "label14";
      this.label14.Size = new Size(131, 15);
      this.label14.TabIndex = 39;
      this.label14.Tag = (object) "cfg_pulse_multiplier";
      this.label14.Text = "Pulse multiplier:";
      this.label14.TextAlign = ContentAlignment.MiddleRight;
      this.ckboxPulseEnable.AutoSize = true;
      this.ckboxPulseEnable.Location = new Point(279, 503);
      this.ckboxPulseEnable.Name = "ckboxPulseEnable";
      this.ckboxPulseEnable.Size = new Size(93, 17);
      this.ckboxPulseEnable.TabIndex = 0;
      this.ckboxPulseEnable.Tag = (object) "CONFIG_ENABLE_PULSE";
      this.ckboxPulseEnable.Text = "Pulse enabled";
      this.ckboxPulseEnable.UseVisualStyleBackColor = true;
      this.ckboxFlowCheckEnabled.AutoSize = true;
      this.ckboxFlowCheckEnabled.Location = new Point(279, 521);
      this.ckboxFlowCheckEnabled.Name = "ckboxFlowCheckEnabled";
      this.ckboxFlowCheckEnabled.Size = new Size(122, 17);
      this.ckboxFlowCheckEnabled.TabIndex = 45;
      this.ckboxFlowCheckEnabled.Tag = (object) "CONFIG_FLOW_CHECK";
      this.ckboxFlowCheckEnabled.Text = "Flow check enabled";
      this.ckboxFlowCheckEnabled.UseVisualStyleBackColor = true;
      this.ckboxLogEnabled.AutoSize = true;
      this.ckboxLogEnabled.Location = new Point(279, 539);
      this.ckboxLogEnabled.Name = "ckboxLogEnabled";
      this.ckboxLogEnabled.Size = new Size(85, 17);
      this.ckboxLogEnabled.TabIndex = 47;
      this.ckboxLogEnabled.Tag = (object) "CONFIG_LOG_ENABLED";
      this.ckboxLogEnabled.Text = "Log enabled";
      this.ckboxLogEnabled.UseVisualStyleBackColor = true;
      this.ckboxIsMagnetDetected.AutoSize = true;
      this.ckboxIsMagnetDetected.Location = new Point(279, 557);
      this.ckboxIsMagnetDetected.Name = "ckboxIsMagnetDetected";
      this.ckboxIsMagnetDetected.Size = new Size(114, 17);
      this.ckboxIsMagnetDetected.TabIndex = 46;
      this.ckboxIsMagnetDetected.Tag = (object) "CONFIG_MAGNET_FOUND";
      this.ckboxIsMagnetDetected.Text = "Magnet was found";
      this.ckboxIsMagnetDetected.UseVisualStyleBackColor = true;
      this.label13.Location = new Point(546, 454);
      this.label13.Name = "label13";
      this.label13.Size = new Size(133, 15);
      this.label13.TabIndex = 43;
      this.label13.Tag = (object) "pulseReading";
      this.label13.Text = "Meter value:";
      this.label13.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseReading.Location = new Point(680, 452);
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
      this.groupBox6.Location = new Point(716, 6);
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
      this.groupBox7.Controls.Add((Control) this.txtManufacturer);
      this.groupBox7.Controls.Add((Control) this.txtSerialnumberRadioMinol);
      this.groupBox7.Controls.Add((Control) this.txtSerialnumberFull);
      this.groupBox7.Controls.Add((Control) this.txtMBusGeneration);
      this.groupBox7.Controls.Add((Control) this.txtSerialnumberMBus);
      this.groupBox7.Controls.Add((Control) this.cboxMBusMedium);
      this.groupBox7.Controls.Add((Control) this.txtMBusAddress);
      this.groupBox7.Controls.Add((Control) this.ckboxUseLongPacketHeader);
      this.groupBox7.Controls.Add((Control) this.gboxLongHeader);
      this.groupBox7.Controls.Add((Control) this.label33);
      this.groupBox7.Controls.Add((Control) this.label32);
      this.groupBox7.Controls.Add((Control) this.label31);
      this.groupBox7.Controls.Add((Control) this.label25);
      this.groupBox7.Controls.Add((Control) this.label21);
      this.groupBox7.Controls.Add((Control) this.label20);
      this.groupBox7.Controls.Add((Control) this.label19);
      this.groupBox7.Location = new Point(6, 5);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new Size(262, 287);
      this.groupBox7.TabIndex = 47;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "Device settings";
      this.txtManufacturer.Location = new Point(151, 135);
      this.txtManufacturer.MaxLength = 3;
      this.txtManufacturer.Name = "txtManufacturer";
      this.txtManufacturer.Size = new Size(102, 20);
      this.txtManufacturer.TabIndex = 70;
      this.txtSerialnumberRadioMinol.Location = new Point(151, 34);
      this.txtSerialnumberRadioMinol.Maximum = new Decimal(new int[4]
      {
        99999999,
        0,
        0,
        0
      });
      this.txtSerialnumberRadioMinol.Name = "txtSerialnumberRadioMinol";
      this.txtSerialnumberRadioMinol.Size = new Size(102, 20);
      this.txtSerialnumberRadioMinol.TabIndex = 67;
      this.txtSerialnumberFull.Location = new Point(151, 14);
      this.txtSerialnumberFull.Name = "txtSerialnumberFull";
      this.txtSerialnumberFull.Size = new Size(102, 20);
      this.txtSerialnumberFull.TabIndex = 65;
      this.txtMBusGeneration.Location = new Point(151, 115);
      this.txtMBusGeneration.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtMBusGeneration.Name = "txtMBusGeneration";
      this.txtMBusGeneration.Size = new Size(102, 20);
      this.txtMBusGeneration.TabIndex = 55;
      this.txtSerialnumberMBus.Location = new Point(151, 54);
      this.txtSerialnumberMBus.Maximum = new Decimal(new int[4]
      {
        99999999,
        0,
        0,
        0
      });
      this.txtSerialnumberMBus.Name = "txtSerialnumberMBus";
      this.txtSerialnumberMBus.Size = new Size(102, 20);
      this.txtSerialnumberMBus.TabIndex = 53;
      this.cboxMBusMedium.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMBusMedium.FormattingEnabled = true;
      this.cboxMBusMedium.Location = new Point(151, 94);
      this.cboxMBusMedium.Name = "cboxMBusMedium";
      this.cboxMBusMedium.Size = new Size(102, 21);
      this.cboxMBusMedium.TabIndex = 52;
      this.txtMBusAddress.Location = new Point(151, 74);
      this.txtMBusAddress.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtMBusAddress.Name = "txtMBusAddress";
      this.txtMBusAddress.Size = new Size(102, 20);
      this.txtMBusAddress.TabIndex = 50;
      this.gboxLongHeader.Controls.Add((Control) this.txtMBusAddressSec);
      this.gboxLongHeader.Controls.Add((Control) this.label35);
      this.gboxLongHeader.Controls.Add((Control) this.txtSerialnumberSec);
      this.gboxLongHeader.Controls.Add((Control) this.txtManufacturerSec);
      this.gboxLongHeader.Controls.Add((Control) this.cboxMBusMediumSec);
      this.gboxLongHeader.Controls.Add((Control) this.txtMBusGenerationSec);
      this.gboxLongHeader.Controls.Add((Control) this.label30);
      this.gboxLongHeader.Controls.Add((Control) this.label29);
      this.gboxLongHeader.Controls.Add((Control) this.label28);
      this.gboxLongHeader.Controls.Add((Control) this.label27);
      this.gboxLongHeader.Location = new Point(6, 157);
      this.gboxLongHeader.Name = "gboxLongHeader";
      this.gboxLongHeader.Size = new Size(250, 124);
      this.gboxLongHeader.TabIndex = 71;
      this.gboxLongHeader.TabStop = false;
      this.txtMBusAddressSec.Location = new Point(165, 41);
      this.txtMBusAddressSec.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtMBusAddressSec.Name = "txtMBusAddressSec";
      this.txtMBusAddressSec.Size = new Size(79, 20);
      this.txtMBusAddressSec.TabIndex = 70;
      this.label35.Location = new Point(7, 42);
      this.label35.Name = "label35";
      this.label35.Size = new Size(152, 15);
      this.label35.TabIndex = 69;
      this.label35.Tag = (object) "cfg_mbus_address_secondary";
      this.label35.Text = "Secondary M-Bus address:";
      this.label35.TextAlign = ContentAlignment.MiddleRight;
      this.txtSerialnumberSec.Location = new Point(165, 21);
      this.txtSerialnumberSec.Maximum = new Decimal(new int[4]
      {
        99999999,
        0,
        0,
        0
      });
      this.txtSerialnumberSec.Name = "txtSerialnumberSec";
      this.txtSerialnumberSec.Size = new Size(79, 20);
      this.txtSerialnumberSec.TabIndex = 61;
      this.txtManufacturerSec.Location = new Point(165, 59);
      this.txtManufacturerSec.MaxLength = 3;
      this.txtManufacturerSec.Name = "txtManufacturerSec";
      this.txtManufacturerSec.Size = new Size(79, 20);
      this.txtManufacturerSec.TabIndex = 68;
      this.cboxMBusMediumSec.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMBusMediumSec.FormattingEnabled = true;
      this.cboxMBusMediumSec.Location = new Point(165, 79);
      this.cboxMBusMediumSec.Name = "cboxMBusMediumSec";
      this.cboxMBusMediumSec.Size = new Size(79, 21);
      this.cboxMBusMediumSec.TabIndex = 60;
      this.txtMBusGenerationSec.Location = new Point(165, 100);
      this.txtMBusGenerationSec.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMBusGenerationSec.Name = "txtMBusGenerationSec";
      this.txtMBusGenerationSec.Size = new Size(79, 20);
      this.txtMBusGenerationSec.TabIndex = 63;
      this.label30.Location = new Point(2, 23);
      this.label30.Name = "label30";
      this.label30.Size = new Size(159, 15);
      this.label30.TabIndex = 56;
      this.label30.Tag = (object) "cfg_serial_secondary";
      this.label30.Text = "Secondary serial number:";
      this.label30.TextAlign = ContentAlignment.MiddleRight;
      this.label29.Location = new Point(2, 62);
      this.label29.Name = "label29";
      this.label29.Size = new Size(159, 15);
      this.label29.TabIndex = 57;
      this.label29.Tag = (object) "cfg_mbus_manid_secondary";
      this.label29.Text = "Secondary manufacturer:";
      this.label29.TextAlign = ContentAlignment.MiddleRight;
      this.label28.Location = new Point(2, 82);
      this.label28.Name = "label28";
      this.label28.Size = new Size(159, 15);
      this.label28.TabIndex = 59;
      this.label28.Tag = (object) "cfg_mbus_medium_secondary";
      this.label28.Text = "Secondary M-Bus medium:";
      this.label28.TextAlign = ContentAlignment.MiddleRight;
      this.label27.Location = new Point(2, 102);
      this.label27.Name = "label27";
      this.label27.Size = new Size(159, 15);
      this.label27.TabIndex = 62;
      this.label27.Tag = (object) "cfg_mbus_version_secondary";
      this.label27.Text = "Secondary M-Bus generation:";
      this.label27.TextAlign = ContentAlignment.MiddleRight;
      this.label33.Location = new Point(4, 137);
      this.label33.Name = "label33";
      this.label33.Size = new Size(141, 15);
      this.label33.TabIndex = 69;
      this.label33.Tag = (object) "cfg_mbus_manid";
      this.label33.Text = "Manufacturer:";
      this.label33.TextAlign = ContentAlignment.MiddleRight;
      this.label32.Location = new Point(4, 35);
      this.label32.Name = "label32";
      this.label32.Size = new Size(141, 15);
      this.label32.TabIndex = 66;
      this.label32.Tag = (object) "cfg_serial_radio_minol";
      this.label32.Text = "Minol serial number:";
      this.label32.TextAlign = ContentAlignment.MiddleRight;
      this.label31.Location = new Point(4, 15);
      this.label31.Name = "label31";
      this.label31.Size = new Size(141, 15);
      this.label31.TabIndex = 64;
      this.label31.Tag = (object) "Con_fullserialnumber";
      this.label31.Text = "Full serial number:";
      this.label31.TextAlign = ContentAlignment.MiddleRight;
      this.label25.Location = new Point(4, 116);
      this.label25.Name = "label25";
      this.label25.Size = new Size(141, 15);
      this.label25.TabIndex = 54;
      this.label25.Tag = (object) "cfg_mbus_version";
      this.label25.Text = "M-Bus generation:";
      this.label25.TextAlign = ContentAlignment.MiddleRight;
      this.label21.Location = new Point(4, 96);
      this.label21.Name = "label21";
      this.label21.Size = new Size(141, 15);
      this.label21.TabIndex = 51;
      this.label21.Tag = (object) "cfg_mbus_medium";
      this.label21.Text = "M-Bus medium:";
      this.label21.TextAlign = ContentAlignment.MiddleRight;
      this.label20.Location = new Point(4, 75);
      this.label20.Name = "label20";
      this.label20.Size = new Size(141, 15);
      this.label20.TabIndex = 49;
      this.label20.Tag = (object) "cfg_mbus_address";
      this.label20.Text = "M-Bus address:";
      this.label20.TextAlign = ContentAlignment.MiddleRight;
      this.label19.Location = new Point(4, 55);
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
      this.txtMinThreshold.Location = new Point(142, 56);
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
      this.txtMinThreshold.Size = new Size(111, 20);
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
      this.panel.Controls.Add((Control) this.gboxRadio);
      this.panel.Controls.Add((Control) this.groupBox3);
      this.panel.Location = new Point(0, 36);
      this.panel.Name = "panel";
      this.panel.Size = new Size(913, 627);
      this.panel.TabIndex = 52;
      this.ckboxRemovalDetectionMasking.AutoSize = true;
      this.ckboxRemovalDetectionMasking.Location = new Point(279, 576);
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
      this.groupBox2.Location = new Point(6, 500);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(262, 122);
      this.groupBox2.TabIndex = 67;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Coil settings";
      this.label23.Location = new Point(5, 56);
      this.label23.Name = "label23";
      this.label23.Size = new Size(131, 15);
      this.label23.TabIndex = 67;
      this.label23.Tag = (object) "cfg_coil_min_threshold";
      this.label23.Text = "Min threshold:";
      this.label23.TextAlign = ContentAlignment.MiddleRight;
      this.label24.Location = new Point(5, 36);
      this.label24.Name = "label24";
      this.label24.Size = new Size(131, 15);
      this.label24.TabIndex = 65;
      this.label24.Tag = (object) "cfg_coil_max_threshold";
      this.label24.Text = "Max threshold:";
      this.label24.TextAlign = ContentAlignment.MiddleRight;
      this.txtMaxThreshold.Location = new Point(142, 36);
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
      this.txtMaxThreshold.Size = new Size(111, 20);
      this.txtMaxThreshold.TabIndex = 66;
      this.label40.Location = new Point(5, 16);
      this.label40.Name = "label40";
      this.label40.Size = new Size(131, 15);
      this.label40.TabIndex = 63;
      this.label40.Tag = (object) "cfg_coil_error_threshold";
      this.label40.Text = "Error threshold:";
      this.label40.TextAlign = ContentAlignment.MiddleRight;
      this.txtErrorThreshold.Location = new Point(142, 16);
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
      this.txtErrorThreshold.Size = new Size(111, 20);
      this.txtErrorThreshold.TabIndex = 64;
      this.label39.Location = new Point(5, 96);
      this.label39.Name = "label39";
      this.label39.Size = new Size(131, 15);
      this.label39.TabIndex = 56;
      this.label39.Tag = (object) "cfg_coil_b_offset";
      this.label39.Text = "B offset:";
      this.label39.TextAlign = ContentAlignment.MiddleRight;
      this.txtB_offset.Location = new Point(142, 96);
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
      this.txtB_offset.Size = new Size(111, 20);
      this.txtB_offset.TabIndex = 57;
      this.label41.Location = new Point(5, 76);
      this.label41.Name = "label41";
      this.label41.Size = new Size(131, 15);
      this.label41.TabIndex = 39;
      this.label41.Tag = (object) "cfg_coil_amplitude_limit";
      this.label41.Text = "Amplitude limit:";
      this.label41.TextAlign = ContentAlignment.MiddleRight;
      this.txtAmplitudeLimit.Location = new Point(142, 76);
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
      this.txtAmplitudeLimit.Size = new Size(111, 20);
      this.txtAmplitudeLimit.TabIndex = 40;
      this.groupBox8.Controls.Add((Control) this.listHardwareErrors);
      this.groupBox8.Location = new Point(716, 269);
      this.groupBox8.Name = "groupBox8";
      this.groupBox8.Size = new Size(192, 171);
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
      this.gboxConstants.Location = new Point(507, 6);
      this.gboxConstants.Name = "gboxConstants";
      this.gboxConstants.Size = new Size(204, 324);
      this.gboxConstants.TabIndex = 52;
      this.gboxConstants.TabStop = false;
      this.gboxConstants.Text = "Constants";
      this.label60.Location = new Point(5, 298);
      this.label60.Margin = new Padding(3, 0, 0, 0);
      this.label60.Name = "label60";
      this.label60.Size = new Size(136, 15);
      this.label60.TabIndex = 89;
      this.label60.Tag = (object) "cfg_pulse_error_threshold";
      this.label60.Text = "Pulse error threshold:";
      this.label60.TextAlign = ContentAlignment.MiddleRight;
      this.label61.Location = new Point(5, 278);
      this.label61.Margin = new Padding(3, 0, 0, 0);
      this.label61.Name = "label61";
      this.label61.Size = new Size(136, 15);
      this.label61.TabIndex = 87;
      this.label61.Tag = (object) "cfg_uart_watchdog";
      this.label61.Text = "UART watchdog:";
      this.label61.TextAlign = ContentAlignment.MiddleRight;
      this.label51.Location = new Point(5, 258);
      this.label51.Margin = new Padding(3, 0, 0, 0);
      this.label51.Name = "label51";
      this.label51.Size = new Size(136, 15);
      this.label51.TabIndex = 85;
      this.label51.Tag = (object) "cfg_burst_limit";
      this.label51.Text = "Burst limit:";
      this.label51.TextAlign = ContentAlignment.MiddleRight;
      this.label52.Location = new Point(5, 238);
      this.label52.Margin = new Padding(3, 0, 0, 0);
      this.label52.Name = "label52";
      this.label52.Size = new Size(136, 15);
      this.label52.TabIndex = 83;
      this.label52.Tag = (object) "cfg_burst_diff";
      this.label52.Text = "Burst diff:";
      this.label52.TextAlign = ContentAlignment.MiddleRight;
      this.label53.Location = new Point(5, 218);
      this.label53.Margin = new Padding(3, 0, 0, 0);
      this.label53.Name = "label53";
      this.label53.Size = new Size(136, 15);
      this.label53.TabIndex = 81;
      this.label53.Tag = (object) "cfg_undersize_limit";
      this.label53.Text = "Undersize limit:";
      this.label53.TextAlign = ContentAlignment.MiddleRight;
      this.label54.Location = new Point(5, 198);
      this.label54.Margin = new Padding(3, 0, 0, 0);
      this.label54.Name = "label54";
      this.label54.Size = new Size(136, 15);
      this.label54.TabIndex = 79;
      this.label54.Tag = (object) "cfg_undersize_diff";
      this.label54.Text = "Undersize diff:";
      this.label54.TextAlign = ContentAlignment.MiddleRight;
      this.label55.Location = new Point(5, 178);
      this.label55.Margin = new Padding(3, 0, 0, 0);
      this.label55.Name = "label55";
      this.label55.Size = new Size(136, 15);
      this.label55.TabIndex = 77;
      this.label55.Tag = (object) "cfg_oversize_limit";
      this.label55.Text = "Oversize limit:";
      this.label55.TextAlign = ContentAlignment.MiddleRight;
      this.label56.Location = new Point(5, 158);
      this.label56.Margin = new Padding(3, 0, 0, 0);
      this.label56.Name = "label56";
      this.label56.Size = new Size(136, 15);
      this.label56.TabIndex = 75;
      this.label56.Tag = (object) "cfg_oversize_diff";
      this.label56.Text = "Oversize diff:";
      this.label56.TextAlign = ContentAlignment.MiddleRight;
      this.label49.Location = new Point(5, 138);
      this.label49.Margin = new Padding(3, 0, 0, 0);
      this.label49.Name = "label49";
      this.label49.Size = new Size(136, 15);
      this.label49.TabIndex = 73;
      this.label49.Tag = (object) "cfg_pulse_unback_limit";
      this.label49.Text = "Pulse unback limit:";
      this.label49.TextAlign = ContentAlignment.MiddleRight;
      this.label48.Location = new Point(5, 118);
      this.label48.Margin = new Padding(3, 0, 0, 0);
      this.label48.Name = "label48";
      this.label48.Size = new Size(136, 15);
      this.label48.TabIndex = 71;
      this.label48.Tag = (object) "cfg_pulse_back_limit";
      this.label48.Text = "Pulse back limit:";
      this.label48.TextAlign = ContentAlignment.MiddleRight;
      this.label47.Location = new Point(5, 98);
      this.label47.Margin = new Padding(3, 0, 0, 0);
      this.label47.Name = "label47";
      this.label47.Size = new Size(136, 15);
      this.label47.TabIndex = 69;
      this.label47.Tag = (object) "cfg_pulse_leak_upper";
      this.label47.Text = "Pulse leak upper:";
      this.label47.TextAlign = ContentAlignment.MiddleRight;
      this.label46.Location = new Point(5, 78);
      this.label46.Margin = new Padding(3, 0, 0, 0);
      this.label46.Name = "label46";
      this.label46.Size = new Size(136, 15);
      this.label46.TabIndex = 67;
      this.label46.Tag = (object) "cfg_pulse_leak_lower";
      this.label46.Text = "Pulse leak lower:";
      this.label46.TextAlign = ContentAlignment.MiddleRight;
      this.label45.Location = new Point(5, 59);
      this.label45.Margin = new Padding(3, 0, 0, 0);
      this.label45.Name = "label45";
      this.label45.Size = new Size(136, 15);
      this.label45.TabIndex = 65;
      this.label45.Tag = (object) "cfg_pulse_unleak_limit";
      this.label45.Text = "Pulse unleak limit:";
      this.label45.TextAlign = ContentAlignment.MiddleRight;
      this.label44.Location = new Point(5, 38);
      this.label44.Margin = new Padding(3, 0, 0, 0);
      this.label44.Name = "label44";
      this.label44.Size = new Size(136, 15);
      this.label44.TabIndex = 63;
      this.label44.Tag = (object) "cfg_pulse_leak_limit";
      this.label44.Text = "Pulse leak limit:";
      this.label44.TextAlign = ContentAlignment.MiddleRight;
      this.label43.Location = new Point(5, 18);
      this.label43.Margin = new Padding(3, 0, 0, 0);
      this.label43.Name = "label43";
      this.label43.Size = new Size(136, 15);
      this.label43.TabIndex = 61;
      this.label43.Tag = (object) "cfg_pulse_block_limit";
      this.label43.Text = "Pulse block limit:";
      this.label43.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseErrorThreshold.Location = new Point(142, 297);
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
      this.txtUARTwatchdog.Location = new Point(142, 277);
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
      this.txtBurstLimit.Location = new Point(142, 257);
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
      this.txtBurstDiff.Location = new Point(142, 237);
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
      this.txtUndersizeLimit.Location = new Point(142, 217);
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
      this.txtUndersizeDiff.Location = new Point(142, 197);
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
      this.txtOversizeLimit.Location = new Point(142, 177);
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
      this.txtOversizeDiff.Location = new Point(142, 157);
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
      this.txtPulseUnbackLimit.Location = new Point(142, 137);
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
      this.txtPulseBackLimit.Location = new Point(142, 117);
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
      this.txtPulseLeakUpper.Location = new Point(142, 97);
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
      this.txtPulseLeakLower.Location = new Point(142, 77);
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
      this.txtPulseUnleakLimit.Location = new Point(142, 57);
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
      this.txtPulseLeakLimit.Location = new Point(142, 37);
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
      this.txtPulseBlockLimit.Location = new Point(142, 17);
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
      this.zennerCoroprateDesign1.Size = new Size(913, 36);
      this.zennerCoroprateDesign1.TabIndex = 20;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(913, 662);
      this.Controls.Add((Control) this.ckboxShowParameterNamesUsedInFirmware);
      this.Controls.Add((Control) this.panel);
      this.Controls.Add((Control) this.label36);
      this.Controls.Add((Control) this.cboxHandlerObject);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConfiguratorEdcRadio);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Configurator for EDC radio";
      this.Load += new System.EventHandler(this.Configurator_Load);
      this.txtMeterID.EndInit();
      this.txtHardwareTypeID.EndInit();
      this.txtMeterInfoID.EndInit();
      this.txtBaseTypeID.EndInit();
      this.txtMeterTypeID.EndInit();
      this.txtSapMaterialNumber.EndInit();
      this.txtSapProductionOrderNumber.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.gboxRadio.ResumeLayout(false);
      this.gboxRadio.PerformLayout();
      this.txtRadioInstallCount.EndInit();
      this.txtRadioErrorInterval.EndInit();
      this.txtRadioInstallInterval.EndInit();
      this.txtPulseActivateRadio.EndInit();
      this.txtRadioFrequencyOffset.EndInit();
      this.txtRadioTransmitInterval.EndInit();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.txtCogCount.EndInit();
      this.txtPulsePeriod.EndInit();
      this.txtTimeZone.EndInit();
      this.txtSensorTimeout.EndInit();
      this.txtPulseMultiplier.EndInit();
      this.txtPulseReading.EndInit();
      this.groupBox6.ResumeLayout(false);
      this.groupBox7.ResumeLayout(false);
      this.groupBox7.PerformLayout();
      this.txtSerialnumberRadioMinol.EndInit();
      this.txtMBusGeneration.EndInit();
      this.txtSerialnumberMBus.EndInit();
      this.txtMBusAddress.EndInit();
      this.gboxLongHeader.ResumeLayout(false);
      this.gboxLongHeader.PerformLayout();
      this.txtMBusAddressSec.EndInit();
      this.txtSerialnumberSec.EndInit();
      this.txtMBusGenerationSec.EndInit();
      this.txtMinThreshold.EndInit();
      this.panel.ResumeLayout(false);
      this.panel.PerformLayout();
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
