// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JTokenEqualityComparer
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Collections.Generic;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public class JTokenEqualityComparer : IEqualityComparer<JToken>
  {
    public bool Equals(JToken x, JToken y) => JToken.DeepEquals(x, y);

    public int GetHashCode(JToken obj) => obj == null ? 0 : obj.GetDeepHashCode();
  }
}
