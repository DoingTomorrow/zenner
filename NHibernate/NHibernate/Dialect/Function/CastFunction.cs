// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.CastFunction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Dialect.Function
{
  [Serializable]
  public class CastFunction : ISQLFunction, IFunctionGrammar
  {
    public IType ReturnType(IType columnType, IMapping mapping) => columnType;

    public bool HasArguments => true;

    public bool HasParenthesesIfNoArguments => true;

    public SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      string typeName = args.Count == 2 ? args[1].ToString() : throw new QueryException("cast() requires two arguments");
      SqlType[] sqlTypeArray = (TypeFactory.HeuristicType(typeName) ?? throw new QueryException(string.Format("invalid Hibernate type for cast(): type {0} not found", (object) typeName))).SqlTypes((IMapping) factory);
      if (sqlTypeArray.Length != 1)
        throw new QueryException("invalid NHibernate type for cast(), was:" + typeName);
      string str = factory.Dialect.GetCastTypeName(sqlTypeArray[0]) ?? typeName;
      return this.CastingIsRequired(str) ? new SqlStringBuilder().Add("cast(").AddObject(args[0]).Add(" as ").Add(str).Add(")").ToSqlString() : new SqlStringBuilder().Add("(").AddObject(args[0]).Add(")").ToSqlString();
    }

    protected virtual bool CastingIsRequired(string sqlType) => true;

    bool IFunctionGrammar.IsSeparator(string token)
    {
      return "as".Equals(token, StringComparison.InvariantCultureIgnoreCase);
    }

    bool IFunctionGrammar.IsKnownArgument(string token) => false;
  }
}
