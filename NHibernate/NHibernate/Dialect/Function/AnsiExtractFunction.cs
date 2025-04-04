// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.AnsiExtractFunction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;
using System.Text.RegularExpressions;

#nullable disable
namespace NHibernate.Dialect.Function
{
  [Serializable]
  public class AnsiExtractFunction : SQLFunctionTemplate, IFunctionGrammar
  {
    public AnsiExtractFunction()
      : base((IType) NHibernateUtil.Int32, "extract(?1 ?2 ?3)")
    {
    }

    bool IFunctionGrammar.IsSeparator(string token) => false;

    bool IFunctionGrammar.IsKnownArgument(string token)
    {
      return Regex.IsMatch(token, "YEAR|MONTH|DAY|HOUR|MINUTE|SECOND|TIMEZONE_HOUR|TIMEZONE_MINUTE|FROM", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }
  }
}
