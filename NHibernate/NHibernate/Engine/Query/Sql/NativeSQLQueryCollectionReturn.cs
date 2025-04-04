// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.Sql.NativeSQLQueryCollectionReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query.Sql
{
  [Serializable]
  public class NativeSQLQueryCollectionReturn : NativeSQLQueryNonScalarReturn
  {
    private readonly string ownerEntityName;
    private readonly string ownerProperty;

    public NativeSQLQueryCollectionReturn(
      string alias,
      string ownerEntityName,
      string ownerProperty,
      IDictionary<string, string[]> propertyResults,
      LockMode lockMode)
      : base(alias, propertyResults, lockMode)
    {
      this.ownerEntityName = ownerEntityName;
      this.ownerProperty = ownerProperty;
    }

    public string OwnerEntityName => this.ownerEntityName;

    public string OwnerProperty => this.ownerProperty;
  }
}
