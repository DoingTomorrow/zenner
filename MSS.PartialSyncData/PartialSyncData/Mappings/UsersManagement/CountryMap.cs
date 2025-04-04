// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.UsersManagement.CountryMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.UsersManagement
{
  public class CountryMap : ClassMap<Country>
  {
    public CountryMap()
    {
      this.Table("t_Country");
      this.Id((Expression<Func<Country, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Country, object>>) (c => c.Code));
      this.Map((Expression<Func<Country, object>>) (c => c.Name));
      this.Map((Expression<Func<Country, object>>) (c => (object) c.DefaultScenarioId));
    }
  }
}
