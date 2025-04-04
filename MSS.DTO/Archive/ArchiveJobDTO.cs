// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Archive.ArchiveJobDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Archiving;
using MSS.Core.Model.Reporting;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.DTO.Archive
{
  public class ArchiveJobDTO
  {
    public Guid Id { get; set; }

    public AutomatedExportJobPeriodicityEnum Periodicity { get; set; }

    public DateTime StartDate { get; set; }

    public string ArchiveName { get; set; }

    public bool DeleteAfterArchive { get; set; }

    public int NumberOfDaysToExport { get; set; }

    public DateTime CreatedOn { get; set; }

    public string ArchivedEntities { get; set; }

    public List<ArchiveEntity> ArchivedEntitiesList { get; set; }

    public virtual DateTime? LastExecutionDate { get; set; }
  }
}
