// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Archiving.ArchiveInformationMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Archiving;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.Archiving
{
  public class ArchiveInformationMap : ClassMap<ArchiveInformation>
  {
    public ArchiveInformationMap()
    {
      this.Table("t_ArchiveInformation");
      this.Id((Expression<Func<ArchiveInformation, object>>) (x => (object) x.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<ArchiveInformation, object>>) (x => x.ArchiveName));
      this.Map((Expression<Func<ArchiveInformation, object>>) (x => (object) x.DateTime));
      this.Map((Expression<Func<ArchiveInformation, object>>) (x => (object) x.StartTime));
      this.Map((Expression<Func<ArchiveInformation, object>>) (x => (object) x.EndTime));
      this.Map((Expression<Func<ArchiveInformation, object>>) (m => m.ArchivedEntities)).Nullable().Length(8000);
    }
  }
}
