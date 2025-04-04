// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IOneToOneInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using System;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IOneToOneInstance : IOneToOneInspector, IInspector
  {
    IAccessInstance Access { get; }

    ICascadeInstance Cascade { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IOneToOneInstance Not { get; }

    IFetchInstance Fetch { get; }

    void Class<T>();

    void Class(Type type);

    void Constrained();

    void ForeignKey(string key);

    void LazyLoad();

    void LazyLoad(Laziness laziness);

    void PropertyRef(string propertyName);

    void OverrideInferredClass(Type type);
  }
}
