// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.List
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class List(PersistentClass owner) : IndexedCollection(owner)
  {
    private int baseIndex;

    public override CollectionType DefaultCollectionType
    {
      get
      {
        if (!this.IsGeneric)
          return TypeFactory.List(this.Role, this.ReferencedPropertyName, this.Embedded);
        this.CheckGenericArgumentsLength(1);
        return TypeFactory.GenericList(this.Role, this.ReferencedPropertyName, this.GenericArguments[0]);
      }
    }

    public int BaseIndex
    {
      get => this.baseIndex;
      set => this.baseIndex = value;
    }

    public override bool IsList => true;
  }
}
