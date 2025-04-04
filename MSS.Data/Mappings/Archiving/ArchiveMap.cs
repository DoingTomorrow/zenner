// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Archiving.ArchiveMap
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
  public class ArchiveMap : ClassMap<Archive>
  {
    public ArchiveMap()
    {
      this.Table("t_Archive");
      this.Id((Expression<Func<Archive, object>>) (x => (object) x.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Archive, object>>) (x => (object) x.DateTime));
      this.Map((Expression<Func<Archive, object>>) (x => (object) x.StartTime));
      this.Map((Expression<Func<Archive, object>>) (x => (object) x.EndTime));
      this.Map((Expression<Func<Archive, object>>) (m => m.ArchivedEntities)).Nullable().Length(int.MaxValue);
    }
  }
}
