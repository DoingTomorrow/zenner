// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.ResultSetMappingDefinition
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine.Query.Sql;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public class ResultSetMappingDefinition
  {
    private readonly string name;
    private readonly List<INativeSQLQueryReturn> queryReturns = new List<INativeSQLQueryReturn>();

    public ResultSetMappingDefinition(string name) => this.name = name;

    public string Name => this.name;

    public void AddQueryReturn(INativeSQLQueryReturn queryReturn)
    {
      if (queryReturn == null)
        return;
      this.queryReturns.Add(queryReturn);
    }

    public INativeSQLQueryReturn[] GetQueryReturns() => this.queryReturns.ToArray();
  }
}
