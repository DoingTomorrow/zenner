// Decompiled with JetBrains decompiler
// Type: TH_Handler.ConfiguartionWindow
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using GmmDbLib;
using HandlerLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZR_ClassLibrary;

#nullable disable
namespace TH_Handler
{
  public partial class ConfiguartionWindow : Window, IComponentConnector
  {
    private TH_Meter meter;
    internal Button btnSave;
    internal Button btnCancel;
    internal TextBox cfg_version;
    internal TextBox cfg_serial;
    internal TextBox cfg_tactile_sw_cycle;
    internal TextBox cfg_tactile_sw_install;
    internal TextBox cfg_tactile_sw_removal;
    internal ComboBox cfg_radio_mode;
    internal ComboBox cfg_radio_scenario;
    internal TextBox cfg_radio_install_count;
    internal TextBox cfg_radio_time_bias;
    internal TextBox cfg_radio_normal_basetime;
    internal TextBox cfg_radio_install_basetime;
    internal TextBox cfg_radio_packetb_offset;
    internal ComboBox cfg_radio_power;
    internal TextBox cfg_radio_freq_offset;
    internal TextBox cfg_humidity_threshold;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_SLEEP;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_ENABLE_LCD;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_ENABLE_RADIO;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_ENABLE_LOG;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_ENABLE_SHT2X;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_ENABLE_NTC;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_ENABLE_T;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_ENABLE_RH;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_FAHRENHEIT;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_RADIO_LONGHEADER;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_RADIO_ENCRYPT;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_RADIO_SYNCHRONOUS;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_RADIO_INSTALL;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_RADIO_PACKET_T;
    internal CheckBoxForEnumWithFlagAttribute CONFIG_RADIO_PACKET_RH;
    internal TextBox cfg_sensor_cycle;
    internal TextBox cfg_LCD_test_timing;
    internal ComboBox cfg_LCD_segment_test;
    internal TextBox cfg_LCD_blinking_cycle;
    internal DatePicker cfg_lowbatt;
    internal TextBox Con_MeterId;
    internal TextBox Con_HardwareTypeId;
    internal TextBox Con_MeterInfoId;
    internal TextBox Con_BaseTypeId;
    internal TextBox Con_MeterTypeId;
    internal TextBox Con_SAP_MaterialNumber;
    internal TextBox Con_SAP_ProductionOrderNumber;
    internal TextBox Con_fullserialnumber;
    internal TextBox Con_fullserialnumberA;
    internal TextBox Con_fullserialnumberB;
    internal TextBox Bak_TimeZoneInQuarterHours;
    internal TextBox txtHumidity;
    internal TextBox txtTemperature;
    private bool _contentLoaded;

    private ConfiguartionWindow() => this.InitializeComponent();

    public static TH_Meter ShowDialog(Window owner, TH_Meter meter)
    {
      if (meter == null)
        return (TH_Meter) null;
      try
      {
        ConfiguartionWindow configuartionWindow = new ConfiguartionWindow();
        configuartionWindow.Owner = owner;
        configuartionWindow.meter = meter.DeepCopy();
        configuartionWindow.LoadValues();
        return configuartionWindow.ShowDialog().Value ? configuartionWindow.meter : meter;
      }
      catch (Exception ex)
      {
        string caption = Ot.Gtt(Tg.Handler_UI, "Error", "Error");
        int num = (int) MessageBox.Show(owner, ex.Message, caption, MessageBoxButton.OK, MessageBoxImage.Hand);
        return meter;
      }
    }

