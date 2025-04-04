// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.DynamicComponentCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class DynamicComponentCustomizer<TComponent> : 
    PropertyContainerCustomizer<TComponent>,
    IDynamicComponentMapper<TComponent>,
    IDynamicComponentAttributesMapper<TComponent>,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    IPropertyContainerMapper<TComponent>,
    ICollectionPropertiesContainerMapper<TComponent>,
    IPlainPropertyContainerMapper<TComponent>,
    IBasePlainPropertyContainerMapper<TComponent>,
    IMinimalPlainPropertyContainerMapper<TComponent>
    where TComponent : class
  {
    public DynamicComponentCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder,
      PropertyPath propertyPath)
      : base(explicitDeclarationsHolder, customizersHolder, propertyPath)
    {
      if (propertyPath == null)
        throw new ArgumentNullException(nameof (propertyPath));
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      explicitDeclarationsHolder.AddAsDynamicComponent(propertyPath.LocalMember, typeof (TComponent));
    }

    public void Access(Accessor accessor)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<IDynamicComponentAttributesMapper>) (m => m.Access(accessor)));
    }

    public void Access(Type accessorType)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<IDynamicComponentAttributesMapper>) (m => m.Access(accessorType)));
    }

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<IDynamicComponentAttributesMapper>) (m => m.OptimisticLock(takeInConsiderationForOptimisticLock)));
    }

    public void Update(bool consideredInUpdateQuery)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<IDynamicComponentAttributesMapper>) (m => m.Update(consideredInUpdateQuery)));
    }

    public void Insert(bool consideredInInsertQuery)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<IDynamicComponentAttributesMapper>) (m => m.Insert(consideredInInsertQuery)));
    }

    public void Unique(bool unique)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<IDynamicComponentAttributesMapper>) (m => m.Unique(unique)));
    }
  }
}
