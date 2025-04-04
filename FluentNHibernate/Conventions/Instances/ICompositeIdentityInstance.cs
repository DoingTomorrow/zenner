// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.ICompositeIdentityInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface ICompositeIdentityInstance : 
    ICompositeIdentityInspector,
    IIdentityInspectorBase,
    IInspector
  {
    void UnsavedValue(string unsavedValue);

    IAccessInstance Access { get; }

    void Mapped();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    ICompositeIdentityInstance Not { get; }

    IEnumerable<IKeyPropertyInstance> KeyProperties { get; }

    IEnumerable<IKeyManyToOneInstance> KeyManyToOnes { get; }
  }
}
