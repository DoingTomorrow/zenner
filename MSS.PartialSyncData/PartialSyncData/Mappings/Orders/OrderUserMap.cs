// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Orders.OrderUserMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Orders;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Orders
{
  public sealed class OrderUserMap : ClassMap<OrderUser>
  {
    public OrderUserMap()
    {
      this.Table("t_OrderUser");
      this.Id((Expression<Func<OrderUser, object>>) (appParam => (object) appParam.Id)).Column("Id").GeneratedBy.Assigned();
      this.References<Order>((Expression<Func<OrderUser, Order>>) (ur => ur.Order), "OrderId").Class<Order>();
      this.References<User>((Expression<Func<OrderUser, User>>) (ur => ur.User), "UserId").Class<User>();
    }
  }
}
