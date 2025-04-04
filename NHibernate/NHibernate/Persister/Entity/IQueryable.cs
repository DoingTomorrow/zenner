// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.IQueryable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public interface IQueryable : 
    ILoadable,
    IEntityPersister,
    IOptimisticCacheSource,
    IPropertyMapping,
    IJoinable
  {
    bool IsExplicitPolymorphism { get; }

    string MappedSuperclass { get; }

    string DiscriminatorSQLValue { get; }

    object DiscriminatorValue { get; }

    bool IsMultiTable { get; }

    string[] ConstraintOrderedTableNameClosure { get; }

    string[][] ContraintOrderedTableKeyColumnClosure { get; }

    string TemporaryIdTableName { get; }

    string TemporaryIdTableDDL { get; }

    bool VersionPropertyInsertable { get; }

    string IdentifierSelectFragment(string name, string suffix);

    string PropertySelectFragment(string alias, string suffix, bool allProperties);

    int GetSubclassPropertyTableNumber(string propertyPath);

    Declarer GetSubclassPropertyDeclarer(string propertyPath);

    string GetSubclassTableName(int number);

    string GenerateFilterConditionAlias(string rootAlias);
  }
}
