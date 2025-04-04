// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.OrderReadingValuesMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Meters
{
  public class OrderReadingValuesMap : ClassMap<OrderReadingValues>
  {
    public OrderReadingValuesMap()
    {
      this.Table("t_OrderReadingValues");
      this.Id((Expression<Func<OrderReadingValues, object>>) (appParam => (object) appParam.Id)).Column("Id").GeneratedBy.GuidComb();
      this.References<Order>((Expression<Func<OrderReadingValues, Order>>) (ur => ur.OrderObj), "OrderId").Class<Order>();
      this.References<MeterReadingValue>((Expression<Func<OrderReadingValues, MeterReadingValue>>) (ur => ur.MeterReadingValue), "MeterReadingValueId").Class<MeterReadingValue>();
    }
  }
}
