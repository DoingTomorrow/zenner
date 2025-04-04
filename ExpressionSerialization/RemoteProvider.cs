// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.RemoteProvider
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

#nullable disable
namespace ExpressionSerialization
{
  public class RemoteProvider : QueryProvider
  {
    private WebHttpClient<IQueryService> client;
    private ExpressionSerializer serializer;
    private TypeResolver resolver;
    private RemoteProvider.StripQuoteVisitor visitor;

    public RemoteProvider(WebHttpClient<IQueryService> client)
    {
      this.client = client;
      this.visitor = new RemoteProvider.StripQuoteVisitor();
      this.resolver = new TypeResolver(knownTypes: (IEnumerable<Type>) ((object) client.knownTypes ?? (object) new Type[0]));
      this.serializer = new ExpressionSerializer(this.resolver, (IEnumerable<CustomExpressionXmlConverter>) new CustomExpressionXmlConverter[2]
      {
        (CustomExpressionXmlConverter) new QueryExpressionXmlConverter(resolver: this.resolver),
        (CustomExpressionXmlConverter) new KnownTypeExpressionXmlConverter(this.resolver)
      });
    }

    public override string GetQueryText(Expression expression) => this.GetType().FullName;

    public override object Execute(Expression e)
    {
      MethodCallExpression node = e.NodeType == ExpressionType.Call ? (MethodCallExpression) e : throw new ArgumentException("Exrpression expected: " + (object) typeof (MethodCallExpression));
      Type returnType;
      if (node.Arguments[0] is ConstantExpression)
      {
        Type elementType = ((IQueryable) ((ConstantExpression) node.Arguments[0]).Value).ElementType;
        if (typeof (IEnumerable<>).MakeGenericType(elementType).IsAssignableFrom(node.Method.ReturnType))
          returnType = elementType.MakeArrayType();
        else
          throw new ArgumentException(string.Format("Expected for {0} of Type {1}\n Return Type of {2}", (object) (ConstantExpression) node.Arguments[0], (object) typeof (ConstantExpression), (object) typeof (IEnumerable<>).MakeGenericType(elementType)));
      }
      else
        returnType = node.Method.ReturnType;
      XElement x = this.serializer.Serialize(this.visitor.Visit((Expression) node));
      return this.client.SynchronousCall((Expression<Func<IQueryService, object>>) (svc => svc.ExecuteQuery(x)), returnType);
    }

    private class StripQuoteVisitor : ExpressionVisitor
    {
      protected override Expression VisitMethodCall(MethodCallExpression m)
      {
        if (!(m.Method.DeclaringType == typeof (Queryable)) || m.Arguments.Count != 2 || !(m.Arguments[1] is UnaryExpression) && m.Arguments[1].NodeType != ExpressionType.Quote)
          return (Expression) m;
        ConstantExpression constantExpression = (ConstantExpression) m.Arguments[0];
        return base.VisitMethodCall(m);
      }

      protected override Expression VisitUnary(UnaryExpression u)
      {
        return this.StripQuotes((Expression) u);
      }

      private Expression StripQuotes(Expression e)
      {
        while (e.NodeType == ExpressionType.Quote)
          e = ((UnaryExpression) e).Operand;
        return e;
      }
    }
  }
}
