// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.VarArgsSQLFunction
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
  public class VarArgsSQLFunction : ISQLFunction
  {
    private readonly string begin;
    private readonly string sep;
    private readonly string end;
    private readonly IType returnType;

    public VarArgsSQLFunction(string begin, string sep, string end)
    {
      this.begin = begin;
      this.sep = sep;
      this.end = end;
    }

    public VarArgsSQLFunction(IType type, string begin, string sep, string end)
      : this(begin, sep, end)
    {
      this.returnType = type;
    }

    public virtual IType ReturnType(IType columnType, IMapping mapping)
    {
      return this.returnType != null ? this.returnType : columnType;
    }

    public bool HasArguments => true;

    public bool HasParenthesesIfNoArguments => true;

    public SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder().Add(this.begin);
      for (int index = 0; index < args.Count; ++index)
      {
        sqlStringBuilder.AddObject(args[index]);
        if (index < args.Count - 1)
          sqlStringBuilder.Add(this.sep);
      }
      return sqlStringBuilder.Add(this.end).ToSqlString();
    }
  }
}
