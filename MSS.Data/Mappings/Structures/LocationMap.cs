// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Structures.LocationMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Structures
{
  public sealed class LocationMap : ClassMap<Location>
  {
    public LocationMap()
    {
      this.Table("t_Location");
      this.Id((Expression<Func<Location, object>>) (l => (object) l.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Location, object>>) (l => l.City)).Length(25).Not.Nullable();
      this.Map((Expression<Func<Location, object>>) (l => l.Street)).Length(90).Not.Nullable();
      this.Map((Expression<Func<Location, object>>) (l => l.ZipCode)).Length(10).Not.Nullable();
      this.Map((Expression<Func<Location, object>>) (l => l.BuildingNr)).Length(13).Not.Nullable();
      this.Map((Expression<Func<Location, object>>) (l => l.Description)).Length(250);
      this.Map((Expression<Func<Location, object>>) (l => (object) l.DueDate)).Nullable();
      this.Map((Expression<Func<Location, object>>) (l => (object) l.Generation)).Not.Nullable();
      this.Map((Expression<Func<Location, object>>) (l => (object) l.Scale)).Not.Nullable();
      this.Map((Expression<Func<Location, object>>) (l => (object) l.HasMaster)).Nullable();
      this.References<Scenario>((Expression<Func<Location, Scenario>>) (l => l.Scenario), "ScenarioId").Class<Scenario>();
      this.Map((Expression<Func<Location, object>>) (m => (object) m.IsDeactivated));
      this.References<Country>((Expression<Func<Location, Country>>) (m => m.Country), "CountryId").Not.LazyLoad();
      this.Map((Expression<Func<Location, object>>) (l => l.Office));
      this.Map((Expression<Func<Location, object>>) (l => l.CreatedBy));
      this.Map((Expression<Func<Location, object>>) (l => l.UpdatedBy));
      this.Map((Expression<Func<Location, object>>) (l => (object) l.LastUpdateBuildingNo));
      this.Map((Expression<Func<Location, object>>) (t => (object) t.LastPortfolioMDMExportOn));
      this.Map((Expression<Func<Location, object>>) (t => (object) t.LastBuildingMDMExportOn));
      this.Map((Expression<Func<Location, object>>) (t => (object) t.LastAddressMDMExportOn));
      this.Map((Expression<Func<Location, object>>) (t => (object) t.LastChangedOn)).Nullable();
    }
  }
}
