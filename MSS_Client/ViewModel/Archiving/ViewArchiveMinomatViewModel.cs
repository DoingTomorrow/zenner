// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.ViewArchiveMinomatViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.DTO.Archive;
using MSS.Localisation;
using Ninject;
using System;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  internal class ViewArchiveMinomatViewModel : ViewArchiveEntityViewModel
  {
    private ArchiveMinomatDTO _Entity { get; }

    [Inject]
    public ViewArchiveMinomatViewModel(ArchiveMinomatDTO entity)
    {
      this._Entity = entity;
      this.ArchiveViewEntityDialogTitle = CultureResources.GetValue("MSS_Client_Archiving_ViewMinomat");
      this.InitializeData(this._Entity);
    }

    private void InitializeData(ArchiveMinomatDTO entity)
    {
      this.MasterRadioId = entity.MasterRadioId;
      this.IsMaster = entity.IsMaster;
      this.IsInMasterPool = entity.IsInMasterPool;
      this.ProviderName = entity.ProviderName;
      this.SimPin = entity.SimPin;
      this.AccessPoint = entity.AccessPoint;
      this.UserId = entity.UserId;
      this.UserPassword = entity.UserPassword;
      this.Challenge = entity.Challenge;
      this.GsmId = entity.GsmId;
      this.SessionKey = entity.SessionKey;
      this.Polling = new int?(entity.Polling);
      this.HostAndPort = entity.HostAndPort;
      this.Url = entity.Url;
      this.Status = entity.Status;
      this.StartDate = entity.StartDate;
      this.EndDate = entity.EndDate;
      this.Registered = entity.Registered;
    }

    public bool IsInMasterPool { get; set; }

    public bool IsMaster { get; set; }

    public string Status { get; set; }

    public string MasterRadioId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string SelectedProvider { get; set; }

    public string SimPin { get; set; }

    public string AccessPoint { get; set; }

    public string UserId { get; set; }

    public string UserPassword { get; set; }

    public bool Registered { get; set; }

    public string Challenge { get; set; }

    public string GsmId { get; set; }

    public int? Polling { get; set; }

    public string HostAndPort { get; set; }

    public string Url { get; set; }

    public string SessionKey { get; set; }

    public string ProviderName { get; set; }
  }
}
