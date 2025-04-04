// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.ErrorCounter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  internal class ErrorCounter : IParseErrorHandler, IErrorReporter
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ErrorCounter));
    private static readonly IInternalLogger hqlLog = LoggerProvider.LoggerFor("NHibernate.Hql.Parser");
    private readonly List<string> _errorList = new List<string>();
    private readonly List<string> _warningList = new List<string>();
    private readonly List<RecognitionException> _recognitionExceptions = new List<RecognitionException>();

    public void ReportError(RecognitionException e)
    {
      this.ReportError(e.ToString());
      this._recognitionExceptions.Add(e);
      if (!ErrorCounter.log.IsDebugEnabled)
        return;
      ErrorCounter.log.Debug((object) e.ToString(), (Exception) e);
    }

    public void ReportError(string message)
    {
      ErrorCounter.hqlLog.Error((object) message);
      this._errorList.Add(message);
    }

    public int GetErrorCount() => this._errorList.Count;

    public void ReportWarning(string message)
    {
      ErrorCounter.hqlLog.Debug((object) message);
      this._warningList.Add(message);
    }

    private string GetErrorString()
    {
      bool flag = true;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string error in this._errorList)
      {
        stringBuilder.Append(error);
        if (!flag)
          stringBuilder.Append('\n');
        flag = false;
      }
      return stringBuilder.ToString();
    }

    public void ThrowQueryException()
    {
      if (this.GetErrorCount() > 0)
      {
        if (this._recognitionExceptions.Count > 0)
          throw QuerySyntaxException.Convert(this._recognitionExceptions[0]);
        throw new QueryException(this.GetErrorString());
      }
      if (!ErrorCounter.log.IsDebugEnabled)
        return;
      ErrorCounter.log.Debug((object) "throwQueryException() : no errors");
    }
  }
}
