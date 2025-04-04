// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.BooleanQueryExpression
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class BooleanQueryExpression : QueryExpression
  {
    public List<PathFilter> Path { get; set; }

    public JValue Value { get; set; }

    public override bool IsMatch(JToken t)
    {
      foreach (JToken jtoken in JPath.Evaluate(this.Path, t, false))
      {
        if (jtoken is JValue jvalue)
        {
          switch (this.Operator)
          {
            case QueryOperator.Equals:
              if (this.EqualsWithStringCoercion(jvalue, this.Value))
                return true;
              continue;
            case QueryOperator.NotEquals:
              if (!this.EqualsWithStringCoercion(jvalue, this.Value))
                return true;
              continue;
            case QueryOperator.Exists:
              return true;
            case QueryOperator.LessThan:
              if (jvalue.CompareTo(this.Value) < 0)
                return true;
              continue;
            case QueryOperator.LessThanOrEquals:
              if (jvalue.CompareTo(this.Value) <= 0)
                return true;
              continue;
            case QueryOperator.GreaterThan:
              if (jvalue.CompareTo(this.Value) > 0)
                return true;
              continue;
            case QueryOperator.GreaterThanOrEquals:
              if (jvalue.CompareTo(this.Value) >= 0)
                return true;
              continue;
            default:
              continue;
          }
        }
        else
        {
          switch (this.Operator)
          {
            case QueryOperator.NotEquals:
            case QueryOperator.Exists:
              return true;
            default:
              continue;
          }
        }
      }
      return false;
    }

    private bool EqualsWithStringCoercion(JValue value, JValue queryValue)
    {
      if (value.Equals(queryValue))
        return true;
      if (queryValue.Type != JTokenType.String)
        return false;
      string b = (string) queryValue.Value;
      string a;
      switch (value.Type)
      {
        case JTokenType.Date:
          using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
          {
            if (value.Value is DateTimeOffset)
              DateTimeUtils.WriteDateTimeOffsetString((TextWriter) stringWriter, (DateTimeOffset) value.Value, DateFormatHandling.IsoDateFormat, (string) null, CultureInfo.InvariantCulture);
            else
              DateTimeUtils.WriteDateTimeString((TextWriter) stringWriter, (DateTime) value.Value, DateFormatHandling.IsoDateFormat, (string) null, CultureInfo.InvariantCulture);
            a = stringWriter.ToString();
            break;
          }
        case JTokenType.Bytes:
          a = Convert.ToBase64String((byte[]) value.Value);
          break;
        case JTokenType.Guid:
        case JTokenType.TimeSpan:
          a = value.Value.ToString();
          break;
        case JTokenType.Uri:
          a = ((Uri) value.Value).OriginalString;
          break;
        default:
          return false;
      }
      return string.Equals(a, b, StringComparison.Ordinal);
    }
  }
}
