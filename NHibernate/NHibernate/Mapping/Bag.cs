// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Bag
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Bag(PersistentClass owner) : Collection(owner)
  {
    public override CollectionType DefaultCollectionType
    {
      get
      {
        if (!this.IsGeneric)
          return TypeFactory.Bag(this.Role, this.ReferencedPropertyName, this.Embedded);
        this.CheckGenericArgumentsLength(1);
        return TypeFactory.GenericBag(this.Role, this.ReferencedPropertyName, this.GenericArguments[0]);
      }
    }

    public override void CreatePrimaryKey()
    {
    }
  }
}
