// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.ILoadable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Type;
using System.Data;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public interface ILoadable : IEntityPersister, IOptimisticCacheSource
  {
    IType DiscriminatorType { get; }

    string[] IdentifierColumnNames { get; }

    string DiscriminatorColumnName { get; }

    bool IsAbstract { get; }

    bool HasSubclasses { get; }

    string GetSubclassForDiscriminatorValue(object value);

    string[] GetIdentifierAliases(string suffix);

    string[] GetPropertyAliases(string suffix, int i);

    string[] GetPropertyColumnNames(int i);

    string GetDiscriminatorAlias(string suffix);

    bool HasRowId { get; }

    object[] Hydrate(
      IDataReader rs,
      object id,
      object obj,
      ILoadable rootLoadable,
      string[][] suffixedPropertyColumns,
      bool allProperties,
      ISessionImplementor session);
  }
}
