// Decompiled with JetBrains decompiler
// Type: MSS.Data.Repository.RepositoryFactoryCreator
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using Common.Library.NHibernate.Data;
using MSS.Business.Utils;
using MSS.Interfaces;
using NHibernate.Event;
using System;
using System.Configuration;

#nullable disable
namespace MSS.Data.Repository
{
  public class RepositoryFactoryCreator : IRepositoryFactoryCreator
  {
    public IRepositoryFactory CreateNewRepositoryFactory()
    {
      return (IRepositoryFactory) new RepositoryFactory(HibernateMultipleDatabasesManager.DataSessionFactory(ConfigurationManager.AppSettings["DatabaseEngine"]).OpenSession());
    }

    public IRepositoryFactory CreateNewPartialSyncRepositoryFactory()
    {
      HibernateMultipleDatabasesManager.FluentConfiguration.ExposeConfiguration((Action<NHibernate.Cfg.Configuration>) (cfg =>
      {
        cfg.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[1]
        {
          (IPreInsertEventListener) new NHibernateEventListener()
        };
        cfg.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[1]
        {
          (IPreUpdateEventListener) new NHibernateEventListener()
        };
        cfg.EventListeners.PostInsertEventListeners = new IPostInsertEventListener[1]
        {
          (IPostInsertEventListener) new NHibernateEventListener()
        };
        cfg.EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[1]
        {
          (IPostUpdateEventListener) new NHibernateEventListener()
        };
        cfg.EventListeners.PostDeleteEventListeners = new IPostDeleteEventListener[1]
        {
          (IPostDeleteEventListener) new NHibernateEventListener()
        };
      }));
      HibernateMultipleDatabasesManager.FluentConfiguration.BuildSessionFactory();
      return (IRepositoryFactory) new RepositoryFactory(HibernateMultipleDatabasesManager.DataSessionFactory("PartialSyncSQLiteDatabase").OpenSession());
    }
  }
}
