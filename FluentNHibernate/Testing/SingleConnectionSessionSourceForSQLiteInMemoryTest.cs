// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Testing.SingleConnectionSessionSourceForSQLiteInMemoryTesting
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Cfg;
using NHibernate;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Testing
{
  public class SingleConnectionSessionSourceForSQLiteInMemoryTesting : SessionSource
  {
    private ISession session;

    public SingleConnectionSessionSourceForSQLiteInMemoryTesting(
      IDictionary<string, string> properties,
      PersistenceModel model)
      : base(properties, model)
    {
    }

    public SingleConnectionSessionSourceForSQLiteInMemoryTesting(FluentConfiguration config)
      : base(config)
    {
    }

    protected void EnsureCurrentSession()
    {
      if (this.session != null)
        return;
      this.session = base.CreateSession();
    }

    public override ISession CreateSession()
    {
      this.EnsureCurrentSession();
      this.session.Clear();
      return this.session;
    }

    public override void BuildSchema() => this.BuildSchema(this.CreateSession());

    public override void BuildSchema(bool script) => this.BuildSchema(this.CreateSession(), script);
  }
}
