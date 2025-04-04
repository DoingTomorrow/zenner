// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Reporting.AutomatedExportJobMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Reporting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Reporting
{
  public class AutomatedExportJobMap : ClassMap<AutomatedExportJob>
  {
    public AutomatedExportJobMap()
    {
      this.Table("t_AutomatedExportJob");
      this.Id((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.Type)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.Periodicity)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.StartDate)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.LastExecutionTime)).Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.ArchiveAfterExport)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.DeleteAfterExport)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => l.DataToExport)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => l.ExportFor)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => l.ExportedFileType)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.DecimalSeparator)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.ValueSeparator)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => l.Path)).Not.Nullable();
      this.Map((Expression<Func<AutomatedExportJob, object>>) (l => (object) l.CreatedOn));
      this.HasMany<AutomatedExportJobCountry>((Expression<Func<AutomatedExportJob, IEnumerable<AutomatedExportJobCountry>>>) (u => u.JobCountries)).KeyColumn("AutomatedExportJobId").Cascade.Delete().Inverse();
    }
  }
}
