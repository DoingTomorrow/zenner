// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IOneToManyCollectionInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IOneToManyCollectionInstance : 
    IOneToManyCollectionInspector,
    ICollectionInstance,
    ICollectionInspector,
    IInspector
  {
    IOneToManyInstance Relationship { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IOneToManyCollectionInstance Not { get; }

    IManyToOneInstance OtherSide { get; }

    void ApplyFilter(string name, string condition);

    void ApplyFilter(string name);

    void ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new();

    void ApplyFilter<TFilter>() where TFilter : FilterDefinition, new();
  }
}
