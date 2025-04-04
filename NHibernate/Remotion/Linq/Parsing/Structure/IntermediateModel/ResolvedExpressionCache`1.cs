// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ResolvedExpressionCache`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class ResolvedExpressionCache<T> where T : Expression
  {
    private readonly ExpressionResolver _resolver;
    private T _cachedExpression;

    public ResolvedExpressionCache(IExpressionNode currentNode)
    {
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (currentNode), currentNode);
      this._resolver = new ExpressionResolver(currentNode);
      this._cachedExpression = default (T);
    }

    public T GetOrCreate(Func<ExpressionResolver, T> generator)
    {
      if ((object) this._cachedExpression == null)
        this._cachedExpression = generator(this._resolver);
      return this._cachedExpression;
    }
  }
}
