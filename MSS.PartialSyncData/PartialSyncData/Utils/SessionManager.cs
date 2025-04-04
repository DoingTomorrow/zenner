// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Utils.SessionManager
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using Common.Library.NHibernate.Data;
using MSS.Interfaces;
using NHibernate;
using System.Configuration;

#nullable disable
namespace MSS.PartialSyncData.Utils
{
  public class SessionManager : ISessionManager
  {
    private readonly string _connectionIdentifier;

    public SessionManager()
    {
      this._connectionIdentifier = ConfigurationManager.AppSettings["DatabaseEngine"];
    }

    public ISession OpenSession()
    {
      return HibernateMultipleDatabasesManager.DataSessionFactory(this._connectionIdentifier).OpenSession();
    }

    public void CloseSession(ISession session) => session.Close();
  }
}
