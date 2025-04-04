// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Structures.StructureNodeLinksMap
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
  public sealed class StructureNodeLinksMap : ClassMap<StructureNodeLinks>
  {
    public StructureNodeLinksMap()
    {
      this.Table("t_StructureNodeLinks");
      this.Id((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.ParentNodeId));
      this.Map((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.StructureType)).Length(20).Not.Nullable();
      this.Map((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.StartDate)).Nullable();
      this.Map((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.EndDate)).Nullable();
      this.Map((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.OrderNr));
      this.Map((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.LastChangedOn)).Nullable();
      this.References<StructureNode>((Expression<Func<StructureNodeLinks, StructureNode>>) (l => l.Node), "NodeId");
      this.References<StructureNode>((Expression<Func<StructureNodeLinks, StructureNode>>) (l => l.RootNode), "RootNodeId");
    }
  }
}
