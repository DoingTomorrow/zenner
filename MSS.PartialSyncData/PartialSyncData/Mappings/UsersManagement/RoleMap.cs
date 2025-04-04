// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.UsersManagement.RoleMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.UsersManagement;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.UsersManagement
{
  public sealed class RoleMap : ClassMap<Role>
  {
    public RoleMap()
    {
      this.Table("t_Role");
      this.Id((Expression<Func<Role, object>>) (appParam => (object) appParam.Id)).Column("Id").GeneratedBy.GuidComb();
      this.Map((Expression<Func<Role, object>>) (appParam => appParam.Name)).Length(100).Not.Nullable();
      this.Map((Expression<Func<Role, object>>) (appParam => (object) appParam.IsStandard)).Not.Nullable();
      this.Map((Expression<Func<Role, object>>) (appParam => (object) appParam.IsDeactivated)).Not.Nullable().Default("0");
      this.HasMany<UserRole>((Expression<Func<Role, IEnumerable<UserRole>>>) (c => c.UserRoles)).KeyColumn("RoleId");
      this.HasMany<RoleOperation>((Expression<Func<Role, IEnumerable<RoleOperation>>>) (c => c.RoleOperations)).KeyColumn("RoleId");
    }
  }
}
