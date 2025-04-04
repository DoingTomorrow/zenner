// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.RoomTypeMap
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
  public sealed class RoomTypeMap : ClassMap<RoomType>
  {
    public RoomTypeMap()
    {
      this.Table("t_RoomType");
      this.Id((Expression<Func<RoomType, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<RoomType, object>>) (c => c.Code)).Length(200);
    }
  }
}
