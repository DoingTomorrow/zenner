// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_BatteryEnergyManagement
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using NLog;
using S4_Handler.Functions;
using System;

#nullable disable
namespace S4_Handler
{
  internal class S4_BatteryEnergyManagement : BatteryEnergyManagement
  {
    internal static Logger S4_SetBatteryEnergyLogger = LogManager.GetLogger("S4_SetBatteryEnergy");
    internal static Logger S4_GetBatteryEnergyLogger = LogManager.GetLogger("S4_GetBatteryEnergy");

    internal S4_BatteryEnergyManagement(
      DateTime deviceTime,
      DateTime batteryEndDate,
      double batteryNominalCapacity_mAh,
      TdcChannelConfig tdcChannelConfig,
      ushort communicationScenario1,
      ushort communicationScenario2,
      bool setConfig)
      : base(batteryNominalCapacity_mAh, deviceTime)
    {
      this.VolumeCycle_s = 2.0;
      this.MinRequiredBatteryEndDate = batteryEndDate;
      this.StandbyEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyForTime_mAh(0.02, 31536000.0);
      double tdcBatteryFactor = tdcChannelConfig.GetTdcBatteryFactor();
      if (tdcChannelConfig.TwoTransducersUsed)
        this.VolumeCycleEnergy_mAh_per_cycle = BatteryEnergyManagement.GetEnergyForTime_mAh(0.05, this.VolumeCycle_s);
      else
        this.VolumeCycleEnergy_mAh_per_cycle = BatteryEnergyManagement.GetEnergyForTime_mAh(0.035, this.VolumeCycle_s);
      this.VolumeCycleEnergy_mAh_per_cycle *= tdcBatteryFactor;
      if (tdcChannelConfig.parallel_TOF_eval)
      {
        if (tdcChannelConfig.TwoTransducersUsed)
          this.VolumeCycleEnergy_mAh_per_cycle += BatteryEnergyManagement.GetEnergyForTime_mAh(0.004, this.VolumeCycle_s);
        else
          this.VolumeCycleEnergy_mAh_per_cycle += BatteryEnergyManagement.GetEnergyForTime_mAh(0.002, this.VolumeCycle_s);
      }
      this.VolumeCycleEnergy_mAh_per_year = BatteryEnergyManagement.GetEnergyPerYear_mAh(this.VolumeCycleEnergy_mAh_per_cycle, this.VolumeCycle_s);
      string message1 = string.Empty;
      if (communicationScenario1 > (ushort) 0)
      {
        ScenarioEnergy scenarioEnergyValues = ScenarioConfigurations.GetScenarioEnergyValues(communicationScenario1);
        this.RadioEnergy_mAh_per_year = scenarioEnergyValues.GetYearly_mAs(out message1);
        this.RadioCycle_s = scenarioEnergyValues.CycleSeconds;
      }
      string message2 = string.Empty;
      if (communicationScenario2 > (ushort) 0)
      {
        ScenarioEnergy scenarioEnergyValues = ScenarioConfigurations.GetScenarioEnergyValues(communicationScenario2);
        this.RadioEnergy_mAh_per_year += scenarioEnergyValues.GetYearly_mAs(out message2);
        double cycleSeconds = scenarioEnergyValues.CycleSeconds;
        if (this.RadioCycle_s == 0.0 || cycleSeconds < this.RadioCycle_s)
          this.RadioCycle_s = cycleSeconds;
      }
      this.StartNLogLogging(setConfig);
      if (this.NlogLogger.IsTraceEnabled)
      {
        if (communicationScenario1 > (ushort) 0)
          this.NlogLogger.Trace("Scenario = " + communicationScenario1.ToString() + "; " + message1);
        if (communicationScenario2 > (ushort) 0)
          this.NlogLogger.Trace("Scenario2 = " + communicationScenario2.ToString() + "; " + message2);
        this.NlogLogger.Trace("TwoTransducerPairs = " + tdcChannelConfig.TwoTransducersUsed.ToString() + "; TdcBatteryFactor = " + tdcBatteryFactor.ToString("0.000"));
      }
      this.LogResultsAndExceptionOnSet();
    }
  }
}
