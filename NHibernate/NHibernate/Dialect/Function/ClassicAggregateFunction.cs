// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.ClassicAggregateFunction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Dialect.Function
{
  [Serializable]
  public class ClassicAggregateFunction : ISQLFunction, IFunctionGrammar
  {
    private IType returnType;
    private readonly string name;
    protected readonly bool acceptAsterisk;

    public ClassicAggregateFunction(string name, bool acceptAsterisk)
    {
      this.name = name;
      this.acceptAsterisk = acceptAsterisk;
    }

    public ClassicAggregateFunction(string name, bool acceptAsterisk, IType typeValue)
      : this(name, acceptAsterisk)
    {
      this.returnType = typeValue;
    }

    public virtual IType ReturnType(IType columnType, IMapping mapping)
    {
      return this.returnType ?? columnType;
    }

    public bool HasArguments => true;

    public bool HasParenthesesIfNoArguments => true;

    public SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      if (args.Count < 1 || args.Count > 2)
        throw new QueryException(string.Format("Aggregate {0}(): Not enough parameters (attended from 1 to 2).", (object) this.name));
      if ("*".Equals(args[args.Count - 1]) && !this.acceptAsterisk)
        throw new QueryException(string.Format("Aggregate {0}(): invalid argument '*'.", (object) this.name));
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add(this.name).Add("(");
      if (args.Count > 1)
      {
        object part = args[0];
        if (!StringHelper.EqualsCaseInsensitive("distinct", part.ToString()) && !StringHelper.EqualsCaseInsensitive("all", part.ToString()))
          throw new QueryException(string.Format("Aggregate {0}(): token unknow {1}.", (object) this.name, part));
        sqlStringBuilder.AddObject(part).Add(" ");
      }
      sqlStringBuilder.AddObject(args[args.Count - 1]).Add(")");
      return sqlStringBuilder.ToSqlString();
    }

    public override string ToString() => this.name;

    bool IFunctionGrammar.IsSeparator(string token) => false;

    bool IFunctionGrammar.IsKnownArgument(string token)
    {
      return "distinct".Equals(token.ToLowerInvariant()) || "all".Equals(token.ToLowerInvariant());
    }
  }
}
