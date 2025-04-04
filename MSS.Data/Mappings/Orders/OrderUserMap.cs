// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Orders.OrderUserMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Orders;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Orders
{
  public sealed class OrderUserMap : ClassMap<OrderUser>
  {
    public OrderUserMap()
    {
      this.Table("t_OrderUser");
      this.Id((Expression<Func<OrderUser, object>>) (appParam => (object) appParam.Id)).Column("Id").GeneratedBy.GuidComb();
      this.References<Order>((Expression<Func<OrderUser, Order>>) (ur => ur.Order), "OrderId").Class<Order>();
      this.References<User>((Expression<Func<OrderUser, User>>) (ur => ur.User), "UserId").Class<User>();
    }
  }
}
