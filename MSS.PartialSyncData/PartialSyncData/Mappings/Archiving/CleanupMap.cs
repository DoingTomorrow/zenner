// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Archiving.CleanupMap
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
