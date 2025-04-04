// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Minomat.MinomatDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using System;

#nullable disable
namespace MSS.DTO.Minomat
{
  public class MinomatDTO : DTOBase
  {
    private int _idEnumStatus;
    private bool _loggingEnabled;
    public string RadioIdMaster;
    private MinomatNetworkStatusEnum _networkStatus;

    public Guid Id { get; set; }

    public string RadioId { get; set; }

    public int idEnumStatus
    {
      get => this._idEnumStatus;
      set
      {
        this._idEnumStatus = value;
        this.OnPropertyChanged(nameof (idEnumStatus));
      }
    }

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

    public virtual bool IsInMasterPool { get; set; }

    public string ProviderName { get; set; }

    public string SimPin { get; set; }

    public string AccessPoint { get; set; }

    public string UserId { get; set; }

    public string UserPassword { get; set; }

    public virtual string SessionKey { get; set; }

    public string CreatedByName { get; set; }

    public Scenario Scenario { get; set; }

    public Provider Provider { get; set; }

    public Country Country { get; set; }

    public bool LoggingEnabled
    {
      get => this._loggingEnabled;
      set
      {
        this._loggingEnabled = value;
        this.OnPropertyChanged(nameof (LoggingEnabled));
      }
    }

    public string SimCardNumber { get; set; }

    public MinomatRadioDetails RadioDetails { get; set; }

    public string LocationDescription { get; set; }

    public MinomatNetworkStatusEnum NetworkStatus
    {
      get => this._networkStatus;
      set
      {
        this._networkStatus = value;
        this.OnPropertyChanged(nameof (NetworkStatus));
      }
    }
  }
}
