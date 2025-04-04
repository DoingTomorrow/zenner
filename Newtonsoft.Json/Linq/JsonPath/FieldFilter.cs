// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.FieldFilter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class FieldFilter : PathFilter
  {
    public string Name { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken t in current)
      {
        if (t is JObject o)
        {
          if (this.Name != null)
          {
            JToken jtoken = o[this.Name];
            if (jtoken != null)
              yield return jtoken;
            else if (errorWhenNoMatch)
              throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.Name));
          }
          else
          {
            foreach (KeyValuePair<string, JToken> keyValuePair in o)
              yield return keyValuePair.Value;
          }
        }
        else if (errorWhenNoMatch)
          throw new JsonException("Property '{0}' not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) (this.Name ?? "*"), (object) t.GetType().Name));
        o = (JObject) null;
      }
    }
  }
}
