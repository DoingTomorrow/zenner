﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.VersionConverter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  public class VersionConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        if ((object) (value as Version) == null)
          throw new JsonSerializationException("Expected Version object value");
        writer.WriteValue(value.ToString());
      }
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      if (reader.TokenType != JsonToken.String)
        throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing version. Token: {0}, Value: {1}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType, reader.Value));
      try
      {
        return (object) new Version((string) reader.Value);
      }
      catch (Exception ex)
      {
        throw JsonSerializationException.Create(reader, "Error parsing version string: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, reader.Value), ex);
      }
    }

    public override bool CanConvert(Type objectType) => objectType == typeof (Version);
  }
}
