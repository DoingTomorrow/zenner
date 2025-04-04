// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.MDM.MDMConfigsMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.MDM;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.MDM
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
