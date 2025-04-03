// Decompiled with JetBrains decompiler
// Type: HandlerLib.BatteryEnergyManagement
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using NLog;
using System;

#nullable disable
namespace HandlerLib
{
  public class BatteryEnergyManagement
  {
    public const uint SecondsPerYear = 31536000;
    public const uint SecondsPerHalfYear = 15768000;
    public const uint SecondsPerMonth = 2592000;
    public const uint SecondsPerDay = 86400;
    protected static Logger SetBatteryEnergyLogger = LogManager.GetLogger("SetBatteryEnergy");
    protected static Logger GetBatteryEnergyLogger = LogManager.GetLogger("GetBatteryEnergy");
    protected double BatteryUsableFactor = 0.7;
    protected double VolumeCycleEnergy_mAh_per_cycle = 0.0;
    protected double TemperatureMeasurement_mAh_per_cycle = 0.0;
    protected double EnergyCalculationEnergy_mAh_per_cycle = 0.0;
    protected double RadioTransmitEnergy_mAh_per_telegram = 0.0;
    protected int NumberOfRadioInputs = 0;
    protected double RadioTransmitEnergy_mAh_per_inputTelegram = 0.0;
    protected double RadioEnergy_mAh_per_cycle;
    protected double StandbyEnergy_mAh_per_year = 0.0;
    protected double VolumeCycleEnergy_mAh_per_year = 0.0;
    protected double TemperatureMeasurement_mAh_per_year = 0.0;
    protected double EnergyCalculationEnergy_mAh_per_year = 0.0;
    protected double RadioEnergy_mAh_per_year = 0.0;
    protected double? PossibleHourDiagnosticYears;
    protected DateTime NewBatteryStartDate;
    protected bool SetConfig;
    protected Logger NlogLogger;

    public double BatteryCapacity_mAh { get; protected set; }

    public DateTime MinRequiredBatteryEndDate { get; protected set; }

    public double VolumeCycle_s { get; protected set; }

    public double EnergyCycle_s { get; protected set; }

    public double RadioCycle_s { get; protected set; }

    public uint UsableBatteryCapacity_mAh
    {
      get => (uint) (this.BatteryCapacity_mAh * this.BatteryUsableFactor);
    }

    public double BatteryEnergyPerYear_mAh
    {
      get
      {
        return this.StandbyEnergy_mAh_per_year + this.VolumeCycleEnergy_mAh_per_year + this.TemperatureMeasurement_mAh_per_year + this.EnergyCalculationEnergy_mAh_per_year + this.RadioEnergy_mAh_per_year;
      }
    }

    public double MaxBatteryYears
    {
      get => (double) this.UsableBatteryCapacity_mAh / this.BatteryEnergyPerYear_mAh;
    }

    public double RequiredBatteryYears
    {
      get
      {
        return (double) this.MinRequiredBatteryEndDate.Subtract(this.NewBatteryStartDate).Days / 365.0;
      }
    }

    public double MissingBatteryMonth => (this.RequiredBatteryYears - this.MaxBatteryYears) * 12.0;

    public double RemainingYears => this.MaxBatteryYears - this.RequiredBatteryYears;

    public double Remaining_mAh
    {
      get => (double) this.UsableBatteryCapacity_mAh / this.MaxBatteryYears * this.RemainingYears;
    }

    public DateTime MaxEndOfBatteryDate
    {
      get
      {
        return this.NewBatteryStartDate.Date.AddDays((double) ((int) (this.MaxBatteryYears * 365.3) + 1));
      }
    }

    public uint? LoRaDiagnosticCounts { get; protected set; }

    public BatteryEnergyManagement(double batteryNominalCapacity_mAh, DateTime deviceTime)
    {
      this.NewBatteryStartDate = deviceTime;
      this.BatteryCapacity_mAh = batteryNominalCapacity_mAh;
      this.VolumeCycle_s = 0.0;
      this.EnergyCycle_s = 0.0;
      this.RadioCycle_s = 0.0;
    }

    public double GetEnergyForLoRaBytes_mAh(int bytes)
    {
      if (bytes == 24)
        return BatteryEnergyManagement.GetEnergyForTime_mAh(29.0, 1.7);
      if (bytes == 30)
        return BatteryEnergyManagement.GetEnergyForTime_mAh(29.0, 1.9);
      throw new Exception("Illegal LoRa telegram bytes: " + bytes.ToString());
    }

