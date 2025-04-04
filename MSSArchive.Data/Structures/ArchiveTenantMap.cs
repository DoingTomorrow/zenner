// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Structures.ArchiveTenantMap
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
  public sealed class ArchiveTenantMap : ClassMap<ArchiveTenant>
  {
    public ArchiveTenantMap()
    {
      this.Table("t_Tenant");
      this.Id((Expression<Func<ArchiveTenant, object>>) (t => (object) t.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => (object) t.Id));
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => (object) t.TenantNr)).Length(4).Not.Nullable();
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => t.Name)).Length(30).Not.Nullable();
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => t.FloorNr)).Length(3);
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => t.FloorName)).Length(10);
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => t.ApartmentNr)).Length(3);
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => t.Description)).Length(500);
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => (object) t.IsGroup));
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => t.CustomerTenantNo)).Length(30);
      this.Map((Expression<Func<ArchiveTenant, object>>) (t => (object) t.IsDeactivated));
      this.References<ArchiveInformation>((Expression<Func<ArchiveTenant, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_Tenant_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
