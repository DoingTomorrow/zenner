// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlStringBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class SqlStringBuilder : ISqlStringBuilder
  {
    private List<object> sqlParts;
    private SqlStringBuilder.AddingSqlStringVisitor addingVisitor;

    private SqlStringBuilder.AddingSqlStringVisitor AddingVisitor
    {
      get
      {
        if (this.addingVisitor == null)
          this.addingVisitor = new SqlStringBuilder.AddingSqlStringVisitor(this);
        return this.addingVisitor;
      }
    }

    public SqlStringBuilder()
      : this(16)
    {
    }

    public SqlStringBuilder(int partsCapacity) => this.sqlParts = new List<object>(partsCapacity);

    public SqlStringBuilder(SqlString sqlString)
    {
      this.sqlParts = new List<object>(sqlString.Count);
      this.Add(sqlString);
    }

    public SqlStringBuilder Add(string sql)
    {
      if (StringHelper.IsNotEmpty(sql))
        this.sqlParts.Add((object) sql);
      return this;
    }

    public SqlStringBuilder Add(Parameter parameter)
    {
      if (parameter != (Parameter) null)
        this.sqlParts.Add((object) parameter);
      return this;
    }

    public SqlStringBuilder AddParameter() => this.Add(Parameter.Placeholder);

    public SqlStringBuilder AddObject(object part)
    {
      if (part == null)
        return this;
      Parameter parameter = part as Parameter;
      if (parameter != (Parameter) null)
        return this.Add(parameter);
      string str = part as string;
      if (StringHelper.IsNotEmpty(str))
        return this.Add(str);
      SqlString sqlString = part as SqlString;
      if (StringHelper.IsNotEmpty(sqlString))
        return this.Add(sqlString);
      if (parameter == (Parameter) null && str == null && sqlString == null)
        throw new ArgumentException("Part was not a Parameter, String, or SqlString.");
      return this;
    }

    public SqlStringBuilder Add(SqlString sqlString)
    {
      sqlString.Visit((ISqlStringVisitor) this.AddingVisitor);
      return this;
    }

    public SqlStringBuilder Add(SqlString sqlString, string prefix, string op, string postfix)
    {
      return this.Add(new SqlString[1]{ sqlString }, prefix, op, postfix, false);
    }

    public SqlStringBuilder Add(SqlString[] sqlStrings, string prefix, string op, string postfix)
    {
      return this.Add(sqlStrings, prefix, op, postfix, true);
    }

    public SqlStringBuilder Add(
      SqlString[] sqlStrings,
      string prefix,
      string op,
      string postfix,
      bool wrapStatement)
    {
      if (StringHelper.IsNotEmpty(prefix))
        this.sqlParts.Add((object) prefix);
      bool flag = false;
      foreach (SqlString sqlString in sqlStrings)
      {
        if (sqlString.Count != 0)
        {
          if (flag)
            this.sqlParts.Add((object) (" " + op + " "));
          flag = true;
          if (wrapStatement)
            this.sqlParts.Add((object) "(");
          this.Add(sqlString);
          if (wrapStatement)
            this.sqlParts.Add((object) ")");
        }
      }
      if (postfix != null)
        this.sqlParts.Add((object) postfix);
      return this;
    }

    public int Count => this.sqlParts.Count;

    public object this[int index]
    {
      get => this.sqlParts[index];
      set => this.sqlParts[index] = value;
    }

    public SqlStringBuilder Insert(int index, string sql)
    {
      this.sqlParts.Insert(index, (object) sql);
      return this;
    }

    public SqlStringBuilder Insert(int index, Parameter param)
    {
      this.sqlParts.Insert(index, (object) param);
      return this;
    }

    public SqlStringBuilder RemoveAt(int index)
    {
      this.sqlParts.RemoveAt(index);
      return this;
    }

    public SqlString ToSqlString() => new SqlString(this.sqlParts.ToArray());

    public override string ToString() => this.ToSqlString().ToString();

    public void Clear() => this.sqlParts.Clear();

    private class AddingSqlStringVisitor : ISqlStringVisitor
    {
      private SqlStringBuilder parent;

      public AddingSqlStringVisitor(SqlStringBuilder parent) => this.parent = parent;

      public void String(string text) => this.parent.Add(text);

      public void String(SqlString sqlString) => this.parent.Add(sqlString);

      public void Parameter(Parameter parameter) => this.parent.Add(parameter);
    }
  }
}
