// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ICollectionElementRelation`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface ICollectionElementRelation<TElement>
  {
    void Element();

    void Element(Action<IElementMapper> mapping);

    void OneToMany();

    void OneToMany(Action<IOneToManyMapper> mapping);

    void ManyToMany();

    void ManyToMany(Action<IManyToManyMapper> mapping);

    void Component(Action<IComponentElementMapper<TElement>> mapping);

    void ManyToAny(Type idTypeOfMetaType, Action<IManyToAnyMapper> mapping);

    void ManyToAny<TIdTypeOfMetaType>(Action<IManyToAnyMapper> mapping);
  }
}
