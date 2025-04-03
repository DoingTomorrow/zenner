// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.JobsManagement.JobsManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Modules.LicenseManagement;
using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Structures;
using MSS.DTO.Jobs;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using Telerik.Windows.Data;
using ZENNER;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.JobsManagement
{
  public class JobsManager
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepository<Scenario> _scenarioRepository;
    private readonly IRepository<ScenarioJobDefinition> _scenarioJobDefinitionRepository;
    private readonly IRepository<JobDefinition> _jobDefinitionRepository;
    private readonly ISession _nhSession;

    public JobsManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._scenarioRepository = repositoryFactory.GetRepository<Scenario>();
      this._scenarioJobDefinitionRepository = repositoryFactory.GetRepository<ScenarioJobDefinition>();
      this._jobDefinitionRepository = repositoryFactory.GetRepository<JobDefinition>();
      this._nhSession = repositoryFactory.GetSession();
    }

    public IEnumerable<ScenarioDTO> GetScenarioDTOs()
    {
      List<ScenarioDTO> list = this._scenarioRepository.GetAll().OrderBy<Scenario, int>((Func<Scenario, int>) (x => x.Code)).Select<Scenario, ScenarioDTO>((Func<Scenario, ScenarioDTO>) (scenario => new ScenarioDTO()
      {
        Id = scenario.Id,
        Code = scenario.Code,
        Name = string.IsNullOrEmpty(CultureResources.GetValue("Scenario_Type_" + (object) scenario.Code)) ? "Scenario_Type_" + (object) scenario.Code : CultureResources.GetValue("Scenario_Type_" + (object) scenario.Code)
      })).ToList<ScenarioDTO>();
      if (!LicenseHelper.LicenseIsDisplayEvaluationFactor())
        list.Remove(list.FirstOrDefault<ScenarioDTO>((Func<ScenarioDTO, bool>) (s => s.Code == 5)));
      return (IEnumerable<ScenarioDTO>) list;
    }

    public IEnumerable<ScenarioJobDefinition> GetScenarioJobDefinitions()
    {
      return (IEnumerable<ScenarioJobDefinition>) this._scenarioJobDefinitionRepository.GetAll();
    }

    public RadObservableCollection<MssReadingJobDto> GetMssReadingJobsDto()
    {
      List<MssReadingJobDto> jobDefCollection = new List<MssReadingJobDto>();
      this._repositoryFactory.GetJobRepository().GetJobs().ForEach((Action<MssReadingJob>) (jd => jobDefCollection.Add(new MssReadingJobDto()
      {
        Id = jd.Id,
        MinomatId = jd.Minomat != null ? jd.Minomat.Id : Guid.Empty,
        SerialNumber = jd.Minomat != null ? jd.Minomat.RadioId : string.Empty,
        StartDate = jd.StartDate,
        EndDate = jd.EndDate,
        JobDefinitionName = jd.JobDefinition.Name,
        ServiceJobName = jd.JobDefinition.ServiceJob,
        StructureNodeId = jd.RootNode != null ? jd.RootNode.Id : Guid.Empty,
        StructureNodeName = jd.RootNode != null ? jd.RootNode.Name : string.Empty,
        ErrorMessage = jd.ErrorMessage,
        LastExecutionDate = new DateTime?(),
        Status = jd.Status.ToString()
      })));
      RadObservableCollection<MssReadingJobDto> mssReadingJobsDto = new RadObservableCollection<MssReadingJobDto>();
      foreach (MssReadingJobDto mssReadingJobDto in jobDefCollection)
        mssReadingJobsDto.Add(mssReadingJobDto);
      return mssReadingJobsDto;
    }

    public RadObservableCollection<JobDefinitionDto> GetJobDefinitionDto(
      bool hasRightServiceJobView = true,
      bool hasRightJobDefinitionView = true)
    {
      List<JobDefinitionDto> jobDefCollection = new List<JobDefinitionDto>();
      IList<JobDefinition> jobDefinitionList = this._jobDefinitionRepository.SearchFor((Expression<Func<JobDefinition, bool>>) (x => x.IsDeactivated == false));
      if (jobDefinitionList == null)
        return (RadObservableCollection<JobDefinitionDto>) null;
      TypeHelperExtensionMethods.ForEach<JobDefinition>((IEnumerable<JobDefinition>) jobDefinitionList, (Action<JobDefinition>) (jd =>
      {
        ServiceTask serviceTask = new ServiceTask();
        MSS.Core.Model.DataFilters.Filter filter = new MSS.Core.Model.DataFilters.Filter();
        bool flag = true;
        if (jd.Filter != null)
        {
          filter = this._repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().FirstOrDefault((Expression<Func<MSS.Core.Model.DataFilters.Filter, bool>>) (x => x.Id == jd.Filter.Id));
        }
        else
        {
          flag = false;
          serviceTask = ServiceTaskManager.GetServices(GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.SystemDevice).FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.Name == jd.System))).OrderBy<ServiceTask, string>((Func<ServiceTask, string>) (s => s.Description)).FirstOrDefault<ServiceTask>((Func<ServiceTask, bool>) (x => x.Method.Name == jd.ServiceJob));
        }
        JobDefinitionDto jobDefinitionDto = new JobDefinitionDto()
        {
          Id = jd.Id,
          System = jd.System,
          EquipmentModel = jd.EquipmentModel,
          FilterName = flag ? filter.Name : (string) null,
          Name = jd.Name,
          StartDate = jd.StartDate,
          Interval = jd.Interval,
          EquipmentParams = jd.EquipmentParams,
          ProfileType = jd.ProfileType,
          ServiceJob = !flag ? serviceTask.Description : (string) null,
          ProfileTypeParams = jd.ProfileTypeParams
        };
        if (!(flag & hasRightJobDefinitionView) && !(!flag & hasRightServiceJobView))
          return;
        jobDefCollection.Add(jobDefinitionDto);
      }));
      RadObservableCollection<JobDefinitionDto> jobDefinitionDto1 = new RadObservableCollection<JobDefinitionDto>();
      foreach (JobDefinitionDto jobDefinitionDto2 in jobDefCollection)
        jobDefinitionDto1.Add(jobDefinitionDto2);
      return jobDefinitionDto1;
    }

    public void UpdateScenario(Guid scenarioId, IEnumerable<JobDefinitionDto> jobDefinitions)
    {
      this._nhSession.FlushMode = FlushMode.Commit;
      ITransaction transaction = this._nhSession.BeginTransaction();
      IRepository<ScenarioJobDefinition> definitionRepository = this._scenarioJobDefinitionRepository;
      Expression<Func<ScenarioJobDefinition, bool>> predicate = (Expression<Func<ScenarioJobDefinition, bool>>) (s => s.Scenario.Id == scenarioId);
      foreach (ScenarioJobDefinition entity in (IEnumerable<ScenarioJobDefinition>) definitionRepository.SearchFor(predicate))
        this._scenarioJobDefinitionRepository.TransactionalDelete(entity);
      foreach (JobDefinitionDto jobDefinition in jobDefinitions)
        this._scenarioJobDefinitionRepository.TransactionalInsert(new ScenarioJobDefinition()
        {
          JobDefinition = this._jobDefinitionRepository.GetById((object) jobDefinition.Id),
          Scenario = this._scenarioRepository.GetById((object) scenarioId)
        });
      transaction.Commit();
    }

    public static byte[] SerializeListOfInts(List<int> ints)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (List<int>));
      MemoryStream memoryStream = new MemoryStream();
      xmlSerializer.Serialize((Stream) memoryStream, (object) ints);
      return memoryStream.ToArray();
    }

    public static List<int> DeserializeListofInts(byte[] bytes)
    {
      return new XmlSerializer(typeof (List<int>)).Deserialize((Stream) new MemoryStream(bytes)) as List<int>;
    }

    public static byte[] SerializeListOfStrings(List<string> strings)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (List<string>));
      MemoryStream memoryStream = new MemoryStream();
      xmlSerializer.Serialize((Stream) memoryStream, (object) strings);
      return memoryStream.ToArray();
    }

    public static List<string> DeserializeListofStrings(byte[] bytes)
    {
      return new XmlSerializer(typeof (List<string>)).Deserialize((Stream) new MemoryStream(bytes)) as List<string>;
    }

    public static byte[] SerializeIntervals(IntervalDto interval)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (IntervalDto));
      MemoryStream memoryStream = new MemoryStream();
      xmlSerializer.Serialize((Stream) memoryStream, (object) interval);
      return memoryStream.ToArray();
    }

    public static IntervalDto DeserializeIntervals(byte[] bytes)
    {
      return new XmlSerializer(typeof (IntervalDto)).Deserialize((Stream) new MemoryStream(bytes)) as IntervalDto;
    }

    public List<MssReadingJob> GetActiveJobs()
    {
      IList<MssReadingJob> mssReadingJobList = this._repositoryFactory.GetRepository<MssReadingJob>().SearchFor((Expression<Func<MssReadingJob, bool>>) (j => j.IsDeactivated == false && j.EndDate == new DateTime?()));
      List<MssReadingJob> source = new List<MssReadingJob>();
      foreach (MssReadingJob mssReadingJob in (IEnumerable<MssReadingJob>) mssReadingJobList)
      {
        IntervalDto intervalDto = JobsManager.DeserializeIntervals(mssReadingJob.JobDefinition.Interval);
        DateTime? nullable = intervalDto.StartDate;
        DateTime now1 = DateTime.Now;
        int num;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() < now1 ? 1 : 0) : 0) != 0)
        {
          nullable = intervalDto.EndDate;
          DateTime now2 = DateTime.Now;
          num = nullable.HasValue ? (nullable.GetValueOrDefault() > now2 ? 1 : 0) : 0;
        }
        else
          num = 0;
        if (num != 0)
          source.Add(mssReadingJob);
      }
      return source.ToList<MssReadingJob>();
    }

    public Job MapMssJobToGmmJob(MssReadingJob job)
    {
      IntervalDto intervalDto = JobsManager.DeserializeIntervals(job.JobDefinition.Interval);
      Scheduler.TriggerItem triggerItem = new Scheduler.TriggerItem();
      triggerItem.StartDate = intervalDto.StartDate.Value;
      triggerItem.EndDate = intervalDto.EndDate.Value;
      triggerItem.TriggerTime = intervalDto.Time.Value;
      triggerItem.TriggerSettings = new Scheduler.TriggerSettings()
      {
        Daily = new Scheduler.TriggerSettingsDaily()
        {
          Interval = (ushort) intervalDto.RepeatIn
        },
        FixedInterval = new Scheduler.TriggerSettingsFixedInterval()
        {
          Interval = intervalDto.FixedInterval.HasValue ? intervalDto.FixedInterval.Value - intervalDto.FixedInterval.Value.Date : TimeSpan.Zero
        },
        OneTimeOnly = new Scheduler.TriggerSettingsOneTimeOnly()
        {
          Date = intervalDto.OneTimeDate
        },
        Weekly = new Scheduler.TriggerSettingsWeekly(),
        Monthly = new Scheduler.TriggerSettingsMonthly()
      };
      bool[] flagArray1 = new bool[7];
      for (int index = 0; index < intervalDto.WeeklyDays.Count; ++index)
        flagArray1[intervalDto.WeeklyDays[index]] = true;
      triggerItem.TriggerSettings.Weekly.DaysOfWeek = flagArray1;
      bool[] flagArray2 = new bool[12];
      for (int index = 0; index < intervalDto.MonthlyMonth.Count; ++index)
        flagArray2[intervalDto.MonthlyMonth[index]] = true;
      triggerItem.TriggerSettings.Monthly.Month = flagArray2;
      bool[] flagArray3 = new bool[32];
      for (int index = 0; index < intervalDto.MonthlyCardinalDay.Count; ++index)
        flagArray3[intervalDto.MonthlyCardinalDay[index]] = true;
      triggerItem.TriggerSettings.Monthly.DaysOfMonth = flagArray3;
      triggerItem.TriggerSettings.Monthly.WeekDay = new Scheduler.TriggerSettingsMonthlyWeekDay();
      bool[] flagArray4 = new bool[7];
      for (int index = 0; index < intervalDto.MonthlyWeekDay.Count; ++index)
        flagArray4[intervalDto.MonthlyWeekDay[index]] = true;
      triggerItem.TriggerSettings.Monthly.WeekDay.DayOfWeek = flagArray4;
      bool[] flagArray5 = new bool[5];
      for (int index = 0; index < intervalDto.MonthlyOrdinalDay.Count; ++index)
        flagArray5[intervalDto.MonthlyOrdinalDay[index]] = true;
      triggerItem.TriggerSettings.Monthly.WeekDay.WeekNumber = flagArray5;
      triggerItem.Enabled = true;
      List<long> valueIdentList = new List<long>();
      if (job.JobDefinition.Filter != null && job.JobDefinition.Filter.Rules != null)
        TypeHelperExtensionMethods.ForEach<Rules>((IEnumerable<Rules>) job.JobDefinition.Filter.Rules, (Action<Rules>) (f => valueIdentList.Add(long.Parse(f.ValueId))));
      DeviceModel model = GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.SystemDevice).FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.Name == job.JobDefinition.System));
      Job gmmJob = new Job()
      {
        System = model,
        Interval = triggerItem,
        Filter = valueIdentList,
        Equipment = GmmInterface.DeviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.Name == job.JobDefinition.EquipmentModel)),
        ServiceTask = job.JobDefinition.ServiceJob != null ? ServiceTaskManager.GetServices(model).Find((Predicate<ServiceTask>) (x => x.Method.Name == job.JobDefinition.ServiceJob)) : (ServiceTask) null,
        JobID = job.Id,
        StoreResultsToDatabase = job.RootNode != null || job.JobDefinition.ServiceJob != null
      };
      if (!string.IsNullOrEmpty(job.JobDefinition.EquipmentParams))
        MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateEquipmentWithSavedParams(gmmJob.Equipment, job.JobDefinition.EquipmentParams);
      gmmJob.ProfileType = GmmInterface.DeviceManager.GetProfileTypes(gmmJob.System, gmmJob.Equipment).FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (p => p.Name == job.JobDefinition.ProfileType));
      if (job.Minomat != null && job.JobDefinition.System == "Minomat V4 Master")
        this.SetChangeableParametersForMinomat(gmmJob.System, job);
      int num = job.Minomat == null ? 0 : (job.Minomat.LoggingEnabled ? 1 : 0);
      gmmJob.LoggingToFileEnabled = num != 0;
      return gmmJob;
    }

    private void SetChangeableParametersForMinomat(DeviceModel system, MssReadingJob job)
    {
      system.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (p => p.Key == "MinomatV4_MinolID")).Value = job.Minomat.RadioId;
      system.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (p => p.Key == "MinomatV4_Challenge")).Value = job.Minomat.Challenge;
      system.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (p => p.Key == "MinomatV4_GSM_ID")).Value = job.Minomat.GsmId;
      system.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (p => p.Key == "MinomatV4_SessionKey")).Value = job.Minomat.SessionKey;
      JobDefinition jobDefinition = job.JobDefinition;
      system.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationDueDate")).Value = jobDefinition.DueDate.Value.ToString().Replace(".", ":");
      system.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationMonth")).Value = jobDefinition.Month.Value.ToString().Replace(".", ":");
      system.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationDay")).Value = jobDefinition.Day.Value.ToString().Replace(".", ":");
      system.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationQuarterHour")).Value = jobDefinition.QuarterHour.Value.ToString().Replace(".", ":");
    }
  }
}
