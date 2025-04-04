// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Archiving.ArchiveInformation
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using System;

#nullable disable
namespace MSSArchive.Core.Model.Archiving
{
  public class ArchiveInformation
  {
    public virtual Guid Id { get; set; }

    public virtual string ArchiveName { get; set; }

    public virtual DateTime DateTime { get; set; }

    public virtual DateTime StartTime { get; set; }

    public virtual DateTime EndTime { get; set; }

    public virtual string ArchivedEntities { get; set; }
  }
}
