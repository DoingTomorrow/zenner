// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.StandardSQLFunction
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
  public class StandardSQLFunction : ISQLFunction
  {
    private IType returnType;
    protected readonly string name;

    public StandardSQLFunction(string name) => this.name = name;

    public StandardSQLFunction(string name, IType typeValue)
      : this(name)
    {
      this.returnType = typeValue;
    }

    public virtual IType ReturnType(IType columnType, IMapping mapping)
    {
      return this.returnType ?? columnType;
    }

    public bool HasArguments => true;

    public bool HasParenthesesIfNoArguments => true;

    public virtual SqlString Render(IList args, ISessionFactoryImplementor factory)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add(this.name).Add("(");
      for (int index = 0; index < args.Count; ++index)
      {
        object part = args[index];
        if ((object) (part as Parameter) != null || part is SqlString)
          sqlStringBuilder.AddObject(part);
        else
          sqlStringBuilder.Add(part.ToString());
        if (index < args.Count - 1)
          sqlStringBuilder.Add(", ");
      }
      return sqlStringBuilder.Add(")").ToSqlString();
    }

    public override string ToString() => this.name;
  }
}
