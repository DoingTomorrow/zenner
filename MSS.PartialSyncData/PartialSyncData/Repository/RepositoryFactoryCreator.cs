// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Repository.RepositoryFactoryCreator
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using Common.Library.NHibernate.Data;
using MSS.Interfaces;
using System.Configuration;

#nullable disable
namespace MSS.PartialSyncData.Repository
{
  public class RepositoryFactoryCreator : IRepositoryFactoryCreator
  {
    public IRepositoryFactory CreateNewRepositoryFactory()
    {
      return (IRepositoryFactory) new RepositoryFactory(HibernateMultipleDatabasesManager.DataSessionFactory(ConfigurationManager.AppSettings["DatabaseEngine"]).OpenSession());
    }
  }
}
