// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.PocoInstantiator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Bytecode;
using NHibernate.Mapping;
using NHibernate.Proxy;
using NHibernate.Util;
using System;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Tuple
{
  [Serializable]
  public class PocoInstantiator : IInstantiator, IDeserializationCallback
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (PocoInstantiator));
    private readonly Type mappedClass;
    [NonSerialized]
    private readonly IInstantiationOptimizer optimizer;
    private readonly IProxyFactory proxyFactory;
    private readonly bool generateFieldInterceptionProxy;
    private readonly bool embeddedIdentifier;
    [NonSerialized]
    private ConstructorInfo constructor;
    private readonly Type proxyInterface;

    public PocoInstantiator()
    {
    }

    public PocoInstantiator(Component component, IInstantiationOptimizer optimizer)
    {
      this.mappedClass = component.ComponentClass;
      this.optimizer = optimizer;
      this.proxyInterface = (Type) null;
      this.embeddedIdentifier = false;
      try
      {
        this.constructor = ReflectHelper.GetDefaultConstructor(this.mappedClass);
      }
      catch (PropertyNotFoundException ex)
      {
        PocoInstantiator.log.Info((object) string.Format("no default (no-argument) constructor for class: {0} (class must be instantiated by Interceptor)", (object) this.mappedClass.FullName));
        this.constructor = (ConstructorInfo) null;
      }
    }

    public PocoInstantiator(
      PersistentClass persistentClass,
      IInstantiationOptimizer optimizer,
      IProxyFactory proxyFactory,
      bool generateFieldInterceptionProxy)
    {
      this.mappedClass = persistentClass.MappedClass;
      this.proxyInterface = persistentClass.ProxyInterface;
      this.embeddedIdentifier = persistentClass.HasEmbeddedIdentifier;
      this.optimizer = optimizer;
      this.proxyFactory = proxyFactory;
      this.generateFieldInterceptionProxy = generateFieldInterceptionProxy;
      try
      {
        this.constructor = ReflectHelper.GetDefaultConstructor(this.mappedClass);
      }
      catch (PropertyNotFoundException ex)
      {
        PocoInstantiator.log.Info((object) string.Format("no default (no-argument) constructor for class: {0} (class must be instantiated by Interceptor)", (object) this.mappedClass.FullName));
        this.constructor = (ConstructorInfo) null;
      }
    }

    public object Instantiate(object id)
    {
      return !this.embeddedIdentifier || id == null || !id.GetType().Equals(this.mappedClass) ? this.Instantiate() : id;
    }

    public object Instantiate()
    {
      if (ReflectHelper.IsAbstractClass(this.mappedClass))
        throw new InstantiationException("Cannot instantiate abstract class or interface: ", this.mappedClass);
      return this.generateFieldInterceptionProxy ? this.proxyFactory.GetFieldInterceptionProxy(this.GetInstance()) : this.GetInstance();
    }

    private object GetInstance()
    {
      if (this.optimizer != null)
        return this.optimizer.CreateInstance();
      if (this.mappedClass.IsValueType)
        return NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(this.mappedClass, true);
      if (this.constructor == null)
        throw new InstantiationException("No default constructor for entity: ", this.mappedClass);
      try
      {
        return this.constructor.Invoke((object[]) null);
      }
      catch (Exception ex)
      {
        throw new InstantiationException("Could not instantiate entity: ", ex, this.mappedClass);
      }
    }

    public bool IsInstance(object obj)
    {
      if (this.mappedClass.IsInstanceOfType(obj))
        return true;
      return this.proxyInterface != null && this.proxyInterface.IsInstanceOfType(obj);
    }

    public void OnDeserialization(object sender)
    {
      this.constructor = ReflectHelper.GetDefaultConstructor(this.mappedClass);
    }
  }
}
