// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_BatteryEnergyManagement
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib;
using NLog;
using System;

#nullable disable
namespace S3_Handler
{
  internal class S3_BatteryEnergyManagement : BatteryEnergyManagement
  {
    internal S3_BatteryEnergyManagement(
      uint hardwareMask,
      double batteryNominalCapacity_mAh,
      double radioCycle_s,
      double volumeCycle_s,
      double energyCycle_s,
      DateTime batteryEndDate,
      S3_Meter theMeter)
      : base(batteryNominalCapacity_mAh, DateTime.Now)
    {
      RadioTransmitter radioTransmitter = (RadioTransmitter) null;
      if (theMeter.MyTransmitParameterManager.Transmitter != null)
        radioTransmitter = theMeter.MyTransmitParameterManager.Transmitter.Radio;
      this.RadioCycle_s = radioCycle_s;
      this.VolumeCycle_s = volumeCycle_s;
      this.EnergyCycle_s = energyCycle_s;
      this.MinRequiredBatteryEndDate = batteryEndDate;
      bool flag1 = (hardwareMask & 8U) > 0U;
      bool flag2 = (hardwareMask & 2U) > 0U;
      if ((hardwareMask & 48U) > 0U)
      {
        if (flag1)
          this.StandbyEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyForTime_mAh(0.012, 31536000.0);
        else
          this.StandbyEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyForTime_mAh(0.01, 31536000.0);
        this.VolumeCycleEnergy_mAh_per_year = 0.0;
      }
      else
      {
        if (flag1)
          this.StandbyEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyForTime_mAh(0.015, 31536000.0);
        else
          this.StandbyEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyForTime_mAh(0.013, 31536000.0);
        this.VolumeCycleEnergy_mAh_per_cycle = BatteryEnergyManagement.GetEnergyForTime_mAh(0.13, 0.02);
        this.VolumeCycleEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyPerYear_mAh(this.VolumeCycleEnergy_mAh_per_cycle, volumeCycle_s);
      }
      this.TemperatureMeasurement_mAh_per_cycle = BatteryEnergyManagement.GetEnergyForTime_mAh(2.0, 0.02);
      this.TemperatureMeasurement_mAh_per_year = BatteryEnergyManagement.GetEnergyPerYear_mAh(this.TemperatureMeasurement_mAh_per_cycle, energyCycle_s);
      this.EnergyCalculationEnergy_mAh_per_cycle = BatteryEnergyManagement.GetEnergyForTime_mAh(0.5, 0.1);
      this.EnergyCalculationEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyPerYear_mAh(this.EnergyCalculationEnergy_mAh_per_cycle, energyCycle_s);
      if (flag1)
      {
        this.RadioTransmitEnergy_mAh_per_telegram = this.GetEnergyForLoRaBytes_mAh(30);
        if (radioCycle_s == 86400.0)
        {
          this.RadioEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyPerYear_mAh(this.GetEnergyForLoRaBytes_mAh(24), 86400.0);
          this.RadioEnergy_mAh_per_year += BatteryEnergyManagement.GetEnergyPerYear_mAh(this.GetEnergyForLoRaBytes_mAh(24), 2592000.0);
          this.RadioEnergy_mAh_per_year += BatteryEnergyManagement.GetEnergyPerYear_mAh(this.GetEnergyForLoRaBytes_mAh(24), 2592000.0);
          this.RadioEnergy_mAh_per_year += BatteryEnergyManagement.GetEnergyPerYear_mAh(this.GetEnergyForLoRaBytes_mAh(30), 15768000.0);
          this.RadioEnergy_mAh_per_year *= 1.1;
        }
        else
        {
          if (radioCycle_s != 2592000.0)
            throw new Exception("Not supported LoRa radio cycle");
          this.RadioEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyPerYear_mAh(this.GetEnergyForLoRaBytes_mAh(24), 2592000.0);
          this.RadioEnergy_mAh_per_year += BatteryEnergyManagement.GetEnergyPerYear_mAh(this.GetEnergyForLoRaBytes_mAh(30), 2592000.0);
          this.RadioEnergy_mAh_per_year += BatteryEnergyManagement.GetEnergyPerYear_mAh(this.GetEnergyForLoRaBytes_mAh(24), 2592000.0);
          this.RadioEnergy_mAh_per_year += BatteryEnergyManagement.GetEnergyPerYear_mAh(this.GetEnergyForLoRaBytes_mAh(24), 2592000.0);
          this.RadioEnergy_mAh_per_year += BatteryEnergyManagement.GetEnergyPerYear_mAh(this.GetEnergyForLoRaBytes_mAh(30), 15768000.0);
          this.RadioEnergy_mAh_per_year *= 1.2;
        }
      }
      else if (flag2)
      {
        this.NumberOfRadioInputs = 0;
        this.RadioTransmitEnergy_mAh_per_inputTelegram = 0.0;
        this.RadioEnergy_mAh_per_cycle = 0.0;
        bool flag3 = false;
        if (radioTransmitter != null)
        {
          switch (radioTransmitter.Get_ActivatedList())
          {
            case "wMBusT1_A":
              this.RadioTransmitEnergy_mAh_per_telegram = BatteryEnergyManagement.GetEnergyForTime_mAh(52.0, 0.012);
              flag3 = true;
              break;
            case "wMBusT1_B":
              this.RadioTransmitEnergy_mAh_per_telegram = BatteryEnergyManagement.GetEnergyForTime_mAh(52.0, 0.016);
              flag3 = true;
              break;
            case "wMBusT1_C":
              this.RadioTransmitEnergy_mAh_per_telegram = BatteryEnergyManagement.GetEnergyForTime_mAh(52.0, 0.017);
              flag3 = true;
              break;
            default:
              this.RadioTransmitEnergy_mAh_per_telegram = BatteryEnergyManagement.GetEnergyForTime_mAh(52.0, 0.017);
              break;
          }
        }
        else
          this.RadioTransmitEnergy_mAh_per_telegram = BatteryEnergyManagement.GetEnergyForTime_mAh(52.0, 0.017);
        if (flag3)
        {
          for (int index = 0; index < 3; ++index)
          {
            InputData inputData = theMeter.MyMeterScaling.inpData[index];
            if (inputData != null && inputData.impulsValueFactor != (ushort) 0)
              ++this.NumberOfRadioInputs;
          }
          if (this.NumberOfRadioInputs > 0)
            this.RadioTransmitEnergy_mAh_per_inputTelegram = BatteryEnergyManagement.GetEnergyForTime_mAh(52.0, 0.017);
        }
        this.RadioEnergy_mAh_per_cycle = this.RadioTransmitEnergy_mAh_per_telegram + this.RadioTransmitEnergy_mAh_per_inputTelegram * (double) this.NumberOfRadioInputs;
        this.RadioEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyPerYear_mAh(this.RadioEnergy_mAh_per_cycle, radioCycle_s);
      }
      uint? diagnosticCounts;
      if (flag1)
      {
        int num = (int) (this.Remaining_mAh / this.RadioTransmitEnergy_mAh_per_telegram);
        if (num < 0)
          this.LoRaDiagnosticCounts = new uint?(0U);
        else
          this.LoRaDiagnosticCounts = new uint?((uint) num);
        diagnosticCounts = this.LoRaDiagnosticCounts;
        this.PossibleHourDiagnosticYears = new double?((double) diagnosticCounts.Value / 24.0 / 365.0);
      }
      this.StartNLogLogging(true);
      if (this.NlogLogger.IsTraceEnabled)
      {
        if (flag2)
          this.NlogLogger.Trace("NumberOfRadioInputs = " + this.NumberOfRadioInputs.ToString() + "; RadioTransmitEnergy_mAh_per_inputTelegram = " + this.RadioTransmitEnergy_mAh_per_inputTelegram.ToString() + "; RadioEnergyPerCycle_mAh = " + this.RadioEnergy_mAh_per_cycle.ToString());
        diagnosticCounts = this.LoRaDiagnosticCounts;
        if (diagnosticCounts.HasValue)
        {
          Logger nlogLogger = this.NlogLogger;
          string str1 = this.PossibleHourDiagnosticYears.ToString();
          diagnosticCounts = this.LoRaDiagnosticCounts;
          string str2 = diagnosticCounts.ToString();
          string message = "PossibleHourDiagnosticYears = " + str1 + "; LoRaDiagnosticCounts = " + str2;
          nlogLogger.Trace(message);
        }
      }
      this.LogResultsAndExceptionOnSet();
    }
  }
}
