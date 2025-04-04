// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.NhQueryable`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using Remotion.Linq;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public class NhQueryable<T> : QueryableBase<T>
  {
    public NhQueryable(ISessionImplementor session)
      : base((IQueryProvider) new DefaultQueryProvider(session))
    {
    }

    public NhQueryable(IQueryProvider provider, Expression expression)
      : base(provider, expression)
    {
    }
  }
}
