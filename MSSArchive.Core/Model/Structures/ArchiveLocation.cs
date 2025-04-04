// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Structures.ArchiveLocation
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Structures;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Structures
{
  public class ArchiveLocation
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual string City { get; set; }

    public virtual string Street { get; set; }

    public virtual string ZipCode { get; set; }

    public virtual string BuildingNr { get; set; }

    public virtual string Description { get; set; }

    public virtual GenerationEnum Generation { get; set; }

    public virtual string ScenarioCode { get; set; }

    public virtual DateTime DueDate { get; set; }

    public virtual ScaleEnum Scale { get; set; }

    public virtual bool HasMaster { get; set; }

    public virtual bool IsDeactivated { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
