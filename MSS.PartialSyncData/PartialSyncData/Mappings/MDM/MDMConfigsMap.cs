// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.MDM.MDMConfigsMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.MDM;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.MDM
{
  public class MDMConfigsMap : ClassMap<MDMConfigs>
  {
    public MDMConfigsMap()
    {
      this.Table("t_MDMConfigs");
      this.Id((Expression<Func<MDMConfigs, object>>) (n => (object) n.Id)).GeneratedBy.Increment();
      this.Map((Expression<Func<MDMConfigs, object>>) (c => c.MDMPassword));
      this.Map((Expression<Func<MDMConfigs, object>>) (c => c.MDMUser));
      this.Map((Expression<Func<MDMConfigs, object>>) (c => c.MDMUrl));
      this.Map((Expression<Func<MDMConfigs, object>>) (c => (object) c.Company));
      this.Map((Expression<Func<MDMConfigs, object>>) (c => c.CustomerNumber));
      this.References<Country>((Expression<Func<MDMConfigs, Country>>) (m => m.Country), "CountryId").Not.LazyLoad();
    }
  }
}
