// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.UsersManagement.OperationMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.UsersManagement;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.UsersManagement
{
  public sealed class OperationMap : ClassMap<Operation>
  {
    public OperationMap()
    {
      this.Table("t_Operation");
      this.Id((Expression<Func<Operation, object>>) (appParam => (object) appParam.Id)).Column("Id").GeneratedBy.GuidComb();
      this.Map((Expression<Func<Operation, object>>) (appParam => appParam.Name)).Length(100).Not.Nullable();
      this.HasMany<RoleOperation>((Expression<Func<Operation, IEnumerable<RoleOperation>>>) (c => c.RoleOperations)).KeyColumn("OperationId");
    }
  }
}
