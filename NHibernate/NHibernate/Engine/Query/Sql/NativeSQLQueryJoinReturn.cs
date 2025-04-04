// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.Sql.NativeSQLQueryJoinReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query.Sql
{
  [Serializable]
  public class NativeSQLQueryJoinReturn : NativeSQLQueryNonScalarReturn
  {
    private readonly string ownerAlias;
    private readonly string ownerProperty;

    public NativeSQLQueryJoinReturn(
      string alias,
      string ownerAlias,
      string ownerProperty,
      IDictionary<string, string[]> propertyResults,
      LockMode lockMode)
      : base(alias, propertyResults, lockMode)
    {
      this.ownerAlias = ownerAlias;
      this.ownerProperty = ownerProperty;
    }

    public string OwnerAlias => this.ownerAlias;

    public string OwnerProperty => this.ownerProperty;
  }
}
