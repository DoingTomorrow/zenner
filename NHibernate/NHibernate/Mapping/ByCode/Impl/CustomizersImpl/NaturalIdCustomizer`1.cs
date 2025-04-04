// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.NaturalIdCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class NaturalIdCustomizer<TEntity>(
    IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
    ICustomizersHolder customizersHolder) : 
    PropertyContainerCustomizer<TEntity>(explicitDeclarationsHolder, customizersHolder, (PropertyPath) null),
    IBasePlainPropertyContainerMapper<TEntity>,
    IMinimalPlainPropertyContainerMapper<TEntity>
    where TEntity : class
  {
    protected override void RegisterPropertyMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IPropertyMapper> mapping)
    {
      MemberInfo member1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property);
      MemberInfo member2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TProperty>(property);
      this.ExplicitDeclarationsHolder.AddAsNaturalId(member1);
      this.ExplicitDeclarationsHolder.AddAsNaturalId(member2);
      base.RegisterPropertyMapping<TProperty>(property, mapping);
    }

    protected override void RegisterNoVisiblePropertyMapping(
      string notVisiblePropertyOrFieldName,
      Action<IPropertyMapper> mapping)
    {
      MemberInfo fieldMatchingName = typeof (TEntity).GetPropertyOrFieldMatchingName(notVisiblePropertyOrFieldName);
      MemberInfo fromReflectedType = fieldMatchingName.GetMemberFromReflectedType(typeof (TEntity));
      this.ExplicitDeclarationsHolder.AddAsNaturalId(fieldMatchingName);
      this.ExplicitDeclarationsHolder.AddAsNaturalId(fromReflectedType);
      base.RegisterNoVisiblePropertyMapping(notVisiblePropertyOrFieldName, mapping);
    }

    protected override void RegisterComponentMapping<TComponent>(
      Expression<Func<TEntity, TComponent>> property,
      Action<IComponentMapper<TComponent>> mapping)
    {
      MemberInfo member1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, TComponent>(property);
      MemberInfo member2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TComponent>(property);
      this.ExplicitDeclarationsHolder.AddAsNaturalId(member1);
      this.ExplicitDeclarationsHolder.AddAsNaturalId(member2);
      base.RegisterComponentMapping<TComponent>(property, mapping);
    }

    protected override void RegisterAnyMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Type idTypeOfMetaType,
      Action<IAnyMapper> mapping)
    {
      MemberInfo member1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property);
      MemberInfo member2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TProperty>(property);
      this.ExplicitDeclarationsHolder.AddAsNaturalId(member1);
      this.ExplicitDeclarationsHolder.AddAsNaturalId(member2);
      base.RegisterAnyMapping<TProperty>(property, idTypeOfMetaType, mapping);
    }

    protected override void RegisterManyToOneMapping<TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      Action<IManyToOneMapper> mapping)
    {
      MemberInfo member1 = TypeExtensions.DecodeMemberAccessExpression<TEntity, TProperty>(property);
      MemberInfo member2 = TypeExtensions.DecodeMemberAccessExpressionOf<TEntity, TProperty>(property);
      this.ExplicitDeclarationsHolder.AddAsNaturalId(member1);
      this.ExplicitDeclarationsHolder.AddAsNaturalId(member2);
      base.RegisterManyToOneMapping<TProperty>(property, mapping);
    }
  }
}
