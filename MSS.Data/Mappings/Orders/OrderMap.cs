// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Orders.OrderMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Orders
{
  public class OrderMap : ClassMap<Order>
  {
    public OrderMap()
    {
      this.Table("t_Order");
      this.Id((Expression<Func<Order, object>>) (o => (object) o.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Order, object>>) (o => o.InstallationNumber)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<Order, object>>) (o => (object) o.RootStructureNodeId));
      this.Map((Expression<Func<Order, object>>) (o => o.Details)).Length(1000);
      this.Map((Expression<Func<Order, object>>) (o => (object) o.Exported));
      this.Map((Expression<Func<Order, object>>) (o => (object) o.Status));
      this.Map((Expression<Func<Order, object>>) (o => o.DeviceNumber)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<Order, object>>) (o => o.TestConfig));
      this.Map((Expression<Func<Order, object>>) (o => (object) o.DueDate));
      this.Map((Expression<Func<Order, object>>) (o => (object) o.IsDeactivated));
      this.Map((Expression<Func<Order, object>>) (o => (object) o.OrderType));
      this.Map((Expression<Func<Order, object>>) (o => (object) o.CloseOrderReason));
      this.Map((Expression<Func<Order, object>>) (m => (object) m.LockedBy)).Nullable();
      this.Map((Expression<Func<Order, object>>) (m => m.StructureBytes)).Nullable().Length(int.MaxValue);
      this.Map((Expression<Func<Order, object>>) (m => (object) m.StructureType)).Nullable();
      this.Map((Expression<Func<Order, object>>) (m => (object) m.CreatedOn));
      this.Map((Expression<Func<Order, object>>) (m => (object) m.LastChangedOn)).Nullable();
      this.HasMany<OrderUser>((Expression<Func<Order, IEnumerable<OrderUser>>>) (c => c.OrderUsers)).KeyColumn("OrderId");
      this.References<MSS.Core.Model.DataFilters.Filter>((Expression<Func<Order, MSS.Core.Model.DataFilters.Filter>>) (n => n.Filter), "FilterId");
      this.HasMany<OrderReadingValues>((Expression<Func<Order, IEnumerable<OrderReadingValues>>>) (c => c.OrderReadingValues)).KeyColumn("OrderId");
    }
  }
}
