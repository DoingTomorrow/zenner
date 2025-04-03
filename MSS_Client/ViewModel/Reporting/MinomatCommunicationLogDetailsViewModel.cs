// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Reporting.MinomatCommunicationLogDetailsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.DTO.Reporting;
using MVVM.ViewModel;
using Ninject;
using System;

#nullable disable
namespace MSS_Client.ViewModel.Reporting
{
  public class MinomatCommunicationLogDetailsViewModel : ViewModelBase
  {
    private readonly MinomatCommunicationLogDTO _selectedMinomatComLog;

    [Inject]
    public MinomatCommunicationLogDetailsViewModel(MinomatCommunicationLogDTO selectedMinomatComLog)
    {
      this._selectedMinomatComLog = selectedMinomatComLog;
    }

    public string MasterRadioId => this._selectedMinomatComLog.MasterRadioId;

    public DateTime TimePoint => this._selectedMinomatComLog.TimePoint;

    public string GsmID => this._selectedMinomatComLog.GsmID;

    public string SessionKey => this._selectedMinomatComLog.SessionKey;

    public string ChallengeKey => this._selectedMinomatComLog.ChallengeKey;

    public string RawData => this._selectedMinomatComLog.RawData;

    public string SCGICommand => this._selectedMinomatComLog.SCGICommand;

    public bool IsIncoming => this._selectedMinomatComLog.IsIncoming;
  }
}
