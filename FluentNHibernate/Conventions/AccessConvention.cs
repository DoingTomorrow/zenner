// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AccessConvention
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public abstract class AccessConvention : 
    IIdConvention,
    IConvention<IIdentityInspector, IIdentityInstance>,
    ICompositeIdentityConvention,
    IConvention<ICompositeIdentityInspector, ICompositeIdentityInstance>,
    IKeyPropertyConvention,
    IConvention<IKeyPropertyInspector, IKeyPropertyInstance>,
    IKeyManyToOneConvention,
    IConvention<IKeyManyToOneInspector, IKeyManyToOneInstance>,
    IVersionConvention,
    IConvention<IVersionInspector, IVersionInstance>,
    IPropertyConvention,
    IConvention<IPropertyInspector, IPropertyInstance>,
    IComponentConvention,
    IConvention<IComponentInspector, IComponentInstance>,
    IDynamicComponentConvention,
    IConvention<IDynamicComponentInspector, IDynamicComponentInstance>,
    IReferenceConvention,
    IConvention<IManyToOneInspector, IManyToOneInstance>,
    IHasOneConvention,
    IConvention<IOneToOneInspector, IOneToOneInstance>,
    ICollectionConvention,
    IConvention<ICollectionInspector, ICollectionInstance>,
    IAnyConvention,
    IConvention<IAnyInspector, IAnyInstance>,
    IConvention
  {
    protected abstract void Apply(Type owner, string name, IAccessInstance access);

    public virtual void Apply(IIdentityInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(ICompositeIdentityInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(IKeyPropertyInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(IKeyManyToOneInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(IVersionInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(IPropertyInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(IComponentInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(IDynamicComponentInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(IOneToOneInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(IManyToOneInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }

    public virtual void Apply(ICollectionInstance instance)
    {
      string name = instance.Name;
      this.Apply(instance.EntityType, name, instance.Access);
    }

    public virtual void Apply(IAnyInstance instance)
    {
      this.Apply(instance.EntityType, instance.Name, instance.Access);
    }
  }
}
