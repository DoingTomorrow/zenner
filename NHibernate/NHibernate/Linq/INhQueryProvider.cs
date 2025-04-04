// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.INhQueryProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public interface INhQueryProvider : IQueryProvider
  {
    object ExecuteFuture(Expression expression);

    void SetResultTransformerAndAdditionalCriteria(
      IQuery query,
      NhLinqExpression nhExpression,
      IDictionary<string, Tuple<object, IType>> parameters);
  }
}
