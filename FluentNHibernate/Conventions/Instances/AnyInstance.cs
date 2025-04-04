// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.AnyInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class AnyInstance : AnyInspector, IAnyInstance, IAnyInspector, IAccessInspector, IInspector
  {
    private readonly AnyMapping mapping;

    public AnyInstance(AnyMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<AnyMapping, string>>) (x => x.Access), 1, value)));
      }
    }
  }
}
