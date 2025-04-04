// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.AnsiTrimFunction
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
  public class AnsiTrimFunction : SQLFunctionTemplate, IFunctionGrammar
  {
    public AnsiTrimFunction()
      : base((IType) NHibernateUtil.String, "trim(?1 ?2 ?3 ?4)")
    {
    }

    bool IFunctionGrammar.IsSeparator(string token) => false;

    bool IFunctionGrammar.IsKnownArgument(string token)
    {
      return Regex.IsMatch(token, "LEADING|TRAILING|BOTH|FROM", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }
  }
}
