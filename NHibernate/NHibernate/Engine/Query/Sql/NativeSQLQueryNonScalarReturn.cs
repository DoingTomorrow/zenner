// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.Sql.NativeSQLQueryNonScalarReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query.Sql
{
  [Serializable]
  public abstract class NativeSQLQueryNonScalarReturn : INativeSQLQueryReturn
  {
    private readonly string alias;
    private readonly Dictionary<string, string[]> propertyResults = new Dictionary<string, string[]>();
    private readonly LockMode lockMode;

    protected internal NativeSQLQueryNonScalarReturn(
      string alias,
      IDictionary<string, string[]> propertyResults,
      LockMode lockMode)
    {
      this.alias = !string.IsNullOrEmpty(alias) ? alias : throw new ArgumentNullException(nameof (alias), "A valid scalar alias must be specified.");
      this.lockMode = lockMode;
      if (propertyResults == null)
        return;
      ArrayHelper.AddAll<string, string[]>((IDictionary<string, string[]>) this.propertyResults, propertyResults);
    }

    public string Alias => this.alias;

    public LockMode LockMode => this.lockMode;

    public IDictionary<string, string[]> PropertyResultsMap
    {
      get => (IDictionary<string, string[]>) this.propertyResults;
    }

    public override bool Equals(object obj) => this.Equals(obj as NativeSQLQueryNonScalarReturn);

    public bool Equals(NativeSQLQueryNonScalarReturn other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.alias, (object) this.alias);
    }

    public override int GetHashCode() => this.alias.GetHashCode();
  }
}
