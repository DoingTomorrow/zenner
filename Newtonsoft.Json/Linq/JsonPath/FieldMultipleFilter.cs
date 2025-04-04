// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.FieldMultipleFilter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class FieldMultipleFilter : PathFilter
  {
    public List<string> Names { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken t in current)
      {
        if (t is JObject o)
        {
          foreach (string name in this.Names)
          {
            JToken jtoken = o[name];
            if (jtoken != null)
              yield return jtoken;
            if (errorWhenNoMatch)
              throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) name));
          }
        }
        else if (errorWhenNoMatch)
          throw new JsonException("Properties {0} not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) string.Join(", ", this.Names.Select<string, string>((Func<string, string>) (n => "'" + n + "'")).ToArray<string>()), (object) t.GetType().Name));
        o = (JObject) null;
      }
    }
  }
}
