// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.MapKeyComponentCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class MapKeyComponentCustomizer<TKey> : IComponentMapKeyMapper<TKey>
  {
    private readonly ICustomizersHolder customizersHolder;
    private readonly PropertyPath propertyPath;

    public MapKeyComponentCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
    {
      this.propertyPath = propertyPath;
      this.customizersHolder = customizersHolder;
    }

    public void Property<TProperty>(
      Expression<Func<TKey, TProperty>> property,
      Action<IPropertyMapper> mapping)
    {
      this.customizersHolder.AddCustomizer(new PropertyPath(this.propertyPath, TypeExtensions.DecodeMemberAccessExpression<TKey, TProperty>(property)), mapping);
    }

    public void Property<TProperty>(Expression<Func<TKey, TProperty>> property)
    {
      this.Property<TProperty>(property, (Action<IPropertyMapper>) (x => { }));
    }

    public void ManyToOne<TProperty>(
      Expression<Func<TKey, TProperty>> property,
      Action<IManyToOneMapper> mapping)
      where TProperty : class
    {
      this.customizersHolder.AddCustomizer(new PropertyPath(this.propertyPath, TypeExtensions.DecodeMemberAccessExpression<TKey, TProperty>(property)), mapping);
    }
  }
}
