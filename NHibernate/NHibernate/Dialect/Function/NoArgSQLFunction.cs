// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.NoArgSQLFunction
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
  public class NoArgSQLFunction : ISQLFunction
  {
    public NoArgSQLFunction(string name, IType returnType)
      : this(name, returnType, true)
    {
    }

    public NoArgSQLFunction(string name, IType returnType, bool hasParenthesesIfNoArguments)
    {
      this.Name = name;
      this.FunctionReturnType = returnType;
      this.HasParenthesesIfNoArguments = hasParenthesesIfNoArguments;
    }

    public IType FunctionReturnType { get; protected set; }

    public string Name { get; protected set; }

    public IType ReturnType(IType columnType, IMapping mapping) => this.FunctionReturnType;

    public bool HasArguments => false;

    public bool HasParenthesesIfNoArguments { get; protected set; }

    public virtual SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      if (args.Count > 0)
        throw new QueryException("function takes no arguments: " + this.Name);
      return this.HasParenthesesIfNoArguments ? new SqlString(this.Name + "()") : new SqlString(this.Name);
    }
  }
}