    private void LoadValues()
    {
      this.cfg_version.Text = this.meter.GetVersion().ToString();
      this.cfg_serial.Text = this.meter.GetSerial().ToString();
      this.cfg_tactile_sw_cycle.Text = this.meter.GetTactileSwCycle().ToString();
      this.cfg_tactile_sw_install.Text = this.meter.GetTactileSwInstall().ToString();
      this.cfg_tactile_sw_removal.Text = this.meter.GetTactileSwRemoval().ToString();
      this.cfg_radio_mode.SelectedItem = (object) this.meter.GetRadioMode();
      this.cfg_radio_scenario.SelectedItem = (object) this.meter.GetRadioScenario();
      this.cfg_radio_install_count.Text = this.meter.GetRadioInstallCount().ToString();
      this.cfg_radio_time_bias.Text = this.meter.GetRadioTimeBias().ToString();
      this.cfg_radio_normal_basetime.Text = this.meter.GetRadioNormalBasetime().ToString();
      this.cfg_radio_install_basetime.Text = this.meter.GetRadioInstallBasetime().ToString();
      this.cfg_radio_packetb_offset.Text = this.meter.GetRadioPacketBOffset().ToString();
      this.cfg_radio_power.SelectedItem = (object) this.meter.GetRadioPower();
      this.cfg_radio_freq_offset.Text = this.meter.GetRadioFreqOffset().ToString();
      this.cfg_humidity_threshold.Text = this.meter.GetHumidityThreshold().ToString();
      this.cfg_sensor_cycle.Text = this.meter.GetSensorCycle().ToString();
      this.cfg_LCD_test_timing.Text = this.meter.GetLCDTestTiming().ToString();
      this.cfg_LCD_segment_test.SelectedItem = (object) this.meter.GetLCDSegmentTest();
      this.cfg_LCD_blinking_cycle.Text = this.meter.GetLCDBlinkingCycle().ToString();
      this.cfg_lowbatt.SelectedDate = this.meter.GetLowBatt();
      this.txtHumidity.Text = this.meter.GetHumidityValue().ToString() + " %";
      this.txtTemperature.Text = this.meter.GetTemperatureValue().ToString() + " °C";
      ConfigFlags configFlags = this.meter.GetConfigFlags();
      this.CONFIG_SLEEP.SetChecked((Enum) configFlags, (Enum) ConfigFlags.SLEEP);
      this.CONFIG_ENABLE_LCD.SetChecked((Enum) configFlags, (Enum) ConfigFlags.ENABLE_LCD);
      this.CONFIG_ENABLE_RADIO.SetChecked((Enum) configFlags, (Enum) ConfigFlags.ENABLE_RADIO);
      this.CONFIG_ENABLE_LOG.SetChecked((Enum) configFlags, (Enum) ConfigFlags.ENABLE_LOG);
      this.CONFIG_ENABLE_SHT2X.SetChecked((Enum) configFlags, (Enum) ConfigFlags.ENABLE_SHT2X);
      this.CONFIG_ENABLE_NTC.SetChecked((Enum) configFlags, (Enum) ConfigFlags.ENABLE_NTC);
      this.CONFIG_ENABLE_T.SetChecked((Enum) configFlags, (Enum) ConfigFlags.ENABLE_T);
      this.CONFIG_ENABLE_RH.SetChecked((Enum) configFlags, (Enum) ConfigFlags.ENABLE_RH);
      this.CONFIG_FAHRENHEIT.SetChecked((Enum) configFlags, (Enum) ConfigFlags.FAHRENHEIT);
      RadioFlags radioFlags = this.meter.GetRadioFlags();
      this.CONFIG_RADIO_LONGHEADER.SetChecked((Enum) radioFlags, (Enum) RadioFlags.LONGHEADER);
      this.CONFIG_RADIO_ENCRYPT.SetChecked((Enum) radioFlags, (Enum) RadioFlags.ENCRYPT);
      this.CONFIG_RADIO_SYNCHRONOUS.SetChecked((Enum) radioFlags, (Enum) RadioFlags.SYNCHRONOUS);
      this.CONFIG_RADIO_INSTALL.SetChecked((Enum) radioFlags, (Enum) RadioFlags.INSTALL);
      this.CONFIG_RADIO_PACKET_T.SetChecked((Enum) radioFlags, (Enum) RadioFlags.PACKET_T);
      this.CONFIG_RADIO_PACKET_RH.SetChecked((Enum) radioFlags, (Enum) RadioFlags.PACKET_RH);
      TH_DeviceIdentification deviceIdentification = this.meter.GetDeviceIdentification();
      if (deviceIdentification != null)
      {
        this.Con_MeterId.Text = deviceIdentification.MeterID.ToString();
        this.Con_HardwareTypeId.Text = deviceIdentification.HardwareTypeID.ToString();
        this.Con_MeterInfoId.Text = deviceIdentification.MeterInfoID.ToString();
        this.Con_BaseTypeId.Text = deviceIdentification.BaseTypeID.ToString();
        this.Con_MeterTypeId.Text = deviceIdentification.MeterTypeID.ToString();
        this.Con_SAP_MaterialNumber.Text = deviceIdentification.SapMaterialNumber.ToString();
        this.Con_SAP_ProductionOrderNumber.Text = deviceIdentification.SapProductionOrderNumber.ToString();
        this.Con_fullserialnumber.Text = deviceIdentification.Fullserialnumber;
        this.Con_fullserialnumberA.Text = deviceIdentification.FullserialnumberA;
        this.Con_fullserialnumberB.Text = deviceIdentification.FullserialnumberB;
      }
      this.Bak_TimeZoneInQuarterHours.Text = this.meter.GetTimeZone().ToString();
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.meter.SetVersion(Convert.ToByte(this.cfg_version.Text));
        this.meter.SetSerial(Convert.ToUInt32(this.cfg_serial.Text));
        this.meter.SetTactileSwCycle(Convert.ToUInt16(this.cfg_tactile_sw_cycle.Text));
        this.meter.SetTactileSwInstall(Convert.ToUInt16(this.cfg_tactile_sw_install.Text));
        this.meter.SetTactileSwRemoval(Convert.ToUInt16(this.cfg_tactile_sw_removal.Text));
        this.meter.SetRadioInstallCount(Convert.ToByte(this.cfg_radio_install_count.Text));
        this.meter.SetRadioTimeBias(Convert.ToInt16(this.cfg_radio_time_bias.Text));
        this.meter.SetRadioNormalBasetime(Convert.ToUInt16(this.cfg_radio_normal_basetime.Text));
        this.meter.SetRadioInstallBasetime(Convert.ToUInt16(this.cfg_radio_install_basetime.Text));
        this.meter.SetRadioPacketBOffset(Convert.ToUInt16(this.cfg_radio_packetb_offset.Text));
        this.meter.SetRadioFreqOffset(Convert.ToInt16(this.cfg_radio_freq_offset.Text));
        this.meter.SetHumidityThreshold(Convert.ToInt16(this.cfg_humidity_threshold.Text));
        this.meter.SetSensorCycle(Convert.ToUInt16(this.cfg_sensor_cycle.Text));
        this.meter.SetLCDTestTiming(Convert.ToUInt16(this.cfg_LCD_test_timing.Text));
        this.meter.SetLCDBlinkingCycle(Convert.ToByte(this.cfg_LCD_blinking_cycle.Text));
        this.meter.SetLowBatt(this.cfg_lowbatt.SelectedDate);
        if (Util.IsNumeric((object) this.Bak_TimeZoneInQuarterHours.Text))
          this.meter.SetTimeZone(Convert.ToInt32(this.Bak_TimeZoneInQuarterHours.Text));
        else
          this.meter.SetTimeZone(0);
        this.meter.SetRadioMode((RadioMode) Enum.Parse(typeof (RadioMode), this.cfg_radio_mode.SelectedItem.ToString()));
        this.meter.SetRadioScenario((RadioScenario) Enum.Parse(typeof (RadioScenario), this.cfg_radio_scenario.SelectedItem.ToString()));
        this.meter.SetRadioPower((RadioPower) Enum.Parse(typeof (RadioPower), this.cfg_radio_power.SelectedItem.ToString()));
        this.meter.SetLCDSegmentTest((LcdTest) Enum.Parse(typeof (LcdTest), this.cfg_LCD_segment_test.SelectedItem.ToString()));
        this.meter.SetConfigFlags((ConfigFlags) this.CONFIG_SLEEP.EnumValue | (ConfigFlags) this.CONFIG_ENABLE_LCD.EnumValue | (ConfigFlags) this.CONFIG_ENABLE_RADIO.EnumValue | (ConfigFlags) this.CONFIG_ENABLE_LOG.EnumValue | (ConfigFlags) this.CONFIG_ENABLE_SHT2X.EnumValue | (ConfigFlags) this.CONFIG_ENABLE_NTC.EnumValue | (ConfigFlags) this.CONFIG_ENABLE_T.EnumValue | (ConfigFlags) this.CONFIG_ENABLE_RH.EnumValue | (ConfigFlags) this.CONFIG_FAHRENHEIT.EnumValue);
        this.meter.SetRadioFlags((RadioFlags) this.CONFIG_RADIO_LONGHEADER.EnumValue | (RadioFlags) this.CONFIG_RADIO_ENCRYPT.EnumValue | (RadioFlags) this.CONFIG_RADIO_SYNCHRONOUS.EnumValue | (RadioFlags) this.CONFIG_RADIO_INSTALL.EnumValue | (RadioFlags) this.CONFIG_RADIO_PACKET_T.EnumValue | (RadioFlags) this.CONFIG_RADIO_PACKET_RH.EnumValue);
        this.meter.SetDeviceIdentification(TH_DeviceIdentification.Parse(this.Con_MeterId.Text, this.Con_HardwareTypeId.Text, this.Con_MeterInfoId.Text, this.Con_BaseTypeId.Text, this.Con_MeterTypeId.Text, this.Con_SAP_MaterialNumber.Text, this.Con_SAP_ProductionOrderNumber.Text, this.Con_fullserialnumber.Text, this.Con_fullserialnumberA.Text, this.Con_fullserialnumberB.Text));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, Ot.Gtt(Tg.Handler_UI, "Error", "Error"), MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      this.DialogResult = new bool?(true);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = new bool?(false);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/TH_Handler;component/configuartionwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.btnSave = (Button) target;
          this.btnSave.Click += new RoutedEventHandler(this.btnSave_Click);
          break;
        case 2:
          this.btnCancel = (Button) target;
          this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
          break;
        case 3:
          this.cfg_version = (TextBox) target;
          break;
        case 4:
          this.cfg_serial = (TextBox) target;
          break;
        case 5:
          this.cfg_tactile_sw_cycle = (TextBox) target;
          break;
        case 6:
          this.cfg_tactile_sw_install = (TextBox) target;
          break;
        case 7:
          this.cfg_tactile_sw_removal = (TextBox) target;
          break;
        case 8:
          this.cfg_radio_mode = (ComboBox) target;
          break;
        case 9:
          this.cfg_radio_scenario = (ComboBox) target;
          break;
        case 10:
          this.cfg_radio_install_count = (TextBox) target;
          break;
        case 11:
          this.cfg_radio_time_bias = (TextBox) target;
          break;
        case 12:
          this.cfg_radio_normal_basetime = (TextBox) target;
          break;
        case 13:
          this.cfg_radio_install_basetime = (TextBox) target;
          break;
        case 14:
          this.cfg_radio_packetb_offset = (TextBox) target;
          break;
        case 15:
          this.cfg_radio_power = (ComboBox) target;
          break;
        case 16:
          this.cfg_radio_freq_offset = (TextBox) target;
          break;
        case 17:
          this.cfg_humidity_threshold = (TextBox) target;
          break;
        case 18:
          this.CONFIG_SLEEP = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 19:
          this.CONFIG_ENABLE_LCD = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 20:
          this.CONFIG_ENABLE_RADIO = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 21:
          this.CONFIG_ENABLE_LOG = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 22:
          this.CONFIG_ENABLE_SHT2X = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 23:
          this.CONFIG_ENABLE_NTC = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 24:
          this.CONFIG_ENABLE_T = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 25:
          this.CONFIG_ENABLE_RH = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 26:
          this.CONFIG_FAHRENHEIT = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 27:
          this.CONFIG_RADIO_LONGHEADER = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 28:
          this.CONFIG_RADIO_ENCRYPT = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 29:
          this.CONFIG_RADIO_SYNCHRONOUS = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 30:
          this.CONFIG_RADIO_INSTALL = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 31:
          this.CONFIG_RADIO_PACKET_T = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 32:
          this.CONFIG_RADIO_PACKET_RH = (CheckBoxForEnumWithFlagAttribute) target;
          break;
        case 33:
          this.cfg_sensor_cycle = (TextBox) target;
          break;
        case 34:
          this.cfg_LCD_test_timing = (TextBox) target;
          break;
        case 35:
          this.cfg_LCD_segment_test = (ComboBox) target;
          break;
        case 36:
          this.cfg_LCD_blinking_cycle = (TextBox) target;
          break;
        case 37:
          this.cfg_lowbatt = (DatePicker) target;
          break;
        case 38:
          this.Con_MeterId = (TextBox) target;
          break;
        case 39:
          this.Con_HardwareTypeId = (TextBox) target;
          break;
        case 40:
          this.Con_MeterInfoId = (TextBox) target;
          break;
        case 41:
          this.Con_BaseTypeId = (TextBox) target;
          break;
        case 42:
          this.Con_MeterTypeId = (TextBox) target;
          break;
        case 43:
          this.Con_SAP_MaterialNumber = (TextBox) target;
          break;
        case 44:
          this.Con_SAP_ProductionOrderNumber = (TextBox) target;
          break;
        case 45:
          this.Con_fullserialnumber = (TextBox) target;
          break;
        case 46:
          this.Con_fullserialnumberA = (TextBox) target;
          break;
        case 47:
          this.Con_fullserialnumberB = (TextBox) target;
          break;
        case 48:
          this.Bak_TimeZoneInQuarterHours = (TextBox) target;
          break;
        case 49:
          this.txtHumidity = (TextBox) target;
          break;
        case 50:
          this.txtTemperature = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
