// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.Filters.OrderFilter`1
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Core.Model.Orders;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSync.Filters
{
  public class OrderFilter<T> : GenericEntityFilter<T> where T : Order
  {
    private Expression DefineFilters(Guid Id)
    {
      StatusOrderEnum enumValue = StatusOrderEnum.Closed;
      Expression<Func<T, bool>> expression = (Expression<Func<T, bool>>) (c => c.Id == Id);
      (Expression<Func<T, bool>>) (c => (int) c.Status != (int) enumValue);
      return (Expression) expression;
    }

    public Expression ApplyReplace(Guid id) => this.VisitTree(this.DefineFilters(id));
  }
}
