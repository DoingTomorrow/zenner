// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Schema;
using System;

#nullable disable
namespace Newtonsoft.Json
{
  public abstract class JsonConverter
  {
    public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

    public abstract object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer);

    public abstract bool CanConvert(Type objectType);

    [Obsolete("JSON Schema validation has been moved to its own package. It is strongly recommended that you do not override GetSchema() in your own converter. It is not used by Json.NET and will be removed at some point in the future. Converter's that override GetSchema() will stop working when it is removed.")]
    public virtual JsonSchema GetSchema() => (JsonSchema) null;

    public virtual bool CanRead => true;

    public virtual bool CanWrite => true;
  }
}
