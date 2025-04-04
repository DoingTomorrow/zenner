// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.UsersManagement.RoleOperationMap
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
  public sealed class RoleOperationMap : ClassMap<RoleOperation>
  {
    public RoleOperationMap()
    {
      this.Table("t_RoleOperation");
      this.Id((Expression<Func<RoleOperation, object>>) (appParam => (object) appParam.Id)).Column("Id").GeneratedBy.GuidComb();
      this.References<Role>((Expression<Func<RoleOperation, Role>>) (ur => ur.Role), "RoleId").Class<Role>();
      this.References<Operation>((Expression<Func<RoleOperation, Operation>>) (ur => ur.Operation), "OperationId").Class<Operation>();
    }
  }
}
