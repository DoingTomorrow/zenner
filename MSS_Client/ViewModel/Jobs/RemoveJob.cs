// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.RemoveJob
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Core.Model.Jobs;
using MSS.DTO.Jobs;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class RemoveJob : ViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private readonly MssReadingJobDto _job;

    public RemoveJob(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      MssReadingJobDto job)
    {
      this._job = job;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this.ConfirmJobDescriptionDeleteDialog = string.Format(Resources.MSS_Client_Job_Remove_Confirmation);
    }

    public string ConfirmJobDescriptionDeleteDialog { get; set; }

    public ICommand RemoveFilterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (jobDefinition =>
        {
          MssReadingJob byId = this._repositoryFactory.GetRepository<MssReadingJob>().GetById((object) this._job.Id);
          this.OnRequestClose(true);
          EventPublisher.Publish<RemoveJobEvent>(new RemoveJobEvent()
          {
            Job = byId
          }, (IViewModel) this);
        }));
      }
    }
  }
}
