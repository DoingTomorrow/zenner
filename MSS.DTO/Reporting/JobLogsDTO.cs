// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Reporting.JobLogsDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Reporting;
using System;

#nullable disable
namespace MSS.DTO.Reporting
{
  public class JobLogsDTO : DTOBase
  {
    public virtual Guid Id { get; set; }

    public virtual string JobEntityNumber { get; set; }

    public virtual string JobName { get; set; }

    public virtual DateTime? StartDate { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual bool Active { get; set; }

    public virtual JobLogStatusEnum Status { get; set; }

    public virtual string Message { get; set; }
  }
}
