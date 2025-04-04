// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.MeterReplacementHistoryMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
{
  public sealed class MeterReplacementHistoryMap : ClassMap<MeterReplacementHistory>
  {
    public MeterReplacementHistoryMap()
    {
      this.Table("t_MeterReplacementHistory");
      this.Id((Expression<Func<MeterReplacementHistory, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MeterReplacementHistory, object>>) (m => (object) m.ReplacementDate));
      this.Map((Expression<Func<MeterReplacementHistory, object>>) (m => (object) m.LastChangedOn)).Nullable();
      this.References<Meter>((Expression<Func<MeterReplacementHistory, Meter>>) (m => m.CurrentMeter), "CurrentMeterId").Not.LazyLoad();
      this.References<Meter>((Expression<Func<MeterReplacementHistory, Meter>>) (m => m.ReplacedMeter), "ReplacedMeterId").Not.LazyLoad();
      this.References<User>((Expression<Func<MeterReplacementHistory, User>>) (m => m.ReplacedByUser), "ReplacedById").Not.LazyLoad();
    }
  }
}
