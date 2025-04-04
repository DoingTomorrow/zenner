// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.ExpandedQueryExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Hql.Ast.ANTLR.Tree;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  internal class ExpandedQueryExpression : IQueryExpression
  {
    private readonly IASTNode _tree;

    public ExpandedQueryExpression(IQueryExpression queryExpression, IASTNode tree, string key)
    {
      this._tree = tree;
      this.Key = key;
      this.Type = queryExpression.Type;
      this.ParameterDescriptors = queryExpression.ParameterDescriptors;
    }

    public IASTNode Translate(ISessionFactoryImplementor sessionFactory) => this._tree;

    public string Key { get; private set; }

    public Type Type { get; private set; }

    public IList<NamedParameterDescriptor> ParameterDescriptors { get; private set; }
  }
}
