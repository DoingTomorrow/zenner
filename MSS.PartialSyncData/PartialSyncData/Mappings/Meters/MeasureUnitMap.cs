// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.MeasureUnitMap
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
  public sealed class MeasureUnitMap : ClassMap<MeasureUnit>
  {
    public MeasureUnitMap()
    {
      this.Table("t_MeasureUnit");
      this.Id((Expression<Func<MeasureUnit, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MeasureUnit, object>>) (c => c.Code)).Length(200);
      this.Map((Expression<Func<MeasureUnit, object>>) (c => c.CelestaCode));
    }
  }
}
