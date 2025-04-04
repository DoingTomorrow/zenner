// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.MeterReadingValueMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
{
  public sealed class MeterReadingValueMap : ClassMap<MeterReadingValue>
  {
    public MeterReadingValueMap()
    {
      this.Table("t_ReadingValues");
      this.Id((Expression<Func<MeterReadingValue, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.Date));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.Value));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.ValueId));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.MeterId));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => m.MeterSerialNumber));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.CreatedOn));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.ExportedOn)).Nullable();
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.MDMExportedOn)).Nullable();
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.StorageInterval));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.PhysicalQuantity));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.MeterType));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.CalculationStart));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.Creation));
      this.Map((Expression<Func<MeterReadingValue, object>>) (m => (object) m.Calculation));
      this.References<MeasureUnit>((Expression<Func<MeterReadingValue, MeasureUnit>>) (m => m.Unit), "UnitId").Not.LazyLoad();
      this.HasMany<OrderReadingValues>((Expression<Func<MeterReadingValue, IEnumerable<OrderReadingValues>>>) (c => c.OrderReadingValues)).KeyColumn("MeterReadingValueId");
    }
  }
}