    public static double GetEnergyForTime_mAh(double current_mA, double activeTime_s)
    {
      return activeTime_s == 0.0 ? 0.0 : current_mA * activeTime_s / 3600.0;
    }

    public static double GetEnergyPerYear_mAh(double energyPerCycle, double cycleTime_s)
    {
      return cycleTime_s == 0.0 ? 0.0 : energyPerCycle * 31536000.0 / cycleTime_s;
    }

    protected void StartNLogLogging(bool setConfig)
    {
      this.SetConfig = setConfig;
      if (this.SetConfig)
      {
        this.NlogLogger = BatteryEnergyManagement.SetBatteryEnergyLogger;
        this.NlogLogger.Trace("*** SetBatteryEnergy ***");
      }
      else
      {
        this.NlogLogger = BatteryEnergyManagement.GetBatteryEnergyLogger;
        this.NlogLogger.Trace("*** GetBatteryEnergy ***");
      }
    }

    protected void LogResultsAndExceptionOnSet()
    {
      if (this.NlogLogger.IsTraceEnabled)
      {
        this.NlogLogger.Trace("BatteryNominalCapacity_mAh = " + this.BatteryCapacity_mAh.ToString("0.0") + "; BatteryUsableFactor = " + this.BatteryUsableFactor.ToString("0.00") + "; UsableBatteryCapacity_mAh = " + this.UsableBatteryCapacity_mAh.ToString());
        this.NlogLogger.Trace("VolumeCycle_s = " + this.VolumeCycle_s.ToString() + "; EnergyCycle_s = " + this.EnergyCycle_s.ToString() + "; RadioCycle_s = " + this.RadioCycle_s.ToString());
        this.NlogLogger.Trace("StandbyEnergy_mAh_per_year = " + this.StandbyEnergy_mAh_per_year.ToString());
        if (this.VolumeCycle_s > 0.0)
          this.NlogLogger.Trace("VolumeCycleEnergy_mAh_per_year = " + this.VolumeCycleEnergy_mAh_per_year.ToString());
        if (this.EnergyCycle_s > 0.0)
          this.NlogLogger.Trace("TemperatureMeasurement_mAh_per_year = " + this.TemperatureMeasurement_mAh_per_year.ToString() + "; EnergyCalculationEnergy_mAh_per_year = " + this.EnergyCalculationEnergy_mAh_per_year.ToString());
        if (this.RadioCycle_s > 0.0)
          this.NlogLogger.Trace("RadioEnergy_mAh_per_year = " + this.RadioEnergy_mAh_per_year.ToString("0.0"));
        Logger nlogLogger1 = this.NlogLogger;
        double num = this.BatteryEnergyPerYear_mAh;
        string message1 = "-> BatteryEnergyPerYear_mAh = " + num.ToString();
        nlogLogger1.Trace(message1);
        this.NlogLogger.Trace("NewBatteryStartDate = " + this.NewBatteryStartDate.ToShortDateString() + "; RequiredBatteryEndDate = " + this.MinRequiredBatteryEndDate.ToShortDateString() + "; MaxBatteryEndDate = " + this.MaxEndOfBatteryDate.ToShortDateString());
        Logger nlogLogger2 = this.NlogLogger;
        num = this.RequiredBatteryYears;
        string str1 = num.ToString("0.00");
        num = this.MaxBatteryYears;
        string str2 = num.ToString("0.00");
        string message2 = "RequiredBatteryYears = " + str1 + "; MaxBatteryYears = " + str2;
        nlogLogger2.Trace(message2);
        if (this.MissingBatteryMonth > 0.0)
        {
          Logger nlogLogger3 = this.NlogLogger;
          num = this.MissingBatteryMonth;
          string message3 = "-> MissingBatteryMonth = " + num.ToString("0.00");
          nlogLogger3.Trace(message3);
        }
        else
        {
          Logger nlogLogger4 = this.NlogLogger;
          num = this.MissingBatteryMonth * -1.0;
          string message4 = "-> BatteryReserveMonth = " + num.ToString("0.00");
          nlogLogger4.Trace(message4);
        }
      }
      if (this.SetConfig && this.MaxEndOfBatteryDate < this.MinRequiredBatteryEndDate)
        throw new BatteryNotEnoughException(this);
    }
  }
}
