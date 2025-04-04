// Decompiled with JetBrains decompiler
// Type: MSS.DIConfiguration.RepositoryModule
// Assembly: MSS.DIConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FF318A7F-B5DB-4F93-8026-33B4E3BCEF3D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DIConfiguration.dll

using MSS.Data.Repository;
using MSS.Data.Utils;
using MSS.Interfaces;
using Ninject.Modules;
using System;

#nullable disable
namespace MSS.DIConfiguration
{
  public class RepositoryModule : NinjectModule
  {
    public override void Load()
    {
      this.Bind(new Type[1]{ typeof (IRepository<>) }).To(typeof (MSS.Data.Repository.Repository<>));
      this.Bind(new Type[1]{ typeof (IUserRepository) }).To(typeof (UserRepository));
      this.Bind(new Type[1]{ typeof (IRepositoryFactory) }).To(typeof (RepositoryFactory));
      this.Bind(new Type[1]{ typeof (IJobRepository) }).To(typeof (JobsRepository));
      this.Bind(new Type[1]{ typeof (ISessionManager) }).To(typeof (SessionManager));
      this.Bind(new Type[1]
      {
        typeof (IRepositoryFactoryCreator)
      }).To(typeof (RepositoryFactoryCreator));
    }
  }
}
