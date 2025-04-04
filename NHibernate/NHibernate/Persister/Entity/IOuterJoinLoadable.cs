// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.IOuterJoinLoadable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public interface IOuterJoinLoadable : 
    ILoadable,
    IEntityPersister,
    IOptimisticCacheSource,
    IJoinable
  {
    EntityType EntityType { get; }

    string SelectFragment(string alias, string suffix);

    int CountSubclassProperties();

    FetchMode GetFetchMode(int i);

    CascadeStyle GetCascadeStyle(int i);

    bool IsDefinedOnSubclass(int i);

    IType GetSubclassPropertyType(int i);

    string GetSubclassPropertyName(int i);

    bool IsSubclassPropertyNullable(int i);

    string[] GetSubclassPropertyColumnNames(int i);

    string GetSubclassPropertyTableName(int i);

    string[] ToColumns(string name, int i);

    string FromTableFragment(string alias);

    string[] GetPropertyColumnNames(string propertyPath);

    string GetPropertyTableName(string propertyName);

    string[] ToIdentifierColumns(string alias);

    string GenerateTableAliasForColumn(string rootAlias, string column);
  }
}
