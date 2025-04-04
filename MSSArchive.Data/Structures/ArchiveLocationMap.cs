// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Structures.ArchiveLocationMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.Structures
{
  public sealed class ArchiveLocationMap : ClassMap<ArchiveLocation>
  {
    public ArchiveLocationMap()
    {
      this.Table("t_Location");
      this.Id((Expression<Func<ArchiveLocation, object>>) (l => (object) l.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => (object) l.Id));
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => l.City)).Length(25).Not.Nullable();
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => l.Street)).Length(90).Not.Nullable();
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => l.ZipCode)).Length(10).Not.Nullable();
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => l.BuildingNr)).Length(13).Not.Nullable();
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => l.Description)).Length(250);
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => (object) l.DueDate)).Not.Nullable();
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => (object) l.Generation)).Not.Nullable();
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => (object) l.Scale)).Not.Nullable();
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => (object) l.HasMaster)).Not.Nullable();
      this.Map((Expression<Func<ArchiveLocation, object>>) (l => l.ScenarioCode)).Not.Nullable();
      this.Map((Expression<Func<ArchiveLocation, object>>) (m => (object) m.IsDeactivated));
      this.References<ArchiveInformation>((Expression<Func<ArchiveLocation, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_Location_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
