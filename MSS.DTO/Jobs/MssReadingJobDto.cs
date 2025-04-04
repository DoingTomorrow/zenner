// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Jobs.MssReadingJobDto
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.Jobs
{
  public class MssReadingJobDto : DTOBase
  {
    private string _status;

    public Guid Id { get; set; }

    public Guid MinomatId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsDeactivated { get; set; }

    public string JobDefinitionName { get; set; }

    public Guid StructureNodeId { get; set; }

    public string ServiceJobName { get; set; }

    public string SerialNumber { get; set; }

    public string StructureNodeName { get; set; }

    public DateTime? LastExecutionDate { get; set; }

    public string ErrorMessage { get; set; }

    public string Status
    {
      get => this._status;
      set
      {
        this._status = value;
        this.OnPropertyChanged(nameof (Status));
      }
    }
  }
}
