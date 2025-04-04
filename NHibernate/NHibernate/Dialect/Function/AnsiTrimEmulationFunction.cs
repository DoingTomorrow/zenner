// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.AnsiTrimEmulationFunction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace NHibernate.Dialect.Function
{
  [Serializable]
  public class AnsiTrimEmulationFunction : ISQLFunction, IFunctionGrammar
  {
    private static readonly ISQLFunction LeadingSpaceTrim = (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "ltrim( ?1 )");
    private static readonly ISQLFunction TrailingSpaceTrim = (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "rtrim( ?1 )");
    private static readonly ISQLFunction BothSpaceTrim = (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "ltrim( rtrim( ?1 ) )");
    private static readonly ISQLFunction BothSpaceTrimFrom = (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "ltrim( rtrim( ?2 ) )");
    private static readonly ISQLFunction LeadingTrim = (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "replace( replace( ltrim( replace( replace( ?1, ' ', '${space}$' ), ?2, ' ' ) ), ' ', ?2 ), '${space}$', ' ' )");
    private static readonly ISQLFunction TrailingTrim = (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "replace( replace( rtrim( replace( replace( ?1, ' ', '${space}$' ), ?2, ' ' ) ), ' ', ?2 ), '${space}$', ' ' )");
    private static readonly ISQLFunction BothTrim = (ISQLFunction) new SQLFunctionTemplate((IType) NHibernateUtil.String, "replace( replace( ltrim( rtrim( replace( replace( ?1, ' ', '${space}$' ), ?2, ' ' ) ) ), ' ', ?2 ), '${space}$', ' ' )");

    public IType ReturnType(IType columnType, IMapping mapping) => (IType) NHibernateUtil.String;

    public bool HasArguments => true;

    public bool HasParenthesesIfNoArguments => true;

    public SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      string b = args.Count >= 1 && args.Count <= 4 ? args[0].ToString() : throw new QueryException("function takes between 1 and 4 arguments");
      if (args.Count == 1)
        return AnsiTrimEmulationFunction.BothSpaceTrim.Render(args, factory);
      if (StringHelper.EqualsCaseInsensitive("from", b))
        return AnsiTrimEmulationFunction.BothSpaceTrimFrom.Render(args, factory);
      bool flag1 = true;
      bool flag2 = true;
      int index = 1;
      if (StringHelper.EqualsCaseInsensitive("leading", b))
        flag2 = false;
      else if (StringHelper.EqualsCaseInsensitive("trailing", b))
        flag1 = false;
      else if (!StringHelper.EqualsCaseInsensitive("both", b))
        index = 0;
      object obj1 = args[index];
      object obj2;
      object obj3;
      if (StringHelper.EqualsCaseInsensitive("from", obj1.ToString()))
      {
        obj2 = (object) "' '";
        obj3 = args[index + 1];
      }
      else if (index + 1 >= args.Count)
      {
        obj2 = (object) "' '";
        obj3 = obj1;
      }
      else
      {
        obj2 = obj1;
        obj3 = !StringHelper.EqualsCaseInsensitive("from", args[index + 1].ToString()) ? args[index + 1] : args[index + 2];
      }
      IList args1 = (IList) new List<object>()
      {
        obj3,
        obj2
      };
      if (obj2.Equals((object) "' '"))
      {
        if (flag1 && flag2)
          return AnsiTrimEmulationFunction.BothSpaceTrim.Render(args1, factory);
        return flag1 ? AnsiTrimEmulationFunction.LeadingSpaceTrim.Render(args1, factory) : AnsiTrimEmulationFunction.TrailingSpaceTrim.Render(args1, factory);
      }
      if (flag1 && flag2)
        return AnsiTrimEmulationFunction.BothTrim.Render(args1, factory);
      return flag1 ? AnsiTrimEmulationFunction.LeadingTrim.Render(args1, factory) : AnsiTrimEmulationFunction.TrailingTrim.Render(args1, factory);
    }

    bool IFunctionGrammar.IsSeparator(string token) => false;

    bool IFunctionGrammar.IsKnownArgument(string token)
    {
      return Regex.IsMatch(token, "LEADING|TRAILING|BOTH|FROM", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }
  }
}
