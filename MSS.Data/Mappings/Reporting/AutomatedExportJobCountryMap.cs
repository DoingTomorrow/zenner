// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Reporting.AutomatedExportJobCountryMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Reporting;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Reporting
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
