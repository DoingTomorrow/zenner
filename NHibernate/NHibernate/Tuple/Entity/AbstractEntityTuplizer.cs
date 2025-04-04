// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Entity.AbstractEntityTuplizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Intercept;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Proxy;
using NHibernate.Type;
using System.Collections;

#nullable disable
namespace NHibernate.Tuple.Entity
{
  public abstract class AbstractEntityTuplizer : IEntityTuplizer, ITuplizer
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AbstractEntityTuplizer));
    private readonly EntityMetamodel entityMetamodel;
    private readonly IGetter idGetter;
    private readonly ISetter idSetter;
    protected int propertySpan;
    protected IGetter[] getters;
    protected ISetter[] setters;
    protected bool hasCustomAccessors;
    private readonly IProxyFactory proxyFactory;
    private readonly IAbstractComponentType identifierMapperType;

    protected AbstractEntityTuplizer(EntityMetamodel entityMetamodel, PersistentClass mappingInfo)
    {
      this.entityMetamodel = entityMetamodel;
      if (!entityMetamodel.IdentifierProperty.IsVirtual)
      {
        this.idGetter = this.BuildPropertyGetter(mappingInfo.IdentifierProperty, mappingInfo);
        this.idSetter = this.BuildPropertySetter(mappingInfo.IdentifierProperty, mappingInfo);
      }
      else
      {
        this.idGetter = (IGetter) null;
        this.idSetter = (ISetter) null;
      }
      this.propertySpan = entityMetamodel.PropertySpan;
      this.getters = new IGetter[this.propertySpan];
      this.setters = new ISetter[this.propertySpan];
      bool flag = false;
      int index = 0;
      foreach (NHibernate.Mapping.Property mappedProperty in mappingInfo.PropertyClosureIterator)
      {
        this.getters[index] = this.BuildPropertyGetter(mappedProperty, mappingInfo);
        this.setters[index] = this.BuildPropertySetter(mappedProperty, mappingInfo);
        if (!mappedProperty.IsBasicPropertyAccessor)
          flag = true;
        ++index;
      }
      if (AbstractEntityTuplizer.log.IsDebugEnabled)
        AbstractEntityTuplizer.log.DebugFormat("{0} accessors found for entity: {1}", flag ? (object) "Custom" : (object) "No custom", (object) mappingInfo.EntityName);
      this.hasCustomAccessors = flag;
      if (entityMetamodel.IsLazy)
      {
        this.proxyFactory = this.BuildProxyFactory(mappingInfo, this.idGetter, this.idSetter);
        if (this.proxyFactory == null)
          entityMetamodel.IsLazy = false;
      }
      else
        this.proxyFactory = (IProxyFactory) null;
      Component identifierMapper = mappingInfo.IdentifierMapper;
      this.identifierMapperType = identifierMapper == null ? (IAbstractComponentType) null : (IAbstractComponentType) identifierMapper.Type;
    }

    public virtual bool IsLifecycleImplementor => false;

    public virtual bool IsValidatableImplementor => false;

    public abstract System.Type ConcreteProxyClass { get; }

    public abstract bool IsInstrumented { get; }

    public object Instantiate(object id)
    {
      object entity = this.Instantiator.Instantiate(id);
      if (id != null)
        this.SetIdentifier(entity, id);
      return entity;
    }

    public object GetIdentifier(object entity)
    {
      object component;
      if (this.entityMetamodel.IdentifierProperty.IsEmbedded)
        component = entity;
      else if (this.idGetter == null)
      {
        if (this.identifierMapperType == null)
          throw new HibernateException("The class has no identifier property: " + this.EntityName);
        ComponentType type = (ComponentType) this.entityMetamodel.IdentifierProperty.Type;
        component = type.Instantiate(this.EntityMode);
        type.SetPropertyValues(component, this.identifierMapperType.GetPropertyValues(entity, this.EntityMode), this.EntityMode);
      }
      else
        component = this.idGetter.Get(entity);
      return component;
    }

    public void SetIdentifier(object entity, object id)
    {
      if (this.entityMetamodel.IdentifierProperty.IsEmbedded)
      {
        if (entity == id)
          return;
        IAbstractComponentType type = (IAbstractComponentType) this.entityMetamodel.IdentifierProperty.Type;
        type.SetPropertyValues(entity, type.GetPropertyValues(id, this.EntityMode), this.EntityMode);
      }
      else
      {
        if (this.idSetter == null)
          return;
        this.idSetter.Set(entity, id);
      }
    }

    public void ResetIdentifier(object entity, object currentId, object currentVersion)
    {
      if (this.entityMetamodel.IdentifierProperty.IdentifierGenerator is Assigned)
        return;
      object defaultValue = this.entityMetamodel.IdentifierProperty.UnsavedValue.GetDefaultValue(currentId);
      this.SetIdentifier(entity, defaultValue);
      VersionProperty versionProperty = this.entityMetamodel.VersionProperty;
      if (!this.entityMetamodel.IsVersioned)
        return;
      this.SetPropertyValue(entity, this.entityMetamodel.VersionPropertyIndex, versionProperty.UnsavedValue.GetDefaultValue(currentVersion));
    }

    public object GetVersion(object entity)
    {
      return !this.entityMetamodel.IsVersioned ? (object) null : this.getters[this.entityMetamodel.VersionPropertyIndex].Get(entity);
    }

    public void SetPropertyValue(object entity, int i, object value)
    {
      this.setters[i].Set(entity, value);
    }

    public void SetPropertyValue(object entity, string propertyName, object value)
    {
      this.setters[this.entityMetamodel.GetPropertyIndex(propertyName)].Set(entity, value);
    }

    public virtual object[] GetPropertyValuesToInsert(
      object entity,
      IDictionary mergeMap,
      ISessionImplementor session)
    {
      int propertySpan = this.entityMetamodel.PropertySpan;
      object[] propertyValuesToInsert = new object[propertySpan];
      for (int index = 0; index < propertySpan; ++index)
        propertyValuesToInsert[index] = this.getters[index].GetForInsert(entity, mergeMap, session);
      return propertyValuesToInsert;
    }

    public object GetPropertyValue(object entity, string propertyPath)
    {
      int length = propertyPath.IndexOf('.');
      int propertyIndex = this.entityMetamodel.GetPropertyIndex(length > 0 ? propertyPath.Substring(0, length) : propertyPath);
      object propertyValue = this.GetPropertyValue(entity, propertyIndex);
      return length > 0 ? this.GetComponentValue((ComponentType) this.entityMetamodel.PropertyTypes[propertyIndex], propertyValue, propertyPath.Substring(length + 1)) : propertyValue;
    }

    public virtual void AfterInitialize(
      object entity,
      bool lazyPropertiesAreUnfetched,
      ISessionImplementor session)
    {
    }

    public bool HasProxy => this.entityMetamodel.IsLazy;

    public object CreateProxy(object id, ISessionImplementor session)
    {
      return (object) this.ProxyFactory.GetProxy(id, session);
    }

    public virtual bool HasUninitializedLazyProperties(object entity) => false;

    public abstract System.Type MappedClass { get; }

    public virtual object[] GetPropertyValues(object entity)
    {
      bool allProperties = this.ShouldGetAllProperties(entity);
      int propertySpan = this.entityMetamodel.PropertySpan;
      object[] propertyValues = new object[propertySpan];
      for (int index = 0; index < propertySpan; ++index)
      {
        StandardProperty property = this.entityMetamodel.Properties[index];
        propertyValues[index] = allProperties || !property.IsLazy ? this.getters[index].Get(entity) : LazyPropertyInitializer.UnfetchedProperty;
      }
      return propertyValues;
    }

    public virtual void SetPropertyValues(object entity, object[] values)
    {
      bool flag = !this.entityMetamodel.HasLazyProperties;
      for (int index = 0; index < this.entityMetamodel.PropertySpan; ++index)
      {
        if (flag || !object.Equals(LazyPropertyInitializer.UnfetchedProperty, values[index]))
          this.setters[index].Set(entity, values[index]);
      }
    }

    public virtual object GetPropertyValue(object entity, int i) => this.getters[i].Get(entity);

    public object Instantiate() => this.Instantiate((object) null);

    public bool IsInstance(object obj) => this.Instantiator.IsInstance(obj);

    public abstract EntityMode EntityMode { get; }

    protected virtual IInstantiator Instantiator { get; set; }

    protected virtual string EntityName => this.entityMetamodel.Name;

    protected virtual ISet<string> SubclassEntityNames => this.entityMetamodel.SubclassEntityNames;

    protected abstract IGetter BuildPropertyGetter(
      NHibernate.Mapping.Property mappedProperty,
      PersistentClass mappedEntity);

    protected abstract ISetter BuildPropertySetter(
      NHibernate.Mapping.Property mappedProperty,
      PersistentClass mappedEntity);

    protected abstract IInstantiator BuildInstantiator(PersistentClass mappingInfo);

    protected abstract IProxyFactory BuildProxyFactory(
      PersistentClass mappingInfo,
      IGetter idGetter,
      ISetter idSetter);

    protected virtual object GetComponentValue(
      ComponentType type,
      object component,
      string propertyPath)
    {
      int length = propertyPath.IndexOf('.');
      string str = length > 0 ? propertyPath.Substring(0, length) : propertyPath;
      string[] propertyNames = type.PropertyNames;
      int i = 0;
      while (i < propertyNames.Length && !str.Equals(propertyNames[i]))
        ++i;
      if (i == propertyNames.Length)
        throw new MappingException("component property not found: " + str);
      object propertyValue = type.GetPropertyValue(component, i, this.EntityMode);
      return length > 0 ? this.GetComponentValue((ComponentType) type.Subtypes[i], propertyValue, propertyPath.Substring(length + 1)) : propertyValue;
    }

    protected virtual IProxyFactory ProxyFactory => this.proxyFactory;

    protected virtual bool ShouldGetAllProperties(object entity)
    {
      return !this.HasUninitializedLazyProperties(entity);
    }

    protected EntityMetamodel EntityMetamodel => this.entityMetamodel;
  }
}
