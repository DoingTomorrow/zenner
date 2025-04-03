// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.AbstractWaveFlowLoggerConfigurator
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace ZR_ClassLibrary
{
  public abstract class AbstractWaveFlowLoggerConfigurator
  {
    public abstract string GetLastErrorString();

    public abstract int GetNumberOfLoggers();

    public abstract bool GetPossibleLoggerTypesForLoggerID(ref ArrayList LoggerTypes, int LoggerID);

    public abstract bool GetPossibleParametersForLoggerType(
      ref ArrayList TypeParameters,
      int LoggerID,
      string LoggerType);

    public abstract bool GetPossibleParametersSettings(
      ref ArrayList ParameterSettings,
      int LoggerID,
      string LoggerType,
      string Parameter);

    public abstract bool GetActualParameterValuesForLoggerIDAndLoggerType(
      int LoggerID,
      string LoggerType,
      ref SortedList<string, string> ParameterSettings);

    public abstract bool GetActualLoggerTypeForLoggerID(int LoggerID, out string LoggerType);

    public abstract bool SetLoggerTypeAndParameterValuesForLoggerID(
      int LoggerID,
      string LoggerType,
      SortedList<string, string> ParameterSettings);

    public abstract bool WriteSettingsToDevice();

    public abstract bool ReadSettingsFromDevice();

    public abstract bool ReadLoggerDataFromDevice(
      int LoggerID,
      ref SortedList<DateTime, List<double>> LoggerValues,
      ref List<MeterDBAccess.ValueTypes> TheValueTypes,
      ref List<string> InputNumbers,
      ref List<string> SerialNumbers);
  }
}
