// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.PositionSubstringFunction
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
  public class PositionSubstringFunction : ISQLFunction
  {
    public IType ReturnType(IType columnType, IMapping mapping) => (IType) NHibernateUtil.Int32;

    public bool HasArguments => true;

    public bool HasParenthesesIfNoArguments => true;

    public SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      if (args.Count < 2)
        throw new QueryException("position(): Not enough parameters (attended from 2 to 3).");
      bool flag = args.Count > 2;
      object part1 = args[0];
      object part2 = args[1];
      object part3 = flag ? args[2] : (object) null;
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      if (flag)
        sqlStringBuilder.Add("(");
      sqlStringBuilder.Add("position(").AddObject(part1).Add(" in ");
      if (flag)
        sqlStringBuilder.Add("substring(");
      sqlStringBuilder.AddObject(part2);
      if (flag)
        sqlStringBuilder.Add(", ").AddObject(part3).Add(")");
      sqlStringBuilder.Add(")");
      if (flag)
        sqlStringBuilder.Add("+").AddObject(part3).Add("-1)");
      return sqlStringBuilder.ToSqlString();
    }
  }
}
