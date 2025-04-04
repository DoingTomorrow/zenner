// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.IRepositoryFactory
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using NHibernate;
using System;

#nullable disable
namespace MSS.Interfaces
{
  public interface IRepositoryFactory : IDisposable
  {
    IRepository<T> GetRepository<T>() where T : class;

    IUserRepository GetUserRepository();

    IStructureNodeRepository GetStructureNodeRepository();

    IJobRepository GetJobRepository();

    ISession GetSession();

    IReadingValuesRepository GetReadingValuesRepository();

    [Obsolete("This shouldn't be used because it breaks the pattern. Each viewModel has a session which is kept in RepositoryFactory.")]
    void SetSession(ISession session);

    ISynchronizationRepository GetSynchronizationRepository();

    IRepositoryFactory GetRepositoryFactoryInstance();
  }
}
