// Decompiled with JetBrains decompiler
// Type: NHibernate.Exceptions.SQLStateConverter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Exceptions
{
  public class SQLStateConverter : ISQLExceptionConverter
  {
    public SQLStateConverter(IViolatedConstraintNameExtracter extracter)
    {
    }

    public Exception Convert(AdoExceptionContextInfo exceptionInfo)
    {
      return (Exception) SQLStateConverter.HandledNonSpecificException(exceptionInfo.SqlException, exceptionInfo.Message, exceptionInfo.Sql);
    }

    public static ADOException HandledNonSpecificException(
      Exception sqlException,
      string message,
      string sql)
    {
      return (ADOException) new GenericADOException(message, sqlException, sql);
    }
  }
}
