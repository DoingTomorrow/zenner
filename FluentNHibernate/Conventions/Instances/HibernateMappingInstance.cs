// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.HibernateMappingInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class HibernateMappingInstance : 
    HibernateMappingInspector,
    IHibernateMappingInstance,
    IHibernateMappingInspector,
    IInspector
  {
    private readonly HibernateMapping mapping;
    private bool nextBool = true;

    public HibernateMappingInstance(HibernateMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public void Catalog(string catalog)
    {
      this.mapping.Set<string>((Expression<Func<HibernateMapping, string>>) (x => x.Catalog), 1, catalog);
    }

    public void Schema(string schema)
    {
      this.mapping.Set<string>((Expression<Func<HibernateMapping, string>>) (x => x.Schema), 1, schema);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IHibernateMappingInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IHibernateMappingInstance) this;
      }
    }

    public void DefaultLazy()
    {
      this.mapping.Set<bool>((Expression<Func<HibernateMapping, bool>>) (x => x.DefaultLazy), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void AutoImport()
    {
      this.mapping.Set<bool>((Expression<Func<HibernateMapping, bool>>) (x => x.AutoImport), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public ICascadeInstance DefaultCascade
    {
      get
      {
        return (ICascadeInstance) new CascadeInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<HibernateMapping, string>>) (x => x.DefaultCascade), 1, value)));
      }
    }

    public IAccessInstance DefaultAccess
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<HibernateMapping, string>>) (x => x.DefaultAccess), 1, value)));
      }
    }
  }
}
