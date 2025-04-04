// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_DiagnosticData
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using S4_Handler.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler
{
  internal class S4_DiagnosticData
  {
    internal const short InvalidShort = -32768;
    internal const uint InvalidUint = 2147483648;
    internal static TimeSpan MonthInterval = new TimeSpan(30, 0, 0, 0);
    internal static TimeSpan YearInterval = new TimeSpan(365, 0, 0, 0);
    internal List<DiagnosticEntry> DiagnosticData = new List<DiagnosticEntry>();
    internal S4_BaseUnits BaseUnit;

    internal void LoadData(S4_DeviceMemory deviceMemory)
    {
      S4_MenuManager s4MenuManager = new S4_MenuManager(deviceMemory);
      s4MenuManager.GetResolution();
      this.BaseUnit = s4MenuManager.GetBaseUnit();
      DiagnosticEntry.AddDiagnostic(deviceMemory, this.DiagnosticData, S4_Params.diag_quarter, new TimeSpan(0, 15, 0), this.BaseUnit);
      DiagnosticEntry.AddDiagnostic(deviceMemory, this.DiagnosticData, S4_Params.diag_hour, new TimeSpan(1, 0, 0), this.BaseUnit);
      DiagnosticEntry.AddDiagnostic(deviceMemory, this.DiagnosticData, S4_Params.diag_6hours, new TimeSpan(6, 0, 0), this.BaseUnit);
      DiagnosticEntry.AddDiagnostic(deviceMemory, this.DiagnosticData, S4_Params.diag_day, new TimeSpan(1, 0, 0, 0), this.BaseUnit);
      if (deviceMemory.IsParameterAvailable(S4_Params.diag_month_d1))
        DiagnosticEntry.AddDiagnostic(deviceMemory, this.DiagnosticData, S4_Params.diag_month_d1, S4_DiagnosticData.MonthInterval, this.BaseUnit);
      if (deviceMemory.IsParameterAvailable(S4_Params.diag_month_d15))
        DiagnosticEntry.AddDiagnostic(deviceMemory, this.DiagnosticData, S4_Params.diag_month_d15, S4_DiagnosticData.MonthInterval, this.BaseUnit);
      DiagnosticEntry.AddDiagnostic(deviceMemory, this.DiagnosticData, S4_Params.diag_year, S4_DiagnosticData.YearInterval, this.BaseUnit);
      this.DiagnosticData.Sort();
    }

    internal async Task ReadAndLoadDataAsync(
      S4_DeviceMemory deviceMemory,
      S4_DeviceCommandsNFC deviceCommand,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<S4_Params> digParams = new List<S4_Params>();
      digParams.Add(S4_Params.diag_quarter);
      digParams.Add(S4_Params.diag_hour);
      digParams.Add(S4_Params.diag_6hours);
      digParams.Add(S4_Params.diag_day);
      if (deviceMemory.IsParameterAvailable(S4_Params.diag_month_d1))
        digParams.Add(S4_Params.diag_month_d1);
      if (deviceMemory.IsParameterAvailable(S4_Params.diag_month_d15))
        digParams.Add(S4_Params.diag_month_d15);
      digParams.Add(S4_Params.diag_year);
      AddressRange range = deviceMemory.GetRangeOfParameters(digParams.ToArray());
      await deviceCommand.CommonNfcCommands.ReadMemoryAsync(range, (DeviceMemory) deviceMemory, progress, cancelToken);
      this.LoadData(deviceMemory);
      digParams = (List<S4_Params>) null;
      range = (AddressRange) null;
    }

    internal static void InitDiagnosticData(
      S4_DeviceMemory deviceMemory,
      DateTime theTime,
      double volumeQm = 0.0,
      bool SetTestPattern = false)
    {
      DateTime setTime = theTime;
      S4_DiagnosticData.TestValueHelper tvh;
      if (theTime > DateTime.MinValue)
      {
        setTime = new DateTime(theTime.Year, theTime.Month, theTime.Day, theTime.Hour, theTime.Minute / 15 * 15, 0);
        tvh = new S4_DiagnosticData.TestValueHelper(volumeQm, SetTestPattern);
      }
      else
        tvh = new S4_DiagnosticData.TestValueHelper(double.NaN, SetTestPattern);
      bool threeValueLogger = true;
      AddressRange parameterAddressRange = deviceMemory.GetParameterAddressRange(S4_Params.diag_quarter);
      if (parameterAddressRange.ByteSize == 64U)
        threeValueLogger = false;
      uint writeAddress = parameterAddressRange.StartAddress;
      S4_DiagnosticData.InitDiagnosticVolumeUnit(deviceMemory, tvh, setTime, ref writeAddress, threeValueLogger);
      if (theTime > DateTime.MinValue)
        setTime = new DateTime(theTime.Year, theTime.Month, theTime.Day, theTime.Hour, 0, 0);
      writeAddress = deviceMemory.GetParameterAddress(S4_Params.diag_hour);
      S4_DiagnosticData.InitDiagnosticVolumeUnit(deviceMemory, tvh, setTime, ref writeAddress, threeValueLogger);
      if (theTime > DateTime.MinValue)
        setTime = new DateTime(theTime.Year, theTime.Month, theTime.Day, theTime.Hour / 6 * 6, 0, 0);
      writeAddress = deviceMemory.GetParameterAddress(S4_Params.diag_6hours);
      S4_DiagnosticData.InitDiagnosticVolumeUnit(deviceMemory, tvh, setTime, ref writeAddress, threeValueLogger);
      if (theTime > DateTime.MinValue)
        setTime = new DateTime(theTime.Year, theTime.Month, theTime.Day);
      writeAddress = deviceMemory.GetParameterAddress(S4_Params.diag_day);
      S4_DiagnosticData.InitDiagnosticVolumeUnit(deviceMemory, tvh, setTime, ref writeAddress, threeValueLogger);
      if (deviceMemory.IsParameterAvailable(S4_Params.diag_month_d1))
      {
        if (theTime > DateTime.MinValue)
          setTime = new DateTime(theTime.Year, theTime.Month, 1);
        writeAddress = deviceMemory.GetParameterAddress(S4_Params.diag_month_d1);
        S4_DiagnosticData.InitDiagnosticVolumeUnit(deviceMemory, tvh, setTime, ref writeAddress, threeValueLogger);
      }
      if (!deviceMemory.IsParameterAvailable(S4_Params.diag_month_d15))
        return;
      if (theTime > DateTime.MinValue)
      {
        setTime = new DateTime(theTime.Year, theTime.Month, 15);
        if (setTime > theTime)
          setTime.AddMonths(-1);
      }
      writeAddress = deviceMemory.GetParameterAddress(S4_Params.diag_month_d15);
      S4_DiagnosticData.InitDiagnosticVolumeUnit(deviceMemory, tvh, setTime, ref writeAddress, threeValueLogger);
    }

    private static void InitDiagnosticVolumeUnit(
      S4_DeviceMemory deviceMemory,
      S4_DiagnosticData.TestValueHelper tvh,
      DateTime setTime,
      ref uint writeAddress,
      bool threeValueLogger)
    {
      deviceMemory.SetValue<uint>(FirmwareDateTimeSupport.ToFirmwareTimeBCD(setTime), writeAddress);
      writeAddress += 4U;
      deviceMemory.SetValue<uint>(FirmwareDateTimeSupport.ToFirmwareDateBCD(setTime), writeAddress);
      writeAddress += 4U;
      if (!threeValueLogger)
        writeAddress += 8U;
      deviceMemory.SetValue<double>(tvh.Value, writeAddress);
      writeAddress += 8U;
      deviceMemory.SetValue<double>(tvh.Value, writeAddress);
      writeAddress += 8U;
      if (threeValueLogger)
      {
        deviceMemory.SetValue<double>(tvh.Value, writeAddress);
        writeAddress += 8U;
        S4_DiagnosticData.InitTempAndState(deviceMemory, tvh, writeAddress, threeValueLogger);
      }
      else
      {
        deviceMemory.SetValue<float>((float) tvh.ZeroInitValue, writeAddress);
        writeAddress += 4U;
        S4_DiagnosticData.InitTempAndState(deviceMemory, tvh, writeAddress, threeValueLogger);
      }
    }

    private static void InitTempAndState(
      S4_DeviceMemory deviceMemory,
      S4_DiagnosticData.TestValueHelper tvh,
      uint writeAddress,
      bool threeValueLogger)
    {
      if (!threeValueLogger)
        writeAddress += 4U;
      deviceMemory.SetValue<short>(tvh.Int16Value, writeAddress);
      writeAddress += 2U;
      deviceMemory.SetValue<short>(tvh.Int16Value, writeAddress);
      writeAddress += 2U;
      if (threeValueLogger)
      {
        deviceMemory.SetValue<short>(tvh.Int16Value, writeAddress);
        writeAddress += 2U;
        deviceMemory.SetValue<short>(tvh.Int16Value, writeAddress);
        writeAddress += 2U;
        deviceMemory.SetValue<short>(tvh.Int16Value, writeAddress);
        writeAddress += 2U;
        deviceMemory.SetValue<short>(tvh.Int16Value, writeAddress);
        writeAddress += 2U;
      }
      else
      {
        deviceMemory.SetValue<short>(tvh.Int16Value, writeAddress);
        writeAddress += 2U;
        writeAddress += 2U;
        deviceMemory.SetValue<uint>(0U, writeAddress);
        writeAddress += 4U;
        deviceMemory.SetValue<uint>(0U, writeAddress);
        writeAddress += 4U;
        deviceMemory.SetValue<uint>(0U, writeAddress);
        writeAddress += 4U;
      }
    }

    internal static void InitKeyDateData(
      S4_DeviceMemory deviceMemory,
      DateTime keyDate,
      double volumeQm = 0.0,
      bool SetTestPattern = false)
    {
      S4_DiagnosticData.TestValueHelper tvh;
      if (keyDate > DateTime.MinValue)
      {
        uint firmwareDateBcd = FirmwareDateTimeSupport.ToFirmwareDateBCD(keyDate);
        deviceMemory.SetParameterValue<uint>(S4_Params.Key_Date, firmwareDateBcd);
        tvh = new S4_DiagnosticData.TestValueHelper(volumeQm, SetTestPattern);
      }
      else
      {
        DateTime? fromFirmwareDateBcd = FirmwareDateTimeSupport.ToDateTimeFromFirmwareDateBCD(deviceMemory.GetParameterValue<uint>(S4_Params.Key_Date));
        int num;
        if (fromFirmwareDateBcd.HasValue)
        {
          DateTime? nullable = fromFirmwareDateBcd;
          DateTime now = DateTime.Now;
          num = nullable.HasValue ? (nullable.GetValueOrDefault() < now ? 1 : 0) : 0;
        }
        else
          num = 1;
        if (num != 0)
        {
          keyDate = new DateTime(DateTime.Now.Year, 1, 1);
          if (keyDate < DateTime.Now)
            keyDate.AddYears(1);
          uint firmwareDateBcd = FirmwareDateTimeSupport.ToFirmwareDateBCD(keyDate);
          deviceMemory.SetParameterValue<uint>(S4_Params.Key_Date, firmwareDateBcd);
        }
        else
          keyDate = fromFirmwareDateBcd.Value;
        tvh = new S4_DiagnosticData.TestValueHelper(double.NaN, SetTestPattern);
      }
      bool threeValueLogger = true;
      if (deviceMemory.GetParameterAddressRange(S4_Params.diag_quarter).ByteSize == 64U)
        threeValueLogger = false;
      uint parameterAddress = deviceMemory.GetParameterAddress(S4_Params.diag_year);
      S4_DiagnosticData.InitDiagnosticVolumeUnit(deviceMemory, tvh, keyDate, ref parameterAddress, threeValueLogger);
    }

    public override string ToString() => this.ToString(DiagnosticEntry.ToStringUnits.VisibleUnits);

    public string ToString(DiagnosticEntry.ToStringUnits units)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (DiagnosticEntry diagnosticEntry in this.DiagnosticData)
      {
        stringBuilder.AppendLine("*************************************************************");
        stringBuilder.AppendLine(diagnosticEntry.ToString(units));
      }
      return stringBuilder.ToString();
    }

    internal class TestValueHelper
    {
      private double SetValue;
      private bool SetTestPattern;

      internal TestValueHelper(double setValue, bool setTestPattern)
      {
        this.SetValue = setValue;
        this.SetTestPattern = setTestPattern;
      }

      internal double Value
      {
        get
        {
          if (this.SetTestPattern && !double.IsNaN(this.SetValue))
            this.SetValue += 100.1;
          return this.SetValue;
        }
      }

      internal int Int32Value
      {
        get
        {
          if (double.IsNaN(this.SetValue))
            return int.MinValue;
          if (this.SetTestPattern)
            this.SetValue += 100.1;
          return (int) this.SetValue;
        }
      }

      internal short Int16Value
      {
        get
        {
          if (double.IsNaN(this.SetValue))
            return short.MinValue;
          if (this.SetTestPattern)
            this.SetValue += 100.1;
          return (short) this.SetValue;
        }
      }

      internal double ZeroInitValue
      {
        get
        {
          if (double.IsNaN(this.SetValue))
            return double.NaN;
          if (!this.SetTestPattern)
            return 0.0;
          this.SetValue += 100.1;
          return this.SetValue;
        }
      }
    }
  }
}
