// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.JobLogsForJobViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.JobsManagement;
using MSS.DTO.Jobs;
using MSS.DTO.Reporting;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.ViewModel;
using NHibernate.Linq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class JobLogsForJobViewModel : ValidationViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private MssReadingJobDto _jobDto;
    private ObservableCollection<JobLogsDTO> _jobLogDTO;
    private string _title;
    private string _pageSize;

    [Inject]
    public JobLogsForJobViewModel(IRepositoryFactory repositoryFactory, MssReadingJobDto jobDto)
    {
      this._repositoryFactory = repositoryFactory;
      this._jobDto = jobDto;
      EventPublisher.Register<JobStateModified>(new Action<JobStateModified>(this.OnJobStartedOrEnded));
      this._pageSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
      this.InitJobLogsList();
      this.Title = Resources.MSS_Client_JobLogsTitle + "   " + jobDto?.JobDefinitionName + " / " + jobDto?.StructureNodeName;
    }

    private void OnJobStartedOrEnded(JobStateModified jobStateModified)
    {
      Guid jobId = jobStateModified.JobId;
      Guid? id = this._jobDto?.Id;
      if (!id.HasValue || !(jobId == id.GetValueOrDefault()))
        return;
      this.InitJobLogsList();
    }

    private void InitJobLogsList()
    {
      JobLogsManager jobLogsManager = new JobLogsManager(this._repositoryFactory);
      IOrderedEnumerable<JobLogsDTO> orderedEnumerable = this._jobDto != null ? jobLogsManager.LoadJobLogs(this._jobDto.Id, new DateTime?(DateTime.MinValue), new DateTime?(DateTime.MaxValue)).OrderByDescending<JobLogsDTO, DateTime?>((Func<JobLogsDTO, DateTime?>) (item => item.StartDate)) : (IOrderedEnumerable<JobLogsDTO>) null;
      this._jobLogDTO = new ObservableCollection<JobLogsDTO>();
      if (orderedEnumerable != null)
        TypeHelperExtensionMethods.ForEach<JobLogsDTO>((IEnumerable<JobLogsDTO>) orderedEnumerable, (Action<JobLogsDTO>) (item => this.JobLogDTOs.Add(item)));
      this.OnPropertyChanged("JobLogDTOs");
    }

    public ObservableCollection<JobLogsDTO> JobLogDTOs
    {
      get => this._jobLogDTO;
      set
      {
        this._jobLogDTO = value;
        this.OnPropertyChanged(nameof (JobLogDTOs));
      }
    }

    public string Title
    {
      get => this._title;
      set
      {
        this._title = value;
        this.OnPropertyChanged(nameof (Title));
      }
    }

    public string PageSize
    {
      get => this._pageSize;
      set
      {
        this._pageSize = value;
        this.OnPropertyChanged(nameof (PageSize));
      }
    }
  }
}
