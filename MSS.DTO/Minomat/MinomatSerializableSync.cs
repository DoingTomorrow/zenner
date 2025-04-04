// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Minomat.MinomatSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Minomat
{
  public class MinomatSerializableSync : ISerializableObject
  {
    public Guid Id { get; set; }

    public string RadioId { get; set; }

    public string Status { get; set; }

    public bool Registered { get; set; }

    public string HostAndPort { get; set; }

    public string Url { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string LastUpdatedBy { get; set; }

    public DateTime? LastChangedOn { get; set; }

    public string Challenge { get; set; }

    public string GsmId { get; set; }

    public bool IsDeactivated { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int Polling { get; set; }

    public bool IsMaster { get; set; }

    public bool IsInMasterPool { get; set; }

    public string ProviderName { get; set; }

    public string SimPin { get; set; }

    public string AccessPoint { get; set; }

    public string UserId { get; set; }

    public string UserPassword { get; set; }

    public string SessionKey { get; set; }

    public string CommParameter { get; set; }

    public string CreatedByName { get; set; }

    public Guid? ScenarioId { get; set; }

    public Guid? ProviderId { get; set; }

    public Guid? CountryId { get; set; }

    public bool LoggingEnabled { get; set; }

    public string RadioIdMaster { get; set; }

    public string SimCardNumber { get; set; }
  }
}
