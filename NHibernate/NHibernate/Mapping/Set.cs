// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Set
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Set(PersistentClass owner) : Collection(owner)
  {
    public override bool IsSet => true;

    public override CollectionType DefaultCollectionType
    {
      get
      {
        if (this.IsGeneric)
        {
          this.CheckGenericArgumentsLength(1);
          if (this.IsSorted)
            return TypeFactory.GenericSortedSet(this.Role, this.ReferencedPropertyName, this.Comparer, this.GenericArguments[0]);
          return this.HasOrder ? TypeFactory.GenericOrderedSet(this.Role, this.ReferencedPropertyName, this.GenericArguments[0]) : TypeFactory.GenericSet(this.Role, this.ReferencedPropertyName, this.GenericArguments[0]);
        }
        if (this.IsSorted)
          return TypeFactory.SortedSet(this.Role, this.ReferencedPropertyName, this.Embedded, (IComparer) this.Comparer);
        return this.HasOrder ? TypeFactory.OrderedSet(this.Role, this.ReferencedPropertyName, this.Embedded) : TypeFactory.Set(this.Role, this.ReferencedPropertyName, this.Embedded);
      }
    }

    public override void CreatePrimaryKey()
    {
      if (this.IsOneToMany)
        return;
      PrimaryKey primaryKey = new PrimaryKey();
      foreach (Column column in this.Key.ColumnIterator)
        primaryKey.AddColumn(column);
      bool flag = false;
      foreach (Column column in this.Element.ColumnIterator)
      {
        if (column.IsNullable)
          flag = true;
        primaryKey.AddColumn(column);
      }
      if (flag)
        return;
      this.CollectionTable.PrimaryKey = primaryKey;
    }
  }
}
