// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.AbstractAuxiliaryDatabaseObject
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public abstract class AbstractAuxiliaryDatabaseObject : IAuxiliaryDatabaseObject, IRelationalModel
  {
    private readonly HashedSet<string> dialectScopes;
    private IDictionary<string, string> parameters = (IDictionary<string, string>) new Dictionary<string, string>();

    protected AbstractAuxiliaryDatabaseObject() => this.dialectScopes = new HashedSet<string>();

    protected AbstractAuxiliaryDatabaseObject(HashedSet<string> dialectScopes)
    {
      this.dialectScopes = dialectScopes;
    }

    public void AddDialectScope(string dialectName) => this.dialectScopes.Add(dialectName);

    public HashedSet<string> DialectScopes => this.dialectScopes;

    public IDictionary<string, string> Parameters => this.parameters;

    public bool AppliesToDialect(NHibernate.Dialect.Dialect dialect)
    {
      return this.dialectScopes.IsEmpty || this.dialectScopes.Contains(dialect.GetType().FullName);
    }

    public abstract string SqlCreateString(
      NHibernate.Dialect.Dialect dialect,
      IMapping p,
      string defaultCatalog,
      string defaultSchema);

    public abstract string SqlDropString(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema);

    public void SetParameterValues(IDictionary<string, string> parameters)
    {
      this.parameters = parameters;
    }
  }
}
