﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonReaderException
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Newtonsoft.Json
{
  [Serializable]
  public class JsonReaderException : JsonException
  {
    public int LineNumber { get; private set; }

    public int LinePosition { get; private set; }

    public string Path { get; private set; }

    public JsonReaderException()
    {
    }

    public JsonReaderException(string message)
      : base(message)
    {
    }

    public JsonReaderException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public JsonReaderException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal JsonReaderException(
      string message,
      Exception innerException,
      string path,
      int lineNumber,
      int linePosition)
      : base(message, innerException)
    {
      this.Path = path;
      this.LineNumber = lineNumber;
      this.LinePosition = linePosition;
    }

    internal static JsonReaderException Create(JsonReader reader, string message)
    {
      return JsonReaderException.Create(reader, message, (Exception) null);
    }

    internal static JsonReaderException Create(JsonReader reader, string message, Exception ex)
    {
      return JsonReaderException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
    }

    internal static JsonReaderException Create(
      IJsonLineInfo lineInfo,
      string path,
      string message,
      Exception ex)
    {
      message = JsonPosition.FormatMessage(lineInfo, path, message);
      int lineNumber;
      int linePosition;
      if (lineInfo != null && lineInfo.HasLineInfo())
      {
        lineNumber = lineInfo.LineNumber;
        linePosition = lineInfo.LinePosition;
      }
      else
      {
        lineNumber = 0;
        linePosition = 0;
      }
      return new JsonReaderException(message, ex, path, lineNumber, linePosition);
    }
  }
}
