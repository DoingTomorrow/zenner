// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Entity.PocoEntityTuplizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Bytecode;
using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Tuple.Entity
{
  public class PocoEntityTuplizer : AbstractEntityTuplizer
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (PocoEntityTuplizer));
    private readonly System.Type mappedClass;
    private readonly System.Type proxyInterface;
    private readonly bool islifecycleImplementor;
    private readonly bool isValidatableImplementor;
    private readonly HashedSet<string> lazyPropertyNames = new HashedSet<string>();
    private readonly HashedSet<string> unwrapProxyPropertyNames = new HashedSet<string>();
    [NonSerialized]
    private IReflectionOptimizer optimizer;
    private readonly IProxyValidator proxyValidator;

    [System.Runtime.Serialization.OnDeserialized]
    internal void OnDeserialized(StreamingContext context) => this.SetReflectionOptimizer();

    protected void SetReflectionOptimizer()
    {
      if (!NHibernate.Cfg.Environment.UseReflectionOptimizer)
        return;
      this.optimizer = NHibernate.Cfg.Environment.BytecodeProvider.GetReflectionOptimizer(this.mappedClass, this.getters, this.setters);
    }

    public PocoEntityTuplizer(EntityMetamodel entityMetamodel, PersistentClass mappedEntity)
      : base(entityMetamodel, mappedEntity)
    {
      this.mappedClass = mappedEntity.MappedClass;
      this.proxyInterface = mappedEntity.ProxyInterface;
      this.islifecycleImplementor = typeof (ILifecycle).IsAssignableFrom(this.mappedClass);
      this.isValidatableImplementor = typeof (IValidatable).IsAssignableFrom(this.mappedClass);
      foreach (NHibernate.Mapping.Property property in mappedEntity.PropertyClosureIterator)
      {
        if (property.IsLazy)
          this.lazyPropertyNames.Add(property.Name);
        if (property.UnwrapProxy)
          this.unwrapProxyPropertyNames.Add(property.Name);
      }
      this.SetReflectionOptimizer();
      this.Instantiator = this.BuildInstantiator(mappedEntity);
      if (this.hasCustomAccessors)
        this.optimizer = (IReflectionOptimizer) null;
      this.proxyValidator = NHibernate.Cfg.Environment.BytecodeProvider.ProxyFactoryFactory.ProxyValidator;
    }

    public override System.Type ConcreteProxyClass => this.proxyInterface;

    public override bool IsInstrumented => FieldInterceptionHelper.IsInstrumented(this.MappedClass);

    public override System.Type MappedClass => this.mappedClass;

    protected override IGetter BuildPropertyGetter(
      NHibernate.Mapping.Property mappedProperty,
      PersistentClass mappedEntity)
    {
      return mappedProperty.GetGetter(mappedEntity.MappedClass);
    }

    protected override ISetter BuildPropertySetter(
      NHibernate.Mapping.Property mappedProperty,
      PersistentClass mappedEntity)
    {
      return mappedProperty.GetSetter(mappedEntity.MappedClass);
    }

    protected override IInstantiator BuildInstantiator(PersistentClass persistentClass)
    {
      if (this.optimizer == null)
      {
        PocoEntityTuplizer.log.Debug((object) ("Create Instantiator without optimizer for:" + persistentClass.MappedClass.FullName));
        return (IInstantiator) new PocoInstantiator(persistentClass, (IInstantiationOptimizer) null, this.ProxyFactory, this.EntityMetamodel.HasLazyProperties || this.EntityMetamodel.HasUnwrapProxyForProperties);
      }
      PocoEntityTuplizer.log.Debug((object) ("Create Instantiator using optimizer for:" + persistentClass.MappedClass.FullName));
      return (IInstantiator) new PocoInstantiator(persistentClass, this.optimizer.InstantiationOptimizer, this.ProxyFactory, this.EntityMetamodel.HasLazyProperties || this.EntityMetamodel.HasUnwrapProxyForProperties);
    }

    protected override IProxyFactory BuildProxyFactory(
      PersistentClass persistentClass,
      IGetter idGetter,
      ISetter idSetter)
    {
      bool flag = true;
      HashedSet<System.Type> hashedSet = new HashedSet<System.Type>();
      hashedSet.Add(typeof (INHibernateProxy));
      HashedSet<System.Type> interfaces = hashedSet;
      System.Type mappedClass1 = persistentClass.MappedClass;
      System.Type proxyInterface1 = persistentClass.ProxyInterface;
      if (proxyInterface1 != null && !mappedClass1.Equals(proxyInterface1))
      {
        if (!proxyInterface1.IsInterface)
          throw new MappingException("proxy must be either an interface, or the class itself: " + this.EntityName);
        flag = false;
        interfaces.Add(proxyInterface1);
      }
      if (mappedClass1.IsInterface)
      {
        flag = false;
        interfaces.Add(mappedClass1);
      }
      foreach (Subclass subclass in persistentClass.SubclassIterator)
      {
        System.Type proxyInterface2 = subclass.ProxyInterface;
        System.Type mappedClass2 = subclass.MappedClass;
        if (proxyInterface2 != null && !mappedClass2.Equals(proxyInterface2))
        {
          if (!proxyInterface2.IsInterface)
            throw new MappingException("proxy must be either an interface, or the class itself: " + subclass.EntityName);
          interfaces.Add(proxyInterface2);
        }
      }
      if (PocoEntityTuplizer.log.IsErrorEnabled && flag)
        this.LogPropertyAccessorsErrors(persistentClass);
      MethodInfo method1 = idGetter == null ? (MethodInfo) null : idGetter.Method;
      MethodInfo method2 = idSetter == null ? (MethodInfo) null : idSetter.Method;
      MethodInfo method3 = method1 == null || proxyInterface1 == null ? (MethodInfo) null : ReflectHelper.TryGetMethod(proxyInterface1, method1);
      MethodInfo method4 = method2 == null || proxyInterface1 == null ? (MethodInfo) null : ReflectHelper.TryGetMethod(proxyInterface1, method2);
      IProxyFactory proxyFactory = this.BuildProxyFactoryInternal(persistentClass, idGetter, idSetter);
      try
      {
        proxyFactory.PostInstantiate(this.EntityName, mappedClass1, (ISet<System.Type>) interfaces, method3, method4, persistentClass.HasEmbeddedIdentifier ? (IAbstractComponentType) persistentClass.Identifier.Type : (IAbstractComponentType) null);
      }
      catch (HibernateException ex)
      {
        PocoEntityTuplizer.log.Warn((object) ("could not create proxy factory for:" + this.EntityName), (Exception) ex);
        proxyFactory = (IProxyFactory) null;
      }
      return proxyFactory;
    }

    private void LogPropertyAccessorsErrors(PersistentClass persistentClass)
    {
      if (this.proxyValidator == null)
        return;
      System.Type mappedClass = persistentClass.MappedClass;
      foreach (NHibernate.Mapping.Property property in persistentClass.PropertyIterator)
      {
        if (!this.proxyValidator.IsProxeable(property.GetGetter(mappedClass).Method))
          PocoEntityTuplizer.log.Error((object) string.Format("Getters of lazy classes cannot be final: {0}.{1}", (object) persistentClass.MappedClass.FullName, (object) property.Name));
        if (!this.proxyValidator.IsProxeable(property.GetSetter(mappedClass).Method))
          PocoEntityTuplizer.log.Error((object) string.Format("Setters of lazy classes cannot be final: {0}.{1}", (object) persistentClass.MappedClass.FullName, (object) property.Name));
      }
    }

    protected virtual IProxyFactory BuildProxyFactoryInternal(
      PersistentClass @class,
      IGetter getter,
      ISetter setter)
    {
      return NHibernate.Cfg.Environment.BytecodeProvider.ProxyFactoryFactory.BuildProxyFactory();
    }

    public override void AfterInitialize(
      object entity,
      bool lazyPropertiesAreUnfetched,
      ISessionImplementor session)
    {
      if (!this.IsInstrumented || !this.EntityMetamodel.HasLazyProperties && !this.EntityMetamodel.HasUnwrapProxyForProperties)
        return;
      HashedSet<string> lazyPropertyNames = !lazyPropertiesAreUnfetched || !this.EntityMetamodel.HasLazyProperties ? (HashedSet<string>) null : this.lazyPropertyNames;
      FieldInterceptionHelper.InjectFieldInterceptor(entity, this.EntityName, this.MappedClass, (ISet<string>) lazyPropertyNames, (ISet<string>) this.unwrapProxyPropertyNames, session);
    }

    public override object[] GetPropertyValues(object entity)
    {
      return this.ShouldGetAllProperties(entity) && this.optimizer != null && this.optimizer.AccessOptimizer != null ? this.GetPropertyValuesWithOptimizer(entity) : base.GetPropertyValues(entity);
    }

    private object[] GetPropertyValuesWithOptimizer(object entity)
    {
      return this.optimizer.AccessOptimizer.GetPropertyValues(entity);
    }

    public override object[] GetPropertyValuesToInsert(
      object entity,
      IDictionary mergeMap,
      ISessionImplementor session)
    {
      return this.ShouldGetAllProperties(entity) && this.optimizer != null && this.optimizer.AccessOptimizer != null ? this.GetPropertyValuesWithOptimizer(entity) : base.GetPropertyValuesToInsert(entity, mergeMap, session);
    }

    public override bool HasUninitializedLazyProperties(object entity)
    {
      if (!this.EntityMetamodel.HasLazyProperties)
        return false;
      IFieldInterceptor fieldInterceptor = FieldInterceptionHelper.ExtractFieldInterceptor(entity);
      return fieldInterceptor != null && !fieldInterceptor.IsInitialized;
    }

    public override bool IsLifecycleImplementor => this.islifecycleImplementor;

    public override void SetPropertyValues(object entity, object[] values)
    {
      if (!this.EntityMetamodel.HasLazyProperties && this.optimizer != null && this.optimizer.AccessOptimizer != null)
        this.SetPropertyValuesWithOptimizer(entity, values);
      else
        base.SetPropertyValues(entity, values);
    }

    private void SetPropertyValuesWithOptimizer(object entity, object[] values)
    {
      try
      {
        this.optimizer.AccessOptimizer.SetPropertyValues(entity, values);
      }
      catch (InvalidCastException ex)
      {
        throw new PropertyAccessException((Exception) ex, "Invalid Cast (check your mapping for property type mismatches);", true, entity.GetType());
      }
    }

    public override bool IsValidatableImplementor => this.isValidatableImplementor;

    public override EntityMode EntityMode => EntityMode.Poco;
  }
}
