// Decompiled with JetBrains decompiler
// Type: MSS.DIConfiguration.NHibernateModule
// Assembly: MSS.DIConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FF318A7F-B5DB-4F93-8026-33B4E3BCEF3D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DIConfiguration.dll

using Common.Library.NHibernate.Data;
using MSS.Interfaces;
using NHibernate;
using Ninject.Activation;
using Ninject.Modules;
using System;
using System.Configuration;

#nullable disable
namespace MSS.DIConfiguration
{
  public class NHibernateModule : NinjectModule
  {
    public override void Load()
    {
      this.Bind<ISession>().ToMethod((Func<IContext, ISession>) (ctx => HibernateMultipleDatabasesManager.DataSessionFactory(ConfigurationManager.AppSettings["DatabaseEngine"]).OpenSession())).InScope((Func<IContext, object>) (ctx =>
      {
        IRequest request = ctx.Request;
        if (typeof (IViewModel).IsAssignableFrom(request.Service))
          return (object) request;
        while ((request = request.ParentRequest) != null)
        {
          if (typeof (IViewModel).IsAssignableFrom(request.Service))
            return (object) request;
        }
        return new object();
      }));
    }
  }
}
