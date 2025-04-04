// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Structures.LocationMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Structures
{
  public sealed class LocationMap : ClassMap<Location>
  {
    public LocationMap()
    {
      this.Table("t_Location");
      this.Id((Expression<Func<Location, object>>) (l => (object) l.Id)).GeneratedBy.Assigned();
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
      this.Map((Expression<Func<Location, object>>) (t => (object) t.LastChangedOn)).Nullable();
    }
  }
}
