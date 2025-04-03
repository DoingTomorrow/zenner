// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.OrdersManagement.ReadingValuesHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Meters;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.OrdersManagement
{
  public class ReadingValuesHelper
  {
    public long GetPredefinedValueId(
      DeviceTypeEnum type,
      ValueIdent.ValueIdPart_MeterType register,
      ValueIdent.ValueIdPart_StorageInterval readingValueType)
    {
      return readingValueType == ValueIdent.ValueIdPart_StorageInterval.DueDate ? this.GetPredefinedDueDateValueId(type, register) : this.GetPredefinedActualValueId(type, register);
    }

    public long GetPredefinedDueDateValueId(
      DeviceTypeEnum type,
      ValueIdent.ValueIdPart_MeterType register)
    {
      long predefinedDueDateValueId;
      switch (type)
      {
        case DeviceTypeEnum.M6:
          predefinedDueDateValueId = 12653192L;
          break;
        case DeviceTypeEnum.M7:
        case DeviceTypeEnum.MinomessMicroRadio3:
          predefinedDueDateValueId = 12653192L;
          break;
        case DeviceTypeEnum.MinotelContactRadio3:
          predefinedDueDateValueId = 12653258L;
          break;
        default:
          predefinedDueDateValueId = !register.Equals((object) ValueIdent.ValueIdPart_MeterType.ColdWater) ? 12652866L : 12653378L;
          break;
      }
      return predefinedDueDateValueId;
    }

    public long GetPredefinedActualValueId(
      DeviceTypeEnum type,
      ValueIdent.ValueIdPart_MeterType register)
    {
      long predefinedActualValueId;
      switch (type)
      {
        case DeviceTypeEnum.M6:
          predefinedActualValueId = 4264584L;
          break;
        case DeviceTypeEnum.M7:
        case DeviceTypeEnum.MinomessMicroRadio3:
          predefinedActualValueId = 4264584L;
          break;
        case DeviceTypeEnum.MinotelContactRadio3:
          predefinedActualValueId = 4264650L;
          break;
        default:
          predefinedActualValueId = !register.Equals((object) ValueIdent.ValueIdPart_MeterType.ColdWater) ? 4264258L : 4264770L;
          break;
      }
      return predefinedActualValueId;
    }

    public static List<MeterReadingValue> ConvertValueIdentToReadingValues(ValueIdentSet valueIdent)
    {
      SortedList<long, SortedList<DateTime, ReadingValue>> availableValues = valueIdent.AvailableValues;
      List<MeterReadingValue> readingValues = new List<MeterReadingValue>();
      if (availableValues != null)
      {
        foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in availableValues)
        {
          long key = keyValuePair1.Key;
          foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in keyValuePair1.Value)
          {
            MeterReadingValue meterReadingValue = new MeterReadingValue()
            {
              MeterSerialNumber = valueIdent.SerialNumber,
              CreatedOn = DateTime.Now,
              Date = keyValuePair2.Key,
              Value = keyValuePair2.Value.value,
              ValueId = key,
              StorageInterval = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(key),
              PhysicalQuantity = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(key),
              MeterType = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(key),
              CalculationStart = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(key),
              Creation = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(key),
              Calculation = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(key),
              Unit = new MeasureUnit()
              {
                Code = ValueIdent.GetUnit(Convert.ToInt64(key))
              }
            };
            readingValues.Add(meterReadingValue);
          }
        }
      }
      return readingValues;
    }
  }
}
