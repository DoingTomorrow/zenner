// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IHibernateMappingInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IHibernateMappingInstance : IHibernateMappingInspector, IInspector
  {
    void Catalog(string catalog);

    void Schema(string schema);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IHibernateMappingInstance Not { get; }

    void DefaultLazy();

    void AutoImport();

    ICascadeInstance DefaultCascade { get; }

    IAccessInstance DefaultAccess { get; }
  }
}
