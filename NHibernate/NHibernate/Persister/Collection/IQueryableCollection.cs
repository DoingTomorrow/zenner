// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Collection.IQueryableCollection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;

#nullable disable
namespace NHibernate.Persister.Collection
{
  public interface IQueryableCollection : IPropertyMapping, IJoinable, ICollectionPersister
  {
    string[] IndexFormulas { get; }

    IEntityPersister ElementPersister { get; }

    FetchMode FetchMode { get; }

    string[] IndexColumnNames { get; }

    string[] ElementColumnNames { get; }

    bool HasWhere { get; }

    string SelectFragment(string alias, string columnSuffix);

    string[] GetIndexColumnNames(string alias);

    string[] GetElementColumnNames(string alias);

    string GetSQLWhereString(string alias);

    string GetSQLOrderByString(string alias);

    string GetManyToManyOrderByString(string alias);
  }
}
