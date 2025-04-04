// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ScanFilter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Collections.Generic;

#nullable disable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ScanFilter : PathFilter
  {
    public string Name { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken root in current)
      {
        if (this.Name == null)
          yield return root;
        JToken value = root;
        JToken jtoken = root;
        while (true)
        {
          if (jtoken != null && jtoken.HasValues)
          {
            value = jtoken.First;
          }
          else
          {
            while (value != null && value != root && value == value.Parent.Last)
              value = (JToken) value.Parent;
            if (value != null && value != root)
              value = value.Next;
            else
              break;
          }
          if (value is JProperty jproperty)
          {
            if (jproperty.Name == this.Name)
              yield return jproperty.Value;
          }
          else if (this.Name == null)
            yield return value;
          jtoken = (JToken) (value as JContainer);
        }
        value = (JToken) null;
      }
    }
  }
}
