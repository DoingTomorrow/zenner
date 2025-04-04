// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Archive.ArchiveMinomatDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.Archive
{
  public class ArchiveMinomatDTO : DTOBase
  {
    public Guid Id { get; set; }

    public string AccessPoint { get; set; }

    public string Challenge { get; set; }

    public string CreatedBy { get; set; }

    public string GsmId { get; set; }

    public string HostAndPort { get; set; }

    public string LastUpdatedBy { get; set; }

    public string LockedBy { get; set; }

    public string MasterRadioId { get; set; }

    public string ProviderName { get; set; }

    public string SessionKey { get; set; }

    public string SimPin { get; set; }

    public string Status { get; set; }

    public string Url { get; set; }

    public string UserId { get; set; }

    public string UserPassword { get; set; }

    public virtual DateTime? CreatedOn { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual bool IsDeactivated { get; set; }

    public virtual bool IsInMasterPool { get; set; }

    public virtual bool IsMaster { get; set; }

    public virtual DateTime? LastUpdatedOn { get; set; }

    public virtual int Polling { get; set; }

    public virtual bool Registered { get; set; }

    public virtual DateTime? StartDate { get; set; }
  }
}
