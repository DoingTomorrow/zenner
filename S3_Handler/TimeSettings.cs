// Decompiled with JetBrains decompiler
// Type: S3_Handler.TimeSettings
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class TimeSettings : Form
  {
    private const string ClassMessageName = "S3_Handler TimeSettings";
    private S3_HandlerFunctions MyFunctions;
    private Color readBackColor = Color.LightGreen;
    private Color changedBackColor;
    private Decimal VolumeCycleTimeInit;
    private Decimal EnergyCycleTimeInit;
    private Decimal SlowEnergyCycleTimeInit;
    private Decimal FastTimeoutTimeInit;
    private Decimal RadioCycleTimeInit;
    private Decimal VolumeCycleTime;
    private Decimal EnergyCycleTime;
    private Decimal SlowEnergyCycleTime;
    private Decimal FastTimeoutTime;
    private Decimal RadioCycleTime;
    private bool meterSettingsUnchanged;
    private StringBuilder messageString = new StringBuilder();
    private int volumeCycleTimeCounter;
    private int temperaturCycleTimeCounter;
    private int temperaturCycleTimeSlotCounter;
    private int fastCyleOffCounter;
    private int radioCycleTimeCounter;
    private int volumeCycleReduceCounter;
    private uint radioNextTime4096;
    private int radioOffsetTime4096;
    private uint Log_NextLoggerTime;
    private uint Bak_TimeBaseSecounds;
    private uint dbPassword;
    private bool dbPasswordIsDefined;
    private uint MeterKey;
    private uint password;
    private TimeSpan connectedMeterTimeDiff = TimeSpan.MaxValue;
    private string oldLoopText;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Label label2;
    private Label label1;
    private Label label4;
    private NumericUpDown numericUpDownVolumeCycle;
    private Label label3;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private NumericUpDown numericUpDownEnergyCycle;
    private Label label9;
    private Label label10;
    private Label label11;
    private TextBox textBoxVolumeCycleTime;
    private Label label12;
    private Label label13;
    private TextBox textBoxEnergyCycleTime;
    private Label label14;
    private Label label15;
    private Label label16;
    private Label label17;
    private NumericUpDown numericUpDownRadioCycle;
    private Label label18;
    private TextBox textBoxRadioCycleTimr;
    private Label label19;
    private Label label20;
    private Label label21;
    private Label label22;
    private NumericUpDown numericUpDownSlowEnergyCycle;
    private Label label23;
    private TextBox textBoxSlowEnergyCycleTime;
    private Label label24;
    private Label label25;
    private Label label26;
    private Label label27;
    private NumericUpDown numericUpDownFastOffTime;
    private Label label28;
    private TextBox textBoxFastOffTime;
    private Button buttonShowState;
    private TextBox textBoxState;
    private Label label29;
    private GroupBox groupBoxTimingSetup;
    private Button buttonShowStateLoop;
    private Button btnSet;
    private Button buttonFast;
    private Button buttonMechanic;
    private Button buttonUltrasonic;
    private GroupBox groupBoxDateAndTime;
    private Button buttonSetTimeImmediately;
    private Button buttonReadTimeAgain;
    private CheckBox checkBoxUsePcTime;
    private TextBox textBoxTimeZone;
    private Label label30;
    private DateTimePicker dateTimePickerTime;
    private DateTimePicker dateTimePickerDate;
    private Timer timer1;
    private GroupBox groupBoxWritePermission;
    private Button buttonManageMeterKey;
    private CheckBox checkBoxConnectedDeviceIsLocked;
    private Button buttonLockDevice;
    private Label label31;
    private TextBox textBoxMeterKey;
    private CheckBox checkBoxMeterKeyIsDefined;
    private Button buttonDateAndTimeSet;
    private TextBox textBoxUtcTime;
    private TextBox textBoxPcTime;
    private Label label34;
    private Label label33;
    private Label label32;
    private GroupBox groupBoxConnectedMeterTime;
    private TextBox textBoxTimeOfConnectedMeter;
    private TextBox textBoxPassword;
    private Button buttonManagePassword;
    private Label label35;
    private Button buttonManageDbPassword;

    public TimeSettings(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      S3_Meter activeMeter = MyFunctions.MyMeters.GetActiveMeter();
      this.InitializeComponent();
      this.changedBackColor = this.numericUpDownVolumeCycle.BackColor;
      this.checkBoxUsePcTime.Checked = this.MyFunctions._usePcTime;
      this.dbPassword = (uint) UserRights.GlobalUserRights.GetUserKeyChecksum("ZelsiusLockKey");
      this.dbPasswordIsDefined = this.dbPassword > 0U;
      if (activeMeter != null)
      {
        int index1 = activeMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.volumeCycleTimeCounterInit.ToString());
        if (index1 >= 0)
        {
          this.VolumeCycleTimeInit = (Decimal) activeMeter.MyParameters.ParameterByName.Values[index1].GetByteValue();
          this.numericUpDownVolumeCycle.Value = this.VolumeCycleTimeInit;
        }
        int index2 = activeMeter.MyParameters.ParameterByName.IndexOfKey("temperaturCycleTimeCounterInit");
        if (index2 >= 0)
        {
          this.EnergyCycleTimeInit = (Decimal) activeMeter.MyParameters.ParameterByName.Values[index2].GetByteValue();
          this.numericUpDownEnergyCycle.Value = this.EnergyCycleTimeInit;
        }
        int index3 = activeMeter.MyParameters.ParameterByName.IndexOfKey("temperaturCycleTimeSlotCounterInit");
        if (index3 >= 0)
        {
          this.SlowEnergyCycleTimeInit = (Decimal) activeMeter.MyParameters.ParameterByName.Values[index3].GetByteValue();
          this.numericUpDownSlowEnergyCycle.Value = this.SlowEnergyCycleTimeInit;
        }
        int index4 = activeMeter.MyParameters.ParameterByName.IndexOfKey("fastCycleOffCounterInit");
        if (index4 >= 0)
        {
          this.FastTimeoutTimeInit = (Decimal) activeMeter.MyParameters.ParameterByName.Values[index4].GetByteValue();
          this.numericUpDownFastOffTime.Value = this.FastTimeoutTimeInit;
        }
        int index5 = activeMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.radioCycleTimeCounterInit.ToString());
        if (index5 >= 0)
        {
          this.RadioCycleTimeInit = (Decimal) activeMeter.MyParameters.ParameterByName.Values[index5].GetUshortValue();
          this.numericUpDownRadioCycle.Value = this.RadioCycleTimeInit;
        }
        this.SetCycleTimes();
        this.ShowMeterTime();
      }
      if (this.MyFunctions.MyMeters.ConnectedMeter != null)
      {
        this.groupBoxWritePermission.Enabled = true;
        this.keyWorkEnd();
      }
      else
        this.groupBoxWritePermission.Enabled = false;
      this.RefreshConnetedDeviceTime();
    }

    private void SetCycleTimes()
    {
      this.meterSettingsUnchanged = true;
      if (this.numericUpDownVolumeCycle.Value == this.VolumeCycleTimeInit)
      {
        this.numericUpDownVolumeCycle.BackColor = this.readBackColor;
      }
      else
      {
        this.numericUpDownVolumeCycle.BackColor = this.changedBackColor;
        this.meterSettingsUnchanged = false;
      }
      this.VolumeCycleTime = this.numericUpDownVolumeCycle.Value * 0.5M;
      this.textBoxVolumeCycleTime.Text = this.VolumeCycleTime.ToString();
      if (this.numericUpDownEnergyCycle.Value == this.EnergyCycleTimeInit)
      {
        this.numericUpDownEnergyCycle.BackColor = this.readBackColor;
      }
      else
      {
        this.numericUpDownEnergyCycle.BackColor = this.changedBackColor;
        this.meterSettingsUnchanged = false;
      }
      this.EnergyCycleTime = this.numericUpDownEnergyCycle.Value * this.VolumeCycleTime;
      this.textBoxEnergyCycleTime.Text = this.EnergyCycleTime.ToString();
      if (this.numericUpDownSlowEnergyCycle.Value == this.SlowEnergyCycleTimeInit)
      {
        this.numericUpDownSlowEnergyCycle.BackColor = this.readBackColor;
      }
      else
      {
        this.numericUpDownSlowEnergyCycle.BackColor = this.changedBackColor;
        this.meterSettingsUnchanged = false;
      }
      this.SlowEnergyCycleTime = this.numericUpDownSlowEnergyCycle.Value * this.EnergyCycleTime;
      this.textBoxSlowEnergyCycleTime.Text = this.SlowEnergyCycleTime.ToString();
      if (this.numericUpDownFastOffTime.Value == this.FastTimeoutTimeInit)
      {
        this.numericUpDownFastOffTime.BackColor = this.readBackColor;
      }
      else
      {
        this.numericUpDownFastOffTime.BackColor = this.changedBackColor;
        this.meterSettingsUnchanged = false;
      }
      this.FastTimeoutTime = this.numericUpDownFastOffTime.Value * this.EnergyCycleTime;
      this.textBoxFastOffTime.Text = this.FastTimeoutTime.ToString();
      if (this.numericUpDownRadioCycle.Value == this.RadioCycleTimeInit)
      {
        this.numericUpDownRadioCycle.BackColor = this.readBackColor;
      }
      else
      {
        this.numericUpDownRadioCycle.BackColor = this.changedBackColor;
        this.meterSettingsUnchanged = false;
      }
      this.RadioCycleTime = this.numericUpDownRadioCycle.Value * this.VolumeCycleTime;
      this.textBoxRadioCycleTimr.Text = this.RadioCycleTime.ToString();
      if (this.meterSettingsUnchanged)
      {
        this.buttonShowState.Enabled = true;
        this.buttonShowStateLoop.Enabled = true;
      }
      else
      {
        this.buttonShowState.Enabled = false;
        this.buttonShowStateLoop.Enabled = false;
      }
    }

    private void Cycle_ValueChanged(object sender, EventArgs e) => this.SetCycleTimes();

    private void buttonShowState_Click(object sender, EventArgs e) => this.ShowState();

    private void ShowState()
    {
      if (!this.MyFunctions.RefreshDynamicRamData())
      {
        this.textBoxState.Text = "Read error";
      }
      else
      {
        this.messageString.Length = 0;
        S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
        this.messageString.AppendLine("**** Counter values ****");
        int index1 = workMeter.MyParameters.ParameterByName.IndexOfKey("volumeCycleTimeCounter");
        if (index1 >= 0)
        {
          this.volumeCycleTimeCounter = (int) workMeter.MyParameters.ParameterByName.Values[index1].GetByteValue();
          this.messageString.AppendLine("volumeCycleTimeCounter = " + this.volumeCycleTimeCounter.ToString());
        }
        int index2 = workMeter.MyParameters.ParameterByName.IndexOfKey("temperaturCycleTimeCounter");
        if (index2 >= 0)
        {
          this.temperaturCycleTimeCounter = (int) workMeter.MyParameters.ParameterByName.Values[index2].GetByteValue();
          this.messageString.AppendLine("temperaturCycleTimeCounter = " + this.temperaturCycleTimeCounter.ToString());
        }
        int index3 = workMeter.MyParameters.ParameterByName.IndexOfKey("temperaturCycleTimeSlotCounter");
        if (index3 >= 0)
        {
          this.temperaturCycleTimeSlotCounter = (int) workMeter.MyParameters.ParameterByName.Values[index3].GetByteValue();
          this.messageString.AppendLine("temperaturCycleTimeSlotCounter = " + this.temperaturCycleTimeSlotCounter.ToString());
        }
        int index4 = workMeter.MyParameters.ParameterByName.IndexOfKey("fastCyleOffCounter");
        if (index4 >= 0)
        {
          this.fastCyleOffCounter = (int) workMeter.MyParameters.ParameterByName.Values[index4].GetByteValue();
          this.messageString.AppendLine("fastCyleOffCounter = " + this.fastCyleOffCounter.ToString());
        }
        int index5 = workMeter.MyParameters.ParameterByName.IndexOfKey("radioCycleTimeCounter");
        if (index5 >= 0)
        {
          this.radioCycleTimeCounter = (int) workMeter.MyParameters.ParameterByName.Values[index5].GetByteValue();
          this.messageString.AppendLine("radioCycleTimeCounter = " + this.radioCycleTimeCounter.ToString());
        }
        int index6 = workMeter.MyParameters.ParameterByName.IndexOfKey("volumeCycleReduceCounter");
        if (index6 >= 0)
        {
          this.volumeCycleReduceCounter = (int) workMeter.MyParameters.ParameterByName.Values[index6].GetByteValue();
          this.messageString.AppendLine("volumeCycleReduceCounter = " + this.volumeCycleReduceCounter.ToString());
        }
        int index7 = workMeter.MyParameters.ParameterByName.IndexOfKey("radioNextTime4096");
        if (index7 >= 0)
        {
          this.radioNextTime4096 = workMeter.MyParameters.ParameterByName.Values[index7].GetUintValue();
          this.messageString.AppendLine("radioNextTime4096 = " + this.radioNextTime4096.ToString());
        }
        int index8 = workMeter.MyParameters.ParameterByName.IndexOfKey("radioOffsetTime4096");
        if (index8 >= 0)
        {
          this.radioOffsetTime4096 = (int) workMeter.MyParameters.ParameterByName.Values[index8].GetByteValue();
          this.messageString.AppendLine("radioOffsetTime4096 = " + this.radioOffsetTime4096.ToString());
        }
        this.messageString.AppendLine();
        int index9 = workMeter.MyParameters.ParameterByName.IndexOfKey("Bak_TimeBaseSecounds");
        DateTime dateTime1;
        if (index9 >= 0)
        {
          this.Bak_TimeBaseSecounds = workMeter.MyParameters.ParameterByName.Values[index9].GetUintValue();
          StringBuilder messageString = this.messageString;
          dateTime1 = ZR_Calendar.Cal_GetDateTime(this.Bak_TimeBaseSecounds);
          string str = "Bak_TimeBaseSecounds = " + dateTime1.ToString("dd.MM.yyyy HH:mm:ss");
          messageString.AppendLine(str);
        }
        int index10 = workMeter.MyParameters.ParameterByName.IndexOfKey("Log_NextLoggerTime");
        if (index10 >= 0)
        {
          this.Log_NextLoggerTime = workMeter.MyParameters.ParameterByName.Values[index10].GetUintValue();
          StringBuilder messageString = this.messageString;
          dateTime1 = ZR_Calendar.Cal_GetDateTime(this.Log_NextLoggerTime);
          string str = "Log_NextLoggerTime = " + dateTime1.ToString("dd.MM.yyyy HH:mm:ss");
          messageString.AppendLine(str);
        }
        this.messageString.AppendLine();
        this.messageString.AppendLine("**** Actual cycle times ****");
        this.messageString.AppendLine("Volume cycle = " + this.VolumeCycleTime.ToString());
        Decimal num1 = this.EnergyCycleTime;
        if (this.fastCyleOffCounter <= 1)
          num1 = this.SlowEnergyCycleTime;
        this.messageString.AppendLine("Energy cycle = " + num1.ToString());
        this.messageString.AppendLine("Radio cycle = " + this.RadioCycleTime.ToString());
        this.messageString.AppendLine();
        this.messageString.AppendLine("**** Next event times ****");
        Decimal num2 = (Decimal) this.volumeCycleTimeCounter * 0.5M;
        this.messageString.AppendLine("Volume calulation = " + num2.ToString());
        Decimal num3 = num2;
        if (this.temperaturCycleTimeCounter > 1)
          num3 += this.VolumeCycleTime * (Decimal) (this.temperaturCycleTimeCounter - 1);
        if (this.fastCyleOffCounter <= 1)
          num3 += this.EnergyCycleTime * (Decimal) (this.temperaturCycleTimeSlotCounter - 1);
        this.messageString.AppendLine("Energy calulation = " + num3.ToString());
        this.messageString.AppendLine("Radio transmition = ");
        this.messageString.AppendLine();
        Decimal num4 = 0M;
        if (this.fastCyleOffCounter > 1)
          num4 = (Decimal) (this.fastCyleOffCounter - 1) * this.EnergyCycleTime - (this.EnergyCycleTime - num3);
        this.messageString.AppendLine("Time to slow energy cycles = " + num4.ToString());
        DateTime dateTime2 = ZR_Calendar.Cal_GetDateTime(this.Bak_TimeBaseSecounds);
        this.messageString.AppendLine("Next logger event = " + ZR_Calendar.Cal_GetDateTime(this.Log_NextLoggerTime).Subtract(dateTime2).TotalSeconds.ToString());
        this.textBoxState.Text = this.messageString.ToString();
      }
    }

    private void buttonShowStateLoop_Click(object sender, EventArgs e)
    {
      if (this.oldLoopText != null)
      {
        this.buttonShowStateLoop.Text = this.oldLoopText;
        this.buttonShowStateLoop.Enabled = false;
        this.oldLoopText = (string) null;
      }
      else
      {
        this.buttonShowState.Enabled = false;
        this.oldLoopText = this.buttonShowStateLoop.Text;
        this.buttonShowStateLoop.Text = "Break loop";
        while (this.oldLoopText != null)
        {
          this.ShowState();
          Application.DoEvents();
        }
        this.buttonShowState.Enabled = true;
        this.buttonShowStateLoop.Enabled = true;
      }
    }

    private void btnSet_Click(object sender, EventArgs e)
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      int index1 = workMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.volumeCycleTimeCounterInit.ToString());
      if (index1 >= 0)
        workMeter.MyParameters.ParameterByName.Values[index1].SetByteValue(Convert.ToByte(this.numericUpDownVolumeCycle.Value));
      int index2 = workMeter.MyParameters.ParameterByName.IndexOfKey("temperaturCycleTimeCounterInit");
      if (index2 >= 0)
        workMeter.MyParameters.ParameterByName.Values[index2].SetByteValue(Convert.ToByte(this.numericUpDownEnergyCycle.Value));
      int index3 = workMeter.MyParameters.ParameterByName.IndexOfKey("temperaturCycleTimeSlotCounterInit");
      if (index3 >= 0)
        workMeter.MyParameters.ParameterByName.Values[index3].SetByteValue(Convert.ToByte(this.numericUpDownSlowEnergyCycle.Value));
      int index4 = workMeter.MyParameters.ParameterByName.IndexOfKey("fastCycleOffCounterInit");
      if (index4 >= 0)
        workMeter.MyParameters.ParameterByName.Values[index4].SetByteValue(Convert.ToByte(this.numericUpDownFastOffTime.Value));
      int index5 = workMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.radioCycleTimeCounterInit.ToString());
      if (index5 < 0)
        return;
      workMeter.MyParameters.ParameterByName.Values[index5].SetUshortValue(Convert.ToUInt16(this.numericUpDownRadioCycle.Value));
    }

    private void buttonUltrasonic_Click(object sender, EventArgs e)
    {
      this.numericUpDownVolumeCycle.Value = 8M;
      this.numericUpDownEnergyCycle.Value = 2M;
      this.numericUpDownSlowEnergyCycle.Value = 4M;
      this.numericUpDownFastOffTime.Value = 8M;
    }

    private void buttonMechanic_Click(object sender, EventArgs e)
    {
      this.numericUpDownVolumeCycle.Value = 20M;
      this.numericUpDownEnergyCycle.Value = 1M;
      this.numericUpDownSlowEnergyCycle.Value = 3M;
      this.numericUpDownFastOffTime.Value = 6M;
    }

    private void buttonFast_Click(object sender, EventArgs e)
    {
      this.numericUpDownVolumeCycle.Value = 4M;
      this.numericUpDownEnergyCycle.Value = 1M;
      this.numericUpDownSlowEnergyCycle.Value = 1M;
      this.numericUpDownFastOffTime.Value = 5M;
    }

    private void buttonReadTimeAgain_Click(object sender, EventArgs e)
    {
      this.RefreshConnetedDeviceTime();
    }

    private void RefreshConnetedDeviceTime()
    {
      this.connectedMeterTimeDiff = TimeSpan.MaxValue;
      if (this.MyFunctions.MyMeters.ConnectedMeter != null)
      {
        S3_Parameter s3Parameter = this.MyFunctions.MyMeters.ConnectedMeter.MyParameters.ParameterByName["Bak_TimeBaseSecounds"];
        if (s3Parameter.ReadParameterFromConnectedDevice())
          this.connectedMeterTimeDiff = DateTime.Now.Subtract(ZR_Calendar.Cal_GetDateTime(s3Parameter.GetUintValue()));
      }
      this.ViewConnectedMeterTime();
      ZR_ClassLibMessages.ClearErrors();
    }

    private void ViewConnectedMeterTime()
    {
      if (this.connectedMeterTimeDiff == TimeSpan.MaxValue)
        this.textBoxTimeOfConnectedMeter.Clear();
      else
        this.textBoxTimeOfConnectedMeter.Text = DateTime.Now.Subtract(this.connectedMeterTimeDiff).ToString("dd.MM.yyyy HH:mm:ss");
    }

    private void buttonSetTimeImmediately_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxState.Clear();
      if (!this.MyFunctions.MyMeters.NewWorkMeter("change time"))
        return;
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      DateTime date = this.dateTimePickerDate.Value;
      date = date.Date;
      uint meterTime = ZR_Calendar.Cal_GetMeterTime(date.Add(this.dateTimePickerTime.Value.TimeOfDay));
      S3_Parameter s3Parameter1 = workMeter.MyParameters.ParameterByName["Bak_TimeBaseSecounds"];
      s3Parameter1.SetUintValue(meterTime);
      if (s3Parameter1.WriteParameterToConnectedDevice())
      {
        double result;
        if (!double.TryParse(this.textBoxTimeZone.Text, out result))
        {
          this.textBoxTimeZone.Text = "???";
          return;
        }
        int num = (int) (result * 4.0);
        S3_Parameter s3Parameter2 = workMeter.MyParameters.ParameterByName["Bak_TimeZoneInQuarterHours"];
        s3Parameter2.SetByteValue((byte) (num & (int) byte.MaxValue));
        if (s3Parameter2.WriteParameterToConnectedDevice())
          this.textBoxState.Text = "Date and time written to connected device";
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.RefreshConnetedDeviceTime();
    }

    private void buttonDateAndTimeSet_Click(object sender, EventArgs e)
    {
      DateTime date = this.dateTimePickerDate.Value;
      date = date.Date;
      DateTime dateTime = date.Add(this.dateTimePickerTime.Value.TimeOfDay);
      if (this.MyFunctions.SetConfigurationParameter(new SortedList<OverrideID, ConfigurationParameter>()
      {
        {
          OverrideID.DeviceClock,
          new ConfigurationParameter(OverrideID.DeviceClock)
          {
            ParameterValue = (object) dateTime
          }
        }
      }))
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("S3_Handler", "Set time done");
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage("S3_Handler", "Set time error", true);
      }
    }

    private void ShowMeterTime()
    {
      S3_Meter activeMeter = this.MyFunctions.MyMeters.GetActiveMeter();
      if (activeMeter == null)
      {
        this.dateTimePickerDate.Value = this.dateTimePickerDate.MinDate;
        this.dateTimePickerTime.Value = this.dateTimePickerDate.MinDate;
        this.textBoxTimeZone.Clear();
      }
      else
      {
        uint uintValue = activeMeter.MyParameters.ParameterByName["Bak_TimeBaseSecounds"].GetUintValue();
        this.dateTimePickerDate.Value = ZR_Calendar.Cal_GetDateTime(uintValue);
        this.dateTimePickerTime.Value = ZR_Calendar.Cal_GetDateTime(uintValue);
        this.textBoxTimeZone.Text = ((double) activeMeter.MyParameters.ParameterByName["Bak_TimeZoneInQuarterHours"].GetFromSignedByteValue() / 4.0).ToString();
      }
    }

    private void checkBoxUsePcTime_CheckedChanged(object sender, EventArgs e)
    {
      this.MyFunctions._usePcTime = this.checkBoxUsePcTime.Checked;
      this.dateTimePickerDate.Enabled = !this.MyFunctions._usePcTime;
      this.dateTimePickerTime.Enabled = !this.MyFunctions._usePcTime;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      this.ViewConnectedMeterTime();
      DateTime now = DateTime.Now;
      this.textBoxPcTime.Text = now.ToString("dd.MM.yyyy HH.mm.ss");
      DateTime universalTime = now.ToUniversalTime();
      this.textBoxUtcTime.Text = universalTime.ToString("dd.MM.yyyy HH.mm.ss");
      if (!this.MyFunctions._usePcTime)
        return;
      double result;
      if (!double.TryParse(this.textBoxTimeZone.Text, out result))
      {
        this.textBoxTimeZone.Text = "???";
      }
      else
      {
        DateTime dateTime = universalTime.AddHours(result);
        this.dateTimePickerDate.Value = dateTime;
        this.dateTimePickerTime.Value = dateTime;
      }
    }

    private void buttonLockDevice_Click(object sender, EventArgs e)
    {
      this.keyWorkStart();
      if (this.MyFunctions.MyCommands.DeviceProtectionSet())
        this.textBoxState.Text = "Protection set ok";
      else
        this.textBoxState.Text = "Protection set error";
      this.keyWorkEnd();
    }

    private void buttonManageMeterKey_Click(object sender, EventArgs e)
    {
      this.keyWorkStart();
      if (!this.checkBoxConnectedDeviceIsLocked.Checked)
      {
        if (!this.checkBoxMeterKeyIsDefined.Checked)
        {
          this.textBoxState.Text = "Set meter key: ";
          if (this.MyFunctions.MyCommands.DeviceProtectionSetKey(this.MeterKey))
            this.textBoxState.AppendText("ok");
          else
            this.textBoxState.AppendText("error");
        }
        else
        {
          this.textBoxState.AppendText(Environment.NewLine);
          this.textBoxState.AppendText("Illegal button enable state");
        }
      }
      else
      {
        this.textBoxState.Text = "Anlock device by meter key: ";
        if (this.MyFunctions.MyCommands.DeviceProtectionReset(this.MeterKey))
          this.textBoxState.AppendText("ok");
        else
          this.textBoxState.AppendText("error");
      }
      this.keyWorkEnd();
    }

    private void buttonManagePassword_Click(object sender, EventArgs e)
    {
      this.keyWorkStart();
      if (!this.checkBoxConnectedDeviceIsLocked.Checked)
      {
        if (!this.checkBoxMeterKeyIsDefined.Checked)
        {
          this.textBoxState.Text = "Set meter key from password: ";
          if (this.MyFunctions.SetMeterKey(this.password))
            this.textBoxState.AppendText("ok");
          else
            this.textBoxState.AppendText("error");
        }
        else
        {
          this.textBoxState.AppendText(Environment.NewLine);
          this.textBoxState.AppendText("Illegal button enable state");
        }
      }
      else
      {
        this.textBoxState.Text = "Anlock device by password: ";
        if (this.MyFunctions.ClearWriteProtection(this.password))
          this.textBoxState.AppendText("ok");
        else
          this.textBoxState.AppendText("error");
      }
      this.keyWorkEnd();
    }

    private void buttonManageDbPassword_Click(object sender, EventArgs e)
    {
      this.keyWorkStart();
      if (!this.checkBoxConnectedDeviceIsLocked.Checked)
      {
        if (!this.checkBoxMeterKeyIsDefined.Checked)
        {
          this.textBoxState.Text = "Set meter key from db password: ";
          if (this.MyFunctions.SetMeterKey(this.dbPassword))
            this.textBoxState.AppendText("ok");
          else
            this.textBoxState.AppendText("error");
        }
        else
        {
          this.textBoxState.AppendText(Environment.NewLine);
          this.textBoxState.AppendText("Illegal button enable state");
        }
      }
      else
      {
        this.textBoxState.Text = "Anlock device by db password: ";
        if (this.MyFunctions.ClearWriteProtection(this.dbPassword))
          this.textBoxState.AppendText("ok");
        else
          this.textBoxState.AppendText("error");
      }
      this.keyWorkEnd();
    }

    private void textBox_TextChanged(object sender, EventArgs e) => this.SetMeterKeyState();

    private void SetMeterKeyState()
    {
      this.MeterKey = uint.MaxValue;
      bool flag1 = uint.TryParse(this.textBoxMeterKey.Text, out this.MeterKey);
      this.password = uint.MaxValue;
      bool flag2 = uint.TryParse(this.textBoxPassword.Text, out this.password);
      if (this.checkBoxConnectedDeviceIsLocked.Checked)
      {
        this.buttonLockDevice.Enabled = false;
        this.buttonManageMeterKey.Text = "Unlock device by key";
        this.buttonManageMeterKey.Enabled = flag1;
        this.buttonManagePassword.Text = "Unlock by password";
        this.buttonManagePassword.Enabled = flag2;
        this.buttonManageDbPassword.Text = "Unlock by db password";
        this.buttonManageDbPassword.Enabled = this.dbPasswordIsDefined;
      }
      else
      {
        this.buttonLockDevice.Enabled = this.checkBoxMeterKeyIsDefined.Checked;
        this.buttonManageMeterKey.Text = "Set meter key";
        this.buttonManageMeterKey.Enabled = flag1 && !this.checkBoxMeterKeyIsDefined.Checked;
        this.buttonManagePassword.Text = "Set key by password";
        this.buttonManagePassword.Enabled = flag2 && !this.checkBoxMeterKeyIsDefined.Checked;
        this.buttonManageDbPassword.Text = "Set key by db password";
        this.buttonManageDbPassword.Enabled = this.dbPasswordIsDefined && !this.checkBoxMeterKeyIsDefined.Checked;
      }
    }

    private void keyWorkStart()
    {
      this.Cursor = Cursors.WaitCursor;
      this.Enabled = false;
      this.textBoxState.Clear();
      ZR_ClassLibMessages.ClearErrors();
    }

    private void keyWorkEnd()
    {
      this.checkBoxConnectedDeviceIsLocked.Checked = this.MyFunctions.MyCommands.DeviceProtectionGet();
      this.checkBoxMeterKeyIsDefined.Checked = !this.MyFunctions.MyCommands.DeviceProtectionSetKey(uint.MaxValue);
      this.SetMeterKeyState();
      this.Enabled = true;
      this.Cursor = Cursors.Default;
      ZR_ClassLibMessages.ClearErrors();
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
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.label2 = new Label();
      this.label1 = new Label();
      this.label4 = new Label();
      this.numericUpDownVolumeCycle = new NumericUpDown();
      this.label3 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.numericUpDownEnergyCycle = new NumericUpDown();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.textBoxVolumeCycleTime = new TextBox();
      this.label12 = new Label();
      this.label13 = new Label();
      this.textBoxEnergyCycleTime = new TextBox();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.label17 = new Label();
      this.numericUpDownRadioCycle = new NumericUpDown();
      this.label18 = new Label();
      this.textBoxRadioCycleTimr = new TextBox();
      this.label19 = new Label();
      this.label20 = new Label();
      this.label21 = new Label();
      this.label22 = new Label();
      this.numericUpDownSlowEnergyCycle = new NumericUpDown();
      this.label23 = new Label();
      this.textBoxSlowEnergyCycleTime = new TextBox();
      this.label24 = new Label();
      this.label25 = new Label();
      this.label26 = new Label();
      this.label27 = new Label();
      this.numericUpDownFastOffTime = new NumericUpDown();
      this.label28 = new Label();
      this.textBoxFastOffTime = new TextBox();
      this.buttonShowState = new Button();
      this.textBoxState = new TextBox();
      this.label29 = new Label();
      this.groupBoxTimingSetup = new GroupBox();
      this.btnSet = new Button();
      this.buttonFast = new Button();
      this.buttonMechanic = new Button();
      this.buttonUltrasonic = new Button();
      this.buttonShowStateLoop = new Button();
      this.groupBoxDateAndTime = new GroupBox();
      this.checkBoxUsePcTime = new CheckBox();
      this.buttonDateAndTimeSet = new Button();
      this.textBoxUtcTime = new TextBox();
      this.textBoxPcTime = new TextBox();
      this.textBoxTimeZone = new TextBox();
      this.label34 = new Label();
      this.label33 = new Label();
      this.label32 = new Label();
      this.label30 = new Label();
      this.dateTimePickerTime = new DateTimePicker();
      this.dateTimePickerDate = new DateTimePicker();
      this.buttonSetTimeImmediately = new Button();
      this.buttonReadTimeAgain = new Button();
      this.timer1 = new Timer(this.components);
      this.groupBoxWritePermission = new GroupBox();
      this.textBoxPassword = new TextBox();
      this.textBoxMeterKey = new TextBox();
      this.buttonManagePassword = new Button();
      this.buttonManageDbPassword = new Button();
      this.buttonManageMeterKey = new Button();
      this.checkBoxMeterKeyIsDefined = new CheckBox();
      this.checkBoxConnectedDeviceIsLocked = new CheckBox();
      this.buttonLockDevice = new Button();
      this.label31 = new Label();
      this.label35 = new Label();
      this.groupBoxConnectedMeterTime = new GroupBox();
      this.textBoxTimeOfConnectedMeter = new TextBox();
      this.numericUpDownVolumeCycle.BeginInit();
      this.numericUpDownEnergyCycle.BeginInit();
      this.numericUpDownRadioCycle.BeginInit();
      this.numericUpDownSlowEnergyCycle.BeginInit();
      this.numericUpDownFastOffTime.BeginInit();
      this.groupBoxTimingSetup.SuspendLayout();
      this.groupBoxDateAndTime.SuspendLayout();
      this.groupBoxWritePermission.SuspendLayout();
      this.groupBoxConnectedMeterTime.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(940, 45);
      this.zennerCoroprateDesign2.TabIndex = 18;
      this.label2.Location = new Point(7, 45);
      this.label2.Name = "label2";
      this.label2.RightToLeft = RightToLeft.Yes;
      this.label2.Size = new Size(89, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "VolumeCycle";
      this.label1.Location = new Point(7, 20);
      this.label1.Name = "label1";
      this.label1.RightToLeft = RightToLeft.Yes;
      this.label1.Size = new Size(89, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "BaseCycle";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(102, 45);
      this.label4.Name = "label4";
      this.label4.Size = new Size(13, 13);
      this.label4.TabIndex = 20;
      this.label4.Text = "=";
      this.numericUpDownVolumeCycle.Location = new Point(121, 43);
      this.numericUpDownVolumeCycle.Maximum = new Decimal(new int[4]
      {
        120,
        0,
        0,
        0
      });
      this.numericUpDownVolumeCycle.Name = "numericUpDownVolumeCycle";
      this.numericUpDownVolumeCycle.Size = new Size(61, 20);
      this.numericUpDownVolumeCycle.TabIndex = 22;
      this.numericUpDownVolumeCycle.Value = new Decimal(new int[4]
      {
        8,
        0,
        0,
        0
      });
      this.numericUpDownVolumeCycle.ValueChanged += new System.EventHandler(this.Cycle_ValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(102, 20);
      this.label3.Name = "label3";
      this.label3.Size = new Size(13, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "=";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(121, 20);
      this.label5.Name = "label5";
      this.label5.Size = new Size(65, 13);
      this.label5.TabIndex = 23;
      this.label5.Text = "0.5 seconds";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(188, 45);
      this.label6.Name = "label6";
      this.label6.Size = new Size(65, 13);
      this.label6.TabIndex = 23;
      this.label6.Text = "x BaseCycle";
      this.label7.Location = new Point(7, 72);
      this.label7.Name = "label7";
      this.label7.RightToLeft = RightToLeft.Yes;
      this.label7.Size = new Size(89, 13);
      this.label7.TabIndex = 19;
      this.label7.Text = "FastEnergyCycle";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(102, 72);
      this.label8.Name = "label8";
      this.label8.Size = new Size(13, 13);
      this.label8.TabIndex = 20;
      this.label8.Text = "=";
      this.numericUpDownEnergyCycle.Location = new Point(121, 70);
      this.numericUpDownEnergyCycle.Maximum = new Decimal(new int[4]
      {
        20,
        0,
        0,
        0
      });
      this.numericUpDownEnergyCycle.Name = "numericUpDownEnergyCycle";
      this.numericUpDownEnergyCycle.Size = new Size(61, 20);
      this.numericUpDownEnergyCycle.TabIndex = 22;
      this.numericUpDownEnergyCycle.Value = new Decimal(new int[4]
      {
        4,
        0,
        0,
        0
      });
      this.numericUpDownEnergyCycle.ValueChanged += new System.EventHandler(this.Cycle_ValueChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(188, 72);
      this.label9.Name = "label9";
      this.label9.Size = new Size(76, 13);
      this.label9.TabIndex = 23;
      this.label9.Text = "x VolumeCycle";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(296, 45);
      this.label10.Name = "label10";
      this.label10.Size = new Size(13, 13);
      this.label10.TabIndex = 20;
      this.label10.Text = "=";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(296, 72);
      this.label11.Name = "label11";
      this.label11.Size = new Size(13, 13);
      this.label11.TabIndex = 20;
      this.label11.Text = "=";
      this.textBoxVolumeCycleTime.Enabled = false;
      this.textBoxVolumeCycleTime.Location = new Point(315, 45);
      this.textBoxVolumeCycleTime.Name = "textBoxVolumeCycleTime";
      this.textBoxVolumeCycleTime.Size = new Size(59, 20);
      this.textBoxVolumeCycleTime.TabIndex = 24;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(380, 48);
      this.label12.Name = "label12";
      this.label12.Size = new Size(47, 13);
      this.label12.TabIndex = 23;
      this.label12.Text = "seconds";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(380, 72);
      this.label13.Name = "label13";
      this.label13.Size = new Size(47, 13);
      this.label13.TabIndex = 23;
      this.label13.Text = "seconds";
      this.textBoxEnergyCycleTime.Enabled = false;
      this.textBoxEnergyCycleTime.Location = new Point(315, 69);
      this.textBoxEnergyCycleTime.Name = "textBoxEnergyCycleTime";
      this.textBoxEnergyCycleTime.Size = new Size(59, 20);
      this.textBoxEnergyCycleTime.TabIndex = 24;
      this.label14.Location = new Point(7, 172);
      this.label14.Name = "label14";
      this.label14.RightToLeft = RightToLeft.Yes;
      this.label14.Size = new Size(89, 13);
      this.label14.TabIndex = 19;
      this.label14.Text = "RadioCycle";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(296, 172);
      this.label15.Name = "label15";
      this.label15.Size = new Size(13, 13);
      this.label15.TabIndex = 20;
      this.label15.Text = "=";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(102, 172);
      this.label16.Name = "label16";
      this.label16.Size = new Size(13, 13);
      this.label16.TabIndex = 20;
      this.label16.Text = "=";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(380, 172);
      this.label17.Name = "label17";
      this.label17.Size = new Size(47, 13);
      this.label17.TabIndex = 23;
      this.label17.Text = "seconds";
      this.numericUpDownRadioCycle.Location = new Point(121, 170);
      this.numericUpDownRadioCycle.Maximum = new Decimal(new int[4]
      {
        30000,
        0,
        0,
        0
      });
      this.numericUpDownRadioCycle.Name = "numericUpDownRadioCycle";
      this.numericUpDownRadioCycle.Size = new Size(61, 20);
      this.numericUpDownRadioCycle.TabIndex = 22;
      this.numericUpDownRadioCycle.Value = new Decimal(new int[4]
      {
        45,
        0,
        0,
        0
      });
      this.numericUpDownRadioCycle.ValueChanged += new System.EventHandler(this.Cycle_ValueChanged);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(188, 172);
      this.label18.Name = "label18";
      this.label18.Size = new Size(76, 13);
      this.label18.TabIndex = 23;
      this.label18.Text = "x VolumeCycle";
      this.textBoxRadioCycleTimr.Enabled = false;
      this.textBoxRadioCycleTimr.Location = new Point(315, 169);
      this.textBoxRadioCycleTimr.Name = "textBoxRadioCycleTimr";
      this.textBoxRadioCycleTimr.Size = new Size(59, 20);
      this.textBoxRadioCycleTimr.TabIndex = 24;
      this.label19.Location = new Point(7, 98);
      this.label19.Name = "label19";
      this.label19.RightToLeft = RightToLeft.Yes;
      this.label19.Size = new Size(89, 13);
      this.label19.TabIndex = 19;
      this.label19.Text = "SlowEnergyCycle";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(296, 98);
      this.label20.Name = "label20";
      this.label20.Size = new Size(13, 13);
      this.label20.TabIndex = 20;
      this.label20.Text = "=";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(102, 98);
      this.label21.Name = "label21";
      this.label21.Size = new Size(13, 13);
      this.label21.TabIndex = 20;
      this.label21.Text = "=";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(380, 98);
      this.label22.Name = "label22";
      this.label22.Size = new Size(47, 13);
      this.label22.TabIndex = 23;
      this.label22.Text = "seconds";
      this.numericUpDownSlowEnergyCycle.Location = new Point(121, 96);
      this.numericUpDownSlowEnergyCycle.Maximum = new Decimal(new int[4]
      {
        20,
        0,
        0,
        0
      });
      this.numericUpDownSlowEnergyCycle.Name = "numericUpDownSlowEnergyCycle";
      this.numericUpDownSlowEnergyCycle.Size = new Size(61, 20);
      this.numericUpDownSlowEnergyCycle.TabIndex = 22;
      this.numericUpDownSlowEnergyCycle.Value = new Decimal(new int[4]
      {
        4,
        0,
        0,
        0
      });
      this.numericUpDownSlowEnergyCycle.ValueChanged += new System.EventHandler(this.Cycle_ValueChanged);
      this.label23.AutoSize = true;
      this.label23.Location = new Point(188, 98);
      this.label23.Name = "label23";
      this.label23.Size = new Size(94, 13);
      this.label23.TabIndex = 23;
      this.label23.Text = "x FastEnergyCycle";
      this.textBoxSlowEnergyCycleTime.Enabled = false;
      this.textBoxSlowEnergyCycleTime.Location = new Point(315, 95);
      this.textBoxSlowEnergyCycleTime.Name = "textBoxSlowEnergyCycleTime";
      this.textBoxSlowEnergyCycleTime.Size = new Size(59, 20);
      this.textBoxSlowEnergyCycleTime.TabIndex = 24;
      this.label24.Location = new Point(7, 124);
      this.label24.Name = "label24";
      this.label24.RightToLeft = RightToLeft.Yes;
      this.label24.Size = new Size(89, 13);
      this.label24.TabIndex = 19;
      this.label24.Text = "FastOftTime";
      this.label25.AutoSize = true;
      this.label25.Location = new Point(296, 124);
      this.label25.Name = "label25";
      this.label25.Size = new Size(13, 13);
      this.label25.TabIndex = 20;
      this.label25.Text = "=";
      this.label26.AutoSize = true;
      this.label26.Location = new Point(102, 124);
      this.label26.Name = "label26";
      this.label26.Size = new Size(13, 13);
      this.label26.TabIndex = 20;
      this.label26.Text = "=";
      this.label27.AutoSize = true;
      this.label27.Location = new Point(380, 124);
      this.label27.Name = "label27";
      this.label27.Size = new Size(47, 13);
      this.label27.TabIndex = 23;
      this.label27.Text = "seconds";
      this.numericUpDownFastOffTime.Location = new Point(121, 122);
      this.numericUpDownFastOffTime.Maximum = new Decimal(new int[4]
      {
        20,
        0,
        0,
        0
      });
      this.numericUpDownFastOffTime.Name = "numericUpDownFastOffTime";
      this.numericUpDownFastOffTime.Size = new Size(61, 20);
      this.numericUpDownFastOffTime.TabIndex = 22;
      this.numericUpDownFastOffTime.Value = new Decimal(new int[4]
      {
        4,
        0,
        0,
        0
      });
      this.numericUpDownFastOffTime.ValueChanged += new System.EventHandler(this.Cycle_ValueChanged);
      this.label28.AutoSize = true;
      this.label28.Location = new Point(188, 124);
      this.label28.Name = "label28";
      this.label28.Size = new Size(94, 13);
      this.label28.TabIndex = 23;
      this.label28.Text = "x FastEnergyCycle";
      this.textBoxFastOffTime.Enabled = false;
      this.textBoxFastOffTime.Location = new Point(315, 121);
      this.textBoxFastOffTime.Name = "textBoxFastOffTime";
      this.textBoxFastOffTime.Size = new Size(59, 20);
      this.textBoxFastOffTime.TabIndex = 24;
      this.buttonShowState.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonShowState.Location = new Point(770, 574);
      this.buttonShowState.Name = "buttonShowState";
      this.buttonShowState.Size = new Size(158, 23);
      this.buttonShowState.TabIndex = 25;
      this.buttonShowState.Text = "Show state";
      this.buttonShowState.UseVisualStyleBackColor = true;
      this.buttonShowState.Click += new System.EventHandler(this.buttonShowState_Click);
      this.textBoxState.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxState.Location = new Point(450, 67);
      this.textBoxState.Multiline = true;
      this.textBoxState.Name = "textBoxState";
      this.textBoxState.ReadOnly = true;
      this.textBoxState.Size = new Size(478, 488);
      this.textBoxState.TabIndex = 26;
      this.label29.Location = new Point(450, 47);
      this.label29.Name = "label29";
      this.label29.RightToLeft = RightToLeft.No;
      this.label29.Size = new Size(89, 13);
      this.label29.TabIndex = 19;
      this.label29.Text = "Timing state";
      this.groupBoxTimingSetup.Controls.Add((Control) this.btnSet);
      this.groupBoxTimingSetup.Controls.Add((Control) this.buttonFast);
      this.groupBoxTimingSetup.Controls.Add((Control) this.buttonMechanic);
      this.groupBoxTimingSetup.Controls.Add((Control) this.buttonUltrasonic);
      this.groupBoxTimingSetup.Controls.Add((Control) this.numericUpDownFastOffTime);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label2);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label1);
      this.groupBoxTimingSetup.Controls.Add((Control) this.textBoxRadioCycleTimr);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label7);
      this.groupBoxTimingSetup.Controls.Add((Control) this.textBoxFastOffTime);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label4);
      this.groupBoxTimingSetup.Controls.Add((Control) this.textBoxSlowEnergyCycleTime);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label19);
      this.groupBoxTimingSetup.Controls.Add((Control) this.textBoxEnergyCycleTime);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label14);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label28);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label24);
      this.groupBoxTimingSetup.Controls.Add((Control) this.textBoxVolumeCycleTime);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label10);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label23);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label11);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label18);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label3);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label9);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label20);
      this.groupBoxTimingSetup.Controls.Add((Control) this.numericUpDownRadioCycle);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label15);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label25);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label6);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label8);
      this.groupBoxTimingSetup.Controls.Add((Control) this.numericUpDownSlowEnergyCycle);
      this.groupBoxTimingSetup.Controls.Add((Control) this.numericUpDownVolumeCycle);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label27);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label21);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label17);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label16);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label22);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label26);
      this.groupBoxTimingSetup.Controls.Add((Control) this.numericUpDownEnergyCycle);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label5);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label13);
      this.groupBoxTimingSetup.Controls.Add((Control) this.label12);
      this.groupBoxTimingSetup.Location = new Point(12, 337);
      this.groupBoxTimingSetup.Name = "groupBoxTimingSetup";
      this.groupBoxTimingSetup.Size = new Size(432, 248);
      this.groupBoxTimingSetup.TabIndex = 27;
      this.groupBoxTimingSetup.TabStop = false;
      this.groupBoxTimingSetup.Text = "Timing setup";
      this.btnSet.Location = new Point(351, 219);
      this.btnSet.Name = "btnSet";
      this.btnSet.Size = new Size(75, 23);
      this.btnSet.TabIndex = 28;
      this.btnSet.Text = "Set";
      this.btnSet.UseVisualStyleBackColor = true;
      this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
      this.buttonFast.Location = new Point(188, 219);
      this.buttonFast.Name = "buttonFast";
      this.buttonFast.Size = new Size(86, 23);
      this.buttonFast.TabIndex = 28;
      this.buttonFast.Text = "Fast";
      this.buttonFast.UseVisualStyleBackColor = true;
      this.buttonFast.Click += new System.EventHandler(this.buttonFast_Click);
      this.buttonMechanic.Location = new Point(96, 219);
      this.buttonMechanic.Name = "buttonMechanic";
      this.buttonMechanic.Size = new Size(86, 23);
      this.buttonMechanic.TabIndex = 28;
      this.buttonMechanic.Text = "Mechanic";
      this.buttonMechanic.UseVisualStyleBackColor = true;
      this.buttonMechanic.Click += new System.EventHandler(this.buttonMechanic_Click);
      this.buttonUltrasonic.Location = new Point(7, 219);
      this.buttonUltrasonic.Name = "buttonUltrasonic";
      this.buttonUltrasonic.Size = new Size(86, 23);
      this.buttonUltrasonic.TabIndex = 28;
      this.buttonUltrasonic.Text = "Ultrasonic";
      this.buttonUltrasonic.UseVisualStyleBackColor = true;
      this.buttonUltrasonic.Click += new System.EventHandler(this.buttonUltrasonic_Click);
      this.buttonShowStateLoop.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonShowStateLoop.Location = new Point(606, 574);
      this.buttonShowStateLoop.Name = "buttonShowStateLoop";
      this.buttonShowStateLoop.Size = new Size(158, 23);
      this.buttonShowStateLoop.TabIndex = 25;
      this.buttonShowStateLoop.Text = "Show state loop";
      this.buttonShowStateLoop.UseVisualStyleBackColor = true;
      this.buttonShowStateLoop.Click += new System.EventHandler(this.buttonShowStateLoop_Click);
      this.groupBoxDateAndTime.Controls.Add((Control) this.checkBoxUsePcTime);
      this.groupBoxDateAndTime.Controls.Add((Control) this.buttonDateAndTimeSet);
      this.groupBoxDateAndTime.Controls.Add((Control) this.textBoxUtcTime);
      this.groupBoxDateAndTime.Controls.Add((Control) this.textBoxPcTime);
      this.groupBoxDateAndTime.Controls.Add((Control) this.textBoxTimeZone);
      this.groupBoxDateAndTime.Controls.Add((Control) this.label34);
      this.groupBoxDateAndTime.Controls.Add((Control) this.label33);
      this.groupBoxDateAndTime.Controls.Add((Control) this.label32);
      this.groupBoxDateAndTime.Controls.Add((Control) this.label30);
      this.groupBoxDateAndTime.Controls.Add((Control) this.dateTimePickerTime);
      this.groupBoxDateAndTime.Controls.Add((Control) this.dateTimePickerDate);
      this.groupBoxDateAndTime.Location = new Point(12, 47);
      this.groupBoxDateAndTime.Name = "groupBoxDateAndTime";
      this.groupBoxDateAndTime.Size = new Size(224, 177);
      this.groupBoxDateAndTime.TabIndex = 28;
      this.groupBoxDateAndTime.TabStop = false;
      this.groupBoxDateAndTime.Text = "Date and Time";
      this.checkBoxUsePcTime.AutoSize = true;
      this.checkBoxUsePcTime.Location = new Point(115, 122);
      this.checkBoxUsePcTime.Name = "checkBoxUsePcTime";
      this.checkBoxUsePcTime.Size = new Size(84, 17);
      this.checkBoxUsePcTime.TabIndex = 18;
      this.checkBoxUsePcTime.Text = "Use PC time";
      this.checkBoxUsePcTime.UseVisualStyleBackColor = true;
      this.checkBoxUsePcTime.CheckedChanged += new System.EventHandler(this.checkBoxUsePcTime_CheckedChanged);
      this.buttonDateAndTimeSet.Location = new Point(6, 146);
      this.buttonDateAndTimeSet.Name = "buttonDateAndTimeSet";
      this.buttonDateAndTimeSet.Size = new Size(212, 23);
      this.buttonDateAndTimeSet.TabIndex = 2;
      this.buttonDateAndTimeSet.Text = "Set";
      this.buttonDateAndTimeSet.UseVisualStyleBackColor = true;
      this.buttonDateAndTimeSet.Click += new System.EventHandler(this.buttonDateAndTimeSet_Click);
      this.textBoxUtcTime.Location = new Point(84, 45);
      this.textBoxUtcTime.Name = "textBoxUtcTime";
      this.textBoxUtcTime.ReadOnly = true;
      this.textBoxUtcTime.Size = new Size(131, 20);
      this.textBoxUtcTime.TabIndex = 18;
      this.textBoxPcTime.Location = new Point(84, 17);
      this.textBoxPcTime.Name = "textBoxPcTime";
      this.textBoxPcTime.ReadOnly = true;
      this.textBoxPcTime.Size = new Size(131, 20);
      this.textBoxPcTime.TabIndex = 18;
      this.textBoxTimeZone.Location = new Point(54, 120);
      this.textBoxTimeZone.Name = "textBoxTimeZone";
      this.textBoxTimeZone.Size = new Size(55, 20);
      this.textBoxTimeZone.TabIndex = 18;
      this.label34.AutoSize = true;
      this.label34.Location = new Point(6, 70);
      this.label34.Name = "label34";
      this.label34.Size = new Size(143, 13);
      this.label34.TabIndex = 18;
      this.label34.Text = "Meter time for set operations:";
      this.label33.AutoSize = true;
      this.label33.Location = new Point(6, 45);
      this.label33.Name = "label33";
      this.label33.Size = new Size(54, 13);
      this.label33.TabIndex = 18;
      this.label33.Text = "UTC time:";
      this.label32.AutoSize = true;
      this.label32.Location = new Point(6, 20);
      this.label32.Name = "label32";
      this.label32.Size = new Size(46, 13);
      this.label32.TabIndex = 18;
      this.label32.Text = "PC time:";
      this.label30.AutoSize = true;
      this.label30.Location = new Point(13, 123);
      this.label30.Name = "label30";
      this.label30.Size = new Size(35, 13);
      this.label30.TabIndex = 18;
      this.label30.Text = "Zone:";
      this.dateTimePickerTime.Format = DateTimePickerFormat.Time;
      this.dateTimePickerTime.Location = new Point(115, 94);
      this.dateTimePickerTime.MinDate = new DateTime(1980, 1, 1, 0, 0, 0, 0);
      this.dateTimePickerTime.Name = "dateTimePickerTime";
      this.dateTimePickerTime.ShowUpDown = true;
      this.dateTimePickerTime.Size = new Size(101, 20);
      this.dateTimePickerTime.TabIndex = 1;
      this.dateTimePickerDate.Format = DateTimePickerFormat.Short;
      this.dateTimePickerDate.Location = new Point(8, 94);
      this.dateTimePickerDate.MinDate = new DateTime(1980, 1, 1, 0, 0, 0, 0);
      this.dateTimePickerDate.Name = "dateTimePickerDate";
      this.dateTimePickerDate.Size = new Size(101, 20);
      this.dateTimePickerDate.TabIndex = 0;
      this.buttonSetTimeImmediately.Location = new Point(5, 74);
      this.buttonSetTimeImmediately.Name = "buttonSetTimeImmediately";
      this.buttonSetTimeImmediately.Size = new Size(213, 21);
      this.buttonSetTimeImmediately.TabIndex = 18;
      this.buttonSetTimeImmediately.Text = "Set time immediately";
      this.buttonSetTimeImmediately.UseVisualStyleBackColor = true;
      this.buttonSetTimeImmediately.Click += new System.EventHandler(this.buttonSetTimeImmediately_Click);
      this.buttonReadTimeAgain.Location = new Point(5, 45);
      this.buttonReadTimeAgain.Name = "buttonReadTimeAgain";
      this.buttonReadTimeAgain.Size = new Size(213, 21);
      this.buttonReadTimeAgain.TabIndex = 18;
      this.buttonReadTimeAgain.Text = "Read time again";
      this.buttonReadTimeAgain.UseVisualStyleBackColor = true;
      this.buttonReadTimeAgain.Click += new System.EventHandler(this.buttonReadTimeAgain_Click);
      this.timer1.Enabled = true;
      this.timer1.Interval = 1000;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      this.groupBoxWritePermission.Controls.Add((Control) this.textBoxPassword);
      this.groupBoxWritePermission.Controls.Add((Control) this.textBoxMeterKey);
      this.groupBoxWritePermission.Controls.Add((Control) this.buttonManagePassword);
      this.groupBoxWritePermission.Controls.Add((Control) this.buttonManageDbPassword);
      this.groupBoxWritePermission.Controls.Add((Control) this.buttonManageMeterKey);
      this.groupBoxWritePermission.Controls.Add((Control) this.checkBoxMeterKeyIsDefined);
      this.groupBoxWritePermission.Controls.Add((Control) this.checkBoxConnectedDeviceIsLocked);
      this.groupBoxWritePermission.Controls.Add((Control) this.buttonLockDevice);
      this.groupBoxWritePermission.Controls.Add((Control) this.label31);
      this.groupBoxWritePermission.Controls.Add((Control) this.label35);
      this.groupBoxWritePermission.Location = new Point(243, 47);
      this.groupBoxWritePermission.Name = "groupBoxWritePermission";
      this.groupBoxWritePermission.Size = new Size(201, 284);
      this.groupBoxWritePermission.TabIndex = 29;
      this.groupBoxWritePermission.TabStop = false;
      this.groupBoxWritePermission.Text = "Write permission";
      this.textBoxPassword.Location = new Point(10, 197);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.Size = new Size(180, 20);
      this.textBoxPassword.TabIndex = 0;
      this.textBoxPassword.TextChanged += new System.EventHandler(this.textBox_TextChanged);
      this.textBoxMeterKey.Location = new Point(10, 116);
      this.textBoxMeterKey.Name = "textBoxMeterKey";
      this.textBoxMeterKey.Size = new Size(180, 20);
      this.textBoxMeterKey.TabIndex = 0;
      this.textBoxMeterKey.TextChanged += new System.EventHandler(this.textBox_TextChanged);
      this.buttonManagePassword.Location = new Point(9, 226);
      this.buttonManagePassword.Name = "buttonManagePassword";
      this.buttonManagePassword.Size = new Size(181, 23);
      this.buttonManagePassword.TabIndex = 4;
      this.buttonManagePassword.Text = "Manage password";
      this.buttonManagePassword.UseVisualStyleBackColor = true;
      this.buttonManagePassword.Click += new System.EventHandler(this.buttonManagePassword_Click);
      this.buttonManageDbPassword.Location = new Point(9, (int) byte.MaxValue);
      this.buttonManageDbPassword.Name = "buttonManageDbPassword";
      this.buttonManageDbPassword.Size = new Size(181, 23);
      this.buttonManageDbPassword.TabIndex = 4;
      this.buttonManageDbPassword.Text = "Manage DB password";
      this.buttonManageDbPassword.UseVisualStyleBackColor = true;
      this.buttonManageDbPassword.Click += new System.EventHandler(this.buttonManageDbPassword_Click);
      this.buttonManageMeterKey.Location = new Point(9, 143);
      this.buttonManageMeterKey.Name = "buttonManageMeterKey";
      this.buttonManageMeterKey.Size = new Size(181, 23);
      this.buttonManageMeterKey.TabIndex = 4;
      this.buttonManageMeterKey.Text = "Manage meter key";
      this.buttonManageMeterKey.UseVisualStyleBackColor = true;
      this.buttonManageMeterKey.Click += new System.EventHandler(this.buttonManageMeterKey_Click);
      this.checkBoxMeterKeyIsDefined.AutoSize = true;
      this.checkBoxMeterKeyIsDefined.Enabled = false;
      this.checkBoxMeterKeyIsDefined.Location = new Point(9, 41);
      this.checkBoxMeterKeyIsDefined.Name = "checkBoxMeterKeyIsDefined";
      this.checkBoxMeterKeyIsDefined.Size = new Size(121, 17);
      this.checkBoxMeterKeyIsDefined.TabIndex = 3;
      this.checkBoxMeterKeyIsDefined.Text = "Meter key is defined";
      this.checkBoxMeterKeyIsDefined.UseVisualStyleBackColor = true;
      this.checkBoxConnectedDeviceIsLocked.AutoSize = true;
      this.checkBoxConnectedDeviceIsLocked.Enabled = false;
      this.checkBoxConnectedDeviceIsLocked.Location = new Point(9, 21);
      this.checkBoxConnectedDeviceIsLocked.Name = "checkBoxConnectedDeviceIsLocked";
      this.checkBoxConnectedDeviceIsLocked.Size = new Size(158, 17);
      this.checkBoxConnectedDeviceIsLocked.TabIndex = 3;
      this.checkBoxConnectedDeviceIsLocked.Text = "Connected device is locked";
      this.checkBoxConnectedDeviceIsLocked.UseVisualStyleBackColor = true;
      this.buttonLockDevice.Location = new Point(9, 60);
      this.buttonLockDevice.Name = "buttonLockDevice";
      this.buttonLockDevice.Size = new Size(181, 23);
      this.buttonLockDevice.TabIndex = 2;
      this.buttonLockDevice.Text = "Lock device";
      this.buttonLockDevice.UseVisualStyleBackColor = true;
      this.buttonLockDevice.Click += new System.EventHandler(this.buttonLockDevice_Click);
      this.label31.AutoSize = true;
      this.label31.Location = new Point(10, 103);
      this.label31.Name = "label31";
      this.label31.Size = new Size(54, 13);
      this.label31.TabIndex = 1;
      this.label31.Text = "Meter key";
      this.label35.AutoSize = true;
      this.label35.Location = new Point(11, 184);
      this.label35.Name = "label35";
      this.label35.Size = new Size(53, 13);
      this.label35.TabIndex = 1;
      this.label35.Text = "Password";
      this.groupBoxConnectedMeterTime.Controls.Add((Control) this.buttonSetTimeImmediately);
      this.groupBoxConnectedMeterTime.Controls.Add((Control) this.buttonReadTimeAgain);
      this.groupBoxConnectedMeterTime.Controls.Add((Control) this.textBoxTimeOfConnectedMeter);
      this.groupBoxConnectedMeterTime.Location = new Point(12, 230);
      this.groupBoxConnectedMeterTime.Name = "groupBoxConnectedMeterTime";
      this.groupBoxConnectedMeterTime.Size = new Size(224, 101);
      this.groupBoxConnectedMeterTime.TabIndex = 30;
      this.groupBoxConnectedMeterTime.TabStop = false;
      this.groupBoxConnectedMeterTime.Text = "Time of connected meter";
      this.textBoxTimeOfConnectedMeter.Location = new Point(6, 17);
      this.textBoxTimeOfConnectedMeter.Name = "textBoxTimeOfConnectedMeter";
      this.textBoxTimeOfConnectedMeter.ReadOnly = true;
      this.textBoxTimeOfConnectedMeter.Size = new Size(212, 20);
      this.textBoxTimeOfConnectedMeter.TabIndex = 18;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(940, 609);
      this.Controls.Add((Control) this.groupBoxConnectedMeterTime);
      this.Controls.Add((Control) this.groupBoxWritePermission);
      this.Controls.Add((Control) this.groupBoxDateAndTime);
      this.Controls.Add((Control) this.groupBoxTimingSetup);
      this.Controls.Add((Control) this.textBoxState);
      this.Controls.Add((Control) this.buttonShowStateLoop);
      this.Controls.Add((Control) this.buttonShowState);
      this.Controls.Add((Control) this.label29);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (TimeSettings);
      this.Text = "Times and permissions";
      this.numericUpDownVolumeCycle.EndInit();
      this.numericUpDownEnergyCycle.EndInit();
      this.numericUpDownRadioCycle.EndInit();
      this.numericUpDownSlowEnergyCycle.EndInit();
      this.numericUpDownFastOffTime.EndInit();
      this.groupBoxTimingSetup.ResumeLayout(false);
      this.groupBoxTimingSetup.PerformLayout();
      this.groupBoxDateAndTime.ResumeLayout(false);
      this.groupBoxDateAndTime.PerformLayout();
      this.groupBoxWritePermission.ResumeLayout(false);
      this.groupBoxWritePermission.PerformLayout();
      this.groupBoxConnectedMeterTime.ResumeLayout(false);
      this.groupBoxConnectedMeterTime.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
