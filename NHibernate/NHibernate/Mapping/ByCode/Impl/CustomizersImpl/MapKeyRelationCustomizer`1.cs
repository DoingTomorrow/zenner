// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.MapKeyRelationCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class MapKeyRelationCustomizer<TKey> : IMapKeyRelation<TKey>
  {
    private readonly ICustomizersHolder customizersHolder;
    private readonly IModelExplicitDeclarationsHolder explicitDeclarationsHolder;
    private readonly PropertyPath propertyPath;

    public MapKeyRelationCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
    {
      this.explicitDeclarationsHolder = explicitDeclarationsHolder != null ? explicitDeclarationsHolder : throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      this.propertyPath = propertyPath;
      this.customizersHolder = customizersHolder;
    }

    public void Element() => this.Element((Action<IMapKeyMapper>) (x => { }));

    public void Element(Action<IMapKeyMapper> mapping)
    {
      MapKeyCustomizer mapKeyCustomizer = new MapKeyCustomizer(this.propertyPath, this.customizersHolder);
      mapping((IMapKeyMapper) mapKeyCustomizer);
    }

    public void ManyToMany() => this.ManyToMany((Action<IMapKeyManyToManyMapper>) (x => { }));

    public void ManyToMany(Action<IMapKeyManyToManyMapper> mapping)
    {
      MapKeyManyToManyCustomizer toManyCustomizer = new MapKeyManyToManyCustomizer(this.explicitDeclarationsHolder, this.propertyPath, this.customizersHolder);
      mapping((IMapKeyManyToManyMapper) toManyCustomizer);
    }

    public void Component(Action<IComponentMapKeyMapper<TKey>> mapping)
    {
      MapKeyComponentCustomizer<TKey> componentCustomizer = new MapKeyComponentCustomizer<TKey>(this.explicitDeclarationsHolder, this.propertyPath, this.customizersHolder);
      mapping((IComponentMapKeyMapper<TKey>) componentCustomizer);
    }
  }
}
