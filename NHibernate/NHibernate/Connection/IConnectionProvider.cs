// Decompiled with JetBrains decompiler
// Type: NHibernate.Connection.IConnectionProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Driver;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Connection
{
  public interface IConnectionProvider : IDisposable
  {
    void Configure(IDictionary<string, string> settings);

    void CloseConnection(IDbConnection conn);

    IDriver Driver { get; }

    IDbConnection GetConnection();
  }
}
