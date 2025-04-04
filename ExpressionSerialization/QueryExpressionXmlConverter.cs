// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.QueryExpressionXmlConverter
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

#nullable disable
namespace ExpressionSerialization
{
  public class QueryExpressionXmlConverter : CustomExpressionXmlConverter
  {
    private QueryCreator creator;
    private TypeResolver resolver;

    public QueryExpressionXmlConverter(QueryCreator creator = null, TypeResolver resolver = null)
    {
      this.creator = creator;
      this.resolver = resolver;
    }

    public override bool TryDeserialize(XElement expressionXml, out Expression e)
    {
      if (this.creator == null || this.resolver == null)
        throw new InvalidOperationException(string.Format("{0} and {1} instances are required for deserialization.", (object) typeof (QueryCreator), (object) typeof (TypeResolver)));
      if (expressionXml.Name.LocalName == "Query")
      {
        object query = this.creator.CreateQuery(this.resolver.GetType(expressionXml.Attribute((XName) "elementType").Value));
        ref Expression local = ref e;
        // ISSUE: reference to a compiler-generated field
        if (QueryExpressionXmlConverter.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          QueryExpressionXmlConverter.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Expression>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Expression), typeof (QueryExpressionXmlConverter)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Expression> target = QueryExpressionXmlConverter.\u003C\u003Eo__3.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Expression>> p1 = QueryExpressionXmlConverter.\u003C\u003Eo__3.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (QueryExpressionXmlConverter.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          QueryExpressionXmlConverter.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Constant", (IEnumerable<Type>) null, typeof (QueryExpressionXmlConverter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = QueryExpressionXmlConverter.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) QueryExpressionXmlConverter.\u003C\u003Eo__3.\u003C\u003Ep__0, typeof (Expression), query);
        Expression expression = target((CallSite) p1, obj);
        local = expression;
        return true;
      }
      e = (Expression) null;
      return false;
    }

    public override bool TrySerialize(Expression e, out XElement x)
    {
      if (e.NodeType == ExpressionType.Constant && typeof (IQueryable).IsAssignableFrom(e.Type))
      {
        Type elementType = ((IQueryable) ((ConstantExpression) e).Value).ElementType;
        if (typeof (Query<>).MakeGenericType(elementType) == e.Type)
        {
          x = new XElement((XName) "Query", (object) new XAttribute((XName) "elementType", (object) elementType.FullName));
          return true;
        }
      }
      x = (XElement) null;
      return false;
    }
  }
}
