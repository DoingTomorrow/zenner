// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Map
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
  public class Map(PersistentClass owner) : IndexedCollection(owner)
  {
    public override bool IsMap => true;

    public override CollectionType CollectionType
    {
      get
      {
        if (!this.IsGeneric || !this.IsSorted)
          return base.CollectionType;
        this.CheckGenericArgumentsLength(2);
        if (this.TypeName.Contains("sorted-list"))
          return TypeFactory.GenericSortedList(this.Role, this.ReferencedPropertyName, this.Comparer, this.GenericArguments[0], this.GenericArguments[1]);
        if (this.TypeName.Contains("sorted-dictionary"))
          return TypeFactory.GenericSortedDictionary(this.Role, this.ReferencedPropertyName, this.Comparer, this.GenericArguments[0], this.GenericArguments[1]);
        throw new MappingException("Use collection-type='sorted-list/sorted-dictionary' to choose implementation for generic map");
      }
    }

    public override CollectionType DefaultCollectionType
    {
      get
      {
        if (this.IsGeneric)
        {
          if (this.HasOrder)
            throw new MappingException("Cannot use order-by with generic map, no appropriate collection implementation is available");
          if (this.IsSorted)
            throw new AssertionFailure("Error in NH: should not get here (Mapping.Map.DefaultCollectionType)");
          this.CheckGenericArgumentsLength(2);
          return TypeFactory.GenericMap(this.Role, this.ReferencedPropertyName, this.GenericArguments[0], this.GenericArguments[1]);
        }
        if (this.IsSorted)
          return TypeFactory.SortedMap(this.Role, this.ReferencedPropertyName, this.Embedded, (IComparer) this.Comparer);
        return this.HasOrder ? TypeFactory.OrderedMap(this.Role, this.ReferencedPropertyName, this.Embedded) : TypeFactory.Map(this.Role, this.ReferencedPropertyName, this.Embedded);
      }
    }

    public override void CreateAllKeys()
    {
      base.CreateAllKeys();
      if (this.IsInverse)
        return;
      this.Index.CreateForeignKey();
    }
  }
}
