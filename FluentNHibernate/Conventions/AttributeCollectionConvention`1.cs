// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AttributeCollectionConvention`1
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
  public abstract class AttributeCollectionConvention<T> : 
    ICollectionConvention,
    IConvention<ICollectionInspector, ICollectionInstance>,
    IConvention,
    ICollectionConventionAcceptance,
    IConventionAcceptance<ICollectionInspector>
    where T : Attribute
  {
    public void Accept(IAcceptanceCriteria<ICollectionInspector> criteria)
    {
      criteria.Expect((Expression<Func<ICollectionInspector, bool>>) (property => Attribute.GetCustomAttribute(property.Member, typeof (T)) as T != (object) null));
    }

    public void Apply(ICollectionInstance instance)
    {
      this.Apply(Attribute.GetCustomAttribute(instance.Member, typeof (T)) as T, instance);
    }

    protected abstract void Apply(T attribute, ICollectionInstance instance);
  }
}
