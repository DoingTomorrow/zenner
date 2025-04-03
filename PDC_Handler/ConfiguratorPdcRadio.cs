// Decompiled with JetBrains decompiler
// Type: PDC_Handler.ConfiguratorPdcRadio
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public class ConfiguratorPdcRadio : Form
  {
    private PDC_Meter tempWorkMeter;
    private PDC_Meter tempTypeMeter;
    private PDC_Meter tempBackupMeter;
    private PDC_Meter tempConnectedMeter;
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
    private Label label12;
    private NumericUpDown txtTimeZone;
    private Label label13;
    private NumericUpDown txtPulseReadingA;
    private Label label16;
    private ComboBox cboxRadioPower;
    private Label label17;
    private NumericUpDown txtRadioFrequencyOffset;
    private TextBox txtAESkey;
    private Label label18;
    private DateTimePicker txtDueDate;
    private Label label22;
    private ToolTip toolTip;
    private Label label26;
    private NumericUpDown txtPulsePeriod;
    private Label label36;
    private ComboBox cboxHandlerObject;
    private Panel panel;
    private Label label34;
    private NumericUpDown txtPulseActivateRadio;
    private GroupBox groupBox8;
    private CheckedListBox listHardwareErrors;
    private Label label59;
    private NumericUpDown txtRadioInstallCount;
    private Label label57;
    private NumericUpDown txtRadioInstallInterval;
    private DateTimePicker txtBatteryEndDate;
    private Label label63;
    private CheckBox ckboxShowParameterNamesUsedInFirmware;
    private TextBox txtManufacturerC;
    private NumericUpDown txtMBusGenerationC;
    private ComboBox cboxMBusMediumC;
    private NumericUpDown txtMBusAddressC;
    private Label label10;
    private Label label14;
    private Label label15;
    private Label label19;
    private Label label23;
    private TextBox txtObisC;
    private Label label37;
    private Label label38;
    private NumericUpDown txtPulseReadingB;
    private Label label40;
    private NumericUpDown txtRadioPacketBOffset;
    private NumericUpDown txtSerialC;
    private GroupBox groupBox4;
    private TextBox txtFullSerialnumberC;
    private Label label41;
    private GroupBox groupBox6;
    private Label label11;
    private TextBox txtFullSerialnumberB;
    private NumericUpDown txtScaleExponentB;
    private Label label20;
    private Label label33;
    private NumericUpDown txtMBusAddressB;
    private NumericUpDown txtScaleMantissaB;
    private NumericUpDown txtMBusGenerationB;
    private NumericUpDown txtVifB;
    private NumericUpDown txtSerialB;
    private Label label35;
    private ComboBox cboxMBusMediumB;
    private Label label64;
    private TextBox txtObisB;
    private Label label65;
    private TextBox txtManufacturerB;
    private Label label66;
    private Label label21;
    private Label label67;
    private Label label24;
    private Label label68;
    private Label label25;
    private Label label69;
    private Label label27;
    private Label label70;
    private Label label28;
    private Label label71;
    private Label label29;
    private Label label72;
    private NumericUpDown txtUndersizeLimitB;
    private Label label73;
    private NumericUpDown txtPulseBlockLimitB;
    private Label label74;
    private NumericUpDown txtPulseLeakLimitB;
    private NumericUpDown txtBurstLimitB;
    private NumericUpDown txtPulseUnleakLimitB;
    private NumericUpDown txtBurstDiffB;
    private NumericUpDown txtPulseLeakLowerB;
    private NumericUpDown txtPulseLeakUpperB;
    private NumericUpDown txtUndersizeDiffB;
    private NumericUpDown txtOversizeDiffB;
    private NumericUpDown txtOversizeLimitB;
    private GroupBox groupBox5;
    private Label label32;
    private NumericUpDown txtScaleExponentA;
    private Label label31;
    private NumericUpDown txtScaleMantissaA;
    private NumericUpDown txtVifA;
    private Label label30;
    private Label label51;
    private Label label52;
    private Label label53;
    private Label label54;
    private Label label55;
    private Label label56;
    private Label label47;
    private Label label46;
    private Label label45;
    private Label label44;
    private Label label43;
    private NumericUpDown txtBurstLimitA;
    private NumericUpDown txtBurstDiffA;
    private NumericUpDown txtUndersizeLimitA;
    private NumericUpDown txtUndersizeDiffA;
    private NumericUpDown txtOversizeLimitA;
    private NumericUpDown txtOversizeDiffA;
    private NumericUpDown txtPulseLeakUpperA;
    private NumericUpDown txtPulseLeakLowerA;
    private NumericUpDown txtPulseUnleakLimitA;
    private NumericUpDown txtPulseLeakLimitA;
    private NumericUpDown txtPulseBlockLimitA;
    private TextBox txtFullSerialnumberA;
    private Label label42;
    private NumericUpDown txtMBusAddressA;
    private NumericUpDown txtMBusGenerationA;
    private NumericUpDown txtSerialA;
    private ComboBox cboxMBusMediumA;
    private TextBox txtObisA;
    private TextBox txtManufacturerA;
    private Label label48;
    private Label label49;
    private Label label50;
    private Label label58;
    private Label label60;
    private Label label61;
    private Label label39;
    private NumericUpDown txtPulseOn;
    private ComboBox cboxRadioListType;
    private Label label62;
    private Label label75;
    private NumericUpDown txtRadioTimeBias;
    private DateTimePicker txtSystemTime;
    private Label label76;
    private GroupBox groupBox7;
    private CheckedListBox listRadioFlags;
    private GroupBox groupBox2;
    private CheckedListBox listConfigFlags;
    private GroupBox groupBox9;
    private CheckedListBox listPersistentFlagsA;
    private GroupBox groupBox10;
    private CheckedListBox listPersistentFlagsB;

    public ConfiguratorPdcRadio() => this.InitializeComponent();

    private void Configurator_Load(object sender, EventArgs e) => this.InitializeForm();

    internal static void ShowDialog(Form owner, PDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null || MyFunctions.WorkMeter == null || MyFunctions.WorkMeter.Version.Type != PDC_DeviceIdentity.PDC_WmBus)
        return;
      using (ConfiguratorPdcRadio configuratorPdcRadio = new ConfiguratorPdcRadio())
      {
        if (MyFunctions.WorkMeter != null)
          configuratorPdcRadio.tempWorkMeter = MyFunctions.WorkMeter.DeepCopy();
        if (MyFunctions.TypeMeter != null)
          configuratorPdcRadio.tempTypeMeter = MyFunctions.TypeMeter.DeepCopy();
        if (MyFunctions.BackupMeter != null)
          configuratorPdcRadio.tempBackupMeter = MyFunctions.BackupMeter.DeepCopy();
        if (MyFunctions.ConnectedMeter != null)
          configuratorPdcRadio.tempConnectedMeter = MyFunctions.ConnectedMeter.DeepCopy();
        if (configuratorPdcRadio.ShowDialog((IWin32Window) owner) == DialogResult.OK)
        {
          MyFunctions.WorkMeter = configuratorPdcRadio.tempWorkMeter;
          MyFunctions.TypeMeter = configuratorPdcRadio.tempTypeMeter;
          MyFunctions.BackupMeter = configuratorPdcRadio.tempBackupMeter;
          MyFunctions.ConnectedMeter = configuratorPdcRadio.tempConnectedMeter;
        }
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
        if (control.Tag != null && control.Tag.ToString() != string.Empty)
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
        PDC_Meter handlerMeter = this.GetHandlerMeter();
        this.panel.Visible = handlerMeter != null;
        if (handlerMeter == null)
          return;
        DeviceIdentification deviceIdentification = handlerMeter.GetDeviceIdentification();
        string serialnumberFull = handlerMeter.GetSerialnumberFull();
        uint? serialMbusPdc = handlerMeter.GetSerialMBusPDC();
        byte? mbusAddressPdc = handlerMeter.GetMBusAddressPDC();
        MBusDeviceType? mediumPdc = handlerMeter.GetMediumPDC();
        string manufacturerPdc = handlerMeter.GetManufacturerPDC();
        byte? mbusGenerationPdc = handlerMeter.GetMBusGenerationPDC();
        string obisPdc = handlerMeter.GetObisPDC();
        string serialnumberFullInputA = handlerMeter.GetSerialnumberFullInputA();
        uint? serialMbusInputA = handlerMeter.GetSerialMBusInputA();
        byte? mbusAddressInputA = handlerMeter.GetMBusAddressInputA();
        MBusDeviceType? mediumInputA = handlerMeter.GetMediumInputA();
        string manufacturerInputA = handlerMeter.GetManufacturerInputA();
        byte? generationInputA = handlerMeter.GetMBusGenerationInputA();
        string obisInputA = handlerMeter.GetObisInputA();
        ushort? blockLimitInputA = handlerMeter.GetPulseBlockLimitInputA();
        ushort? pulseLeakLimitInputA = handlerMeter.GetPulseLeakLimitInputA();
        ushort? unleakLimitInputA = handlerMeter.GetPulseUnleakLimitInputA();
        short? pulseLeakLowerInputA = handlerMeter.GetPulseLeakLowerInputA();
        short? pulseLeakUpperInputA = handlerMeter.GetPulseLeakUpperInputA();
        ushort? oversizeDiffInputA = handlerMeter.GetOversizeDiffInputA();
        ushort? oversizeLimitInputA = handlerMeter.GetOversizeLimitInputA();
        ushort? undersizeDiffInputA = handlerMeter.GetUndersizeDiffInputA();
        ushort? undersizeLimitInputA = handlerMeter.GetUndersizeLimitInputA();
        ushort? burstDiffInputA = handlerMeter.GetBurstDiffInputA();
        ushort? burstLimitInputA = handlerMeter.GetBurstLimitInputA();
        byte? vifInputA = handlerMeter.GetVIFInputA();
        ushort? scaleMantissaInputA = handlerMeter.GetScaleMantissaInputA();
        sbyte? scaleExponentInputA = handlerMeter.GetScaleExponentInputA();
        Warning? warningsInputA = handlerMeter.GetWarningsInputA();
        string serialnumberFullInputB = handlerMeter.GetSerialnumberFullInputB();
        uint? serialMbusInputB = handlerMeter.GetSerialMBusInputB();
        byte? mbusAddressInputB = handlerMeter.GetMBusAddressInputB();
        MBusDeviceType? mediumInputB = handlerMeter.GetMediumInputB();
        string manufacturerInputB = handlerMeter.GetManufacturerInputB();
        byte? generationInputB = handlerMeter.GetMBusGenerationInputB();
        string obisInputB = handlerMeter.GetObisInputB();
        ushort? blockLimitInputB = handlerMeter.GetPulseBlockLimitInputB();
        ushort? pulseLeakLimitInputB = handlerMeter.GetPulseLeakLimitInputB();
        ushort? unleakLimitInputB = handlerMeter.GetPulseUnleakLimitInputB();
        short? pulseLeakLowerInputB = handlerMeter.GetPulseLeakLowerInputB();
        short? pulseLeakUpperInputB = handlerMeter.GetPulseLeakUpperInputB();
        ushort? oversizeDiffInputB = handlerMeter.GetOversizeDiffInputB();
        ushort? oversizeLimitInputB = handlerMeter.GetOversizeLimitInputB();
        ushort? undersizeDiffInputB = handlerMeter.GetUndersizeDiffInputB();
        ushort? undersizeLimitInputB = handlerMeter.GetUndersizeLimitInputB();
        ushort? burstDiffInputB = handlerMeter.GetBurstDiffInputB();
        ushort? burstLimitInputB = handlerMeter.GetBurstLimitInputB();
        byte? vifInputB = handlerMeter.GetVIFInputB();
        ushort? scaleMantissaInputB = handlerMeter.GetScaleMantissaInputB();
        sbyte? scaleExponentInputB = handlerMeter.GetScaleExponentInputB();
        Warning? warningsInputB = handlerMeter.GetWarningsInputB();
        RadioMode? radioMode = handlerMeter.GetRadioMode();
        RadioPower? radioPower = handlerMeter.GetRadioPower();
        RadioList? radioListType = handlerMeter.GetRadioListType();
        short? radioTimeBias = handlerMeter.GetRadioTimeBias();
        ushort? transmitInterval = handlerMeter.GetRadioTransmitInterval();
        ushort? radioInstallInterval = handlerMeter.GetRadioInstallInterval();
        byte? radioInstallCount = handlerMeter.GetRadioInstallCount();
        short? frequencyOffset = handlerMeter.GetFrequencyOffset();
        short? radioPacketBoffset = handlerMeter.GetRadioPacketBOffset();
        byte[] aeSkey = handlerMeter.GetAESkey();
        DateTime? systemTime = handlerMeter.GetSystemTime();
        DateTime? dueDate = handlerMeter.GetDueDate();
        int? timeZone = handlerMeter.GetTimeZone();
        DateTime batteryEndDate = handlerMeter.GetBatteryEndDate();
        byte? pulseActivateRadio = handlerMeter.GetPulseActivateRadio();
        ushort? pulsePeriod = handlerMeter.GetPulsePeriod();
        byte? pulseOn = handlerMeter.GetPulseOn();
        ConfigFlagsPDCwMBus? configFlagsPdCwMbus1 = handlerMeter.GetConfigFlagsPDCwMBus();
        RadioFlagsPDCwMBus? radioFlagsPdCwMbus1 = handlerMeter.GetRadioFlagsPDCwMBus();
        HardwareError? hardwareErrors = handlerMeter.GetHardwareErrors();
        uint? meterValueA = handlerMeter.GetMeterValueA();
        uint? meterValueB = handlerMeter.GetMeterValueB();
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
        this.txtFullSerialnumberC.Text = serialnumberFull ?? string.Empty;
        this.txtSerialC.Value = (Decimal) (serialMbusPdc.HasValue ? serialMbusPdc.Value : 0U);
        this.txtMBusAddressC.Value = (Decimal) (mbusAddressPdc.HasValue ? (int) mbusAddressPdc.Value : 0);
        this.cboxMBusMediumC.SelectedItem = mediumPdc.HasValue ? (object) mediumPdc.Value.ToString() : (object) string.Empty;
        this.txtManufacturerC.Text = manufacturerPdc ?? string.Empty;
        this.txtMBusGenerationC.Value = (Decimal) (mbusGenerationPdc.HasValue ? (int) mbusGenerationPdc.Value : 0);
        this.txtObisC.Text = obisPdc ?? string.Empty;
        this.txtFullSerialnumberA.Text = serialnumberFullInputA ?? string.Empty;
        this.txtSerialA.Value = (Decimal) (serialMbusInputA.HasValue ? serialMbusInputA.Value : 0U);
        this.txtMBusAddressA.Value = (Decimal) (mbusAddressInputA.HasValue ? (int) mbusAddressInputA.Value : 0);
        this.cboxMBusMediumA.SelectedItem = mediumInputA.HasValue ? (object) mediumInputA.Value.ToString() : (object) string.Empty;
        this.txtManufacturerA.Text = manufacturerInputA ?? string.Empty;
        this.txtMBusGenerationA.Value = (Decimal) (generationInputA.HasValue ? (int) generationInputA.Value : 0);
        this.txtObisA.Text = obisInputA ?? string.Empty;
        this.txtPulseBlockLimitA.Value = (Decimal) (blockLimitInputA.HasValue ? (int) blockLimitInputA.Value : 0);
        this.txtPulseLeakLimitA.Value = (Decimal) (pulseLeakLimitInputA.HasValue ? (int) pulseLeakLimitInputA.Value : 0);
        this.txtPulseUnleakLimitA.Value = (Decimal) (unleakLimitInputA.HasValue ? (int) unleakLimitInputA.Value : 0);
        this.txtPulseLeakLowerA.Value = (Decimal) (pulseLeakLowerInputA.HasValue ? (int) pulseLeakLowerInputA.Value : 0);
        this.txtPulseLeakUpperA.Value = (Decimal) (pulseLeakUpperInputA.HasValue ? (int) pulseLeakUpperInputA.Value : 0);
        this.txtOversizeDiffA.Value = (Decimal) (oversizeDiffInputA.HasValue ? (int) oversizeDiffInputA.Value : 0);
        this.txtOversizeLimitA.Value = (Decimal) (oversizeLimitInputA.HasValue ? (int) oversizeLimitInputA.Value : 0);
        this.txtUndersizeDiffA.Value = (Decimal) (undersizeDiffInputA.HasValue ? (int) undersizeDiffInputA.Value : 0);
        this.txtUndersizeLimitA.Value = (Decimal) (undersizeLimitInputA.HasValue ? (int) undersizeLimitInputA.Value : 0);
        this.txtBurstDiffA.Value = (Decimal) (burstDiffInputA.HasValue ? (int) burstDiffInputA.Value : 0);
        this.txtBurstLimitA.Value = (Decimal) (burstLimitInputA.HasValue ? (int) burstLimitInputA.Value : 0);
        this.txtVifA.Value = (Decimal) (vifInputA.HasValue ? (int) vifInputA.Value : 0);
        this.txtScaleMantissaA.Value = (Decimal) (scaleMantissaInputA.HasValue ? (int) scaleMantissaInputA.Value : 0);
        this.txtScaleExponentA.Value = (Decimal) (scaleExponentInputA.HasValue ? (int) scaleExponentInputA.Value : 0);
        this.txtFullSerialnumberB.Text = serialnumberFullInputB ?? string.Empty;
        this.txtSerialB.Value = (Decimal) (serialMbusInputB.HasValue ? serialMbusInputB.Value : 0U);
        this.txtMBusAddressB.Value = (Decimal) (mbusAddressInputB.HasValue ? (int) mbusAddressInputB.Value : 0);
        this.cboxMBusMediumB.SelectedItem = mediumInputB.HasValue ? (object) mediumInputB.Value.ToString() : (object) string.Empty;
        this.txtManufacturerB.Text = manufacturerInputB ?? string.Empty;
        this.txtMBusGenerationB.Value = (Decimal) (generationInputB.HasValue ? (int) generationInputB.Value : 0);
        this.txtObisB.Text = obisInputB ?? string.Empty;
        this.txtPulseBlockLimitB.Value = (Decimal) (blockLimitInputB.HasValue ? (int) blockLimitInputB.Value : 0);
        this.txtPulseLeakLimitB.Value = (Decimal) (pulseLeakLimitInputB.HasValue ? (int) pulseLeakLimitInputB.Value : 0);
        this.txtPulseUnleakLimitB.Value = (Decimal) (unleakLimitInputB.HasValue ? (int) unleakLimitInputB.Value : 0);
        this.txtPulseLeakLowerB.Value = (Decimal) (pulseLeakLowerInputB.HasValue ? (int) pulseLeakLowerInputB.Value : 0);
        this.txtPulseLeakUpperB.Value = (Decimal) (pulseLeakUpperInputB.HasValue ? (int) pulseLeakUpperInputB.Value : 0);
        this.txtOversizeDiffB.Value = (Decimal) (oversizeDiffInputB.HasValue ? (int) oversizeDiffInputB.Value : 0);
        this.txtOversizeLimitB.Value = (Decimal) (oversizeLimitInputB.HasValue ? (int) oversizeLimitInputB.Value : 0);
        this.txtUndersizeDiffB.Value = (Decimal) (undersizeDiffInputB.HasValue ? (int) undersizeDiffInputB.Value : 0);
        this.txtUndersizeLimitB.Value = (Decimal) (undersizeLimitInputB.HasValue ? (int) undersizeLimitInputB.Value : 0);
        this.txtBurstDiffB.Value = (Decimal) (burstDiffInputB.HasValue ? (int) burstDiffInputB.Value : 0);
        this.txtBurstLimitB.Value = (Decimal) (burstLimitInputB.HasValue ? (int) burstLimitInputB.Value : 0);
        this.txtVifB.Value = (Decimal) (vifInputB.HasValue ? (int) vifInputB.Value : 0);
        this.txtScaleMantissaB.Value = (Decimal) (scaleMantissaInputB.HasValue ? (int) scaleMantissaInputB.Value : 0);
        this.txtScaleExponentB.Value = (Decimal) (scaleExponentInputB.HasValue ? (int) scaleExponentInputB.Value : 0);
        this.cboxRadioMode.SelectedItem = radioMode.HasValue ? (object) radioMode.Value.ToString() : (object) string.Empty;
        this.cboxRadioPower.SelectedItem = radioPower.HasValue ? (object) radioPower.Value.ToString() : (object) string.Empty;
        this.cboxRadioListType.SelectedItem = radioListType.HasValue ? (object) radioListType.Value.ToString() : (object) string.Empty;
        this.txtRadioTimeBias.Value = (Decimal) (radioTimeBias.HasValue ? (int) radioTimeBias.Value : 0);
        this.txtRadioTransmitInterval.Value = (Decimal) (transmitInterval.HasValue ? (int) transmitInterval.Value : 0);
        this.txtRadioInstallInterval.Value = (Decimal) (radioInstallInterval.HasValue ? (int) radioInstallInterval.Value : 0);
        this.txtRadioInstallCount.Value = (Decimal) (radioInstallCount.HasValue ? (int) radioInstallCount.Value : 0);
        this.txtRadioFrequencyOffset.Value = (Decimal) (frequencyOffset.HasValue ? (int) frequencyOffset.Value : 0);
        this.txtRadioPacketBOffset.Value = (Decimal) (radioPacketBoffset.HasValue ? (int) radioPacketBoffset.Value : 0);
        this.txtAESkey.Text = aeSkey != null ? ZR_ClassLibrary.Util.ByteArrayToHexString(aeSkey) : string.Empty;
        this.txtSystemTime.Value = systemTime.HasValue ? systemTime.Value : PDC_HandlerFunctions.DateTimeNull;
        this.txtDueDate.Value = dueDate.HasValue ? dueDate.Value : PDC_HandlerFunctions.DateTimeNull;
        this.txtTimeZone.Value = (Decimal) (timeZone.HasValue ? timeZone.Value : 0);
        this.txtBatteryEndDate.Value = batteryEndDate;
        this.txtPulseActivateRadio.Value = (Decimal) (pulseActivateRadio.HasValue ? (int) pulseActivateRadio.Value : 0);
        this.txtPulsePeriod.Value = (Decimal) (pulsePeriod.HasValue ? (int) pulsePeriod.Value : 0);
        this.txtPulseOn.Value = (Decimal) (pulseOn.HasValue ? (int) pulseOn.Value : 0);
        this.txtPulseReadingA.Value = (Decimal) (meterValueA.HasValue ? meterValueA.Value : 0U);
        this.txtPulseReadingB.Value = (Decimal) (meterValueB.HasValue ? meterValueB.Value : 0U);
        if (hardwareErrors.HasValue)
        {
          for (int index = 0; index < this.listHardwareErrors.Items.Count; ++index)
          {
            HardwareError hardwareError1 = (HardwareError) Enum.Parse(typeof (HardwareError), this.listHardwareErrors.GetItemText(this.listHardwareErrors.Items[index]), true);
            HardwareError? nullable1 = hardwareErrors;
            HardwareError hardwareError2 = hardwareError1;
            HardwareError? nullable2 = nullable1.HasValue ? new HardwareError?(nullable1.GetValueOrDefault() & hardwareError2) : new HardwareError?();
            HardwareError hardwareError3 = hardwareError1;
            bool flag = nullable2.GetValueOrDefault() == hardwareError3 & nullable2.HasValue;
            this.listHardwareErrors.SetItemChecked(index, flag);
          }
        }
        Warning? nullable3;
        if (warningsInputA.HasValue)
        {
          for (int index = 0; index < this.listPersistentFlagsA.Items.Count; ++index)
          {
            Warning warning1 = (Warning) Enum.Parse(typeof (Warning), this.listPersistentFlagsA.GetItemText(this.listPersistentFlagsA.Items[index]), true);
            nullable3 = warningsInputA;
            Warning warning2 = warning1;
            Warning? nullable4 = nullable3.HasValue ? new Warning?(nullable3.GetValueOrDefault() & warning2) : new Warning?();
            Warning warning3 = warning1;
            bool flag = nullable4.GetValueOrDefault() == warning3 & nullable4.HasValue;
            this.listPersistentFlagsA.SetItemChecked(index, flag);
          }
        }
        if (warningsInputB.HasValue)
        {
          for (int index = 0; index < this.listPersistentFlagsB.Items.Count; ++index)
          {
            Warning warning4 = (Warning) Enum.Parse(typeof (Warning), this.listPersistentFlagsB.GetItemText(this.listPersistentFlagsB.Items[index]), true);
            nullable3 = warningsInputB;
            Warning warning5 = warning4;
            Warning? nullable5 = nullable3.HasValue ? new Warning?(nullable3.GetValueOrDefault() & warning5) : new Warning?();
            Warning warning6 = warning4;
            bool flag = nullable5.GetValueOrDefault() == warning6 & nullable5.HasValue;
            this.listPersistentFlagsB.SetItemChecked(index, flag);
          }
        }
        if (configFlagsPdCwMbus1.HasValue)
        {
          for (int index = 0; index < this.listConfigFlags.Items.Count; ++index)
          {
            ConfigFlagsPDCwMBus configFlagsPdCwMbus2 = (ConfigFlagsPDCwMBus) Enum.Parse(typeof (ConfigFlagsPDCwMBus), this.listConfigFlags.GetItemText(this.listConfigFlags.Items[index]), true);
            ConfigFlagsPDCwMBus? nullable6 = configFlagsPdCwMbus1;
            ConfigFlagsPDCwMBus configFlagsPdCwMbus3 = configFlagsPdCwMbus2;
            ConfigFlagsPDCwMBus? nullable7 = nullable6.HasValue ? new ConfigFlagsPDCwMBus?(nullable6.GetValueOrDefault() & configFlagsPdCwMbus3) : new ConfigFlagsPDCwMBus?();
            ConfigFlagsPDCwMBus configFlagsPdCwMbus4 = configFlagsPdCwMbus2;
            bool flag = nullable7.GetValueOrDefault() == configFlagsPdCwMbus4 & nullable7.HasValue;
            this.listConfigFlags.SetItemChecked(index, flag);
          }
        }
        if (!radioFlagsPdCwMbus1.HasValue)
          return;
        for (int index = 0; index < this.listRadioFlags.Items.Count; ++index)
        {
          RadioFlagsPDCwMBus radioFlagsPdCwMbus2 = (RadioFlagsPDCwMBus) Enum.Parse(typeof (RadioFlagsPDCwMBus), this.listRadioFlags.GetItemText(this.listRadioFlags.Items[index]), true);
          RadioFlagsPDCwMBus? nullable8 = radioFlagsPdCwMbus1;
          RadioFlagsPDCwMBus radioFlagsPdCwMbus3 = radioFlagsPdCwMbus2;
          RadioFlagsPDCwMBus? nullable9 = nullable8.HasValue ? new RadioFlagsPDCwMBus?(nullable8.GetValueOrDefault() & radioFlagsPdCwMbus3) : new RadioFlagsPDCwMBus?();
          RadioFlagsPDCwMBus radioFlagsPdCwMbus4 = radioFlagsPdCwMbus2;
          bool flag = nullable9.GetValueOrDefault() == radioFlagsPdCwMbus4 & nullable9.HasValue;
          this.listRadioFlags.SetItemChecked(index, flag);
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
        PDC_Meter handlerMeter = this.GetHandlerMeter();
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
        handlerMeter.SetSerialnumberFull(this.txtFullSerialnumberC.Text);
        handlerMeter.SetSerialMBusPDC(Convert.ToUInt32(this.txtSerialC.Value));
        handlerMeter.SetMBusAddressPDC(Convert.ToByte(this.txtMBusAddressC.Value));
        handlerMeter.SetMediumPDC((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMediumC.SelectedItem.ToString(), true));
        handlerMeter.SetManufacturerPDC(this.txtManufacturerC.Text);
        handlerMeter.SetMBusGenerationPDC(Convert.ToByte(this.txtMBusGenerationC.Value));
        handlerMeter.SetObisPDC(this.txtObisC.Text);
        handlerMeter.SetSerialnumberFullInputA(this.txtFullSerialnumberA.Text);
        handlerMeter.SetSerialMBusInputA(Convert.ToUInt32(this.txtSerialA.Value));
        handlerMeter.SetMBusAddressInputA(Convert.ToByte(this.txtMBusAddressA.Value));
        handlerMeter.SetMediumInputA((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMediumA.SelectedItem.ToString(), true));
        handlerMeter.SetManufacturerInputA(this.txtManufacturerA.Text);
        handlerMeter.SetMBusGenerationInputA(Convert.ToByte(this.txtMBusGenerationA.Value));
        handlerMeter.SetObisInputA(this.txtObisA.Text);
        handlerMeter.SetPulseBlockLimitInputA(Convert.ToUInt16(this.txtPulseBlockLimitA.Value));
        handlerMeter.SetPulseLeakLimitInputA(Convert.ToUInt16(this.txtPulseLeakLimitA.Value));
        handlerMeter.SetPulseUnleakLimitInputA(Convert.ToUInt16(this.txtPulseUnleakLimitA.Value));
        handlerMeter.SetPulseLeakLowerInputA(Convert.ToInt16(this.txtPulseLeakLowerA.Value));
        handlerMeter.SetPulseLeakUpperInputA(Convert.ToInt16(this.txtPulseLeakUpperA.Value));
        handlerMeter.SetOversizeDiffInputA(Convert.ToUInt16(this.txtOversizeDiffA.Value));
        handlerMeter.SetOversizeLimitInputA(Convert.ToUInt16(this.txtOversizeLimitA.Value));
        handlerMeter.SetUndersizeDiffInputA(Convert.ToUInt16(this.txtUndersizeDiffA.Value));
        handlerMeter.SetUndersizeLimitInputA(Convert.ToUInt16(this.txtUndersizeLimitA.Value));
        handlerMeter.SetBurstDiffInputA(Convert.ToUInt16(this.txtBurstDiffA.Value));
        handlerMeter.SetBurstLimitInputA(Convert.ToUInt16(this.txtBurstLimitA.Value));
        handlerMeter.SetVIFInputA(Convert.ToByte(this.txtVifA.Value));
        handlerMeter.SetScaleMantissaInputA(Convert.ToUInt16(this.txtScaleMantissaA.Value));
        handlerMeter.SetScaleExponentInputA(Convert.ToSByte(this.txtScaleExponentA.Value));
        handlerMeter.SetWarningsInputA(this.GetWarningsInputA());
        handlerMeter.SetSerialnumberFullInputB(this.txtFullSerialnumberB.Text);
        handlerMeter.SetSerialMBusInputB(Convert.ToUInt32(this.txtSerialB.Value));
        handlerMeter.SetMBusAddressInputB(Convert.ToByte(this.txtMBusAddressB.Value));
        handlerMeter.SetMediumInputB((MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMediumB.SelectedItem.ToString(), true));
        handlerMeter.SetManufacturerInputB(this.txtManufacturerB.Text);
        handlerMeter.SetMBusGenerationInputB(Convert.ToByte(this.txtMBusGenerationB.Value));
        handlerMeter.SetObisInputB(this.txtObisB.Text);
        handlerMeter.SetPulseBlockLimitInputB(Convert.ToUInt16(this.txtPulseBlockLimitB.Value));
        handlerMeter.SetPulseLeakLimitInputB(Convert.ToUInt16(this.txtPulseLeakLimitB.Value));
        handlerMeter.SetPulseUnleakLimitInputB(Convert.ToUInt16(this.txtPulseUnleakLimitB.Value));
        handlerMeter.SetPulseLeakLowerInputB(Convert.ToInt16(this.txtPulseLeakLowerB.Value));
        handlerMeter.SetPulseLeakUpperInputB(Convert.ToInt16(this.txtPulseLeakUpperB.Value));
        handlerMeter.SetOversizeDiffInputB(Convert.ToUInt16(this.txtOversizeDiffB.Value));
        handlerMeter.SetOversizeLimitInputB(Convert.ToUInt16(this.txtOversizeLimitB.Value));
        handlerMeter.SetUndersizeDiffInputB(Convert.ToUInt16(this.txtUndersizeDiffB.Value));
        handlerMeter.SetUndersizeLimitInputB(Convert.ToUInt16(this.txtUndersizeLimitB.Value));
        handlerMeter.SetBurstDiffInputB(Convert.ToUInt16(this.txtBurstDiffB.Value));
        handlerMeter.SetBurstLimitInputB(Convert.ToUInt16(this.txtBurstLimitB.Value));
        handlerMeter.SetVIFInputB(Convert.ToByte(this.txtVifB.Value));
        handlerMeter.SetScaleMantissaInputB(Convert.ToUInt16(this.txtScaleMantissaB.Value));
        handlerMeter.SetScaleExponentInputB(Convert.ToSByte(this.txtScaleExponentB.Value));
        handlerMeter.SetWarningsInputB(this.GetWarningsInputB());
        handlerMeter.SetRadioMode((RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioMode.SelectedItem.ToString(), true));
        handlerMeter.SetRadioPower((RadioPower) Enum.Parse(typeof (RadioPower), this.cboxRadioPower.SelectedItem.ToString(), true));
        handlerMeter.SetRadioListType((RadioList) Enum.Parse(typeof (RadioList), this.cboxRadioListType.SelectedItem.ToString(), true));
        handlerMeter.SetRadioTimeBias(Convert.ToInt16(this.txtRadioTimeBias.Value));
        handlerMeter.SetRadioTransmitInterval(Convert.ToUInt16(this.txtRadioTransmitInterval.Value));
        handlerMeter.SetRadioInstallInterval(Convert.ToUInt16(this.txtRadioInstallInterval.Value));
        handlerMeter.SetRadioInstallCount(Convert.ToByte(this.txtRadioInstallCount.Value));
        handlerMeter.SetFrequencyOffset(Convert.ToInt16(this.txtRadioFrequencyOffset.Value));
        handlerMeter.SetRadioPacketBOffset(Convert.ToInt16(this.txtRadioPacketBOffset.Value));
        handlerMeter.SetAESkey(ZR_ClassLibrary.Util.HexStringToByteArray(this.txtAESkey.Text));
        handlerMeter.SetDueDate(this.txtDueDate.Value);
        handlerMeter.SetTimeZone(Convert.ToInt32(this.txtTimeZone.Value));
        handlerMeter.SetBatteryEndDate(this.txtBatteryEndDate.Value);
        handlerMeter.SetPulseActivateRadio(Convert.ToByte(this.txtPulseActivateRadio.Value));
        handlerMeter.SetPulsePeriod(Convert.ToUInt16(this.txtPulsePeriod.Value));
        handlerMeter.SetPulseOn(Convert.ToByte(this.txtPulseOn.Value));
        handlerMeter.SetHardwareErrors(this.GetCheckedHardwareErrors());
        handlerMeter.SetRadioFlagsPDCwMBus(this.GetRadioFlagsPDCwMBus());
        handlerMeter.SetConfigFlagsPDCwMBus(this.GetConfigFlagsPDCwMBus());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Error occurred while save the values", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private Warning GetWarningsInputA()
    {
      Warning warningsInputA = (Warning) 0;
      foreach (object checkedItem in this.listPersistentFlagsA.CheckedItems)
      {
        Warning warning = (Warning) Enum.Parse(typeof (Warning), checkedItem.ToString(), true);
        warningsInputA |= warning;
      }
      return warningsInputA;
    }

    private Warning GetWarningsInputB()
    {
      Warning warningsInputB = (Warning) 0;
      foreach (object checkedItem in this.listPersistentFlagsB.CheckedItems)
      {
        Warning warning = (Warning) Enum.Parse(typeof (Warning), checkedItem.ToString(), true);
        warningsInputB |= warning;
      }
      return warningsInputB;
    }

    private ConfigFlagsPDCwMBus GetConfigFlagsPDCwMBus()
    {
      ConfigFlagsPDCwMBus configFlagsPdCwMbus1 = (ConfigFlagsPDCwMBus) 0;
      foreach (object checkedItem in this.listConfigFlags.CheckedItems)
      {
        ConfigFlagsPDCwMBus configFlagsPdCwMbus2 = (ConfigFlagsPDCwMBus) Enum.Parse(typeof (ConfigFlagsPDCwMBus), checkedItem.ToString(), true);
        configFlagsPdCwMbus1 |= configFlagsPdCwMbus2;
      }
      return configFlagsPdCwMbus1;
    }

    private RadioFlagsPDCwMBus GetRadioFlagsPDCwMBus()
    {
      RadioFlagsPDCwMBus radioFlagsPdCwMbus1 = (RadioFlagsPDCwMBus) 0;
      foreach (object checkedItem in this.listRadioFlags.CheckedItems)
      {
        RadioFlagsPDCwMBus radioFlagsPdCwMbus2 = (RadioFlagsPDCwMBus) Enum.Parse(typeof (RadioFlagsPDCwMBus), checkedItem.ToString(), true);
        radioFlagsPdCwMbus1 |= radioFlagsPdCwMbus2;
      }
      return radioFlagsPdCwMbus1;
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

    private PDC_Meter GetHandlerMeter()
    {
      PDC_Meter handlerMeter;
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
      this.cboxMBusMediumA.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (MBusDeviceType));
      this.cboxMBusMediumB.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (MBusDeviceType));
      this.cboxMBusMediumC.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (MBusDeviceType));
      this.cboxRadioPower.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (RadioPower));
      this.cboxRadioListType.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (RadioList));
      List<string> stringList = new List<string>((IEnumerable<string>) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (RadioMode)));
      stringList.Remove(RadioMode.Radio2.ToString());
      stringList.Remove(RadioMode.Radio3.ToString());
      this.cboxRadioMode.DataSource = (object) stringList;
      this.listHardwareErrors.Items.Clear();
      foreach (object obj in ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (HardwareError)))
        this.listHardwareErrors.Items.Add(obj, false);
      this.listConfigFlags.Items.Clear();
      foreach (object obj in ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (ConfigFlagsPDCwMBus)))
        this.listConfigFlags.Items.Add(obj, false);
      this.listRadioFlags.Items.Clear();
      foreach (object obj in ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (RadioFlagsPDCwMBus)))
        this.listRadioFlags.Items.Add(obj, false);
      this.listPersistentFlagsA.Items.Clear();
      this.listPersistentFlagsB.Items.Clear();
      foreach (string str in ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (Warning)))
      {
        this.listPersistentFlagsA.Items.Add((object) str, false);
        this.listPersistentFlagsB.Items.Add((object) str, false);
      }
      this.cboxHandlerObject.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (HandlerMeterType));
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConfiguratorPdcRadio));
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
      this.label75 = new Label();
      this.txtRadioTimeBias = new NumericUpDown();
      this.cboxRadioListType = new ComboBox();
      this.label62 = new Label();
      this.label40 = new Label();
      this.txtRadioPacketBOffset = new NumericUpDown();
      this.label59 = new Label();
      this.txtRadioInstallCount = new NumericUpDown();
      this.label57 = new Label();
      this.txtRadioInstallInterval = new NumericUpDown();
      this.txtAESkey = new TextBox();
      this.label18 = new Label();
      this.label17 = new Label();
      this.txtRadioFrequencyOffset = new NumericUpDown();
      this.label16 = new Label();
      this.cboxRadioPower = new ComboBox();
      this.label9 = new Label();
      this.txtRadioTransmitInterval = new NumericUpDown();
      this.label8 = new Label();
      this.cboxRadioMode = new ComboBox();
      this.label34 = new Label();
      this.txtPulseActivateRadio = new NumericUpDown();
      this.groupBox3 = new GroupBox();
      this.txtSystemTime = new DateTimePicker();
      this.label76 = new Label();
      this.label39 = new Label();
      this.txtPulseOn = new NumericUpDown();
      this.txtBatteryEndDate = new DateTimePicker();
      this.label63 = new Label();
      this.label26 = new Label();
      this.txtPulsePeriod = new NumericUpDown();
      this.label12 = new Label();
      this.txtTimeZone = new NumericUpDown();
      this.txtDueDate = new DateTimePicker();
      this.label22 = new Label();
      this.label13 = new Label();
      this.txtPulseReadingA = new NumericUpDown();
      this.txtSerialC = new NumericUpDown();
      this.txtObisC = new TextBox();
      this.label37 = new Label();
      this.label23 = new Label();
      this.label19 = new Label();
      this.label15 = new Label();
      this.label14 = new Label();
      this.label10 = new Label();
      this.txtManufacturerC = new TextBox();
      this.txtMBusAddressC = new NumericUpDown();
      this.cboxMBusMediumC = new ComboBox();
      this.txtMBusGenerationC = new NumericUpDown();
      this.toolTip = new ToolTip();
      this.label36 = new Label();
      this.cboxHandlerObject = new ComboBox();
      this.panel = new Panel();
      this.groupBox10 = new GroupBox();
      this.listPersistentFlagsB = new CheckedListBox();
      this.groupBox9 = new GroupBox();
      this.listPersistentFlagsA = new CheckedListBox();
      this.groupBox7 = new GroupBox();
      this.listRadioFlags = new CheckedListBox();
      this.groupBox2 = new GroupBox();
      this.listConfigFlags = new CheckedListBox();
      this.groupBox6 = new GroupBox();
      this.label11 = new Label();
      this.txtFullSerialnumberB = new TextBox();
      this.txtScaleExponentB = new NumericUpDown();
      this.label20 = new Label();
      this.label33 = new Label();
      this.txtMBusAddressB = new NumericUpDown();
      this.txtScaleMantissaB = new NumericUpDown();
      this.txtMBusGenerationB = new NumericUpDown();
      this.txtVifB = new NumericUpDown();
      this.txtSerialB = new NumericUpDown();
      this.label35 = new Label();
      this.cboxMBusMediumB = new ComboBox();
      this.label64 = new Label();
      this.txtObisB = new TextBox();
      this.label65 = new Label();
      this.txtManufacturerB = new TextBox();
      this.label66 = new Label();
      this.label21 = new Label();
      this.label67 = new Label();
      this.label24 = new Label();
      this.label68 = new Label();
      this.label25 = new Label();
      this.label69 = new Label();
      this.label27 = new Label();
      this.label70 = new Label();
      this.label28 = new Label();
      this.label71 = new Label();
      this.label29 = new Label();
      this.label72 = new Label();
      this.txtUndersizeLimitB = new NumericUpDown();
      this.label73 = new Label();
      this.txtPulseBlockLimitB = new NumericUpDown();
      this.label74 = new Label();
      this.txtPulseLeakLimitB = new NumericUpDown();
      this.txtBurstLimitB = new NumericUpDown();
      this.txtPulseUnleakLimitB = new NumericUpDown();
      this.txtBurstDiffB = new NumericUpDown();
      this.txtPulseLeakLowerB = new NumericUpDown();
      this.txtPulseLeakUpperB = new NumericUpDown();
      this.txtUndersizeDiffB = new NumericUpDown();
      this.txtOversizeDiffB = new NumericUpDown();
      this.txtOversizeLimitB = new NumericUpDown();
      this.groupBox5 = new GroupBox();
      this.label32 = new Label();
      this.txtScaleExponentA = new NumericUpDown();
      this.label31 = new Label();
      this.txtScaleMantissaA = new NumericUpDown();
      this.txtVifA = new NumericUpDown();
      this.label30 = new Label();
      this.label51 = new Label();
      this.label52 = new Label();
      this.label53 = new Label();
      this.label54 = new Label();
      this.label55 = new Label();
      this.label56 = new Label();
      this.label47 = new Label();
      this.label46 = new Label();
      this.label45 = new Label();
      this.label44 = new Label();
      this.label43 = new Label();
      this.txtBurstLimitA = new NumericUpDown();
      this.txtBurstDiffA = new NumericUpDown();
      this.txtUndersizeLimitA = new NumericUpDown();
      this.txtUndersizeDiffA = new NumericUpDown();
      this.txtOversizeLimitA = new NumericUpDown();
      this.txtOversizeDiffA = new NumericUpDown();
      this.txtPulseLeakUpperA = new NumericUpDown();
      this.txtPulseLeakLowerA = new NumericUpDown();
      this.txtPulseUnleakLimitA = new NumericUpDown();
      this.txtPulseLeakLimitA = new NumericUpDown();
      this.txtPulseBlockLimitA = new NumericUpDown();
      this.txtFullSerialnumberA = new TextBox();
      this.label42 = new Label();
      this.txtMBusAddressA = new NumericUpDown();
      this.txtMBusGenerationA = new NumericUpDown();
      this.txtSerialA = new NumericUpDown();
      this.cboxMBusMediumA = new ComboBox();
      this.txtObisA = new TextBox();
      this.txtManufacturerA = new TextBox();
      this.label48 = new Label();
      this.label49 = new Label();
      this.label50 = new Label();
      this.label58 = new Label();
      this.label60 = new Label();
      this.label61 = new Label();
      this.groupBox4 = new GroupBox();
      this.txtFullSerialnumberC = new TextBox();
      this.label41 = new Label();
      this.label38 = new Label();
      this.txtPulseReadingB = new NumericUpDown();
      this.groupBox8 = new GroupBox();
      this.listHardwareErrors = new CheckedListBox();
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
      this.txtRadioTimeBias.BeginInit();
      this.txtRadioPacketBOffset.BeginInit();
      this.txtRadioInstallCount.BeginInit();
      this.txtRadioInstallInterval.BeginInit();
      this.txtRadioFrequencyOffset.BeginInit();
      this.txtRadioTransmitInterval.BeginInit();
      this.txtPulseActivateRadio.BeginInit();
      this.groupBox3.SuspendLayout();
      this.txtPulseOn.BeginInit();
      this.txtPulsePeriod.BeginInit();
      this.txtTimeZone.BeginInit();
      this.txtPulseReadingA.BeginInit();
      this.txtSerialC.BeginInit();
      this.txtMBusAddressC.BeginInit();
      this.txtMBusGenerationC.BeginInit();
      this.panel.SuspendLayout();
      this.groupBox10.SuspendLayout();
      this.groupBox9.SuspendLayout();
      this.groupBox7.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.txtScaleExponentB.BeginInit();
      this.txtMBusAddressB.BeginInit();
      this.txtScaleMantissaB.BeginInit();
      this.txtMBusGenerationB.BeginInit();
      this.txtVifB.BeginInit();
      this.txtSerialB.BeginInit();
      this.txtUndersizeLimitB.BeginInit();
      this.txtPulseBlockLimitB.BeginInit();
      this.txtPulseLeakLimitB.BeginInit();
      this.txtBurstLimitB.BeginInit();
      this.txtPulseUnleakLimitB.BeginInit();
      this.txtBurstDiffB.BeginInit();
      this.txtPulseLeakLowerB.BeginInit();
      this.txtPulseLeakUpperB.BeginInit();
      this.txtUndersizeDiffB.BeginInit();
      this.txtOversizeDiffB.BeginInit();
      this.txtOversizeLimitB.BeginInit();
      this.groupBox5.SuspendLayout();
      this.txtScaleExponentA.BeginInit();
      this.txtScaleMantissaA.BeginInit();
      this.txtVifA.BeginInit();
      this.txtBurstLimitA.BeginInit();
      this.txtBurstDiffA.BeginInit();
      this.txtUndersizeLimitA.BeginInit();
      this.txtUndersizeDiffA.BeginInit();
      this.txtOversizeLimitA.BeginInit();
      this.txtOversizeDiffA.BeginInit();
      this.txtPulseLeakUpperA.BeginInit();
      this.txtPulseLeakLowerA.BeginInit();
      this.txtPulseUnleakLimitA.BeginInit();
      this.txtPulseLeakLimitA.BeginInit();
      this.txtPulseBlockLimitA.BeginInit();
      this.txtMBusAddressA.BeginInit();
      this.txtMBusGenerationA.BeginInit();
      this.txtSerialA.BeginInit();
      this.groupBox4.SuspendLayout();
      this.txtPulseReadingB.BeginInit();
      this.groupBox8.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(947, 598);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(77, 29);
      this.btnCancel.TabIndex = 19;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.ImeMode = ImeMode.NoControl;
      this.btnSave.Location = new Point(847, 598);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(77, 29);
      this.btnSave.TabIndex = 18;
      this.btnSave.Text = "Save";
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.label1.Location = new Point(11, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(135, 15);
      this.label1.TabIndex = 21;
      this.label1.Tag = (object) "Con_MeterId";
      this.label1.Text = "Meter ID:";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.label2.Location = new Point(11, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(135, 15);
      this.label2.TabIndex = 23;
      this.label2.Tag = (object) "Con_HardwareTypeId";
      this.label2.Text = "Hardware Type ID:";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.label3.Location = new Point(11, 55);
      this.label3.Name = "label3";
      this.label3.Size = new Size(135, 15);
      this.label3.TabIndex = 25;
      this.label3.Tag = (object) "Con_MeterInfoId";
      this.label3.Text = "Meter Info ID:";
      this.label3.TextAlign = ContentAlignment.MiddleRight;
      this.label4.Location = new Point(11, 74);
      this.label4.Name = "label4";
      this.label4.Size = new Size(135, 15);
      this.label4.TabIndex = 27;
      this.label4.Tag = (object) "Con_BaseTypeId";
      this.label4.Text = "Base Type ID:";
      this.label4.TextAlign = ContentAlignment.MiddleRight;
      this.label5.Location = new Point(11, 94);
      this.label5.Name = "label5";
      this.label5.Size = new Size(135, 15);
      this.label5.TabIndex = 29;
      this.label5.Tag = (object) "Con_MeterTypeId";
      this.label5.Text = "Meter Type ID:";
      this.label5.TextAlign = ContentAlignment.MiddleRight;
      this.label6.Location = new Point(11, 111);
      this.label6.Name = "label6";
      this.label6.Size = new Size(135, 15);
      this.label6.TabIndex = 31;
      this.label6.Tag = (object) "Con_SAP_MaterialNumber";
      this.label6.Text = "SAP Material Nr:";
      this.label6.TextAlign = ContentAlignment.MiddleRight;
      this.label7.Location = new Point(11, 129);
      this.label7.Name = "label7";
      this.label7.Size = new Size(135, 15);
      this.label7.TabIndex = 33;
      this.label7.Tag = (object) "Con_SAP_ProductionOrderNumber";
      this.label7.Text = "SAP Order Nr:";
      this.label7.TextAlign = ContentAlignment.MiddleRight;
      this.txtMeterID.Location = new Point(152, 15);
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
      this.txtHardwareTypeID.Location = new Point(152, 34);
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
      this.txtMeterInfoID.Location = new Point(152, 53);
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
      this.txtBaseTypeID.Location = new Point(152, 72);
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
      this.txtMeterTypeID.Location = new Point(152, 91);
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
      this.txtSapMaterialNumber.Location = new Point(152, 110);
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
      this.txtSapProductionOrderNumber.Location = new Point(152, 129);
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
      this.groupBox1.Location = new Point(785, 1);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(245, 154);
      this.groupBox1.TabIndex = 41;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Identification";
      this.gboxRadio.Controls.Add((Control) this.label75);
      this.gboxRadio.Controls.Add((Control) this.txtRadioTimeBias);
      this.gboxRadio.Controls.Add((Control) this.cboxRadioListType);
      this.gboxRadio.Controls.Add((Control) this.label62);
      this.gboxRadio.Controls.Add((Control) this.label40);
      this.gboxRadio.Controls.Add((Control) this.txtRadioPacketBOffset);
      this.gboxRadio.Controls.Add((Control) this.label59);
      this.gboxRadio.Controls.Add((Control) this.txtRadioInstallCount);
      this.gboxRadio.Controls.Add((Control) this.label57);
      this.gboxRadio.Controls.Add((Control) this.txtRadioInstallInterval);
      this.gboxRadio.Controls.Add((Control) this.txtAESkey);
      this.gboxRadio.Controls.Add((Control) this.label18);
      this.gboxRadio.Controls.Add((Control) this.label17);
      this.gboxRadio.Controls.Add((Control) this.txtRadioFrequencyOffset);
      this.gboxRadio.Controls.Add((Control) this.label16);
      this.gboxRadio.Controls.Add((Control) this.cboxRadioPower);
      this.gboxRadio.Controls.Add((Control) this.label9);
      this.gboxRadio.Controls.Add((Control) this.txtRadioTransmitInterval);
      this.gboxRadio.Controls.Add((Control) this.label8);
      this.gboxRadio.Controls.Add((Control) this.cboxRadioMode);
      this.gboxRadio.Location = new Point(4, 157);
      this.gboxRadio.Name = "gboxRadio";
      this.gboxRadio.Size = new Size(266, 232);
      this.gboxRadio.TabIndex = 42;
      this.gboxRadio.TabStop = false;
      this.gboxRadio.Text = "Radio settings";
      this.label75.Location = new Point(5, 77);
      this.label75.Name = "label75";
      this.label75.Size = new Size(194, 15);
      this.label75.TabIndex = 104;
      this.label75.Tag = (object) "cfg_radio_time_bias";
      this.label75.Text = "Radio time bias:";
      this.label75.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioTimeBias.Location = new Point(205, 76);
      this.txtRadioTimeBias.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtRadioTimeBias.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtRadioTimeBias.Name = "txtRadioTimeBias";
      this.txtRadioTimeBias.Size = new Size(53, 20);
      this.txtRadioTimeBias.TabIndex = 105;
      this.txtRadioTimeBias.Value = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.cboxRadioListType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioListType.FormattingEnabled = true;
      this.cboxRadioListType.Location = new Point(166, 54);
      this.cboxRadioListType.Name = "cboxRadioListType";
      this.cboxRadioListType.Size = new Size(93, 21);
      this.cboxRadioListType.TabIndex = 102;
      this.label62.Location = new Point(6, 54);
      this.label62.Name = "label62";
      this.label62.Size = new Size(153, 15);
      this.label62.TabIndex = 103;
      this.label62.Tag = (object) "cfg_list";
      this.label62.Text = "List type:";
      this.label62.TextAlign = ContentAlignment.MiddleRight;
      this.label40.Location = new Point(4, 172);
      this.label40.Name = "label40";
      this.label40.Size = new Size(194, 15);
      this.label40.TabIndex = 61;
      this.label40.Tag = (object) "cfg_radio_packetb_offset";
      this.label40.Text = "Radio packet B offset:";
      this.label40.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioPacketBOffset.Location = new Point(205, 171);
      this.txtRadioPacketBOffset.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtRadioPacketBOffset.Name = "txtRadioPacketBOffset";
      this.txtRadioPacketBOffset.Size = new Size(53, 20);
      this.txtRadioPacketBOffset.TabIndex = 62;
      this.label59.Location = new Point(4, 134);
      this.label59.Name = "label59";
      this.label59.Size = new Size(194, 15);
      this.label59.TabIndex = 59;
      this.label59.Tag = (object) "cfg_radio_install_count";
      this.label59.Text = "Install count:";
      this.label59.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioInstallCount.Location = new Point(205, 133);
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
      this.label57.Location = new Point(4, 115);
      this.label57.Name = "label57";
      this.label57.Size = new Size(194, 15);
      this.label57.TabIndex = 55;
      this.label57.Tag = (object) "cfg_radio_install_basetime";
      this.label57.Text = "Install interval:";
      this.label57.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioInstallInterval.Location = new Point(205, 114);
      this.txtRadioInstallInterval.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
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
      this.txtAESkey.Location = new Point(7, 206);
      this.txtAESkey.MaxLength = 32;
      this.txtAESkey.Name = "txtAESkey";
      this.txtAESkey.Size = new Size(215, 20);
      this.txtAESkey.TabIndex = 46;
      this.label18.Location = new Point(10, 190);
      this.label18.Name = "label18";
      this.label18.Size = new Size(149, 15);
      this.label18.TabIndex = 45;
      this.label18.Tag = (object) "cfg_key";
      this.label18.Text = "AES key (32 chars as HEX)";
      this.label17.Location = new Point(4, 153);
      this.label17.Name = "label17";
      this.label17.Size = new Size(194, 15);
      this.label17.TabIndex = 43;
      this.label17.Tag = (object) "cfg_radio_freq_offset";
      this.label17.Text = "Frequency offset:";
      this.label17.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioFrequencyOffset.Location = new Point(205, 152);
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
      this.label16.Location = new Point(7, 34);
      this.label16.Name = "label16";
      this.label16.Size = new Size(153, 15);
      this.label16.TabIndex = 42;
      this.label16.Tag = (object) "cfg_radio_power";
      this.label16.Text = "Power:";
      this.label16.TextAlign = ContentAlignment.MiddleRight;
      this.cboxRadioPower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioPower.FormattingEnabled = true;
      this.cboxRadioPower.Location = new Point(166, 34);
      this.cboxRadioPower.Name = "cboxRadioPower";
      this.cboxRadioPower.Size = new Size(93, 21);
      this.cboxRadioPower.TabIndex = 41;
      this.label9.Location = new Point(4, 96);
      this.label9.Name = "label9";
      this.label9.Size = new Size(194, 15);
      this.label9.TabIndex = 36;
      this.label9.Tag = (object) "cfg_radio_normal_basetime";
      this.label9.Text = "Transmit interval:";
      this.label9.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioTransmitInterval.Location = new Point(205, 95);
      this.txtRadioTransmitInterval.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
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
      this.label8.Location = new Point(7, 14);
      this.label8.Name = "label8";
      this.label8.Size = new Size(153, 15);
      this.label8.TabIndex = 22;
      this.label8.Tag = (object) "cfg_radio_mode";
      this.label8.Text = "Mode:";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.cboxRadioMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioMode.FormattingEnabled = true;
      this.cboxRadioMode.Location = new Point(166, 14);
      this.cboxRadioMode.Name = "cboxRadioMode";
      this.cboxRadioMode.Size = new Size(93, 21);
      this.cboxRadioMode.TabIndex = 0;
      this.label34.Location = new Point(8, 98);
      this.label34.Name = "label34";
      this.label34.Size = new Size(157, 15);
      this.label34.TabIndex = 51;
      this.label34.Tag = (object) "cfg_pulse_activate";
      this.label34.Text = "Pulse activate:";
      this.label34.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseActivateRadio.Location = new Point(171, 97);
      this.txtPulseActivateRadio.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseActivateRadio.Name = "txtPulseActivateRadio";
      this.txtPulseActivateRadio.Size = new Size(86, 20);
      this.txtPulseActivateRadio.TabIndex = 52;
      this.groupBox3.Controls.Add((Control) this.txtSystemTime);
      this.groupBox3.Controls.Add((Control) this.label76);
      this.groupBox3.Controls.Add((Control) this.label39);
      this.groupBox3.Controls.Add((Control) this.txtPulseOn);
      this.groupBox3.Controls.Add((Control) this.txtBatteryEndDate);
      this.groupBox3.Controls.Add((Control) this.label34);
      this.groupBox3.Controls.Add((Control) this.label63);
      this.groupBox3.Controls.Add((Control) this.txtPulseActivateRadio);
      this.groupBox3.Controls.Add((Control) this.label26);
      this.groupBox3.Controls.Add((Control) this.txtPulsePeriod);
      this.groupBox3.Controls.Add((Control) this.label12);
      this.groupBox3.Controls.Add((Control) this.txtTimeZone);
      this.groupBox3.Controls.Add((Control) this.txtDueDate);
      this.groupBox3.Controls.Add((Control) this.label22);
      this.groupBox3.Location = new Point(4, 390);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(266, 160);
      this.groupBox3.TabIndex = 43;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Common settings";
      this.txtSystemTime.CustomFormat = "dd.MM.yyyy HH:mm";
      this.txtSystemTime.Enabled = false;
      this.txtSystemTime.Format = DateTimePickerFormat.Custom;
      this.txtSystemTime.Location = new Point(147, 16);
      this.txtSystemTime.Name = "txtSystemTime";
      this.txtSystemTime.ShowUpDown = true;
      this.txtSystemTime.Size = new Size(111, 20);
      this.txtSystemTime.TabIndex = 70;
      this.label76.Location = new Point(7, 16);
      this.label76.Name = "label76";
      this.label76.Size = new Size(134, 15);
      this.label76.TabIndex = 69;
      this.label76.Tag = (object) "hwSystemDate";
      this.label76.Text = "System time:";
      this.label76.TextAlign = ContentAlignment.MiddleRight;
      this.label39.Location = new Point(5, 135);
      this.label39.Name = "label39";
      this.label39.Size = new Size(160, 15);
      this.label39.TabIndex = 67;
      this.label39.Tag = (object) "cfg_pulse_on";
      this.label39.Text = "Pulse on:";
      this.label39.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseOn.Location = new Point(171, 135);
      this.txtPulseOn.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtPulseOn.Name = "txtPulseOn";
      this.txtPulseOn.Size = new Size(86, 20);
      this.txtPulseOn.TabIndex = 68;
      this.txtBatteryEndDate.CustomFormat = "dd.MM.yyyy";
      this.txtBatteryEndDate.Format = DateTimePickerFormat.Custom;
      this.txtBatteryEndDate.Location = new Point(171, 77);
      this.txtBatteryEndDate.Name = "txtBatteryEndDate";
      this.txtBatteryEndDate.ShowUpDown = true;
      this.txtBatteryEndDate.Size = new Size(87, 20);
      this.txtBatteryEndDate.TabIndex = 66;
      this.label63.Location = new Point(5, 77);
      this.label63.Name = "label63";
      this.label63.Size = new Size(160, 15);
      this.label63.TabIndex = 65;
      this.label63.Tag = (object) "cfg_lowbatt_year_month_day";
      this.label63.Text = "Battery end date:";
      this.label63.TextAlign = ContentAlignment.MiddleRight;
      this.label26.Location = new Point(5, 116);
      this.label26.Name = "label26";
      this.label26.Size = new Size(160, 15);
      this.label26.TabIndex = 56;
      this.label26.Tag = (object) "cfg_pulse_period";
      this.label26.Text = "Pulse period:";
      this.label26.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulsePeriod.Location = new Point(171, 116);
      this.txtPulsePeriod.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtPulsePeriod.Name = "txtPulsePeriod";
      this.txtPulsePeriod.Size = new Size(86, 20);
      this.txtPulsePeriod.TabIndex = 57;
      this.label12.Location = new Point(5, 58);
      this.label12.Name = "label12";
      this.label12.Size = new Size(160, 15);
      this.label12.TabIndex = 39;
      this.label12.Tag = (object) "Bak_TimeZoneInQuarterHours";
      this.label12.Text = "Timezone (qarter hours):";
      this.label12.TextAlign = ContentAlignment.MiddleRight;
      this.txtTimeZone.Location = new Point(171, 58);
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
      this.txtTimeZone.Size = new Size(86, 20);
      this.txtTimeZone.TabIndex = 40;
      this.txtDueDate.CustomFormat = "dd.MM";
      this.txtDueDate.Format = DateTimePickerFormat.Custom;
      this.txtDueDate.Location = new Point(171, 38);
      this.txtDueDate.Name = "txtDueDate";
      this.txtDueDate.ShowUpDown = true;
      this.txtDueDate.Size = new Size(87, 20);
      this.txtDueDate.TabIndex = 44;
      this.label22.Location = new Point(5, 38);
      this.label22.Name = "label22";
      this.label22.Size = new Size(160, 15);
      this.label22.TabIndex = 43;
      this.label22.Tag = (object) "cfg_stichtag_month_day";
      this.label22.Text = "Due date (day, month):";
      this.label22.TextAlign = ContentAlignment.MiddleRight;
      this.label13.Location = new Point(25, 553);
      this.label13.Name = "label13";
      this.label13.Size = new Size(133, 15);
      this.label13.TabIndex = 43;
      this.label13.Tag = (object) "pulseReadingA";
      this.label13.Text = "Pulses Input A:";
      this.label13.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseReadingA.Location = new Point(164, 551);
      this.txtPulseReadingA.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseReadingA.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtPulseReadingA.Name = "txtPulseReadingA";
      this.txtPulseReadingA.ReadOnly = true;
      this.txtPulseReadingA.Size = new Size(106, 20);
      this.txtPulseReadingA.TabIndex = 44;
      this.txtSerialC.Location = new Point(158, 34);
      this.txtSerialC.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtSerialC.Name = "txtSerialC";
      this.txtSerialC.Size = new Size(102, 20);
      this.txtSerialC.TabIndex = 97;
      this.txtObisC.Location = new Point(158, 129);
      this.txtObisC.MaxLength = 3;
      this.txtObisC.Name = "txtObisC";
      this.txtObisC.Size = new Size(102, 20);
      this.txtObisC.TabIndex = 96;
      this.txtObisC.Tag = (object) "";
      this.label37.Location = new Point(5, 129);
      this.label37.Name = "label37";
      this.label37.Size = new Size(147, 15);
      this.label37.TabIndex = 95;
      this.label37.Tag = (object) "cfg_obis_c";
      this.label37.Text = "Obis:";
      this.label37.TextAlign = ContentAlignment.MiddleRight;
      this.label23.Location = new Point(5, 53);
      this.label23.Name = "label23";
      this.label23.Size = new Size(147, 15);
      this.label23.TabIndex = 71;
      this.label23.Tag = (object) "cfg_mbus_address_c";
      this.label23.Text = "M-Bus address:";
      this.label23.TextAlign = ContentAlignment.MiddleRight;
      this.label19.Location = new Point(5, 72);
      this.label19.Name = "label19";
      this.label19.Size = new Size(147, 15);
      this.label19.TabIndex = 73;
      this.label19.Tag = (object) "cfg_mbus_medium_c";
      this.label19.Text = "M-Bus medium:";
      this.label19.TextAlign = ContentAlignment.MiddleRight;
      this.label15.Location = new Point(5, 110);
      this.label15.Name = "label15";
      this.label15.Size = new Size(147, 15);
      this.label15.TabIndex = 75;
      this.label15.Tag = (object) "cfg_mbus_version_c";
      this.label15.Text = "M-Bus generation:";
      this.label15.TextAlign = ContentAlignment.MiddleRight;
      this.label14.Location = new Point(5, 15);
      this.label14.Name = "label14";
      this.label14.Size = new Size(147, 15);
      this.label14.TabIndex = 77;
      this.label14.Tag = (object) "Con_fullserialnumber";
      this.label14.Text = "Full Serialnumber:";
      this.label14.TextAlign = ContentAlignment.MiddleRight;
      this.label10.Location = new Point(5, 92);
      this.label10.Name = "label10";
      this.label10.Size = new Size(147, 15);
      this.label10.TabIndex = 79;
      this.label10.Tag = (object) "cfg_mbus_manid_c";
      this.label10.Text = "Manufacturer:";
      this.label10.TextAlign = ContentAlignment.MiddleRight;
      this.txtManufacturerC.Location = new Point(158, 92);
      this.txtManufacturerC.MaxLength = 3;
      this.txtManufacturerC.Name = "txtManufacturerC";
      this.txtManufacturerC.Size = new Size(102, 20);
      this.txtManufacturerC.TabIndex = 80;
      this.txtManufacturerC.Tag = (object) "";
      this.txtMBusAddressC.Location = new Point(158, 53);
      this.txtMBusAddressC.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMBusAddressC.Name = "txtMBusAddressC";
      this.txtMBusAddressC.Size = new Size(102, 20);
      this.txtMBusAddressC.TabIndex = 72;
      this.cboxMBusMediumC.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMBusMediumC.FormattingEnabled = true;
      this.cboxMBusMediumC.Location = new Point(158, 72);
      this.cboxMBusMediumC.Name = "cboxMBusMediumC";
      this.cboxMBusMediumC.Size = new Size(102, 21);
      this.cboxMBusMediumC.TabIndex = 74;
      this.txtMBusGenerationC.Location = new Point(158, 110);
      this.txtMBusGenerationC.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMBusGenerationC.Name = "txtMBusGenerationC";
      this.txtMBusGenerationC.Size = new Size(102, 20);
      this.txtMBusGenerationC.TabIndex = 76;
      this.toolTip.AutoPopDelay = 10000;
      this.toolTip.InitialDelay = 500;
      this.toolTip.ReshowDelay = 100;
      this.toolTip.ShowAlways = true;
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
      this.panel.Controls.Add((Control) this.groupBox10);
      this.panel.Controls.Add((Control) this.groupBox9);
      this.panel.Controls.Add((Control) this.groupBox7);
      this.panel.Controls.Add((Control) this.groupBox2);
      this.panel.Controls.Add((Control) this.groupBox6);
      this.panel.Controls.Add((Control) this.groupBox5);
      this.panel.Controls.Add((Control) this.groupBox4);
      this.panel.Controls.Add((Control) this.label38);
      this.panel.Controls.Add((Control) this.txtPulseReadingB);
      this.panel.Controls.Add((Control) this.label13);
      this.panel.Controls.Add((Control) this.groupBox8);
      this.panel.Controls.Add((Control) this.txtPulseReadingA);
      this.panel.Controls.Add((Control) this.btnCancel);
      this.panel.Controls.Add((Control) this.btnSave);
      this.panel.Controls.Add((Control) this.groupBox1);
      this.panel.Controls.Add((Control) this.gboxRadio);
      this.panel.Controls.Add((Control) this.groupBox3);
      this.panel.Location = new Point(0, 36);
      this.panel.Name = "panel";
      this.panel.Size = new Size(1036, 634);
      this.panel.TabIndex = 52;
      this.groupBox10.Controls.Add((Control) this.listPersistentFlagsB);
      this.groupBox10.Location = new Point(528, 424);
      this.groupBox10.Name = "groupBox10";
      this.groupBox10.Size = new Size(252, 203);
      this.groupBox10.TabIndex = 50;
      this.groupBox10.TabStop = false;
      this.groupBox10.Tag = (object) "persistentFlagsB";
      this.groupBox10.Text = "Warnings B";
      this.listPersistentFlagsB.CheckOnClick = true;
      this.listPersistentFlagsB.FormattingEnabled = true;
      this.listPersistentFlagsB.Location = new Point(3, 16);
      this.listPersistentFlagsB.Name = "listPersistentFlagsB";
      this.listPersistentFlagsB.Size = new Size(246, 184);
      this.listPersistentFlagsB.TabIndex = 0;
      this.groupBox9.Controls.Add((Control) this.listPersistentFlagsA);
      this.groupBox9.Location = new Point(276, 423);
      this.groupBox9.Name = "groupBox9";
      this.groupBox9.Size = new Size(246, 204);
      this.groupBox9.TabIndex = 49;
      this.groupBox9.TabStop = false;
      this.groupBox9.Tag = (object) "persistentFlagsA";
      this.groupBox9.Text = "Warnings A";
      this.listPersistentFlagsA.CheckOnClick = true;
      this.listPersistentFlagsA.FormattingEnabled = true;
      this.listPersistentFlagsA.Location = new Point(3, 16);
      this.listPersistentFlagsA.Name = "listPersistentFlagsA";
      this.listPersistentFlagsA.Size = new Size(241, 184);
      this.listPersistentFlagsA.TabIndex = 0;
      this.groupBox7.Controls.Add((Control) this.listRadioFlags);
      this.groupBox7.Location = new Point(786, 303);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new Size(244, 130);
      this.groupBox7.TabIndex = 50;
      this.groupBox7.TabStop = false;
      this.groupBox7.Tag = (object) "cfg_radio_flags";
      this.groupBox7.Text = "Radio flags";
      this.listRadioFlags.CheckOnClick = true;
      this.listRadioFlags.FormattingEnabled = true;
      this.listRadioFlags.Location = new Point(3, 16);
      this.listRadioFlags.Name = "listRadioFlags";
      this.listRadioFlags.Size = new Size(235, 109);
      this.listRadioFlags.TabIndex = 0;
      this.groupBox2.Controls.Add((Control) this.listConfigFlags);
      this.groupBox2.Location = new Point(786, 157);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(244, 146);
      this.groupBox2.TabIndex = 49;
      this.groupBox2.TabStop = false;
      this.groupBox2.Tag = (object) "cfg_config_flags";
      this.groupBox2.Text = "Config flags";
      this.listConfigFlags.CheckOnClick = true;
      this.listConfigFlags.FormattingEnabled = true;
      this.listConfigFlags.Location = new Point(3, 16);
      this.listConfigFlags.Name = "listConfigFlags";
      this.listConfigFlags.Size = new Size(235, 124);
      this.listConfigFlags.TabIndex = 0;
      this.groupBox6.Controls.Add((Control) this.label11);
      this.groupBox6.Controls.Add((Control) this.txtFullSerialnumberB);
      this.groupBox6.Controls.Add((Control) this.txtScaleExponentB);
      this.groupBox6.Controls.Add((Control) this.label20);
      this.groupBox6.Controls.Add((Control) this.label33);
      this.groupBox6.Controls.Add((Control) this.txtMBusAddressB);
      this.groupBox6.Controls.Add((Control) this.txtScaleMantissaB);
      this.groupBox6.Controls.Add((Control) this.txtMBusGenerationB);
      this.groupBox6.Controls.Add((Control) this.txtVifB);
      this.groupBox6.Controls.Add((Control) this.txtSerialB);
      this.groupBox6.Controls.Add((Control) this.label35);
      this.groupBox6.Controls.Add((Control) this.cboxMBusMediumB);
      this.groupBox6.Controls.Add((Control) this.label64);
      this.groupBox6.Controls.Add((Control) this.txtObisB);
      this.groupBox6.Controls.Add((Control) this.label65);
      this.groupBox6.Controls.Add((Control) this.txtManufacturerB);
      this.groupBox6.Controls.Add((Control) this.label66);
      this.groupBox6.Controls.Add((Control) this.label21);
      this.groupBox6.Controls.Add((Control) this.label67);
      this.groupBox6.Controls.Add((Control) this.label24);
      this.groupBox6.Controls.Add((Control) this.label68);
      this.groupBox6.Controls.Add((Control) this.label25);
      this.groupBox6.Controls.Add((Control) this.label69);
      this.groupBox6.Controls.Add((Control) this.label27);
      this.groupBox6.Controls.Add((Control) this.label70);
      this.groupBox6.Controls.Add((Control) this.label28);
      this.groupBox6.Controls.Add((Control) this.label71);
      this.groupBox6.Controls.Add((Control) this.label29);
      this.groupBox6.Controls.Add((Control) this.label72);
      this.groupBox6.Controls.Add((Control) this.txtUndersizeLimitB);
      this.groupBox6.Controls.Add((Control) this.label73);
      this.groupBox6.Controls.Add((Control) this.txtPulseBlockLimitB);
      this.groupBox6.Controls.Add((Control) this.label74);
      this.groupBox6.Controls.Add((Control) this.txtPulseLeakLimitB);
      this.groupBox6.Controls.Add((Control) this.txtBurstLimitB);
      this.groupBox6.Controls.Add((Control) this.txtPulseUnleakLimitB);
      this.groupBox6.Controls.Add((Control) this.txtBurstDiffB);
      this.groupBox6.Controls.Add((Control) this.txtPulseLeakLowerB);
      this.groupBox6.Controls.Add((Control) this.txtPulseLeakUpperB);
      this.groupBox6.Controls.Add((Control) this.txtUndersizeDiffB);
      this.groupBox6.Controls.Add((Control) this.txtOversizeDiffB);
      this.groupBox6.Controls.Add((Control) this.txtOversizeLimitB);
      this.groupBox6.Location = new Point(528, 1);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new Size(252, 420);
      this.groupBox6.TabIndex = 101;
      this.groupBox6.TabStop = false;
      this.groupBox6.Text = "Input B";
      this.label11.Location = new Point(8, 397);
      this.label11.Margin = new Padding(3, 0, 0, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(178, 15);
      this.label11.TabIndex = 154;
      this.label11.Tag = (object) "cfg_scale_exponent_b";
      this.label11.Text = "Scale exponent:";
      this.label11.TextAlign = ContentAlignment.MiddleRight;
      this.txtFullSerialnumberB.Location = new Point(144, 16);
      this.txtFullSerialnumberB.Name = "txtFullSerialnumberB";
      this.txtFullSerialnumberB.Size = new Size(102, 20);
      this.txtFullSerialnumberB.TabIndex = 99;
      this.txtFullSerialnumberB.Tag = (object) "";
      this.txtScaleExponentB.Location = new Point(189, 396);
      this.txtScaleExponentB.Maximum = new Decimal(new int[4]
      {
        (int) sbyte.MaxValue,
        0,
        0,
        0
      });
      this.txtScaleExponentB.Minimum = new Decimal(new int[4]
      {
        128,
        0,
        0,
        int.MinValue
      });
      this.txtScaleExponentB.Name = "txtScaleExponentB";
      this.txtScaleExponentB.Size = new Size(57, 20);
      this.txtScaleExponentB.TabIndex = 155;
      this.label20.Location = new Point(5, 34);
      this.label20.Name = "label20";
      this.label20.Size = new Size(133, 15);
      this.label20.TabIndex = 98;
      this.label20.Tag = (object) "cfg_serial_b";
      this.label20.Text = "Serialnumber:";
      this.label20.TextAlign = ContentAlignment.MiddleRight;
      this.label33.Location = new Point(8, 378);
      this.label33.Margin = new Padding(3, 0, 0, 0);
      this.label33.Name = "label33";
      this.label33.Size = new Size(178, 15);
      this.label33.TabIndex = 152;
      this.label33.Tag = (object) "cfg_scale_mantissa_b";
      this.label33.Text = "Scale mantissa:";
      this.label33.TextAlign = ContentAlignment.MiddleRight;
      this.txtMBusAddressB.Location = new Point(144, 53);
      this.txtMBusAddressB.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMBusAddressB.Name = "txtMBusAddressB";
      this.txtMBusAddressB.Size = new Size(102, 20);
      this.txtMBusAddressB.TabIndex = 72;
      this.txtScaleMantissaB.Location = new Point(189, 377);
      this.txtScaleMantissaB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtScaleMantissaB.Name = "txtScaleMantissaB";
      this.txtScaleMantissaB.Size = new Size(57, 20);
      this.txtScaleMantissaB.TabIndex = 153;
      this.txtMBusGenerationB.Location = new Point(144, 110);
      this.txtMBusGenerationB.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMBusGenerationB.Name = "txtMBusGenerationB";
      this.txtMBusGenerationB.Size = new Size(102, 20);
      this.txtMBusGenerationB.TabIndex = 76;
      this.txtVifB.Location = new Point(189, 358);
      this.txtVifB.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtVifB.Name = "txtVifB";
      this.txtVifB.Size = new Size(57, 20);
      this.txtVifB.TabIndex = 151;
      this.txtSerialB.Location = new Point(144, 35);
      this.txtSerialB.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtSerialB.Name = "txtSerialB";
      this.txtSerialB.Size = new Size(102, 20);
      this.txtSerialB.TabIndex = 97;
      this.label35.Location = new Point(50, 358);
      this.label35.Name = "label35";
      this.label35.Size = new Size(136, 15);
      this.label35.TabIndex = 150;
      this.label35.Tag = (object) "cfg_vif_b";
      this.label35.Text = "VIF:";
      this.label35.TextAlign = ContentAlignment.MiddleRight;
      this.cboxMBusMediumB.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMBusMediumB.FormattingEnabled = true;
      this.cboxMBusMediumB.Location = new Point(144, 72);
      this.cboxMBusMediumB.Name = "cboxMBusMediumB";
      this.cboxMBusMediumB.Size = new Size(102, 21);
      this.cboxMBusMediumB.TabIndex = 74;
      this.label64.Location = new Point(8, 340);
      this.label64.Margin = new Padding(3, 0, 0, 0);
      this.label64.Name = "label64";
      this.label64.Size = new Size(178, 15);
      this.label64.TabIndex = 148;
      this.label64.Tag = (object) "cfg_burst_b_limit";
      this.label64.Text = "Burst limit:";
      this.label64.TextAlign = ContentAlignment.MiddleRight;
      this.txtObisB.Location = new Point(144, 129);
      this.txtObisB.MaxLength = 3;
      this.txtObisB.Name = "txtObisB";
      this.txtObisB.Size = new Size(102, 20);
      this.txtObisB.TabIndex = 96;
      this.txtObisB.Tag = (object) "";
      this.label65.Location = new Point(8, 321);
      this.label65.Margin = new Padding(3, 0, 0, 0);
      this.label65.Name = "label65";
      this.label65.Size = new Size(178, 15);
      this.label65.TabIndex = 146;
      this.label65.Tag = (object) "cfg_burst_b_diff";
      this.label65.Text = "Burst diff:";
      this.label65.TextAlign = ContentAlignment.MiddleRight;
      this.txtManufacturerB.Location = new Point(144, 92);
      this.txtManufacturerB.MaxLength = 3;
      this.txtManufacturerB.Name = "txtManufacturerB";
      this.txtManufacturerB.Size = new Size(102, 20);
      this.txtManufacturerB.TabIndex = 80;
      this.txtManufacturerB.Tag = (object) "";
      this.label66.Location = new Point(8, 302);
      this.label66.Margin = new Padding(3, 0, 0, 0);
      this.label66.Name = "label66";
      this.label66.Size = new Size(178, 15);
      this.label66.TabIndex = 144;
      this.label66.Tag = (object) "cfg_undersize_b_limit";
      this.label66.Text = "Undersize limit:";
      this.label66.TextAlign = ContentAlignment.MiddleRight;
      this.label21.Location = new Point(5, 92);
      this.label21.Name = "label21";
      this.label21.Size = new Size(133, 15);
      this.label21.TabIndex = 79;
      this.label21.Tag = (object) "cfg_mbus_manid_b";
      this.label21.Text = "Manufacturer:";
      this.label21.TextAlign = ContentAlignment.MiddleRight;
      this.label67.Location = new Point(8, 283);
      this.label67.Margin = new Padding(3, 0, 0, 0);
      this.label67.Name = "label67";
      this.label67.Size = new Size(178, 15);
      this.label67.TabIndex = 142;
      this.label67.Tag = (object) "cfg_undersize_b_diff";
      this.label67.Text = "Undersize diff:";
      this.label67.TextAlign = ContentAlignment.MiddleRight;
      this.label24.Location = new Point(5, 129);
      this.label24.Name = "label24";
      this.label24.Size = new Size(133, 15);
      this.label24.TabIndex = 95;
      this.label24.Tag = (object) "cfg_obis_b";
      this.label24.Text = "Obis:";
      this.label24.TextAlign = ContentAlignment.MiddleRight;
      this.label68.Location = new Point(8, 264);
      this.label68.Margin = new Padding(3, 0, 0, 0);
      this.label68.Name = "label68";
      this.label68.Size = new Size(178, 15);
      this.label68.TabIndex = 140;
      this.label68.Tag = (object) "cfg_oversize_b_limit";
      this.label68.Text = "Oversize limit:";
      this.label68.TextAlign = ContentAlignment.MiddleRight;
      this.label25.Location = new Point(5, 15);
      this.label25.Name = "label25";
      this.label25.Size = new Size(133, 15);
      this.label25.TabIndex = 77;
      this.label25.Tag = (object) "Con_fullserialnumberB";
      this.label25.Text = "Full Serialnumber:";
      this.label25.TextAlign = ContentAlignment.MiddleRight;
      this.label69.Location = new Point(8, 245);
      this.label69.Margin = new Padding(3, 0, 0, 0);
      this.label69.Name = "label69";
      this.label69.Size = new Size(178, 15);
      this.label69.TabIndex = 138;
      this.label69.Tag = (object) "cfg_oversize_b_diff";
      this.label69.Text = "Oversize diff:";
      this.label69.TextAlign = ContentAlignment.MiddleRight;
      this.label27.Location = new Point(5, 110);
      this.label27.Name = "label27";
      this.label27.Size = new Size(133, 15);
      this.label27.TabIndex = 75;
      this.label27.Tag = (object) "cfg_mbus_version_b";
      this.label27.Text = "M-Bus generation:";
      this.label27.TextAlign = ContentAlignment.MiddleRight;
      this.label70.Location = new Point(8, 226);
      this.label70.Margin = new Padding(3, 0, 0, 0);
      this.label70.Name = "label70";
      this.label70.Size = new Size(178, 15);
      this.label70.TabIndex = 136;
      this.label70.Tag = (object) "cfg_pulse_b_leak_upper";
      this.label70.Text = "Pulse leak upper:";
      this.label70.TextAlign = ContentAlignment.MiddleRight;
      this.label28.Location = new Point(5, 72);
      this.label28.Name = "label28";
      this.label28.Size = new Size(133, 15);
      this.label28.TabIndex = 73;
      this.label28.Tag = (object) "cfg_mbus_medium_b";
      this.label28.Text = "M-Bus medium:";
      this.label28.TextAlign = ContentAlignment.MiddleRight;
      this.label71.Location = new Point(8, 207);
      this.label71.Margin = new Padding(3, 0, 0, 0);
      this.label71.Name = "label71";
      this.label71.Size = new Size(178, 15);
      this.label71.TabIndex = 134;
      this.label71.Tag = (object) "cfg_pulse_b_leak_lower";
      this.label71.Text = "Pulse leak lower:";
      this.label71.TextAlign = ContentAlignment.MiddleRight;
      this.label29.Location = new Point(5, 53);
      this.label29.Name = "label29";
      this.label29.Size = new Size(133, 15);
      this.label29.TabIndex = 71;
      this.label29.Tag = (object) "cfg_mbus_address_b";
      this.label29.Text = "M-Bus address:";
      this.label29.TextAlign = ContentAlignment.MiddleRight;
      this.label72.Location = new Point(8, 189);
      this.label72.Margin = new Padding(3, 0, 0, 0);
      this.label72.Name = "label72";
      this.label72.Size = new Size(178, 15);
      this.label72.TabIndex = 132;
      this.label72.Tag = (object) "cfg_pulse_b_unleak_limit";
      this.label72.Text = "Pulse unleak limit:";
      this.label72.TextAlign = ContentAlignment.MiddleRight;
      this.txtUndersizeLimitB.Location = new Point(189, 301);
      this.txtUndersizeLimitB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtUndersizeLimitB.Name = "txtUndersizeLimitB";
      this.txtUndersizeLimitB.Size = new Size(57, 20);
      this.txtUndersizeLimitB.TabIndex = 145;
      this.label73.Location = new Point(8, 169);
      this.label73.Margin = new Padding(3, 0, 0, 0);
      this.label73.Name = "label73";
      this.label73.Size = new Size(178, 15);
      this.label73.TabIndex = 130;
      this.label73.Tag = (object) "cfg_pulse_b_leak_limit";
      this.label73.Text = "Pulse leak limit:";
      this.label73.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseBlockLimitB.Location = new Point(189, 149);
      this.txtPulseBlockLimitB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseBlockLimitB.Name = "txtPulseBlockLimitB";
      this.txtPulseBlockLimitB.Size = new Size(57, 20);
      this.txtPulseBlockLimitB.TabIndex = 129;
      this.label74.Location = new Point(8, 150);
      this.label74.Margin = new Padding(3, 0, 0, 0);
      this.label74.Name = "label74";
      this.label74.Size = new Size(178, 15);
      this.label74.TabIndex = 128;
      this.label74.Tag = (object) "cfg_pulse_b_block_limit";
      this.label74.Text = "Pulse block limit:";
      this.label74.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseLeakLimitB.Location = new Point(189, 168);
      this.txtPulseLeakLimitB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseLeakLimitB.Name = "txtPulseLeakLimitB";
      this.txtPulseLeakLimitB.Size = new Size(57, 20);
      this.txtPulseLeakLimitB.TabIndex = 131;
      this.txtBurstLimitB.Location = new Point(189, 339);
      this.txtBurstLimitB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtBurstLimitB.Name = "txtBurstLimitB";
      this.txtBurstLimitB.Size = new Size(57, 20);
      this.txtBurstLimitB.TabIndex = 149;
      this.txtPulseUnleakLimitB.Location = new Point(189, 187);
      this.txtPulseUnleakLimitB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseUnleakLimitB.Name = "txtPulseUnleakLimitB";
      this.txtPulseUnleakLimitB.Size = new Size(57, 20);
      this.txtPulseUnleakLimitB.TabIndex = 133;
      this.txtBurstDiffB.Location = new Point(189, 320);
      this.txtBurstDiffB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtBurstDiffB.Name = "txtBurstDiffB";
      this.txtBurstDiffB.Size = new Size(57, 20);
      this.txtBurstDiffB.TabIndex = 147;
      this.txtPulseLeakLowerB.Location = new Point(189, 206);
      this.txtPulseLeakLowerB.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseLeakLowerB.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtPulseLeakLowerB.Name = "txtPulseLeakLowerB";
      this.txtPulseLeakLowerB.Size = new Size(57, 20);
      this.txtPulseLeakLowerB.TabIndex = 135;
      this.txtPulseLeakUpperB.Location = new Point(189, 225);
      this.txtPulseLeakUpperB.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseLeakUpperB.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtPulseLeakUpperB.Name = "txtPulseLeakUpperB";
      this.txtPulseLeakUpperB.Size = new Size(57, 20);
      this.txtPulseLeakUpperB.TabIndex = 137;
      this.txtUndersizeDiffB.Location = new Point(189, 282);
      this.txtUndersizeDiffB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtUndersizeDiffB.Name = "txtUndersizeDiffB";
      this.txtUndersizeDiffB.Size = new Size(57, 20);
      this.txtUndersizeDiffB.TabIndex = 143;
      this.txtOversizeDiffB.Location = new Point(189, 244);
      this.txtOversizeDiffB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtOversizeDiffB.Name = "txtOversizeDiffB";
      this.txtOversizeDiffB.Size = new Size(57, 20);
      this.txtOversizeDiffB.TabIndex = 139;
      this.txtOversizeLimitB.Location = new Point(189, 263);
      this.txtOversizeLimitB.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtOversizeLimitB.Name = "txtOversizeLimitB";
      this.txtOversizeLimitB.Size = new Size(57, 20);
      this.txtOversizeLimitB.TabIndex = 141;
      this.groupBox5.Controls.Add((Control) this.label32);
      this.groupBox5.Controls.Add((Control) this.txtScaleExponentA);
      this.groupBox5.Controls.Add((Control) this.label31);
      this.groupBox5.Controls.Add((Control) this.txtScaleMantissaA);
      this.groupBox5.Controls.Add((Control) this.txtVifA);
      this.groupBox5.Controls.Add((Control) this.label30);
      this.groupBox5.Controls.Add((Control) this.label51);
      this.groupBox5.Controls.Add((Control) this.label52);
      this.groupBox5.Controls.Add((Control) this.label53);
      this.groupBox5.Controls.Add((Control) this.label54);
      this.groupBox5.Controls.Add((Control) this.label55);
      this.groupBox5.Controls.Add((Control) this.label56);
      this.groupBox5.Controls.Add((Control) this.label47);
      this.groupBox5.Controls.Add((Control) this.label46);
      this.groupBox5.Controls.Add((Control) this.label45);
      this.groupBox5.Controls.Add((Control) this.label44);
      this.groupBox5.Controls.Add((Control) this.label43);
      this.groupBox5.Controls.Add((Control) this.txtBurstLimitA);
      this.groupBox5.Controls.Add((Control) this.txtBurstDiffA);
      this.groupBox5.Controls.Add((Control) this.txtUndersizeLimitA);
      this.groupBox5.Controls.Add((Control) this.txtUndersizeDiffA);
      this.groupBox5.Controls.Add((Control) this.txtOversizeLimitA);
      this.groupBox5.Controls.Add((Control) this.txtOversizeDiffA);
      this.groupBox5.Controls.Add((Control) this.txtPulseLeakUpperA);
      this.groupBox5.Controls.Add((Control) this.txtPulseLeakLowerA);
      this.groupBox5.Controls.Add((Control) this.txtPulseUnleakLimitA);
      this.groupBox5.Controls.Add((Control) this.txtPulseLeakLimitA);
      this.groupBox5.Controls.Add((Control) this.txtPulseBlockLimitA);
      this.groupBox5.Controls.Add((Control) this.txtFullSerialnumberA);
      this.groupBox5.Controls.Add((Control) this.label42);
      this.groupBox5.Controls.Add((Control) this.txtMBusAddressA);
      this.groupBox5.Controls.Add((Control) this.txtMBusGenerationA);
      this.groupBox5.Controls.Add((Control) this.txtSerialA);
      this.groupBox5.Controls.Add((Control) this.cboxMBusMediumA);
      this.groupBox5.Controls.Add((Control) this.txtObisA);
      this.groupBox5.Controls.Add((Control) this.txtManufacturerA);
      this.groupBox5.Controls.Add((Control) this.label48);
      this.groupBox5.Controls.Add((Control) this.label49);
      this.groupBox5.Controls.Add((Control) this.label50);
      this.groupBox5.Controls.Add((Control) this.label58);
      this.groupBox5.Controls.Add((Control) this.label60);
      this.groupBox5.Controls.Add((Control) this.label61);
      this.groupBox5.Location = new Point(276, 1);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new Size(246, 420);
      this.groupBox5.TabIndex = 100;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Input A";
      this.label32.Location = new Point(7, 397);
      this.label32.Margin = new Padding(3, 0, 0, 0);
      this.label32.Name = "label32";
      this.label32.Size = new Size(174, 15);
      this.label32.TabIndex = 126;
      this.label32.Tag = (object) "cfg_scale_exponent_a";
      this.label32.Text = "Scale exponent:";
      this.label32.TextAlign = ContentAlignment.MiddleRight;
      this.txtScaleExponentA.Location = new Point(184, 396);
      this.txtScaleExponentA.Maximum = new Decimal(new int[4]
      {
        (int) sbyte.MaxValue,
        0,
        0,
        0
      });
      this.txtScaleExponentA.Minimum = new Decimal(new int[4]
      {
        128,
        0,
        0,
        int.MinValue
      });
      this.txtScaleExponentA.Name = "txtScaleExponentA";
      this.txtScaleExponentA.Size = new Size(57, 20);
      this.txtScaleExponentA.TabIndex = (int) sbyte.MaxValue;
      this.label31.Location = new Point(7, 378);
      this.label31.Margin = new Padding(3, 0, 0, 0);
      this.label31.Name = "label31";
      this.label31.Size = new Size(174, 15);
      this.label31.TabIndex = 124;
      this.label31.Tag = (object) "cfg_scale_mantissa_a";
      this.label31.Text = "Scale mantissa:";
      this.label31.TextAlign = ContentAlignment.MiddleRight;
      this.txtScaleMantissaA.Location = new Point(184, 377);
      this.txtScaleMantissaA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtScaleMantissaA.Name = "txtScaleMantissaA";
      this.txtScaleMantissaA.Size = new Size(57, 20);
      this.txtScaleMantissaA.TabIndex = 125;
      this.txtVifA.Location = new Point(184, 358);
      this.txtVifA.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtVifA.Name = "txtVifA";
      this.txtVifA.Size = new Size(57, 20);
      this.txtVifA.TabIndex = 123;
      this.label30.Location = new Point(49, 358);
      this.label30.Name = "label30";
      this.label30.Size = new Size(132, 15);
      this.label30.TabIndex = 122;
      this.label30.Tag = (object) "cfg_vif_a";
      this.label30.Text = "VIF:";
      this.label30.TextAlign = ContentAlignment.MiddleRight;
      this.label51.Location = new Point(7, 340);
      this.label51.Margin = new Padding(3, 0, 0, 0);
      this.label51.Name = "label51";
      this.label51.Size = new Size(174, 15);
      this.label51.TabIndex = 120;
      this.label51.Tag = (object) "cfg_burst_a_limit";
      this.label51.Text = "Burst limit:";
      this.label51.TextAlign = ContentAlignment.MiddleRight;
      this.label52.Location = new Point(7, 321);
      this.label52.Margin = new Padding(3, 0, 0, 0);
      this.label52.Name = "label52";
      this.label52.Size = new Size(174, 15);
      this.label52.TabIndex = 118;
      this.label52.Tag = (object) "cfg_burst_a_diff";
      this.label52.Text = "Burst diff:";
      this.label52.TextAlign = ContentAlignment.MiddleRight;
      this.label53.Location = new Point(7, 302);
      this.label53.Margin = new Padding(3, 0, 0, 0);
      this.label53.Name = "label53";
      this.label53.Size = new Size(174, 15);
      this.label53.TabIndex = 116;
      this.label53.Tag = (object) "cfg_undersize_a_limit";
      this.label53.Text = "Undersize limit:";
      this.label53.TextAlign = ContentAlignment.MiddleRight;
      this.label54.Location = new Point(7, 283);
      this.label54.Margin = new Padding(3, 0, 0, 0);
      this.label54.Name = "label54";
      this.label54.Size = new Size(174, 15);
      this.label54.TabIndex = 114;
      this.label54.Tag = (object) "cfg_undersize_a_diff";
      this.label54.Text = "Undersize diff:";
      this.label54.TextAlign = ContentAlignment.MiddleRight;
      this.label55.Location = new Point(7, 264);
      this.label55.Margin = new Padding(3, 0, 0, 0);
      this.label55.Name = "label55";
      this.label55.Size = new Size(174, 15);
      this.label55.TabIndex = 112;
      this.label55.Tag = (object) "cfg_oversize_a_limit";
      this.label55.Text = "Oversize limit:";
      this.label55.TextAlign = ContentAlignment.MiddleRight;
      this.label56.Location = new Point(7, 245);
      this.label56.Margin = new Padding(3, 0, 0, 0);
      this.label56.Name = "label56";
      this.label56.Size = new Size(174, 15);
      this.label56.TabIndex = 110;
      this.label56.Tag = (object) "cfg_oversize_a_diff";
      this.label56.Text = "Oversize diff:";
      this.label56.TextAlign = ContentAlignment.MiddleRight;
      this.label47.Location = new Point(7, 226);
      this.label47.Margin = new Padding(3, 0, 0, 0);
      this.label47.Name = "label47";
      this.label47.Size = new Size(174, 15);
      this.label47.TabIndex = 108;
      this.label47.Tag = (object) "cfg_pulse_a_leak_upper";
      this.label47.Text = "Pulse leak upper:";
      this.label47.TextAlign = ContentAlignment.MiddleRight;
      this.label46.Location = new Point(7, 207);
      this.label46.Margin = new Padding(3, 0, 0, 0);
      this.label46.Name = "label46";
      this.label46.Size = new Size(174, 15);
      this.label46.TabIndex = 106;
      this.label46.Tag = (object) "cfg_pulse_a_leak_lower";
      this.label46.Text = "Pulse leak lower:";
      this.label46.TextAlign = ContentAlignment.MiddleRight;
      this.label45.Location = new Point(7, 189);
      this.label45.Margin = new Padding(3, 0, 0, 0);
      this.label45.Name = "label45";
      this.label45.Size = new Size(174, 15);
      this.label45.TabIndex = 104;
      this.label45.Tag = (object) "cfg_pulse_a_unleak_limit";
      this.label45.Text = "Pulse unleak limit:";
      this.label45.TextAlign = ContentAlignment.MiddleRight;
      this.label44.Location = new Point(7, 169);
      this.label44.Margin = new Padding(3, 0, 0, 0);
      this.label44.Name = "label44";
      this.label44.Size = new Size(174, 15);
      this.label44.TabIndex = 102;
      this.label44.Tag = (object) "cfg_pulse_a_leak_limit";
      this.label44.Text = "Pulse leak limit:";
      this.label44.TextAlign = ContentAlignment.MiddleRight;
      this.label43.Location = new Point(7, 150);
      this.label43.Margin = new Padding(3, 0, 0, 0);
      this.label43.Name = "label43";
      this.label43.Size = new Size(174, 15);
      this.label43.TabIndex = 100;
      this.label43.Tag = (object) "cfg_pulse_a_block_limit";
      this.label43.Text = "Pulse block limit:";
      this.label43.TextAlign = ContentAlignment.MiddleRight;
      this.txtBurstLimitA.Location = new Point(184, 339);
      this.txtBurstLimitA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtBurstLimitA.Name = "txtBurstLimitA";
      this.txtBurstLimitA.Size = new Size(57, 20);
      this.txtBurstLimitA.TabIndex = 121;
      this.txtBurstDiffA.Location = new Point(184, 320);
      this.txtBurstDiffA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtBurstDiffA.Name = "txtBurstDiffA";
      this.txtBurstDiffA.Size = new Size(57, 20);
      this.txtBurstDiffA.TabIndex = 119;
      this.txtUndersizeLimitA.Location = new Point(184, 301);
      this.txtUndersizeLimitA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtUndersizeLimitA.Name = "txtUndersizeLimitA";
      this.txtUndersizeLimitA.Size = new Size(57, 20);
      this.txtUndersizeLimitA.TabIndex = 117;
      this.txtUndersizeDiffA.Location = new Point(184, 282);
      this.txtUndersizeDiffA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtUndersizeDiffA.Name = "txtUndersizeDiffA";
      this.txtUndersizeDiffA.Size = new Size(57, 20);
      this.txtUndersizeDiffA.TabIndex = 115;
      this.txtOversizeLimitA.Location = new Point(184, 263);
      this.txtOversizeLimitA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtOversizeLimitA.Name = "txtOversizeLimitA";
      this.txtOversizeLimitA.Size = new Size(57, 20);
      this.txtOversizeLimitA.TabIndex = 113;
      this.txtOversizeDiffA.Location = new Point(184, 244);
      this.txtOversizeDiffA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtOversizeDiffA.Name = "txtOversizeDiffA";
      this.txtOversizeDiffA.Size = new Size(57, 20);
      this.txtOversizeDiffA.TabIndex = 111;
      this.txtPulseLeakUpperA.Location = new Point(184, 225);
      this.txtPulseLeakUpperA.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseLeakUpperA.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtPulseLeakUpperA.Name = "txtPulseLeakUpperA";
      this.txtPulseLeakUpperA.Size = new Size(57, 20);
      this.txtPulseLeakUpperA.TabIndex = 109;
      this.txtPulseLeakLowerA.Location = new Point(184, 206);
      this.txtPulseLeakLowerA.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseLeakLowerA.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtPulseLeakLowerA.Name = "txtPulseLeakLowerA";
      this.txtPulseLeakLowerA.Size = new Size(57, 20);
      this.txtPulseLeakLowerA.TabIndex = 107;
      this.txtPulseUnleakLimitA.Location = new Point(184, 187);
      this.txtPulseUnleakLimitA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseUnleakLimitA.Name = "txtPulseUnleakLimitA";
      this.txtPulseUnleakLimitA.Size = new Size(57, 20);
      this.txtPulseUnleakLimitA.TabIndex = 105;
      this.txtPulseLeakLimitA.Location = new Point(184, 168);
      this.txtPulseLeakLimitA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseLeakLimitA.Name = "txtPulseLeakLimitA";
      this.txtPulseLeakLimitA.Size = new Size(57, 20);
      this.txtPulseLeakLimitA.TabIndex = 103;
      this.txtPulseBlockLimitA.Location = new Point(184, 149);
      this.txtPulseBlockLimitA.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtPulseBlockLimitA.Name = "txtPulseBlockLimitA";
      this.txtPulseBlockLimitA.Size = new Size(57, 20);
      this.txtPulseBlockLimitA.TabIndex = 101;
      this.txtFullSerialnumberA.Location = new Point(140, 15);
      this.txtFullSerialnumberA.Name = "txtFullSerialnumberA";
      this.txtFullSerialnumberA.Size = new Size(102, 20);
      this.txtFullSerialnumberA.TabIndex = 99;
      this.txtFullSerialnumberA.Tag = (object) "";
      this.label42.Location = new Point(5, 34);
      this.label42.Name = "label42";
      this.label42.Size = new Size(129, 15);
      this.label42.TabIndex = 98;
      this.label42.Tag = (object) "cfg_serial_a";
      this.label42.Text = "Serialnumber:";
      this.label42.TextAlign = ContentAlignment.MiddleRight;
      this.txtMBusAddressA.Location = new Point(140, 53);
      this.txtMBusAddressA.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMBusAddressA.Name = "txtMBusAddressA";
      this.txtMBusAddressA.Size = new Size(102, 20);
      this.txtMBusAddressA.TabIndex = 72;
      this.txtMBusGenerationA.Location = new Point(140, 111);
      this.txtMBusGenerationA.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtMBusGenerationA.Name = "txtMBusGenerationA";
      this.txtMBusGenerationA.Size = new Size(102, 20);
      this.txtMBusGenerationA.TabIndex = 76;
      this.txtSerialA.Location = new Point(140, 34);
      this.txtSerialA.Maximum = new Decimal(new int[4]
      {
        -1,
        0,
        0,
        0
      });
      this.txtSerialA.Name = "txtSerialA";
      this.txtSerialA.Size = new Size(102, 20);
      this.txtSerialA.TabIndex = 97;
      this.cboxMBusMediumA.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMBusMediumA.FormattingEnabled = true;
      this.cboxMBusMediumA.Location = new Point(140, 72);
      this.cboxMBusMediumA.Name = "cboxMBusMediumA";
      this.cboxMBusMediumA.Size = new Size(102, 21);
      this.cboxMBusMediumA.TabIndex = 74;
      this.txtObisA.Location = new Point(140, 130);
      this.txtObisA.MaxLength = 1;
      this.txtObisA.Name = "txtObisA";
      this.txtObisA.Size = new Size(102, 20);
      this.txtObisA.TabIndex = 96;
      this.txtObisA.Tag = (object) "";
      this.txtManufacturerA.Location = new Point(140, 92);
      this.txtManufacturerA.MaxLength = 3;
      this.txtManufacturerA.Name = "txtManufacturerA";
      this.txtManufacturerA.Size = new Size(102, 20);
      this.txtManufacturerA.TabIndex = 80;
      this.txtManufacturerA.Tag = (object) "";
      this.label48.Location = new Point(5, 92);
      this.label48.Name = "label48";
      this.label48.Size = new Size(129, 15);
      this.label48.TabIndex = 79;
      this.label48.Tag = (object) "cfg_mbus_manid_a";
      this.label48.Text = "Manufacturer:";
      this.label48.TextAlign = ContentAlignment.MiddleRight;
      this.label49.Location = new Point(5, 130);
      this.label49.Name = "label49";
      this.label49.Size = new Size(129, 15);
      this.label49.TabIndex = 95;
      this.label49.Tag = (object) "cfg_obis_a";
      this.label49.Text = "Obis:";
      this.label49.TextAlign = ContentAlignment.MiddleRight;
      this.label50.Location = new Point(5, 15);
      this.label50.Name = "label50";
      this.label50.Size = new Size(129, 15);
      this.label50.TabIndex = 77;
      this.label50.Tag = (object) "Con_fullserialnumberA";
      this.label50.Text = "Full Serialnumber:";
      this.label50.TextAlign = ContentAlignment.MiddleRight;
      this.label58.Location = new Point(5, 111);
      this.label58.Name = "label58";
      this.label58.Size = new Size(129, 15);
      this.label58.TabIndex = 75;
      this.label58.Tag = (object) "cfg_mbus_version_a";
      this.label58.Text = "M-Bus generation:";
      this.label58.TextAlign = ContentAlignment.MiddleRight;
      this.label60.Location = new Point(5, 72);
      this.label60.Name = "label60";
      this.label60.Size = new Size(129, 15);
      this.label60.TabIndex = 73;
      this.label60.Tag = (object) "cfg_mbus_medium_a";
      this.label60.Text = "M-Bus medium:";
      this.label60.TextAlign = ContentAlignment.MiddleRight;
      this.label61.Location = new Point(5, 53);
      this.label61.Name = "label61";
      this.label61.Size = new Size(129, 15);
      this.label61.TabIndex = 71;
      this.label61.Tag = (object) "cfg_mbus_address_a";
      this.label61.Text = "M-Bus address:";
      this.label61.TextAlign = ContentAlignment.MiddleRight;
      this.groupBox4.Controls.Add((Control) this.txtFullSerialnumberC);
      this.groupBox4.Controls.Add((Control) this.label41);
      this.groupBox4.Controls.Add((Control) this.txtMBusAddressC);
      this.groupBox4.Controls.Add((Control) this.txtMBusGenerationC);
      this.groupBox4.Controls.Add((Control) this.txtSerialC);
      this.groupBox4.Controls.Add((Control) this.cboxMBusMediumC);
      this.groupBox4.Controls.Add((Control) this.txtObisC);
      this.groupBox4.Controls.Add((Control) this.txtManufacturerC);
      this.groupBox4.Controls.Add((Control) this.label10);
      this.groupBox4.Controls.Add((Control) this.label37);
      this.groupBox4.Controls.Add((Control) this.label14);
      this.groupBox4.Controls.Add((Control) this.label15);
      this.groupBox4.Controls.Add((Control) this.label19);
      this.groupBox4.Controls.Add((Control) this.label23);
      this.groupBox4.Location = new Point(4, 1);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(266, 155);
      this.groupBox4.TabIndex = 60;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Main device (C)";
      this.txtFullSerialnumberC.Location = new Point(158, 15);
      this.txtFullSerialnumberC.Name = "txtFullSerialnumberC";
      this.txtFullSerialnumberC.Size = new Size(102, 20);
      this.txtFullSerialnumberC.TabIndex = 99;
      this.txtFullSerialnumberC.Tag = (object) "";
      this.label41.Location = new Point(5, 34);
      this.label41.Name = "label41";
      this.label41.Size = new Size(147, 15);
      this.label41.TabIndex = 98;
      this.label41.Tag = (object) "cfg_serial_c";
      this.label41.Text = "Serialnumber:";
      this.label41.TextAlign = ContentAlignment.MiddleRight;
      this.label38.Location = new Point(18, 577);
      this.label38.Name = "label38";
      this.label38.Size = new Size(140, 15);
      this.label38.TabIndex = 53;
      this.label38.Tag = (object) "pulseReadingB";
      this.label38.Text = "Pulses Input B:";
      this.label38.TextAlign = ContentAlignment.MiddleRight;
      this.txtPulseReadingB.Location = new Point(164, 577);
      this.txtPulseReadingB.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseReadingB.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtPulseReadingB.Name = "txtPulseReadingB";
      this.txtPulseReadingB.ReadOnly = true;
      this.txtPulseReadingB.Size = new Size(106, 20);
      this.txtPulseReadingB.TabIndex = 54;
      this.groupBox8.Controls.Add((Control) this.listHardwareErrors);
      this.groupBox8.Location = new Point(786, 434);
      this.groupBox8.Name = "groupBox8";
      this.groupBox8.Size = new Size(244, 157);
      this.groupBox8.TabIndex = 48;
      this.groupBox8.TabStop = false;
      this.groupBox8.Tag = (object) "hwStatusFlags";
      this.groupBox8.Text = "Hardware errors";
      this.listHardwareErrors.CheckOnClick = true;
      this.listHardwareErrors.FormattingEnabled = true;
      this.listHardwareErrors.Location = new Point(3, 16);
      this.listHardwareErrors.Name = "listHardwareErrors";
      this.listHardwareErrors.Size = new Size(235, 139);
      this.listHardwareErrors.TabIndex = 0;
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
      this.zennerCoroprateDesign1.Size = new Size(1036, 36);
      this.zennerCoroprateDesign1.TabIndex = 20;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1036, 669);
      this.Controls.Add((Control) this.ckboxShowParameterNamesUsedInFirmware);
      this.Controls.Add((Control) this.panel);
      this.Controls.Add((Control) this.label36);
      this.Controls.Add((Control) this.cboxHandlerObject);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConfiguratorPdcRadio);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Configurator PDC wireless M-Bus";
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
      this.txtRadioTimeBias.EndInit();
      this.txtRadioPacketBOffset.EndInit();
      this.txtRadioInstallCount.EndInit();
      this.txtRadioInstallInterval.EndInit();
      this.txtRadioFrequencyOffset.EndInit();
      this.txtRadioTransmitInterval.EndInit();
      this.txtPulseActivateRadio.EndInit();
      this.groupBox3.ResumeLayout(false);
      this.txtPulseOn.EndInit();
      this.txtPulsePeriod.EndInit();
      this.txtTimeZone.EndInit();
      this.txtPulseReadingA.EndInit();
      this.txtSerialC.EndInit();
      this.txtMBusAddressC.EndInit();
      this.txtMBusGenerationC.EndInit();
      this.panel.ResumeLayout(false);
      this.groupBox10.ResumeLayout(false);
      this.groupBox9.ResumeLayout(false);
      this.groupBox7.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox6.ResumeLayout(false);
      this.groupBox6.PerformLayout();
      this.txtScaleExponentB.EndInit();
      this.txtMBusAddressB.EndInit();
      this.txtScaleMantissaB.EndInit();
      this.txtMBusGenerationB.EndInit();
      this.txtVifB.EndInit();
      this.txtSerialB.EndInit();
      this.txtUndersizeLimitB.EndInit();
      this.txtPulseBlockLimitB.EndInit();
      this.txtPulseLeakLimitB.EndInit();
      this.txtBurstLimitB.EndInit();
      this.txtPulseUnleakLimitB.EndInit();
      this.txtBurstDiffB.EndInit();
      this.txtPulseLeakLowerB.EndInit();
      this.txtPulseLeakUpperB.EndInit();
      this.txtUndersizeDiffB.EndInit();
      this.txtOversizeDiffB.EndInit();
      this.txtOversizeLimitB.EndInit();
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.txtScaleExponentA.EndInit();
      this.txtScaleMantissaA.EndInit();
      this.txtVifA.EndInit();
      this.txtBurstLimitA.EndInit();
      this.txtBurstDiffA.EndInit();
      this.txtUndersizeLimitA.EndInit();
      this.txtUndersizeDiffA.EndInit();
      this.txtOversizeLimitA.EndInit();
      this.txtOversizeDiffA.EndInit();
      this.txtPulseLeakUpperA.EndInit();
      this.txtPulseLeakLowerA.EndInit();
      this.txtPulseUnleakLimitA.EndInit();
      this.txtPulseLeakLimitA.EndInit();
      this.txtPulseBlockLimitA.EndInit();
      this.txtMBusAddressA.EndInit();
      this.txtMBusGenerationA.EndInit();
      this.txtSerialA.EndInit();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.txtPulseReadingB.EndInit();
      this.groupBox8.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
