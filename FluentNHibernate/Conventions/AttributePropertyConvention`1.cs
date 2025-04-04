// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AttributePropertyConvention`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public abstract class AttributePropertyConvention<T> : 
    IPropertyConvention,
    IConvention<IPropertyInspector, IPropertyInstance>,
    IConvention,
    IPropertyConventionAcceptance,
    IConventionAcceptance<IPropertyInspector>
    where T : Attribute
  {
    public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
    {
      criteria.Expect((Expression<Func<IPropertyInspector, bool>>) (property => Attribute.GetCustomAttribute(property.Property.MemberInfo, typeof (T)) as T != (object) null));
    }

    public void Apply(IPropertyInstance instance)
    {
      this.Apply(Attribute.GetCustomAttribute(instance.Property.MemberInfo, typeof (T)) as T, instance);
    }

    protected abstract void Apply(T attribute, IPropertyInstance instance);
  }
}
