﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.EntityKeyMemberConverter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

#nullable disable
namespace Newtonsoft.Json.Converters
{
  public class EntityKeyMemberConverter : JsonConverter
  {
    private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";
    private const string KeyPropertyName = "Key";
    private const string TypePropertyName = "Type";
    private const string ValuePropertyName = "Value";
    private static ReflectionObject _reflectionObject;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      EntityKeyMemberConverter.EnsureReflectionObject(value.GetType());
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      string str = (string) EntityKeyMemberConverter._reflectionObject.GetValue(value, "Key");
      object obj = EntityKeyMemberConverter._reflectionObject.GetValue(value, "Value");
      Type type = obj?.GetType();
      writer.WriteStartObject();
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Key") : "Key");
      writer.WriteValue(str);
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Type") : "Type");
      writer.WriteValue(type != (Type) null ? type.FullName : (string) null);
      writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Value") : "Value");
      if (type != (Type) null)
      {
        string s;
        if (JsonSerializerInternalWriter.TryConvertToString(obj, type, out s))
          writer.WriteValue(s);
        else
          writer.WriteValue(obj);
      }
      else
        writer.WriteNull();
      writer.WriteEndObject();
    }

    private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
    {
      reader.ReadAndAssert();
      if (reader.TokenType != JsonToken.PropertyName || !string.Equals(reader.Value.ToString(), propertyName, StringComparison.OrdinalIgnoreCase))
        throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName));
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      EntityKeyMemberConverter.EnsureReflectionObject(objectType);
      object target = EntityKeyMemberConverter._reflectionObject.Creator();
      EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Key");
      reader.ReadAndAssert();
      EntityKeyMemberConverter._reflectionObject.SetValue(target, "Key", (object) reader.Value.ToString());
      EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Type");
      reader.ReadAndAssert();
      Type type = Type.GetType(reader.Value.ToString());
      EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Value");
      reader.ReadAndAssert();
      EntityKeyMemberConverter._reflectionObject.SetValue(target, "Value", serializer.Deserialize(reader, type));
      reader.ReadAndAssert();
      return target;
    }

    private static void EnsureReflectionObject(Type objectType)
    {
      if (EntityKeyMemberConverter._reflectionObject != null)
        return;
      EntityKeyMemberConverter._reflectionObject = ReflectionObject.Create(objectType, "Key", "Value");
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType.AssignableToTypeName("System.Data.EntityKeyMember");
    }
  }
}
