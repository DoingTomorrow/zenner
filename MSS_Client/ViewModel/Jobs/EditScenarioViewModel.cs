// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.EditScenarioViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.GMM;
using MSS.Business.Modules.JobsManagement;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Jobs;
using MSS.DIConfiguration;
using MSS.DTO.Jobs;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate.Linq;
using Ninject;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class EditScenarioViewModel : ViewModelBase
  {
    private readonly Guid _scenarioId;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly ISessionManager _sessionManager;
    private readonly string _scenarioName = string.Empty;
    private RadObservableCollection<JobDefinitionDto> _jobDefinitionList;
    private RadObservableCollection<JobDefinitionDto> _scenarioJobDefinitionList = new RadObservableCollection<JobDefinitionDto>();

    public string ScenarioName => this._scenarioName;

    [Inject]
    public EditScenarioViewModel(
      ScenarioDTO sm,
      IRepositoryFactory repositoryFactory,
      ISessionManager sessionManager)
    {
      this._scenarioId = sm.Id;
      this._repositoryFactory = repositoryFactory;
      this._sessionManager = sessionManager;
      this._scenarioName = sm.Name;
      IEnumerable<ScenarioJobDefinition> scenarioJobDefinitions = this.GetJobsManagerInstance().GetScenarioJobDefinitions();
      this._jobDefinitionList = this.GetJobsManagerInstance().GetJobDefinitionDto();
      foreach (ScenarioJobDefinition scenarioJobDefinition1 in scenarioJobDefinitions)
      {
        ScenarioJobDefinition scenarioJobDefinition = scenarioJobDefinition1;
        if (!(scenarioJobDefinition.Scenario.Id != sm.Id))
        {
          JobDefinitionDto jobDefinitionDto = this.JobDefinitionList.FirstOrDefault<JobDefinitionDto>((Func<JobDefinitionDto, bool>) (op => op.Id == scenarioJobDefinition.JobDefinition.Id));
          this.ScenarioJobDefinitionList.Add(jobDefinitionDto);
          this.JobDefinitionList.Remove(jobDefinitionDto);
        }
      }
    }

    public RadObservableCollection<JobDefinitionDto> JobDefinitionList
    {
      get => this._jobDefinitionList;
      set
      {
        this._jobDefinitionList = value;
        this.OnPropertyChanged(nameof (JobDefinitionList));
      }
    }

    public RadObservableCollection<JobDefinitionDto> ScenarioJobDefinitionList
    {
      get => this._scenarioJobDefinitionList;
      set
      {
        this._scenarioJobDefinitionList = value;
        this.OnPropertyChanged(nameof (ScenarioJobDefinitionList));
      }
    }

    public ICommand EditScenarioCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          JobsManager jobsManagerInstance = this.GetJobsManagerInstance();
          List<Guid> currentJobDefinitions = this._repositoryFactory.GetRepository<ScenarioJobDefinition>().SearchFor((Expression<Func<ScenarioJobDefinition, bool>>) (sc => sc.Scenario.Id == this._scenarioId)).Select<ScenarioJobDefinition, Guid>((Func<ScenarioJobDefinition, Guid>) (x => x.JobDefinition.Id)).ToList<Guid>();
          jobsManagerInstance.UpdateScenario(this._scenarioId, this.ScenarioJobDefinitionList.ToEnumerable<JobDefinitionDto>());
          List<Guid> scenarioJobDefinitionIds = this.ScenarioJobDefinitionList.Select<JobDefinitionDto, Guid>((Func<JobDefinitionDto, Guid>) (x => x.Id)).ToList<Guid>();
          List<MssReadingJob> jobs = new List<MssReadingJob>();
          List<MssReadingJob> list1 = this._repositoryFactory.GetRepository<MssReadingJob>().SearchFor((Expression<Func<MssReadingJob, bool>>) (j => j.Scenario.Id == this._scenarioId && j.EndDate == new DateTime?() && !scenarioJobDefinitionIds.Contains(j.JobDefinition.Id))).ToList<MssReadingJob>();
          if (list1.Any<MssReadingJob>())
            jobs.AddRange((IEnumerable<MssReadingJob>) list1);
          List<Guid> jobDefinitionForCreateNew = new List<Guid>();
          TypeHelperExtensionMethods.ForEach<JobDefinitionDto>((IEnumerable<JobDefinitionDto>) this.ScenarioJobDefinitionList, (Action<JobDefinitionDto>) (x =>
          {
            if (currentJobDefinitions.Contains(x.Id))
              return;
            jobDefinitionForCreateNew.Add(x.Id);
          }));
          List<Minomat> list2 = this._repositoryFactory.GetRepository<Minomat>().SearchFor((Expression<Func<Minomat, bool>>) (x => x.Scenario.Id == this._scenarioId)).ToList<Minomat>();
          GMMJobsManager.Instance(DIConfigurator.GetConfigurator().Get<IRepositoryFactoryCreator>(), false).FinalizeJobs((IEnumerable<MssReadingJob>) jobs, true);
          new ManageMSSReadingJobs(this._repositoryFactory).CreateJobs(list2, jobDefinitionForCreateNew, this._scenarioId);
          this.OnRequestClose(true);
        }));
      }
    }

    private JobsManager GetJobsManagerInstance() => new JobsManager(this._repositoryFactory);
  }
}
