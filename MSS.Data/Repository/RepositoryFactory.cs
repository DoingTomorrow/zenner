// Decompiled with JetBrains decompiler
// Type: MSS.Data.Repository.RepositoryFactory
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using Common.Library.NHibernate.Data;
using MSS.Interfaces;
using NHibernate;
using System;
using System.Configuration;

#nullable disable
namespace MSS.Data.Repository
{
  public class RepositoryFactory : IRepositoryFactory, IDisposable
  {
    protected ISession _session;

    public RepositoryFactory(ISession nhSession) => this._session = nhSession;

    public IRepository<T> GetRepository<T>() where T : class
    {
      return (IRepository<T>) new MSS.Data.Repository.Repository<T>(this._session);
    }

    public IUserRepository GetUserRepository()
    {
      return (IUserRepository) new UserRepository(this._session);
    }

    public IJobRepository GetJobRepository() => (IJobRepository) new JobsRepository(this._session);

    public IStructureNodeRepository GetStructureNodeRepository()
    {
      return (IStructureNodeRepository) new StructuresNodeRepository(this._session);
    }

    public IReadingValuesRepository GetReadingValuesRepository()
    {
      return (IReadingValuesRepository) new ReadingValuesRepository(this._session);
    }

    public void SetSession(ISession session) => this._session = session;

    public ISynchronizationRepository GetSynchronizationRepository()
    {
      return (ISynchronizationRepository) new SynchronizationRepository(this._session);
    }

    public IRepositoryFactory GetRepositoryFactoryInstance()
    {
      return (IRepositoryFactory) new RepositoryFactory(HibernateMultipleDatabasesManager.DataSessionFactory(ConfigurationManager.AppSettings["DatabaseEngine"]).OpenSession());
    }

    public ISession GetSession() => this._session;

    public void Dispose() => this._session?.Dispose();
  }
}
