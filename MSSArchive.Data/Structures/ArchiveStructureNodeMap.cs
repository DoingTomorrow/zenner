// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Structures.ArchiveStructureNodeMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.Structures
{
  public sealed class ArchiveStructureNodeMap : ClassMap<ArchiveStructureNode>
  {
    public ArchiveStructureNodeMap()
    {
      this.Table("t_StructureNode");
      this.Id((Expression<Func<ArchiveStructureNode, object>>) (m => (object) m.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveStructureNode, object>>) (n => (object) n.Id));
      this.Map((Expression<Func<ArchiveStructureNode, object>>) (n => n.Name)).Length(200);
      this.Map((Expression<Func<ArchiveStructureNode, object>>) (n => n.Description)).Length(1000);
      this.Map((Expression<Func<ArchiveStructureNode, object>>) (n => (object) n.EntityId));
      this.Map((Expression<Func<ArchiveStructureNode, object>>) (n => n.EntityName)).Length(200);
      this.Map((Expression<Func<ArchiveStructureNode, object>>) (n => (object) n.StartDate)).Nullable();
      this.Map((Expression<Func<ArchiveStructureNode, object>>) (n => (object) n.EndDate)).Nullable();
      this.Map((Expression<Func<ArchiveStructureNode, object>>) (n => (object) n.NodeType));
      this.Map((Expression<Func<ArchiveStructureNode, object>>) (m => (object) m.LockedBy)).Nullable();
      this.References<ArchiveInformation>((Expression<Func<ArchiveStructureNode, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_StructureNode_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
