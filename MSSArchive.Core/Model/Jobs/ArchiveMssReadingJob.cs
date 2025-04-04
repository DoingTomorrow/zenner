// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Jobs.ArchiveMssReadingJob
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSS.Business.Utils;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Jobs
{
  public class ArchiveMssReadingJob
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual DateTime? StartDate { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual bool IsDeactivated { get; set; }

    public virtual Guid JobDefinitionId { get; set; }

    public virtual string JobDefinitionName { get; set; }

    public virtual string JobDefinitionEquipmentModel { get; set; }

    public virtual string JobDefinitionServiceJob { get; set; }

    public virtual string JobDefinitionEquipmentParams { get; set; }

    public virtual string JobDefinitionSystem { get; set; }

    public virtual string JobDefinitionProfileType { get; set; }

    public virtual DateTime? JobDefinitionStartDate { get; set; }

    public virtual DateTime? JobDefinitionEndDate { get; set; }

    public virtual bool JobDefinitionIsDeactivated { get; set; }

    public virtual string JobDefinitionRules { get; set; }

    public virtual byte[] JobDefinitionInterval { get; set; }

    public virtual string Scenario { get; set; }

    public virtual string Minomat { get; set; }

    public virtual string RootNode { get; set; }

    public virtual bool IsUpdate { get; set; }

    public virtual DateTime? LastExecutionDate { get; set; }

    public virtual string ErrorMessage { get; set; }

    public virtual JobStatusEnum Status { get; set; }

    public virtual DateTime CreatedOn { get; set; }

    public virtual DateTime? LastUpdatedOn { get; set; }
  }
}
