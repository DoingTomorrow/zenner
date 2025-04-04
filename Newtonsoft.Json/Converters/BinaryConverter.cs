// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.BinaryConverter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  public class BinaryConverter : JsonConverter
  {
    private const string BinaryTypeName = "System.Data.Linq.Binary";
    private const string BinaryToArrayName = "ToArray";
    private ReflectionObject _reflectionObject;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        byte[] byteArray = this.GetByteArray(value);
        writer.WriteValue(byteArray);
      }
    }

    private byte[] GetByteArray(object value)
    {
      if (value.GetType().AssignableToTypeName("System.Data.Linq.Binary"))
      {
        this.EnsureReflectionObject(value.GetType());
        return (byte[]) this._reflectionObject.GetValue(value, "ToArray");
      }
      return value is SqlBinary sqlBinary ? sqlBinary.Value : throw new JsonSerializationException("Unexpected value type when writing binary: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
    }

    private void EnsureReflectionObject(Type t)
    {
      if (this._reflectionObject != null)
        return;
      this._reflectionObject = ReflectionObject.Create(t, (MethodBase) t.GetConstructor(new Type[1]
      {
        typeof (byte[])
      }), "ToArray");
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullable(objectType))
          throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      byte[] numArray;
      if (reader.TokenType == JsonToken.StartArray)
        numArray = this.ReadByteArray(reader);
      else
        numArray = reader.TokenType == JsonToken.String ? Convert.FromBase64String(reader.Value.ToString()) : throw JsonSerializationException.Create(reader, "Unexpected token parsing binary. Expected String or StartArray, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      if (type.AssignableToTypeName("System.Data.Linq.Binary"))
      {
        this.EnsureReflectionObject(type);
        return this._reflectionObject.Creator((object) numArray);
      }
      if (type == typeof (SqlBinary))
        return (object) new SqlBinary(numArray);
      throw JsonSerializationException.Create(reader, "Unexpected object type when writing binary: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
    }

    private byte[] ReadByteArray(JsonReader reader)
    {
      List<byte> byteList = new List<byte>();
      while (reader.Read())
      {
        switch (reader.TokenType)
        {
          case JsonToken.Comment:
            continue;
          case JsonToken.Integer:
            byteList.Add(Convert.ToByte(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            continue;
          case JsonToken.EndArray:
            return byteList.ToArray();
          default:
            throw JsonSerializationException.Create(reader, "Unexpected token when reading bytes: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        }
      }
      throw JsonSerializationException.Create(reader, "Unexpected end when reading bytes.");
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType.AssignableToTypeName("System.Data.Linq.Binary") || objectType == typeof (SqlBinary) || objectType == typeof (SqlBinary?);
    }
  }
}
