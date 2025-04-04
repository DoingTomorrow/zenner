// Decompiled with JetBrains decompiler
// Type: NHibernate.Exceptions.SqlStateExtracter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data.Common;

#nullable disable
namespace NHibernate.Exceptions
{
  public abstract class SqlStateExtracter
  {
    public int ExtractErrorCode(DbException sqle)
    {
      int singleErrorCode = this.ExtractSingleErrorCode(sqle);
      for (Exception innerException = sqle.InnerException; singleErrorCode == 0 && innerException != null; innerException = sqle.InnerException)
      {
        if (innerException is DbException)
          singleErrorCode = this.ExtractSingleErrorCode(sqle);
      }
      return singleErrorCode;
    }

    public string ExtractSqlState(DbException sqle)
    {
      string singleSqlState = this.ExtractSingleSqlState(sqle);
      for (Exception innerException = sqle.InnerException; singleSqlState.Length == 0 && innerException != null; innerException = sqle.InnerException)
      {
        if (innerException is DbException)
          singleSqlState = this.ExtractSingleSqlState(sqle);
      }
      return singleSqlState;
    }

    public abstract int ExtractSingleErrorCode(DbException sqle);

    public abstract string ExtractSingleSqlState(DbException sqle);
  }
}
