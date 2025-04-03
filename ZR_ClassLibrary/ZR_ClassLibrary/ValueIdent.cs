// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ValueIdent
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Resources;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public static class ValueIdent
  {
    private static ResourceManager valueIdentResource;
    private static SortedList<long, string> valueIdPart_PhysicalQuantityEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> valueIdPart_MeterTypeEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> valueIdPart_CalculationEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> valueIdPart_CalculationStartEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> valueIdPart_StorageIntervalEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> valueIdPart_CreationEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> valueIdPart_IndexEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> valueIdPart_IndexErrorEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> valueIdPart_IndexWarningEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> valueIdPart_IndexInfoEnumToTranslation = new SortedList<long, string>();
    private static SortedList<long, string> generatedValueIdTranslations = new SortedList<long, string>();
    private static SortedList<long, string> generatedHumanReadableValueIdTranslations = new SortedList<long, string>();
    private static SortedList<long, string> generatedValueId = new SortedList<long, string>();
    private static SortedList<long, string> generatedHumanReadableValueId = new SortedList<long, string>();

    static ValueIdent()
    {
      ValueIdent.valueIdentResource = new ResourceManager("ZR_ClassLibrary.ValueIdentTranslation", typeof (ValueIdent).Assembly);
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_PhysicalQuantityEnumToTranslation, typeof (ValueIdent.ValueIdPart_PhysicalQuantity));
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_MeterTypeEnumToTranslation, typeof (ValueIdent.ValueIdPart_MeterType));
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_CalculationEnumToTranslation, typeof (ValueIdent.ValueIdPart_Calculation));
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_CalculationStartEnumToTranslation, typeof (ValueIdent.ValueIdPart_CalculationStart));
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_StorageIntervalEnumToTranslation, typeof (ValueIdent.ValueIdPart_StorageInterval));
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_CreationEnumToTranslation, typeof (ValueIdent.ValueIdPart_Creation));
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_IndexEnumToTranslation, typeof (ValueIdent.ValueIdPart_Index));
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_IndexErrorEnumToTranslation, typeof (ValueIdent.ValueIdentError));
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_IndexWarningEnumToTranslation, typeof (ValueIdent.ValueIdentWarning));
      ValueIdent.FillSortedList(ValueIdent.valueIdPart_IndexInfoEnumToTranslation, typeof (ValueIdent.ValueIdentInfo));
    }

    public static string GetUnit(long valueIdent)
    {
      return ValueIdent.GetUnit((ValueIdent.ValueIdPart_PhysicalQuantity) (valueIdent & 63L));
    }

    public static string GetUnit(
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity)
    {
      long num = (long) physicalQuantity;
      if ((ulong) num <= 26UL)
      {
        switch ((uint) num)
        {
          case 0:
            return "";
          case 1:
            return "m\u00B3";
          case 2:
            return "MWh";
          case 3:
            return "m\u00B3/h";
          case 4:
            return "MW";
          case 5:
            return "kg";
          case 6:
            return "°C";
          case 7:
            return "°K";
          case 8:
            return "HCA";
          case 9:
            return "HCAW";
          case 10:
            return "1/Imp";
          case 11:
            return "MWh";
          case 12:
            return "°C";
          case 13:
            return "°C";
          case 14:
            return "h";
          case 15:
            return "";
          case 16:
            return "";
          case 17:
            return "";
          case 18:
            return "dBm";
          case 19:
            return "m\u00B3";
          case 20:
            return "%";
          case 21:
            return "";
          case 22:
            return "";
          case 23:
            return "m\u00B3";
          case 24:
            return "L/Imp";
          case 25:
            return "m\u00B3";
          case 26:
            return "";
        }
      }
      throw new ArgumentException("Illegal unit part on ValueID");
    }

    public static string GetUnit(byte physicalQuantityEnumIndex)
    {
      return ValueIdent.GetUnit((long) physicalQuantityEnumIndex);
    }

    public static long GetValueIdent(
      byte valueIdentIndex,
      byte physicalQuantity,
      byte meterType,
      byte calculation,
      byte calculationStart,
      byte storageInterval,
      byte creation)
    {
      return (long) calculation * 4096L + (long) calculationStart * 65536L + (long) creation * 268435456L + (long) meterType * 64L + (long) physicalQuantity + (long) storageInterval * 4194304L + (long) valueIdentIndex * 2147483648L;
    }

    public static List<byte> ValueIdPart_Get<T>(List<long> valueIdents)
    {
      if (valueIdents == null)
        return (List<byte>) null;
      SortedList<byte, T> sortedList = new SortedList<byte, T>();
      foreach (long valueIdent in valueIdents)
      {
        T obj = ValueIdent.ValueIdPart_Get<T>(valueIdent);
        long int64 = Convert.ToInt64((object) obj);
        if (typeof (T) == typeof (ValueIdent.ValueIdPart_Calculation))
          int64 /= 4096L;
        else if (typeof (T) == typeof (ValueIdent.ValueIdPart_CalculationStart))
          int64 /= 65536L;
        else if (typeof (T) == typeof (ValueIdent.ValueIdPart_Creation))
          int64 /= 268435456L;
        else if (typeof (T) == typeof (ValueIdent.ValueIdPart_MeterType))
          int64 /= 64L;
        else if (typeof (T) == typeof (ValueIdent.ValueIdPart_PhysicalQuantity))
          int64 /= 1L;
        else if (typeof (T) == typeof (ValueIdent.ValueIdPart_StorageInterval))
          int64 /= 4194304L;
        else if (typeof (T) == typeof (ValueIdent.ValueIdPart_Index))
          int64 /= 2147483648L;
        byte key = (byte) int64;
        if (!sortedList.ContainsKey(key))
          sortedList.Add(key, obj);
      }
      return new List<byte>((IEnumerable<byte>) sortedList.Keys);
    }

    public static long GetValueIdForValueEnum(
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity,
      ValueIdent.ValueIdPart_MeterType meterType,
      ValueIdent.ValueIdPart_Calculation calculation,
      ValueIdent.ValueIdPart_CalculationStart calculationStart,
      ValueIdent.ValueIdPart_StorageInterval storageInterval,
      ValueIdent.ValueIdPart_Creation creation,
      object index)
    {
      try
      {
        long valueId = (long) (calculation + (long) calculationStart + (long) physicalQuantity + (long) meterType + (long) storageInterval + (long) creation + (long) index);
        ValueIdent.IsValid(valueId, true);
        return valueId;
      }
      catch (Exception ex)
      {
        throw new Exception("Try to create illegal ValueID", ex);
      }
    }

    public static T ValueIdPart_Get<T>(long valueId)
    {
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = (ValueIdent.ValueIdPart_PhysicalQuantity) (valueId & 63L);
      if (typeof (T) == typeof (ValueIdent.ValueIdPart_PhysicalQuantity))
        return Enum.IsDefined(typeof (ValueIdent.ValueIdPart_PhysicalQuantity), (object) physicalQuantity) ? (T) (Enum) physicalQuantity : throw new OverflowException("ValueIdPart_PhysicalQuantity");
      if (typeof (T) == typeof (ValueIdent.ValueIdPart_MeterType))
      {
        ValueIdent.ValueIdPart_MeterType valueIdPartMeterType = (ValueIdent.ValueIdPart_MeterType) (valueId & 4032L);
        return Enum.IsDefined(typeof (ValueIdent.ValueIdPart_MeterType), (object) valueIdPartMeterType) ? (T) (Enum) valueIdPartMeterType : throw new OverflowException("ValueIdPart_PhysicalQuantity");
      }
      if (typeof (T) == typeof (ValueIdent.ValueIdPart_Calculation))
      {
        ValueIdent.ValueIdPart_Calculation idPartCalculation = (ValueIdent.ValueIdPart_Calculation) (valueId & 61440L);
        return Enum.IsDefined(typeof (ValueIdent.ValueIdPart_Calculation), (object) idPartCalculation) ? (T) (Enum) idPartCalculation : throw new OverflowException("ValueIdPart_Calculation");
      }
      if (typeof (T) == typeof (ValueIdent.ValueIdPart_CalculationStart))
      {
        ValueIdent.ValueIdPart_CalculationStart calculationStart = (ValueIdent.ValueIdPart_CalculationStart) (valueId & 4128768L);
        return Enum.IsDefined(typeof (ValueIdent.ValueIdPart_CalculationStart), (object) calculationStart) ? (T) (Enum) calculationStart : throw new OverflowException("ValueIdPart_CalculationStart");
      }
      if (typeof (T) == typeof (ValueIdent.ValueIdPart_StorageInterval))
      {
        ValueIdent.ValueIdPart_StorageInterval partStorageInterval = (ValueIdent.ValueIdPart_StorageInterval) (valueId & 264241152L);
        return Enum.IsDefined(typeof (ValueIdent.ValueIdPart_StorageInterval), (object) partStorageInterval) ? (T) (Enum) partStorageInterval : throw new OverflowException("ValueIdPart_StorageInterval");
      }
      if (typeof (T) == typeof (ValueIdent.ValueIdPart_Creation))
      {
        ValueIdent.ValueIdPart_Creation valueIdPartCreation = (ValueIdent.ValueIdPart_Creation) (valueId & 1879048192L);
        return Enum.IsDefined(typeof (ValueIdent.ValueIdPart_Creation), (object) valueIdPartCreation) ? (T) (Enum) valueIdPartCreation : throw new OverflowException("ValueIdPart_Creation");
      }
      switch (physicalQuantity)
      {
        case ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber:
          if (!(typeof (T) == typeof (ValueIdent.ValueIdentError)))
            throw new Exception("Illegal ValueIdPart_PhysicalQuantity for ValueIdentError");
          ValueIdent.ValueIdentError valueIdentError = (ValueIdent.ValueIdentError) (valueId & 8793945538560L);
          return Enum.IsDefined(typeof (ValueIdent.ValueIdentError), (object) valueIdentError) ? (T) (Enum) valueIdentError : throw new OverflowException("ValueIdentError");
        case ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber:
          if (!(typeof (T) == typeof (ValueIdent.ValueIdentWarning)))
            throw new Exception("Illegal ValueIdPart_PhysicalQuantity for ValueIdentWarning");
          ValueIdent.ValueIdentWarning valueIdentWarning = (ValueIdent.ValueIdentWarning) (valueId & 8793945538560L);
          return Enum.IsDefined(typeof (ValueIdent.ValueIdentWarning), (object) valueIdentWarning) ? (T) (Enum) valueIdentWarning : throw new OverflowException("ValueIdentWarning");
        case ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber:
          if (!(typeof (T) == typeof (ValueIdent.ValueIdentInfo)))
            throw new Exception("Illegal ValueIdPart_PhysicalQuantity for ValueIdPart_InfoIndex");
          ValueIdent.ValueIdentInfo valueIdentInfo = (ValueIdent.ValueIdentInfo) (valueId & 8793945538560L);
          return Enum.IsDefined(typeof (ValueIdent.ValueIdentInfo), (object) valueIdentInfo) ? (T) (Enum) valueIdentInfo : throw new OverflowException("ValueIdPart_InfoIndex");
        default:
          if (!(typeof (T) == typeof (ValueIdent.ValueIdPart_Index)))
            throw new Exception("Illegal ValueIdPart_PhysicalQuantity for ValueIdPart_Index");
          ValueIdent.ValueIdPart_Index valueIdPartIndex = (ValueIdent.ValueIdPart_Index) (valueId & 8793945538560L);
          return Enum.IsDefined(typeof (ValueIdent.ValueIdPart_Index), (object) valueIdPartIndex) ? (T) (Enum) valueIdPartIndex : throw new OverflowException("ValueIdPart_Index");
      }
    }

    public static ValueIdent.ValueIdPart_PhysicalQuantity Get_ValueIdPart_PhysicalQuantity(
      long valueId)
    {
      return (ValueIdent.ValueIdPart_PhysicalQuantity) (valueId & 63L);
    }

    public static ValueIdent.ValueIdentInfo Get_ValueIdPart_ValueIdentInfo(long valueId)
    {
      return (ValueIdent.ValueIdentInfo) (valueId & 8793945538560L);
    }

    public static ValueIdent.ValueIdentWarning Get_ValueIdPart_ValueIdentWarning(long valueId)
    {
      return (ValueIdent.ValueIdentWarning) (valueId & 8793945538560L);
    }

    public static ValueIdent.ValueIdentError Get_ValueIdPart_ValueIdentError(long valueId)
    {
      return (ValueIdent.ValueIdentError) (valueId & 8793945538560L);
    }

    public static void ChangePhysicalQuantity(
      ref long valueID,
      ValueIdent.ValueIdPart_PhysicalQuantity newPhysicalQuantity)
    {
      valueID = (long) ((ValueIdent.ValueIdPart_PhysicalQuantity) (valueID & -64L) | newPhysicalQuantity);
    }

    public static void ChangeToInfo(ref long valueID, ValueIdent.ValueIdentInfo newInfo)
    {
      valueID = (long) ((ValueIdent.ValueIdentInfo) (valueID & -8793945538624L | 26L) | newInfo);
    }

    public static void ChangeToWarning(ref long valueID, ValueIdent.ValueIdentWarning newWarning)
    {
      valueID = (long) ((ValueIdent.ValueIdentWarning) (valueID & -8793945538624L | 22L) | newWarning);
    }

    public static void ChangeToError(ref long valueID, ValueIdent.ValueIdentError newError)
    {
      valueID = (long) ((ValueIdent.ValueIdentError) (valueID & -8793945538624L | 15L) | newError);
    }

    public static bool SetValueIdentPart(
      long PartMask,
      long Part,
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (ValueList == null)
        return false;
      bool flag1 = false;
      bool flag2;
      do
      {
        flag2 = false;
        for (int index = 0; index < ValueList.Keys.Count; ++index)
        {
          if ((ValueList.Keys[index] & PartMask) == 0L)
          {
            long key = ValueList.Keys[index] | Part;
            SortedList<DateTime, ReadingValue> sortedList = ValueList.Values[index];
            ValueList.RemoveAt(index);
            ValueList.Add(key, sortedList);
            flag1 = true;
            flag2 = true;
            break;
          }
        }
      }
      while (flag2);
      return flag1;
    }

    public static object ValueIdPart_GetIndex(long valueIdent)
    {
      switch (ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueIdent))
      {
        case ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber:
          return (object) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentError>(valueIdent);
        case ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber:
          return (object) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentWarning>(valueIdent);
        case ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber:
          return (object) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentInfo>(valueIdent);
        default:
          return (object) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Index>(valueIdent);
      }
    }

    public static string GetPredefinedValueID_EnumName(long ValueId)
    {
      string empty = string.Empty;
      Array values = Enum.GetValues(typeof (ValueIdent.ValueId_Predefined));
      for (int index = 0; index < values.Length; ++index)
      {
        long num = (long) values.GetValue(index);
        if (num == ValueId)
        {
          empty = ((ValueIdent.ValueId_Predefined) num).ToString();
          break;
        }
      }
      return empty;
    }

    public static string GetPredefinedValueList()
    {
      StringBuilder stringBuilder = new StringBuilder(2000);
      string[] names = Enum.GetNames(typeof (ValueIdent.ValueId_Predefined));
      SortedList<string, long> sortedList = new SortedList<string, long>();
      for (int index = 0; index < names.Length; ++index)
        sortedList.Add(names[index], 0L);
      for (int index = 0; index < sortedList.Count; ++index)
      {
        string key = sortedList.Keys[index];
        long valueId = (long) Enum.Parse(typeof (ValueIdent.ValueId_Predefined), key);
        string str;
        try
        {
          str = ValueIdent.GetTranslatedValueNameForValueId(valueId, true);
        }
        catch
        {
          str = "translation error";
        }
        stringBuilder.Append(key);
        stringBuilder.Append("; ");
        stringBuilder.Append(valueId.ToString());
        stringBuilder.Append("; ");
        stringBuilder.Append(str);
        stringBuilder.AppendLine();
      }
      return stringBuilder.ToString();
    }

    public static string GetTranslatedEnumsAsHex()
    {
      StringBuilder stringBuilder = new StringBuilder(5000);
      stringBuilder.AppendLine(ValueIdent.GetTranslatedEnumAsHex(typeof (ValueIdent.ValueId_Mask), "Mask:"));
      stringBuilder.AppendLine(ValueIdent.GetTranslatedEnumAsHex(typeof (ValueIdent.ValueId_MinValue), "MinValues:"));
      stringBuilder.AppendLine(ValueIdent.GetTranslatedEnumAsHex(typeof (ValueIdent.ValueIdPart_PhysicalQuantity), "PhysicalQuantity:"));
      stringBuilder.AppendLine(ValueIdent.GetTranslatedEnumAsHex(typeof (ValueIdent.ValueIdPart_MeterType), "MeterType:"));
      stringBuilder.AppendLine(ValueIdent.GetTranslatedEnumAsHex(typeof (ValueIdent.ValueIdPart_StorageInterval), "StorageInterval:"));
      stringBuilder.AppendLine(ValueIdent.GetTranslatedEnumAsHex(typeof (ValueIdent.ValueIdPart_Calculation), "Calculation:"));
      stringBuilder.AppendLine(ValueIdent.GetTranslatedEnumAsHex(typeof (ValueIdent.ValueIdPart_CalculationStart), "CalculationStart:"));
      stringBuilder.AppendLine(ValueIdent.GetTranslatedEnumAsHex(typeof (ValueIdent.ValueIdPart_Creation), "Creation:"));
      return stringBuilder.ToString();
    }

    private static string GetTranslatedEnumAsHex(Type t, string caption)
    {
      StringBuilder stringBuilder = new StringBuilder(2000);
      string[] names = Enum.GetNames(t);
      SortedList<string, long> sortedList1 = new SortedList<string, long>();
      for (int index = 0; index < names.Length; ++index)
        sortedList1.Add(names[index], 0L);
      SortedList<long, string> sortedList2 = new SortedList<long, string>();
      for (int index = 0; index < sortedList1.Count; ++index)
        sortedList2.Add((long) Enum.Parse(t, sortedList1.Keys[index]), sortedList1.Keys[index]);
      stringBuilder.AppendLine(caption);
      for (int index = 0; index < sortedList2.Count; ++index)
      {
        stringBuilder.Append(sortedList2.Keys[index].ToString("x16").ToUpper());
        stringBuilder.Append("->");
        stringBuilder.Append(sortedList2.Values[index]);
        stringBuilder.AppendLine();
      }
      return stringBuilder.ToString();
    }

    public static string GetTranslatedValueNameForPartOfValueId(Enum valueIdPart)
    {
      if (valueIdPart == null)
        return string.Empty;
      long int64 = Convert.ToInt64((object) valueIdPart);
      if (valueIdPart is ValueIdent.ValueIdPart_Calculation)
      {
        if (ValueIdent.valueIdPart_CalculationEnumToTranslation.ContainsKey(int64))
          return ValueIdent.valueIdPart_CalculationEnumToTranslation[int64];
      }
      else if (valueIdPart is ValueIdent.ValueIdPart_CalculationStart)
      {
        if (ValueIdent.valueIdPart_CalculationStartEnumToTranslation.ContainsKey(int64))
          return ValueIdent.valueIdPart_CalculationStartEnumToTranslation[int64];
      }
      else if (valueIdPart is ValueIdent.ValueIdPart_Creation)
      {
        if (ValueIdent.valueIdPart_CreationEnumToTranslation.ContainsKey(int64))
          return ValueIdent.valueIdPart_CreationEnumToTranslation[int64];
      }
      else if (valueIdPart is ValueIdent.ValueIdPart_MeterType)
      {
        if (ValueIdent.valueIdPart_MeterTypeEnumToTranslation.ContainsKey(int64))
          return ValueIdent.valueIdPart_MeterTypeEnumToTranslation[int64];
      }
      else if (valueIdPart is ValueIdent.ValueIdPart_PhysicalQuantity)
      {
        if (ValueIdent.valueIdPart_PhysicalQuantityEnumToTranslation.ContainsKey(int64))
          return ValueIdent.valueIdPart_PhysicalQuantityEnumToTranslation[int64];
      }
      else if (valueIdPart is ValueIdent.ValueIdPart_StorageInterval && ValueIdent.valueIdPart_StorageIntervalEnumToTranslation.ContainsKey(int64))
        return ValueIdent.valueIdPart_StorageIntervalEnumToTranslation[int64];
      return string.Empty;
    }

    public static string GetTranslatedValueNameForValueId(long valueId, bool humanReadable)
    {
      if (!ValueIdent.IsValid(valueId))
        return "Not defined";
      if (humanReadable && ValueIdent.generatedHumanReadableValueIdTranslations.ContainsKey(valueId))
        return ValueIdent.generatedHumanReadableValueIdTranslations[valueId];
      if (!humanReadable && ValueIdent.generatedValueIdTranslations.ContainsKey(valueId))
        return ValueIdent.generatedValueIdTranslations[valueId];
      ValueIdent.ValueIdPart_Calculation key1 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueId);
      ValueIdent.ValueIdPart_CalculationStart key2 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueId);
      ValueIdent.ValueIdPart_Creation key3 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueId);
      ValueIdent.ValueIdPart_MeterType key4 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueId);
      ValueIdent.ValueIdPart_PhysicalQuantity key5 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueId);
      ValueIdent.ValueIdPart_StorageInterval key6 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueId);
      if (humanReadable)
      {
        string valueNameForValueId = ValueIdent.valueIdPart_PhysicalQuantityEnumToTranslation[(long) key5] + ", " + ValueIdent.valueIdPart_MeterTypeEnumToTranslation[(long) key4] + ", " + ValueIdent.valueIdPart_CalculationEnumToTranslation[(long) key1];
        if (key6 != ValueIdent.ValueIdPart_StorageInterval.None)
          valueNameForValueId = valueNameForValueId + ", " + ValueIdent.valueIdPart_StorageIntervalEnumToTranslation[(long) key6];
        if (key2 != ValueIdent.ValueIdPart_CalculationStart.MeterProduction)
          valueNameForValueId = valueNameForValueId + ", " + ValueIdent.valueIdPart_CalculationStartEnumToTranslation[(long) key2];
        ValueIdent.generatedHumanReadableValueIdTranslations.Add(valueId, valueNameForValueId);
        return valueNameForValueId;
      }
      StringBuilder stringBuilder = new StringBuilder();
      if (ValueIdent.valueIdPart_PhysicalQuantityEnumToTranslation.ContainsKey((long) key5))
      {
        stringBuilder.Append(ValueIdent.valueIdPart_PhysicalQuantityEnumToTranslation[(long) key5]);
        stringBuilder.Append(", ");
      }
      if (ValueIdent.valueIdPart_MeterTypeEnumToTranslation.ContainsKey((long) key4))
      {
        stringBuilder.Append(ValueIdent.valueIdPart_MeterTypeEnumToTranslation[(long) key4]);
        stringBuilder.Append(", ");
      }
      if (ValueIdent.valueIdPart_CalculationEnumToTranslation.ContainsKey((long) key1))
      {
        stringBuilder.Append(ValueIdent.valueIdPart_CalculationEnumToTranslation[(long) key1]);
        stringBuilder.Append(", ");
      }
      if (ValueIdent.valueIdPart_CalculationStartEnumToTranslation.ContainsKey((long) key2))
      {
        stringBuilder.Append(ValueIdent.valueIdPart_CalculationStartEnumToTranslation[(long) key2]);
        stringBuilder.Append(", ");
      }
      if (ValueIdent.valueIdPart_CreationEnumToTranslation.ContainsKey((long) key3))
      {
        stringBuilder.Append(ValueIdent.valueIdPart_CreationEnumToTranslation[(long) key3]);
        stringBuilder.Append(", ");
      }
      if (ValueIdent.valueIdPart_StorageIntervalEnumToTranslation.ContainsKey((long) key6))
      {
        stringBuilder.Append(ValueIdent.valueIdPart_StorageIntervalEnumToTranslation[(long) key6]);
        stringBuilder.Append(", ");
      }
      if (key5 == ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber)
      {
        ValueIdent.ValueIdentError key7 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentError>(valueId);
        stringBuilder.Append(ValueIdent.valueIdPart_IndexErrorEnumToTranslation[(long) key7]);
      }
      if (key5 == ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber)
      {
        ValueIdent.ValueIdentWarning key8 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentWarning>(valueId);
        stringBuilder.Append(ValueIdent.valueIdPart_IndexWarningEnumToTranslation[(long) key8]);
      }
      if (key5 == ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber)
      {
        ValueIdent.ValueIdentInfo key9 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentInfo>(valueId);
        stringBuilder.Append(ValueIdent.valueIdPart_IndexInfoEnumToTranslation[(long) key9]);
      }
      ValueIdent.generatedValueIdTranslations.Add(valueId, stringBuilder.ToString());
      return stringBuilder.ToString();
    }

    public static SortedList<long, string> GetTranslatedStringListForValueIdPart(Type valueIdType)
    {
      SortedList<long, string> listForValueIdPart = (SortedList<long, string>) null;
      if (valueIdType == typeof (ValueIdent.ValueIdPart_Calculation))
        listForValueIdPart = ValueIdent.valueIdPart_CalculationEnumToTranslation;
      else if (valueIdType == typeof (ValueIdent.ValueIdPart_CalculationStart))
        listForValueIdPart = ValueIdent.valueIdPart_CalculationStartEnumToTranslation;
      else if (valueIdType == typeof (ValueIdent.ValueIdPart_Creation))
        listForValueIdPart = ValueIdent.valueIdPart_CreationEnumToTranslation;
      else if (valueIdType == typeof (ValueIdent.ValueIdPart_MeterType))
        listForValueIdPart = ValueIdent.valueIdPart_MeterTypeEnumToTranslation;
      else if (valueIdType == typeof (ValueIdent.ValueIdPart_PhysicalQuantity))
        listForValueIdPart = ValueIdent.valueIdPart_PhysicalQuantityEnumToTranslation;
      else if (valueIdType == typeof (ValueIdent.ValueIdPart_StorageInterval))
        listForValueIdPart = ValueIdent.valueIdPart_StorageIntervalEnumToTranslation;
      else if (valueIdType == typeof (ValueIdent.ValueIdPart_Index))
        listForValueIdPart = ValueIdent.valueIdPart_IndexEnumToTranslation;
      else if (valueIdType == typeof (ValueIdent.ValueIdentError))
        listForValueIdPart = ValueIdent.valueIdPart_IndexErrorEnumToTranslation;
      else if (valueIdType == typeof (ValueIdent.ValueIdentWarning))
        listForValueIdPart = ValueIdent.valueIdPart_IndexWarningEnumToTranslation;
      return listForValueIdPart;
    }

    public static string GetShortNameForValueId(long valueId)
    {
      if (!ValueIdent.IsValid(valueId))
        return "Not defined";
      ValueIdent.ValueIdPart_Calculation idPartCalculation = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueId);
      ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueId);
      ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueId);
      ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueId);
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueId);
      ValueIdent.ValueIdPart_StorageInterval partStorageInterval = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueId);
      ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Index>(valueId);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(physicalQuantity.ToString()).Append(", ");
      if (partStorageInterval != ValueIdent.ValueIdPart_StorageInterval.None && partStorageInterval != 0)
        stringBuilder.Append(partStorageInterval.ToString()).Append(", ");
      if (idPartCalculation != 0)
        stringBuilder.Append(idPartCalculation.ToString());
      return stringBuilder.ToString();
    }

    public static string GetValueNameForValueId(long valueId, bool humanReadable)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (ValueIdent.generatedHumanReadableValueId.ContainsKey(valueId))
        return humanReadable ? ValueIdent.generatedHumanReadableValueId[valueId] : ValueIdent.generatedValueId[valueId];
      ValueIdent.ValueIdPart_Calculation idPartCalculation = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueId);
      ValueIdent.ValueIdPart_CalculationStart calculationStart = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueId);
      ValueIdent.ValueIdPart_Creation valueIdPartCreation = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueId);
      ValueIdent.ValueIdPart_MeterType valueIdPartMeterType = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueId);
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueId);
      ValueIdent.ValueIdPart_StorageInterval partStorageInterval = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueId);
      ValueIdent.ValueIdPart_Index valueIdPartIndex = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Index>(valueId);
      string str1 = physicalQuantity.ToString() + ", " + valueIdPartMeterType.ToString() + ", " + idPartCalculation.ToString();
      ValueIdent.generatedHumanReadableValueId.Add(valueId, str1);
      string str2 = physicalQuantity.ToString() + ", " + valueIdPartMeterType.ToString() + ", " + idPartCalculation.ToString() + ", " + calculationStart.ToString() + ", " + valueIdPartCreation.ToString() + ", " + partStorageInterval.ToString() + ", " + valueIdPartIndex.ToString();
      ValueIdent.generatedValueId.Add(valueId, str2);
      return humanReadable ? str1 : str2;
    }

    public static string GetValueIdNameAsPath(long valueId)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = ((ValueIdent.ValueIdPart_MeterType) (valueId & 4032L)).ToString();
      if (str1 != "Any")
        stringBuilder.Append("/type:" + str1);
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = (ValueIdent.ValueIdPart_PhysicalQuantity) (valueId & 63L);
      string str2 = physicalQuantity.ToString();
      if (str2 != "Any")
        stringBuilder.Append("/physic:" + str2);
      string str3 = ((ValueIdent.ValueIdPart_Calculation) (valueId & 61440L)).ToString();
      if (str3 != "Any")
        stringBuilder.Append("/calc:" + str3);
      string str4 = ((ValueIdent.ValueIdPart_CalculationStart) (valueId & 4128768L)).ToString();
      if (str4 != "Any")
        stringBuilder.Append("/calc_start:" + str4);
      string str5 = ((ValueIdent.ValueIdPart_StorageInterval) (valueId & 264241152L)).ToString();
      if (str5 != "Any")
        stringBuilder.Append("/interval:" + str5);
      string str6 = ((ValueIdent.ValueIdPart_Creation) (valueId & 1879048192L)).ToString();
      if (str6 != "Any")
        stringBuilder.Append("/creation:" + str6);
      string str7 = (string) null;
      switch (physicalQuantity)
      {
        case ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber:
          str7 = ((ValueIdent.ValueIdentError) (valueId & 8793945538560L)).ToString();
          break;
        case ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber:
          str7 = ((ValueIdent.ValueIdentWarning) (valueId & 8793945538560L)).ToString();
          break;
        case ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber:
          str7 = ((ValueIdent.ValueIdentInfo) (valueId & 8793945538560L)).ToString();
          break;
      }
      if (str7 != null)
        stringBuilder.Append("/index:" + str7);
      return stringBuilder.ToString();
    }

    public static string GetPhysicalName(long valueId)
    {
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = (ValueIdent.ValueIdPart_PhysicalQuantity) (valueId & 63L);
      string physicalName = physicalQuantity.ToString();
      switch (physicalQuantity)
      {
        case ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber:
          string str1 = ((ValueIdent.ValueIdentError) (valueId & 8793945538560L)).ToString();
          return physicalName + "." + str1;
        case ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber:
          string str2 = ((ValueIdent.ValueIdentWarning) (valueId & 8793945538560L)).ToString();
          return physicalName + "." + str2;
        case ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber:
          string str3 = ((ValueIdent.ValueIdentInfo) (valueId & 8793945538560L)).ToString();
          return physicalName + "." + str3;
        default:
          return physicalName;
      }
    }

    public static bool IsValid(long valueId, bool exceptionOnError = false)
    {
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = (ValueIdent.ValueIdPart_PhysicalQuantity) (valueId & 63L);
      if (!Enum.IsDefined(typeof (ValueIdent.ValueIdPart_PhysicalQuantity), (object) physicalQuantity))
      {
        if (exceptionOnError)
          throw new Exception("ValueIdPart_PhysicalQuantity not defined.");
        return false;
      }
      if (!Enum.IsDefined(typeof (ValueIdent.ValueIdPart_MeterType), (object) (ValueIdent.ValueIdPart_MeterType) (valueId & 4032L)))
      {
        if (exceptionOnError)
          throw new Exception("ValueIdPart_MeterType not defined.");
        return false;
      }
      if (!Enum.IsDefined(typeof (ValueIdent.ValueIdPart_Calculation), (object) (ValueIdent.ValueIdPart_Calculation) (valueId & 61440L)))
      {
        if (exceptionOnError)
          throw new Exception("ValueIdPart_Calculation not defined.");
        return false;
      }
      if (!Enum.IsDefined(typeof (ValueIdent.ValueIdPart_CalculationStart), (object) (ValueIdent.ValueIdPart_CalculationStart) (valueId & 4128768L)))
      {
        if (exceptionOnError)
          throw new Exception("ValueIdPart_CalculationStart not defined.");
        return false;
      }
      if (!Enum.IsDefined(typeof (ValueIdent.ValueIdPart_StorageInterval), (object) (ValueIdent.ValueIdPart_StorageInterval) (valueId & 264241152L)))
      {
        if (exceptionOnError)
          throw new Exception("ValueIdPart_StorageInterval not defined.");
        return false;
      }
      if (!Enum.IsDefined(typeof (ValueIdent.ValueIdPart_Creation), (object) (ValueIdent.ValueIdPart_Creation) (valueId & 1879048192L)))
      {
        if (exceptionOnError)
          throw new Exception("ValueIdPart_Creation not defined.");
        return false;
      }
      switch (physicalQuantity)
      {
        case ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber:
          if (!Enum.IsDefined(typeof (ValueIdent.ValueIdentError), (object) (ValueIdent.ValueIdentError) (valueId & 8793945538560L)))
          {
            if (exceptionOnError)
              throw new Exception("ValueIdentError not defined.");
            return false;
          }
          break;
        case ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber:
          if (!Enum.IsDefined(typeof (ValueIdent.ValueIdentWarning), (object) (ValueIdent.ValueIdentWarning) (valueId & 8793945538560L)))
          {
            if (exceptionOnError)
              throw new Exception("ValueIdentWarning not defined.");
            return false;
          }
          break;
        case ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber:
          if (!Enum.IsDefined(typeof (ValueIdent.ValueIdentInfo), (object) (ValueIdent.ValueIdentInfo) (valueId & 8793945538560L)))
          {
            if (exceptionOnError)
              throw new Exception("ValueIdPart_InfoIndex not defined.");
            return false;
          }
          break;
        default:
          if (!Enum.IsDefined(typeof (ValueIdent.ValueIdPart_Index), (object) (ValueIdent.ValueIdPart_Index) (valueId & 8793945538560L)))
          {
            if (exceptionOnError)
              throw new Exception("ValueIdPart_Index not defined.");
            return false;
          }
          break;
      }
      return true;
    }

    public static bool IsEvent(long valueId)
    {
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = ValueIdent.Get_ValueIdPart_PhysicalQuantity(valueId);
      return physicalQuantity != ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber || physicalQuantity != ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber || physicalQuantity != ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber;
    }

    public static KeyValuePair<long, SortedList<DateTime, ReadingValue>>? TryGetValues(
      long expectedValueIdent,
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity1 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(expectedValueIdent);
      ValueIdent.ValueIdPart_MeterType valueIdPartMeterType1 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(expectedValueIdent);
      ValueIdent.ValueIdPart_Calculation idPartCalculation1 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(expectedValueIdent);
      ValueIdent.ValueIdPart_CalculationStart calculationStart1 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(expectedValueIdent);
      ValueIdent.ValueIdPart_StorageInterval partStorageInterval1 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(expectedValueIdent);
      ValueIdent.ValueIdPart_Creation valueIdPartCreation1 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(expectedValueIdent);
      foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair in valueList)
      {
        if (keyValuePair.Value != null && keyValuePair.Value.Count != 0)
        {
          ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity2 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(keyValuePair.Key);
          ValueIdent.ValueIdPart_MeterType valueIdPartMeterType2 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(keyValuePair.Key);
          ValueIdent.ValueIdPart_Calculation idPartCalculation2 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(keyValuePair.Key);
          ValueIdent.ValueIdPart_CalculationStart calculationStart2 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(keyValuePair.Key);
          ValueIdent.ValueIdPart_StorageInterval partStorageInterval2 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(keyValuePair.Key);
          ValueIdent.ValueIdPart_Creation valueIdPartCreation2 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(keyValuePair.Key);
          if ((physicalQuantity1 == ValueIdent.ValueIdPart_PhysicalQuantity.Any || physicalQuantity1 == physicalQuantity2) && (valueIdPartMeterType1 == ValueIdent.ValueIdPart_MeterType.Any || valueIdPartMeterType1 == valueIdPartMeterType2) && (idPartCalculation1 == ValueIdent.ValueIdPart_Calculation.Any || idPartCalculation1 == idPartCalculation2) && (calculationStart1 == ValueIdent.ValueIdPart_CalculationStart.Any || calculationStart1 == calculationStart2) && (partStorageInterval1 == ValueIdent.ValueIdPart_StorageInterval.Any || partStorageInterval1 == partStorageInterval2) && (valueIdPartCreation1 == ValueIdent.ValueIdPart_Creation.Any || valueIdPartCreation1 == valueIdPartCreation2))
            return new KeyValuePair<long, SortedList<DateTime, ReadingValue>>?(keyValuePair);
        }
      }
      return new KeyValuePair<long, SortedList<DateTime, ReadingValue>>?();
    }

    private static KeyValuePair<long, SortedList<DateTime, ReadingValue>>? TryGetCurrentValues(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      return ValueIdent.TryGetValues(ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.Any, ValueIdent.ValueIdPart_MeterType.Any, ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any), valueList);
    }

    private static KeyValuePair<long, SortedList<DateTime, ReadingValue>>? TryGetMonthValues(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      return ValueIdent.TryGetValues(ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.Any, ValueIdent.ValueIdPart_MeterType.Any, ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.Month, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any), valueList);
    }

    public static bool Contains(long valueIdent, ValueIdent.ValueIdPart_Calculation calculated)
    {
      return ValueIdent.Contains(valueIdent, (long) calculated);
    }

    public static bool Contains(List<long> valueIdents, ValueIdent.ValueIdentError indexError)
    {
      if (valueIdents == null)
        return false;
      foreach (long valueIdent in valueIdents)
      {
        if (ValueIdent.Contains(valueIdent, (long) indexError))
          return true;
      }
      return false;
    }

    public static bool Contains(List<long> valueIdents, ValueIdent.ValueIdPart_MeterType meterType)
    {
      if (valueIdents == null)
        return false;
      foreach (long valueIdent in valueIdents)
      {
        if (ValueIdent.Contains(valueIdent, (long) meterType))
          return true;
      }
      return false;
    }

    public static bool Contains(
      List<long> valueIdents,
      ValueIdent.ValueIdPart_StorageInterval storageInterval)
    {
      if (valueIdents == null)
        return false;
      foreach (long valueIdent in valueIdents)
      {
        if (ValueIdent.Contains(valueIdent, (long) storageInterval))
          return true;
      }
      return false;
    }

    public static bool Contains(long valueIdentToCheck, long valueIdentMask)
    {
      return ((valueIdentToCheck & 61440L) == 0L || ((ulong) valueIdentMask & 61440UL) <= 0UL || (valueIdentToCheck & 61440L) == (valueIdentMask & 61440L)) && ((valueIdentToCheck & 4128768L) == 0L || ((ulong) valueIdentMask & 4128768UL) <= 0UL || (valueIdentToCheck & 4128768L) == (valueIdentMask & 4128768L)) && ((valueIdentToCheck & 1879048192L) == 0L || ((ulong) valueIdentMask & 1879048192UL) <= 0UL || (valueIdentToCheck & 1879048192L) == (valueIdentMask & 1879048192L)) && ((valueIdentToCheck & 4032L) == 0L || ((ulong) valueIdentMask & 4032UL) <= 0UL || (valueIdentToCheck & 4032L) == (valueIdentMask & 4032L)) && ((valueIdentToCheck & 63L) == 0L || ((ulong) valueIdentMask & 63UL) <= 0UL || (valueIdentToCheck & 63L) == (valueIdentMask & 63L)) && ((valueIdentToCheck & 264241152L) == 0L || ((ulong) valueIdentMask & 264241152UL) <= 0UL || (valueIdentToCheck & 264241152L) == (valueIdentMask & 264241152L)) && ((valueIdentToCheck & 8793945538560L) == 0L || (valueIdentMask & 8793945538560L) == 0L || (valueIdentToCheck & 8793945538560L) == (valueIdentMask & 8793945538560L));
    }

    public static void CleanUpEmptyValueIdents(
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (ValueList == null || ValueList.Count <= 0)
        return;
      for (int index = ValueList.Count - 1; index >= 0; --index)
      {
        if (ValueList.Values[index].Count == 0)
          ValueList.RemoveAt(index);
      }
    }

    public static bool IsExpectedValueIdent(List<long> filter, long fullValueIdent)
    {
      if (filter == null)
        return true;
      foreach (long num1 in filter)
      {
        if (num1 == fullValueIdent)
          return true;
        long num2 = 0;
        if ((num1 & 63L) > 0L)
          num2 |= 63L;
        if ((num1 & 4032L) > 0L)
          num2 |= 4032L;
        if ((num1 & 61440L) > 0L)
          num2 |= 61440L;
        if ((num1 & 4128768L) > 0L)
          num2 |= 4128768L;
        if ((num1 & 264241152L) > 0L)
          num2 |= 264241152L;
        if ((num1 & 1879048192L) > 0L)
          num2 |= 1879048192L;
        if ((num1 & 8793945538560L) > 0L)
          num2 |= 8793945538560L;
        if ((num1 & num2) == (fullValueIdent & num2))
          return true;
      }
      return false;
    }

    public static SortedList<long, SortedList<DateTime, ReadingValue>> GetDefaultValueListTemplate()
    {
      return new SortedList<long, SortedList<DateTime, ReadingValue>>()
      {
        {
          ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.Any, ValueIdent.ValueIdPart_MeterType.Any, ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any),
          new SortedList<DateTime, ReadingValue>()
        },
        {
          ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.Any, ValueIdent.ValueIdPart_MeterType.Any, ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.DueDate, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any),
          new SortedList<DateTime, ReadingValue>()
        },
        {
          ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.Any, ValueIdent.ValueIdPart_MeterType.Any, ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.Month, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any),
          new SortedList<DateTime, ReadingValue>()
        },
        {
          ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.Any, ValueIdent.ValueIdPart_MeterType.Any, ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.HalfMonth, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any),
          new SortedList<DateTime, ReadingValue>()
        },
        {
          ValueIdent.GetValueIdForValueEnum(ValueIdent.ValueIdPart_PhysicalQuantity.SignalStrength, ValueIdent.ValueIdPart_MeterType.Any, ValueIdent.ValueIdPart_Calculation.Current, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any),
          new SortedList<DateTime, ReadingValue>()
        }
      };
    }

    public static SortedList<long, SortedList<DateTime, ReadingValue>> GetValueList(
      long[] ValueIdList)
    {
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      for (int index = 0; index < ValueIdList.Length; ++index)
        valueList.Add(ValueIdList[index], new SortedList<DateTime, ReadingValue>());
      return valueList;
    }

    public static ValueIdent.ValueIdPart_MeterType ConvertToMeterType(MBusDeviceType mbusMedium)
    {
      return ValueIdent.ConvertToMeterType((byte) mbusMedium);
    }

    public static ValueIdent.ValueIdPart_MeterType ConvertToMeterType(byte mbusMedium)
    {
      switch (mbusMedium)
      {
        case 1:
          return ValueIdent.ValueIdPart_MeterType.Oil;
        case 2:
          return ValueIdent.ValueIdPart_MeterType.Electricity;
        case 3:
          return ValueIdent.ValueIdPart_MeterType.Gas;
        case 4:
          return ValueIdent.ValueIdPart_MeterType.Heat;
        case 5:
          return ValueIdent.ValueIdPart_MeterType.Steam;
        case 6:
          return ValueIdent.ValueIdPart_MeterType.HotWater;
        case 7:
          return ValueIdent.ValueIdPart_MeterType.Water;
        case 8:
          return ValueIdent.ValueIdPart_MeterType.HeatCostAllocator;
        case 9:
          return ValueIdent.ValueIdPart_MeterType.Barometer;
        case 10:
          return ValueIdent.ValueIdPart_MeterType.Cooling;
        case 11:
          return ValueIdent.ValueIdPart_MeterType.Cooling;
        case 12:
          return ValueIdent.ValueIdPart_MeterType.Heat;
        case 13:
          return ValueIdent.ValueIdPart_MeterType.ChangeOverHeat;
        case 22:
          return ValueIdent.ValueIdPart_MeterType.ColdWater;
        case 24:
          return ValueIdent.ValueIdPart_MeterType.Barometer;
        default:
          return ValueIdent.ValueIdPart_MeterType.Any;
      }
    }

    public static ValueIdent.ValueIdPart_MeterType ConvertToMeterType(DeviceTypes deviceType)
    {
      switch (deviceType)
      {
        case DeviceTypes.ZR_EHCA:
        case DeviceTypes.EHCA_M5:
        case DeviceTypes.EHCA_M5p:
        case DeviceTypes.EHCA_M6:
        case DeviceTypes.EHCA_M6_Radio3:
          return ValueIdent.ValueIdPart_MeterType.HeatCostAllocator;
        case DeviceTypes.MinotelContact:
        case DeviceTypes.MinotelContactRadio3:
        case DeviceTypes.ISF:
        case DeviceTypes.PDC:
          return ValueIdent.ValueIdPart_MeterType.PulseCounter;
        case DeviceTypes.Aqua:
        case DeviceTypes.AquaMicro:
        case DeviceTypes.AquaMicroRadio3:
        case DeviceTypes.EDC:
          return ValueIdent.ValueIdPart_MeterType.Water;
        case DeviceTypes.MinoConnect:
          return ValueIdent.ValueIdPart_MeterType.Transceiver;
        case DeviceTypes.SmokeDetector:
          return ValueIdent.ValueIdPart_MeterType.SmokeDetector;
        case DeviceTypes.TemperatureSensor:
          return ValueIdent.ValueIdPart_MeterType.Thermometer;
        case DeviceTypes.HumiditySensor:
          return ValueIdent.ValueIdPart_MeterType.Hygrometer;
        default:
          return ValueIdent.ValueIdPart_MeterType.Any;
      }
    }

    private static void FillSortedList(SortedList<long, string> sortedList, Type ValueIdPart)
    {
      foreach (string str1 in Util.GetNamesOfEnum(ValueIdPart))
      {
        string name = ValueIdPart.Name + str1;
        string str2 = ValueIdent.valueIdentResource.GetString(name);
        if (string.IsNullOrEmpty(str2))
        {
          Debug.WriteLine("NOTE: Translation text in ValueIdentTranslatin.resx not found! Key: " + name);
          str2 = str1;
        }
        long key = (long) Enum.Parse(ValueIdPart, str1, true);
        if (sortedList.ContainsKey(key))
          Debug.WriteLine("NOTE: Translation text in ValueIdentTranslatin.resx has not unique result! name: " + str1);
        else
          sortedList.Add(key, str2);
      }
    }

    public static bool TryFindDueDateValuesAndAddToExistingValueList(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      DateTime expectedDueDatePeriod,
      int minDaysOfDueDatePeriod,
      int maxDaysOfDueDatePeriod)
    {
      if (valueList == null || valueList.Count == 0)
        return false;
      KeyValuePair<long, SortedList<DateTime, ReadingValue>>? nullable1 = new KeyValuePair<long, SortedList<DateTime, ReadingValue>>?();
      KeyValuePair<long, SortedList<DateTime, ReadingValue>>? currentValues = ValueIdent.TryGetCurrentValues(valueList);
      KeyValuePair<long, SortedList<DateTime, ReadingValue>>? nullable2;
      if (currentValues.HasValue)
      {
        nullable2 = currentValues;
      }
      else
      {
        KeyValuePair<long, SortedList<DateTime, ReadingValue>>? monthValues = ValueIdent.TryGetMonthValues(valueList);
        if (!monthValues.HasValue)
          return false;
        nullable2 = monthValues;
      }
      List<DateTime> dateTimeList = new List<DateTime>();
      foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair in nullable2.Value.Value)
      {
        DateTime possibleDueDate = keyValuePair.Key;
        DateTime dueDatePeriod = new DateTime(possibleDueDate.Year, expectedDueDatePeriod.Month, expectedDueDatePeriod.Day);
        DateTime dateTime1 = dueDatePeriod.AddDays((double) minDaysOfDueDatePeriod);
        DateTime dateTime2 = dueDatePeriod.AddDays((double) maxDaysOfDueDatePeriod);
        dateTime2 = dateTime2.AddHours(23.0);
        DateTime dateTime3 = dateTime2.AddMinutes(59.0);
        if (possibleDueDate >= dateTime1 && possibleDueDate <= dateTime3)
        {
          double diff = Math.Abs(dueDatePeriod.Subtract(possibleDueDate).TotalMinutes);
          DateTime dateTime4 = dateTimeList.Find((Predicate<DateTime>) (t => t.Year == possibleDueDate.Year && Math.Abs(dueDatePeriod.Subtract(t).TotalMinutes) > diff));
          if (dateTime4 != DateTime.MinValue)
          {
            dateTimeList.Remove(dateTime4);
            dateTimeList.Add(possibleDueDate);
          }
          else if (!(dateTimeList.Find((Predicate<DateTime>) (t => t.Year == possibleDueDate.Year && Math.Abs(dueDatePeriod.Subtract(t).TotalMinutes) < diff)) != DateTime.MinValue))
            dateTimeList.Add(possibleDueDate);
        }
      }
      if (dateTimeList.Count == 0)
        return false;
      KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 = nullable2.Value;
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(keyValuePair1.Key);
      keyValuePair1 = nullable2.Value;
      ValueIdent.ValueIdPart_MeterType meterType = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(keyValuePair1.Key);
      long valueIdForValueEnum = ValueIdent.GetValueIdForValueEnum(physicalQuantity, meterType, ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, ValueIdent.ValueIdPart_StorageInterval.DueDate, ValueIdent.ValueIdPart_Creation.ReadingSystem, (object) ValueIdent.ValueIdPart_Index.Any);
      SortedList<DateTime, ReadingValue> sortedList1 = new SortedList<DateTime, ReadingValue>();
      foreach (DateTime key1 in dateTimeList)
      {
        SortedList<DateTime, ReadingValue> sortedList2 = sortedList1;
        DateTime key2 = key1;
        keyValuePair1 = nullable2.Value;
        ReadingValue readingValue = keyValuePair1.Value[key1];
        sortedList2.Add(key2, readingValue);
      }
      if (!valueList.ContainsKey(valueIdForValueEnum))
      {
        valueList.Add(valueIdForValueEnum, sortedList1);
      }
      else
      {
        foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in sortedList1)
        {
          if (!valueList[valueIdForValueEnum].ContainsKey(keyValuePair2.Key))
            valueList[valueIdForValueEnum].Add(keyValuePair2.Key, keyValuePair2.Value);
        }
      }
      return true;
    }

    public static List<string> Translate(List<long> valueIdents)
    {
      List<string> stringList = new List<string>();
      foreach (long valueIdent in valueIdents)
      {
        string str = ValueIdent.GetTranslatedValueNameForValueId(valueIdent, false).Replace(", 0", "");
        stringList.Add("(" + valueIdent.ToString() + ") " + str);
      }
      return stringList;
    }

    public static string Translate<T>(byte enumIndex)
    {
      if (typeof (T) == typeof (ValueIdent.ValueIdPart_Calculation))
      {
        long key = (long) enumIndex * 4096L;
        if (ValueIdent.valueIdPart_CalculationEnumToTranslation.ContainsKey(key))
          return ValueIdent.valueIdPart_CalculationEnumToTranslation[key];
      }
      else if (typeof (T) == typeof (ValueIdent.ValueIdPart_CalculationStart))
      {
        long key = (long) enumIndex * 65536L;
        if (ValueIdent.valueIdPart_CalculationStartEnumToTranslation.ContainsKey(key))
          return ValueIdent.valueIdPart_CalculationStartEnumToTranslation[key];
      }
      else if (typeof (T) == typeof (ValueIdent.ValueIdPart_Creation))
      {
        long key = (long) enumIndex * 268435456L;
        if (ValueIdent.valueIdPart_CreationEnumToTranslation.ContainsKey(key))
          return ValueIdent.valueIdPart_CreationEnumToTranslation[key];
      }
      else if (typeof (T) == typeof (ValueIdent.ValueIdPart_MeterType))
      {
        long key = (long) enumIndex * 64L;
        if (ValueIdent.valueIdPart_MeterTypeEnumToTranslation.ContainsKey(key))
          return ValueIdent.valueIdPart_MeterTypeEnumToTranslation[key];
      }
      else if (typeof (T) == typeof (ValueIdent.ValueIdPart_PhysicalQuantity))
      {
        long key = (long) enumIndex;
        if (ValueIdent.valueIdPart_PhysicalQuantityEnumToTranslation.ContainsKey(key))
          return ValueIdent.valueIdPart_PhysicalQuantityEnumToTranslation[key];
      }
      else if (typeof (T) == typeof (ValueIdent.ValueIdPart_StorageInterval))
      {
        long key = (long) enumIndex * 4194304L;
        if (ValueIdent.valueIdPart_StorageIntervalEnumToTranslation.ContainsKey(key))
          return ValueIdent.valueIdPart_StorageIntervalEnumToTranslation[key];
      }
      return string.Empty;
    }

    public static string TranslateIndex(byte physicalQuantity, byte valueIdentIndex)
    {
      switch (ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(ValueIdent.GetValueIdent(valueIdentIndex, physicalQuantity, (byte) 0, (byte) 0, (byte) 0, (byte) 0, (byte) 0)))
      {
        case ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber:
          long key1 = (long) valueIdentIndex * 2147483648L;
          if (ValueIdent.valueIdPart_IndexErrorEnumToTranslation.ContainsKey(key1))
            return ValueIdent.valueIdPart_IndexErrorEnumToTranslation[key1];
          break;
        case ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber:
          long key2 = (long) valueIdentIndex * 2147483648L;
          if (ValueIdent.valueIdPart_IndexWarningEnumToTranslation.ContainsKey(key2))
            return ValueIdent.valueIdPart_IndexWarningEnumToTranslation[key2];
          break;
        case ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber:
          long key3 = (long) valueIdentIndex * 2147483648L;
          if (ValueIdent.valueIdPart_IndexInfoEnumToTranslation.ContainsKey(key3))
            return ValueIdent.valueIdPart_IndexInfoEnumToTranslation[key3];
          break;
      }
      return string.Empty;
    }

    public static string TranslateIndex(long valueIdent, byte valueIdentIndex)
    {
      switch (ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueIdent))
      {
        case ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber:
          long key1 = (long) valueIdentIndex * 2147483648L;
          if (ValueIdent.valueIdPart_IndexErrorEnumToTranslation.ContainsKey(key1))
            return ValueIdent.valueIdPart_IndexErrorEnumToTranslation[key1];
          break;
        case ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber:
          long key2 = (long) valueIdentIndex * 2147483648L;
          if (ValueIdent.valueIdPart_IndexWarningEnumToTranslation.ContainsKey(key2))
            return ValueIdent.valueIdPart_IndexWarningEnumToTranslation[key2];
          break;
        case ValueIdent.ValueIdPart_PhysicalQuantity.InfoNumber:
          long key3 = (long) valueIdentIndex * 2147483648L;
          if (ValueIdent.valueIdPart_IndexInfoEnumToTranslation.ContainsKey(key3))
            return ValueIdent.valueIdPart_IndexInfoEnumToTranslation[key3];
          break;
      }
      return string.Empty;
    }

    public static long GetValueIdentOfError(
      ValueIdent.ValueIdPart_MeterType meterType,
      ValueIdent.ValueIdentError error)
    {
      return (long) (139279L + meterType + 4194304L + 268435456L + (long) error);
    }

    public static long GetValueIdentOfWarninig(
      ValueIdent.ValueIdPart_MeterType meterType,
      ValueIdent.ValueIdentWarning error,
      ValueIdent.ValueIdPart_Creation creation)
    {
      return (long) (139286L + meterType + 4194304L + (long) creation + (long) error);
    }

    public static void AddValueToValueIdentList(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      DateTime timePoint,
      long valueIdent,
      object obj)
    {
      if (valueList == null)
        valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      ReadingValue readingValue = new ReadingValue();
      readingValue.value = Util.ToDouble(obj);
      readingValue.state = ReadingValueState.ok;
      if (valueList.ContainsKey(valueIdent))
      {
        if (valueList[valueIdent].ContainsKey(timePoint))
          return;
        valueList[valueIdent].Add(timePoint, readingValue);
      }
      else
        valueList.Add(valueIdent, new SortedList<DateTime, ReadingValue>()
        {
          {
            timePoint,
            readingValue
          }
        });
    }

    public static SortedList<long, SortedList<DateTime, ReadingValue>> FilterMeterValues(
      SortedList<long, SortedList<DateTime, ReadingValue>> source,
      List<long> filter)
    {
      if (source == null)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      if (filter == null || filter.Count == 0)
        return source;
      for (int index = source.Keys.Count - 1; index >= 0; --index)
      {
        long key = source.Keys[index];
        if (!ValueIdent.IsExpectedValueIdent(filter, key))
          source.Remove(key);
      }
      return source;
    }

    public static string ToString(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      Dictionary<long, Type> types)
    {
      if (valueList == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in valueList)
      {
        long key1 = keyValuePair1.Key;
        SortedList<DateTime, ReadingValue> sortedList = keyValuePair1.Value;
        string valueNameForValueId = ValueIdent.GetTranslatedValueNameForValueId(key1, false);
        foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in sortedList)
        {
          DateTime key2 = keyValuePair2.Key;
          double num = keyValuePair2.Value.value;
          stringBuilder.Append(key2.ToString("dd.MM.yy HH:mm"));
          stringBuilder.Append(" Value: ");
          stringBuilder.Append(num);
          stringBuilder.Append(" ");
          stringBuilder.Append(ValueIdent.GetUnit(key1));
          stringBuilder.Append(" ");
          if (types != null && types.ContainsKey(key1))
          {
            Type type = types[key1];
            if (type.IsEnum)
            {
              object obj = Enum.Parse(type, Convert.ToInt64(num).ToString(), true);
              stringBuilder.Append(" (");
              stringBuilder.Append(obj);
              stringBuilder.Append(")");
            }
            else
            {
              if (!type.Equals(typeof (bool)))
                throw new NotSupportedException();
              bool boolean = Convert.ToBoolean(Convert.ToInt64(num));
              stringBuilder.Append(" (");
              stringBuilder.Append(boolean);
              stringBuilder.Append(")");
            }
          }
          stringBuilder.Append(" ");
          stringBuilder.Append(key1);
          stringBuilder.Append(": ");
          stringBuilder.AppendLine(valueNameForValueId);
        }
      }
      return stringBuilder.ToString();
    }

    public static string ToPathLikeText(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      if (valueList == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in valueList)
      {
        long key1 = keyValuePair1.Key;
        SortedList<DateTime, ReadingValue> sortedList = keyValuePair1.Value;
        ValueIdent.GetValueIdNameAsPath(key1);
        foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in sortedList)
        {
          DateTime key2 = keyValuePair2.Key;
          double doubleValue = keyValuePair2.Value.value;
          stringBuilder.AppendLine(ValueIdent.ToPathLine(key2, doubleValue, key1));
        }
      }
      return stringBuilder.ToString();
    }

    public static string ToPathLikeTextByTimeOrder(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      if (valueList == null)
        return string.Empty;
      List<ValueIdentTimeListItem> timeListFromIdList = ValueIdent.ToTimeListFromIdList(valueList);
      StringBuilder stringBuilder = new StringBuilder();
      int itemNumber = 0;
      foreach (ValueIdentTimeListItem identTimeListItem in timeListFromIdList)
      {
        stringBuilder.AppendLine(identTimeListItem.ToPathLikeString(itemNumber));
        ++itemNumber;
      }
      return stringBuilder.ToString();
    }

    public static string ToPathLikeErrorAndWarningTextByTimeOrder(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      if (valueList == null)
        return string.Empty;
      List<ValueIdentTimeListItem> eventTimeList = ValueIdent.ToEventTimeList(valueList);
      StringBuilder stringBuilder = new StringBuilder();
      int itemNumber = 0;
      foreach (ValueIdentTimeListItem identTimeListItem in eventTimeList)
      {
        stringBuilder.AppendLine(identTimeListItem.ToPathLikeString(itemNumber));
        ++itemNumber;
      }
      return stringBuilder.ToString();
    }

    public static string ToPathLine(DateTime timePoint, double doubleValue, long valueIdent)
    {
      return timePoint.ToString("dd.MM.yy HH:mm:ss") + (" " + ValueIdent.GetPhysicalName(valueIdent)).PadLeft(40, '.') + ": " + (doubleValue.ToString() + " " + ValueIdent.GetUnit(valueIdent) + " ").PadRight(20, '.') + " " + ValueIdent.GetValueIdNameAsPath(valueIdent) + " (0x" + valueIdent.ToString("x016") + ")";
    }

    public static List<ValueIdentTimeListItem> ToTimeListFromIdList(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      if (valueList == null)
        return (List<ValueIdentTimeListItem>) null;
      List<ValueIdentTimeListItem> timeListFromIdList = new List<ValueIdentTimeListItem>();
      foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in valueList)
      {
        long key1 = keyValuePair1.Key;
        foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in keyValuePair1.Value)
        {
          DateTime key2 = keyValuePair2.Key;
          ReadingValue readingValue = keyValuePair2.Value;
          if (readingValue.state == 0)
            timeListFromIdList.Add(new ValueIdentTimeListItem(key2, key1, readingValue.value));
        }
      }
      timeListFromIdList.Sort();
      return timeListFromIdList;
    }

    public static List<ValueIdentTimeListItem> ToEventTimeList(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      if (valueList == null)
        return (List<ValueIdentTimeListItem>) null;
      List<ValueIdentTimeListItem> eventTimeList = new List<ValueIdentTimeListItem>();
      foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in valueList)
      {
        long key1 = keyValuePair1.Key;
        if (ValueIdent.IsEvent(key1))
        {
          foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in keyValuePair1.Value)
          {
            DateTime key2 = keyValuePair2.Key;
            ReadingValue readingValue = keyValuePair2.Value;
            if (readingValue.state == 0)
              eventTimeList.Add(new ValueIdentTimeListItem(key2, key1, readingValue.value));
          }
        }
      }
      eventTimeList.Sort();
      return eventTimeList;
    }

    public static DataTable ToDataTable(
      SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      if (valueList == null)
        return (DataTable) null;
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add("#", typeof (int));
      dataTable.Columns.Add(nameof (ValueIdent), typeof (ulong));
      dataTable.Columns.Add("Date", typeof (DateTime));
      dataTable.Columns.Add("Value", typeof (double));
      dataTable.Columns.Add("Unit", typeof (string));
      dataTable.Columns.Add("Description", typeof (string));
      int num1 = 1;
      foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in valueList)
      {
        long key1 = keyValuePair1.Key;
        SortedList<DateTime, ReadingValue> sortedList = keyValuePair1.Value;
        string valueNameForValueId = ValueIdent.GetTranslatedValueNameForValueId(key1, false);
        foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in sortedList)
        {
          DateTime key2 = keyValuePair2.Key;
          double num2 = keyValuePair2.Value.value;
          dataTable.Rows.Add((object) num1++, (object) key1, (object) key2, (object) num2, (object) ValueIdent.GetUnit(key1), (object) valueNameForValueId);
        }
      }
      return dataTable;
    }

    public static ValueIdent.ValueIdPart_PhysicalQuantity ConvertToPhysicalQuantity(byte vif)
    {
      byte num = vif;
      if (num <= (byte) 15)
        return ValueIdent.ValueIdPart_PhysicalQuantity.Energy;
      switch (num)
      {
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
        case 21:
        case 22:
          return ValueIdent.ValueIdPart_PhysicalQuantity.Volume;
        case 110:
          return ValueIdent.ValueIdPart_PhysicalQuantity.Pulse;
        default:
          throw new NotSupportedException("VIF 0x" + vif.ToString("X2"));
      }
    }

    public enum ValueId_Mask : long
    {
      PhysicalQuantity = 63, // 0x000000000000003F
      MeterType = 4032, // 0x0000000000000FC0
      Calculation = 61440, // 0x000000000000F000
      CalculationStart = 4128768, // 0x00000000003F0000
      StorageInterval = 264241152, // 0x000000000FC00000
      Creation = 1879048192, // 0x0000000070000000
      Index = 8793945538560, // 0x000007FF80000000
      Tarif = 272678883688448, // 0x0000F80000000000
      full = 9223372036854775807, // 0x7FFFFFFFFFFFFFFF
    }

    public enum ValueId_MinValue : long
    {
      PhysicalQuantity = 1,
      MeterType = 64, // 0x0000000000000040
      Calculation = 4096, // 0x0000000000001000
      CalculationStart = 65536, // 0x0000000000010000
      StorageInterval = 4194304, // 0x0000000000400000
      Creation = 268435456, // 0x0000000010000000
      Index = 2147483648, // 0x0000000080000000
      Tarif = 8796093022208, // 0x0000080000000000
      NotDefined = 281474976710656, // 0x0001000000000000
    }

    public enum ValueIdPart_PhysicalQuantity : long
    {
      Any,
      Volume,
      Energy,
      Flow,
      Power,
      Mass,
      Temperature,
      TempDiff,
      HCA,
      HCA_Weighted,
      Pulse,
      CoolingEnergy,
      TempFlow,
      TempReturn,
      OperatingHours,
      ErrorNumber,
      StatusNumber,
      DigitalInputOutput,
      SignalStrength,
      CoolingVolume,
      Percent,
      DateTime,
      WarningNumber,
      ReturnVolume,
      LiterPerImpuls,
      FlowVolume,
      InfoNumber,
    }

    public enum ValueIdPart_MeterType : long
    {
      Any = 0,
      Water = 64, // 0x0000000000000040
      ColdWater = 128, // 0x0000000000000080
      WarmWater = 192, // 0x00000000000000C0
      HotWater = 256, // 0x0000000000000100
      Heat = 320, // 0x0000000000000140
      Electricity = 384, // 0x0000000000000180
      Gas = 448, // 0x00000000000001C0
      Oil = 512, // 0x0000000000000200
      Steam = 576, // 0x0000000000000240
      HeatCostAllocator = 640, // 0x0000000000000280
      PulseCounter = 704, // 0x00000000000002C0
      VolumeMeter = 768, // 0x0000000000000300
      Cooling = 832, // 0x0000000000000340
      ChangeOverHeat = 896, // 0x0000000000000380
      Barometer = 960, // 0x00000000000003C0
      Other = 1024, // 0x0000000000000400
      Collector = 1088, // 0x0000000000000440
      Thermometer = 1152, // 0x0000000000000480
      Hygrometer = 1216, // 0x00000000000004C0
      Transceiver = 1280, // 0x0000000000000500
      SmokeDetector = 1344, // 0x0000000000000540
    }

    public enum ValueIdPart_Calculation : long
    {
      Any = 0,
      Accumulated = 4096, // 0x0000000000001000
      Current = 8192, // 0x0000000000002000
      Maximum = 12288, // 0x0000000000003000
      Minimum = 16384, // 0x0000000000004000
      Average = 20480, // 0x0000000000005000
      Difference = 24576, // 0x0000000000006000
    }

    public enum ValueIdPart_CalculationStart : long
    {
      Any = 0,
      MeterProduction = 65536, // 0x0000000000010000
      Current = 131072, // 0x0000000000020000
      MeterCycle = 196608, // 0x0000000000030000
      DueDate = 262144, // 0x0000000000040000
      Year = 327680, // 0x0000000000050000
      Month = 393216, // 0x0000000000060000
      HalfMonth = 458752, // 0x0000000000070000
      Week = 524288, // 0x0000000000080000
      Day = 589824, // 0x0000000000090000
      Hour = 655360, // 0x00000000000A0000
      HalfHour = 720896, // 0x00000000000B0000
      QuarterHour = 786432, // 0x00000000000C0000
      Event = 851968, // 0x00000000000D0000
    }

    public enum ValueIdPart_StorageInterval : long
    {
      Any = 0,
      None = 4194304, // 0x0000000000400000
      MeterCycle = 8388608, // 0x0000000000800000
      DueDate = 12582912, // 0x0000000000C00000
      Year = 16777216, // 0x0000000001000000
      Month = 20971520, // 0x0000000001400000
      HalfMonth = 25165824, // 0x0000000001800000
      Week = 29360128, // 0x0000000001C00000
      Day = 33554432, // 0x0000000002000000
      Hour = 37748736, // 0x0000000002400000
      HalfHour = 41943040, // 0x0000000002800000
      QuarterHour = 46137344, // 0x0000000002C00000
    }

    public enum ValueIdPart_Creation : long
    {
      Any = 0,
      Meter = 268435456, // 0x0000000010000000
      ReadingSystem = 536870912, // 0x0000000020000000
      Estimation = 805306368, // 0x0000000030000000
      BitCompression = 1073741824, // 0x0000000040000000
      Manually = 1342177280, // 0x0000000050000000
      MeterLogger = 1610612736, // 0x0000000060000000
    }

    public enum ValueIdPart_Index : long
    {
      Any,
    }

    public enum ValueIdentInfo : long
    {
      Any = 0,
      HornFailureGone = 2147483648, // 0x0000000080000000
      PushButtonFailureGone = 4294967296, // 0x0000000100000000
      Mount = 6442450944, // 0x0000000180000000
      PollutionGone = 8589934592, // 0x0000000200000000
      ObstructionGone = 10737418240, // 0x0000000280000000
      ObjectInSurroundingAreaGone = 12884901888, // 0x0000000300000000
      TestAlarm = 15032385536, // 0x0000000380000000
      DeviceWakeup = 17179869184, // 0x0000000400000000
      WaterInMeasuringTube = 19327352832, // 0x0000000480000000
      ConfigurationChanged = 21474836480, // 0x0000000500000000
      UnitChanged = 23622320128, // 0x0000000580000000
    }

    public enum ValueIdentWarning : long
    {
      Any = 0,
      BatteryLow = 2147483648, // 0x0000000080000000
      RadioDisabled = 4294967296, // 0x0000000100000000
      LoggerDisabled = 6442450944, // 0x0000000180000000
      PulseDisabled = 8589934592, // 0x0000000200000000
      FailedToRead = 10737418240, // 0x0000000280000000
      Leak = 12884901888, // 0x0000000300000000
      Blockage = 15032385536, // 0x0000000380000000
      Backflow = 17179869184, // 0x0000000400000000
      Tamper = 19327352832, // 0x0000000480000000
      Removal = 21474836480, // 0x0000000500000000
      Oversized = 23622320128, // 0x0000000580000000
      Undersized = 25769803776, // 0x0000000600000000
      Burst = 27917287424, // 0x0000000680000000
      TemporaryError = 30064771072, // 0x0000000700000000
      TemperatureOutOfMeasuringRangeLow = 32212254720, // 0x0000000780000000
      TemperatureOutOfMeasuringRangeHigh = 34359738368, // 0x0000000800000000
      ShortCircuitReturnSensor = 36507222016, // 0x0000000880000000
      InterruptionReturnSensor = 38654705664, // 0x0000000900000000
      ShortCircuitSupplySensor = 40802189312, // 0x0000000980000000
      InterruptionSupplySensor = 42949672960, // 0x0000000A00000000
      NoWaterInMeasuringTube = 45097156608, // 0x0000000A80000000
      ReverseWaterFlowDetected = 47244640256, // 0x0000000B00000000
      AirInsideMedium = 49392123904, // 0x0000000B80000000
      VolumeFlowOutOfRange = 51539607552, // 0x0000000C00000000
      OpticalInterfacePowerOff = 53687091200, // 0x0000000C80000000
      TimebaseError = 55834574848, // 0x0000000D00000000
      Manipulation = 57982058496, // 0x0000000D80000000
      BatteryOver = 60129542144, // 0x0000000E00000000
      BatteryForeWarning = 62277025792, // 0x0000000E80000000
      Battery = 64424509440, // 0x0000000F00000000
      Pollution = 66571993088, // 0x0000000F80000000
      SmokeAlarm = 68719476736, // 0x0000001000000000
      ObstructionDetected = 70866960384, // 0x0000001080000000
      ObjectInSurroundingAreaDetected = 73014444032, // 0x0000001100000000
    }

    public enum ValueIdentError : long
    {
      Any = 0,
      DeviceError = 2147483648, // 0x0000000080000000
      Manipulation = 4294967296, // 0x0000000100000000
      BatteryVoltage = 6442450944, // 0x0000000180000000
      HardwareErrorRef1 = 8589934592, // 0x0000000200000000
      HardwareErrorRef2 = 10737418240, // 0x0000000280000000
      HardwareErrorUltrasonic = 12884901888, // 0x0000000300000000
      HardwareErrorInterpreter = 15032385536, // 0x0000000380000000
      ErrorWritePermission = 17179869184, // 0x0000000400000000
      ErrorWirelessInterface = 19327352832, // 0x0000000480000000
      StatusEndOfBattery = 21474836480, // 0x0000000500000000
      StatusInitialVerificationExpired = 23622320128, // 0x0000000580000000
      BatteryFault = 25769803776, // 0x0000000600000000
      HornFailure = 27917287424, // 0x0000000680000000
      PushButtonFailure = 30064771072, // 0x0000000700000000
      LED_Failure = 32212254720, // 0x0000000780000000
      Reset = 34359738368, // 0x0000000800000000
      Watchdog = 36507222016, // 0x0000000880000000
      MeasurementWatchdog = 38654705664, // 0x0000000900000000
      BackupReloaded = 40802189312, // 0x0000000980000000
      RAM_NotPlausible = 42949672960, // 0x0000000A00000000
      CRC_Error = 45097156608, // 0x0000000A80000000
    }

    public enum ValueId_Predefined : long
    {
      Water_Current = 272699457, // 0x0000000010411041
      HCA_Current = 272700040, // 0x0000000010411288
      HCA_CurrentW = 272700041, // 0x0000000010411289
      Pulse_Current = 272700106, // 0x00000000104112CA
      SignalStrength = 272703506, // 0x0000000010412012
      Heat_Volume = 272769345, // 0x0000000010422141
      Heat_Energy = 272769346, // 0x0000000010422142
      Heat_TempDiff = 272769351, // 0x0000000010422147
      Heat_CoolingEnergy = 272769355, // 0x000000001042214B
      Heat_TempFlow = 272769356, // 0x000000001042214C
      Heat_TempReturn = 272769357, // 0x000000001042214D
      T_Current = 272770182, // 0x0000000010422486
      RH_Current = 272770260, // 0x00000000104224D4
      Water_DueDate = 281088065, // 0x0000000010C11041
      HCA_DueDate = 281088648, // 0x0000000010C11288
      HCA_DueDateW = 281088649, // 0x0000000010C11289
      Pulse_DueDate = 281088714, // 0x0000000010C112CA
      Water_Month = 289476673, // 0x0000000011411041
      HCA_Month = 289477256, // 0x0000000011411288
      HCA_MonthW = 289477257, // 0x0000000011411289
      Pulse_Month = 289477322, // 0x00000000114112CA
      RH_Month = 289821652, // 0x00000000114653D4
      T_Month = 289821830, // 0x0000000011465486
      Water_HalfMonth = 293670977, // 0x0000000011811041
      HCA_HalfMonth = 293671560, // 0x0000000011811288
      HCA_HalfMonthW = 293671561, // 0x0000000011811289
      Pulse_HalfMonth = 293671626, // 0x00000000118112CA
      RH_HalfMonth = 294081492, // 0x00000000118753D4
      T_HalfMonth = 294081670, // 0x0000000011875486
      Water_Day = 302059585, // 0x0000000012011041
      HCA_Day = 302060168, // 0x0000000012011288
      HCA_DayW = 302060169, // 0x0000000012011289
      Pulse_Day = 302060234, // 0x00000000120112CA
      RH_Day = 302601172, // 0x00000000120953D4
      T_Day = 302601350, // 0x0000000012095486
      Water_QuarterHour = 314642497, // 0x0000000012C11041
      SD_Month = 1094788432, // 0x0000000041412550
      SD_HalfMonth = 1098982736, // 0x0000000041812550
      SD_Day = 1107371344, // 0x0000000042012550
      TemperatureDistribution_Current = 1107907718, // 0x0000000042095486
      HumidityDistribution_Current = 1107907796, // 0x00000000420954D4
    }
  }
}
