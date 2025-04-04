// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Structures.StructureNodeLinksMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Structures
{
  public sealed class StructureNodeLinksMap : ClassMap<StructureNodeLinks>
  {
    public StructureNodeLinksMap()
    {
      this.Table("t_StructureNodeLinks");
      this.Id((Expression<Func<StructureNodeLinks, object>>) (l => (object) l.Id)).GeneratedBy.GuidComb();
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
