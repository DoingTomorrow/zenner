// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MeasurementSet
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class MeasurementSet : 
    SortedList<uint, Dictionary<MeasurementDataType, MeasurementData>>
  {
    private static Logger logger = LogManager.GetLogger(nameof (MeasurementSet));
    public const string COLUMN_ID = "Serialnumber";
    public const string COLUMN_TYPE = "Type";
    public const string COLUMN_DATE = "Timepoint";
    public const string COLUMN_VALUE = "Value";
    private string header;

    public void Add(MeasurementData data)
    {
      if (data == null)
        return;
      uint id = data.Header.ID;
      if (!this.ContainsKey(id))
      {
        this.Add(id, new Dictionary<MeasurementDataType, MeasurementData>()
        {
          {
            data.Header.Type,
            data
          }
        });
      }
      else
      {
        Dictionary<MeasurementDataType, MeasurementData> dictionary = this[id];
        if (dictionary.ContainsKey(data.Header.Type))
        {
          foreach (KeyValuePair<DateTime, Decimal?> keyValuePair in data.Data)
          {
            if (!dictionary[data.Header.Type].Data.ContainsKey(keyValuePair.Key))
              dictionary[data.Header.Type].Data.Add(keyValuePair.Key, keyValuePair.Value);
          }
        }
        else
          dictionary.Add(data.Header.Type, data);
      }
    }

    public void Add(MeasurementSet set)
    {
      if (set == null)
        return;
      foreach (KeyValuePair<uint, Dictionary<MeasurementDataType, MeasurementData>> keyValuePair1 in (SortedList<uint, Dictionary<MeasurementDataType, MeasurementData>>) set)
      {
        if (!this.ContainsKey(keyValuePair1.Key))
        {
          this.Add(keyValuePair1.Key, keyValuePair1.Value);
        }
        else
        {
          Dictionary<MeasurementDataType, MeasurementData> dictionary = this[keyValuePair1.Key];
          foreach (KeyValuePair<MeasurementDataType, MeasurementData> keyValuePair2 in keyValuePair1.Value)
          {
            if (dictionary.ContainsKey(keyValuePair2.Key))
            {
              foreach (KeyValuePair<DateTime, Decimal?> keyValuePair3 in keyValuePair2.Value.Data)
              {
                if (!dictionary[keyValuePair2.Key].Data.ContainsKey(keyValuePair3.Key))
                  dictionary[keyValuePair2.Key].Data.Add(keyValuePair3.Key, keyValuePair3.Value);
              }
            }
            else
              dictionary.Add(keyValuePair2.Key, keyValuePair2.Value);
          }
        }
      }
    }

    public MeasurementSet()
    {
      this.header = "".PadRight(44, '-') + "\n " + "Serialnumber".PadRight(10) + "| " + "Type".PadRight(10) + "| " + "Timepoint".PadRight(10) + "| " + "Value".PadRight(10) + "| " + "".PadRight(44, '-') + "\n";
    }

    public DataTable CreateTable()
    {
      DataTable emptyTable = this.CreateEmptyTable();
      foreach (KeyValuePair<uint, Dictionary<MeasurementDataType, MeasurementData>> keyValuePair1 in (SortedList<uint, Dictionary<MeasurementDataType, MeasurementData>>) this)
      {
        foreach (KeyValuePair<MeasurementDataType, MeasurementData> keyValuePair2 in keyValuePair1.Value)
        {
          foreach (KeyValuePair<DateTime, Decimal?> keyValuePair3 in keyValuePair2.Value.Data)
          {
            ValueIdent.ValueIdPart_MeterType typeOfMinolDevice = NumberRanges.GetValueIdPart_MeterTypeOfMinolDevice((long) keyValuePair1.Key);
            emptyTable.Rows.Add((object) keyValuePair1.Key, (object) typeOfMinolDevice, (object) keyValuePair3.Key, (object) keyValuePair3.Value);
          }
        }
      }
      return emptyTable;
    }

    private DataTable CreateEmptyTable()
    {
      return new DataTable(nameof (MeasurementSet))
      {
        Columns = {
          {
            "Serialnumber",
            typeof (uint)
          },
          {
            "Type",
            typeof (string)
          },
          {
            "Timepoint",
            typeof (DateTime)
          },
          {
            "Value",
            typeof (uint)
          }
        }
      };
    }

    public override string ToString()
    {
      string str = "\t";
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<uint, Dictionary<MeasurementDataType, MeasurementData>> keyValuePair1 in (SortedList<uint, Dictionary<MeasurementDataType, MeasurementData>>) this)
      {
        foreach (KeyValuePair<MeasurementDataType, MeasurementData> keyValuePair2 in keyValuePair1.Value)
        {
          foreach (KeyValuePair<DateTime, Decimal?> keyValuePair3 in keyValuePair2.Value.Data)
            stringBuilder.Append(keyValuePair1.Key).Append(str).Append((object) keyValuePair2.Key).Append(str).Append((object) keyValuePair3.Key).Append(str).Append((object) keyValuePair3.Value).Append(Environment.NewLine);
        }
      }
      return stringBuilder.ToString();
    }
  }
}
