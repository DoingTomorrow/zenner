// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Entity.DynamicMapEntityTuplizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Proxy;
using NHibernate.Proxy.Map;
using NHibernate.Type;
using System;
using System.Collections;
using System.Reflection;

#nullable disable
namespace NHibernate.Tuple.Entity
{
  public class DynamicMapEntityTuplizer : AbstractEntityTuplizer
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (PocoEntityTuplizer));

    internal DynamicMapEntityTuplizer(EntityMetamodel entityMetamodel, PersistentClass mappingInfo)
      : base(entityMetamodel, mappingInfo)
    {
      this.Instantiator = this.BuildInstantiator(mappingInfo);
    }

    public override System.Type ConcreteProxyClass => typeof (IDictionary);

    public override bool IsInstrumented => false;

    public override System.Type MappedClass => typeof (IDictionary);

    public override EntityMode EntityMode => EntityMode.Map;

    protected override IGetter BuildPropertyGetter(
      NHibernate.Mapping.Property mappedProperty,
      PersistentClass mappedEntity)
    {
      return this.BuildPropertyAccessor(mappedProperty).GetGetter((System.Type) null, mappedProperty.Name);
    }

    private IPropertyAccessor BuildPropertyAccessor(NHibernate.Mapping.Property property)
    {
      return PropertyAccessorFactory.DynamicMapPropertyAccessor;
    }

    protected override ISetter BuildPropertySetter(
      NHibernate.Mapping.Property mappedProperty,
      PersistentClass mappedEntity)
    {
      return this.BuildPropertyAccessor(mappedProperty).GetSetter((System.Type) null, mappedProperty.Name);
    }

    protected override IInstantiator BuildInstantiator(PersistentClass mappingInfo)
    {
      return (IInstantiator) new DynamicMapInstantiator(mappingInfo);
    }

    protected override IProxyFactory BuildProxyFactory(
      PersistentClass mappingInfo,
      IGetter idGetter,
      ISetter idSetter)
    {
      IProxyFactory proxyFactory = (IProxyFactory) new MapProxyFactory();
      try
      {
        proxyFactory.PostInstantiate(this.EntityName, (System.Type) null, (ISet<System.Type>) null, (MethodInfo) null, (MethodInfo) null, (IAbstractComponentType) null);
      }
      catch (HibernateException ex)
      {
        DynamicMapEntityTuplizer.log.Warn((object) ("could not create proxy factory for:" + this.EntityName), (Exception) ex);
        proxyFactory = (IProxyFactory) null;
      }
      return proxyFactory;
    }
  }
}
