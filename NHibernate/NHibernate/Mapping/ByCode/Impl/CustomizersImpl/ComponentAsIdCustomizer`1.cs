// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CustomizersImpl.ComponentAsIdCustomizer`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl.CustomizersImpl
{
  public class ComponentAsIdCustomizer<TComponent> : 
    PropertyContainerCustomizer<TComponent>,
    IComponentAsIdMapper<TComponent>,
    IComponentAsIdAttributesMapper<TComponent>,
    IAccessorPropertyMapper,
    IMinimalPlainPropertyContainerMapper<TComponent>
    where TComponent : class
  {
    public ComponentAsIdCustomizer(
      IModelExplicitDeclarationsHolder explicitDeclarationsHolder,
      ICustomizersHolder customizersHolder,
      PropertyPath propertyPath)
      : base(explicitDeclarationsHolder, customizersHolder, propertyPath)
    {
      if (explicitDeclarationsHolder == null)
        throw new ArgumentNullException(nameof (explicitDeclarationsHolder));
      if (propertyPath == null)
        throw new ArgumentNullException(nameof (propertyPath));
      explicitDeclarationsHolder.AddAsComponent(typeof (TComponent));
      explicitDeclarationsHolder.AddAsPoid(propertyPath.LocalMember);
    }

    public void Class<TConcrete>() where TConcrete : TComponent
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<IComponentAsIdAttributesMapper>) (m => m.Class(typeof (TConcrete))));
    }

    public void Access(Accessor accessor)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<IComponentAsIdAttributesMapper>) (m => m.Access(accessor)));
    }

    public void Access(Type accessorType)
    {
      this.CustomizersHolder.AddCustomizer(this.PropertyPath, (Action<IComponentAsIdAttributesMapper>) (m => m.Access(accessorType)));
    }
  }
}
