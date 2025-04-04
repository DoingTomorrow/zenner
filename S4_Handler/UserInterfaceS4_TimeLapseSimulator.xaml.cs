// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_TimeLapseSimulator
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using HandlerLib;
using NLog;
using S4_Handler.Functions;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_TimeLapseSimulator : Window, IComponentConnector
  {
    private static Logger S4_TimeLapseSimulatorLogger = LogManager.GetLogger(nameof (S4_TimeLapseSimulator));
    private ChannelLogger NLog_cl;
    private CancellationTokenSource cancelTokenSource;
    private CancellationToken cancelToken;
    private ProgressHandler progress;
    private S4_HandlerWindowFunctions myWindowFunctions;
    private S4_HandlerFunctions myFunctions;
    private NfcDeviceCommands myCommands;
    private S4_DeviceMemory workMeterMemory = (S4_DeviceMemory) null;
    private S4_DeviceMemory connectedMeterMemory = (S4_DeviceMemory) null;
    private AddressRange counterRange = (AddressRange) null;
    private AddressRange minMaxFlowRange = (AddressRange) null;
    private float? MinHourlyFlow;
    private float? MaxHourlyFlow;
    private float? Min5MinutesFlow;
    private float? Max5MinutesFlow;
    private AddressRange flowLimitsRangeQx = (AddressRange) null;
    private AddressRange flowLimitsRangeQminMax = (AddressRange) null;
    private bool IsWriteProtected;
    private DateTimeOffset? DeviceDateTimeOffset;
    private Decimal TimeZone;
    private int StopSecondsBeforeStep;
    private int WaitSecondsPerStep;
    private LoRaEventData Next_LoRaEventData;
    private List<Button> DisableButtonList;
    private StringBuilder Warnings = new StringBuilder();
    internal StackPanel StackPanelSimulation;
    internal TextBox TextBoxSimulatedFlow;
    internal Button ButtonSetSimulatedFlow;
    internal TextBox TextBoxStopSecondsBevoreStep;
    internal TextBox TextBoxWaitSecondsPerStep;
    internal DatePicker DatePickerStartDate;
    internal TextBox TextBoxStartTime;
    internal TextBox TextBoxIntervalTime;
    internal RadioButton RadioButtonOnlyTime;
    internal RadioButton RadioButtonDay;
    internal RadioButton RadioButtonHalfMonth;
    internal RadioButton RadioButtonMonth;
    internal RadioButton RadioButtonYear;
    internal RadioButton RadioButtonLoRaCycle;
    internal RadioButton RadioButtonTransmitterCycle;
    internal RadioButton RadioButton_wMBusNoStartCycle;
    internal CheckBox CheckBoxUse5MinutesSteps;
    internal CheckBox CheckBoxUseHourSteps;
    internal CheckBox CheckBoxIgnoreLoRa24Hbreaks;
    internal CheckBox CheckBoxSingleStepsMode;
    internal Button ButtonStartTimeSimulation;
    internal Button ButtonBreak;
    internal Button ButtonClear;
    internal TabControl TabControlFunctions;
    internal TextBox TextBoxVolumeIncrement;
    internal RadioButton RadioButtonVolumeIncrementLikeDefined;
    internal RadioButton RadioButtonVolumeIncrementFromFlow;
    internal GroupBox GroupBoxHourlyMaxFlow;
    internal CheckBox CheckBoxHourlyMaxFlowLikeFlow;
    internal TextBox TextBoxHourlyMaxFlow;
    internal GroupBox GroupBoxHourlyMinFlow;
    internal CheckBox CheckBoxHourlyMinFlowLikeFlow;
    internal TextBox TextBoxHourlyMinFlow;
    internal GroupBox GroupBox5MinutesMaxFlow;
    internal CheckBox CheckBox5MinutesMaxFlowLikeFlow;
    internal TextBox TextBox5MinutesMaxFlow;
    internal GroupBox GroupBox5MinutesMinFlow;
    internal CheckBox CheckBox5MinutesMinFlowLikeFlow;
    internal TextBox TextBox5MinutesMinFlow;
    internal Button ButtonSetAllToNotANumber;
    internal TabItem TabItemState;
    internal TextBox TextBoxQmax;
    internal TextBox TextBoxQ4;
    internal TextBox TextBoxQ3;
    internal TextBox TextBoxQ2;
    internal TextBox TextBoxQ1;
    internal TextBox TextBoxQmin;
    internal Button ButtonReadFlowLimits;
    internal Button ButtonSetDefaultFlowLimits;
    internal Button ButtonWriteFlowLimits;
    internal Button ButtonCheckLoRaState;
    internal Button ButtonShowLoRaAlarmPreparation;
    internal TabItem TabItemManual;
    internal TextBox TextBoxDeviceTime;
    internal Button ButtonLoadDeviceTime;
    internal TextBox TextBoxSetTime;
    internal TextBox TextBoxShiftSeconds;
    internal Button ButtonSetDeviceTime;
    internal Button ButtonShiftDeviceTime;
    internal Button ButtonResetTimeShift;
    internal CheckBox CheckBoxShowLoRaAlarmChanges;
    internal CheckBox CheckBoxShowEventLoggerChanges;
    internal TextBox TextBoxOut;
    private bool _contentLoaded;

    private bool IsRadioControlled
    {
      get
      {
        int num;
        if (!this.RadioButtonLoRaCycle.IsChecked.Value)
        {
          bool? isChecked = this.RadioButtonTransmitterCycle.IsChecked;
          if (!isChecked.Value)
          {
            isChecked = this.RadioButton_wMBusNoStartCycle.IsChecked;
            num = isChecked.Value ? 1 : 0;
            goto label_4;
          }
        }
        num = 1;
label_4:
        return num != 0;
      }
    }

    public S4_TimeLapseSimulator(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.myCommands = this.myFunctions.checkedNfcCommands;
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.NLog_cl = new ChannelLogger(S4_TimeLapseSimulator.S4_TimeLapseSimulatorLogger, this.myFunctions.checkedPort.GetReadoutConfiguration());
      this.InitializeComponent();
      if (this.myFunctions.myMeters.WorkMeter == null || this.myFunctions.myMeters.WorkMeter.meterMemory == null)
      {
        GmmMessage.Show("Please read the meter first");
      }
      else
      {
        this.workMeterMemory = this.myFunctions.myMeters.checkedWorkMeter.meterMemory;
        this.connectedMeterMemory = this.myFunctions.myMeters.checkedConnectedMeter.meterMemory;
        this.counterRange = this.workMeterMemory.GetRangeOfParameters(new S4_Params[3]
        {
          S4_Params.volQmSum,
          S4_Params.volQmPos,
          S4_Params.volQmNeg
        });
        if (!this.workMeterMemory.AreDataAvailable(this.counterRange))
        {
          GmmMessage.Show("Please increase the read range. Volume data are not available");
        }
        else
        {
          try
          {
            GMMConfig gmmConfiguration1 = PlugInLoader.GmmConfiguration;
            S4_HandlerWindowFunctions.ConfigVariables configVariables = S4_HandlerWindowFunctions.ConfigVariables.TimeLapseShowLoRaAlarms;
            string strVariable1 = configVariables.ToString();
            string str1 = gmmConfiguration1.GetValue("S4_Handler", strVariable1);
            if (!string.IsNullOrEmpty(str1))
              this.CheckBoxShowLoRaAlarmChanges.IsChecked = new bool?(bool.Parse(str1));
            GMMConfig gmmConfiguration2 = PlugInLoader.GmmConfiguration;
            configVariables = S4_HandlerWindowFunctions.ConfigVariables.TimeLapseShowEvnetLogger;
            string strVariable2 = configVariables.ToString();
            string str2 = gmmConfiguration2.GetValue("S4_Handler", strVariable2);
            if (!string.IsNullOrEmpty(str2))
              this.CheckBoxShowEventLoggerChanges.IsChecked = new bool?(bool.Parse(str2));
          }
          catch
          {
          }
          if (this.workMeterMemory.IsParameterInMap(S4_Params.MinHourlyFlow))
          {
            AddressRange rangeOfParameters;
            if (this.workMeterMemory.IsParameterInMap(S4_Params.Max5minFlow))
            {
              S4_Params[] theParameters = new S4_Params[4]
              {
                S4_Params.MinHourlyFlow,
                S4_Params.MaxHourlyFlow,
                S4_Params.Min5minFlow,
                S4_Params.Max5minFlow
              };
              this.minMaxFlowRange = this.workMeterMemory.GetRangeOfParameters(theParameters);
              rangeOfParameters = this.connectedMeterMemory.GetRangeOfParameters(theParameters);
              if (this.minMaxFlowRange.ByteSize != 16U)
                throw new Exception("Illegal address range for Min/Max flow variables");
            }
            else
            {
              S4_Params[] theParameters = new S4_Params[2]
              {
                S4_Params.MinHourlyFlow,
                S4_Params.MaxHourlyFlow
              };
              this.minMaxFlowRange = this.workMeterMemory.GetRangeOfParameters(theParameters);
              rangeOfParameters = this.connectedMeterMemory.GetRangeOfParameters(theParameters);
              if (this.minMaxFlowRange.ByteSize != 8U)
                throw new Exception("Illegal address range for Min/Max flow variables");
            }
            if (this.minMaxFlowRange.CompareTo(rangeOfParameters) != 0)
              throw new Exception("Connected meter ranges != work meter ranges! Not allowed for simulation!");
          }
          if (this.workMeterMemory.IsParameterInMap(S4_Params.sfunc_alarm))
          {
            this.ButtonShowLoRaAlarmPreparation.IsEnabled = true;
            this.CheckBoxShowLoRaAlarmChanges.IsEnabled = true;
          }
          else
          {
            this.ButtonShowLoRaAlarmPreparation.IsEnabled = false;
            this.CheckBoxShowLoRaAlarmChanges.IsEnabled = false;
            this.CheckBoxShowLoRaAlarmChanges.IsChecked = new bool?(false);
          }
          if (this.workMeterMemory.IsParameterAvailable(S4_Params.minimumFlowrateQ1))
          {
            this.flowLimitsRangeQx = this.workMeterMemory.GetRangeOfParameters(new S4_Params[4]
            {
              S4_Params.minimumFlowrateQ1,
              S4_Params.transitionalFlowrateQ2,
              S4_Params.permanentFlowrateQ3,
              S4_Params.overloadFlowrateQ4
            });
            this.flowLimitsRangeQminMax = this.workMeterMemory.GetRangeOfParameters(new S4_Params[2]
            {
              S4_Params.maxPositivFlow,
              S4_Params.minPositivFlow
            });
            if (this.flowLimitsRangeQx.ByteSize != 16U || this.flowLimitsRangeQminMax.ByteSize != 8U)
              throw new Exception("Illegal flow limit byte sizes");
            this.ShowFlowLimits();
          }
          this.IsWriteProtected = this.myFunctions.myMeters.WorkMeter.deviceIdentification.IsProtected;
          this.DatePickerStartDate.SelectedDate = new DateTime?(DateTime.Now.Date);
          this.TextBoxStartTime.Text = DateTime.Now.Hour.ToString() + ":59:58";
          if (this.workMeterMemory.IsParameterAvailable(S4_Params.devFlowSimulation))
          {
            float parameterValue = this.workMeterMemory.GetParameterValue<float>(S4_Params.devFlowSimulation);
            if ((double) parameterValue != double.NaN)
              this.TextBoxSimulatedFlow.Text = parameterValue.ToString();
          }
          if (!this.myFunctions.myMeters.WorkMeter.deviceIdentification.IsRadioDevice.Value)
          {
            this.RadioButtonLoRaCycle.IsEnabled = false;
            this.RadioButtonTransmitterCycle.IsEnabled = false;
            this.RadioButton_wMBusNoStartCycle.IsEnabled = false;
          }
          else
          {
            ushort parameterValue = this.workMeterMemory.GetParameterValue<ushort>(S4_Params.com_scenario_intern);
            if (parameterValue > (ushort) 200 && parameterValue < (ushort) 300)
              this.RadioButton_wMBusNoStartCycle.IsEnabled = false;
            else if (parameterValue > (ushort) 300 && parameterValue < (ushort) 400)
            {
              this.RadioButtonLoRaCycle.IsEnabled = false;
            }
            else
            {
              this.RadioButtonLoRaCycle.IsEnabled = false;
              this.RadioButtonTransmitterCycle.IsEnabled = false;
              this.RadioButton_wMBusNoStartCycle.IsEnabled = false;
            }
          }
          this.DisableButtonList = new List<Button>();
          this.DisableButtonList.Add(this.ButtonLoadDeviceTime);
          this.DisableButtonList.Add(this.ButtonReadFlowLimits);
          this.DisableButtonList.Add(this.ButtonSetDefaultFlowLimits);
          this.DisableButtonList.Add(this.ButtonSetDefaultFlowLimits);
          this.DisableButtonList.Add(this.ButtonCheckLoRaState);
          this.DisableButtonList.Add(this.ButtonWriteFlowLimits);
          this.DisableButtonList.Add(this.ButtonSetDeviceTime);
        }
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.RadioButtonOnlyTime.IsChecked = new bool?(true);
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (this.cancelTokenSource != null)
      {
        this.cancelTokenSource.Cancel();
        Thread.Sleep(200);
      }
      GMMConfig gmmConfiguration1 = PlugInLoader.GmmConfiguration;
      string strVariable1 = S4_HandlerWindowFunctions.ConfigVariables.TimeLapseShowLoRaAlarms.ToString();
      bool? isChecked = this.CheckBoxShowLoRaAlarmChanges.IsChecked;
      bool flag = isChecked.Value;
      string strInhalt1 = flag.ToString();
      gmmConfiguration1.SetOrUpdateValue("S4_Handler", strVariable1, strInhalt1);
      GMMConfig gmmConfiguration2 = PlugInLoader.GmmConfiguration;
      string strVariable2 = S4_HandlerWindowFunctions.ConfigVariables.TimeLapseShowEvnetLogger.ToString();
      isChecked = this.CheckBoxShowEventLoggerChanges.IsChecked;
      flag = isChecked.Value;
      string strInhalt2 = flag.ToString();
      gmmConfiguration2.SetOrUpdateValue("S4_Handler", strVariable2, strInhalt2);
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      }
      else
      {
        if (!(obj.Tag is ProgressWarning))
          return;
        this.Warnings.AppendLine("--->" + obj.Message + obj.Tag.ToString());
      }
    }

    private void SetRunState()
    {
      foreach (UIElement disableButton in this.DisableButtonList)
        disableButton.IsEnabled = false;
      this.StackPanelSimulation.IsEnabled = false;
      this.cancelTokenSource = new CancellationTokenSource();
      this.cancelToken = this.cancelTokenSource.Token;
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.Cursor = Cursors.Arrow;
      this.progress.Reset();
      foreach (UIElement disableButton in this.DisableButtonList)
        disableButton.IsEnabled = true;
      this.StackPanelSimulation.IsEnabled = true;
    }

    private async void ButtonSetSimulatedFlow_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        if (sender != this.ButtonSetSimulatedFlow)
          return;
        float simulatedVolume;
        if (!float.TryParse(this.TextBoxSimulatedFlow.Text, out simulatedVolume))
          throw new Exception("Illegal volume simulation value. (not a float)");
        S4_DeviceMemory mem = this.myFunctions.myMeters.checkedWorkMeter.meterMemory;
        if (!mem.IsParameterAvailable(S4_Params.devFlowSimulation))
          throw new Exception("Volume simulation not supported in this firmware");
        mem.SetParameterValue<float>(S4_Params.devFlowSimulation, simulatedVolume);
        await this.myCommands.WriteMemoryAsync(mem.GetParameterAddressRange(S4_Params.devFlowSimulation), (DeviceMemory) mem, this.progress, this.cancelTokenSource.Token);
        mem = (S4_DeviceMemory) null;
      }
      catch (TimeoutException ex)
      {
        ExceptionViewer.Show((Exception) ex);
      }
      catch (HandlerMessageException ex)
      {
        ExceptionViewer.Show((Exception) ex);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void RadioButtonVolumeIncrementLikeDefined_Checked(object sender, RoutedEventArgs e)
    {
      this.TextBoxVolumeIncrement.IsEnabled = true;
    }

    private void RadioButtonVolumeIncrementLikeDefined_Unchecked(object sender, RoutedEventArgs e)
    {
      this.TextBoxVolumeIncrement.IsEnabled = false;
    }

    private async Task ReadMinMaxValues()
    {
      if (this.minMaxFlowRange == null)
        return;
      await this.myFunctions.checkedCommands.ReadMemoryAsync(this.minMaxFlowRange, (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
      this.MinHourlyFlow = new float?(this.workMeterMemory.GetParameterValue<float>(S4_Params.MinHourlyFlow));
      this.MaxHourlyFlow = new float?(this.workMeterMemory.GetParameterValue<float>(S4_Params.MaxHourlyFlow));
      if (this.minMaxFlowRange.ByteSize == 16U)
      {
        this.Min5MinutesFlow = new float?(this.workMeterMemory.GetParameterValue<float>(S4_Params.Min5minFlow));
        this.Max5MinutesFlow = new float?(this.workMeterMemory.GetParameterValue<float>(S4_Params.Max5minFlow));
      }
    }

    private async Task WriteMinMaxValues(float flow)
    {
      if (this.minMaxFlowRange == null)
        return;
      await this.myFunctions.checkedCommands.ReadMemoryAsync(this.minMaxFlowRange, (DeviceMemory) this.connectedMeterMemory, this.progress, this.cancelTokenSource.Token);
      this.MinHourlyFlow = new float?();
      bool? isChecked = this.CheckBoxHourlyMinFlowLikeFlow.IsChecked;
      float flowOut;
      if (isChecked.Value)
        this.MinHourlyFlow = new float?(flow);
      else if (this.TextBoxHourlyMinFlow.Text == "NaN")
        this.MinHourlyFlow = new float?(float.NaN);
      else if (float.TryParse(this.TextBoxHourlyMinFlow.Text, out flowOut))
        this.MinHourlyFlow = new float?(flowOut);
      if (this.MinHourlyFlow.HasValue)
      {
        float readMinHourlyFlow = this.connectedMeterMemory.GetParameterValue<float>(S4_Params.MinHourlyFlow);
        if ((double) readMinHourlyFlow != (double) this.MinHourlyFlow.Value)
        {
          this.workMeterMemory.SetParameterValue<float>(S4_Params.MinHourlyFlow, this.MinHourlyFlow.Value);
          await this.myFunctions.checkedCommands.WriteMemoryAsync(this.workMeterMemory.GetParameterAddressRange(S4_Params.MinHourlyFlow), (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
        }
      }
      this.MaxHourlyFlow = new float?();
      isChecked = this.CheckBoxHourlyMaxFlowLikeFlow.IsChecked;
      if (isChecked.Value)
        this.MaxHourlyFlow = new float?(flow);
      else if (this.TextBoxHourlyMaxFlow.Text == "NaN")
        this.MaxHourlyFlow = new float?(float.NaN);
      else if (float.TryParse(this.TextBoxHourlyMaxFlow.Text, out flowOut))
        this.MaxHourlyFlow = new float?(flowOut);
      if (this.MaxHourlyFlow.HasValue)
      {
        float readMaxHourlyFlow = this.connectedMeterMemory.GetParameterValue<float>(S4_Params.MaxHourlyFlow);
        if ((double) readMaxHourlyFlow != (double) this.MaxHourlyFlow.Value)
        {
          this.workMeterMemory.SetParameterValue<float>(S4_Params.MaxHourlyFlow, this.MaxHourlyFlow.Value);
          await this.myFunctions.checkedCommands.WriteMemoryAsync(this.workMeterMemory.GetParameterAddressRange(S4_Params.MaxHourlyFlow), (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
        }
      }
      if (this.minMaxFlowRange.ByteSize == 16U)
      {
        this.Min5MinutesFlow = new float?();
        isChecked = this.CheckBox5MinutesMinFlowLikeFlow.IsChecked;
        if (isChecked.Value)
          this.Min5MinutesFlow = new float?(flow);
        else if (this.TextBoxHourlyMinFlow.Text == "NaN")
          this.Min5MinutesFlow = new float?(float.NaN);
        else if (float.TryParse(this.TextBoxHourlyMinFlow.Text, out flowOut))
          this.Min5MinutesFlow = new float?(flowOut);
        if (this.Min5MinutesFlow.HasValue)
        {
          float readMin5MinutesFlow = this.connectedMeterMemory.GetParameterValue<float>(S4_Params.MinHourlyFlow);
          if ((double) readMin5MinutesFlow != (double) this.Min5MinutesFlow.Value)
          {
            this.workMeterMemory.SetParameterValue<float>(S4_Params.MinHourlyFlow, this.Min5MinutesFlow.Value);
            await this.myFunctions.checkedCommands.WriteMemoryAsync(this.workMeterMemory.GetParameterAddressRange(S4_Params.MinHourlyFlow), (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
          }
        }
        this.Max5MinutesFlow = new float?();
        isChecked = this.CheckBox5MinutesMaxFlowLikeFlow.IsChecked;
        if (isChecked.Value)
          this.Max5MinutesFlow = new float?(flow);
        else if (this.TextBoxHourlyMaxFlow.Text == "NaN")
          this.Max5MinutesFlow = new float?(float.NaN);
        else if (float.TryParse(this.TextBoxHourlyMaxFlow.Text, out flowOut))
          this.Max5MinutesFlow = new float?(flowOut);
        if (this.Max5MinutesFlow.HasValue)
        {
          float readMax5MinutesFlow = this.connectedMeterMemory.GetParameterValue<float>(S4_Params.MaxHourlyFlow);
          if ((double) readMax5MinutesFlow != (double) this.Max5MinutesFlow.Value)
          {
            this.workMeterMemory.SetParameterValue<float>(S4_Params.MaxHourlyFlow, this.Max5MinutesFlow.Value);
            await this.myFunctions.checkedCommands.WriteMemoryAsync(this.workMeterMemory.GetParameterAddressRange(S4_Params.MaxHourlyFlow), (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
          }
        }
      }
    }

    private async void ButtonLoadDeviceTime_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        await this.ReadAndShowDeviceTimeForCycle();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonCheckLoRaState_Click(object sender, RoutedEventArgs e)
    {
      this.RadioButtonLoRaCycle.IsChecked = new bool?(true);
      while (this.Cursor == Cursors.Wait)
        await Task.Delay(200);
      this.SetRunState();
      try
      {
        Common32BitCommands.Scenarios scenario = this.myFunctions.checkedCommands.CMDs_Device.GetCommunicationScenario(this.progress, this.cancelToken);
        await this.GetRadioStateAsync();
        TextBox textBoxOut1 = this.TextBoxOut;
        ushort? nullable = scenario.ScenarioOne;
        string textData1 = "Scenario_1: " + nullable.ToString() + Environment.NewLine;
        textBoxOut1.AppendText(textData1);
        TextBox textBoxOut2 = this.TextBoxOut;
        nullable = scenario.ScenarioTwo;
        string textData2 = "Scenario_2: " + nullable.ToString() + Environment.NewLine;
        textBoxOut2.AppendText(textData2);
        this.TextBoxOut.AppendText(Environment.NewLine + this.Next_LoRaEventData.GetHeader() + Environment.NewLine);
        this.TextBoxOut.AppendText(this.Next_LoRaEventData.GetLine() + Environment.NewLine);
        scenario = (Common32BitCommands.Scenarios) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonShowLoRaAlarmPreparation_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        S4_LoRaAlarmManager alarmManager = new S4_LoRaAlarmManager(this.myFunctions.checkedCommands, this.workMeterMemory);
        List<LoRa_AlarmEntry> loRaAlarmEntryList = await alarmManager.Read(this.progress, this.cancelTokenSource.Token);
        this.TextBoxOut.AppendText(alarmManager.GetAlarmState());
        alarmManager = (S4_LoRaAlarmManager) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void RadioButtonOnlyTime_Checked(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        await this.ReadAndShowDeviceTimeForCycle();
        DateTime nextStartTime = this.GetNextStartTime(this.DeviceDateTimeOffset.Value.DateTime, true);
        this.DatePickerStartDate.SelectedDate = new DateTime?(nextStartTime.Date);
        this.DatePickerStartDate.IsEnabled = true;
        this.TextBoxStartTime.Text = nextStartTime.ToLongTimeString();
        this.TextBoxStartTime.IsEnabled = true;
        this.TextBoxIntervalTime.Text = "01:00:00";
        this.TextBoxIntervalTime.IsEnabled = true;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void RadioButtonTime_Checked(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        await this.ReadAndShowDeviceTimeForCycle();
        DateTime nextStartTime = this.GetNextStartTime(this.DeviceDateTimeOffset.Value.DateTime, true);
        this.DatePickerStartDate.SelectedDate = new DateTime?(nextStartTime.Date);
        this.DatePickerStartDate.IsEnabled = true;
        this.TextBoxStartTime.Text = nextStartTime.ToLongTimeString();
        this.TextBoxStartTime.IsEnabled = true;
        this.TextBoxIntervalTime.Text = "Selected";
        this.TextBoxIntervalTime.IsEnabled = false;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void RadioButtonRadioCycle_Checked(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        await this.GetRadioStateAsync();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonSetDeviceTime_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        DateTime setDateTime = this.GetDateTimeFromTextBox(this.TextBoxSetTime);
        this.CalculateTimeShift();
        DateTimeOffset dateTimeOffset = await this.myFunctions.SetDeviceTime(setDateTime, this.TimeZone, this.progress, this.cancelTokenSource.Token);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonShiftDeviceTime_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        DateTime dateTime = await this.ReadDeviceDateTime();
        int timeShift = this.CalculateTimeShift();
        ushort negativeMask = 0;
        if (timeShift < 0)
        {
          negativeMask = (ushort) 32768;
          timeShift *= -1;
        }
        if (timeShift >= 32768)
          throw new Exception("Time shift out of range");
        ushort specialShiftNumber = (ushort) ((uint) negativeMask + (uint) timeShift);
        byte[] timeShiftData = BitConverter.GetBytes(specialShiftNumber);
        await this.myFunctions.myDeviceCommands.CMDs_Device.TimeShiftAsync(timeShiftData, this.progress, this.cancelToken);
        timeShiftData = (byte[]) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonResetTimeShift_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        ushort specialShiftNumber = 0;
        byte[] timeShiftData = BitConverter.GetBytes(specialShiftNumber);
        await this.myFunctions.myDeviceCommands.CMDs_Device.TimeShiftAsync(timeShiftData, this.progress, this.cancelToken);
        timeShiftData = (byte[]) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonTimeSimulation_Click(object sender, RoutedEventArgs e)
    {
      DateTime nextCycleTime = DateTime.MinValue;
      try
      {
        this.SetRunState();
        if (!int.TryParse(this.TextBoxStopSecondsBevoreStep.Text, out this.StopSecondsBeforeStep))
          throw new Exception("Illegal stop seconds");
        if (!int.TryParse(this.TextBoxWaitSecondsPerStep.Text, out this.WaitSecondsPerStep))
          throw new Exception("Illegal wait seconds");
        DateTimeOffset dateTimeOffset1 = await this.myFunctions.GetSystemDateTime(this.progress, this.cancelToken);
        this.DeviceDateTimeOffset = new DateTimeOffset?(dateTimeOffset1);
        dateTimeOffset1 = new DateTimeOffset();
        DateTimeOffset dateTimeOffset3 = this.DeviceDateTimeOffset.Value;
        TimeSpan timeSpan = dateTimeOffset3.Offset;
        this.TimeZone = (Decimal) timeSpan.Hours;
        bool fullFirmwareSupport = new FirmwareVersion(this.myFunctions.myMeters.WorkMeter.deviceIdentification.FirmwareVersion.Value) >= (object) "1.7.1 IUW";
        if (!this.DatePickerStartDate.SelectedDate.HasValue)
          throw new Exception("Start date not defined");
        nextCycleTime = this.DatePickerStartDate.SelectedDate.Value.Date;
        TimeSpan startTime;
        if (!TimeSpan.TryParse(this.TextBoxStartTime.Text, out startTime))
          throw new Exception("Illegal start time");
        nextCycleTime += startTime;
        StringBuilder am = new StringBuilder();
        am.AppendLine("PC time at start: ........ " + this.GetDateTimeString(DateTime.Now));
        am.AppendLine("Wait seconds per cycle: .. " + this.WaitSecondsPerStep.ToString());
        am.AppendLine("Stop seconds bevore: ..... " + this.StopSecondsBeforeStep.ToString());
        string intervalString = "";
        bool? isChecked;
        if (this.RadioButtonOnlyTime.IsChecked.Value)
        {
          TimeSpan intervalTime = new TimeSpan();
          if (!TimeSpan.TryParse(this.TextBoxIntervalTime.Text, out intervalTime))
            throw new Exception("Illegal intervall time span");
          intervalString = intervalTime.ToString();
        }
        else
        {
          isChecked = this.RadioButtonDay.IsChecked;
          if (isChecked.Value)
          {
            intervalString = "Day";
          }
          else
          {
            isChecked = this.RadioButtonHalfMonth.IsChecked;
            if (isChecked.Value)
            {
              intervalString = "Half month";
            }
            else
            {
              isChecked = this.RadioButtonMonth.IsChecked;
              if (isChecked.Value)
              {
                intervalString = "Month";
              }
              else
              {
                isChecked = this.RadioButtonYear.IsChecked;
                if (isChecked.Value)
                  intervalString = "Year";
                else if (this.IsRadioControlled)
                  intervalString = "LoRa controlled";
              }
            }
          }
        }
        am.AppendLine("Interval time: ........... " + intervalString);
        if (this.IsWriteProtected)
        {
          this.NLog_cl.Trace("Protected! Interval:" + intervalString);
          am.AppendLine();
          am.AppendLine("*** Device is protected. TimeShift is used. No volume simulation. ***");
        }
        else
        {
          this.NLog_cl.Trace("Not protected! Interval:" + intervalString);
          if (this.IsRadioControlled)
          {
            S4_SystemState iuwState = await this.myFunctions.GetDeviceState(this.progress, this.cancelToken);
            if (iuwState.DeviceMode != S4_DeviceModes.RadioTestRadioSimulation)
            {
              if (!fullFirmwareSupport)
                throw new Exception("Radio simulation without write protection not possible by this firmware. Use newer firmware.");
              am.AppendLine("Radio simulation without protection need RadioSimulation Mode. This mode is automatical activated now.");
              await this.myFunctions.SetModeAsync(this.progress, this.cancelTokenSource.Token, (Enum) S4_DeviceModes.RadioTestRadioSimulation, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
            }
            iuwState = (S4_SystemState) null;
          }
        }
        am.AppendLine();
        am.AppendLine("Meter data before simulation:");
        S4_CurrentData deviceData = await this.myFunctions.checkedCommands.ReadCurrentDataAsync(this.progress, this.cancelTokenSource.Token);
        am.AppendLine(deviceData.ToString());
        am.AppendLine();
        await this.ReadMinMaxValues();
        int num;
        if (!this.IsWriteProtected & fullFirmwareSupport)
        {
          isChecked = this.RadioButton_wMBusNoStartCycle.IsChecked;
          num = isChecked.Value ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          this.NLog_cl.Trace("Clear first_day_flag");
          await this.myFunctions.checkedCommands.Delete_wMBus_first_day_flag(this.progress, this.cancelTokenSource.Token, this.workMeterMemory);
        }
        S4_LoRaAlarmManager LoRaAlarmManager = (S4_LoRaAlarmManager) null;
        isChecked = this.CheckBoxShowLoRaAlarmChanges.IsChecked;
        if (isChecked.Value)
        {
          LoRaAlarmManager = new S4_LoRaAlarmManager(this.myFunctions.checkedCommands, this.workMeterMemory);
          List<LoRa_AlarmEntry> loRaAlarmEntryList = await LoRaAlarmManager.Read(this.progress, this.cancelTokenSource.Token);
          am.Append(LoRaAlarmManager.GetAlarmState());
          am.AppendLine();
        }
        S4_LoggerManager LoggerManager = (S4_LoggerManager) null;
        isChecked = this.CheckBoxShowEventLoggerChanges.IsChecked;
        LoggerNames loggerNames;
        if (isChecked.Value)
        {
          LoggerManager = new S4_LoggerManager();
          S4_LoggerManager s4LoggerManager = LoggerManager;
          loggerNames = LoggerNames.Event;
          string loggerName = loggerNames.ToString();
          S4_DeviceCommandsNFC checkedCommands = this.myFunctions.checkedCommands;
          ProgressHandler progress = this.progress;
          CancellationToken token = this.cancelTokenSource.Token;
          IEnumerable enumerable = await s4LoggerManager.ReadLoggerDataAsync(loggerName, checkedCommands, progress, token);
          am.AppendLine();
        }
        int simulatonStep = 0;
        while (true)
        {
          this.TextBoxOut.AppendText(am.ToString());
          am.Clear();
          if (this.Warnings.Length > 0)
          {
            this.TextBoxOut.AppendText(this.Warnings.ToString());
            this.Warnings.Clear();
          }
          DateTime preSetTime = nextCycleTime.AddSeconds((double) (this.StopSecondsBeforeStep * -1));
          DateTime afterWaitTime = preSetTime.AddSeconds((double) this.WaitSecondsPerStep);
          string waitTimeSpan = (preSetTime.ToString("mm:ss") + "-" + afterWaitTime.ToString("mm:ss")).PadLeft(12);
          string waitTimeSpanHeader = "Work time".PadLeft(12);
          if (!this.IsWriteProtected)
          {
            this.NLog_cl.Trace("SetDeviceTime");
            DateTimeOffset dateTimeOffset4 = await this.myFunctions.SetDeviceTime(preSetTime, this.TimeZone, this.progress, this.cancelTokenSource.Token);
          }
          else
          {
            this.NLog_cl.Trace("ShiftDeviceTime");
            DateTimeOffset dateTimeOffset2 = await this.myFunctions.GetSystemDateTime(this.progress, this.cancelToken);
            this.DeviceDateTimeOffset = new DateTimeOffset?(dateTimeOffset2);
            dateTimeOffset2 = new DateTimeOffset();
            ref DateTime local = ref preSetTime;
            dateTimeOffset3 = this.DeviceDateTimeOffset.Value;
            DateTime dateTime = dateTimeOffset3.DateTime;
            timeSpan = local.Subtract(dateTime);
            int remainingJumpSeconds = (int) timeSpan.TotalSeconds;
            while (remainingJumpSeconds > 0 && !this.cancelToken.IsCancellationRequested)
            {
              ushort singleShift = remainingJumpSeconds <= (int) short.MaxValue ? (ushort) remainingJumpSeconds : (ushort) short.MaxValue;
              remainingJumpSeconds -= (int) singleShift;
              --singleShift;
              if (singleShift > (ushort) 2)
              {
                this.NLog_cl.Trace("Shift seconds: " + singleShift.ToString());
                byte[] timeShiftData = BitConverter.GetBytes(singleShift);
                await this.myFunctions.myDeviceCommands.CMDs_Device.TimeShiftAsync(timeShiftData, this.progress, this.cancelToken);
                timeShiftData = (byte[]) null;
              }
            }
          }
          this.TextBoxOut.AppendText(this.GetDateTimeString(preSetTime) + " - Wait " + this.WaitSecondsPerStep.ToString() + "s ...");
          this.TextBoxOut.ScrollToEnd();
          this.NLog_cl.Trace("Wait seconds: " + this.WaitSecondsPerStep.ToString());
          await Task.Delay(this.WaitSecondsPerStep * 1000, this.cancelTokenSource.Token);
          this.Next_LoRaEventData = (LoRaEventData) null;
          DateTime nextDateAndTime;
          if (this.IsRadioControlled)
          {
            await this.UpdateLoRaValuesAsync(afterWaitTime);
            nextDateAndTime = this.Next_LoRaEventData.NextSendTime;
          }
          else
            nextDateAndTime = this.GetNextStartTime(nextCycleTime);
          bool isHourStep = false;
          isChecked = this.CheckBoxUseHourSteps.IsChecked;
          DateTime dateTime1;
          if (isChecked.Value)
          {
            DateTime deviceTime = preSetTime.AddSeconds((double) this.WaitSecondsPerStep);
            dateTime1 = deviceTime.Date;
            DateTime nextHourTime = dateTime1.AddHours((double) (deviceTime.Hour + 1));
            if (nextHourTime < nextDateAndTime)
            {
              nextDateAndTime = nextHourTime;
              isHourStep = true;
            }
          }
          bool is5MinutesStep = false;
          isChecked = this.CheckBoxUse5MinutesSteps.IsChecked;
          if (isChecked.Value)
          {
            DateTime deviceTime = preSetTime.AddSeconds((double) this.WaitSecondsPerStep);
            DateTime lastHourTime = new DateTime(deviceTime.Year, deviceTime.Month, deviceTime.Day, deviceTime.Hour, 0, 0);
            int offsetMinutes = (deviceTime.Minute / 5 + 1) * 5;
            DateTime next5MinutesTime = lastHourTime.AddMinutes((double) offsetMinutes);
            if (next5MinutesTime < nextDateAndTime)
            {
              nextDateAndTime = next5MinutesTime;
              is5MinutesStep = true;
            }
          }
          deviceData = await this.myFunctions.checkedCommands.ReadCurrentDataAsync(this.progress, this.cancelTokenSource.Token);
          string str1 = simulatonStep.ToString("d04");
          dateTime1 = DateTime.Now;
          string str2 = dateTime1.ToString("HH:mm:ss");
          string StepAndPC_time = str1 + " " + str2 + " ";
          if (!this.IsWriteProtected)
          {
            double VolumeIncrement = double.NaN;
            isChecked = this.RadioButtonVolumeIncrementLikeDefined.IsChecked;
            if (isChecked.Value)
            {
              if (!double.TryParse(this.TextBoxVolumeIncrement.Text, out VolumeIncrement))
                break;
            }
            else
            {
              isChecked = this.RadioButtonVolumeIncrementFromFlow.IsChecked;
              if (isChecked.Value)
              {
                timeSpan = nextDateAndTime - nextCycleTime;
                int timeShiftSeconds = (int) (timeSpan.TotalSeconds - (double) this.WaitSecondsPerStep);
                VolumeIncrement = (double) deviceData.Flow * (double) timeShiftSeconds / 3600.0;
              }
              else
                goto label_78;
            }
            if (!double.IsNaN(VolumeIncrement) && this.counterRange != null)
            {
              this.NLog_cl.Trace("Write simulated volume");
              await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(this.counterRange, (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
              double volQmSum = this.myFunctions.myMeters.WorkMeter.meterMemory.GetParameterValue<double>(S4_Params.volQmSum);
              double volQmPos = this.myFunctions.myMeters.WorkMeter.meterMemory.GetParameterValue<double>(S4_Params.volQmPos);
              double volQmNeg = this.myFunctions.myMeters.WorkMeter.meterMemory.GetParameterValue<double>(S4_Params.volQmNeg);
              volQmSum += VolumeIncrement;
              if (VolumeIncrement > 0.0)
                volQmPos += VolumeIncrement;
              else
                volQmNeg += VolumeIncrement;
              this.myFunctions.myMeters.WorkMeter.meterMemory.SetParameterValue<double>(S4_Params.volQmSum, volQmSum);
              this.myFunctions.myMeters.WorkMeter.meterMemory.SetParameterValue<double>(S4_Params.volQmPos, volQmPos);
              this.myFunctions.myMeters.WorkMeter.meterMemory.SetParameterValue<double>(S4_Params.volQmNeg, volQmNeg);
              await this.myFunctions.myDeviceCommands.CommonNfcCommands.WriteMemoryAsync(this.counterRange, (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
            }
            await this.WriteMinMaxValues(deviceData.Flow);
          }
          if (simulatonStep % 30 == 0)
          {
            if (this.Next_LoRaEventData != null)
              am.AppendLine("Step PC_time  " + deviceData.GetHeader(waitTimeSpanHeader) + " | " + this.Next_LoRaEventData.GetHeader());
            else
              am.AppendLine("Step PC_time  " + deviceData.GetHeader(waitTimeSpanHeader));
          }
          if (is5MinutesStep)
            am.AppendLine(StepAndPC_time + deviceData.GetLine(waitTimeSpan) + " | " + this.GetDateTimeString(afterWaitTime).PadLeft(20) + this.GetDateTimeString(nextDateAndTime).PadLeft(20) + " -> next 5 minutes stop");
          else if (isHourStep)
            am.AppendLine(StepAndPC_time + deviceData.GetLine(waitTimeSpan) + " | " + this.GetDateTimeString(afterWaitTime).PadLeft(20) + this.GetDateTimeString(nextDateAndTime).PadLeft(20) + " -> next hour stop");
          else if (this.Next_LoRaEventData != null)
            am.AppendLine(StepAndPC_time + deviceData.GetLine(waitTimeSpan) + " | " + this.Next_LoRaEventData.GetLine());
          else
            am.AppendLine(StepAndPC_time + deviceData.GetLine(waitTimeSpan));
          if (LoRaAlarmManager != null)
          {
            List<LoRa_AlarmEntry> loRaAlarmEntryList = await LoRaAlarmManager.Read(this.progress, this.cancelTokenSource.Token);
            string changes = LoRaAlarmManager.GetAlarmChanges();
            if (changes.Length > 0)
              am.Append(changes);
            changes = (string) null;
          }
          if (LoggerManager != null)
          {
            S4_LoggerManager s4LoggerManager = LoggerManager;
            loggerNames = LoggerNames.Event;
            string loggerName = loggerNames.ToString();
            S4_DeviceCommandsNFC checkedCommands = this.myFunctions.checkedCommands;
            ProgressHandler progress = this.progress;
            CancellationToken token = this.cancelTokenSource.Token;
            IEnumerable enumerable = await s4LoggerManager.ReadLoggerDataAsync(loggerName, checkedCommands, progress, token);
            string changes = LoggerManager.GetEventLoggerChanges();
            if (changes.Length > 0)
              am.Append(changes);
            changes = (string) null;
          }
          ++simulatonStep;
          isChecked = this.CheckBoxSingleStepsMode.IsChecked;
          if (!isChecked.Value)
          {
            nextCycleTime = nextDateAndTime;
            this.TextBoxOut.Undo();
            waitTimeSpan = (string) null;
            waitTimeSpanHeader = (string) null;
            StepAndPC_time = (string) null;
          }
          else
            goto label_111;
        }
        throw new Exception("Illegal volume increment");
label_78:
        throw new Exception("Internal error. Interval not defined by radio button");
label_111:
        this.TextBoxOut.Undo();
        am.AppendLine();
        am.Append("Stop by single step simulation.");
        this.TextBoxOut.AppendText(am.ToString());
        this.TextBoxOut.AppendText("Undo dummy");
        startTime = new TimeSpan();
        am = (StringBuilder) null;
        intervalString = (string) null;
        deviceData = (S4_CurrentData) null;
        LoRaAlarmManager = (S4_LoRaAlarmManager) null;
        LoggerManager = (S4_LoggerManager) null;
      }
      catch (TaskCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        nextCycleTime = this.GetNextStartTime(nextCycleTime);
        this.DatePickerStartDate.DisplayDate = nextCycleTime.Date;
        this.TextBoxStartTime.Text = nextCycleTime.ToString("HH:mm:ss");
        this.ButtonStartTimeSimulation.IsEnabled = true;
        this.SetStopState();
        this.DatePickerStartDate.SelectedDate = new DateTime?(nextCycleTime);
        this.TextBoxOut.Undo();
        this.TextBoxOut.AppendText(Environment.NewLine + "Simulation stopped" + Environment.NewLine + Environment.NewLine);
      }
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      if (this.cancelTokenSource == null)
        return;
      this.cancelTokenSource.Cancel();
    }

    private void ButtonClear_Click(object sender, RoutedEventArgs e) => this.TextBoxOut.Clear();

    private void TextBoxSetTime_LostFocus(object sender, RoutedEventArgs e)
    {
    }

    private DateTime GetNextStartTime(DateTime startTimeBefore, bool rounded = false)
    {
      int.Parse(this.TextBoxWaitSecondsPerStep.Text);
      DateTime nextStartTime = startTimeBefore;
      if (this.RadioButtonOnlyTime.IsChecked.Value)
      {
        if (rounded)
        {
          TimeSpan result;
          nextStartTime = !TimeSpan.TryParse(this.TextBoxIntervalTime.Text, out result) || result.TotalSeconds == 3600.0 ? new DateTime(startTimeBefore.Year, startTimeBefore.Month, startTimeBefore.Day, startTimeBefore.Hour, 0, 0).AddHours(1.0) : startTimeBefore.Add(result);
        }
        else
        {
          TimeSpan result;
          if (!TimeSpan.TryParse(this.TextBoxIntervalTime.Text, out result))
            throw new Exception("Illegal interval time");
          nextStartTime = startTimeBefore.Add(result);
        }
      }
      else if (this.RadioButtonDay.IsChecked.Value)
        nextStartTime = !rounded ? startTimeBefore.AddDays(1.0) : startTimeBefore.Date.AddDays(1.0);
      else if (this.RadioButtonHalfMonth.IsChecked.Value)
      {
        if (rounded)
        {
          nextStartTime = new DateTime(startTimeBefore.Year, startTimeBefore.Month, 16);
          if (nextStartTime <= startTimeBefore)
            nextStartTime = new DateTime(startTimeBefore.Year, startTimeBefore.Month, 1).AddMonths(1);
        }
        else if (startTimeBefore.Day >= 16)
        {
          nextStartTime = startTimeBefore.AddDays(-15.0).AddMonths(1);
        }
        else
        {
          nextStartTime = startTimeBefore.AddDays(15.0);
          if (nextStartTime.Month != startTimeBefore.Month)
            nextStartTime = new DateTime(startTimeBefore.Year, startTimeBefore.Month, 1).AddMonths(1);
        }
      }
      else if (this.RadioButtonMonth.IsChecked.Value)
        nextStartTime = !rounded ? startTimeBefore.AddMonths(1) : new DateTime(startTimeBefore.Year, startTimeBefore.Month, 1).AddMonths(1);
      else if (this.RadioButtonYear.IsChecked.Value)
        nextStartTime = !rounded ? startTimeBefore.AddYears(1) : new DateTime(startTimeBefore.Year, 1, 1).AddYears(1);
      return nextStartTime;
    }

    private int CalculateTimeShift()
    {
      DateTime dateTime = this.DeviceDateTimeOffset.Value.DateTime;
      int totalSeconds = (int) this.GetDateTimeFromTextBox(this.TextBoxSetTime).Subtract(dateTime).TotalSeconds;
      this.TextBoxShiftSeconds.Text = totalSeconds.ToString();
      return totalSeconds;
    }

    private async Task GetRadioStateAsync()
    {
      await this.ReadAndShowDeviceTimeForCycle();
      await this.UpdateLoRaValuesAsync(this.DeviceDateTimeOffset.Value.DateTime);
      this.DatePickerStartDate.SelectedDate = new DateTime?(this.Next_LoRaEventData.NextSendTime.Date);
      this.DatePickerStartDate.IsEnabled = false;
      this.TextBoxStartTime.Text = this.Next_LoRaEventData.NextSendTime.ToLongTimeString();
      this.TextBoxStartTime.IsEnabled = false;
      this.TextBoxIntervalTime.Text = "Radio controlled";
      this.TextBoxIntervalTime.IsEnabled = false;
    }

    private async Task ReadAndShowDeviceTimeForCycle()
    {
      DateTime deviceDateTime = await this.ReadDeviceDateTime();
      string cycle = "Radio controlled";
      if (this.RadioButtonOnlyTime.IsChecked.Value)
      {
        cycle = "Time";
      }
      else
      {
        bool? isChecked = this.RadioButtonDay.IsChecked;
        if (isChecked.Value)
        {
          cycle = "Day";
        }
        else
        {
          isChecked = this.RadioButtonHalfMonth.IsChecked;
          if (isChecked.Value)
          {
            cycle = "Half month";
          }
          else
          {
            isChecked = this.RadioButtonMonth.IsChecked;
            if (isChecked.Value)
            {
              cycle = "Month";
            }
            else
            {
              isChecked = this.RadioButtonYear.IsChecked;
              if (isChecked.Value)
                cycle = "Year";
            }
          }
        }
      }
      this.TextBoxOut.Text = "Cycle setup: " + cycle + Environment.NewLine + "Device time: " + this.GetDateTimeString(this.DeviceDateTimeOffset.Value.DateTime) + Environment.NewLine + Environment.NewLine;
      DateTime setDateTime = deviceDateTime.Date.Add(new TimeSpan(23, 59, 58));
      this.SetDateTimeToTextBox(this.TextBoxSetTime, setDateTime);
      this.CalculateTimeShift();
      cycle = (string) null;
    }

    private async Task UpdateLoRaValuesAsync(DateTime deviceTime)
    {
      this.Next_LoRaEventData = new LoRaEventData(this.NLog_cl);
      this.NLog_cl.Debug("UpdateLoRaValues");
      S4_DeviceMemory workMeterMemory = this.myFunctions.myMeters.WorkMeter.meterMemory;
      AddressRange radio_transmitter_Range = workMeterMemory.GetParameterAddressRange(S4_Params.radio_transmitter);
      workMeterMemory.GarantMemoryAvailable(radio_transmitter_Range);
      await this.myCommands.ReadMemoryAsync(radio_transmitter_Range, (DeviceMemory) workMeterMemory, this.progress, this.cancelToken);
      byte[] radio_transmitter_Array = workMeterMemory.GetData(radio_transmitter_Range);
      if (!this.RadioButtonLoRaCycle.IsChecked.Value)
      {
        this.Next_LoRaEventData.SetBy_radio_transmitter(deviceTime, radio_transmitter_Array);
        workMeterMemory = (S4_DeviceMemory) null;
        radio_transmitter_Range = (AddressRange) null;
        radio_transmitter_Array = (byte[]) null;
      }
      else
      {
        this.Next_LoRaEventData.SetBy_radio_transmitter(deviceTime, radio_transmitter_Array, true);
        AddressRange time_to_send_data_Range = workMeterMemory.GetParameterAddressRange(S4_Params.time_to_send_data);
        workMeterMemory.GarantMemoryAvailable(time_to_send_data_Range);
        await this.myCommands.ReadMemoryAsync(time_to_send_data_Range, (DeviceMemory) workMeterMemory, this.progress, this.cancelToken);
        byte[] time_to_send_data_Array = workMeterMemory.GetData(time_to_send_data_Range);
        this.Next_LoRaEventData.SetBy_time_to_send_data(deviceTime, time_to_send_data_Array);
        time_to_send_data_Range = (AddressRange) null;
        time_to_send_data_Array = (byte[]) null;
        workMeterMemory = (S4_DeviceMemory) null;
        radio_transmitter_Range = (AddressRange) null;
        radio_transmitter_Array = (byte[]) null;
      }
    }

    private async Task<DateTime> ReadDeviceDateTime()
    {
      DateTimeOffset dateTimeOffset = await this.myFunctions.GetSystemDateTime(this.progress, this.cancelToken);
      this.DeviceDateTimeOffset = new DateTimeOffset?(dateTimeOffset);
      dateTimeOffset = new DateTimeOffset();
      this.TimeZone = (Decimal) this.DeviceDateTimeOffset.Value.Offset.Hours;
      TextBox textBoxDeviceTime = this.TextBoxDeviceTime;
      DateTimeOffset dateTimeOffset1 = this.DeviceDateTimeOffset.Value;
      DateTime dateTime = dateTimeOffset1.DateTime;
      this.SetDateTimeToTextBox(textBoxDeviceTime, dateTime);
      dateTimeOffset1 = this.DeviceDateTimeOffset.Value;
      return dateTimeOffset1.DateTime;
    }

    private DateTime GetDateTimeFromTextBox(TextBox textBox)
    {
      string[] strArray = textBox.Text.Split(' ');
      DateTime dateTimeFromTextBox = DateTime.Parse(strArray[0]);
      TimeSpan timeSpan = TimeSpan.Parse(strArray[1]);
      dateTimeFromTextBox = dateTimeFromTextBox.Add(timeSpan);
      return dateTimeFromTextBox;
    }

    private void SetDateTimeToTextBox(TextBox textBox, DateTime dateTime)
    {
      textBox.Text = this.GetDateTimeString(dateTime);
    }

    private string GetDateTimeString(DateTime theDateTime)
    {
      return theDateTime.ToShortDateString() + " " + theDateTime.ToLongTimeString();
    }

    private void ShowFlowLimits()
    {
      this.TextBoxQmax.Text = this.workMeterMemory.GetParameterValue<float>(S4_Params.maxPositivFlow).ToString();
      this.TextBoxQ4.Text = this.workMeterMemory.GetParameterValue<float>(S4_Params.overloadFlowrateQ4).ToString();
      this.TextBoxQ3.Text = this.workMeterMemory.GetParameterValue<float>(S4_Params.permanentFlowrateQ3).ToString();
      this.TextBoxQ2.Text = this.workMeterMemory.GetParameterValue<float>(S4_Params.transitionalFlowrateQ2).ToString();
      this.TextBoxQ1.Text = this.workMeterMemory.GetParameterValue<float>(S4_Params.minimumFlowrateQ1).ToString();
      this.TextBoxQmin.Text = this.workMeterMemory.GetParameterValue<float>(S4_Params.minPositivFlow).ToString();
    }

    private async void ButtonReadFlowLimits_Click(object sender, RoutedEventArgs e)
    {
      if (this.flowLimitsRangeQx == null)
        return;
      await this.myFunctions.checkedCommands.ReadMemoryAsync(this.flowLimitsRangeQminMax, (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
      await this.myFunctions.checkedCommands.ReadMemoryAsync(this.flowLimitsRangeQx, (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
      this.ShowFlowLimits();
      this.TextBoxOut.AppendText("Read flow limits done." + Environment.NewLine);
    }

    private void ButtonSetDefaultFlowLimits_Click(object sender, RoutedEventArgs e)
    {
      float parameterValue1 = this.workMeterMemory.GetParameterValue<float>(S4_Params.maxPositivFlow);
      float parameterValue2 = this.workMeterMemory.GetParameterValue<float>(S4_Params.minPositivFlow);
      float num1 = parameterValue1 - parameterValue2;
      float num2 = (float) Math.Round((double) parameterValue2 + (double) num1 * 0.01, 3);
      float num3 = (float) Math.Round((double) parameterValue2 + (double) num1 * 0.05, 3);
      float num4 = (float) Math.Round((double) parameterValue2 + (double) num1 * 0.2, 3);
      float num5 = (float) Math.Round((double) parameterValue2 + (double) num1 * 0.9, 3);
      this.TextBoxQ1.Text = num2.ToString();
      this.TextBoxQ2.Text = num3.ToString();
      this.TextBoxQ3.Text = num4.ToString();
      this.TextBoxQ4.Text = num5.ToString();
    }

    private async void ButtonWriteFlowLimits_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.workMeterMemory.SetParameterValue<float>(S4_Params.maxPositivFlow, float.Parse(this.TextBoxQmax.Text));
        this.workMeterMemory.SetParameterValue<float>(S4_Params.overloadFlowrateQ4, float.Parse(this.TextBoxQ4.Text));
        this.workMeterMemory.SetParameterValue<float>(S4_Params.permanentFlowrateQ3, float.Parse(this.TextBoxQ3.Text));
        this.workMeterMemory.SetParameterValue<float>(S4_Params.transitionalFlowrateQ2, float.Parse(this.TextBoxQ2.Text));
        this.workMeterMemory.SetParameterValue<float>(S4_Params.minimumFlowrateQ1, float.Parse(this.TextBoxQ1.Text));
        this.workMeterMemory.SetParameterValue<float>(S4_Params.minPositivFlow, float.Parse(this.TextBoxQmin.Text));
        await this.myFunctions.checkedCommands.WriteMemoryAsync(this.flowLimitsRangeQminMax, (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
        await this.myFunctions.checkedCommands.WriteMemoryAsync(this.flowLimitsRangeQx, (DeviceMemory) this.workMeterMemory, this.progress, this.cancelTokenSource.Token);
        this.ShowFlowLimits();
        this.TextBoxOut.AppendText("Write flow limits done." + Environment.NewLine);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void CheckBoxHourlyMaxFlowLikeFlow_Checked(object sender, RoutedEventArgs e)
    {
      this.TextBoxHourlyMaxFlow.IsEnabled = false;
      this.TextBoxHourlyMaxFlow.Clear();
    }

    private void CheckBoxHourlyMaxFlowLikeFlow_Unchecked(object sender, RoutedEventArgs e)
    {
      this.TextBoxHourlyMaxFlow.IsEnabled = true;
    }

    private void CheckBoxHourlyMinFlowLikeFlow_Checked(object sender, RoutedEventArgs e)
    {
      this.TextBoxHourlyMinFlow.IsEnabled = false;
      this.TextBoxHourlyMinFlow.Clear();
    }

    private void CheckBoxHourlyMinFlowLikeFlow_Unchecked(object sender, RoutedEventArgs e)
    {
      this.TextBoxHourlyMinFlow.IsEnabled = true;
    }

    private void CheckBox5MinutesMaxFlowLikeFlow_Checked(object sender, RoutedEventArgs e)
    {
      this.TextBox5MinutesMaxFlow.IsEnabled = false;
      this.TextBox5MinutesMaxFlow.Clear();
    }

    private void CheckBox5MinutesMaxFlowLikeFlow_Unchecked(object sender, RoutedEventArgs e)
    {
      this.TextBox5MinutesMaxFlow.IsEnabled = true;
    }

    private void CheckBox5MinutesMinFlowLikeFlow_Checked(object sender, RoutedEventArgs e)
    {
      this.TextBox5MinutesMinFlow.IsEnabled = false;
      this.TextBox5MinutesMinFlow.Clear();
    }

    private void CheckBox5MinutesMinFlowLikeFlow_Unchecked(object sender, RoutedEventArgs e)
    {
      this.TextBox5MinutesMinFlow.IsEnabled = true;
    }

    private void ButtonSetAllToNotANumber_Click(object sender, RoutedEventArgs e)
    {
      this.CheckBoxHourlyMaxFlowLikeFlow.IsChecked = new bool?(false);
      this.TextBoxHourlyMaxFlow.Text = "NaN";
      this.CheckBoxHourlyMinFlowLikeFlow.IsChecked = new bool?(false);
      this.TextBoxHourlyMinFlow.Text = "NaN";
      this.CheckBox5MinutesMaxFlowLikeFlow.IsChecked = new bool?(false);
      this.TextBox5MinutesMaxFlow.Text = "NaN";
      this.CheckBox5MinutesMinFlowLikeFlow.IsChecked = new bool?(false);
      this.TextBox5MinutesMinFlow.Text = "NaN";
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_timelapsesimulator.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closing);
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.StackPanelSimulation = (StackPanel) target;
          break;
        case 3:
          this.TextBoxSimulatedFlow = (TextBox) target;
          break;
        case 4:
          this.ButtonSetSimulatedFlow = (Button) target;
          this.ButtonSetSimulatedFlow.Click += new RoutedEventHandler(this.ButtonSetSimulatedFlow_Click);
          break;
        case 5:
          this.TextBoxStopSecondsBevoreStep = (TextBox) target;
          break;
        case 6:
          this.TextBoxWaitSecondsPerStep = (TextBox) target;
          break;
        case 7:
          this.DatePickerStartDate = (DatePicker) target;
          break;
        case 8:
          this.TextBoxStartTime = (TextBox) target;
          break;
        case 9:
          this.TextBoxIntervalTime = (TextBox) target;
          break;
        case 10:
          this.RadioButtonOnlyTime = (RadioButton) target;
          this.RadioButtonOnlyTime.Checked += new RoutedEventHandler(this.RadioButtonOnlyTime_Checked);
          break;
        case 11:
          this.RadioButtonDay = (RadioButton) target;
          this.RadioButtonDay.Checked += new RoutedEventHandler(this.RadioButtonTime_Checked);
          break;
        case 12:
          this.RadioButtonHalfMonth = (RadioButton) target;
          this.RadioButtonHalfMonth.Checked += new RoutedEventHandler(this.RadioButtonTime_Checked);
          break;
        case 13:
          this.RadioButtonMonth = (RadioButton) target;
          this.RadioButtonMonth.Checked += new RoutedEventHandler(this.RadioButtonTime_Checked);
          break;
        case 14:
          this.RadioButtonYear = (RadioButton) target;
          this.RadioButtonYear.Checked += new RoutedEventHandler(this.RadioButtonTime_Checked);
          break;
        case 15:
          this.RadioButtonLoRaCycle = (RadioButton) target;
          this.RadioButtonLoRaCycle.Checked += new RoutedEventHandler(this.RadioButtonRadioCycle_Checked);
          break;
        case 16:
          this.RadioButtonTransmitterCycle = (RadioButton) target;
          this.RadioButtonTransmitterCycle.Checked += new RoutedEventHandler(this.RadioButtonRadioCycle_Checked);
          break;
        case 17:
          this.RadioButton_wMBusNoStartCycle = (RadioButton) target;
          this.RadioButton_wMBusNoStartCycle.Checked += new RoutedEventHandler(this.RadioButtonRadioCycle_Checked);
          break;
        case 18:
          this.CheckBoxUse5MinutesSteps = (CheckBox) target;
          break;
        case 19:
          this.CheckBoxUseHourSteps = (CheckBox) target;
          break;
        case 20:
          this.CheckBoxIgnoreLoRa24Hbreaks = (CheckBox) target;
          break;
        case 21:
          this.CheckBoxSingleStepsMode = (CheckBox) target;
          break;
        case 22:
          this.ButtonStartTimeSimulation = (Button) target;
          this.ButtonStartTimeSimulation.Click += new RoutedEventHandler(this.ButtonTimeSimulation_Click);
          break;
        case 23:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 24:
          this.ButtonClear = (Button) target;
          this.ButtonClear.Click += new RoutedEventHandler(this.ButtonClear_Click);
          break;
        case 25:
          this.TabControlFunctions = (TabControl) target;
          break;
        case 26:
          this.TextBoxVolumeIncrement = (TextBox) target;
          break;
        case 27:
          this.RadioButtonVolumeIncrementLikeDefined = (RadioButton) target;
          this.RadioButtonVolumeIncrementLikeDefined.Checked += new RoutedEventHandler(this.RadioButtonVolumeIncrementLikeDefined_Checked);
          this.RadioButtonVolumeIncrementLikeDefined.Unchecked += new RoutedEventHandler(this.RadioButtonVolumeIncrementLikeDefined_Unchecked);
          break;
        case 28:
          this.RadioButtonVolumeIncrementFromFlow = (RadioButton) target;
          break;
        case 29:
          this.GroupBoxHourlyMaxFlow = (GroupBox) target;
          break;
        case 30:
          this.CheckBoxHourlyMaxFlowLikeFlow = (CheckBox) target;
          this.CheckBoxHourlyMaxFlowLikeFlow.Checked += new RoutedEventHandler(this.CheckBoxHourlyMaxFlowLikeFlow_Checked);
          this.CheckBoxHourlyMaxFlowLikeFlow.Unchecked += new RoutedEventHandler(this.CheckBoxHourlyMaxFlowLikeFlow_Unchecked);
          break;
        case 31:
          this.TextBoxHourlyMaxFlow = (TextBox) target;
          break;
        case 32:
          this.GroupBoxHourlyMinFlow = (GroupBox) target;
          break;
        case 33:
          this.CheckBoxHourlyMinFlowLikeFlow = (CheckBox) target;
          this.CheckBoxHourlyMinFlowLikeFlow.Checked += new RoutedEventHandler(this.CheckBoxHourlyMinFlowLikeFlow_Checked);
          this.CheckBoxHourlyMinFlowLikeFlow.Unchecked += new RoutedEventHandler(this.CheckBoxHourlyMinFlowLikeFlow_Unchecked);
          break;
        case 34:
          this.TextBoxHourlyMinFlow = (TextBox) target;
          break;
        case 35:
          this.GroupBox5MinutesMaxFlow = (GroupBox) target;
          break;
        case 36:
          this.CheckBox5MinutesMaxFlowLikeFlow = (CheckBox) target;
          this.CheckBox5MinutesMaxFlowLikeFlow.Checked += new RoutedEventHandler(this.CheckBox5MinutesMaxFlowLikeFlow_Checked);
          this.CheckBox5MinutesMaxFlowLikeFlow.Unchecked += new RoutedEventHandler(this.CheckBox5MinutesMaxFlowLikeFlow_Unchecked);
          break;
        case 37:
          this.TextBox5MinutesMaxFlow = (TextBox) target;
          break;
        case 38:
          this.GroupBox5MinutesMinFlow = (GroupBox) target;
          break;
        case 39:
          this.CheckBox5MinutesMinFlowLikeFlow = (CheckBox) target;
          this.CheckBox5MinutesMinFlowLikeFlow.Checked += new RoutedEventHandler(this.CheckBox5MinutesMinFlowLikeFlow_Checked);
          this.CheckBox5MinutesMinFlowLikeFlow.Unchecked += new RoutedEventHandler(this.CheckBox5MinutesMinFlowLikeFlow_Unchecked);
          break;
        case 40:
          this.TextBox5MinutesMinFlow = (TextBox) target;
          break;
        case 41:
          this.ButtonSetAllToNotANumber = (Button) target;
          this.ButtonSetAllToNotANumber.Click += new RoutedEventHandler(this.ButtonSetAllToNotANumber_Click);
          break;
        case 42:
          this.TabItemState = (TabItem) target;
          break;
        case 43:
          this.TextBoxQmax = (TextBox) target;
          break;
        case 44:
          this.TextBoxQ4 = (TextBox) target;
          break;
        case 45:
          this.TextBoxQ3 = (TextBox) target;
          break;
        case 46:
          this.TextBoxQ2 = (TextBox) target;
          break;
        case 47:
          this.TextBoxQ1 = (TextBox) target;
          break;
        case 48:
          this.TextBoxQmin = (TextBox) target;
          break;
        case 49:
          this.ButtonReadFlowLimits = (Button) target;
          this.ButtonReadFlowLimits.Click += new RoutedEventHandler(this.ButtonReadFlowLimits_Click);
          break;
        case 50:
          this.ButtonSetDefaultFlowLimits = (Button) target;
          this.ButtonSetDefaultFlowLimits.Click += new RoutedEventHandler(this.ButtonSetDefaultFlowLimits_Click);
          break;
        case 51:
          this.ButtonWriteFlowLimits = (Button) target;
          this.ButtonWriteFlowLimits.Click += new RoutedEventHandler(this.ButtonWriteFlowLimits_Click);
          break;
        case 52:
          this.ButtonCheckLoRaState = (Button) target;
          this.ButtonCheckLoRaState.Click += new RoutedEventHandler(this.ButtonCheckLoRaState_Click);
          break;
        case 53:
          this.ButtonShowLoRaAlarmPreparation = (Button) target;
          this.ButtonShowLoRaAlarmPreparation.Click += new RoutedEventHandler(this.ButtonShowLoRaAlarmPreparation_Click);
          break;
        case 54:
          this.TabItemManual = (TabItem) target;
          break;
        case 55:
          this.TextBoxDeviceTime = (TextBox) target;
          break;
        case 56:
          this.ButtonLoadDeviceTime = (Button) target;
          this.ButtonLoadDeviceTime.Click += new RoutedEventHandler(this.ButtonLoadDeviceTime_Click);
          break;
        case 57:
          this.TextBoxSetTime = (TextBox) target;
          this.TextBoxSetTime.LostFocus += new RoutedEventHandler(this.TextBoxSetTime_LostFocus);
          break;
        case 58:
          this.TextBoxShiftSeconds = (TextBox) target;
          break;
        case 59:
          this.ButtonSetDeviceTime = (Button) target;
          this.ButtonSetDeviceTime.Click += new RoutedEventHandler(this.ButtonSetDeviceTime_Click);
          break;
        case 60:
          this.ButtonShiftDeviceTime = (Button) target;
          this.ButtonShiftDeviceTime.Click += new RoutedEventHandler(this.ButtonShiftDeviceTime_Click);
          break;
        case 61:
          this.ButtonResetTimeShift = (Button) target;
          this.ButtonResetTimeShift.Click += new RoutedEventHandler(this.ButtonResetTimeShift_Click);
          break;
        case 62:
          this.CheckBoxShowLoRaAlarmChanges = (CheckBox) target;
          break;
        case 63:
          this.CheckBoxShowEventLoggerChanges = (CheckBox) target;
          break;
        case 64:
          this.TextBoxOut = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
