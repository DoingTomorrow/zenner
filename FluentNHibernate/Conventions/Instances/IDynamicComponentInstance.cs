// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IDynamicComponentInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IDynamicComponentInstance : 
    IComponentBaseInstance,
    IDynamicComponentInspector,
    IComponentBaseInspector,
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IDynamicComponentInstance Not { get; }
  }
}
