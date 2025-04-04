// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessResultOperatorReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessResultOperatorReturn
  {
    public HqlTreeNode TreeNode { get; set; }

    public Action<IQuery, IDictionary<string, Tuple<object, IType>>> AdditionalCriteria { get; set; }

    public LambdaExpression ListTransformer { get; set; }

    public LambdaExpression PostExecuteTransformer { get; set; }

    public HqlBooleanExpression WhereClause { get; set; }

    public HqlGroupBy GroupBy { get; set; }

    public HqlTreeNode AdditionalFrom { get; set; }
  }
}
