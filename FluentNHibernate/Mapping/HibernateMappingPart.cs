// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.HibernateMappingPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using System;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class HibernateMappingPart : IHibernateMappingProvider
  {
    private readonly CascadeExpression<HibernateMappingPart> defaultCascade;
    private readonly AccessStrategyBuilder<HibernateMappingPart> defaultAccess;
    private readonly AttributeStore attributes = new AttributeStore();
    private bool nextBool = true;

    public HibernateMappingPart()
    {
      this.defaultCascade = new CascadeExpression<HibernateMappingPart>(this, (Action<string>) (value => this.attributes.Set(nameof (DefaultCascade), 2, (object) value)));
      this.defaultAccess = new AccessStrategyBuilder<HibernateMappingPart>(this, (Action<string>) (value => this.attributes.Set(nameof (DefaultAccess), 2, (object) value)));
    }

    public HibernateMappingPart Schema(string schema)
    {
      this.attributes.Set(nameof (Schema), 2, (object) schema);
      return this;
    }

    public CascadeExpression<HibernateMappingPart> DefaultCascade => this.defaultCascade;

    public AccessStrategyBuilder<HibernateMappingPart> DefaultAccess => this.defaultAccess;

    public HibernateMappingPart AutoImport()
    {
      this.attributes.Set(nameof (AutoImport), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public HibernateMappingPart DefaultLazy()
    {
      this.attributes.Set(nameof (DefaultLazy), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public HibernateMappingPart Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public HibernateMappingPart Catalog(string catalog)
    {
      this.attributes.Set(nameof (Catalog), 2, (object) catalog);
      return this;
    }

    public HibernateMappingPart Namespace(string ns)
    {
      this.attributes.Set(nameof (Namespace), 2, (object) ns);
      return this;
    }

    public HibernateMappingPart Assembly(string assembly)
    {
      this.attributes.Set(nameof (Assembly), 2, (object) assembly);
      return this;
    }

    HibernateMapping IHibernateMappingProvider.GetHibernateMapping()
    {
      return new HibernateMapping(this.attributes.Clone());
    }
  }
}
