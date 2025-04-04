// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Jobs.GMMJobValueMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Jobs
{
  public class GMMJobValueMap : ClassMap<GMMJobValue>
  {
    public GMMJobValueMap()
    {
      this.Table("t_GMMJobValues");
      this.Id((Expression<Func<GMMJobValue, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<GMMJobValue, object>>) (n => (object) n.JobId)).Nullable();
      this.Map((Expression<Func<GMMJobValue, object>>) (n => n.SerialNumber)).Not.Nullable();
      this.Map((Expression<Func<GMMJobValue, object>>) (n => (object) n.ReceivedOn)).Not.Nullable();
    }
  }
}
