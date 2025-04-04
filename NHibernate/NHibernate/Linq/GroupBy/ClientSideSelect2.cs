// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupBy.ClientSideSelect2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.ResultOperators;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.GroupBy
{
  internal class ClientSideSelect2 : ClientSideTransformOperator
  {
    public LambdaExpression SelectClause { get; private set; }

    public ClientSideSelect2(LambdaExpression selectClause) => this.SelectClause = selectClause;
  }
}
