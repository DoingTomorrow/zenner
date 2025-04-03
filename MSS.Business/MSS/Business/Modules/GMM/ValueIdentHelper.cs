// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.ValueIdentHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public static class ValueIdentHelper
  {
    public static IEnumerable<string> GetPhysicalQuantitiesEnumerable()
    {
      return (IEnumerable<string>) EnumHelper.GetEnumElements<ValueIdent.ValueIdPart_PhysicalQuantity>().Keys.OrderBy<string, string>((Func<string, string>) (s => s));
    }

    public static IEnumerable<string> GetMeterTypeEnumerable()
    {
      return (IEnumerable<string>) EnumHelper.GetEnumElements<ValueIdent.ValueIdPart_MeterType>().Keys.OrderBy<string, string>((Func<string, string>) (s => s));
    }

    public static IEnumerable<string> GetCalculationEnumerable()
    {
      return (IEnumerable<string>) EnumHelper.GetEnumElements<ValueIdent.ValueIdPart_Calculation>().Keys.OrderBy<string, string>((Func<string, string>) (s => s));
    }

    public static IEnumerable<string> GetCalculationStartEnumerable()
    {
      return (IEnumerable<string>) EnumHelper.GetEnumElements<ValueIdent.ValueIdPart_CalculationStart>().Keys.OrderBy<string, string>((Func<string, string>) (s => s));
    }

    public static IEnumerable<string> GetStorageIntervalEnumerable()
    {
      return (IEnumerable<string>) EnumHelper.GetEnumElements<ValueIdent.ValueIdPart_StorageInterval>().Keys.OrderBy<string, string>((Func<string, string>) (s => s));
    }

    public static IEnumerable<string> GetCreationEnumerable()
    {
      return (IEnumerable<string>) EnumHelper.GetEnumElements<ValueIdent.ValueIdPart_Creation>().Keys.OrderBy<string, string>((Func<string, string>) (s => s));
    }

    public static IEnumerable<ValueIdent.ValueIdPart_PhysicalQuantity> GetPhysicalQuantitiesEnumerableAsValueIdPart()
    {
      return (IEnumerable<ValueIdent.ValueIdPart_PhysicalQuantity>) Enum.GetValues(typeof (ValueIdent.ValueIdPart_PhysicalQuantity)).Cast<ValueIdent.ValueIdPart_PhysicalQuantity>().OrderBy<ValueIdent.ValueIdPart_PhysicalQuantity, ValueIdent.ValueIdPart_PhysicalQuantity>((Func<ValueIdent.ValueIdPart_PhysicalQuantity, ValueIdent.ValueIdPart_PhysicalQuantity>) (s => s));
    }

    public static IEnumerable<ValueIdent.ValueIdPart_MeterType> GetMeterTypeEnumerableAsValueIdPart()
    {
      return (IEnumerable<ValueIdent.ValueIdPart_MeterType>) Enum.GetValues(typeof (ValueIdent.ValueIdPart_MeterType)).Cast<ValueIdent.ValueIdPart_MeterType>().OrderBy<ValueIdent.ValueIdPart_MeterType, ValueIdent.ValueIdPart_MeterType>((Func<ValueIdent.ValueIdPart_MeterType, ValueIdent.ValueIdPart_MeterType>) (s => s));
    }

    public static IEnumerable<ValueIdent.ValueIdPart_Calculation> GetCalculationEnumerableAsValueIdPart()
    {
      return (IEnumerable<ValueIdent.ValueIdPart_Calculation>) Enum.GetValues(typeof (ValueIdent.ValueIdPart_Calculation)).Cast<ValueIdent.ValueIdPart_Calculation>().OrderBy<ValueIdent.ValueIdPart_Calculation, ValueIdent.ValueIdPart_Calculation>((Func<ValueIdent.ValueIdPart_Calculation, ValueIdent.ValueIdPart_Calculation>) (s => s));
    }

    public static IEnumerable<ValueIdent.ValueIdPart_CalculationStart> GetCalculationStartEnumerableAsValueIdPart()
    {
      return (IEnumerable<ValueIdent.ValueIdPart_CalculationStart>) Enum.GetValues(typeof (ValueIdent.ValueIdPart_CalculationStart)).Cast<ValueIdent.ValueIdPart_CalculationStart>().OrderBy<ValueIdent.ValueIdPart_CalculationStart, ValueIdent.ValueIdPart_CalculationStart>((Func<ValueIdent.ValueIdPart_CalculationStart, ValueIdent.ValueIdPart_CalculationStart>) (s => s));
    }

    public static IEnumerable<ValueIdent.ValueIdPart_StorageInterval> GetStorageIntervalEnumerableAsValueIdPart()
    {
      return (IEnumerable<ValueIdent.ValueIdPart_StorageInterval>) Enum.GetValues(typeof (ValueIdent.ValueIdPart_StorageInterval)).Cast<ValueIdent.ValueIdPart_StorageInterval>().OrderBy<ValueIdent.ValueIdPart_StorageInterval, ValueIdent.ValueIdPart_StorageInterval>((Func<ValueIdent.ValueIdPart_StorageInterval, ValueIdent.ValueIdPart_StorageInterval>) (s => s));
    }

    public static IEnumerable<ValueIdent.ValueIdPart_Creation> GetCreationEnumerableAsValueIdPart()
    {
      return (IEnumerable<ValueIdent.ValueIdPart_Creation>) Enum.GetValues(typeof (ValueIdent.ValueIdPart_Creation)).Cast<ValueIdent.ValueIdPart_Creation>().OrderBy<ValueIdent.ValueIdPart_Creation, ValueIdent.ValueIdPart_Creation>((Func<ValueIdent.ValueIdPart_Creation, ValueIdent.ValueIdPart_Creation>) (s => s));
    }

    public static string GetValueId(
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity,
      ValueIdent.ValueIdPart_MeterType meterType,
      ValueIdent.ValueIdPart_Calculation calculation,
      ValueIdent.ValueIdPart_CalculationStart calculationStart,
      ValueIdent.ValueIdPart_StorageInterval storageInterval,
      ValueIdent.ValueIdPart_Creation creation,
      int ruleIndex)
    {
      return ((long) (physicalQuantity + (long) meterType + (long) calculation + (long) calculationStart + (long) storageInterval + (long) creation + (long) ruleIndex * 2147483648L)).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string GetValueId(
      string physicalQuantity,
      string meterType,
      string calculation,
      string calculationStart,
      string storageInterval,
      string creation,
      int ruleIndex)
    {
      return ((physicalQuantity != null ? (long) Enum.Parse(typeof (ValueIdent.ValueIdPart_PhysicalQuantity), physicalQuantity) : 0L) + (meterType != null ? (long) Enum.Parse(typeof (ValueIdent.ValueIdPart_MeterType), meterType) : 0L) + (calculation != null ? (long) Enum.Parse(typeof (ValueIdent.ValueIdPart_Calculation), calculation) : 0L) + (calculationStart != null ? (long) Enum.Parse(typeof (ValueIdent.ValueIdPart_CalculationStart), calculationStart) : 0L) + (storageInterval != null ? (long) Enum.Parse(typeof (ValueIdent.ValueIdPart_StorageInterval), storageInterval) : 0L) + (creation != null ? (long) Enum.Parse(typeof (ValueIdent.ValueIdPart_Creation), creation) : 0L) + (long) ruleIndex * 2147483648L).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
