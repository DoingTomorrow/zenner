// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.MeasureUnitMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
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
