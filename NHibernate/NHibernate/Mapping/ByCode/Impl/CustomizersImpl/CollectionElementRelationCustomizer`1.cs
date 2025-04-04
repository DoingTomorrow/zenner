// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.CollectionElementRelationCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class CollectionElementRelationCustomizer<TElement> : ICollectionElementRelation<TElement>
  {
    private readonly ICustomizersHolder customizersHolder;
    private readonly IModelExplicitDeclarationsHolder explicitDeclarationsHolder;
    private readonly PropertyPath propertyPath;

    public CollectionElementRelationCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
    {
      this.explicitDeclarationsHolder = explicitDeclarationsHolder != null ? explicitDeclarationsHolder : throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      this.propertyPath = propertyPath;
      this.customizersHolder = customizersHolder;
    }

    public void Element() => this.Element((Action<IElementMapper>) (x => { }));

    public void Element(Action<IElementMapper> mapping)
    {
      CollectionElementCustomizer elementCustomizer = new CollectionElementCustomizer(this.propertyPath, this.customizersHolder);
      mapping((IElementMapper) elementCustomizer);
    }

    public void OneToMany() => this.OneToMany((Action<IOneToManyMapper>) (x => { }));

    public void OneToMany(Action<IOneToManyMapper> mapping)
    {
      OneToManyCustomizer toManyCustomizer = new OneToManyCustomizer(this.explicitDeclarationsHolder, this.propertyPath, this.customizersHolder);
      mapping((IOneToManyMapper) toManyCustomizer);
    }

    public void ManyToMany() => this.ManyToMany((Action<IManyToManyMapper>) (x => { }));

    public void ManyToMany(Action<IManyToManyMapper> mapping)
    {
      ManyToManyCustomizer toManyCustomizer = new ManyToManyCustomizer(this.explicitDeclarationsHolder, this.propertyPath, this.customizersHolder);
      mapping((IManyToManyMapper) toManyCustomizer);
    }

    public void Component(Action<IComponentElementMapper<TElement>> mapping)
    {
      this.explicitDeclarationsHolder.AddAsComponent(typeof (TElement));
      ComponentElementCustomizer<TElement> elementCustomizer = new ComponentElementCustomizer<TElement>(this.explicitDeclarationsHolder, this.propertyPath, this.customizersHolder);
      mapping((IComponentElementMapper<TElement>) elementCustomizer);
    }

    public void ManyToAny(Type idTypeOfMetaType, Action<IManyToAnyMapper> mapping)
    {
      if (mapping == null)
        throw new ArgumentNullException(nameof (mapping));
      ManyToAnyCustomizer manyToAnyCustomizer = new ManyToAnyCustomizer(this.explicitDeclarationsHolder, this.propertyPath, this.customizersHolder);
      mapping((IManyToAnyMapper) manyToAnyCustomizer);
    }

    public void ManyToAny<TIdTypeOfMetaType>(Action<IManyToAnyMapper> mapping)
    {
      this.ManyToAny(typeof (TIdTypeOfMetaType), mapping);
    }
  }
}
