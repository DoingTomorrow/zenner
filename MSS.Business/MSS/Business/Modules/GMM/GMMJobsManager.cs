// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.GMMJobsManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Events;
using MSS.Business.Modules.JobsManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Reporting;
using MSS.Core.Model.Structures;
using MSS.DTO.Jobs;
using MSS.Interfaces;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class GMMJobsManager
  {
    private IRepositoryFactoryCreator _repositoryFactoryCreator;
    private bool _startListener;
    private readonly JobManager _zennerJobManager;
    private static GMMJobsManager _instance;

    private GMMJobsManager(IRepositoryFactoryCreator repositoryFactoryCreator, bool startListener)
    {
      this._repositoryFactoryCreator = repositoryFactoryCreator;
      this._startListener = startListener;
      this._zennerJobManager = GmmInterface.JobManager;
      this._zennerJobManager.OnError += new EventHandler<Exception>(this.JobManager_OnError);
      this._zennerJobManager.OnJobStarted += new EventHandler<Job>(this.JobManager_JobStarted);
      this._zennerJobManager.OnJobCompleted += new EventHandler<Job>(this.JobManager_JobCompleted);
      this._zennerJobManager.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.JobManager_ValueIdentSetReceived);
      this._zennerJobManager.OnMinomatConnected += new EventHandler<MinomatDevice>(this.JobManager_OnMinomatConnected);
      if (!startListener)
        return;
      GMMJobsLogger.LogDebug("START LISTENER FROM CONSTRUCTOR!");
      this._zennerJobManager.StartListener();
    }

    ~GMMJobsManager()
    {
      this._zennerJobManager.OnError -= new EventHandler<Exception>(this.JobManager_OnError);
      this._zennerJobManager.OnJobStarted -= new EventHandler<Job>(this.JobManager_JobStarted);
      this._zennerJobManager.OnJobCompleted -= new EventHandler<Job>(this.JobManager_JobCompleted);
      this._zennerJobManager.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.JobManager_ValueIdentSetReceived);
      this._zennerJobManager.OnMinomatConnected -= new EventHandler<MinomatDevice>(this.JobManager_OnMinomatConnected);
      if (!this._startListener)
        return;
      this._zennerJobManager.StopListener();
    }

    public static GMMJobsManager Instance(
      IRepositoryFactoryCreator repositoryFactoryCreator,
      bool startListener)
    {
      lock (typeof (GMMJobsManager))
      {
        if (GMMJobsManager._instance != null)
          GMMJobsManager._instance._repositoryFactoryCreator = repositoryFactoryCreator;
        else
          GMMJobsManager._instance = new GMMJobsManager(repositoryFactoryCreator, startListener);
        return GMMJobsManager._instance;
      }
    }

    public void UpdateJobsRelatedToJobDefinition(Guid jobDefinition)
    {
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      lock (repositoryFactory)
      {
        try
        {
          ManageMSSReadingJobs manageMssReadingJobs = new ManageMSSReadingJobs(repositoryFactory);
          List<MssReadingJob> list = repositoryFactory.GetRepository<MssReadingJob>().SearchFor((Expression<Func<MssReadingJob, bool>>) (job => job.JobDefinition.Id == jobDefinition && job.EndDate == new DateTime?())).ToList<MssReadingJob>();
          list.ForEach(new Action<MssReadingJob>(this.RemoveJob));
          manageMssReadingJobs.UpdateJobsRelatedToJobDefinition(jobDefinition, list);
        }
        finally
        {
          repositoryFactory.GetSession().Close();
        }
      }
    }

    public void FinalizeJobs(IEnumerable<MssReadingJob> jobs, bool deleteJobs = false)
    {
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      lock (repositoryFactory)
      {
        try
        {
          ManageMSSReadingJobs manageMssReadingJobs = new ManageMSSReadingJobs(repositoryFactory);
          if (!(jobs is IList<MssReadingJob> mssReadingJobList1))
            mssReadingJobList1 = (IList<MssReadingJob>) jobs.ToList<MssReadingJob>();
          IList<MssReadingJob> mssReadingJobList2 = mssReadingJobList1;
          List<MssReadingJob> refreshedJobs = new List<MssReadingJob>();
          TypeHelperExtensionMethods.ForEach<MssReadingJob>((IEnumerable<MssReadingJob>) mssReadingJobList2, (Action<MssReadingJob>) (j => refreshedJobs.Add(repositoryFactory.GetRepository<MssReadingJob>().GetById((object) j.Id))));
          manageMssReadingJobs.FinalizeJobs((IEnumerable<MssReadingJob>) refreshedJobs, deleteJobs);
          refreshedJobs.ForEach(new Action<MssReadingJob>(this.RemoveJob));
        }
        finally
        {
          repositoryFactory.GetSession().Close();
        }
      }
    }

    public void AddActiveJobsToGMM()
    {
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      if (this._zennerJobManager.Jobs != null && this._zennerJobManager.Jobs.Count > 0)
      {
        GMMJobsLogger.LogDebug("GMM active jobs:");
        this._zennerJobManager.Jobs.ForEach((Action<Job>) (j => GMMJobsLogger.LogDebug("JobId = " + j.JobID.ToString())));
      }
      else
        GMMJobsLogger.LogDebug("NO JOBS REGISTERED IN GMM!");
      lock (repositoryFactory)
      {
        try
        {
          foreach (MssReadingJob activeJob in new JobsManager(repositoryFactory).GetActiveJobs())
          {
            MssReadingJob mssReadingJob = activeJob;
            if (mssReadingJob.Minomat != null && mssReadingJob.Minomat.GsmId == null)
            {
              GMMJobsLogger.LogDebug("Job not sent to GMM because minomat has no GSMId");
            }
            else
            {
              if (mssReadingJob.IsUpdate && this._zennerJobManager.Jobs != null && this._zennerJobManager.Jobs.Any<Job>((Func<Job, bool>) (j => j.JobID == mssReadingJob.Id)))
                this._zennerJobManager.RemoveJob(mssReadingJob.Id);
              if (!this._zennerJobManager.Jobs.Any<Job>((Func<Job, bool>) (j => j.JobID == mssReadingJob.Id)))
              {
                List<MSS.Core.Model.Meters.Meter> source = (List<MSS.Core.Model.Meters.Meter>) null;
                if (mssReadingJob.RootNode != null)
                  source = new StructuresManager(repositoryFactory).LoadStructure(mssReadingJob.RootNode.Id).Meters;
                this.AddJob(mssReadingJob, source != null ? source.Where<MSS.Core.Model.Meters.Meter>((Func<MSS.Core.Model.Meters.Meter, bool>) (item => item.ReadingEnabled)).ToList<MSS.Core.Model.Meters.Meter>() : (List<MSS.Core.Model.Meters.Meter>) null);
                IRepository<MssReadingJob> repository = repositoryFactory.GetRepository<MssReadingJob>();
                if (mssReadingJob.IsUpdate)
                  mssReadingJob.IsUpdate = false;
                mssReadingJob.Status = JobStatusEnum.Active;
                repository.Update(mssReadingJob);
                if (mssReadingJob.RootNode != null)
                  JobCyclesCache.Instance.AddJobToCache(repositoryFactory, mssReadingJob, mssReadingJob.RootNode.Id);
              }
            }
          }
        }
        finally
        {
          repositoryFactory.GetSession().Close();
        }
      }
    }

    public void RemoveDeletedJobsFromGMM()
    {
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      lock (repositoryFactory)
      {
        try
        {
          TypeHelperExtensionMethods.ForEach<MssReadingJob>((IEnumerable<MssReadingJob>) repositoryFactory.GetRepository<MssReadingJob>().SearchFor((Expression<Func<MssReadingJob, bool>>) (x => x.IsDeactivated && x.IsUpdate)), (Action<MssReadingJob>) (job =>
          {
            JobCyclesCache.Instance.RemoveJobFromCache(job.Id);
            this.RemoveJob(job);
            GMMJobsLogger.LogDebug("Job removed from GMM: JobId=" + (object) job.Id);
          }));
        }
        finally
        {
          repositoryFactory.GetSession().Close();
        }
      }
    }

    public void StartListener()
    {
      if (this._startListener)
        return;
      GMMJobsLogger.LogDebug("START LISTENER!");
      this._zennerJobManager.StartListener();
      this._startListener = true;
    }

    public void AddJob(MssReadingJob mssReadingJob, List<MSS.Core.Model.Meters.Meter> meters, Guid rootNodeId = default (Guid))
    {
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      lock (repositoryFactory)
      {
        try
        {
          Dictionary<MSS.Core.Model.Meters.Meter, StructureNodeEquipmentSettings> readingParameters = this.GetMeterReadingParameters(meters, repositoryFactory);
          Job gmmJob = new JobsManager(repositoryFactory).MapMssJobToGmmJob(mssReadingJob);
          DateTime nextTriggerTime = gmmJob.Interval.GetNextTriggerTime();
          if (nextTriggerTime > DateTime.Now && mssReadingJob.Minomat != null)
            GMMJobsLogger.LogDebug(mssReadingJob.Minomat != null ? string.Format("Job {0} for minomat {1} is planned for execution on {2:F} and is not sent to GMM", (object) mssReadingJob.JobDefinition.Name, (object) mssReadingJob.Minomat.RadioId, (object) nextTriggerTime) : string.Format("Job {0} for structure {1} is planned for execution on {2:F} and is not sent to GMM", (object) mssReadingJob.JobDefinition.Name, (object) mssReadingJob.RootNode.Name, (object) nextTriggerTime));
          else if (mssReadingJob.Minomat != null && mssReadingJob.LastExecutionDate.HasValue && mssReadingJob.LastExecutionDate.Value.Date == DateTime.Today)
          {
            GMMJobsLogger.LogDebug(string.Format("Job {0} for minomat {1} has already run and is not sent to GMM", (object) mssReadingJob.JobDefinition.Name, (object) mssReadingJob.Minomat.RadioId));
          }
          else
          {
            if (meters != null)
            {
              gmmJob.Meters = GMMHelper.GetGMMMeters(meters.Where<MSS.Core.Model.Meters.Meter>((Func<MSS.Core.Model.Meters.Meter, bool>) (item => item.ReadingEnabled)).ToList<MSS.Core.Model.Meters.Meter>());
              if (readingParameters.Any<KeyValuePair<MSS.Core.Model.Meters.Meter, StructureNodeEquipmentSettings>>())
              {
                foreach (ZENNER.CommonLibrary.Entities.Meter meter in gmmJob.Meters)
                {
                  ZENNER.CommonLibrary.Entities.Meter meterZ = meter;
                  MSS.Core.Model.Meters.Meter key = readingParameters.Keys.FirstOrDefault<MSS.Core.Model.Meters.Meter>((Func<MSS.Core.Model.Meters.Meter, bool>) (item => item.SerialNumber == meterZ.SerialNumber));
                  if (key != null && readingParameters[key] != null)
                    meterZ.DeviceModel = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateDeviceModelWithSavedParams(meterZ.DeviceModel, readingParameters[key].DeviceModelReadingParams);
                }
              }
              gmmJob.ProfileType = GmmInterface.DeviceManager.GetProfileTypes(gmmJob.Meters, gmmJob.Equipment, new ProfileTypeTags?(ProfileTypeTags.JobManager)).FirstOrDefault<ProfileType>();
              if (gmmJob.ProfileType != null && !string.IsNullOrEmpty(mssReadingJob.JobDefinition.ProfileTypeParams))
                MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateProfileTypeWithSavedParams(gmmJob.ProfileType, mssReadingJob.JobDefinition.ProfileTypeParams);
            }
            this._zennerJobManager.AddJob(gmmJob);
            if (rootNodeId != new Guid())
              JobCyclesCache.Instance.AddJobToCache(repositoryFactory, mssReadingJob, rootNodeId);
            GMMJobsLogger.LogDebug(string.Format("Job added to GMM: JObId={0}, JobDefinition={1}, Entity={2}", (object) gmmJob.JobID, (object) mssReadingJob.JobDefinition.Name, mssReadingJob.Minomat != null ? (object) mssReadingJob.Minomat.RadioId : (object) mssReadingJob.RootNode.Name));
          }
        }
        finally
        {
          repositoryFactory.GetSession().Close();
        }
      }
    }

    public void RemoveJob(MssReadingJob mssReadingJob)
    {
      if (!this._zennerJobManager.Jobs.Any<Job>((Func<Job, bool>) (j => j.JobID == mssReadingJob.Id)))
        return;
      this._zennerJobManager.RemoveJob(mssReadingJob.Id);
      JobCyclesCache.Instance.RemoveJobFromCache(mssReadingJob.Id);
    }

    private void JobManager_JobStarted(object sender, Job job)
    {
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      lock (repositoryFactory)
      {
        try
        {
          JobLogsManager jobLogsManager = new JobLogsManager(repositoryFactory);
          GMMJobsLogger.LogJobStarted(job);
          jobLogsManager.CreateJobLog(job, JobLogStatusEnum.Running, "Job started");
        }
        finally
        {
          repositoryFactory.GetSession().Close();
          EventPublisher.Publish<JobStateModified>(new JobStateModified()
          {
            JobId = job.JobID
          }, (object) this);
          JobCyclesCache.Instance.UpdateJobCycles(new JobCacheUpdate()
          {
            JobId = job.JobID,
            UpdateType = JobCacheUpdateEnum.JobStarted
          });
        }
      }
    }

    private void JobManager_JobCompleted(object sender, Job job)
    {
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      lock (repositoryFactory)
      {
        try
        {
          GMMJobsLogger.LogJobCompleted(job);
          new JobLogsManager(repositoryFactory).CreateJobLog(job, JobLogStatusEnum.Finished, "Job completed");
          IRepository<MssReadingJob> repository = repositoryFactory.GetRepository<MssReadingJob>();
          MssReadingJob byId = repository.GetById((object) job.JobID);
          if (byId != null)
          {
            IntervalDto intervalDto = JobsManager.DeserializeIntervals(byId.JobDefinition.Interval);
            if (intervalDto != null && intervalDto.OneTimeDate.HasValue)
              this.FinalizeJobs((IEnumerable<MssReadingJob>) new List<MssReadingJob>()
              {
                byId
              });
            byId.LastExecutionDate = new DateTime?(DateTime.Now);
            repository.Update(byId);
          }
          if (this.IsMinomatJob(job))
            return;
          new MBusJobsManager(this._repositoryFactoryCreator).SaveReadingValues(job);
        }
        finally
        {
          repositoryFactory.GetSession().Close();
          EventPublisher.Publish<JobStateModified>(new JobStateModified()
          {
            JobId = job.JobID
          }, (object) this);
          JobCyclesCache.Instance.UpdateJobCycles(new JobCacheUpdate()
          {
            JobId = job.JobID,
            UpdateType = JobCacheUpdateEnum.JobCompleted
          });
        }
      }
    }

    private void JobManager_OnError(object sender, Exception e)
    {
      GMMJobsLogger.LogGMMError(e);
      IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
      lock (repositoryFactory)
      {
        try
        {
          switch (e)
          {
            case InvalidJobException _:
              InvalidJobException invalidJobException = e as InvalidJobException;
              new JobLogsManager(repositoryFactory).CreateJobLog(invalidJobException.Job, JobLogStatusEnum.Error, invalidJobException.Message);
              string str = string.Empty;
              if (invalidJobException.InnerException is InvalidMeterException)
                str = (invalidJobException.InnerException as InvalidMeterException).Meter.SerialNumber;
              JobCyclesCache.Instance.UpdateJobCycles(new JobCacheUpdate()
              {
                JobId = invalidJobException.Job.JobID,
                SerialNumberToUpdate = str,
                UpdateType = JobCacheUpdateEnum.JobError
              });
              break;
            case InvalidMeterException _:
              InvalidMeterException invalidMeterException = e as InvalidMeterException;
              JobCyclesCache.Instance.UpdateJobCycles(new JobCacheUpdate()
              {
                JobId = Guid.Empty,
                SerialNumberToUpdate = invalidMeterException.Meter.SerialNumber,
                UpdateType = JobCacheUpdateEnum.JobError
              });
              break;
          }
        }
        finally
        {
          repositoryFactory.GetSession().Close();
        }
      }
    }

    public void JobManager_ValueIdentSetReceived(object sender, ValueIdentSet e)
    {
      GMMJobsLogger.GetLogger().Debug("Received DEVICE value. Serial no:" + e.SerialNumber);
      Job tag = e.Tag as Job;
      if (this.IsMinomatJob(tag))
        MinomatJobsManager.SaveReadingValues(e);
      if (tag == null)
        return;
      JobCyclesCache.Instance.UpdateJobCycles(new JobCacheUpdate()
      {
        JobId = tag.JobID,
        SerialNumberToUpdate = e.SerialNumber,
        UpdateType = JobCacheUpdateEnum.JobValueIdentSetReceived
      });
    }

    public void JobManager_OnMinomatConnected(object sender, MinomatDevice e)
    {
      GMMJobsLogger.LogMinomatConnected(e);
      if (!e.GsmID.HasValue)
      {
        GMMJobsLogger.GetLogger().Debug("A minomat without GSMId was received. No action will be taken.");
      }
      else
      {
        IRepositoryFactory repositoryFactory = this._repositoryFactoryCreator.CreateNewRepositoryFactory();
        lock (repositoryFactory)
        {
          try
          {
            IRepository<Minomat> repository1 = repositoryFactory.GetRepository<Minomat>();
            IRepository<MSS.Core.Model.DataCollectors.MinomatConnectionLog> repository2 = repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.MinomatConnectionLog>();
            IRepository<Scenario> repository3 = repositoryFactory.GetRepository<Scenario>();
            Minomat minomat1 = repository1.FirstOrDefault((Expression<Func<Minomat, bool>>) (m => m.GsmId == e.GsmID.ToString()));
            ManageMSSReadingJobs manageMssReadingJobs1 = new ManageMSSReadingJobs(repositoryFactory);
            MSS.Core.Model.DataCollectors.MinomatConnectionLog minomatConnectionLog1 = new MSS.Core.Model.DataCollectors.MinomatConnectionLog();
            MSS.Core.Model.DataCollectors.MinomatConnectionLog minomatConnectionLog2 = minomatConnectionLog1;
            uint? nullable;
            string str1;
            if (e.MinolID.HasValue)
            {
              nullable = e.MinolID;
              str1 = nullable.ToString();
            }
            else
              str1 = (string) null;
            minomatConnectionLog2.MinolId = str1;
            MSS.Core.Model.DataCollectors.MinomatConnectionLog minomatConnectionLog3 = minomatConnectionLog1;
            nullable = e.ChallengeKey;
            string str2;
            if (!nullable.HasValue)
            {
              str2 = (string) null;
            }
            else
            {
              nullable = e.ChallengeKey;
              str2 = nullable.ToString();
            }
            minomatConnectionLog3.ChallengeKey = str2;
            MSS.Core.Model.DataCollectors.MinomatConnectionLog minomatConnectionLog4 = minomatConnectionLog1;
            nullable = e.GsmID;
            string str3;
            if (!nullable.HasValue)
            {
              str3 = (string) null;
            }
            else
            {
              nullable = e.GsmID;
              str3 = nullable.ToString();
            }
            minomatConnectionLog4.GsmID = str3;
            minomatConnectionLog1.IsTestConnection = e.IsTestConnection;
            MSS.Core.Model.DataCollectors.MinomatConnectionLog minomatConnectionLog5 = minomatConnectionLog1;
            ulong? sessionKey = e.SessionKey;
            string str4;
            if (!sessionKey.HasValue)
            {
              str4 = (string) null;
            }
            else
            {
              sessionKey = e.SessionKey;
              str4 = sessionKey.ToString();
            }
            minomatConnectionLog5.SessionKey = str4;
            minomatConnectionLog1.TimePoint = DateTime.Now;
            minomatConnectionLog1.MinomatId = minomat1 == null ? new Guid?() : new Guid?(minomat1.Id);
            MSS.Core.Model.DataCollectors.MinomatConnectionLog minomatConnectionLog6 = minomatConnectionLog1;
            nullable = e.ScenarioNumber;
            string str5;
            if (!nullable.HasValue)
            {
              str5 = (string) null;
            }
            else
            {
              nullable = e.ScenarioNumber;
              str5 = nullable.ToString();
            }
            minomatConnectionLog6.ScenarioNumber = str5;
            MSS.Core.Model.DataCollectors.MinomatConnectionLog entity = minomatConnectionLog1;
            repository2.Insert(entity);
            if (minomat1 == null)
            {
              GMMJobsLogger.GetLogger().Debug("No minomat with the given GSM id exists");
            }
            else
            {
              if (minomat1.Scenario == null)
              {
                ManageMSSReadingJobs manageMssReadingJobs2 = manageMssReadingJobs1;
                MinomatDevice e1 = e;
                Minomat minomat2 = minomat1;
                IRepository<Scenario> scenarioRepository = repository3;
                nullable = e.ScenarioNumber;
                int num = nullable.HasValue ? 1 : 0;
                manageMssReadingJobs2.SetScenarioAndCreateJobs(e1, minomat2, scenarioRepository, num != 0);
              }
              else
              {
                nullable = e.ScenarioNumber;
                if (nullable.HasValue)
                {
                  long code = (long) minomat1.Scenario.Code;
                  nullable = e.ScenarioNumber;
                  long num = (long) nullable.Value;
                  if (code != num)
                    manageMssReadingJobs1.SetScenarioAndRecreateJobs(e, minomat1, repository3);
                }
              }
              sessionKey = e.SessionKey;
              if (sessionKey.HasValue)
              {
                Minomat minomat3 = minomat1;
                sessionKey = e.SessionKey;
                string str6 = sessionKey.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                minomat3.SessionKey = str6;
              }
              nullable = e.ChallengeKey;
              if (nullable.HasValue)
              {
                Minomat minomat4 = minomat1;
                nullable = e.ChallengeKey;
                string str7 = nullable.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                minomat4.Challenge = str7;
              }
              repository1.Update(minomat1);
            }
          }
          finally
          {
            repositoryFactory.GetSession().Close();
          }
        }
      }
    }

    private bool IsMinomatJob(Job job) => job.ProfileType != null && job.ProfileType.Name == "GSM";

    private Dictionary<MSS.Core.Model.Meters.Meter, StructureNodeEquipmentSettings> GetMeterReadingParameters(
      List<MSS.Core.Model.Meters.Meter> meters,
      IRepositoryFactory repositoryFactory)
    {
      Dictionary<MSS.Core.Model.Meters.Meter, StructureNodeEquipmentSettings> readingParameters = new Dictionary<MSS.Core.Model.Meters.Meter, StructureNodeEquipmentSettings>();
      IRepository<StructureNodeEquipmentSettings> repository = repositoryFactory.GetRepository<StructureNodeEquipmentSettings>();
      List<Guid> meterIds = meters.Select<MSS.Core.Model.Meters.Meter, Guid>((Func<MSS.Core.Model.Meters.Meter, Guid>) (item => item.Id)).ToList<Guid>();
      List<StructureNodeEquipmentSettings> list = repository.Where((Expression<Func<StructureNodeEquipmentSettings, bool>>) (item => meterIds.Contains(item.StructureNode.EntityId))).ToList<StructureNodeEquipmentSettings>();
      if (list.Any<StructureNodeEquipmentSettings>())
      {
        foreach (MSS.Core.Model.Meters.Meter meter1 in meters)
        {
          MSS.Core.Model.Meters.Meter meter = meter1;
          readingParameters.Add(meter, list.FirstOrDefault<StructureNodeEquipmentSettings>((Func<StructureNodeEquipmentSettings, bool>) (item => item.StructureNode.EntityId == meter.Id)));
        }
      }
      return readingParameters;
    }
  }
}
