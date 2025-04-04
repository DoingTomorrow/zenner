// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.Filters.GenericEntityFilter`1
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.PartialSync.ExpressionVisitor;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSync.Filters
{
  public abstract class GenericEntityFilter<T> where T : class
  {
    public virtual Expression VisitTree(Expression expression)
    {
      return new ReplaceVisitor().Visit(expression);
    }
  }
}
