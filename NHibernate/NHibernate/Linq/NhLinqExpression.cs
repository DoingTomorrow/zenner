// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.NhLinqExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Linq.Visitors;
using NHibernate.Param;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public class NhLinqExpression : IQueryExpression
  {
    internal Expression _expression;
    internal IDictionary<ConstantExpression, NamedParameter> _constantToParameterMap;

    public string Key { get; private set; }

    public System.Type Type { get; private set; }

    public IList<NamedParameterDescriptor> ParameterDescriptors { get; private set; }

    public NhLinqExpressionReturnType ReturnType { get; private set; }

    public IDictionary<string, Tuple<object, IType>> ParameterValuesByName { get; private set; }

    public ExpressionToHqlTranslationResults ExpressionToHqlTranslationResults { get; private set; }

    public NhLinqExpression(Expression expression, ISessionFactory sessionFactory)
    {
      this._expression = NhPartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees(expression);
      LinqLogging.LogExpression("Expression (partially evaluated)", this._expression);
      this._constantToParameterMap = ExpressionParameterVisitor.Visit(this._expression, sessionFactory);
      this.ParameterValuesByName = (IDictionary<string, Tuple<object, IType>>) this._constantToParameterMap.Values.ToDictionary<NamedParameter, string, Tuple<object, IType>>((Func<NamedParameter, string>) (p => p.Name), (Func<NamedParameter, Tuple<object, IType>>) (p => new Tuple<object, IType>()
      {
        First = p.Value,
        Second = p.Type
      }));
      this.Key = ExpressionKeyVisitor.Visit(this._expression, this._constantToParameterMap);
      this.Type = this._expression.Type;
      this.ReturnType = NhLinqExpressionReturnType.Scalar;
      if (!typeof (IQueryable).IsAssignableFrom(this.Type))
        return;
      this.Type = this.Type.GetGenericArguments()[0];
      this.ReturnType = NhLinqExpressionReturnType.Sequence;
    }

    public IASTNode Translate(ISessionFactoryImplementor sessionFactory)
    {
      List<NamedParameterDescriptor> requiredHqlParameters = new List<NamedParameterDescriptor>();
      QuerySourceNamer querySourceNamer = new QuerySourceNamer();
      this.ExpressionToHqlTranslationResults = QueryModelVisitor.GenerateHqlQuery(NhRelinqQueryParser.Parse(this._expression), new VisitorParameters(sessionFactory, this._constantToParameterMap, requiredHqlParameters, querySourceNamer), true);
      this.ParameterDescriptors = (IList<NamedParameterDescriptor>) requiredHqlParameters.AsReadOnly();
      return this.ExpressionToHqlTranslationResults.Statement.AstNode;
    }

    internal void CopyExpressionTranslation(NhLinqExpression other)
    {
      this.ExpressionToHqlTranslationResults = other.ExpressionToHqlTranslationResults;
      this.ParameterDescriptors = other.ParameterDescriptors;
    }
  }
}
