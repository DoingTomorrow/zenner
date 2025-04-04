// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.NhFetchRequest`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public class NhFetchRequest<TQueried, TFetch>(IQueryProvider provider, Expression expression) : 
    QueryableBase<TQueried>(provider, expression),
    INhFetchRequest<TQueried, TFetch>,
    IOrderedQueryable<TQueried>,
    IQueryable<TQueried>,
    IEnumerable<TQueried>,
    IOrderedQueryable,
    IQueryable,
    IEnumerable
  {
  }
}
