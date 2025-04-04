// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.UsersManagement.UserMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Orders;
using MSS.Core.Model.UsersManagement;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.UsersManagement
{
  public sealed class UserMap : ClassMap<User>
  {
    public UserMap()
    {
      this.Table("t_User");
      this.Id((Expression<Func<User, object>>) (u => (object) u.Id)).Column("Id").GeneratedBy.GuidComb();
      this.Map((Expression<Func<User, object>>) (u => u.FirstName)).Length(100).Not.Nullable();
      this.Map((Expression<Func<User, object>>) (u => u.LastName)).Length(100).Not.Nullable();
      this.Map((Expression<Func<User, object>>) (u => u.Password)).Length(100).Not.Nullable();
      this.Map((Expression<Func<User, object>>) (u => u.Username)).Length(100).Not.Nullable();
      this.Map((Expression<Func<User, object>>) (u => u.Language)).Length(100).Not.Nullable();
      this.Map((Expression<Func<User, object>>) (u => (object) u.IsDeactivated)).Not.Nullable().Default("0");
      this.HasMany<UserRole>((Expression<Func<User, IEnumerable<UserRole>>>) (u => u.UserRoles)).KeyColumn("UserId").Cascade.Delete().Inverse();
      this.HasMany<OrderUser>((Expression<Func<User, IEnumerable<OrderUser>>>) (u => u.OrderUsers)).KeyColumn("UserId").Cascade.Delete().Inverse();
      this.Map((Expression<Func<User, object>>) (u => u.Office));
      this.References<Country>((Expression<Func<User, Country>>) (m => m.Country), "CountryId").Not.LazyLoad();
    }
  }
}
