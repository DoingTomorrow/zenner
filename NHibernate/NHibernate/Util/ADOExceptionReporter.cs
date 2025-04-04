// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.ADOExceptionReporter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Util
{
  public class ADOExceptionReporter
  {
    public const string DefaultExceptionMsg = "SQL Exception";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ADOExceptionReporter));

    private ADOExceptionReporter()
    {
    }

    public static void LogExceptions(Exception ex)
    {
      ADOExceptionReporter.LogExceptions(ex, (string) null);
    }

    public static void LogExceptions(Exception ex, string message)
    {
      if (!ADOExceptionReporter.log.IsErrorEnabled)
        return;
      if (ADOExceptionReporter.log.IsDebugEnabled)
      {
        message = StringHelper.IsNotEmpty(message) ? message : "SQL Exception";
        ADOExceptionReporter.log.Debug((object) message, ex);
      }
      for (; ex != null; ex = ex.InnerException)
      {
        ADOExceptionReporter.log.Warn((object) ex);
        ADOExceptionReporter.log.Error((object) ex.Message);
      }
    }
  }
}
