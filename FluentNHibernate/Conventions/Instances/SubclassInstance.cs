// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.SubclassInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class SubclassInstance : 
    SubclassInspector,
    ISubclassInstance,
    ISubclassInspector,
    ISubclassInspectorBase,
    IInspector
  {
    private readonly SubclassMapping mapping;
    private bool nextBool = true;

    public SubclassInstance(SubclassMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ISubclassInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (ISubclassInstance) this;
      }
    }

    public void DiscriminatorValue(object value)
    {
      this.mapping.Set<object>((Expression<Func<SubclassMapping, object>>) (x => x.DiscriminatorValue), 1, value);
    }

    public void Abstract()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.Abstract), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void DynamicInsert()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.DynamicInsert), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void DynamicUpdate()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.DynamicUpdate), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void LazyLoad()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.Lazy), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Proxy(Type type)
    {
      this.mapping.Set<string>((Expression<Func<SubclassMapping, string>>) (x => x.Proxy), 1, type.AssemblyQualifiedName);
    }

    public void Proxy<T>() => this.Proxy(typeof (T));

    public void SelectBeforeUpdate()
    {
      this.mapping.Set<bool>((Expression<Func<SubclassMapping, bool>>) (x => x.SelectBeforeUpdate), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Extends<T>() => this.Extends(typeof (T));

    public void Extends(Type type)
    {
      this.mapping.Set<Type>((Expression<Func<SubclassMapping, Type>>) (x => x.Extends), 1, type);
    }
  }
}
