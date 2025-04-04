// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.KeyPropertyInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class KeyPropertyInstance : 
    KeyPropertyInspector,
    IKeyPropertyInstance,
    IKeyPropertyInspector,
    IInspector
  {
    private readonly KeyPropertyMapping mapping;

    public KeyPropertyInstance(KeyPropertyMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<KeyPropertyMapping, string>>) (x => x.Access), 1, value)));
      }
    }

    public void Length(int length)
    {
      this.mapping.Set<int>((Expression<Func<KeyPropertyMapping, int>>) (x => x.Length), 1, length);
    }
  }
}
