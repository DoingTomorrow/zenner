// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Structures.StructureNodeTypeMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Structures
{
  public sealed class StructureNodeTypeMap : ClassMap<StructureNodeType>
  {
    public StructureNodeTypeMap()
    {
      this.Table("t_StructureNodeType");
      this.Id((Expression<Func<StructureNodeType, object>>) (n => (object) n.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<StructureNodeType, object>>) (n => n.Name)).Length(200);
      this.Map((Expression<Func<StructureNodeType, object>>) (n => n.IconPath)).Length(200);
      this.Map((Expression<Func<StructureNodeType, object>>) (n => (object) n.IsFixed));
      this.Map((Expression<Func<StructureNodeType, object>>) (n => (object) n.IsLogical));
      this.Map((Expression<Func<StructureNodeType, object>>) (n => (object) n.IsPhysical));
    }
  }
}
