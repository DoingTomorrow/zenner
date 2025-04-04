// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.QueryExpression
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal abstract class QueryExpression
  {
    public QueryOperator Operator { get; set; }

    public abstract bool IsMatch(JToken t);
  }
}
