// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.ComponentElementCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class ComponentElementCustomizer<TComponent> : 
    IComponentElementMapper<TComponent>,
    IComponentAttributesMapper<TComponent>,
    IEntityPropertyMapper,
    IAccessorPropertyMapper
  {
    private readonly ICustomizersHolder _customizersHolder;
    private readonly IModelExplicitDeclarationsHolder _explicitDeclarationsHolder;
    private readonly PropertyPath _propertyPath;

    public ComponentElementCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      PropertyPath propertyPath,
      ICustomizersHolder customizersHolder)
    {
      this._explicitDeclarationsHolder = explicitDeclarationsHolder != null ? explicitDeclarationsHolder : throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      this._propertyPath = propertyPath;
      this._customizersHolder = customizersHolder;
      this._explicitDeclarationsHolder.AddAsComponent(typeof (TComponent));
      if (propertyPath == null)
        return;
      this._explicitDeclarationsHolder.AddAsPersistentMember(propertyPath.LocalMember);
    }

    public void Parent<TProperty>(Expression<Func<TComponent, TProperty>> parent) where TProperty : class
    {
      this.Parent<TProperty>(parent, (Action<IComponentParentMapper>) (x => { }));
    }

    public void Parent<TProperty>(
      Expression<Func<TComponent, TProperty>> parent,
      Action<IComponentParentMapper> parentMapping)
      where TProperty : class
    {
      MemberInfo member = TypeExtensions.DecodeMemberAccessExpression<TComponent, TProperty>(parent);
      this._customizersHolder.AddCustomizer(typeof (TComponent), (Action<IComponentAttributesMapper>) (x => x.Parent(member, parentMapping)));
      this._explicitDeclarationsHolder.AddAsPersistentMember(member);
    }

    public void Update(bool consideredInUpdateQuery)
    {
      this._customizersHolder.AddCustomizer(typeof (TComponent), (Action<IComponentAttributesMapper>) (x => x.Update(consideredInUpdateQuery)));
    }

    public void Insert(bool consideredInInsertQuery)
    {
      this._customizersHolder.AddCustomizer(typeof (TComponent), (Action<IComponentAttributesMapper>) (x => x.Insert(consideredInInsertQuery)));
    }

    public void Lazy(bool isLazy)
    {
      this._customizersHolder.AddCustomizer(typeof (TComponent), (Action<IComponentAttributesMapper>) (x => x.Lazy(isLazy)));
    }

    public void Unique(bool unique)
    {
      this._customizersHolder.AddCustomizer(typeof (TComponent), (Action<IComponentAttributesMapper>) (x => x.Unique(unique)));
    }

    public void Class<TConcrete>() where TConcrete : TComponent
    {
      this._customizersHolder.AddCustomizer(typeof (TComponent), (Action<IComponentAttributesMapper>) (x => x.Class(typeof (TConcrete))));
    }

    public void Property<TProperty>(
      Expression<Func<TComponent, TProperty>> property,
      Action<IPropertyMapper> mapping)
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TComponent, TProperty>(property);
      this._customizersHolder.AddCustomizer(new PropertyPath(this._propertyPath, memberInfo1), mapping);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TComponent, TProperty>(property);
      this._customizersHolder.AddCustomizer(new PropertyPath(this._propertyPath, memberInfo2), mapping);
      this._explicitDeclarationsHolder.AddAsProperty(memberInfo1);
      this._explicitDeclarationsHolder.AddAsProperty(memberInfo2);
    }

    public void Property<TProperty>(Expression<Func<TComponent, TProperty>> property)
    {
      this.Property<TProperty>(property, (Action<IPropertyMapper>) (x => { }));
    }

    public void Component<TNestedComponent>(
      Expression<Func<TComponent, TNestedComponent>> property,
      Action<IComponentElementMapper<TNestedComponent>> mapping)
      where TNestedComponent : class
    {
      MemberInfo localMember1 = TypeExtensions.DecodeMemberAccessExpression<TComponent, TNestedComponent>(property);
      mapping((IComponentElementMapper<TNestedComponent>) new ComponentElementCustomizer<TNestedComponent>(this._explicitDeclarationsHolder, new PropertyPath(this._propertyPath, localMember1), this._customizersHolder));
      MemberInfo localMember2 = TypeExtensions.DecodeMemberAccessExpressionOf<TComponent, TNestedComponent>(property);
      mapping((IComponentElementMapper<TNestedComponent>) new ComponentElementCustomizer<TNestedComponent>(this._explicitDeclarationsHolder, new PropertyPath(this._propertyPath, localMember2), this._customizersHolder));
    }

    public void ManyToOne<TProperty>(
      Expression<Func<TComponent, TProperty>> property,
      Action<IManyToOneMapper> mapping)
      where TProperty : class
    {
      MemberInfo memberInfo1 = TypeExtensions.DecodeMemberAccessExpression<TComponent, TProperty>(property);
      this._customizersHolder.AddCustomizer(new PropertyPath(this._propertyPath, memberInfo1), mapping);
      MemberInfo memberInfo2 = TypeExtensions.DecodeMemberAccessExpressionOf<TComponent, TProperty>(property);
      this._customizersHolder.AddCustomizer(new PropertyPath(this._propertyPath, memberInfo2), mapping);
      this._explicitDeclarationsHolder.AddAsManyToOneRelation(memberInfo1);
      this._explicitDeclarationsHolder.AddAsManyToOneRelation(memberInfo2);
    }

    public void ManyToOne<TProperty>(Expression<Func<TComponent, TProperty>> property) where TProperty : class
    {
      this.ManyToOne<TProperty>(property, (Action<IManyToOneMapper>) (x => { }));
    }

    public void Access(Accessor accessor)
    {
      this._customizersHolder.AddCustomizer(typeof (TComponent), (Action<IComponentAttributesMapper>) (x => x.Access(accessor)));
    }

    public void Access(Type accessorType)
    {
      this._customizersHolder.AddCustomizer(typeof (TComponent), (Action<IComponentAttributesMapper>) (x => x.Access(accessorType)));
    }

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this._customizersHolder.AddCustomizer(typeof (TComponent), (Action<IComponentAttributesMapper>) (x => x.OptimisticLock(takeInConsiderationForOptimisticLock)));
    }
  }
}
