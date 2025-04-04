// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Component.DynamicMapComponentTuplizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Tuple.Component
{
  [Serializable]
  public class DynamicMapComponentTuplizer(NHibernate.Mapping.Component component) : 
    AbstractComponentTuplizer(component)
  {
    public override Type MappedClass => typeof (IDictionary);

    protected internal override IInstantiator BuildInstantiator(NHibernate.Mapping.Component component)
    {
      return (IInstantiator) new DynamicMapInstantiator();
    }

    protected internal override IGetter BuildGetter(NHibernate.Mapping.Component component, NHibernate.Mapping.Property prop)
    {
      return this.BuildPropertyAccessor(prop).GetGetter((Type) null, prop.Name);
    }

    protected internal override ISetter BuildSetter(NHibernate.Mapping.Component component, NHibernate.Mapping.Property prop)
    {
      return this.BuildPropertyAccessor(prop).GetSetter((Type) null, prop.Name);
    }

    private IPropertyAccessor BuildPropertyAccessor(NHibernate.Mapping.Property property)
    {
      return PropertyAccessorFactory.DynamicMapPropertyAccessor;
    }
  }
}
