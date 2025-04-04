// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.NhJoinClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class NhJoinClause : AdditionalFromClause
  {
    public bool IsInner { get; private set; }

    public void MakeInner() => this.IsInner = true;

    public NhJoinClause(string itemName, Type itemType, Expression fromExpression)
      : base(itemName, itemType, fromExpression)
    {
      this.IsInner = false;
    }
  }
}
