// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.NvlFunction
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
  public class NvlFunction : ISQLFunction
  {
    public IType ReturnType(IType columnType, IMapping mapping) => columnType;

    public bool HasArguments => true;

    public bool HasParenthesesIfNoArguments => true;

    public SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      if (args.Count == 0)
        throw new QueryException("nvl(): Not enough parameters.");
      int index = args.Count - 1;
      object part1 = args[index];
      args.RemoveAt(index);
      if (index == 0)
        return new SqlString(new object[1]{ part1 });
      object part2 = args[index - 1];
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(5).Add("nvl(").AddObject(part2).Add(", ").AddObject(part1).Add(")");
      args[index - 1] = (object) sqlStringBuilder.ToSqlString();
      return this.Render(args, factory);
    }
  }
}
