// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Archiving.ArchiveJobMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Archiving;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Archiving
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
