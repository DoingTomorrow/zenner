// Decompiled with JetBrains decompiler
// Type: HandlerLib.SmartFunctions_EDC
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace HandlerLib
{
  internal static class SmartFunctions_EDC
  {
    public static Dictionary<string, int> WaterMeterRollersInLiterCogCountEDC = new Dictionary<string, int>()
    {
      {
        "8R",
        8
      },
      {
        "6R+3ZK",
        9
      },
      {
        "6R+3ZK+1V",
        10
      }
    };

    public static bool CalculateSmartFunctionsParamEDCByQ3ANDVolumeUnit(
      string volWaterMeterPulseValueInLiterString,
      string waterMeterNominalFlowQMPerHourString,
      out int burstDiff,
      out int undersizeDiff,
      out int OversizeDiff,
      out int leak_lower,
      out int leak_upper)
    {
      burstDiff = int.MinValue;
      undersizeDiff = int.MinValue;
      OversizeDiff = int.MinValue;
      leak_lower = -1900;
      leak_upper = 1900;
      double Q3InQMPerHour = double.MinValue;
      if (!SmartFunctions_EDC.GetQ3FlowQMPerHourFromCacheString(waterMeterNominalFlowQMPerHourString, out Q3InQMPerHour))
        return false;
      double waterMeterNominalFlowLiter = Q3InQMPerHour * 1000.0;
      double wmDiscMultiplierLiterPerRotation = double.MinValue;
      return SmartFunctions_EDC.GetvolMeterPulseValueFromCacheString(volWaterMeterPulseValueInLiterString, out wmDiscMultiplierLiterPerRotation) && SmartFunctions_EDC.CalculateSmartFunctionsParamEDCByQ3ANDVolumeUnit(wmDiscMultiplierLiterPerRotation, waterMeterNominalFlowLiter, out burstDiff, out undersizeDiff, out OversizeDiff, out leak_lower, out leak_upper);
    }

    private static bool CalculateSmartFunctionsParamEDCByQ3ANDVolumeUnit(
      double WaterMeterPulseValueInLiter,
      double waterMeterNominalFlowLiter,
      out int burstDiff,
      out int undersizeDiff,
      out int OversizeDiff,
      out int leak_lower,
      out int leak_upper)
    {
      burstDiff = (int) Convert.ToUInt16(0.3 * waterMeterNominalFlowLiter / WaterMeterPulseValueInLiter / 4.0);
      undersizeDiff = (int) Convert.ToUInt16(waterMeterNominalFlowLiter / WaterMeterPulseValueInLiter / 4.0);
      OversizeDiff = (int) Convert.ToUInt16(0.1 * waterMeterNominalFlowLiter / WaterMeterPulseValueInLiter / 4.0);
      leak_lower = -1900;
      leak_upper = 1900;
      return true;
    }

    private static bool calculateRotationSpeedDiscInRotationsPerSecond(
      string FlowLiterPerHourWaterMeterCacheString,
      string PulseMultiplierWaterMeterLiterPerRotationCacheString,
      out double equivalentRotationSpeedDiscEDC)
    {
      equivalentRotationSpeedDiscEDC = double.MinValue;
      double Q3InQMPerHour = double.MinValue;
      if (!SmartFunctions_EDC.GetQ3FlowQMPerHourFromCacheString(FlowLiterPerHourWaterMeterCacheString, out Q3InQMPerHour))
        return false;
      double FlowLiterPerHourWaterMeter = Q3InQMPerHour * 1000.0;
      double wmDiscMultiplierLiterPerRotation = double.MinValue;
      if (!SmartFunctions_EDC.GetvolMeterPulseValueFromCacheString(PulseMultiplierWaterMeterLiterPerRotationCacheString, out wmDiscMultiplierLiterPerRotation))
        return false;
      equivalentRotationSpeedDiscEDC = SmartFunctions_EDC.calculateRotationSpeedDiscInRotationsPerSecond(FlowLiterPerHourWaterMeter, wmDiscMultiplierLiterPerRotation);
      return true;
    }

    private static double calculateRotationSpeedDiscInRotationsPerSecond(
      double FlowLiterPerHourWaterMeter,
      double PulseMultiplierWaterMeterLiterPerRotation)
    {
      return FlowLiterPerHourWaterMeter / 3600.0 / PulseMultiplierWaterMeterLiterPerRotation;
    }

    private static uint countdigits(uint x)
    {
      uint num1 = 1;
      uint num2 = 10;
      while (x >= num2)
      {
        num2 *= 10U;
        ++num1;
      }
      return num1;
    }

    public static bool CalculateMultiplierEDC(
      int waterMeterMultiplierLiterPerRotation,
      int edcVolumeUnitInLiter,
      out int edcMultiplier)
    {
      edcMultiplier = int.MinValue;
      double d = (double) waterMeterMultiplierLiterPerRotation / (double) edcVolumeUnitInLiter;
      if (Math.Floor(d) / d != 1.0)
        return false;
      edcMultiplier = (int) d;
      if (edcMultiplier >= 1)
        return true;
      edcMultiplier = int.MinValue;
      return false;
    }

    public static bool CalculateCogCountEDC(
      string numberOfRollsCache,
      uint volumeUnitEDCInLiter,
      out int cogCountEDC)
    {
      int num;
      if (!SmartFunctions_EDC.WaterMeterRollersInLiterCogCountEDC.TryGetValue(numberOfRollsCache, out num))
      {
        cogCountEDC = int.MinValue;
        return false;
      }
      cogCountEDC = num - ((int) SmartFunctions_EDC.countdigits(volumeUnitEDCInLiter) - 1);
      return true;
    }

    private static bool GetQ3FlowQMPerHourFromCacheString(
      string flowInQMPerHour,
      out double Q3InQMPerHour)
    {
      Q3InQMPerHour = double.MinValue;
      if (!flowInQMPerHour.Contains("m\u00B3/h"))
        return false;
      if (flowInQMPerHour.Contains("Q3 "))
        flowInQMPerHour = flowInQMPerHour.Replace("Q3 ", "");
      return !flowInQMPerHour.Contains(",") && double.TryParse(Regex.Replace(flowInQMPerHour, "[^0-9.]", ""), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out Q3InQMPerHour);
    }

    private static bool GetEDCVolumeUnitFromCacheString(
      string edcVolumeUnit,
      out double edcVolumeUnitValue)
    {
      edcVolumeUnitValue = double.MinValue;
      return (edcVolumeUnit.Contains("L") || edcVolumeUnit.Contains("l")) && !edcVolumeUnit.Contains(",") && double.TryParse(Regex.Replace(edcVolumeUnit, "[^0-9.]", ""), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out edcVolumeUnitValue);
    }

    public static bool GetvolMeterPulseValueFromCacheString(
      string volMeterPulseValueInLiterPerRotation,
      out double wmDiscMultiplierLiterPerRotation)
    {
      wmDiscMultiplierLiterPerRotation = double.MinValue;
      return (volMeterPulseValueInLiterPerRotation.Contains("L") || volMeterPulseValueInLiterPerRotation.Contains("l")) && !volMeterPulseValueInLiterPerRotation.Contains(",") && double.TryParse(Regex.Replace(volMeterPulseValueInLiterPerRotation, "[^0-9.]", ""), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out wmDiscMultiplierLiterPerRotation);
    }
  }
}
