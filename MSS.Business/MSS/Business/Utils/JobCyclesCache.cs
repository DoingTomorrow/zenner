// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.JobCyclesCache
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Business.Events;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.Jobs;
using MSS.DTO.Meters;
using MSS.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace MSS.Business.Utils
{
  public sealed class JobCyclesCache
  {
    private static readonly JobCyclesCache _instance = new JobCyclesCache();
    private static ObservableCollection<JobCycles> _jobCycles = new ObservableCollection<JobCycles>();

    private JobCyclesCache()
    {
    }

    public static JobCyclesCache Instance => JobCyclesCache._instance;

    public void UpdateJobCycles(JobCacheUpdate jobUpdate)
    {
      if (jobUpdate == null)
        return;
      JobCycles jobCycles = (JobCycles) null;
      if (jobUpdate.JobId != Guid.Empty)
        jobCycles = JobCyclesCache._jobCycles.FirstOrDefault<JobCycles>((Func<JobCycles, bool>) (item => item.Job.Id == jobUpdate.JobId));
      if (jobCycles != null || jobCycles == null && jobUpdate.UpdateType == JobCacheUpdateEnum.JobError)
      {
        switch (jobUpdate.UpdateType)
        {
          case JobCacheUpdateEnum.JobStarted:
            jobCycles.Job.Status = JobStatusEnum.Active;
            break;
          case JobCacheUpdateEnum.JobError:
            if (jobCycles != null)
              jobCycles.Job.Status = JobStatusEnum.Iserror;
            if (jobUpdate.JobId == Guid.Empty && !string.IsNullOrEmpty(jobUpdate.SerialNumberToUpdate))
            {
              foreach (JobCycles jobCycle in (Collection<JobCycles>) JobCyclesCache._jobCycles)
              {
                StructureNodeDTO foundMeter = (StructureNodeDTO) null;
                this.GetMeterFromStructure(jobCycle.StructureRootNode, jobUpdate.SerialNumberToUpdate, ref foundMeter);
                if (foundMeter != null)
                {
                  foundMeter.Cycles++;
                  foundMeter.Failed++;
                }
              }
            }
            if (jobUpdate.JobId != Guid.Empty && !string.IsNullOrEmpty(jobUpdate.SerialNumberToUpdate) && jobCycles != null)
            {
              StructureNodeDTO foundMeter = (StructureNodeDTO) null;
              this.GetMeterFromStructure(jobCycles.StructureRootNode, jobUpdate.SerialNumberToUpdate, ref foundMeter);
              if (foundMeter != null)
              {
                foundMeter.Cycles++;
                foundMeter.Failed++;
              }
              break;
            }
            break;
          case JobCacheUpdateEnum.JobValueIdentSetReceived:
            StructureNodeDTO foundMeter1 = (StructureNodeDTO) null;
            this.GetMeterFromStructure(jobCycles.StructureRootNode, jobUpdate.SerialNumberToUpdate, ref foundMeter1);
            if (foundMeter1 != null)
            {
              foundMeter1.Cycles++;
              foundMeter1.Succeeded++;
              break;
            }
            break;
        }
      }
    }

    public void AddJobToCache(
      IRepositoryFactory repositoryFactory,
      MssReadingJob newJob,
      Guid rootNodeId)
    {
      if (!(rootNodeId != new Guid()) || JobCyclesCache._jobCycles.FirstOrDefault<JobCycles>((Func<JobCycles, bool>) (item => item.Job.Id == newJob.Id)) != null)
        return;
      JobCyclesCache._jobCycles.Add(new JobCycles()
      {
        Job = newJob,
        StructureRootNode = StructuresHelper.LoadStructureFromRootNodeId(repositoryFactory, rootNodeId)
      });
    }

    public void RemoveJobFromCache(Guid jobId)
    {
      if (!(jobId != new Guid()))
        return;
      JobCycles jobCycles = JobCyclesCache._jobCycles.FirstOrDefault<JobCycles>((Func<JobCycles, bool>) (item => item.Job.Id == jobId));
      if (jobCycles != null)
        JobCyclesCache._jobCycles.Remove(jobCycles);
    }

    public StructureNodeDTO GetJobStructureByJobId(Guid jobId)
    {
      return JobCyclesCache._jobCycles.FirstOrDefault<JobCycles>((Func<JobCycles, bool>) (item => item.Job.Id == jobId))?.StructureRootNode;
    }

    private void GetMeterFromStructure(
      StructureNodeDTO rootNode,
      string serialNumber,
      ref StructureNodeDTO foundMeter)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) rootNode.SubNodes)
      {
        if (subNode.NodeType.Name == "Meter" || subNode.NodeType.Name == "RadioMeter")
        {
          if (subNode?.Entity is MeterDTO entity && entity.SerialNumber == serialNumber)
            foundMeter = subNode;
          else
            this.GetMeterFromStructure(subNode, serialNumber, ref foundMeter);
        }
        else
          this.GetMeterFromStructure(subNode, serialNumber, ref foundMeter);
      }
    }
  }
}
