// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ArrayMultipleIndexFilter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Collections.Generic;

#nullable disable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ArrayMultipleIndexFilter : PathFilter
  {
    public List<int> Indexes { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken t in current)
      {
        foreach (int index in this.Indexes)
        {
          JToken tokenIndex = PathFilter.GetTokenIndex(t, errorWhenNoMatch, index);
          if (tokenIndex != null)
            yield return tokenIndex;
        }
      }
    }
  }
}
