// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Reporting.MinomatCommunicationLogDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.Reporting
{
  public class MinomatCommunicationLogDTO
  {
    public virtual Guid Id { get; set; }

    public virtual string MasterRadioId { get; set; }

    public virtual DateTime TimePoint { get; set; }

    public virtual string GsmID { get; set; }

    public virtual string SessionKey { get; set; }

    public virtual string ChallengeKey { get; set; }

    public virtual string RawData { get; set; }

    public virtual bool IsIncoming { get; set; }

    public virtual string SCGICommand { get; set; }
  }
}
