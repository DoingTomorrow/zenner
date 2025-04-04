// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.ExecuteUpdateResultCheckStyle
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using System;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public class ExecuteUpdateResultCheckStyle
  {
    public static readonly ExecuteUpdateResultCheckStyle None = new ExecuteUpdateResultCheckStyle("none");
    public static readonly ExecuteUpdateResultCheckStyle Count = new ExecuteUpdateResultCheckStyle("rowcount");
    private readonly string name;

    private ExecuteUpdateResultCheckStyle(string name) => this.name = name;

    public static ExecuteUpdateResultCheckStyle Parse(string name)
    {
      switch (name)
      {
        case "none":
          return ExecuteUpdateResultCheckStyle.None;
        case "rowcount":
          return ExecuteUpdateResultCheckStyle.Count;
        default:
          return (ExecuteUpdateResultCheckStyle) null;
      }
    }

    public static ExecuteUpdateResultCheckStyle DetermineDefault(SqlString customSql, bool callable)
    {
      return ExecuteUpdateResultCheckStyle.Count;
    }

    public override bool Equals(object obj)
    {
      return obj is ExecuteUpdateResultCheckStyle resultCheckStyle && this.name == resultCheckStyle.name;
    }

    public override int GetHashCode() => this.name.GetHashCode();

    public override string ToString() => this.name;
  }
}
