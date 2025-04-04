// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.MinomatSerializableDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.DataCollectors;
using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class MinomatSerializableDTO : DTOBase
  {
    private MinomatNetworkStatusEnum _networkStatus;

    public virtual Guid Id { get; set; }

    public virtual string RadioId { get; set; }

    public virtual string Status { get; set; }

    public virtual bool Registered { get; set; }

    public virtual string HostAndPort { get; set; }

    public virtual string Url { get; set; }

    public virtual string CreatedBy { get; set; }

    public virtual DateTime? CreatedOn { get; set; }

    public virtual string LastUpdatedBy { get; set; }

    public virtual DateTime? LastChangedOn { get; set; }

    public virtual string Challenge { get; set; }

    public virtual string GsmId { get; set; }

    public virtual bool IsDeactivated { get; set; }

    public virtual DateTime? StartDate { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual int Polling { get; set; }

    public virtual bool IsMaster { get; set; }

    public virtual bool IsInMasterPool { get; set; }

    public string ProviderName { get; set; }

    public string SimPin { get; set; }

    public string AccessPoint { get; set; }

    public string UserId { get; set; }

    public string UserPassword { get; set; }

    public virtual string SessionKey { get; set; }

    public virtual Guid? ScenarioId { get; set; }

    public virtual Guid ProviderId { get; set; }

    public virtual Guid CountryId { get; set; }

    public virtual Guid? MinomatMasterId { get; set; }

    public virtual string Text { get; set; }

    public virtual string MinomatStatus { get; set; }

    public virtual byte[] ReadingValues { get; set; }

    public virtual bool IsTestMaster { get; set; }

    public virtual string NrOfRegisteredDevices { get; set; }

    public MinomatNetworkStatusEnum NetworkStatus
    {
      get => this._networkStatus;
      set
      {
        this._networkStatus = value;
        this.OnPropertyChanged(nameof (NetworkStatus));
      }
    }

    public virtual string CelestaId { get; set; }

    public string SimCardNumber { get; set; }
  }
}
