// Decompiled with JetBrains decompiler
// Type: HandlerLib.VolumeCalibrationValues
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class VolumeCalibrationValues
  {
    public SortedList<double, double> CalibrationFactors;

    public VolumeCalibrationValues(ConfigurationParameterListSupport confListSupport)
    {
      double key1 = double.NaN;
      double num1 = double.NaN;
      double key2 = double.NaN;
      double num2 = double.NaN;
      double key3 = double.NaN;
      double num3 = double.NaN;
      ConfigurationParameter parameterFromList1 = confListSupport.GetWorkParameterFromList(OverrideID.CalVolMaxFlowLiterPerHour);
      if (parameterFromList1 != null && parameterFromList1.ParameterValue != null && ((double) parameterFromList1.ParameterValue).CompareTo(double.NaN) != 0)
        key1 = (double) parameterFromList1.ParameterValue / 1000.0;
      ConfigurationParameter parameterFromList2 = confListSupport.GetWorkParameterFromList(OverrideID.CalVolMaxErrorPercent);
      if (parameterFromList2 != null && parameterFromList2.ParameterValue != null && ((double) parameterFromList2.ParameterValue).CompareTo(double.NaN) != 0)
      {
        if (key1.CompareTo(double.NaN) == 0)
          throw new ArgumentException("CalVolMaxFlowLiterPerHour cannot be NaN if CalVolMaxErrorPercent is set");
        num1 = (double) parameterFromList2.ParameterValue;
      }
      else if (key1.CompareTo(double.NaN) != 0)
        throw new ArgumentException("CalVolMaxErrorPercent cannot be NaN if CalVolMaxFlowLiterPerHour is set");
      ConfigurationParameter parameterFromList3 = confListSupport.GetWorkParameterFromList(OverrideID.CalVolNominalFlowLiterPerHour);
      if (parameterFromList3 != null && parameterFromList3.ParameterValue != null && ((double) parameterFromList3.ParameterValue).CompareTo(double.NaN) != 0)
      {
        key2 = (double) parameterFromList3.ParameterValue / 1000.0;
        if (key1.CompareTo(double.NaN) != 0 && key1 <= key2)
          throw new ArgumentException("CalVolMaxFlowLiterPerHour has to be higher then CalVolNominalFlowLiterPerHour");
      }
      ConfigurationParameter parameterFromList4 = confListSupport.GetWorkParameterFromList(OverrideID.CalVolNominalErrorPercent);
      if (parameterFromList4 != null && parameterFromList4.ParameterValue != null && ((double) parameterFromList4.ParameterValue).CompareTo(double.NaN) != 0)
      {
        if (key2.CompareTo(double.NaN) == 0)
          throw new ArgumentException("CalVolNominalFlowLiterPerHour cannot be NaN if CalVolNominalErrorPercent is set");
        num2 = (double) parameterFromList4.ParameterValue;
      }
      else if (key2.CompareTo(double.NaN) != 0)
        throw new ArgumentException("CalVolNominalErrorPercent cannot be NaN if CalVolNominalFlowLiterPerHour is set");
      ConfigurationParameter parameterFromList5 = confListSupport.GetWorkParameterFromList(OverrideID.CalVolMinFlowLiterPerHour);
      if (parameterFromList5 != null && parameterFromList5.ParameterValue != null && ((double) parameterFromList5.ParameterValue).CompareTo(double.NaN) != 0)
      {
        key3 = (double) parameterFromList5.ParameterValue / 1000.0;
        if (key1.CompareTo(double.NaN) != 0 && key1 <= key3)
          throw new ArgumentException("CalVolMaxFlowLiterPerHour has to be higher then CalVolMinFlowLiterPerHour");
        if (key2.CompareTo(double.NaN) != 0 && key2 <= key3)
          throw new ArgumentException("CalVolNominalFlowLiterPerHour has to be higher then CalVolMinFlowLiterPerHour");
      }
      ConfigurationParameter parameterFromList6 = confListSupport.GetWorkParameterFromList(OverrideID.CalVolMinErrorPercent);
      if (parameterFromList6 != null && parameterFromList6.ParameterValue != null && ((double) parameterFromList6.ParameterValue).CompareTo(double.NaN) != 0)
      {
        if (key3.CompareTo(double.NaN) == 0)
          throw new ArgumentException("CalVolMinFlowLiterPerHour cannot be NaN if CalVolMinErrorPercent is set");
        num3 = (double) parameterFromList6.ParameterValue;
      }
      else if (key3.CompareTo(double.NaN) != 0)
        throw new ArgumentException("CalVolMinErrorPercent cannot be NaN if CalVolMinFlowLiterPerHour is set");
      this.CalibrationFactors = new SortedList<double, double>();
      if (key1.CompareTo(double.NaN) != 0)
        this.CalibrationFactors.Add(key1, 100.0 / (100.0 + num1));
      if (key2.CompareTo(double.NaN) != 0)
        this.CalibrationFactors.Add(key2, 100.0 / (100.0 + num2));
      if (key3.CompareTo(double.NaN) == 0)
        return;
      this.CalibrationFactors.Add(key3, 100.0 / (100.0 + num3));
    }
  }
}
