// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Orders.OrderMessagesMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Orders
{
  public class OrderMessagesMap : ClassMap<OrderMessage>
  {
    public OrderMessagesMap()
    {
      this.Table("t_OrderMessages");
      this.Id((Expression<Func<OrderMessage, object>>) (c => (object) c.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<OrderMessage, object>>) (c => (object) c.Level)).Length(20);
      this.Map((Expression<Func<OrderMessage, object>>) (c => (object) c.Timepoint));
      this.Map((Expression<Func<OrderMessage, object>>) (c => c.Message)).Length(1000);
      this.Map((Expression<Func<OrderMessage, object>>) (c => (object) c.LastChangedOn)).Nullable();
      this.References<Order>((Expression<Func<OrderMessage, Order>>) (c => c.Order), "OrderId").Not.LazyLoad();
      this.References<Meter>((Expression<Func<OrderMessage, Meter>>) (c => c.Meter), "MeterId").Not.LazyLoad();
    }
  }
}
