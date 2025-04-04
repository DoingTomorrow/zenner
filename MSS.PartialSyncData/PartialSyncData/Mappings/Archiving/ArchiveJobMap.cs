// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Archiving.ArchiveJobMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Archiving;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Archiving
{
  internal class ArchiveJobMap : ClassMap<ArchiveJob>
  {
    public ArchiveJobMap()
    {
      this.Table("t_ArchiveJob");
      this.Id((Expression<Func<ArchiveJob, object>>) (aj => (object) aj.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<ArchiveJob, object>>) (aj => (object) aj.Periodicity));
      this.Map((Expression<Func<ArchiveJob, object>>) (aj => (object) aj.StartDate));
      this.Map((Expression<Func<ArchiveJob, object>>) (aj => (object) aj.NumberOfDaysToExport));
      this.Map((Expression<Func<ArchiveJob, object>>) (aj => aj.ArchiveName));
      this.Map((Expression<Func<ArchiveJob, object>>) (aj => (object) aj.DeleteAfterArchive));
      this.Map((Expression<Func<ArchiveJob, object>>) (aj => (object) aj.CreatedOn));
      this.Map((Expression<Func<ArchiveJob, object>>) (aj => aj.ArchivedEntities)).Nullable().Length(int.MaxValue);
      this.Map((Expression<Func<ArchiveJob, object>>) (aj => (object) aj.LastExecutionDate));
    }
  }
}
