// Decompiled with JetBrains decompiler
// Type: NHibernate.Connection.UserSuppliedConnectionProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Connection
{
  public class UserSuppliedConnectionProvider : ConnectionProvider
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (UserSuppliedConnectionProvider));

    public override void CloseConnection(IDbConnection conn)
    {
      throw new InvalidOperationException("The User is responsible for closing ADO.NET connection - not NHibernate.");
    }

    public override IDbConnection GetConnection()
    {
      throw new InvalidOperationException("The user must provide an ADO.NET connection - NHibernate is not creating it.");
    }

    public override void Configure(IDictionary<string, string> settings)
    {
      UserSuppliedConnectionProvider.log.Warn((object) "No connection properties specified - the user must supply an ADO.NET connection");
      this.ConfigureDriver(settings);
    }
  }
}
