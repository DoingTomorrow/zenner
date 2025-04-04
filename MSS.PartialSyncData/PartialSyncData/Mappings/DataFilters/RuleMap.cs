// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.DataFilters.RuleMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataFilters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.DataFilters
{
  public class RuleMap : ClassMap<Rules>
  {
    public RuleMap()
    {
      this.Table("t_Rules");
      this.Id((Expression<Func<Rules, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Rules, object>>) (c => (object) c.PhysicalQuantity)).Length(100).Not.Nullable();
      this.Map((Expression<Func<Rules, object>>) (c => (object) c.MeterType)).Length(100).Not.Nullable();
      this.Map((Expression<Func<Rules, object>>) (c => (object) c.Calculation)).Length(100).Not.Nullable();
      this.Map((Expression<Func<Rules, object>>) (c => (object) c.CalculationStart)).Length(100).Not.Nullable();
      this.Map((Expression<Func<Rules, object>>) (c => (object) c.StorageInterval)).Length(100).Not.Nullable();
      this.Map((Expression<Func<Rules, object>>) (c => (object) c.Creation)).Length(100).Not.Nullable();
      this.Map((Expression<Func<Rules, object>>) (c => (object) c.RuleIndex)).Not.Nullable();
      this.Map((Expression<Func<Rules, object>>) (c => c.ValueId)).Not.Nullable();
      this.References<MSS.Core.Model.DataFilters.Filter>((Expression<Func<Rules, MSS.Core.Model.DataFilters.Filter>>) (f => f.Filter), "FilterId").Class<MSS.Core.Model.DataFilters.Filter>();
    }
  }
}
