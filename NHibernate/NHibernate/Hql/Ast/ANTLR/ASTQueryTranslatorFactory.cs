// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.ASTQueryTranslatorFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  public class ASTQueryTranslatorFactory : IQueryTranslatorFactory2, IQueryTranslatorFactory
  {
    public IQueryTranslator[] CreateQueryTranslators(
      string queryString,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> filters,
      ISessionFactoryImplementor factory)
    {
      return ASTQueryTranslatorFactory.CreateQueryTranslators(new HqlParseEngine(queryString, collectionRole != null, factory).Parse(), queryString, collectionRole, shallow, filters, factory);
    }

    public IQueryTranslator[] CreateQueryTranslators(
      string queryIdentifier,
      IQueryExpression queryExpression,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> filters,
      ISessionFactoryImplementor factory)
    {
      return ASTQueryTranslatorFactory.CreateQueryTranslators(queryExpression.Translate(factory), queryIdentifier, collectionRole, shallow, filters, factory);
    }

    private static IQueryTranslator[] CreateQueryTranslators(
      IASTNode ast,
      string queryIdentifier,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> filters,
      ISessionFactoryImplementor factory)
    {
      QueryTranslatorImpl[] array = ((IEnumerable<IASTNode>) AstPolymorphicProcessor.Process(ast, factory)).Select<IASTNode, QueryTranslatorImpl>((Func<IASTNode, QueryTranslatorImpl>) (hql => new QueryTranslatorImpl(queryIdentifier, hql, filters, factory))).ToArray<QueryTranslatorImpl>();
      foreach (QueryTranslatorImpl queryTranslatorImpl in array)
      {
        if (collectionRole == null)
          queryTranslatorImpl.Compile(factory.Settings.QuerySubstitutions, shallow);
        else
          queryTranslatorImpl.Compile(collectionRole, factory.Settings.QuerySubstitutions, shallow);
      }
      return (IQueryTranslator[]) array;
    }
  }
}
