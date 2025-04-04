// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Reporting.HeatersMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Reporting;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Reporting
{
  public class HeatersMap : ClassMap<Heaters>
  {
    public HeatersMap()
    {
      this.Table("t_Heaters");
      this.Id((Expression<Func<Heaters, object>>) (rtd => (object) rtd.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Heaters, object>>) (rt => rt.Name));
      this.Map((Expression<Func<Heaters, object>>) (rt => rt.Description));
      this.Map((Expression<Func<Heaters, object>>) (rt => rt.GroupName));
      this.Map((Expression<Func<Heaters, object>>) (rt => (object) rt.EvaluationFactor));
    }
  }
}
