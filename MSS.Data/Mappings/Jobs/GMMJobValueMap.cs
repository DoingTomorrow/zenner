// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Jobs.GMMJobValueMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Jobs
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
