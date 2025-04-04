// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.UsersManagement.UserRoleMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.UsersManagement
{
  public sealed class UserRoleMap : ClassMap<UserRole>
  {
    public UserRoleMap()
    {
      this.Table("t_UserRole");
      this.Id((Expression<Func<UserRole, object>>) (appParam => (object) appParam.Id)).Column("Id").GeneratedBy.GuidComb();
      this.References<Role>((Expression<Func<UserRole, Role>>) (ur => ur.Role), "RoleId").Class<Role>();
      this.References<User>((Expression<Func<UserRole, User>>) (ur => ur.User), "UserId").Class<User>();
    }
  }
}
