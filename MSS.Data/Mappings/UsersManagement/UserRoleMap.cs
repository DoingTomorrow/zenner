// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.UsersManagement.UserRoleMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.UsersManagement
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
