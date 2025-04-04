// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.ProxyConvention
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public class ProxyConvention : 
    IClassConvention,
    IConvention<IClassInspector, IClassInstance>,
    ISubclassConvention,
    IConvention<ISubclassInspector, ISubclassInstance>,
    IHasOneConvention,
    IConvention<IOneToOneInspector, IOneToOneInstance>,
    IReferenceConvention,
    IConvention<IManyToOneInspector, IManyToOneInstance>,
    ICollectionConvention,
    IConvention<ICollectionInspector, ICollectionInstance>,
    IConvention
  {
    private readonly Func<Type, Type> _mapPersistentTypeToProxyInterfaceType;
    private readonly Func<Type, Type> _mapProxyInterfaceTypeToPersistentType;

    public ProxyConvention(
      Func<Type, Type> mapPersistentTypeToProxyInterfaceType,
      Func<Type, Type> mapProxyInterfaceTypeToPersistentType)
    {
      if (mapPersistentTypeToProxyInterfaceType == null)
        throw new ArgumentNullException(nameof (mapPersistentTypeToProxyInterfaceType));
      if (mapProxyInterfaceTypeToPersistentType == null)
        throw new ArgumentNullException(nameof (mapProxyInterfaceTypeToPersistentType));
      this._mapPersistentTypeToProxyInterfaceType = mapPersistentTypeToProxyInterfaceType;
      this._mapProxyInterfaceTypeToPersistentType = mapProxyInterfaceTypeToPersistentType;
    }

    public void Apply(IClassInstance instance)
    {
      Type proxyType = this.GetProxyType(instance.EntityType);
      if (proxyType == null)
        return;
      instance.Proxy(proxyType);
    }

    public void Apply(ISubclassInstance instance)
    {
      Type proxyType = this.GetProxyType(instance.EntityType);
      if (proxyType == null)
        return;
      instance.Proxy(proxyType);
    }

    public void Apply(IManyToOneInstance instance)
    {
      Type type = this._mapProxyInterfaceTypeToPersistentType(instance.Class.GetUnderlyingSystemType());
      if (type == null)
        return;
      instance.OverrideInferredClass(type);
    }

    public void Apply(ICollectionInstance instance)
    {
      Type persistentType = this.GetPersistentType(instance.Relationship.Class.GetUnderlyingSystemType());
      if (persistentType == null)
        return;
      instance.Relationship.CustomClass(persistentType);
    }

    public void Apply(IOneToOneInstance instance)
    {
      Type type = this._mapProxyInterfaceTypeToPersistentType(instance.Class.GetUnderlyingSystemType());
      if (type == null)
        return;
      instance.OverrideInferredClass(type);
    }

    private Type GetProxyType(Type persistentType)
    {
      return persistentType.IsAbstract ? (Type) null : this._mapPersistentTypeToProxyInterfaceType(persistentType);
    }

    private Type GetPersistentType(Type proxyType)
    {
      return !proxyType.IsInterface ? (Type) null : this._mapProxyInterfaceTypeToPersistentType(proxyType);
    }
  }
}
