// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Component.PocoComponentTuplizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Bytecode;
using NHibernate.Engine;
using NHibernate.Properties;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Tuple.Component
{
  [Serializable]
  public class PocoComponentTuplizer : AbstractComponentTuplizer
  {
    private readonly Type componentClass;
    private readonly ISetter parentSetter;
    private readonly IGetter parentGetter;
    [NonSerialized]
    private IReflectionOptimizer optimizer;

    [System.Runtime.Serialization.OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
      this.optimizer = NHibernate.Cfg.Environment.BytecodeProvider.GetReflectionOptimizer(this.componentClass, this.getters, this.setters);
    }

    public PocoComponentTuplizer(NHibernate.Mapping.Component component)
      : base(component)
    {
      this.componentClass = component.ComponentClass;
      NHibernate.Mapping.Property parentProperty = component.ParentProperty;
      if (parentProperty == null)
      {
        this.parentSetter = (ISetter) null;
        this.parentGetter = (IGetter) null;
      }
      else
      {
        this.parentSetter = parentProperty.GetSetter(this.componentClass);
        this.parentGetter = parentProperty.GetGetter(this.componentClass);
      }
      if (this.hasCustomAccessors || !NHibernate.Cfg.Environment.UseReflectionOptimizer)
        this.optimizer = (IReflectionOptimizer) null;
      else
        this.optimizer = NHibernate.Cfg.Environment.BytecodeProvider.GetReflectionOptimizer(this.componentClass, this.getters, this.setters);
    }

    public override Type MappedClass => this.componentClass;

    public override object[] GetPropertyValues(object component)
    {
      if (object.Equals(BackrefPropertyAccessor.Unknown, component) || component == null)
        return new object[this.propertySpan];
      return this.optimizer != null && this.optimizer.AccessOptimizer != null ? this.optimizer.AccessOptimizer.GetPropertyValues(component) : base.GetPropertyValues(component);
    }

    public override void SetPropertyValues(object component, object[] values)
    {
      if (this.optimizer != null && this.optimizer.AccessOptimizer != null)
        this.optimizer.AccessOptimizer.SetPropertyValues(component, values);
      else
        base.SetPropertyValues(component, values);
    }

    public override object GetParent(object component) => this.parentGetter.Get(component);

    public override void SetParent(
      object component,
      object parent,
      ISessionFactoryImplementor factory)
    {
      this.parentSetter.Set(component, parent);
    }

    public override bool HasParentProperty => this.parentGetter != null;

    protected internal override IInstantiator BuildInstantiator(NHibernate.Mapping.Component component)
    {
      return this.optimizer == null ? (IInstantiator) new PocoInstantiator(component, (IInstantiationOptimizer) null) : (IInstantiator) new PocoInstantiator(component, this.optimizer.InstantiationOptimizer);
    }

    protected internal override IGetter BuildGetter(NHibernate.Mapping.Component component, NHibernate.Mapping.Property prop)
    {
      return prop.GetGetter(component.ComponentClass);
    }

    protected internal override ISetter BuildSetter(NHibernate.Mapping.Component component, NHibernate.Mapping.Property prop)
    {
      return prop.GetSetter(component.ComponentClass);
    }
  }
}
