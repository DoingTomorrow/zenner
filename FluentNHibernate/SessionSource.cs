// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.SessionSource
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace FluentNHibernate
{
  public class SessionSource : ISessionSource
  {
    private NHibernate.Dialect.Dialect dialect;

    public Configuration Configuration { get; private set; }

    public ISessionFactory SessionFactory { get; private set; }

    public SessionSource(PersistenceModel model)
    {
      this.Initialize(new Configuration().Configure(), model);
    }

    public SessionSource(IDictionary<string, string> properties, PersistenceModel model)
    {
      this.Initialize(new Configuration().AddProperties(properties), model);
    }

    public SessionSource(FluentConfiguration config)
    {
      this.Configuration = config.Configuration;
      this.SessionFactory = config.BuildSessionFactory();
      this.dialect = NHibernate.Dialect.Dialect.GetDialect(this.Configuration.Properties);
    }

    protected void Initialize(Configuration nhibernateConfig, PersistenceModel model)
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model), "Model cannot be null");
      this.Configuration = nhibernateConfig;
      model.Configure(this.Configuration);
      this.SessionFactory = this.Configuration.BuildSessionFactory();
      this.dialect = NHibernate.Dialect.Dialect.GetDialect(this.Configuration.Properties);
    }

    public virtual ISession CreateSession() => this.SessionFactory.OpenSession();

    public virtual void BuildSchema()
    {
      using (ISession session = this.CreateSession())
        this.BuildSchema(session);
    }

    public virtual void BuildSchema(bool script)
    {
      using (ISession session = this.CreateSession())
        this.BuildSchema(session, script);
    }

    public void BuildSchema(ISession session) => this.BuildSchema(session, false);

    public void BuildSchema(ISession session, bool script)
    {
      new SchemaExport(this.Configuration).Execute(script, true, false, session.Connection, (TextWriter) null);
    }
  }
}
