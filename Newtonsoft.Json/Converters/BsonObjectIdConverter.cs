﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.BsonObjectIdConverter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  public class BsonObjectIdConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      BsonObjectId bsonObjectId = (BsonObjectId) value;
      if (writer is BsonWriter bsonWriter)
        bsonWriter.WriteObjectId(bsonObjectId.Value);
      else
        writer.WriteValue(bsonObjectId.Value);
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      return reader.TokenType == JsonToken.Bytes ? (object) new BsonObjectId((byte[]) reader.Value) : throw new JsonSerializationException("Expected Bytes but got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    public override bool CanConvert(Type objectType) => objectType == typeof (BsonObjectId);
  }
}
