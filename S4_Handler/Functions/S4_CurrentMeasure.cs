// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_CurrentMeasure
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using HandlerLib.NFC;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  public class S4_CurrentMeasure
  {
    internal static Logger S4_CurrentMeasureLogger = LogManager.GetLogger(nameof (S4_CurrentMeasure));
    private static double[] k_sample = new double[8]
    {
      1.5,
      3.5,
      7.5,
      12.5,
      19.5,
      39.5,
      79.5,
      160.5
    };
    private static double MaxMeasureTime = S4_CurrentMeasure.GetMeasureTime_ms(7, 7) / 1000.0;
    private S4_HandlerFunctions MyFunctions;
    private NfcDeviceCommands NFC;
    private HardwareTypeTables.HardwareAndFirmwareInfoRow HardwareAndFirmwareInfo;
    internal int StartSamplesSuppression = 5;
    private const double SuspectedStartVoltage = 3.6;
    private const double DefaultMeareVoltageLimit = 3.1;
    private const double DefaultMeasureVoltageLimitWithDiode = 3.2;
    private const int VoltageSampleCount = 100;
    private const int StartVoltageSamples = 3;
    private const int EndVoltageSamples = 3;
    private bool CommunicationFirmwareChecked;
    internal List<KeyValuePair<string, string>> TestInfos;
    private string PCB_Description;
    private double PullUp = 100000.0;
    private double? PullDown;
    private double? Capacity;
    private double? MeasureVoltageLimit;
    private ushort? StandbySetup;
    private ushort? OperationSetup;
    private ushort? WorkingSetup;
    private ushort? RadioSetup;
    internal int VoltagStartRangeMinIndex;
    internal int VoltagStartRangeIndexBehind;
    internal double StartVoltage;
    internal ushort StartValueADC;
    internal int VoltagEndRangeMinIndex;
    internal int VoltagEndRangeIndexBehind;
    internal double EndVoltage;
    internal ushort EndValueADC;
    internal double? ResistorPreventedDiscargeVoltage;
    internal double? RC_TimeConstant;
    internal double DischargeTime_ms;
    internal double DischargeVoltage;
    internal StringBuilder PossibleTimes;
    private ushort[] AdcValues;

    private S4_Meter MyWorkMeter => this.MyFunctions.myMeters.checkedWorkMeter;

    public double? MaxCurrentStandby_mA { get; private set; }

    public double? MaxCurrentOperation_mA { get; private set; }

    public double? MaxCurrentWorking_mA { get; private set; }

    public double? MaxCurrentRadio_mA { get; private set; }

    public double? MinCurrentStandby_mA { get; private set; }

    public double? MinCurrentOperation_mA { get; private set; }

    public double? MinCurrentWorking_mA { get; private set; }

    public double? MinCurrentRadio_mA { get; private set; }

    public bool FirmwareAbleForCurrentTest { get; private set; }

    public bool ConfigFromDatabaseOK { get; private set; }

    public StringBuilder TestInfoText { get; private set; }

    public double[] Voltages { get; private set; }

    public double Current_mA { get; private set; }

    public Exception DatabaseDataException { get; private set; }

    public S4_CurrentMeasureMode? PreparedMode { get; private set; }

    public double PreparedMeasureTime_ms { get; private set; }

    public ushort? PreparedTimeSetup { get; private set; }

    public double? PreparedMaxCurrent_mA { get; private set; }

    public double? PreparedMinCurrent_mA { get; private set; }

    internal int PreparedOversamplingSetup => (int) this.PreparedTimeSetup.Value & 15;

    internal int PreparedHoldSetup => (int) this.PreparedTimeSetup.Value >> 4;

    public bool TwoPointMode
    {
      get
      {
        S4_CurrentMeasureMode? preparedMode1 = this.PreparedMode;
        S4_CurrentMeasureMode currentMeasureMode1 = S4_CurrentMeasureMode.Standby;
        int num;
        if (!(preparedMode1.GetValueOrDefault() == currentMeasureMode1 & preparedMode1.HasValue))
        {
          S4_CurrentMeasureMode? preparedMode2 = this.PreparedMode;
          S4_CurrentMeasureMode currentMeasureMode2 = S4_CurrentMeasureMode.OperationMode;
          num = preparedMode2.GetValueOrDefault() == currentMeasureMode2 & preparedMode2.HasValue ? 1 : 0;
        }
        else
          num = 1;
        return num != 0;
      }
    }

    internal S4_CurrentMeasure(S4_HandlerFunctions functions)
    {
      this.MyFunctions = functions;
      this.NFC = functions.checkedNfcCommands;
      this.PCB_Description = "unknown";
      this.Current_mA = double.NaN;
      this.TestInfoText = new StringBuilder();
      this.PossibleTimes = new StringBuilder();
      S4_DeviceIdentification deviceIdentification = this.MyWorkMeter.deviceIdentification;
      if (new FirmwareVersion(deviceIdentification.FirmwareVersion.Value) < (object) "1.7.2 IUW")
      {
        this.TestInfoText.AppendLine("!!! Firmware not able for current tests !!!");
      }
      else
      {
        this.FirmwareAbleForCurrentTest = true;
        this.TestInfoText.AppendLine("*** TestInfo from database ***");
        HardwareTypeSupport hardwareTypeSupport = this.MyFunctions.myHardwareTypeSupport;
        uint? nullable = deviceIdentification.HardwareID;
        int hardwareVersion = (int) nullable.Value;
        nullable = deviceIdentification.FirmwareVersion;
        int firmwareVersion = (int) nullable.Value;
        this.HardwareAndFirmwareInfo = hardwareTypeSupport.GetHardwareAndFirmwareInfo(hardwareVersion, firmwareVersion);
        if (this.HardwareAndFirmwareInfo != null && !this.HardwareAndFirmwareInfo.IsTestinfoNull())
        {
          try
          {
            if (!this.HardwareAndFirmwareInfo.IsDescriptionNull())
              this.PCB_Description = this.HardwareAndFirmwareInfo.Description;
            this.TestInfos = DbUtil.KeyValueStringListToKeyValuePairList(this.HardwareAndFirmwareInfo.Testinfo);
            this.TestInfoText.AppendLine("PCB: " + this.PCB_Description);
            foreach (KeyValuePair<string, string> testInfo in this.TestInfos)
            {
              if (testInfo.Value != null)
                this.TestInfoText.AppendLine(testInfo.Key + ": " + testInfo.Value);
              else
                this.TestInfoText.AppendLine(testInfo.Key + ": null");
            }
            this.PullDown = DbUtil.GetDoubleForKey(S4_CurrentMeasure.DatabaseToken.PullDown.ToString(), this.TestInfos);
            this.Capacity = DbUtil.GetDoubleForKey(S4_CurrentMeasure.DatabaseToken.Capacity.ToString(), this.TestInfos);
            S4_CurrentMeasure.DatabaseToken databaseToken = S4_CurrentMeasure.DatabaseToken.V_Min;
            this.MeasureVoltageLimit = DbUtil.GetDoubleForKey(databaseToken.ToString(), this.TestInfos);
            if (!this.MeasureVoltageLimit.HasValue)
            {
              int num1;
              if (this.PullDown.HasValue)
              {
                double? pullDown = this.PullDown;
                double num2 = 100000.0;
                num1 = pullDown.GetValueOrDefault() == num2 & pullDown.HasValue ? 1 : 0;
              }
              else
                num1 = 1;
              this.MeasureVoltageLimit = num1 == 0 ? new double?(3.1) : new double?(3.2);
            }
            databaseToken = S4_CurrentMeasure.DatabaseToken.I_StandbyMax;
            this.MaxCurrentStandby_mA = DbUtil.GetDoubleForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.I_OperationMax;
            this.MaxCurrentOperation_mA = DbUtil.GetDoubleForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.I_WorkingMax;
            this.MaxCurrentWorking_mA = DbUtil.GetDoubleForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.I_RadioMax;
            this.MaxCurrentRadio_mA = DbUtil.GetDoubleForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.I_StandbyMin;
            this.MinCurrentStandby_mA = DbUtil.GetDoubleForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.I_OperationMin;
            this.MinCurrentOperation_mA = DbUtil.GetDoubleForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.I_WorkingMin;
            this.MinCurrentWorking_mA = DbUtil.GetDoubleForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.I_RadioMin;
            this.MinCurrentRadio_mA = DbUtil.GetDoubleForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.Standby;
            this.StandbySetup = DbUtil.GetHexUshortForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.Operation;
            this.OperationSetup = DbUtil.GetHexUshortForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.Working;
            this.WorkingSetup = DbUtil.GetHexUshortForKey(databaseToken.ToString(), this.TestInfos);
            databaseToken = S4_CurrentMeasure.DatabaseToken.Radio;
            this.RadioSetup = DbUtil.GetHexUshortForKey(databaseToken.ToString(), this.TestInfos);
            this.ConfigFromDatabaseOK = true;
            if (this.IsMeasureModePossible(S4_CurrentMeasureMode.Standby))
              this.TestInfoText.AppendLine("MeasureMode standby possible");
            else if (this.IsMeasureModePossible(S4_CurrentMeasureMode.Standby, false))
            {
              this.TestInfoText.AppendLine("MeasureMode standby only usable from Handler");
            }
            else
            {
              this.TestInfoText.AppendLine("MeasureMode standby not possible");
              this.ConfigFromDatabaseOK = false;
            }
            if (this.IsMeasureModePossible(S4_CurrentMeasureMode.OperationMode))
              this.TestInfoText.AppendLine("MeasureMode operation possible");
            else if (this.IsMeasureModePossible(S4_CurrentMeasureMode.OperationMode, false))
            {
              this.TestInfoText.AppendLine("MeasureMode operation only usable from Handler");
            }
            else
            {
              this.TestInfoText.AppendLine("MeasureMode operation not possible");
              this.ConfigFromDatabaseOK = false;
            }
            if (this.IsMeasureModePossible(S4_CurrentMeasureMode.WorkingMode))
              this.TestInfoText.AppendLine("MeasureMode working possible");
            else if (this.IsMeasureModePossible(S4_CurrentMeasureMode.WorkingMode, false))
            {
              this.TestInfoText.AppendLine("MeasureMode working only usable from Handler");
            }
            else
            {
              this.TestInfoText.AppendLine("MeasureMode working not possible");
              this.ConfigFromDatabaseOK = false;
            }
            if (this.MyWorkMeter.deviceIdentification.IsRadioDevice.Value)
            {
              if (this.IsMeasureModePossible(S4_CurrentMeasureMode.RadioCarrierOn))
                this.TestInfoText.AppendLine("MeasureMode radio possible");
              else if (this.IsMeasureModePossible(S4_CurrentMeasureMode.RadioCarrierOn, false))
              {
                this.TestInfoText.AppendLine("MeasureMode radio only usable from Handler");
              }
              else
              {
                this.TestInfoText.AppendLine("MeasureMode radio not possible");
                this.ConfigFromDatabaseOK = false;
              }
            }
            else
              this.TestInfoText.AppendLine("No radio device");
          }
          catch (Exception ex)
          {
            this.ConfigFromDatabaseOK = false;
            this.DatabaseDataException = ex;
            this.TestInfoText.AppendLine("TestInfo out of database not ok -> exception");
          }
        }
        else
          this.TestInfoText.AppendLine("No database data");
        this.TestInfoText.AppendLine();
        if (this.ConfigFromDatabaseOK)
          this.TestInfoText.AppendLine("Config from database ok");
        else
          this.TestInfoText.AppendLine("!!! Config from database NOT ok !!!");
      }
    }

    public async Task<bool> IsShuntAssembled(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      uint? hardwareId = this.MyWorkMeter.deviceIdentification.HardwareID;
      uint num1 = 3840;
      uint? nullable = hardwareId.HasValue ? new uint?(hardwareId.GetValueOrDefault() & num1) : new uint?();
      uint num2 = 0;
      if (!((int) nullable.GetValueOrDefault() == (int) num2 & nullable.HasValue))
        return false;
      double maxTestTime_ms = 8000.0;
      double dischargeVoltageLimit = 0.1;
      double dischargeTimeLimit_ms = this.Capacity.Value * dischargeVoltageLimit / (this.MinCurrentWorking_mA.Value / 1000.0) * 1000.0;
      double dischargeTestTime_ms = dischargeTimeLimit_ms * 3.0;
      if (dischargeTestTime_ms > maxTestTime_ms)
      {
        dischargeVoltageLimit = dischargeVoltageLimit / dischargeTestTime_ms * maxTestTime_ms;
        dischargeTestTime_ms = maxTestTime_ms;
      }
      await this.RunMeasureAsync(progress, cancelToken, S4_CurrentMeasureMode.WorkingMode, dischargeTestTime_ms);
      return this.DischargeVoltage < dischargeVoltageLimit;
    }

    public async Task<double> MeasureDeviceCurrent(
      S4_CurrentMeasureMode measureMode,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.RunMeasureAsync(progress, cancelToken, measureMode);
      return this.Current_mA;
    }

    public async Task<double> MeasureDeviceCurrent(
      S4_CurrentMeasureMode measureMode,
      double minTime,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.RunMeasureAsync(progress, cancelToken, measureMode, minTime);
      return this.Current_mA;
    }

    internal async Task RunMeasureAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      S4_CurrentMeasureMode measureMode,
      double min_time_ms = double.NaN)
    {
      if (!this.CommunicationFirmwareChecked)
      {
        await NFC_Versions.CheckVersions(this.MyWorkMeter.deviceIdentification.FirmwareVersionObj, this.MyFunctions.myPort, this.NFC, progress, cancelToken);
        this.CommunicationFirmwareChecked = true;
      }
      this.Voltages = (double[]) null;
      this.PreparedMode = new S4_CurrentMeasureMode?();
      if (!this.FirmwareAbleForCurrentTest)
        throw new Exception("Firmware not prepared for current tests");
      this.PrepareMeasure(measureMode);
      if (!this.PullDown.HasValue)
        throw new Exception("Pull down not defined");
      if (!this.Capacity.HasValue)
        throw new Exception("Capacity not defined");
      ushort? preparedTimeSetup = this.PreparedTimeSetup;
      if (!preparedTimeSetup.HasValue && double.IsNaN(min_time_ms))
        throw new Exception("Measure time not defined");
      if (!this.PullDown.HasValue || !this.Capacity.HasValue)
        throw new Exception("Pull down or capacity not defined");
      if (measureMode == S4_CurrentMeasureMode.RadioCarrierOn && !this.MyWorkMeter.deviceIdentification.IsRadioDevice.Value)
        throw new FunctionDoesNotMakeSenseException("Radio test for non radio divices are not possible");
      if (!double.IsNaN(min_time_ms))
        this.CalculateTimeSystem(min_time_ms);
      S4_CurrentMeasureMode? preparedMode = this.PreparedMode;
      if (!preparedMode.HasValue)
        throw new Exception("Measure mode not defined");
      this.PossibleTimes.Clear();
      ushort read_offset_ms = 500;
      byte[] modeParameters;
      if (this.TwoPointMode)
      {
        modeParameters = new byte[3];
        byte[] numArray = modeParameters;
        preparedMode = this.PreparedMode;
        int num1 = (int) (byte) preparedMode.Value;
        numArray[0] = (byte) num1;
        preparedTimeSetup = this.PreparedTimeSetup;
        BitConverter.GetBytes(preparedTimeSetup.Value).CopyTo((Array) modeParameters, 1);
        preparedMode = this.PreparedMode;
        S4_CurrentMeasureMode currentMeasureMode1 = S4_CurrentMeasureMode.OperationMode;
        int num2;
        if (!(preparedMode.GetValueOrDefault() == currentMeasureMode1 & preparedMode.HasValue))
        {
          preparedMode = this.PreparedMode;
          S4_CurrentMeasureMode currentMeasureMode2 = S4_CurrentMeasureMode.Standby;
          num2 = preparedMode.GetValueOrDefault() == currentMeasureMode2 & preparedMode.HasValue ? 1 : 0;
        }
        else
          num2 = 1;
        if (num2 != 0)
          read_offset_ms = (ushort) 2000;
        preparedTimeSetup = this.PreparedTimeSetup;
        int theSetup = (int) preparedTimeSetup.Value - 5;
        if (theSetup < 1)
          theSetup = 1;
        this.PossibleTimes.AppendLine("Possible time near to the current setup");
        for (int i = 0; i < 10; ++i)
        {
          this.PossibleTimes.AppendLine("Setup: 0x" + theSetup.ToString("x04") + "; Time [ms]: " + ((double) theSetup / 256.0 * 1000.0).ToString("0.0000"));
          ++theSetup;
        }
      }
      else
      {
        modeParameters = new byte[2];
        byte[] numArray1 = modeParameters;
        preparedMode = this.PreparedMode;
        int num3 = (int) (byte) preparedMode.Value;
        numArray1[0] = (byte) num3;
        byte[] numArray2 = modeParameters;
        preparedTimeSetup = this.PreparedTimeSetup;
        int num4 = (int) (byte) preparedTimeSetup.Value;
        numArray2[1] = (byte) num4;
      }
      bool isModeSet = false;
      int cnt = 0;
      do
      {
        ++cnt;
        try
        {
          await this.NFC.SetModeAsync(S4_DeviceModes.CurrentTest, progress, cancelToken, modeParameters);
          isModeSet = true;
        }
        catch (Exception ex)
        {
          if (cnt > 5)
            throw new Exception("NFC.SetModeAsync(S4_DeviceModes.CurrentTest..) Error: ", ex);
          S4_CurrentMeasure.S4_CurrentMeasureLogger.Trace("Failed: SetModeAsync(S4_DeviceModes.CurrentTest.. ) Counter: " + cnt.ToString());
          await Task.Delay(100);
          this.NFC.Port.Close();
          progress.Report("Set Mode failed:  " + cnt.ToString());
          await Task.Delay(100);
        }
      }
      while (!isModeSet);
      byte[] numArray3 = await this.NFC.mySubunitCommands.SetRfOffAsync(progress, cancelToken);
      double waitSeconds = (this.PreparedMeasureTime_ms + (double) read_offset_ms + 1000.0) / 1000.0;
      while (waitSeconds > 0.0)
      {
        progress.Report("Remaining time [s]: " + waitSeconds.ToString("0.000"));
        if (waitSeconds > 1.0)
        {
          await Task.Delay(1000, cancelToken);
          --waitSeconds;
        }
        else
        {
          await Task.Delay((int) (waitSeconds * 1000.0), cancelToken);
          waitSeconds = 0.0;
        }
      }
      await Task.Delay(600);
      await this.ReadCurrentSelfTestValues(progress, cancelToken);
      this.CalculateCurrent();
      modeParameters = (byte[]) null;
    }

    internal void CalculateCurrent()
    {
      if (this.Voltages == null || this.Voltages.Length != 100)
        throw new ArgumentException("Illegal voltage samples");
      if (this.TwoPointMode)
      {
        this.StartValueADC = this.AdcValues[0];
        this.StartVoltage = this.Voltages[0];
        this.EndValueADC = this.AdcValues[this.AdcValues.Length - 1];
        this.EndVoltage = this.Voltages[this.Voltages.Length - 1];
        this.DischargeTime_ms = this.PreparedMeasureTime_ms;
      }
      else
      {
        this.VoltagStartRangeMinIndex = this.StartSamplesSuppression;
        this.VoltagStartRangeIndexBehind = 3 + this.StartSamplesSuppression;
        this.StartVoltage = 0.0;
        this.StartValueADC = (ushort) 0;
        for (int startRangeMinIndex = this.VoltagStartRangeMinIndex; startRangeMinIndex < this.VoltagStartRangeIndexBehind; ++startRangeMinIndex)
        {
          this.StartVoltage += this.Voltages[startRangeMinIndex];
          this.StartValueADC += this.AdcValues[startRangeMinIndex];
        }
        this.StartVoltage /= 3.0;
        this.StartValueADC /= (ushort) 3;
        this.VoltagEndRangeMinIndex = this.Voltages.Length - 3;
        this.VoltagEndRangeIndexBehind = this.Voltages.Length;
        double num1 = this.MeasureVoltageLimit.Value;
        S4_CurrentMeasureMode? preparedMode = this.PreparedMode;
        S4_CurrentMeasureMode currentMeasureMode = S4_CurrentMeasureMode.RadioCarrierOn;
        if (preparedMode.GetValueOrDefault() == currentMeasureMode & preparedMode.HasValue)
          num1 = 3.05;
        for (int index1 = this.VoltagStartRangeMinIndex + 1; index1 < this.VoltagEndRangeMinIndex; ++index1)
        {
          double num2 = 0.0;
          for (int index2 = index1; index2 < index1 + 3; ++index2)
            num2 += this.Voltages[index2];
          if (num2 / 3.0 <= num1)
          {
            this.VoltagEndRangeMinIndex = index1 - 1;
            this.VoltagEndRangeIndexBehind = this.VoltagEndRangeMinIndex + 3;
            break;
          }
        }
        this.EndVoltage = 0.0;
        this.EndValueADC = (ushort) 0;
        for (int endRangeMinIndex = this.VoltagEndRangeMinIndex; endRangeMinIndex < this.VoltagEndRangeIndexBehind; ++endRangeMinIndex)
        {
          this.EndVoltage += this.Voltages[endRangeMinIndex];
          this.EndValueADC += this.AdcValues[endRangeMinIndex];
        }
        this.EndVoltage /= 3.0;
        this.EndValueADC /= (ushort) 3;
        this.DischargeTime_ms = this.PreparedMeasureTime_ms / 100.0 * (double) (this.VoltagEndRangeMinIndex - this.VoltagStartRangeMinIndex);
      }
      this.DischargeVoltage = this.StartVoltage - this.EndVoltage;
      double num3 = this.DischargeTime_ms / 1000.0;
      this.ResistorPreventedDiscargeVoltage = new double?();
      this.RC_TimeConstant = new double?();
      this.Current_mA = 0.0;
      if (this.TwoPointMode)
      {
        double endVoltage = this.EndVoltage;
        double? measureVoltageLimit = this.MeasureVoltageLimit;
        double valueOrDefault = measureVoltageLimit.GetValueOrDefault();
        if (endVoltage < valueOrDefault & measureVoltageLimit.HasValue)
        {
          this.Current_mA = double.MaxValue;
          return;
        }
      }
      else if (this.VoltagEndRangeMinIndex - this.VoltagStartRangeMinIndex < 5)
      {
        this.Current_mA = double.MaxValue;
        return;
      }
      double num4 = (this.StartVoltage + this.EndVoltage) / 2.0 / (this.PullUp + this.PullDown.Value);
      this.Current_mA = (this.Capacity.Value * this.DischargeVoltage / num3 - num4) * 1000.0;
    }

    private void CalculateTimeSystem(double min_time_ms)
    {
      if (this.TwoPointMode)
      {
        double num1 = min_time_ms / 1000.0 * 256.0;
        this.PreparedTimeSetup = new ushort?((ushort) num1);
        ushort? preparedTimeSetup = this.PreparedTimeSetup;
        double? nullable1 = preparedTimeSetup.HasValue ? new double?((double) preparedTimeSetup.GetValueOrDefault()) : new double?();
        double num2 = num1;
        if (nullable1.GetValueOrDefault() < num2 & nullable1.HasValue)
        {
          preparedTimeSetup = this.PreparedTimeSetup;
          ushort? nullable2 = preparedTimeSetup;
          this.PreparedTimeSetup = nullable2.HasValue ? new ushort?((ushort) ((uint) nullable2.GetValueOrDefault() + 1U)) : new ushort?();
        }
        preparedTimeSetup = this.PreparedTimeSetup;
        this.PreparedMeasureTime_ms = (double) preparedTimeSetup.Value / 256.0 * 1000.0;
      }
      else
      {
        this.PreparedMeasureTime_ms = double.MaxValue;
        int num3 = 7;
        int num4 = 7;
        for (int oversamplingSetup = 0; oversamplingSetup < 8; ++oversamplingSetup)
        {
          for (int holdSetup = 0; holdSetup < 8; ++holdSetup)
          {
            double measureTimeMs = S4_CurrentMeasure.GetMeasureTime_ms(S4_CurrentMeasure.GetOversampleTime_ms(oversamplingSetup, holdSetup));
            if (measureTimeMs >= min_time_ms && measureTimeMs < this.PreparedMeasureTime_ms)
            {
              this.PreparedMeasureTime_ms = measureTimeMs;
              num3 = oversamplingSetup;
              num4 = holdSetup;
            }
          }
        }
        this.PreparedTimeSetup = new ushort?((ushort) (byte) (num4 << 4 | num3));
      }
    }

    internal void PrepareMeasure(S4_CurrentMeasureMode mode)
    {
      this.Current_mA = double.NaN;
      this.PreparedMeasureTime_ms = 0.0;
      this.PreparedMode = new S4_CurrentMeasureMode?(mode);
      switch (mode)
      {
        case S4_CurrentMeasureMode.OperationMode:
          this.PreparedTimeSetup = this.OperationSetup;
          this.PreparedMaxCurrent_mA = this.MaxCurrentOperation_mA;
          this.PreparedMinCurrent_mA = this.MinCurrentOperation_mA;
          break;
        case S4_CurrentMeasureMode.WorkingMode:
          this.PreparedTimeSetup = this.WorkingSetup;
          this.PreparedMaxCurrent_mA = this.MaxCurrentWorking_mA;
          this.PreparedMinCurrent_mA = this.MinCurrentWorking_mA;
          break;
        case S4_CurrentMeasureMode.RadioCarrierOn:
          this.PreparedTimeSetup = this.RadioSetup;
          this.PreparedMaxCurrent_mA = this.MaxCurrentRadio_mA;
          this.PreparedMinCurrent_mA = this.MinCurrentRadio_mA;
          break;
        default:
          this.PreparedTimeSetup = this.StandbySetup;
          this.PreparedMaxCurrent_mA = this.MaxCurrentStandby_mA;
          this.PreparedMinCurrent_mA = this.MinCurrentStandby_mA;
          break;
      }
      ushort? preparedTimeSetup = this.PreparedTimeSetup;
      if (!preparedTimeSetup.HasValue)
        return;
      if (this.TwoPointMode)
      {
        preparedTimeSetup = this.PreparedTimeSetup;
        this.PreparedMeasureTime_ms = (double) preparedTimeSetup.Value / 256.0 * 1000.0;
      }
      else
      {
        preparedTimeSetup = this.PreparedTimeSetup;
        this.PreparedMeasureTime_ms = (double) S4_CurrentMeasure.GetMeasureTime_ms((byte) preparedTimeSetup.Value);
      }
    }

    public bool IsMeasureModePossible(S4_CurrentMeasureMode mode, bool checkSetupAndLimits = true)
    {
      bool flag = true;
      if (!this.PullDown.HasValue)
        flag = false;
      if (!this.Capacity.HasValue)
        flag = false;
      if (mode == S4_CurrentMeasureMode.RadioCarrierOn && !this.MyWorkMeter.deviceIdentification.IsRadioDevice.Value)
        flag = false;
      if (flag & checkSetupAndLimits)
      {
        switch (mode)
        {
          case S4_CurrentMeasureMode.OperationMode:
            if (!this.OperationSetup.HasValue)
              flag = false;
            if (!this.MaxCurrentOperation_mA.HasValue)
              flag = false;
            if (!this.MinCurrentOperation_mA.HasValue)
            {
              flag = false;
              break;
            }
            break;
          case S4_CurrentMeasureMode.WorkingMode:
            if (!this.WorkingSetup.HasValue)
              flag = false;
            if (!this.MaxCurrentWorking_mA.HasValue)
              flag = false;
            if (!this.MinCurrentWorking_mA.HasValue)
            {
              flag = false;
              break;
            }
            break;
          case S4_CurrentMeasureMode.RadioCarrierOn:
            if (!this.RadioSetup.HasValue)
              flag = false;
            if (!this.MaxCurrentRadio_mA.HasValue)
              flag = false;
            if (!this.MinCurrentRadio_mA.HasValue)
            {
              flag = false;
              break;
            }
            break;
          default:
            if (!this.StandbySetup.HasValue)
              flag = false;
            if (!this.MaxCurrentStandby_mA.HasValue)
              flag = false;
            if (!this.MinCurrentStandby_mA.HasValue)
            {
              flag = false;
              break;
            }
            break;
        }
      }
      return flag;
    }

    private async Task ReadCurrentSelfTestValues(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      S4_DeviceMemory meterMemory = this.MyFunctions.myMeters.checkedWorkMeter.meterMemory;
      if (new FirmwareVersion(this.MyWorkMeter.deviceIdentification.FirmwareVersion.Value) <= (object) "1.7.1 IUW")
        throw new Exception("CurrentSelfTest is not supported by this firmware_version");
      AddressRange adrRange = new AddressRange(meterMemory.UsedParametersByName[S4_Params.Test_CurrMeasValues.ToString()].Address, 200U);
      await this.NFC.ReadMemoryAsync(adrRange, (DeviceMemory) meterMemory, progress, cancelToken);
      byte[] selfTestData = meterMemory.GetData(adrRange);
      this.AdcValues = new ushort[selfTestData.Length / 2];
      this.Voltages = new double[this.AdcValues.Length];
      StringBuilder outputData = new StringBuilder();
      for (int i = 0; i < this.AdcValues.Length; ++i)
      {
        this.AdcValues[i] = (ushort) ((uint) selfTestData[i * 2] + ((uint) selfTestData[i * 2 + 1] << 8));
        double ADC_Voltage = (double) this.AdcValues[i] * 3.0 / 4096.0;
        this.Voltages[i] = ADC_Voltage / this.PullDown.Value * (this.PullUp + this.PullDown.Value);
        outputData.Append(";" + this.Voltages[i].ToString("0.00"));
      }
      S4_CurrentMeasure.S4_CurrentMeasureLogger.Trace("Voltages [V]: " + outputData.ToString());
      if (!this.TwoPointMode)
      {
        meterMemory = (S4_DeviceMemory) null;
        adrRange = (AddressRange) null;
        selfTestData = (byte[]) null;
        outputData = (StringBuilder) null;
      }
      else
      {
        double startValue = this.Voltages[0];
        double endValue = this.Voltages[this.Voltages.Length - 1];
        double stepValue = (endValue - startValue) / (double) (this.Voltages.Length - 1);
        double indexValue = startValue + stepValue;
        for (int i = 1; i < this.Voltages.Length - 1; ++i)
        {
          this.Voltages[i] = indexValue;
          indexValue += stepValue;
        }
        meterMemory = (S4_DeviceMemory) null;
        adrRange = (AddressRange) null;
        selfTestData = (byte[]) null;
        outputData = (StringBuilder) null;
      }
    }

    internal static int GetMeasureTime_ms(byte timeCode)
    {
      return (int) S4_CurrentMeasure.GetMeasureTime_ms((int) timeCode & 15, (int) timeCode >> 4);
    }

    private static double GetMeasureTime_ms(int oversamplingSetup, int holdSetup)
    {
      return 100.0 * S4_CurrentMeasure.GetOversampleTime_ms(oversamplingSetup, holdSetup);
    }

    private static double GetMeasureTime_ms(double oversampleTime_ms) => 100.0 * oversampleTime_ms;

    private static double GetOversampleTime_ms(int oversamplingSetup, int holdSetup)
    {
      double num = (S4_CurrentMeasure.k_sample[holdSetup] + 12.5) / 524.0;
      return Math.Pow(2.0, (double) (oversamplingSetup + 1)) * num;
    }

    public string GetPreparedDataAsText()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("*** Prepared data for measuring ***");
      stringBuilder1.AppendLine();
      try
      {
        stringBuilder1.AppendLine("PCB: " + this.PCB_Description);
        stringBuilder1.AppendLine("PullUp: [Ohm]" + this.PullUp.ToString());
        stringBuilder1.AppendLine("PullDown [Ohm]: " + this.PullDown.ToString());
        stringBuilder1.AppendLine("Capacity [mF]: " + (this.Capacity.Value * 1000.0).ToString("0.0000"));
        stringBuilder1.AppendLine();
        stringBuilder1.AppendLine("Prepared mode: " + this.PreparedMode.ToString());
        if (this.TwoPointMode)
        {
          stringBuilder1.AppendLine("Setup: 0x" + this.PreparedTimeSetup.Value.ToString("x04"));
        }
        else
        {
          stringBuilder1.AppendLine("Setup: 0x" + this.PreparedTimeSetup.Value.ToString("x02"));
          stringBuilder1.AppendLine("PreparedOversamplingSetup: " + this.PreparedOversamplingSetup.ToString());
          stringBuilder1.AppendLine("PreparedHoldSetup: " + this.PreparedHoldSetup.ToString());
        }
        stringBuilder1.AppendLine("MeasureTime [ms]: " + this.PreparedMeasureTime_ms.ToString("0.000"));
        double? nullable;
        int num1;
        if (this.PreparedMaxCurrent_mA.HasValue)
        {
          nullable = this.PreparedMaxCurrent_mA;
          if (nullable.HasValue)
          {
            nullable = this.PreparedMaxCurrent_mA;
            num1 = !nullable.HasValue ? 1 : 0;
            goto label_8;
          }
        }
        num1 = 1;
label_8:
        if (num1 != 0)
        {
          stringBuilder1.AppendLine("Limits not prepared");
        }
        else
        {
          StringBuilder stringBuilder2 = stringBuilder1;
          nullable = this.PreparedMaxCurrent_mA;
          double num2 = nullable.Value;
          string str1 = "MaxCurrent [mA]: " + num2.ToString("0.000");
          stringBuilder2.AppendLine(str1);
          StringBuilder stringBuilder3 = stringBuilder1;
          nullable = this.PreparedMinCurrent_mA;
          num2 = nullable.Value;
          string str2 = "MinCurrent [mA]: " + num2.ToString("0.000");
          stringBuilder3.AppendLine(str2);
          StringBuilder stringBuilder4 = stringBuilder1;
          num2 = this.MeasureVoltageLimit.Value;
          string str3 = "MeasureVoltageLimit [V]: " + num2.ToString("0.00");
          stringBuilder4.AppendLine(str3);
        }
        stringBuilder1.AppendLine();
        stringBuilder1.AppendLine(this.PossibleTimes.ToString());
      }
      catch
      {
        stringBuilder1.AppendLine("*** Not all prepared data defined *** ");
      }
      return stringBuilder1.ToString();
    }

    public override string ToString()
    {
      if (this.Voltages == null)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("*** Measuring result ***");
      stringBuilder.AppendLine();
      if (!double.IsNaN(this.Current_mA))
      {
        stringBuilder.AppendLine("Current [mA]: " + this.Current_mA.ToString("0.000"));
        if (this.Current_mA == double.MaxValue)
        {
          if (this.TwoPointMode)
            stringBuilder.AppendLine("End voltage out of measure range");
          else
            stringBuilder.AppendLine("Discharge time to short");
        }
      }
      else
        stringBuilder.AppendLine("Current not available");
      stringBuilder.AppendLine();
      if (this.TwoPointMode)
      {
        stringBuilder.AppendLine("StartVoltage [V]: " + this.StartVoltage.ToString("0.00"));
        stringBuilder.AppendLine("EndVoltage [V]: " + this.EndVoltage.ToString("0.00"));
      }
      else
      {
        stringBuilder.AppendLine("VoltagStartRangeMinIndex: " + this.VoltagStartRangeMinIndex.ToString());
        stringBuilder.AppendLine("VoltagStartRangeIndexBehind: " + this.VoltagStartRangeIndexBehind.ToString());
        stringBuilder.AppendLine("StartVoltage [V]: " + this.StartVoltage.ToString("0.00"));
        stringBuilder.AppendLine("StartValueADC: " + this.StartValueADC.ToString());
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("VoltagEndRangeMinIndex: " + this.VoltagEndRangeMinIndex.ToString());
        stringBuilder.AppendLine("VoltagEndRangeIndexBehind: " + this.VoltagEndRangeIndexBehind.ToString());
        stringBuilder.AppendLine("EndVoltage [V]: " + this.EndVoltage.ToString("0.00"));
        stringBuilder.AppendLine("EndValueADC: " + this.EndValueADC.ToString());
      }
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("DischargeTime [ms]: " + this.DischargeTime_ms.ToString("0.000"));
      stringBuilder.AppendLine("DischargeVoltage  [V]: " + this.DischargeVoltage.ToString("0.00"));
      stringBuilder.AppendLine("ADC digits: " + ((int) this.StartValueADC - (int) this.EndValueADC).ToString());
      stringBuilder.AppendLine();
      if (this.ResistorPreventedDiscargeVoltage.HasValue)
        stringBuilder.AppendLine("ResistorPreventedDiscargeVoltage [V]: " + this.ResistorPreventedDiscargeVoltage.Value.ToString("0.000"));
      if (this.RC_TimeConstant.HasValue)
        stringBuilder.AppendLine("RC_TimeConstant [s]: " + this.RC_TimeConstant.Value.ToString("0.000"));
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(this.GetPreparedDataAsText());
      return stringBuilder.ToString();
    }

    private enum DatabaseToken
    {
      PullDown,
      Capacity,
      V_Min,
      Standby,
      Operation,
      Working,
      Radio,
      I_StandbyMax,
      I_StandbyMin,
      I_OperationMax,
      I_OperationMin,
      I_WorkingMax,
      I_WorkingMin,
      I_RadioMax,
      I_RadioMin,
    }
  }
}
