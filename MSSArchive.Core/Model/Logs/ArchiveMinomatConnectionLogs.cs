// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Logs.ArchiveMinomatConnectionLogs
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSS.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Logs
{
  public class ArchiveMinomatConnectionLogs
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual string MinolId { get; set; }

    public virtual bool IsTestConnection { get; set; }

    public virtual string GsmID { get; set; }

    public virtual string ChallengeKey { get; set; }

    public virtual DateTime TimePoint { get; set; }

    public virtual string SessionKey { get; set; }

    public virtual string ScenarioNumber { get; set; }

    public virtual Guid? MinomatId { get; set; }

    public virtual DateTime? LastDCUConnectionMDMExportOn { get; set; }

    [ExcludeProperty]
    public virtual ArchiveJob ArchiveJob { get; set; }
  }
}
