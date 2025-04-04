// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.CallableParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Text.RegularExpressions;

#nullable disable
namespace NHibernate.Engine.Query
{
  public static class CallableParser
  {
    private static readonly Regex functionNameFinder = new Regex("\\{[\\S\\s]*call[\\s]+([\\w\\.]+)[^\\w]");
    private static readonly int NewLineLength = Environment.NewLine.Length;

    public static CallableParser.Detail Parse(string sqlString)
    {
      CallableParser.Detail detail = new CallableParser.Detail();
      detail.IsCallable = sqlString.IndexOf("{") == 0 && sqlString.IndexOf("}") == sqlString.Length - 1 && sqlString.IndexOf("call") > 0;
      if (!detail.IsCallable)
        return detail;
      Match match = CallableParser.functionNameFinder.Match(sqlString);
      if (!match.Success || match.Groups.Count < 2)
        throw new HibernateException("Could not determine function name for callable SQL: " + sqlString);
      detail.FunctionName = match.Groups[1].Value;
      detail.HasReturn = sqlString.IndexOf("call") > 0 && sqlString.IndexOf("?") > 0 && sqlString.IndexOf("=") > 0 && sqlString.IndexOf("?") < sqlString.IndexOf("call") && sqlString.IndexOf("=") < sqlString.IndexOf("call");
      return detail;
    }

    public class Detail
    {
      public bool IsCallable;
      public bool HasReturn;
      public string FunctionName;
    }
  }
}
