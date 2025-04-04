// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.AnsiSubstringFunction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Dialect.Function
{
  [Serializable]
  public class AnsiSubstringFunction : ISQLFunction
  {
    public IType ReturnType(IType columnType, IMapping mapping) => (IType) NHibernateUtil.String;

    public bool HasArguments => true;

    public bool HasParenthesesIfNoArguments => true;

    public SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      if (args.Count < 2 || args.Count > 3)
        throw new QueryException("substring(): Incorrect number of parameters (expected 2 or 3, got " + (object) args.Count + ")");
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add("substring(").AddObject(args[0]).Add(" from ").AddObject(args[1]);
      if (args.Count > 2)
        sqlStringBuilder.Add(" for ").AddObject(args[2]);
      sqlStringBuilder.Add(")");
      return sqlStringBuilder.ToSqlString();
    }
  }
}
