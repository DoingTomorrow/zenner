// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.OneToOneInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class OneToOneInstance : 
    OneToOneInspector,
    IOneToOneInstance,
    IOneToOneInspector,
    IInspector
  {
    private readonly OneToOneMapping mapping;
    private bool nextBool = true;

    public OneToOneInstance(OneToOneMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<OneToOneMapping, string>>) (x => x.Access), 1, value)));
      }
    }

    public ICascadeInstance Cascade
    {
      get
      {
        return (ICascadeInstance) new CascadeInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<OneToOneMapping, string>>) (x => x.Cascade), 1, value)));
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IOneToOneInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IOneToOneInstance) this;
      }
    }

    public IFetchInstance Fetch
    {
      get
      {
        return (IFetchInstance) new FetchInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<OneToOneMapping, string>>) (x => x.Fetch), 1, value)));
      }
    }

    public void Class<T>() => this.Class(typeof (T));

    public void Class(Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<OneToOneMapping, TypeReference>>) (x => x.Class), 1, new TypeReference(type));
    }

    public void Constrained()
    {
      this.mapping.Set<bool>((Expression<Func<OneToOneMapping, bool>>) (x => x.Constrained), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void ForeignKey(string key)
    {
      this.mapping.Set<string>((Expression<Func<OneToOneMapping, string>>) (x => x.ForeignKey), 1, key);
    }

    public void LazyLoad()
    {
      if (this.nextBool)
        this.LazyLoad(Laziness.Proxy);
      else
        this.LazyLoad(Laziness.False);
      this.nextBool = true;
    }

    public void LazyLoad(Laziness laziness)
    {
      this.mapping.Set<string>((Expression<Func<OneToOneMapping, string>>) (x => x.Lazy), 1, laziness.ToString());
      this.nextBool = true;
    }

    public void PropertyRef(string propertyName)
    {
      this.mapping.Set<string>((Expression<Func<OneToOneMapping, string>>) (x => x.PropertyRef), 1, propertyName);
    }

    public void OverrideInferredClass(Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<OneToOneMapping, TypeReference>>) (x => x.Class), 1, new TypeReference(type));
    }
  }
}
