// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.LinqLogging
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using NHibernate.Proxy;
using NHibernate.Util;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  internal static class LinqLogging
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor("NHibernate.Linq");

    internal static void LogExpression(string msg, Expression expression)
    {
      if (!LinqLogging.Log.IsDebugEnabled)
        return;
      Expression expression1 = new LinqLogging.ProxyReplacingExpressionTreeVisitor().VisitExpression(expression);
      LinqLogging.Log.DebugFormat("{0}: {1}", (object) msg, (object) expression1.ToString());
    }

    private class ProxyReplacingExpressionTreeVisitor : NhExpressionTreeVisitor
    {
      protected override Expression VisitConstantExpression(ConstantExpression expression)
      {
        return expression.Value.IsProxy() ? (Expression) Expression.Parameter(expression.Type, ObjectUtils.IdentityToString(expression.Value)) : base.VisitConstantExpression(expression);
      }
    }
  }
}
