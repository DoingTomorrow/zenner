// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Structures.ArchiveStructureNode
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSS.Core.Model.Structures;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Structures
{
  public class ArchiveStructureNode
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual Guid EntityId { get; set; }

    public virtual string EntityName { get; set; }

    public virtual string Name { get; set; }

    public virtual string Description { get; set; }

    public virtual StructureNodeTypeEnum NodeType { get; set; }

    public virtual DateTime? StartDate { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual Guid? LockedBy { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
