// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.DataFilters.FilterMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Jobs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.DataFilters
{
  public class FilterMap : ClassMap<MSS.Core.Model.DataFilters.Filter>
  {
    public FilterMap()
    {
      this.Table("t_Filters");
      this.Id((Expression<Func<MSS.Core.Model.DataFilters.Filter, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MSS.Core.Model.DataFilters.Filter, object>>) (c => c.Name)).Length(100).Not.Nullable();
      this.Map((Expression<Func<MSS.Core.Model.DataFilters.Filter, object>>) (c => c.Description)).Length(1000);
      this.HasMany<Rules>((Expression<Func<MSS.Core.Model.DataFilters.Filter, IEnumerable<Rules>>>) (r => r.Rules)).KeyColumn("FilterId").Cascade.Delete().Inverse();
      this.HasMany<JobDefinition>((Expression<Func<MSS.Core.Model.DataFilters.Filter, IEnumerable<JobDefinition>>>) (r => r.JobDefinitions)).KeyColumn("FilterId").Cascade.Delete().Inverse();
    }
  }
}
