// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.OracleLiteDataClientDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Driver
{
  public class OracleLiteDataClientDriver : ReflectionBasedDriver, IEmbeddedBatcherFactoryProvider
  {
    public OracleLiteDataClientDriver()
      : base("Oracle.DataAccess.Lite_w32", "Oracle.DataAccess.Lite.OracleConnection", "Oracle.DataAccess.Lite.OracleCommand")
    {
    }

    public override bool UseNamedPrefixInSql => false;

    public override bool UseNamedPrefixInParameter => false;

    public override string NamedPrefix => string.Empty;

    protected override void InitializeParameter(
      IDbDataParameter dbParam,
      string name,
      SqlType sqlType)
    {
      if (sqlType.DbType == DbType.Boolean)
        sqlType = SqlTypeFactory.Int16;
      base.InitializeParameter(dbParam, name, sqlType);
    }

    Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass
    {
      get => typeof (OracleDataClientBatchingBatcherFactory);
    }
  }
}
