// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IComponentBaseInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IComponentBaseInstance : 
    IComponentBaseInspector,
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    IAccessInstance Access { get; }

    void Update();

    void Insert();

    void Unique();

    void OptimisticLock();

    IEnumerable<IOneToOneInstance> OneToOnes { get; }

    IEnumerable<IPropertyInstance> Properties { get; }
  }
}
