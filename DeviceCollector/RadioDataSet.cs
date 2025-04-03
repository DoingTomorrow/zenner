// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioDataSet
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public sealed class RadioDataSet
  {
    public byte? RSSI_Min { get; private set; }

    public byte? RSSI_Max { get; private set; }

    public RadioPacket LastRadioPacket { get; private set; }

    public int PacketsCount { get; private set; }

    public SortedList<long, SortedList<DateTime, ReadingValue>> Data { get; private set; }

    public int? RSSI_dBm_Average
    {
      get
      {
        return this.RSSI_Average.HasValue ? new int?(Util.RssiToRssi_dBm(this.RSSI_Average.Value)) : new int?();
      }
    }

    public byte? RSSI_Average
    {
      get
      {
        byte? rssiAverage;
        int num1;
        if (this.RSSI_Min.HasValue)
        {
          rssiAverage = this.RSSI_Max;
          num1 = rssiAverage.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          rssiAverage = this.RSSI_Max;
          byte num2 = rssiAverage.Value;
          rssiAverage = this.RSSI_Min;
          byte num3 = rssiAverage.Value;
          return (int) num2 > (int) num3 ? new byte?((byte) ((uint) num3 + (uint) (((int) num2 - (int) num3) / 2))) : new byte?((byte) ((uint) num2 + (uint) (((int) num3 - (int) num2) / 2)));
        }
        rssiAverage = new byte?();
        return rssiAverage;
      }
    }

    internal bool UpdateData(RadioPacket packet)
    {
      if (packet == null)
        return false;
      this.LastRadioPacket = packet;
      ++this.PacketsCount;
      byte? nullable1 = this.RSSI_Max;
      int num;
      if (nullable1.HasValue)
      {
        nullable1 = this.RSSI_Min;
        num = nullable1.HasValue ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        nullable1 = this.RSSI_Max;
        int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        nullable1 = packet.RSSI;
        int? nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        if (nullable2.GetValueOrDefault() < nullable3.GetValueOrDefault() & nullable2.HasValue & nullable3.HasValue)
          this.RSSI_Max = packet.RSSI;
        nullable1 = this.RSSI_Min;
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        nullable1 = packet.RSSI;
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        if (nullable3.GetValueOrDefault() > nullable2.GetValueOrDefault() & nullable3.HasValue & nullable2.HasValue)
          this.RSSI_Min = packet.RSSI;
      }
      else
      {
        this.RSSI_Max = packet.RSSI;
        this.RSSI_Min = packet.RSSI;
      }
      packet.RSSI = this.RSSI_Average;
      this.Data = packet.Merge(this.Data);
      return this.Data != null;
    }

    public bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      List<long> filter = (List<long>) null;
      if (valueList != null && valueList.Count > 0)
      {
        filter = new List<long>();
        filter.AddRange((IEnumerable<long>) valueList.Keys);
      }
      valueList = ValueIdent.FilterMeterValues(this.Data, filter);
      ValueIdent.CleanUpEmptyValueIdents(valueList);
      return true;
    }
  }
}
