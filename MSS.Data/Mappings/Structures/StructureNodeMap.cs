// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Structures.StructureNodeMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Structures
{
  public sealed class StructureNodeMap : ClassMap<StructureNode>
  {
    public StructureNodeMap()
    {
      this.Table("t_StructureNode");
      this.Id((Expression<Func<StructureNode, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<StructureNode, object>>) (n => n.Name)).Length(200);
      this.Map((Expression<Func<StructureNode, object>>) (n => n.Description)).Length(1000);
      this.Map((Expression<Func<StructureNode, object>>) (n => (object) n.EntityId));
      this.Map((Expression<Func<StructureNode, object>>) (n => n.EntityName)).Length(200);
      this.Map((Expression<Func<StructureNode, object>>) (n => (object) n.StartDate)).Nullable();
      this.Map((Expression<Func<StructureNode, object>>) (n => (object) n.EndDate)).Nullable();
      this.Map((Expression<Func<StructureNode, object>>) (n => (object) n.LastChangedOn)).Nullable();
      this.References<StructureNodeType>((Expression<Func<StructureNode, StructureNodeType>>) (n => n.NodeType), "NodeTypeId");
      this.HasMany<Note>((Expression<Func<StructureNode, IEnumerable<Note>>>) (n => n.Notes)).KeyColumn("StructureNodeId").Cascade.All();
      this.HasMany<PhotoMeter>((Expression<Func<StructureNode, IEnumerable<PhotoMeter>>>) (m => m.Photos)).KeyColumn("PhotoId").Cascade.All().KeyNullable();
    }
  }
}
