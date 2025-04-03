// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.JobsManagement.ManageMSSReadingJobs
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Events;
using MSS.Business.Modules.GMM;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Structures;
using MSS.Interfaces;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.JobsManagement
{
  public class ManageMSSReadingJobs
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public ManageMSSReadingJobs(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
    }

    public void FinalizeJobs(IEnumerable<MssReadingJob> jobs, bool deleteJobs = false)
    {
      this._repositoryFactory.GetSession().BeginTransaction();
      try
      {
        TypeHelperExtensionMethods.ForEach<MssReadingJob>(jobs, (Action<MssReadingJob>) (job =>
        {
          job.EndDate = new DateTime?(DateTime.Now);
          job.Status = JobStatusEnum.Inactive;
          if (deleteJobs)
            job.IsDeactivated = true;
          this._repositoryFactory.GetRepository<MssReadingJob>().TransactionalUpdate(job);
        }));
        this._repositoryFactory.GetSession().Transaction.Commit();
        EventPublisher.Publish<RefreshGridEvent>(new RefreshGridEvent(), (object) this);
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public void RecreateJobs(IEnumerable<MssReadingJob> jobs)
    {
      this._repositoryFactory.GetSession().BeginTransaction();
      try
      {
        TypeHelperExtensionMethods.ForEach<MssReadingJob>(jobs, (Action<MssReadingJob>) (x =>
        {
          MssReadingJob entity = new MssReadingJob()
          {
            JobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) x.JobDefinition.Id),
            StartDate = new DateTime?(DateTime.Now),
            Minomat = x.Minomat != null ? this._repositoryFactory.GetRepository<Minomat>().GetById((object) x.Minomat.Id) : (Minomat) null,
            IsDeactivated = false,
            RootNode = x.RootNode,
            Status = JobStatusEnum.Isnew,
            CreatedOn = DateTime.Now
          };
          if (x.Scenario != null)
            entity.Scenario = this._repositoryFactory.GetRepository<Scenario>().GetById((object) x.Scenario.Id);
          this._repositoryFactory.GetRepository<MssReadingJob>().TransactionalInsert(entity);
        }));
        this._repositoryFactory.GetSession().Transaction.Commit();
        EventPublisher.Publish<RefreshGridEvent>(new RefreshGridEvent(), (object) this);
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public void CreateJobs(string serialNumber, IEnumerable<Guid> jobDefinitions, Guid scenarioId)
    {
      this._repositoryFactory.GetSession().BeginTransaction();
      try
      {
        TypeHelperExtensionMethods.ForEach<Guid>(jobDefinitions, (Action<Guid>) (x =>
        {
          // ISSUE: reference to a compiler-generated field
          MssReadingJob entity = new MssReadingJob()
          {
            JobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) x),
            StartDate = new DateTime?(DateTime.Now),
            Minomat = this._repositoryFactory.GetRepository<Minomat>().FirstOrDefault((Expression<Func<Minomat, bool>>) (y => y.RadioId == this.serialNumber)),
            IsDeactivated = false,
            Scenario = this._repositoryFactory.GetRepository<Scenario>().GetById((object) scenarioId),
            CreatedOn = DateTime.Now,
            Status = JobStatusEnum.Isnew
          };
          this._repositoryFactory.GetRepository<MssReadingJob>().TransactionalInsert(entity);
        }));
        this._repositoryFactory.GetSession().Transaction.Commit();
        EventPublisher.Publish<RefreshGridEvent>(new RefreshGridEvent(), (object) this);
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public void CreateJob(Guid minomatId, Guid jobDefinition, Guid scenarioId)
    {
      this._repositoryFactory.GetSession().BeginTransaction();
      try
      {
        MssReadingJob entity = new MssReadingJob()
        {
          JobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) jobDefinition),
          StartDate = new DateTime?(DateTime.Now),
          Minomat = this._repositoryFactory.GetRepository<Minomat>().GetById((object) minomatId),
          IsDeactivated = false,
          Scenario = this._repositoryFactory.GetRepository<Scenario>().GetById((object) scenarioId),
          CreatedOn = DateTime.Now
        };
        this._repositoryFactory.GetRepository<MssReadingJob>().TransactionalInsert(entity);
        this._repositoryFactory.GetSession().Transaction.Commit();
        GMMJobsLogger.LogDebug(string.Format("New job created: MinomatId={0}, GSMId={1}, JOb Defintion Name={2}", (object) entity.Minomat.RadioId, (object) entity.Minomat.GsmId, (object) entity.JobDefinition.Name));
        EventPublisher.Publish<RefreshGridEvent>(new RefreshGridEvent(), (object) this);
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public void SetScenarioAndRecreateJobs(
      MinomatDevice e,
      Minomat minomat,
      IRepository<Scenario> scenarioRepository)
    {
      this.FinalizeJobs((IEnumerable<MssReadingJob>) this._repositoryFactory.GetRepository<MssReadingJob>().SearchFor((Expression<Func<MssReadingJob, bool>>) (x => x.Minomat.Id == minomat.Id)));
      minomat.Scenario = scenarioRepository.FirstOrDefault((Expression<Func<Scenario, bool>>) (x => (long) x.Code == (long) e.ScenarioNumber.Value));
      this._repositoryFactory.GetRepository<Minomat>().Update(minomat);
      IEnumerable<Guid> jobDefinitions = this._repositoryFactory.GetRepository<ScenarioJobDefinition>().SearchFor((Expression<Func<ScenarioJobDefinition, bool>>) (x => x.Scenario.Id == minomat.Scenario.Id)).Select<ScenarioJobDefinition, Guid>((Func<ScenarioJobDefinition, Guid>) (x => x.JobDefinition.Id));
      this.CreateJobs(minomat.RadioId, jobDefinitions, minomat.Scenario.Id);
    }

    public void SetScenarioAndCreateJobs(
      MinomatDevice e,
      Minomat minomat,
      IRepository<Scenario> scenarioRepository,
      bool hasValue)
    {
      if (hasValue)
      {
        minomat.Scenario = scenarioRepository.FirstOrDefault((Expression<Func<Scenario, bool>>) (s => (long) s.Code == (long) e.ScenarioNumber.Value));
      }
      else
      {
        if (minomat.Country == null || !(minomat.Country.DefaultScenarioId != Guid.Empty))
          return;
        minomat.Scenario = scenarioRepository.FirstOrDefault((Expression<Func<Scenario, bool>>) (x => x.Id == minomat.Country.DefaultScenarioId));
      }
      this._repositoryFactory.GetRepository<Minomat>().Update(minomat);
    }

    public void UpdateJobsRelatedToScenario(Guid scenarioId, string serialNumber)
    {
      if (this._repositoryFactory.GetRepository<Minomat>().FirstOrDefault((Expression<Func<Minomat, bool>>) (x => x.RadioId == serialNumber && x.Scenario.Id == scenarioId)) != null)
        return;
      Minomat currentScenario = this._repositoryFactory.GetRepository<Minomat>().FirstOrDefault((Expression<Func<Minomat, bool>>) (y => y.RadioId == serialNumber));
      IEnumerable<Guid> jDef = this._repositoryFactory.GetRepository<ScenarioJobDefinition>().SearchFor((Expression<Func<ScenarioJobDefinition, bool>>) (x => x.Scenario.Id == currentScenario.Scenario.Id)).Select<ScenarioJobDefinition, Guid>((Func<ScenarioJobDefinition, Guid>) (x => x.JobDefinition.Id));
      this.FinalizeJobs((IEnumerable<MssReadingJob>) this._repositoryFactory.GetRepository<MssReadingJob>().SearchFor((Expression<Func<MssReadingJob, bool>>) (job => jDef.Contains<Guid>(job.JobDefinition.Id) && job.EndDate == new DateTime?())));
      IEnumerable<Guid> jobDefinitions = this._repositoryFactory.GetRepository<ScenarioJobDefinition>().SearchFor((Expression<Func<ScenarioJobDefinition, bool>>) (x => x.Scenario.Id == currentScenario.Scenario.Id)).Select<ScenarioJobDefinition, Guid>((Func<ScenarioJobDefinition, Guid>) (x => x.JobDefinition.Id));
      this.CreateJobs(serialNumber, jobDefinitions, scenarioId);
    }

    public void UpdateJobsRelatedToJobDefinition(
      Guid jobDefinition,
      List<MssReadingJob> mssReadingJobs)
    {
      try
      {
        this._repositoryFactory.GetSession().BeginTransaction();
        foreach (MssReadingJob mssReadingJob in mssReadingJobs)
        {
          mssReadingJob.IsUpdate = true;
          mssReadingJob.Status = JobStatusEnum.Inactive;
          this._repositoryFactory.GetRepository<MssReadingJob>().TransactionalUpdate(mssReadingJob);
        }
        this._repositoryFactory.GetSession().Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
    }

    public void CreateJobs(
      List<Minomat> minomats,
      List<Guid> jobDefinitionForCreateNew,
      Guid scenarioId)
    {
      try
      {
        this._repositoryFactory.GetSession().BeginTransaction();
        minomats.ForEach((Action<Minomat>) (minomat => jobDefinitionForCreateNew.ForEach((Action<Guid>) (x =>
        {
          // ISSUE: reference to a compiler-generated field
          MssReadingJob entity = new MssReadingJob()
          {
            JobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) x),
            StartDate = new DateTime?(DateTime.Now),
            Minomat = this._repositoryFactory.GetRepository<Minomat>().FirstOrDefault((Expression<Func<Minomat, bool>>) (m => m.RadioId == this.minomat.RadioId)),
            IsDeactivated = false,
            Scenario = this._repositoryFactory.GetRepository<Scenario>().GetById((object) scenarioId),
            CreatedOn = DateTime.Now
          };
          this._repositoryFactory.GetRepository<MssReadingJob>().TransactionalInsert(entity);
        }))));
        this._repositoryFactory.GetSession().Transaction.Commit();
      }
      catch (Exception ex)
      {
        this._repositoryFactory.GetSession().Transaction.Rollback();
        throw;
      }
      EventPublisher.Publish<RefreshGridEvent>(new RefreshGridEvent(), (object) this);
    }
  }
}
