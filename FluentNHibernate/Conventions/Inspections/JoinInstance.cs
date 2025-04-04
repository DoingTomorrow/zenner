// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.JoinInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class JoinInstance : JoinInspector, IJoinInstance, IJoinInspector, IInspector
  {
    private readonly JoinMapping mapping;
    private bool nextBool = true;

    public JoinInstance(JoinMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IJoinInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IJoinInstance) this;
      }
    }

    public IFetchInstance Fetch
    {
      get
      {
        return (IFetchInstance) new FetchInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<JoinMapping, string>>) (x => x.Fetch), 1, value)));
      }
    }

    public void Inverse()
    {
      this.mapping.Set<bool>((Expression<Func<JoinMapping, bool>>) (x => x.Inverse), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public IKeyInstance Key => (IKeyInstance) new KeyInstance(this.mapping.Key);

    public void Optional()
    {
      this.mapping.Set<bool>((Expression<Func<JoinMapping, bool>>) (x => x.Optional), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Schema(string schema)
    {
      this.mapping.Set<string>((Expression<Func<JoinMapping, string>>) (x => x.Schema), 1, schema);
    }

    public void Table(string table)
    {
      this.mapping.Set<string>((Expression<Func<JoinMapping, string>>) (x => x.TableName), 1, table);
    }

    public void Catalog(string catalog)
    {
      this.mapping.Set<string>((Expression<Func<JoinMapping, string>>) (x => x.Catalog), 1, catalog);
    }

    public void Subselect(string subselect)
    {
      this.mapping.Set<string>((Expression<Func<JoinMapping, string>>) (x => x.Subselect), 1, subselect);
    }
  }
}
