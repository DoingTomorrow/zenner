// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Reporting.ArchiveJobLogs
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSS.Core.Model.Reporting;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Jobs;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Reporting
{
  public class ArchiveJobLogs
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual string JobEntityNumber { get; set; }

    public virtual ArchiveMssReadingJob Job { get; set; }

    public virtual DateTime CreatedOn { get; set; }

    public virtual DateTime? StartDate { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual bool Active { get; set; }

    public virtual JobLogStatusEnum Status { get; set; }

    public virtual string Message { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
