// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.KeyManyToOneInstance
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
  public class KeyManyToOneInstance : 
    KeyManyToOneInspector,
    IKeyManyToOneInstance,
    IKeyManyToOneInspector,
    IInspector
  {
    private readonly KeyManyToOneMapping mapping;
    private bool nextBool = true;

    public KeyManyToOneInstance(KeyManyToOneMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<KeyManyToOneMapping, string>>) (x => x.Access), 1, value)));
      }
    }

    public void ForeignKey(string name)
    {
      this.mapping.Set<string>((Expression<Func<KeyManyToOneMapping, string>>) (x => x.ForeignKey), 1, name);
    }

    public void Lazy()
    {
      this.mapping.Set<bool>((Expression<Func<KeyManyToOneMapping, bool>>) (x => x.Lazy), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public INotFoundInstance NotFound
    {
      get
      {
        return (INotFoundInstance) new NotFoundInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<KeyManyToOneMapping, string>>) (x => x.NotFound), 1, value)));
      }
    }

    public IKeyManyToOneInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IKeyManyToOneInstance) this;
      }
    }
  }
}
