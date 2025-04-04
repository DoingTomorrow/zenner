// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Reporting.AutomatedExportJobCountryMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Reporting;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Reporting
{
  public sealed class AutomatedExportJobCountryMap : ClassMap<AutomatedExportJobCountry>
  {
    public AutomatedExportJobCountryMap()
    {
      this.Table("t_AutomatedExportJobCountry");
      this.Id((Expression<Func<AutomatedExportJobCountry, object>>) (aejc => (object) aejc.Id)).Column("Id").GeneratedBy.GuidComb();
      this.References<AutomatedExportJob>((Expression<Func<AutomatedExportJobCountry, AutomatedExportJob>>) (aejc => aejc.AutomatedExportJob), "AutomatedExportJobId").Class<AutomatedExportJob>();
      this.References<Country>((Expression<Func<AutomatedExportJobCountry, Country>>) (aejc => aejc.Country), "CountryId").Class<Country>();
    }
  }
}
