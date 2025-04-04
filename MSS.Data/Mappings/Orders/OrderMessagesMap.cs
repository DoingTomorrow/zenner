// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Orders.OrderMessagesMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Orders
{
  public class OrderMessagesMap : ClassMap<OrderMessage>
  {
    public OrderMessagesMap()
    {
      this.Table("t_OrderMessages");
      this.Id((Expression<Func<OrderMessage, object>>) (c => (object) c.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<OrderMessage, object>>) (c => (object) c.Level)).Length(20);
      this.Map((Expression<Func<OrderMessage, object>>) (c => (object) c.Timepoint));
      this.Map((Expression<Func<OrderMessage, object>>) (c => c.Message)).Length(1000);
      this.Map((Expression<Func<OrderMessage, object>>) (c => (object) c.LastChangedOn)).Nullable();
      this.References<Order>((Expression<Func<OrderMessage, Order>>) (c => c.Order), "OrderId").Not.LazyLoad();
      this.References<Meter>((Expression<Func<OrderMessage, Meter>>) (c => c.Meter), "MeterId").Not.LazyLoad();
    }
  }
}
