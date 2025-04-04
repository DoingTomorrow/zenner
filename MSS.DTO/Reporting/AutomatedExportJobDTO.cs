// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Reporting.AutomatedExportJobDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Reporting;
using System;

#nullable disable
namespace MSS.DTO.Reporting
{
  public class AutomatedExportJobDTO
  {
    public AutomatedExportJobDTO() => this.StartDate = DateTime.Now;

    public Guid Id { get; set; }

    public AutomatedExportJobPeriodicityEnum Periodicity { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? LastExecutionTime { get; set; }

    public bool ArchiveAfterExport { get; set; }

    public bool DeleteAfterExport { get; set; }

    public DataToExport DataToExport { get; set; }

    public string ExportedDataFormatted { get; set; }

    public string ExportFor { get; set; }

    public string ExportedFileType { get; set; }

    public string DecimalSeparator { get; set; }

    public string ValueSeparator { get; set; }

    public string ExportPath { get; set; }
  }
}
