﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonPosition
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace Newtonsoft.Json
{
  internal struct JsonPosition(JsonContainerType type)
  {
    private static readonly char[] SpecialCharacters = new char[6]
    {
      '.',
      ' ',
      '[',
      ']',
      '(',
      ')'
    };
    internal JsonContainerType Type = type;
    internal int Position = -1;
    internal string PropertyName = (string) null;
    internal bool HasIndex = JsonPosition.TypeHasIndex(type);

    internal int CalculateLength()
    {
      switch (this.Type)
      {
        case JsonContainerType.Object:
          return this.PropertyName.Length + 5;
        case JsonContainerType.Array:
        case JsonContainerType.Constructor:
          return MathUtils.IntLength((ulong) this.Position) + 2;
        default:
          throw new ArgumentOutOfRangeException("Type");
      }
    }

    internal void WriteTo(StringBuilder sb)
    {
      switch (this.Type)
      {
        case JsonContainerType.Object:
          string propertyName = this.PropertyName;
          if (propertyName.IndexOfAny(JsonPosition.SpecialCharacters) != -1)
          {
            sb.Append("['");
            sb.Append(propertyName);
            sb.Append("']");
            break;
          }
          if (sb.Length > 0)
            sb.Append('.');
          sb.Append(propertyName);
          break;
        case JsonContainerType.Array:
        case JsonContainerType.Constructor:
          sb.Append('[');
          sb.Append(this.Position);
          sb.Append(']');
          break;
      }
    }

    internal static bool TypeHasIndex(JsonContainerType type)
    {
      return type == JsonContainerType.Array || type == JsonContainerType.Constructor;
    }

    internal static string BuildPath(List<JsonPosition> positions, JsonPosition? currentPosition)
    {
      int capacity = 0;
      JsonPosition jsonPosition;
      if (positions != null)
      {
        for (int index = 0; index < positions.Count; ++index)
        {
          int num = capacity;
          jsonPosition = positions[index];
          int length = jsonPosition.CalculateLength();
          capacity = num + length;
        }
      }
      if (currentPosition.HasValue)
      {
        int num = capacity;
        jsonPosition = currentPosition.GetValueOrDefault();
        int length = jsonPosition.CalculateLength();
        capacity = num + length;
      }
      StringBuilder sb = new StringBuilder(capacity);
      if (positions != null)
      {
        foreach (JsonPosition position in positions)
          position.WriteTo(sb);
      }
      if (currentPosition.HasValue)
      {
        jsonPosition = currentPosition.GetValueOrDefault();
        jsonPosition.WriteTo(sb);
      }
      return sb.ToString();
    }

    internal static string FormatMessage(IJsonLineInfo lineInfo, string path, string message)
    {
      if (!message.EndsWith(Environment.NewLine, StringComparison.Ordinal))
      {
        message = message.Trim();
        if (!message.EndsWith('.'))
          message += ".";
        message += " ";
      }
      message += "Path '{0}'".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) path);
      if (lineInfo != null && lineInfo.HasLineInfo())
        message += ", line {0}, position {1}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) lineInfo.LineNumber, (object) lineInfo.LinePosition);
      message += ".";
      return message;
    }
  }
}
