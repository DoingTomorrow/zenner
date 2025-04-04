// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.Filters.OrderUserFilter`1
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Core.Model.Orders;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSync.Filters
{
  public class OrderUserFilter<T> : GenericEntityFilter<T> where T : OrderUser
  {
    private Expression DefineFilters()
    {
      Guid userId = MSS.Business.Utils.AppContext.Current.LoggedUser.Id;
      return (Expression) (c => c.User.Id == userId);
    }

    public Expression ApplyReplace() => this.VisitTree(this.DefineFilters());
  }
}
