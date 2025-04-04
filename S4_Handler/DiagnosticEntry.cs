// Decompiled with JetBrains decompiler
// Type: S4_Handler.DiagnosticEntry
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using S4_Handler.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler
{
  public class DiagnosticEntry : IComparable<DiagnosticEntry>
  {
    internal DateTime DateAndTime;
    internal TimeSpan Interval;
    internal DiagnosticVolumeEntry Volume;
    internal DiagnosticTemperatureEntry Temperature;
    internal DiagnosticStatusEntry Status;
    internal S4_BaseUnits BaseUnit;

    internal static void AddDiagnostic(
      S4_DeviceMemory deviceMemory,
      List<DiagnosticEntry> resultList,
      S4_Params parameter,
      TimeSpan interval,
      S4_BaseUnits baseUnit)
    {
      uint parameterAddress = deviceMemory.GetParameterAddress(parameter);
      AddressRange testRange = new AddressRange(parameterAddress, 56U);
      if (!deviceMemory.AreDataAvailable(testRange))
        return;
      bool flag = true;
      if (deviceMemory.GetParameterAddressRange(S4_Params.diag_quarter).ByteSize == 64U)
        flag = false;
      DiagnosticEntry diagnosticEntry = new DiagnosticEntry();
      diagnosticEntry.BaseUnit = baseUnit;
      TimeSpan fromFirmwareTimeBcd = FirmwareDateTimeSupport.ToTimeSpanFromFirmwareTimeBCD(deviceMemory.GetValue<uint>(parameterAddress));
      uint address1 = parameterAddress + 4U;
      DateTime? fromFirmwareDateBcd = FirmwareDateTimeSupport.ToDateTimeFromFirmwareDateBCD(deviceMemory.GetValue<uint>(address1));
      uint address2 = address1 + 4U;
      DateTime? nullable1 = fromFirmwareDateBcd;
      TimeSpan timeSpan = fromFirmwareTimeBcd;
      DateTime? nullable2 = nullable1.HasValue ? new DateTime?(nullable1.GetValueOrDefault() + timeSpan) : new DateTime?();
      diagnosticEntry.DateAndTime = !nullable2.HasValue ? DateTime.MinValue : nullable2.Value;
      diagnosticEntry.Interval = interval;
      if (!flag)
        address2 += 8U;
      DiagnosticVolumeEntry diagnosticVolumeEntry = new DiagnosticVolumeEntry();
      DiagnosticTemperatureEntry temperatureEntry = new DiagnosticTemperatureEntry();
      DiagnosticStatusEntry diagnosticStatusEntry = new DiagnosticStatusEntry();
      if (flag)
      {
        temperatureEntry.threeValueLogger = true;
        diagnosticStatusEntry.threeValueLogger = true;
      }
      diagnosticVolumeEntry.Last = deviceMemory.GetValue<double>(address2);
      uint address3 = address2 + 8U;
      diagnosticVolumeEntry.SecondLast = deviceMemory.GetValue<double>(address3);
      uint address4 = address3 + 8U;
      uint address5;
      if (flag)
      {
        diagnosticVolumeEntry.ThirdLast = deviceMemory.GetValue<double>(address4);
        address5 = address4 + 8U;
      }
      else
        address5 = address4 + 4U + 4U;
      diagnosticEntry.Volume = diagnosticVolumeEntry;
      temperatureEntry.Last = DiagnosticEntry.TempValue(deviceMemory.GetValue<short>(address5));
      uint address6 = address5 + 2U;
      temperatureEntry.SecondLast = DiagnosticEntry.TempValue(deviceMemory.GetValue<short>(address6));
      uint address7 = address6 + 2U;
      uint num;
      if (flag)
      {
        temperatureEntry.ThirdLast = DiagnosticEntry.TempValue(deviceMemory.GetValue<short>(address7));
        uint address8 = address7 + 2U;
        diagnosticStatusEntry.Last = (uint) deviceMemory.GetValue<ushort>(address8);
        uint address9 = address8 + 2U;
        diagnosticStatusEntry.SecondLast = (uint) deviceMemory.GetValue<ushort>(address9);
        uint address10 = address9 + 2U;
        diagnosticStatusEntry.ThirdLast = (uint) deviceMemory.GetValue<ushort>(address10);
        num = address10 + 2U;
      }
      else
      {
        uint address11 = address7 + 2U + 2U;
        diagnosticStatusEntry.Last = deviceMemory.GetValue<uint>(address11);
        uint address12 = address11 + 4U;
        diagnosticStatusEntry.SecondLast = deviceMemory.GetValue<uint>(address12);
        uint address13 = address12 + 4U;
        diagnosticStatusEntry.ThirdLast = deviceMemory.GetValue<uint>(address13);
        num = address13 + 4U;
      }
      diagnosticEntry.Temperature = temperatureEntry;
      diagnosticEntry.Status = diagnosticStatusEntry;
      resultList.Add(diagnosticEntry);
    }

    private static float TempValue(short tempSource)
    {
      return tempSource == short.MinValue ? float.NaN : (float) tempSource / 100f;
    }

    public string ToString(DiagnosticEntry.ToStringUnits units)
    {
      if (units == DiagnosticEntry.ToStringUnits.VisibleUnits)
        return this.ToString();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("TimePoint = " + this.GetTimePointString(this.DateAndTime));
      stringBuilder.Append("Interval = ");
      if (this.Interval.TotalDays == 1.0)
        stringBuilder.AppendLine("Day");
      else if (this.Interval.TotalDays == 30.0)
        stringBuilder.AppendLine("Month");
      else if (this.Interval.TotalDays == 365.0)
        stringBuilder.AppendLine("Year");
      else
        stringBuilder.AppendLine(this.Interval.ToString());
      stringBuilder.AppendLine("Last values:");
      stringBuilder.AppendLine("   Volume: .... " + DiagnosticEntry.GetVolumeString(this.Volume.Last, " m\u00B3"));
      stringBuilder.AppendLine("   Temperature: " + DiagnosticEntry.GetTemperatureString(this.Temperature.Last, " °C"));
      stringBuilder.AppendLine("   State: ..... " + this.Status.ToString(this.Status.Last));
      DateTime lastDate1 = this.GetLastDate(this.DateAndTime, this.Interval);
      stringBuilder.AppendLine("Second last values: (" + this.GetTimePointString(lastDate1) + ")");
      stringBuilder.AppendLine("   Volume: .... " + DiagnosticEntry.GetVolumeString(this.Volume.SecondLast, " m\u00B3"));
      stringBuilder.AppendLine("   Temperature: " + DiagnosticEntry.GetTemperatureString(this.Temperature.SecondLast, " °C"));
      stringBuilder.AppendLine("   State: ..... " + this.Status.ToString(this.Status.SecondLast));
      if (this.Status.threeValueLogger)
      {
        DateTime lastDate2 = this.GetLastDate(lastDate1, this.Interval);
        stringBuilder.AppendLine("Third last values: (" + this.GetTimePointString(lastDate2) + ")");
        stringBuilder.AppendLine("   Volume: .... " + DiagnosticEntry.GetVolumeString(this.Volume.ThirdLast, " m\u00B3/h"));
        stringBuilder.AppendLine("   Temperature: " + DiagnosticEntry.GetTemperatureString(this.Temperature.ThirdLast, " °C"));
        stringBuilder.AppendLine("   State: ..... " + this.Status.ToString(this.Status.ThirdLast));
      }
      return stringBuilder.ToString();
    }

    private string GetTimePointString(DateTime dateTime)
    {
      return dateTime > DateTime.MinValue ? dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString() : "invalid";
    }

    private DateTime GetLastDate(DateTime date, TimeSpan interval)
    {
      if (date == DateTime.MinValue)
        return date;
      if (!(interval >= S4_DiagnosticData.MonthInterval))
        return date - interval;
      if (interval == S4_DiagnosticData.MonthInterval)
        return date.AddMonths(-1);
      if (interval == S4_DiagnosticData.YearInterval)
        return date.AddYears(-1);
      throw new Exception("Not supported interval found");
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("TimePoint = " + this.GetTimePointString(this.DateAndTime));
      stringBuilder.AppendLine("Interval = " + this.Interval.ToString());
      DiagnosticVolumeEntry valuesForBaseUnit1 = this.Volume.GetValuesForBaseUnit(this.BaseUnit);
      DiagnosticTemperatureEntry valuesForBaseUnit2 = this.Temperature.GetValuesForBaseUnit(this.BaseUnit);
      stringBuilder.AppendLine("Last values:");
      stringBuilder.AppendLine("   Volume: .... " + DiagnosticEntry.GetVolumeString(valuesForBaseUnit1.Last, valuesForBaseUnit1.VolumeUnitString));
      stringBuilder.AppendLine("   Temperature: " + DiagnosticEntry.GetTemperatureString(valuesForBaseUnit2.Last, valuesForBaseUnit2.UnitString));
      stringBuilder.AppendLine("   State: ..... " + this.Status.ToString(this.Status.Last));
      DateTime lastDate1 = this.GetLastDate(this.DateAndTime, this.Interval);
      stringBuilder.AppendLine("Second last values: (" + this.GetTimePointString(lastDate1) + ")");
      stringBuilder.AppendLine("   Volume: .... " + DiagnosticEntry.GetVolumeString(valuesForBaseUnit1.SecondLast, valuesForBaseUnit1.VolumeUnitString));
      stringBuilder.AppendLine("   Temperature: " + DiagnosticEntry.GetTemperatureString(valuesForBaseUnit2.SecondLast, valuesForBaseUnit2.UnitString));
      stringBuilder.AppendLine("   State: ..... " + this.Status.ToString(this.Status.SecondLast));
      DateTime lastDate2 = this.GetLastDate(lastDate1, this.Interval);
      stringBuilder.AppendLine("Third last values: (" + this.GetTimePointString(lastDate2) + ")");
      stringBuilder.AppendLine("   Volume: .... " + DiagnosticEntry.GetVolumeString(valuesForBaseUnit1.ThirdLast, valuesForBaseUnit1.FlowUnitString));
      stringBuilder.AppendLine("   Temperature: " + DiagnosticEntry.GetTemperatureString(valuesForBaseUnit2.ThirdLast, valuesForBaseUnit2.UnitString));
      stringBuilder.AppendLine("   State: ..... " + this.Status.ToString(this.Status.ThirdLast));
      return stringBuilder.ToString();
    }

    internal static string GetVolumeString(double volume, string unitString)
    {
      return double.IsNaN(volume) ? "invalid" : volume.ToString() + unitString;
    }

    internal static string GetTemperatureString(float temperature, string unitString)
    {
      return float.IsNaN(temperature) ? "invalid" : temperature.ToString() + unitString;
    }

    public int CompareTo(DiagnosticEntry obj)
    {
      if (obj == null)
        return 1;
      TimeSpan interval1 = this.Interval;
      TimeSpan interval2 = obj.Interval;
      if (true)
      {
        int num = this.Interval.CompareTo(obj.Interval);
        if (num != 0)
          return num;
      }
      DateTime dateAndTime1 = this.DateAndTime;
      DateTime dateAndTime2 = obj.DateAndTime;
      if (true)
      {
        int num = this.DateAndTime.CompareTo(obj.DateAndTime);
        if (num != 0)
          return num;
      }
      return 0;
    }

    public enum ToStringUnits
    {
      InternalUnits,
      VisibleUnits,
    }
  }
}
