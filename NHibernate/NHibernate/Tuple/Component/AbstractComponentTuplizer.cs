// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Component.AbstractComponentTuplizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Properties;
using System;

#nullable disable
namespace NHibernate.Tuple.Component
{
  [Serializable]
  public abstract class AbstractComponentTuplizer : IComponentTuplizer, ITuplizer
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AbstractComponentTuplizer));
    protected internal int propertySpan;
    protected internal IGetter[] getters;
    protected internal ISetter[] setters;
    protected internal IInstantiator instantiator;
    protected internal bool hasCustomAccessors;

    protected internal AbstractComponentTuplizer(NHibernate.Mapping.Component component)
    {
      this.propertySpan = component.PropertySpan;
      this.getters = new IGetter[this.propertySpan];
      this.setters = new ISetter[this.propertySpan];
      bool flag = false;
      int index1 = 0;
      foreach (NHibernate.Mapping.Property prop in component.PropertyIterator)
      {
        this.getters[index1] = this.BuildGetter(component, prop);
        this.setters[index1] = this.BuildSetter(component, prop);
        if (!prop.IsBasicPropertyAccessor)
          flag = true;
        ++index1;
      }
      if (AbstractComponentTuplizer.log.IsDebugEnabled)
        AbstractComponentTuplizer.log.DebugFormat("{0} accessors found for component: {1}", flag ? (object) "Custom" : (object) "No custom", (object) component.ComponentClassName);
      this.hasCustomAccessors = flag;
      string[] strArray1 = new string[this.propertySpan];
      string[] strArray2 = new string[this.propertySpan];
      Type[] typeArray = new Type[this.propertySpan];
      for (int index2 = 0; index2 < this.propertySpan; ++index2)
      {
        strArray1[index2] = this.getters[index2].PropertyName;
        strArray2[index2] = this.setters[index2].PropertyName;
        typeArray[index2] = this.getters[index2].ReturnType;
      }
      this.instantiator = this.BuildInstantiator(component);
    }

    public virtual object GetParent(object component) => (object) null;

    public virtual void SetParent(
      object component,
      object parent,
      ISessionFactoryImplementor factory)
    {
      throw new NotSupportedException();
    }

    public virtual bool HasParentProperty => false;

    public abstract Type MappedClass { get; }

    public virtual object[] GetPropertyValues(object component)
    {
      object[] propertyValues = new object[this.propertySpan];
      if (component != null)
      {
        for (int i = 0; i < this.propertySpan; ++i)
          propertyValues[i] = this.GetPropertyValue(component, i);
      }
      return propertyValues;
    }

    public virtual void SetPropertyValues(object component, object[] values)
    {
      for (int index = 0; index < this.propertySpan; ++index)
        this.setters[index].Set(component, values[index]);
    }

    public virtual object GetPropertyValue(object component, int i)
    {
      return this.getters[i].Get(component);
    }

    public virtual object Instantiate() => this.instantiator.Instantiate();

    public virtual bool IsInstance(object obj) => this.instantiator.IsInstance(obj);

    protected internal abstract IInstantiator BuildInstantiator(NHibernate.Mapping.Component component);

    protected internal abstract IGetter BuildGetter(NHibernate.Mapping.Component component, NHibernate.Mapping.Property prop);

    protected internal abstract ISetter BuildSetter(NHibernate.Mapping.Component component, NHibernate.Mapping.Property prop);
  }
}
