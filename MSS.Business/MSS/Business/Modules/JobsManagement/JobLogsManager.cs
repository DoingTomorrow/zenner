// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.JobsManagement.JobLogsManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using AutoMapper;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Reporting;
using MSS.DTO.Reporting;
using MSS.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.JobsManagement
{
  public class JobLogsManager
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public JobLogsManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
    }

    public void CreateJobLog(Job job, JobLogStatusEnum status, string message)
    {
      if (message.Length > 500)
        message = message.Substring(0, 500);
      IRepository<JobLogs> repository = this._repositoryFactory.GetRepository<JobLogs>();
      JobLogs entity = repository.SearchFor((Expression<Func<JobLogs, bool>>) (jl => jl.Job.Id == job.JobID && jl.EndDate == new DateTime?())).ToList<JobLogs>().LastOrDefault<JobLogs>();
      if (entity == null)
        entity = new JobLogs()
        {
          Active = true,
          Job = this._repositoryFactory.GetRepository<MssReadingJob>().GetById((object) job.JobID),
          Status = status,
          CreatedOn = DateTime.Now,
          Message = message
        };
      else if (status == JobLogStatusEnum.Error)
      {
        if (entity.Status != JobLogStatusEnum.Error)
        {
          entity.Status = status;
          entity.Message = message;
        }
        else
          entity.Message = entity.Message + Environment.NewLine + message;
      }
      else if (status == JobLogStatusEnum.Finished && entity.Status != JobLogStatusEnum.Error)
      {
        entity.Status = status;
        entity.Message = message;
      }
      switch (status)
      {
        case JobLogStatusEnum.Running:
          entity.StartDate = new DateTime?(DateTime.Now);
          break;
        case JobLogStatusEnum.Finished:
          entity.Active = false;
          entity.EndDate = new DateTime?(DateTime.Now);
          break;
      }
      if (entity.Job != null && entity.Job.RootNode != null)
        entity.JobEntityNumber = entity.Job.RootNode.Name;
      else if (entity.Job != null && entity.Job.Minomat != null)
        entity.JobEntityNumber = entity.Job.Minomat.RadioId;
      repository.Update(entity);
    }

    public ObservableCollection<JobLogsDTO> LoadJobLogs(
      string jobEntityName,
      DateTime? startDate,
      DateTime? endDate)
    {
      ObservableCollection<JobLogsDTO> destination = new ObservableCollection<JobLogsDTO>();
      Mapper.Map<IList<JobLogs>, ObservableCollection<JobLogsDTO>>(this._repositoryFactory.GetRepository<JobLogs>().SearchFor((Expression<Func<JobLogs, bool>>) (j => j.JobEntityNumber.ToLowerInvariant() == jobEntityName.ToLowerInvariant() && (DateTime?) j.CreatedOn >= startDate && (DateTime?) j.CreatedOn <= endDate)), destination);
      return destination;
    }

    public ObservableCollection<JobLogsDTO> LoadJobLogs(
      Guid jobId,
      DateTime? startDate,
      DateTime? endDate)
    {
      ObservableCollection<JobLogsDTO> destination = new ObservableCollection<JobLogsDTO>();
      this._repositoryFactory.GetSession().Clear();
      Mapper.Map<IList<JobLogs>, ObservableCollection<JobLogsDTO>>(this._repositoryFactory.GetRepository<JobLogs>().SearchFor((Expression<Func<JobLogs, bool>>) (j => j.Job.Id == jobId && (DateTime?) j.CreatedOn >= startDate && (DateTime?) j.CreatedOn <= endDate)), destination);
      return destination;
    }
  }
}
