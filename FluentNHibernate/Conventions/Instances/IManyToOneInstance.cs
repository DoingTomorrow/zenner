// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IManyToOneInstance
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
  public interface IManyToOneInstance : 
    IManyToOneInspector,
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    void Column(string columnName);

    void CustomClass<T>();

    void CustomClass(Type type);

    IAccessInstance Access { get; }

    ICascadeInstance Cascade { get; }

    IFetchInstance Fetch { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IManyToOneInstance Not { get; }

    INotFoundInstance NotFound { get; }

    void Index(string index);

    void Insert();

    void OptimisticLock();

    void LazyLoad();

    void LazyLoad(Laziness laziness);

    void Nullable();

    void PropertyRef(string property);

    void ReadOnly();

    void Unique();

    void UniqueKey(string key);

    void Update();

    void ForeignKey(string key);

    void Formula(string formula);

    void OverrideInferredClass(Type type);
  }
}
