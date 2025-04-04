// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.DynamicComponentInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class DynamicComponentInstance : 
    DynamicComponentInspector,
    IDynamicComponentInstance,
    IComponentBaseInstance,
    IDynamicComponentInspector,
    IComponentBaseInspector,
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    private readonly ComponentMapping mapping;
    private bool nextBool;

    public DynamicComponentInstance(ComponentMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
      this.nextBool = true;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IDynamicComponentInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IDynamicComponentInstance) this;
      }
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<ComponentMapping, string>>) (x => x.Access), 1, value)));
      }
    }

    public void Update()
    {
      this.mapping.Set<bool>((Expression<Func<ComponentMapping, bool>>) (x => x.Update), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Insert()
    {
      this.mapping.Set<bool>((Expression<Func<ComponentMapping, bool>>) (x => x.Insert), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Unique()
    {
      this.mapping.Set<bool>((Expression<Func<ComponentMapping, bool>>) (x => x.Unique), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void OptimisticLock()
    {
      this.mapping.Set<bool>((Expression<Func<ComponentMapping, bool>>) (x => x.OptimisticLock), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public IEnumerable<IOneToOneInstance> OneToOnes
    {
      get
      {
        return this.mapping.OneToOnes.Select<OneToOneMapping, OneToOneInstance>((Func<OneToOneMapping, OneToOneInstance>) (x => new OneToOneInstance(x))).Cast<IOneToOneInstance>();
      }
    }

    public IEnumerable<IPropertyInstance> Properties
    {
      get
      {
        return this.mapping.Properties.Select<PropertyMapping, PropertyInstance>((Func<PropertyMapping, PropertyInstance>) (x => new PropertyInstance(x))).Cast<IPropertyInstance>();
      }
    }
  }
}
