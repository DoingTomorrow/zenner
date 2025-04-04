// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.UsersManagement.RoleOperationMap
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
