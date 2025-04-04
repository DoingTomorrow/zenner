// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Collection.CollectionPropertyMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Persister.Collection
{
  public class CollectionPropertyMapping : IPropertyMapping
  {
    private readonly IQueryableCollection memberPersister;

    public CollectionPropertyMapping(IQueryableCollection memberPersister)
    {
      this.memberPersister = memberPersister;
    }

    public IType ToType(string propertyName)
    {
      switch (propertyName)
      {
        case "elements":
          return this.memberPersister.ElementType;
        case "indices":
          return this.memberPersister.HasIndex ? this.memberPersister.IndexType : throw new QueryException("unindexed collection before indices()");
        case "size":
          return (IType) NHibernateUtil.Int32;
        case "maxIndex":
        case "minIndex":
          return this.memberPersister.IndexType;
        case "maxElement":
        case "minElement":
          return this.memberPersister.ElementType;
        default:
          throw new QueryException("illegal syntax near collection: " + propertyName);
      }
    }

    public bool TryToType(string propertyName, out IType type)
    {
      try
      {
        type = this.ToType(propertyName);
        return true;
      }
      catch (Exception ex)
      {
        type = (IType) null;
        return false;
      }
    }

    public string[] ToColumns(string alias, string propertyName)
    {
      switch (propertyName)
      {
        case "elements":
          return this.memberPersister.GetElementColumnNames(alias);
        case "indices":
          return this.memberPersister.HasIndex ? this.memberPersister.GetIndexColumnNames(alias) : throw new QueryException("unindexed collection before indices()");
        case "size":
          string[] keyColumnNames = this.memberPersister.KeyColumnNames;
          return new string[1]
          {
            "count(" + alias + (object) '.' + keyColumnNames[0] + (object) ')'
          };
        case "maxIndex":
          string[] strArray1 = this.memberPersister.HasIndex ? this.memberPersister.GetIndexColumnNames(alias) : throw new QueryException("unindexed collection in maxIndex()");
          return strArray1.Length == 1 ? new string[1]
          {
            "max(" + strArray1[0] + (object) ')'
          } : throw new QueryException("composite collection index in maxIndex()");
        case "minIndex":
          string[] strArray2 = this.memberPersister.HasIndex ? this.memberPersister.GetIndexColumnNames(alias) : throw new QueryException("unindexed collection in minIndex()");
          return strArray2.Length == 1 ? new string[1]
          {
            "min(" + strArray2[0] + (object) ')'
          } : throw new QueryException("composite collection index in minIndex()");
        case "maxElement":
          string[] elementColumnNames1 = this.memberPersister.GetElementColumnNames(alias);
          return elementColumnNames1.Length == 1 ? new string[1]
          {
            "max(" + elementColumnNames1[0] + (object) ')'
          } : throw new QueryException("composite collection element in maxElement()");
        case "minElement":
          string[] elementColumnNames2 = this.memberPersister.GetElementColumnNames(alias);
          return elementColumnNames2.Length == 1 ? new string[1]
          {
            "min(" + elementColumnNames2[0] + (object) ')'
          } : throw new QueryException("composite collection element in minElement()");
        default:
          throw new QueryException("illegal syntax near collection: " + propertyName);
      }
    }

    public string[] ToColumns(string propertyName)
    {
      throw new NotSupportedException("References to collections must be define a SQL alias");
    }

    public IType Type => (IType) this.memberPersister.CollectionType;
  }
}
