// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.PartialSyncProviders.BasePartialSyncProvider`1
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using Common.Library.NHibernate.Data;
using MSS.Interfaces;
using NHibernate;
using System.Configuration;

#nullable disable
namespace MSS.PartialSync.PartialSyncProviders
{
  public abstract class BasePartialSyncProvider<TEntity> where TEntity : IPartialSynchronizableEntity
  {
    public virtual ISessionFactory CreateRemoteSession()
    {
      HibernateMultipleDatabasesManager.Initialize(ConfigurationManager.AppSettings["MSSQLDatabase"]);
      return HibernateMultipleDatabasesManager.DataSessionFactory(ConfigurationManager.AppSettings["MSSQLDatabase"]);
    }
  }
}
