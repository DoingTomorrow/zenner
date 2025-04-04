// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.ChannelMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Meters
{
  public sealed class ChannelMap : ClassMap<Channel>
  {
    public ChannelMap()
    {
      this.Table("t_Channel");
      this.Id((Expression<Func<Channel, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Channel, object>>) (c => c.Code)).Length(200);
      this.Map((Expression<Func<Channel, object>>) (c => c.CelestaCode));
    }
  }
}
