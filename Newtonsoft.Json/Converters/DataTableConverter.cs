// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DataTableConverter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  public class DataTableConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DataTable dataTable = (DataTable) value;
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      writer.WriteStartArray();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        writer.WriteStartObject();
        foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
        {
          object obj = row[column];
          if (serializer.NullValueHandling != NullValueHandling.Ignore || obj != null && obj != DBNull.Value)
          {
            writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName(column.ColumnName) : column.ColumnName);
            serializer.Serialize(writer, obj);
          }
        }
        writer.WriteEndObject();
      }
      writer.WriteEndArray();
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      if (!(existingValue is DataTable dt))
        dt = objectType == typeof (DataTable) ? new DataTable() : (DataTable) Activator.CreateInstance(objectType);
      if (reader.TokenType == JsonToken.PropertyName)
      {
        dt.TableName = (string) reader.Value;
        reader.ReadAndAssert();
        if (reader.TokenType == JsonToken.Null)
          return (object) dt;
      }
      if (reader.TokenType != JsonToken.StartArray)
        throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable. Expected StartArray, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      reader.ReadAndAssert();
      while (reader.TokenType != JsonToken.EndArray)
      {
        DataTableConverter.CreateRow(reader, dt, serializer);
        reader.ReadAndAssert();
      }
      return (object) dt;
    }

    private static void CreateRow(JsonReader reader, DataTable dt, JsonSerializer serializer)
    {
      DataRow row = dt.NewRow();
      reader.ReadAndAssert();
      while (reader.TokenType == JsonToken.PropertyName)
      {
        string str = (string) reader.Value;
        reader.ReadAndAssert();
        DataColumn column = dt.Columns[str];
        if (column == null)
        {
          Type columnDataType = DataTableConverter.GetColumnDataType(reader);
          column = new DataColumn(str, columnDataType);
          dt.Columns.Add(column);
        }
        if (column.DataType == typeof (DataTable))
        {
          if (reader.TokenType == JsonToken.StartArray)
            reader.ReadAndAssert();
          DataTable dt1 = new DataTable();
          while (reader.TokenType != JsonToken.EndArray)
          {
            DataTableConverter.CreateRow(reader, dt1, serializer);
            reader.ReadAndAssert();
          }
          row[str] = (object) dt1;
        }
        else if (column.DataType.IsArray && column.DataType != typeof (byte[]))
        {
          if (reader.TokenType == JsonToken.StartArray)
            reader.ReadAndAssert();
          List<object> objectList = new List<object>();
          while (reader.TokenType != JsonToken.EndArray)
          {
            objectList.Add(reader.Value);
            reader.ReadAndAssert();
          }
          Array instance = Array.CreateInstance(column.DataType.GetElementType(), objectList.Count);
          Array.Copy((Array) objectList.ToArray(), instance, objectList.Count);
          row[str] = (object) instance;
        }
        else
          row[str] = reader.Value != null ? serializer.Deserialize(reader, column.DataType) : (object) DBNull.Value;
        reader.ReadAndAssert();
      }
      row.EndEdit();
      dt.Rows.Add(row);
    }

    private static Type GetColumnDataType(JsonReader reader)
    {
      JsonToken tokenType = reader.TokenType;
      switch (tokenType)
      {
        case JsonToken.StartArray:
          reader.ReadAndAssert();
          return reader.TokenType == JsonToken.StartObject ? typeof (DataTable) : DataTableConverter.GetColumnDataType(reader).MakeArrayType();
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Date:
        case JsonToken.Bytes:
          return reader.ValueType;
        case JsonToken.Null:
        case JsonToken.Undefined:
          return typeof (string);
        default:
          throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) tokenType));
      }
    }

    public override bool CanConvert(Type valueType)
    {
      return typeof (DataTable).IsAssignableFrom(valueType);
    }
  }
}
