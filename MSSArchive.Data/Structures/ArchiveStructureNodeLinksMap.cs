// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Structures.ArchiveStructureNodeLinksMap
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
  public sealed class ArchiveStructureNodeLinksMap : ClassMap<ArchiveStructureNodeLinks>
  {
    public ArchiveStructureNodeLinksMap()
    {
      this.Table("t_StructureNodeLinks");
      this.Id((Expression<Func<ArchiveStructureNodeLinks, object>>) (m => (object) m.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveStructureNodeLinks, object>>) (n => (object) n.Id));
      this.Map((Expression<Func<ArchiveStructureNodeLinks, object>>) (n => (object) n.NodeId));
      this.Map((Expression<Func<ArchiveStructureNodeLinks, object>>) (n => (object) n.ParentNodeId));
      this.Map((Expression<Func<ArchiveStructureNodeLinks, object>>) (n => (object) n.RootNodeId));
      this.Map((Expression<Func<ArchiveStructureNodeLinks, object>>) (n => (object) n.StructureType)).Length(20).Not.Nullable();
      this.Map((Expression<Func<ArchiveStructureNodeLinks, object>>) (n => (object) n.StartDate)).Nullable();
      this.Map((Expression<Func<ArchiveStructureNodeLinks, object>>) (n => (object) n.EndDate)).Nullable();
      this.Map((Expression<Func<ArchiveStructureNodeLinks, object>>) (m => (object) m.LockedBy)).Nullable();
      this.Map((Expression<Func<ArchiveStructureNodeLinks, object>>) (l => (object) l.OrderNr));
      this.References<ArchiveInformation>((Expression<Func<ArchiveStructureNodeLinks, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_StructureNodeLinks_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
