// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.ExpressionVisitor.ReplaceVisitor
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace MSS.PartialSync.ExpressionVisitor
{
  public class ReplaceVisitor : System.Linq.Expressions.ExpressionVisitor
  {
    protected override Expression VisitMember(MemberExpression node)
    {
      return node.Expression.Type.IsDefined(typeof (CompilerGeneratedAttribute), false) ? (Expression) Expression.Constant(Expression.Lambda<Func<object>>((Expression) Expression.Convert((Expression) node, typeof (object))).Compile()()) : base.VisitMember(node);
    }
  }
}
