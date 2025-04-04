// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.OrderReadingValuesMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
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
