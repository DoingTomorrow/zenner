// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.ComponentCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class ComponentCustomizer<TComponent> : 
    PropertyContainerCustomizer<TComponent>,
    IComponentMapper<TComponent>,
    IComponentAttributesMapper<TComponent>,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    IPropertyContainerMapper<TComponent>,
    ICollectionPropertiesContainerMapper<TComponent>,
    IPlainPropertyContainerMapper<TComponent>,
    IBasePlainPropertyContainerMapper<TComponent>,
    IMinimalPlainPropertyContainerMapper<TComponent>,
    IConformistHoldersProvider
    where TComponent : class
  {
    public ComponentCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder)
      : base(explicitDeclarationsHolder, customizersHolder, (PropertyPath) null)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsComponent(typeof (TComponent));
    }

    public ComponentCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder,
      PropertyPath propertyPath)
      : base(explicitDeclarationsHolder, customizersHolder, propertyPath)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsComponent(typeof (TComponent));
      if (propertyPath == null)
        return;
      explicitDeclarationsHolder.AddAsPersistentMember(propertyPath.LocalMember);
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
      this.AddCustomizer((Action<IComponentAttributesMapper>) (m => m.Parent(member, parentMapping)));
    }

    public void Update(bool consideredInUpdateQuery)
    {
      this.AddCustomizer((Action<IComponentAttributesMapper>) (m => m.Update(consideredInUpdateQuery)));
    }

    public void Insert(bool consideredInInsertQuery)
    {
      this.AddCustomizer((Action<IComponentAttributesMapper>) (m => m.Insert(consideredInInsertQuery)));
    }

    public void Lazy(bool isLazy)
    {
      this.AddCustomizer((Action<IComponentAttributesMapper>) (m => m.Lazy(isLazy)));
    }

    public void Unique(bool unique)
    {
      this.AddCustomizer((Action<IComponentAttributesMapper>) (m => m.Unique(unique)));
    }

    public void Class<TConcrete>() where TConcrete : TComponent
    {
      this.AddCustomizer((Action<IComponentAttributesMapper>) (m => m.Class(typeof (TConcrete))));
    }

    public void Access(Accessor accessor)
    {
      this.AddCustomizer((Action<IComponentAttributesMapper>) (m => m.Access(accessor)));
    }

    public void Access(Type accessorType)
    {
      this.AddCustomizer((Action<IComponentAttributesMapper>) (m => m.Access(accessorType)));
    }

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this.AddCustomizer((Action<IComponentAttributesMapper>) (m => m.OptimisticLock(takeInConsiderationForOptimisticLock)));
    }

    private void AddCustomizer(Action<IComponentAttributesMapper> classCustomizer)
    {
      if (this.PropertyPath == null)
        this.CustomizersHolder.AddCustomizer(typeof (TComponent), classCustomizer);
      else
        this.CustomizersHolder.AddCustomizer(this.PropertyPath, classCustomizer);
    }

    ICustomizersHolder IConformistHoldersProvider.CustomizersHolder => this.CustomizersHolder;

    IModelExplicitDeclarationsHolder IConformistHoldersProvider.ExplicitDeclarationsHolder
    {
      get => this.ExplicitDeclarationsHolder;
    }
  }
}
