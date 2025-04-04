// Decompiled with JetBrains decompiler
// Type: MSS.Data.Utils.SessionManager
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using Common.Library.NHibernate.Data;
using MSS.Interfaces;
using NHibernate;
using System.Configuration;

#nullable disable
namespace MSS.Data.Utils
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
