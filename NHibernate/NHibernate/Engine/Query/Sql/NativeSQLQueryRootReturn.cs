// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.Sql.NativeSQLQueryRootReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query.Sql
{
  [Serializable]
  public class NativeSQLQueryRootReturn : NativeSQLQueryNonScalarReturn
  {
    private readonly string returnEntityName;

    public NativeSQLQueryRootReturn(string alias, string entityName, LockMode lockMode)
      : this(alias, entityName, (IDictionary<string, string[]>) null, lockMode)
    {
    }

    public NativeSQLQueryRootReturn(
      string alias,
      string entityName,
      IDictionary<string, string[]> propertyResults,
      LockMode lockMode)
      : base(alias, propertyResults, lockMode)
    {
      this.returnEntityName = entityName;
    }

    public string ReturnEntityName => this.returnEntityName;
  }
}
