// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.DataCollectors.ArchiveMinomat
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.DataCollectors
{
  public class ArchiveMinomat
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual string AccessPoint { get; set; }

    public virtual string Challenge { get; set; }

    public virtual string CreatedBy { get; set; }

    public virtual string GsmId { get; set; }

    public virtual string HostAndPort { get; set; }

    public virtual string LastUpdatedBy { get; set; }

    public virtual string LockedBy { get; set; }

    public virtual string MasterRadioId { get; set; }

    public virtual string ProviderName { get; set; }

    public virtual string SessionKey { get; set; }

    public virtual string SimPin { get; set; }

    public virtual string Status { get; set; }

    public virtual string Url { get; set; }

    public virtual string UserId { get; set; }

    public virtual string UserPassword { get; set; }

    public virtual DateTime? CreatedOn { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual bool IsDeactivated { get; set; }

    public virtual bool IsInMasterPool { get; set; }

    public virtual bool IsMaster { get; set; }

    public virtual DateTime? LastUpdatedOn { get; set; }

    public virtual int Polling { get; set; }

    public virtual bool Registered { get; set; }

    public virtual DateTime? StartDate { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
