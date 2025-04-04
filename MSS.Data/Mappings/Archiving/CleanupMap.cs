// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Archiving.CleanupMap
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
  public class CleanupMap : ClassMap<Cleanup>
  {
    public CleanupMap()
    {
      this.Table("t_Cleanup");
      this.Id((Expression<Func<Cleanup, object>>) (x => (object) x.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Cleanup, object>>) (x => (object) x.CleanupDate));
      this.References<Archive>((Expression<Func<Cleanup, Archive>>) (m => m.Archive), "ArchiveId").Not.LazyLoad();
    }
  }
}
