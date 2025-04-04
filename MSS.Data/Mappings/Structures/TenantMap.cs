// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Structures.TenantMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Structures
{
  public sealed class TenantMap : ClassMap<Tenant>
  {
    public TenantMap()
    {
      this.Table("t_Tenant");
      this.Id((Expression<Func<Tenant, object>>) (t => (object) t.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Tenant, object>>) (t => (object) t.TenantNr)).Length(4).Not.Nullable();
      this.Map((Expression<Func<Tenant, object>>) (t => t.Name)).Length(30).Not.Nullable();
      this.Map((Expression<Func<Tenant, object>>) (t => t.FloorNr)).Length(3);
      this.Map((Expression<Func<Tenant, object>>) (t => t.FloorName)).Length(10);
      this.Map((Expression<Func<Tenant, object>>) (t => t.ApartmentNr)).Length(3);
      this.Map((Expression<Func<Tenant, object>>) (t => t.Direction)).Length(10);
      this.Map((Expression<Func<Tenant, object>>) (t => t.Description)).Length(500);
      this.Map((Expression<Func<Tenant, object>>) (t => t.CustomerTenantNo)).Length(30);
      this.Map((Expression<Func<Tenant, object>>) (t => (object) t.RealTenantNo));
      this.Map((Expression<Func<Tenant, object>>) (t => (object) t.IsDeactivated));
      this.Map((Expression<Func<Tenant, object>>) (t => t.CreatedBy));
      this.Map((Expression<Func<Tenant, object>>) (t => t.UpdatedBy));
      this.Map((Expression<Func<Tenant, object>>) (t => (object) t.LastChangedOn)).Nullable();
      this.Map((Expression<Func<Tenant, object>>) (t => (object) t.LastTenantInfoMDMExportOn));
      this.Map((Expression<Func<Tenant, object>>) (t => (object) t.LastTenantFlatMDMExportOn));
      this.Map((Expression<Func<Tenant, object>>) (t => t.Entrance));
    }
  }
}
