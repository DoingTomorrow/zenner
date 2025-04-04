// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Structures.ArchiveTenant
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Structures
{
  public class ArchiveTenant
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual int TenantNr { get; set; }

    public virtual string Name { get; set; }

    public virtual string FloorNr { get; set; }

    public virtual string FloorName { get; set; }

    public virtual string ApartmentNr { get; set; }

    public virtual string Description { get; set; }

    public virtual bool IsGroup { get; set; }

    public virtual string CustomerTenantNo { get; set; }

    public virtual bool IsDeactivated { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
