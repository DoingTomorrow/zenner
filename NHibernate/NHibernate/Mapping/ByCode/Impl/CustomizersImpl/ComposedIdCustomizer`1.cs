// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.ComposedIdCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class ComposedIdCustomizer<TEntity>(
    IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
    ICustomizersHolder customizersHolder) : 
    PropertyContainerCustomizer<TEntity>(explicitDeclarationsHolder, customizersHolder, (PropertyPath) null),
    IComposedIdMapper<TEntity>,
    IMinimalPlainPropertyContainerMapper<TEntity>
    where TEntity : class
  {
    protected override void RegisterPropertyMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IPropertyMapper> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPartOfComposedId(TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TProperty>(property));
      base.RegisterPropertyMapping<TProperty>(property, mapping);
    }

    protected override void RegisterManyToOneMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IManyToOneMapper> mapping)
    {
      this.ExplicitDeclarationsHolder.AddAsPartOfComposedId(TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TProperty>(property));
      base.RegisterManyToOneMapping<TProperty>(property, mapping);
    }
  }
}
