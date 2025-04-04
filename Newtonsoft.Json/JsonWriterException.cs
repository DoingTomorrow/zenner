// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonWriterException
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Newtonsoft.Json
{
  [Serializable]
  public class JsonWriterException : JsonException
  {
    public string Path { get; private set; }

    public JsonWriterException()
    {
    }

    public JsonWriterException(string message)
      : base(message)
    {
    }

    public JsonWriterException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonWriterException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal JsonWriterException(string message, Exception innerException, string path)
      : base(message, innerException)
    {
      this.Path = path;
    }

    internal static JsonWriterException Create(JsonWriter writer, string message, Exception ex)
    {
      return JsonWriterException.Create(writer.ContainerPath, message, ex);
    }

    internal static JsonWriterException Create(string path, string message, Exception ex)
    {
      message = JsonPosition.FormatMessage((IJsonLineInfo) null, path, message);
      return new JsonWriterException(message, ex, path);
    }
  }
}
